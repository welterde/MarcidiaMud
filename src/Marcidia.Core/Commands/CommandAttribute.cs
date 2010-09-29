using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string CommandName { get; private set; }

        public CommandAttribute(string commandName)
        {            
            if (String.IsNullOrEmpty(commandName))
                throw new ArgumentException("commandName is null or empty.", "commandName");

            CommandName = commandName.ToLower();
        }
    }
}
