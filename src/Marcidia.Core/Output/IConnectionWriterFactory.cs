using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.ComponentModel;
using Marcidia;
using System.Reflection;
using Marcidia.Net;
using Marcidia.Logging;

namespace Marcidia.Output
{
    public interface IConnectionWriterFactory
    {
        IConnectionWriter GetWriterFor(IConnection connection);
    }
}
