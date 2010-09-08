using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Marcidia.Logging;

namespace Marcidia.Net.Telnet
{
    public class TelnetConnectionSource : IConnectionSource
    {
        bool started;
        Socket listenSocket;
        IPEndPoint localEndPoint;
        ILogger logger;

        public TelnetConnectionSource(IPEndPoint localEndPoint, ILogger logger)
        {
            this.localEndPoint = localEndPoint;
            this.logger = logger;
        }

        public event EventHandler<ConnectionEventArgs> NewConnection;

        private void OnNewConnection(IConnection connection)
        {
            if (NewConnection != null)
            {
                NewConnection(this, new ConnectionEventArgs(connection));
            }
        }

        public void Start()
        {
            if (started)
                throw new InvalidOperationException("The connection source has already been started");

            started = true;
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(localEndPoint);
            listenSocket.Listen(10);
            listenSocket.BeginAccept(OnConnectionAccepted, null);

            logger.Log(LogLevels.Standard, "Started Listener Socket: {0}", localEndPoint);
        }

        private void OnConnectionAccepted(IAsyncResult result)
        {
            Socket socket = listenSocket.EndAccept(result);

            OnNewConnection(new TelnetConnection(socket));

            listenSocket.BeginAccept(OnConnectionAccepted, null);
        }

        public void Stop()
        {
            if (started)
            {                
                listenSocket.Dispose();
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
