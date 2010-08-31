using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Marcidia.Logging
{
    class FileLogger : ILogger
    {
        private string logFolder;

        public FileLogger(string logFolder)
        {
            this.logFolder = logFolder;
        }

        public void Log(LogLevels logLevel, string str)
        {
            using (var sw = GetStreamWriter())
            {
                sw.WriteLine("{0}:{1:HH-mm-ss}: {2}", logLevel, DateTime.Now, str);
            }
        }

        public void Log(LogLevels logLevel, string format, params object[] args)
        {
            string str = string.Format(format, args);

            Log(logLevel, str);
        }

        private StreamWriter GetStreamWriter()
        {
            StreamWriter sw = new StreamWriter(CreateLogFilePath(), true);

            return sw;
        }

        private string CreateLogFilePath()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");

            string filename = Path.ChangeExtension(date, "log");

            return Path.Combine(logFolder, filename);
        }
    }
}
