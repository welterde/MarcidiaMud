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
    public class ConnectionWriterBuilder
    {
        Type[] ignoredConnectionWriters =
        {
            typeof(DefaultConnectionWriter),
            typeof(NullConnectionWriter)
        };

        ILogger logger;
        Dictionary<Type, Type> connectionToWriterTypeMap;

        public ConnectionWriterBuilder(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException("logger", "logger is null.");

            this.logger = logger;
            connectionToWriterTypeMap = new Dictionary<Type, Type>();
        }

        public void Initialize()
        {
            FindAllWriters();
        }

        public IConnectionWriter Build(IConnection connection)
        {
            Type connectionType = connection.GetType();

            lock (connectionToWriterTypeMap)
            {
                if (!connectionToWriterTypeMap.ContainsKey(connectionType))
                {
                    logger.Log(LogLevels.Warning, "Connection type {0} does not have an associated ConnectionWriter. Using Default Writer", connectionType.Name);
                    return new DefaultConnectionWriter(connection);
                }

                Type writerType = connectionToWriterTypeMap[connectionType];

                return Activator.CreateInstance(writerType, connection) as IConnectionWriter;
            }
        }

        private void FindAllWriters()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                FindWritersIn(assembly);
            }
        }

        private void FindWritersIn(Assembly assembly)
        {
            IEnumerable<Type> writerTypes = assembly.GetTypes().Where(t => typeof(IConnectionWriter).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var writerType in writerTypes)
            {                
                if (ignoredConnectionWriters.Contains(writerType))
                    continue;

                WriterForAttribute forAttribute = null;

                try
                {
                    forAttribute = writerType.GetCustomAttributes(false).OfType<WriterForAttribute>()
                                                                        .FirstOrDefault();
                }
                catch (ArgumentException ex)
                {
                    logger.Log(LogLevels.Warning, ex.Message);
                    continue;
                }

                if (forAttribute == null)
                {
                    logger.Log(LogLevels.Warning, "Found Writer {0} but writer was missing the WriterFor attribute", writerType);
                    continue;
                }

                AddConnectionWriterMap(writerType, forAttribute);
            }
        }

        private void AddConnectionWriterMap(Type writerType, WriterForAttribute forAttribute)
        {
            lock (connectionToWriterTypeMap)
            {
                connectionToWriterTypeMap.Add(forAttribute.ConnectionType, writerType);
            }
        }
    }
}
