using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.ComponentModel;
using System.Threading;

namespace Marcidia
{
    public abstract class Mud
    {
        AutoResetEvent shutdownSignal;

        public Mud()
        {
            shutdownSignal = new AutoResetEvent(false);
            Components = new MarcidiaComponentCollection();
            Services = new ServiceCollection();
        }

        public event EventHandler Initializing;

        protected virtual void OnInitializing()
        {
            if (Initializing != null)
                Initializing(this, EventArgs.Empty);
        }

        public event EventHandler Initialized;

        protected virtual void OnInitialized()
        {
            if (Initialized != null)
                Initialized(this, EventArgs.Empty);
        }

        public event EventHandler ShuttingDown;

        protected virtual void OnShuttingDown()
        {
            if (ShuttingDown != null)
                ShuttingDown(this, EventArgs.Empty);
        }

        public event EventHandler ShutdownCompleted;


        protected virtual void OnShutdownCompleted()
        {
            if (ShutdownCompleted != null)
                ShutdownCompleted(this, EventArgs.Empty);
        }

        public MarcidiaComponentCollection Components { get; private set; }

        public ServiceCollection Services { get; private set; }

        public void Run()
        {
            Initialize();
            WaitForShutdownSignal();
            DisposeComponents();
        }

        public void Shutdown()
        {
            shutdownSignal.Set();
        }

        protected virtual void Initialize()
        {
            foreach (var component in Components)
            {
                component.Initialize();
            }
        }
        
        private void WaitForShutdownSignal()
        {
            shutdownSignal.WaitOne();
        }

        private void DisposeComponents()
        {
            foreach (var disposableComponent in Components.OfType<IDisposable>())
            {
                disposableComponent.Dispose();
            }

            Components.Clear();
        }                
    }
}
