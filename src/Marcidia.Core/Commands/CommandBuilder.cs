using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Commands
{
    public class CommandBuilder
    {
        IServiceProvider services;

        public CommandBuilder(IServiceProvider services)
        {
            if (services == null)
                throw new ArgumentNullException("services", "services is null.");

            this.services = services;
        }

        public ICommand Build(object actor, CommandInfo commandInfo, CommandArguments arguments)
        {
            return Activator.CreateInstance(commandInfo.CommandType, actor, arguments, services) as ICommand;
        }
    }
}
