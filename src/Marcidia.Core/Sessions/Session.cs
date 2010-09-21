﻿using System;
using Marcidia.Net;

namespace Marcidia.Sessions
{
    public class Session : IDisposable
    {
        private IConnection connection;
        private SessionStateStack stateStack;

        private Session()
        {
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

        public event EventHandler<ValueChangedEventArgs<IConnection>> ConnectionChanged;

        private void OnConnectionChanged(IConnection oldConnection, IConnection newConnection)
        {
            if (oldConnection != null)
            {
                oldConnection.ConnectionClosed -= OnConnectionClosed;
                oldConnection.ConnectionLost -= OnConnectionLost;
            }

            if (newConnection != null)
            {
                newConnection.ConnectionClosed += OnConnectionClosed;
                newConnection.ConnectionLost += OnConnectionLost;
            }

            if (ConnectionChanged != null)
                ConnectionChanged(this, new ValueChangedEventArgs<IConnection>(oldConnection, newConnection));
        }

        public event EventHandler SessionClosed;

        private void OnSessionClosed()
        {
            if (SessionClosed != null)
                SessionClosed(this, EventArgs.Empty);
        }

        public static Session Create(IConnection connection, SessionState sessionState)
        {
            if (connection == null)
                throw new ArgumentNullException("connection", "connection is null.");
            if (sessionState == null)
                throw new ArgumentNullException("sessionState", "sessionState is null.");

            Session session =
                new Session()
                {
                    Connection = connection
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

        private void OnConnectionLost(object sender, EventArgs e)
        {
            connection = null;
        }

        private void OnConnectionClosed(object sender, EventArgs e)
        {
            connection = null;
        }
    }
}