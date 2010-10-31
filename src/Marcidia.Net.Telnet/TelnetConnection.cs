using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Marcidia.Net.Telnet
{
    class TelnetConnection : IConnection
    {
        private Socket socket;

        public TelnetConnection(Socket socket)
        {            
            this.socket = socket;
            ConnectionEndPoint = socket.RemoteEndPoint.ToString();
            Connected = true;
        }

        public string ConnectionType
        {
            get { return "Telnet"; }
        }

        public string ConnectionEndPoint
        {
            get;
            private set;
        }

        public bool Connected
        {
            get;
            private set;
        }

        public event EventHandler ConnectionLost;

        private void OnConnectionLost()
        {
            Connected = false;

            if (ConnectionLost != null)
                ConnectionLost(this, EventArgs.Empty);
        }

        public event EventHandler ConnectionClosed;

        private void OnConnectionClosed()
        {
            Connected = false;

            if (ConnectionClosed != null)
                ConnectionClosed(this, EventArgs.Empty);
        }

        public int Read(byte[] buffer, int offset, int count)
        {            
            int bytesRead = socket.Receive(buffer, offset, count, SocketFlags.None);

            if (bytesRead == 0)
                OnConnectionLost();

            return bytesRead;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            if (count == 0)
                return;

            int bytesSent = 0;

            do
            {
                bytesSent = socket.Send(buffer, offset, count, SocketFlags.None);
                count -= bytesSent;
            } while (count > 0 && bytesSent > 0);

            if (bytesSent == 0)
                OnConnectionLost();
        }

        public void Close()
        {
            OnConnectionClosed();
            socket.Close();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
