using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Net
{
    public interface IConnectionHandlerRegistrar
    {
        void RegisterConnectionHandler(string sourceName, IConnectionHandler connectionHandler);
    }
}
