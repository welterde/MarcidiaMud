using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Commands
{
    public abstract class Command<T> : ICommand
    {
        public IServiceProvider Services { get; private set; }
        public T Actor { get; private set; }        
        public CommandArguments Arguments { get; private set; }

        public Command(T actor, CommandArguments arguments, IServiceProvider services)
        {
            if (actor == null)
                throw new ArgumentNullException("actor", "actor is null.");
            if (arguments == null)
                throw new ArgumentNullException("arguments", "arguments is null.");
            if (services == null)
                throw new ArgumentNullException("services", "services is null.");

            Services = services;
            Actor = actor;
            Arguments = arguments;
        }

        public abstract void Execute();
    }
}
