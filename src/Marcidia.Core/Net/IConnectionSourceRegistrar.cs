using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Net
{
    public interface IConnectionSourceRegistrar
    {
        void RegisterConnectionSource(string name, IConnectionSource connectionSource);
    }
}
