using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Marcidia.ComponentModel
{
    /// <summary>
    /// Base class for components that do repeatable work on a secondary thread.
    /// </summary>    
    public abstract class MarcidiaWorkerComponent : MarcidiaComponent, IDisposable
    {
        bool running;
        Thread workerThread;
        
        public MarcidiaWorkerComponent(Mud mud)
            : base(mud)
        {
            workerThread = new Thread(DoWork);
        }
        
        public override void Initialize()
        {
            workerThread.Start();
        }

        public void Dispose()
        {
            running = false;

            if (workerThread != null && workerThread.IsAlive)
            {
                workerThread.Interrupt();

                if (workerThread != Thread.CurrentThread)
                    workerThread.Join();
            }
        }

        private void DoThreadLoop()
        {
            try
            {
                while (running)
                {
                    DoWork();
                }
            }
            catch (ThreadInterruptedException)
            {
                // We expect this
            }
        }

        /// <summary>
        /// Does the repeatable work of the <see cref="MarcidiaWorkerComponent"/>
        /// </summary>
        /// <remarks>
        /// When inheriting from this class, if you catch Exception rather 
        /// than catch specific exceptions make sure you rethrow ThreadInterruptedException if they are caught
        /// or you may cause the Mud server to halt on shutdown.
        /// </remarks>
        protected abstract void DoWork();
    }
}
