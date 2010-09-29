using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.ComponentModel;

namespace Marcidia.Commands
{
    [MarcidiaComponent("Command Manager", false)]
    public class CommandManager : MarcidiaComponent, ICommandRegistrar, ICommandContextFactory
    {
        CommandInfoBuilder commandInfoBuilder;
        Dictionary<Type, List<CommandInfo>> actorCommandMap;

        public CommandManager(Mud mud)
            : base(mud)
        {
            commandInfoBuilder = new CommandInfoBuilder();
            actorCommandMap = new Dictionary<Type, List<CommandInfo>>();

            Mud.Services.AddService<ICommandRegistrar>(this);
            Mud.Services.AddService<ICommandContextFactory>(this);
        }


        public override void Initialize()
        {         
        }

        public void RegisterCommand(Type commandType)
        {
            CommandInfo commandInfo = GetCommandInfo(commandType);

            lock (actorCommandMap)
            {
                List<CommandInfo> commands = null;

                commands = GetOrCreateCommandListForActorType(commandInfo.ActorType);

                if (commands.Any(c => c.CommandType == commandType))
                    throw new ArgumentException(
                        string.Format("Command of type {0} has already been registered", commandType),
                        "commandType");

                if (commands.Any(c => c.CommandName == commandInfo.CommandName))
                    throw new ArgumentException(
                        string.Format("A command with the name {0} has already been regisetered", commandInfo.CommandName),
                        "commandType");

                commands.Add(commandInfo);
            }
        }

        public CommandContext CreateCommandContext()
        {
            CommandBuilder commandBuilder = new CommandBuilder(Mud.Services);

            return new CommandContext(actorCommandMap, commandBuilder);
        }

        private List<CommandInfo> GetOrCreateCommandListForActorType(Type actorType)
        {
            List<CommandInfo> commands;
            if (actorCommandMap.ContainsKey(actorType))
            {
                commands = actorCommandMap[actorType];
            }
            else
            {
                commands = new List<CommandInfo>();
                actorCommandMap.Add(actorType, commands);
            }
            return commands;
        }

        private CommandInfo GetCommandInfo(Type commandType)
        {
            return commandInfoBuilder.Build(commandType);
        }
    }
}