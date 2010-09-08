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
        bool initialized;
        bool shuttingDown;
        AutoResetEvent shutdownSignal;

        public Mud()
        {
            shutdownSignal = new AutoResetEvent(false);
            Components = new MarcidiaComponentCollection();           
            Services = new ServiceCollection();

            Components.ComponentAdded += Components_ComponentAdded;
            Components.ComponentRemoved += new EventHandler<MarcidiaComponentEventArgs>(Components_ComponentRemoved);
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
            OnInitializing();

            foreach (var component in Components.ToArray())
            {
                component.Initialize();
            }

            initialized = true;
            
            OnInitialized();
        }
        
        private void WaitForShutdownSignal()
        {
            shutdownSignal.WaitOne();
        }

        private void DisposeComponents()
        {
            shuttingDown = true;
            OnShuttingDown();

            foreach (var disposableComponent in Components.OfType<IDisposable>())
            {
                disposableComponent.Dispose();
            }

            Components.Clear();

            OnShutdownCompleted();
        }

        void Components_ComponentRemoved(object sender, MarcidiaComponentEventArgs e)
        {
            if (!shuttingDown)
            {
                IDisposable disposable = e.Component as IDisposable;

                if (disposable != null)
                    disposable.Dispose();
            }
        }

        void Components_ComponentAdded(object sender, MarcidiaComponentEventArgs e)
        {
            if (initialized)
                e.Component.Initialize();
        }
    }
}
