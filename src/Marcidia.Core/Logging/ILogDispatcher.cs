using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Logging
{
    // Dispenses log output to multiple loggers
    public interface ILogDispatcher
    {
        void AddLogger(LogLevels logLevels, ILogger logger);
        void RemoveLogger(ILogger logger);
        bool Contains(ILogger logger);
    }
}
