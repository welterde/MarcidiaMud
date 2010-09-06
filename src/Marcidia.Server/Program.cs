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

    class CommandLineComponent : MarcidiaComponent, IDisposable
    {
        Thread listenThread;

        public CommandLineComponent(Mud mud)
            : base(mud)
        {
            
        }
                            
        public override void Initialize()
        {
            listenThread = new Thread(DoConsoleReading);
            listenThread.Start();
        }

        private void DoConsoleReading()
        {
            try
            {
                while (true)
                {
                    Console.Write(">");
                    string read = Console.In.ReadLine();

                    if (read == "components")
                    {
                        ListComponents();
                    }
                    else if (read == "shutdown")
                    {
                        Mud.Shutdown();
                    }
                }
            }
            catch (ThreadInterruptedException)
            {
                // Do nothing, as we expect this
            }
        }

        public void Dispose()
        {
            if (listenThread != null && listenThread.IsAlive)
            {
                listenThread.Interrupt();

                if (Thread.CurrentThread != listenThread)
                    listenThread.Join();
            }
        }

        private void ListComponents()
        {
            foreach (var component in Mud.Components)
            {
                var componentInfo = ComponentInfo.GetInformationFor(component);

                if (componentInfo == null)
                {
                    Console.WriteLine(component.GetType().Name);
                }
                else
                {
                    Console.WriteLine("{0} {1} by {2}", componentInfo.Name, componentInfo.Version, componentInfo.Author);
                }
            }
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

            CommandLineComponent cmdLineComponent = new CommandLineComponent(this);
            Components.Add(cmdLineComponent);
        }

        protected override void Initialize()
        {
            base.Initialize();

            logger = Services.GetService<ILogger>();

            logger.Log(LogLevels.Standard, "Loaded Components");
        }

    }
}
