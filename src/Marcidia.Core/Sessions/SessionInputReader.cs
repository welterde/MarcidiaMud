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
        bool disposed;
        Session session;
        Thread inputThread;

        public SessionInputReader(Session session)
        {
            if (session == null)
                throw new ArgumentNullException("session", "session is null.");

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
            Stop();
            Start();
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
                using (ConnectionStream stream = new ConnectionStream(session.Connection))
                using (StreamReader streamReader = new StreamReader(stream, Encoding.ASCII))
                {
                    string input = null;

                    do
                    {
                        input = streamReader.ReadLine();

                        if (input != null)
                            session.SendInput(input);

                    } while (input != null);
                }
            }
            catch (ThreadInterruptedException)
            {
                // we expect this!
            }
        }
    }
}
