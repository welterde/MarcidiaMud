using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Marcidia.Net;
using System.IO;

namespace Marcidia.Sessions
{
    public class SessionInputReader : IDisposable
    {
        bool connectionChanged;
        bool disposed;
        Session session;
        Thread inputThread;

        public SessionInputReader(Session session)
        {
            if (session == null)
                throw new ArgumentNullException("session", "session is null.");

            this.connectionChanged = false;
            this.session = session;
            this.inputThread = null;
            this.disposed = false;

            WireUpEvents();
        }

        public void Start()
        {
            if (disposed)
                throw new ObjectDisposedException("SessionInputReader");

            if (inputThread != null && inputThread.IsAlive)
                throw new InvalidOperationException("SessionInputReader has already been started");

            if (session.Connection == null)
                return;

            inputThread = new Thread(ReadAndForwardInputToSession);
            inputThread.Start();
        }

        public void Stop()
        {
            if (disposed)
                throw new ObjectDisposedException("SessionInputReader");

            if (inputThread != null && inputThread.IsAlive)
            {
                inputThread.Interrupt();

                if (inputThread != Thread.CurrentThread)
                    inputThread.Join();
            }
        }

        private void WireUpEvents()
        {
            session.ConnectionChanged += SessionConnectionChanged;
            session.SessionClosed += SessionClosed;
        }

        private void SessionClosed(object sender, EventArgs e)
        {
            Dispose();
        }

        private void SessionConnectionChanged(object sender, ValueChangedEventArgs<IConnection> e)
        {
            if (inputThread != Thread.CurrentThread)
            {
                Stop();
                Start();
            }
            else
                connectionChanged = true;
        }

        public event EventHandler Disposed;

        private void OnDisposed()
        {
            disposed = true;

            if (Disposed != null)
                Disposed(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            Stop();
            OnDisposed();
        }

        private void ReadAndForwardInputToSession()
        {
            try
            {
                do
                {
                    connectionChanged = false;

                    DoReadLoop();

                // if connectionChanged gets set back to true before hitting here
                // it means we need to start reading from a new connection, so we
                // just loop back round...
                } while (connectionChanged);
            }
            catch (ThreadInterruptedException)
            {
                // we expect this!
            }
        }

        /// <summary>
        /// Does the "infinite" read loop. Loops round as long as it recieves input.
        /// Blocks while it waits.
        /// Exits when the stream its reading from is closed;
        /// </summary>
        private void DoReadLoop()
        {
            using (ConnectionStream stream = new ConnectionStream(session.Connection))
            using (StreamReader streamReader = new StreamReader(stream, Encoding.ASCII))
            {
                string input = null;

                do
                {
                    input = streamReader.ReadLine();

                    if (input != null)
                    {
                        input = ProcessInputCharacters(input);
                        session.SendInput(input);
                    }

                } while (input != null && !connectionChanged);
            }
        }

        private string ProcessInputCharacters(string input)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '\b')
                {
                    if (builder.Length > 0)
                        builder.Remove(builder.Length - 1, 1);

                    continue;
                }

                builder.Append(input[i]);
            }

            return builder.ToString();
        }
    }
}
