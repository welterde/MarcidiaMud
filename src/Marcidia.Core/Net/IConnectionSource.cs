using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Net
{
    public interface IConnectionSource : IDisposable
    {
        event EventHandler<ConnectionEventArgs> NewConnection;
        void Start();
        void Stop();
    }
}
