using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Net
{
    public interface IConnection : IDisposable
    {
        string ConnectionType { get; }
        string ConnectionEndPoint { get; }

        public bool Connected { get; }
        public bool DataAvailable { get; }

        event EventHandler ConnectionLost;
        event EventHandler ConnectionClosed;

        public int Read(byte[] buffer, int offset, int count);
        public void Write(byte[] buffer, int offset, int count);
        public void Close();
    }
}
