using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Sessions.Configuration
{
    class SessionStartConfiguration
    {
        public Type InitialSessionState { get; private set; }
        public string ConnectionSourceName { get; private set; }

        public SessionStartConfiguration(Type initialSessionState, string connectionSourceName)
        {
            if (initialSessionState == null)
                throw new ArgumentNullException("initialSessionState", "initialSessionState is null.");
            if (String.IsNullOrEmpty(connectionSourceName))
                throw new ArgumentException("connectionSourceName is null or empty.", "connectionSourceName");

            InitialSessionState = initialSessionState;
            ConnectionSourceName = connectionSourceName;
        }
    }
}
