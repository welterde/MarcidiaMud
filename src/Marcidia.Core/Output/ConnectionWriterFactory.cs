using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.ComponentModel;
using Marcidia;
using System.Reflection;
using Marcidia.Net;
using Marcidia.Logging;

namespace Marcidia.Output
{
    [MarcidiaComponent("Connection Writer Factory", false)]
    public class ConnectionWriterFactory : MarcidiaComponent, IConnectionWriterFactory
    {
        ILogger logger;
        ConnectionWriterBuilder builder;
        Dictionary<IConnection, IConnectionWriter> connectionWriterMap;

        public ConnectionWriterFactory(Mud mud)
            : base(mud)
        {
            connectionWriterMap = new Dictionary<IConnection, IConnectionWriter>();

            Mud.Services.AddService<IConnectionWriterFactory>(this);
            mud.Initialized += mud_Initialized;
        }

        public override void Initialize()
        {
            logger = Mud.Services.GetService<ILogger>();
            builder = new ConnectionWriterBuilder(logger);
        }

        public IConnectionWriter GetWriterFor(IConnection connection)
        {
            if (connection == null)
                return new NullConnectionWriter();

            lock (connectionWriterMap)
            {
                if (connectionWriterMap.ContainsKey(connection))
                    return connectionWriterMap[connection];

                IConnectionWriter writer = builder.Build(connection);

                connection.ConnectionClosed += OnConnectionClosedOrLost;
                connection.ConnectionLost += OnConnectionClosedOrLost;

                connectionWriterMap.Add(connection, writer);

                return writer;
            }
        }

        private void OnConnectionClosedOrLost(object sender, EventArgs e)
        {
            IConnection connection = (IConnection)sender;

            lock (connectionWriterMap)
            {
                connectionWriterMap.Remove(connection);
            }
        }

        void mud_Initialized(object sender, EventArgs e)
        {
            // We have to wait for everything to be loaded to be sure all needed assemblies are in memory so we do
            // our stuff here
            builder.Initialize();
        }        
    }
}
