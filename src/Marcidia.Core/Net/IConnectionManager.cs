using System;
using System.Collections.Generic;
namespace Marcidia.Net
{
    interface IConnectionManager
    {
        IEnumerable<IConnection> GetAllConnections();
    }
}
