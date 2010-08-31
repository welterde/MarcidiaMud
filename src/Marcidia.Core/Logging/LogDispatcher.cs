using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Logging
{

    internal class LogDispatcher : ILogger, ILogDispatcher
    {
        List<DispatchLogger> loggers;

        public LogDispatcher()
        {
            loggers = new List<DispatchLogger>();
        }

        public void AddLogger(LogLevels logLevels, ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            if (logger == this)
                throw new ArgumentException("can't add the log dispatcher to itself", "logger");

            if (Contains(logger))
                throw new ArgumentException("That logger has already been added to the log dispatcher", "logger");

            lock (loggers)
            {
                loggers.Add(
                    new DispatchLogger(logLevels, logger));
            }            
        }

        public void RemoveLogger(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            if (Contains(logger))
            {
                lock (loggers)
                {
                    loggers.RemoveAll(p => p.UnderlyingLogger == logger);
                }
            }
        }

        public bool Contains(ILogger logger)
        {
            lock (loggers)
            {
                return loggers.Any(d => d.UnderlyingLogger == logger);
            }
        }

        public void Log(LogLevels logLevel, string str)
        {
            DispatchToLoggers(logLevel, logger => logger.Log(logLevel, str));
        }

        public void Log(LogLevels logLevel, string format, params object[] args)
        {
            DispatchToLoggers(logLevel, logger => logger.Log(logLevel, format, args));
        }

        private void DispatchToLoggers(LogLevels logLevel, Action<ILogger> dispatchAction)
        {
            foreach (var logDispatcher in loggers)
            {
                if ((logDispatcher.LogLevel & logLevel) == logLevel)
                {
                    dispatchAction(logDispatcher);
                }
            }
        }

        private class DispatchLogger : ILogger
        {           
            public DispatchLogger(LogLevels logLevel, ILogger logger)
            {
                if (logger == null)
                    throw new ArgumentNullException("logger");

                LogLevel = logLevel;
                this.UnderlyingLogger = logger;
            }

            /// <summary>
            /// Which log levels should this logger be used for
            /// </summary>
            public LogLevels LogLevel { get; private set; }

            public ILogger UnderlyingLogger { get; private set; }

            public void Log(LogLevels logLevel, string str)
            {
                UnderlyingLogger.Log(logLevel, str);
            }

            public void Log(LogLevels logLevel, string format, params object[] args)
            {
                UnderlyingLogger.Log(logLevel, format, args);
            }
        }
    }
}
