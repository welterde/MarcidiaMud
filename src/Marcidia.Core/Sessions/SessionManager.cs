using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.ComponentModel;
using Marcidia.Net;
using Marcidia.Sessions.Configuration;
using System.Configuration;
using Marcidia;

namespace Marcidia.Sessions
{
    [MarcidiaComponent("Session Manager", false)]
    public class SessionManager : MarcidiaComponent, IDisposable
    {
        private const string SessionStartConfigurationSection = "Marcidia.Sessions";

        SessionStateBuilder sessionStateBuilder;
        List<Session> sessions;
        List<SessionInputReader> sessionInputReaders;
        
        public SessionManager(Mud mud)
            : base(mud)
        {
            sessionStateBuilder = new SessionStateBuilder();
            sessions = new List<Session>();
            sessionInputReaders = new List<SessionInputReader>();
        }
         
        public override void Initialize()
        {
            IConnectionHandlerRegistrar connectionHandlerRegistrar = Mud.Services.GetService<IConnectionHandlerRegistrar>();

            IEnumerable<SessionStartConfiguration> sessionStartConfigurations = LoadAndValidateConfiguration();

            foreach (var sessionStartConfiguration in sessionStartConfigurations)
            {
                IConnectionHandler handler = new SessionStartConnectionHandler(sessionStartConfiguration.InitialSessionState, this);

                connectionHandlerRegistrar.RegisterConnectionHandler(sessionStartConfiguration.ConnectionSourceName, handler);
            }
        }

        private IEnumerable<SessionStartConfiguration> LoadAndValidateConfiguration()
        {
            return (IEnumerable<SessionStartConfiguration>)ConfigurationManager.GetSection(SessionStartConfigurationSection);
        }

        public void CreateSessionFor(IConnection connection, Type initialSessionStateType)
        {
            SessionState sessionState = sessionStateBuilder.Build(initialSessionStateType);

            Session session = null;

            lock (sessions)
            {
                session = Session.Create(connection, sessionState);

                sessions.Add(session);
            }

            CreateAndStartInputThreadFor(session);
        }

        private void CreateAndStartInputThreadFor(Session session)
        {
            SessionInputReader inputReader = new SessionInputReader(session);

            lock (sessionInputReaders)
                sessionInputReaders.Add(inputReader);

            inputReader.Disposed += InputReaderDisposed;

            inputReader.Start();
        }

        private void InputReaderDisposed(object sender, EventArgs e)
        {
            lock (sessionInputReaders)
                sessionInputReaders.Remove((SessionInputReader)sender);
        }

        public void Dispose()
        {
            lock (sessions)
            {
                foreach (var session in sessions)
                {
                    session.Dispose();
                    sessions.Remove(session);
                }
            }
        }
    }
}
