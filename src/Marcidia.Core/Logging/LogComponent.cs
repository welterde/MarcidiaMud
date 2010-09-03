using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.ComponentModel;
using Marcidia;
using System.IO;
using System.Configuration;

namespace Marcidia.Logging
{
    // We set autoload to false because we want to AutoComponentLoader to ignore it
    [MarcidiaComponent("Logging Subsystem", false)]
    public class LogComponent : MarcidiaComponent
    {
        LogDispatcher logDispatcher;
        
        public LogComponent(Mud mud)
            : base(mud)
        {
            logDispatcher = new LogDispatcher();
            
            mud.Services.AddService<ILogger>(logDispatcher);
            mud.Services.AddService<ILogDispatcher>(logDispatcher);
        }

        public override void Initialize()
        {
            string LogFolder = GetConfiguredLogFolder();

            Directory.CreateDirectory(LogFolder);

            logDispatcher.AddLogger(
                LogLevels.Standard | LogLevels.Warning | LogLevels.Error,
                new FileLogger(LogFolder));

#if DEBUG
            logDispatcher.AddLogger(
                LogLevels.Standard | LogLevels.Warning | LogLevels.Error,
                new ConsoleLogger());
#endif            
        }

        private string GetConfiguredLogFolder()
        {
            return (string)ConfigurationManager.GetSection("Marcidia.FileLogger");
        }
    }
}
