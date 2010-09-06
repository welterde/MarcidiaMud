﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.ComponentModel;
using Marcidia;

namespace Marcidia.Net
{
    [MarcidiaComponent(
        "Network and Connections Subsystem", false, 
        Description="The core component of the Network and Connection sub system")]
    public class ConnectionComponent : MarcidiaComponent, IDisposable
    {
        Dictionary<string, IConnectionSource> connectionSources;
        Dictionary<string, IConnectionHandler> sourceToHandlerMap;

        public ConnectionComponent(Mud mud)
            : base(mud)
        {
            connectionSources = new Dictionary<string, IConnectionSource>();
            sourceToHandlerMap = new Dictionary<string, IConnectionHandler>();

            mud.Initialized += mud_Initialized;
        }        

        public override void Initialize()
        {
            // We don't need to do anything
        }

        public void RegisterConnectionSource(string name, IConnectionSource connectionSource)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name is null or empty.", "name");

            if (connectionSource == null)
                throw new ArgumentNullException("connectionSource", "connectionSource is null.");

            if (connectionSources.ContainsKey(name))
                throw new ArgumentException(
                    string.Format("A connection source has already been registered with the name: {0}", name),
                    "name");

            connectionSources.Add(name, connectionSource);
        }

        public void RegisterConnectionHandler(string sourceName, IConnectionHandler connectionHandler)
        {
            if (String.IsNullOrEmpty(sourceName))
                throw new ArgumentException("sourceName is null or empty.", "sourceName");
            if (connectionHandler == null)
                throw new ArgumentNullException("connectionHandler", "connectionHandler is null.");

            if (connectionSources.ContainsKey(sourceName))
                throw new ArgumentException(
                    string.Format("A Connection handler has already been registered for connection source: {0}", sourceName),
                    "sourceName");

            sourceToHandlerMap.Add(sourceName, connectionHandler);
        }

        public void Dispose()
        {
            foreach (var connectionSource in connectionSources.Values)
            {
                connectionSource.NewConnection -= connectionSource_NewConnection;
                connectionSource.Stop();                
            }
        }

        private void mud_Initialized(object sender, EventArgs e)
        {
            // As we need to wait until everything is initialized prior to wiring everything up,
            // we tie into the mud initialized event. Here we check for matching sources and handlers and wire them up
            foreach (var sources in connectionSources)
            {
                string sourceName = sources.Key;
                IConnectionSource connectionSource = sources.Value;

                if (sourceToHandlerMap.ContainsKey(sourceName))
                {
                    connectionSource.NewConnection += connectionSource_NewConnection;
                    connectionSource.Start();
                }

            }
        }

        private void connectionSource_NewConnection(object sender, ConnectionEventArgs e)
        {
            string connectionSourceKey = connectionSources.Single(s => s.Value == sender).Key;

            IConnectionHandler handler = sourceToHandlerMap[connectionSourceKey];

            handler.HandleConnection(e.Connection);
        }
    }
}
