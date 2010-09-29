using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Commands
{
    public class CommandContext
    {
        CommandBuilder commandBuilder;
        Dictionary<Type, object> actors;
        Dictionary<Type, List<CommandInfo>> actorCommandMap;
        List<CommandInfo> availableCommands;

        internal CommandContext(Dictionary<Type, List<CommandInfo>> actorCommandMap, CommandBuilder commandBuilder)
        {
            if (commandBuilder == null)
                throw new ArgumentNullException("services", "services is null.");
            if (actorCommandMap == null)
                throw new ArgumentNullException("actorCommandMap", "actorCommandMap is null.");

            this.availableCommands = new List<CommandInfo>();
            this.actors = new Dictionary<Type, object>();
            this.commandBuilder = commandBuilder;            
            this.actorCommandMap = actorCommandMap;
        }

        public void RegisterActor<ActorT>(ActorT actor)
        {
            lock (actors)
            {
                if (actors.ContainsKey(typeof(ActorT)))
                    throw new ArgumentException(
                        string.Format(
                            "An actor of type {0} has already been registered in this command context",
                            typeof(ActorT)),
                        "actor");

                actors.Add(typeof(ActorT), actor);
            }

            UpdateAvailableCommands();
        }

        public ICommand FindCommand(CommandArguments arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException("arguments", "arguments is null.");

            lock (availableCommands)
            {
                string lowerCommandName = arguments.CommandName.ToLower();

                CommandInfo commandInfo = availableCommands.FirstOrDefault(c => c.CommandName.StartsWith(lowerCommandName));

                if (commandInfo == null)
                    return null;

                object actor = actors[commandInfo.ActorType];

                return commandBuilder.Build(actor, commandInfo, arguments);
            }
        }

        private void UpdateAvailableCommands()
        {
            lock (availableCommands)
            {
                availableCommands.Clear();

                lock (actors)
                    lock (actorCommandMap)
                    {
                        foreach (var item in actors)
                        {
                            if (actorCommandMap.ContainsKey(item.Key))
                                availableCommands.AddRange(actorCommandMap[item.Key]);
                        }
                    }

                availableCommands.Sort((x, y) => x.CommandName.CompareTo(y.CommandName));
            }
        }
    }
}
