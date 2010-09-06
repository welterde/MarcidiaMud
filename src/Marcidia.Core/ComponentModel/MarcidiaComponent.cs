using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Marcidia.ComponentModel
{
    public abstract class MarcidiaComponent : IInitializable
    {
        public MarcidiaComponent(Mud mud)
        {
            if (mud == null)
                throw new ArgumentNullException("services", "services is null.");

            this.Mud = mud;
        }

        public Mud Mud { get; private set; }

        public abstract void Initialize();
    }
}
