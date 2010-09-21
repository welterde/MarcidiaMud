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
    public class SessionStartConnectionHandler : IConnectionHandler
    {
        Type initialSessionStateType;
        SessionManager sessionManager;

        public SessionStartConnectionHandler(Type initialSessionStateType, SessionManager sessionManager)
        {
            if (initialSessionStateType == null)
                throw new ArgumentNullException("initialSessionStateType", "initialSessionState is null.");
            if (sessionManager == null)
                throw new ArgumentNullException("sessionManager", "sessionManager is null.");


            this.initialSessionStateType = initialSessionStateType;
            this.sessionManager = sessionManager;
        }

        public void HandleConnection(IConnection connection)
        {
            sessionManager.CreateSessionFor(connection, initialSessionStateType);
        }
    }
}
