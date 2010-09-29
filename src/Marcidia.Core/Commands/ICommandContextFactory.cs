using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.ComponentModel;

namespace Marcidia.Commands
{
    public interface ICommandContextFactory
    {
        CommandContext CreateCommandContext();
    }
}
