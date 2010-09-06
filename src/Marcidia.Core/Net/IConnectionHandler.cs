using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Net
{
    public interface IConnectionHandler
    {
        void HandleConnection(IConnection connection);
    }
}
