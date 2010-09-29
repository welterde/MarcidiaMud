using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Commands
{
    public class CommandInfo
    {
        public CommandInfo(string commandName, Type commandType, Type actorType)
        {
            if (String.IsNullOrEmpty(commandName))
                throw new ArgumentException("commandName is null or empty.", "commandName");
            if (commandType == null)
                throw new ArgumentNullException("commandType", "commandType is null.");
            if (actorType == null)
                throw new ArgumentNullException("actorType", "actorType is null.");

            CommandName = commandName;
            CommandType = commandType;
            ActorType = actorType;
        }

        public Type CommandType { get; private set; }
        public string CommandName { get; private set; }
        public Type ActorType { get; private set; }
    }
}
