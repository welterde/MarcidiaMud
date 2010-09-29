using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Net;
using Marcidia.Output;

namespace Marcidia.Sessions
{
    public abstract class SessionState
    {
        public SessionState()
        {
        }

        public Session Session
        {
            get;
            internal set;
        }

        public IServiceProvider Services
        {
            get 
            {
                if (Session == null)
                    return null;

                return Session.Services; 
            }
        }

        public IConnectionWriter Output
        {
            get
            {
                if (Session == null)
                    return null;

                return Session.Output;
            }
        }

        internal void SendInput(string input)
        {
            OnInputRecieved(input);
        }

        protected abstract void OnInputRecieved(string input);
        public abstract void Start();
        public abstract void Stop();
        public abstract void Resume();
        public abstract void Pause();
    }
}
