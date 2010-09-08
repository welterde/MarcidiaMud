using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.ComponentModel;
using Marcidia.Net.Telnet.Configuration;
using System.Configuration;
using System.Net;
using Marcidia.Logging;

namespace Marcidia.Net.Telnet
{
    [MarcidiaComponent("Telnet Connection Component", true, Description="Allows the use of TCP Telnet ports as a connection source")]
    public class TelnetConnectionComponent : MarcidiaComponent
    {
        IConnectionSourceRegistrar connectionSoruceRegistrar;
        ILogger logger;

        public TelnetConnectionComponent(Mud mud)
            : base(mud)
        {
        }

        public override void Initialize()
        {
            RetrieveServices();

            Configure();            
        }

        private void RetrieveServices()
        {
            connectionSoruceRegistrar = Mud.Services.GetService<IConnectionSourceRegistrar>();
            logger = Mud.Services.GetService<ILogger>();
        }

        private void Configure()
        {
            IEnumerable<TelnetListenerConfiguration> listenerConfigs = LoadListenerCongfigurations();

            foreach (var config in listenerConfigs)
            {
                IPEndPoint localEndPoint = new IPEndPoint(config.IPAddress, config.Port);

                IConnectionSource connectionSource = new TelnetConnectionSource(localEndPoint, logger);

                connectionSoruceRegistrar.RegisterConnectionSource(config.SourceName, connectionSource);
            }
        }

        private IEnumerable<TelnetListenerConfiguration> LoadListenerCongfigurations()
        {
            return ConfigurationManager.GetSection("Marcidia.Net.Telnet") as IEnumerable<TelnetListenerConfiguration>;
        }
    }
}
