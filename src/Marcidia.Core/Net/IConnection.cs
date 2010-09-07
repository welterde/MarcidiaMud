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

        bool Connected { get; }
        bool DataAvailable { get; }

        event EventHandler ConnectionLost;
        event EventHandler ConnectionClosed;

        int Read(byte[] buffer, int offset, int count);
        void Write(byte[] buffer, int offset, int count);
        void Close();
    }
}
