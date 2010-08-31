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

    public class ConnectionEventArgs : EventArgs
    {
        public ConnectionEventArgs(IConnection connection)
        {            
            if (connection == null)
                throw new ArgumentNullException("connection", "connection is null.");

            Connection = connection;
        }

        public IConnection Connection { get; set; }
    }
}
