using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Commands
{
    public class CommandInfoBuilder
    {
        public CommandInfoBuilder()
        {

        }

        public CommandInfo Build(Type commandType)
        {
            if (commandType == null)
                throw new ArgumentNullException("commandType", "commandType is null.");            

            Type actorType = ExtractActorType(commandType);

            string commandName = GetCommandName(commandType);

            return new CommandInfo(commandName, commandType, actorType);
        }

        private string GetCommandName(Type commandType)
        {
            var commandAttribute = commandType.GetCustomAttributes(false).OfType<CommandAttribute>().FirstOrDefault();

            if (commandAttribute == null)
                throw new ArgumentException(
                    string.Format("Command of type {0} is does not have a CommandAttribute", commandType.Name),
                    "commandType");

            return commandAttribute.CommandName;
        }

        private Type ExtractActorType(Type commandType)
        {
            Type baseGenericType = FindBaseGenericType(commandType);

            if (baseGenericType == null)
                throw new ArgumentException("Command Type does not inherit from Command<T>", "commandType");

            return baseGenericType.GetGenericArguments().First();
        }

        private Type FindBaseGenericType(Type commandType)
        {
            Type type = commandType;

            while (!IsGenericCommandBaseType(type))
            {               
                if (type == typeof(object))
                    return null;

                type = type.BaseType;
            }

            return type;
        }

        private bool IsGenericCommandBaseType(Type type)
        {
            return !(type == typeof(object)) &&
                    type.IsGenericType &&
                    type.GetGenericTypeDefinition() == typeof(Command<>);
        }
    }
}
