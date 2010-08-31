using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Marcidia.ComponentModel
{
    public class MarcidiaComponentEventArgs : EventArgs
    {
        public MarcidiaComponentEventArgs(MarcidiaComponent component)
        {
            if (component == null)
                throw new ArgumentNullException("component", "component is null.");

            Component = component;
        }

        public MarcidiaComponent Component { get; private set; }
    }
}
