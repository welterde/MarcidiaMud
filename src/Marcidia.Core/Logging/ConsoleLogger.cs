using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Logging
{
    class ConsoleLogger : ILogger
    {
        public void Log(LogLevels logLevel, string str)
        {
            SetConsoleColor(logLevel);

            Console.WriteLine(str);
        }

        public void Log(LogLevels logLevel, string format, params object[] args)
        {
            SetConsoleColor(logLevel);

            Console.WriteLine(format, args);
        }

        private void SetConsoleColor(LogLevels logLevel)
        {
            switch (logLevel)
            {
                case LogLevels.Standard:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevels.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevels.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
        }
    }
}
