using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Logging
{
    [Flags]
    public enum LogLevels
    {
        Standard = 1,
        Warning = 2,
        Error = 4
    }

    public interface ILogger
    {
        void Log(LogLevels logLevel, string str);
        void Log(LogLevels logLevel, string format, params object[] args);
    }
}
