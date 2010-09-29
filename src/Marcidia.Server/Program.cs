using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Logging;
using System.IO;
using System.Threading;
using Marcidia.ComponentLoading;
using Marcidia.Net;
using Marcidia.ComponentModel;
using Marcidia;

namespace Marcidia.Server
{
    class Program
    {
        static void Main(string[] args)
        {            
            MarcidiaMud mud = new MarcidiaMud();

            mud.Run();
        }
    }
}
