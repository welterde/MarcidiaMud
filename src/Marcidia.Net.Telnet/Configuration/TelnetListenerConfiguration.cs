using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Marcidia.Net.Telnet.Configuration
{
    class TelnetListenerConfiguration
    {       
        public TelnetListenerConfiguration(string name, System.Net.IPAddress ipAddress, int port)
        {           
            this.SourceName = name;
            this.IPAddress = ipAddress;
            this.Port = port;
        }

        public string SourceName { get; private set; }
        public IPAddress IPAddress { get; private set; }
        public int Port { get; private set; }
    }
}
