using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Logging;
using System.IO;
using System.Threading;
using Marcidia.ComponentLoading;
using Marcidia.Net;
using Marcidia.ComponentModel;
using Marcidia;
using Marcidia.Sessions;
using Marcidia.Output;

namespace Marcidia.Server
{
    class MarcidiaMud : Mud
    {
        ILogger logger;

        public MarcidiaMud()
        {
            LogComponent loggingComponent = new LogComponent(this);
            Components.Add(loggingComponent);

            ConnectionWriterFactory connectionWriterFactory = new ConnectionWriterFactory(this);
            Components.Add(connectionWriterFactory);

            ConnectionManager connectionComponent = new ConnectionManager(this);
            Components.Add(connectionComponent);

            SessionManager sessionManager = new SessionManager(this);
            Components.Add(sessionManager);

            AutoComponentLoader componentLoader = new AutoComponentLoader(this);
            Components.Add(componentLoader);
        }

        protected override void Initialize()
        {
            base.Initialize();

            logger = Services.GetService<ILogger>();

            logger.Log(LogLevels.Standard, "Loaded Components");
        }
    }
}
