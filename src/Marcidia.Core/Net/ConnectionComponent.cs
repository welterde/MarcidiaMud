using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.ComponentModel;
using Marcidia;

namespace Marcidia.Net
{
    [MarcidiaComponent(
        "Network and Connections Subsystem", false, 
        Description="The core component of the Network and Connection sub system")]
    public class ConnectionComponent : MarcidiaComponent
    {
        public ConnectionComponent(Mud mud)
            : base(mud)
        {
            
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
