using System;
using Marcidia.Net;
using Marcidia.Output;

namespace Marcidia.Sessions
{
    public class Session : IDisposable
    {
        private IConnectionWriterFactory connectionWriterFactory;
        private IConnection connection;
        private SessionStateStack stateStack;

        private Session(IConnectionWriterFactory connectionWriterFactory)
        {
            if (connectionWriterFactory == null)
                throw new ArgumentNullException("connectionWriterFactory", "connectionWriterFactory is null.");

            this.connectionWriterFactory = connectionWriterFactory;
            stateStack = new SessionStateStack(this);
        }

        public IConnection Connection
        {
            get { return connection; }
            set
            {
                if (connection == value)
                    return;

                IConnection old = connection;
                connection = value;

                OnConnectionChanged(old, connection);
            }
        }

        public IConnectionWriter Output
        {
            get;
            private set;
        }

        public IServiceProvider Services
        {
            get;
            private set;
        }

        public event EventHandler<ValueChangedEventArgs<IConnection>> ConnectionChanged;

        private void OnConnectionChanged(IConnection oldConnection, IConnection newConnection)
        {
            if (oldConnection != null)
            {
                oldConnection.ConnectionClosed -= OnConnectionLostOrClosed;
                oldConnection.ConnectionLost -= OnConnectionLostOrClosed;
            }

            if (newConnection != null)
            {
                newConnection.ConnectionClosed += OnConnectionLostOrClosed;
                newConnection.ConnectionLost += OnConnectionLostOrClosed;
            }

            Output = connectionWriterFactory.GetWriterFor(newConnection);

            if (ConnectionChanged != null)
                ConnectionChanged(this, new ValueChangedEventArgs<IConnection>(oldConnection, newConnection));
        }

        public event EventHandler SessionClosed;

        private void OnSessionClosed()
        {
            if (SessionClosed != null)
                SessionClosed(this, EventArgs.Empty);
        }

        public static Session Create(IConnection connection, SessionState sessionState, IConnectionWriterFactory connectionWriterFactory, IServiceProvider services)
        {
            if (connection == null)
                throw new ArgumentNullException("connection", "connection is null.");
            if (sessionState == null)
                throw new ArgumentNullException("sessionState", "sessionState is null.");

            Session session =
                new Session(connectionWriterFactory)
                {
                    Connection = connection,
                    Services = services
                };

            session.PushState(sessionState);

            return session;
        }

        public void Close()
        {
            if (connection != null)
                connection.Close();

            OnSessionClosed();
        }

        public void PushState(SessionState sessionState)
        {
            stateStack.Push(sessionState);
        }

        public SessionState PopState()
        {
            return stateStack.Pop();
        }

        public void SendInput(string input)
        {
            SessionState current = stateStack.Peek();

            if (current != null)
                current.SendInput(input);
        }

        public void Dispose()
        {
            Close();
        }

        // The name implies the reverse of what happens, yet I can't think of a better name.
        // Your not taking control of the Session
        // Your replacing your connection with the connection of the passed in session.
        // which gives the impression that the passed in session has taken control of
        // this session.
        public void TakeControlOf(Session session)
        {
            IConnection oldConnection = this.Connection;
            IConnection newConnection = session.Connection;
            
            session.Connection = null;
            this.Connection = newConnection;

            // We have to close the old, now orphaned connection
            oldConnection.Close();
            // and the session who we've just wrestled control from
            session.Close();
        }

        private void OnConnectionLostOrClosed(object sender, EventArgs e)
        {
            connection = null;
        }
    }
}