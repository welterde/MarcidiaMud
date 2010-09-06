using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Logging;
using System.IO;
using System.Threading;
using Marcidia.ComponentLoading;
using Marcidia.Net;

namespace Marcidia.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            MarcidiaMud mud = new MarcidiaMud();

            mud.Run();
        }
    }

    class MarcidiaMud : Mud
    {
        ILogger logger;

        public MarcidiaMud()
        {
            LogComponent loggingComponent = new LogComponent(this);
            Components.Add(loggingComponent);

            AutoComponentLoader componentLoader = new AutoComponentLoader(this);
            Components.Add(componentLoader);

            ConnectionComponent connectionComponent = new ConnectionComponent(this);
            Components.Add(connectionComponent);
        }

        protected override void Initialize()
        {
            base.Initialize();

            logger = Services.GetService<ILogger>();

            logger.Log(LogLevels.Standard, "Loaded Components");
        }

    }
}
