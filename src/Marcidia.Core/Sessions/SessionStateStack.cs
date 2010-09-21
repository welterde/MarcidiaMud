using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Net;

namespace Marcidia.Sessions
{
    class SessionStateStack
    {
        Session owner;
        Stack<SessionState> sessionStates;

        public SessionStateStack(Session owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner", "owner is null.");

            this.owner = owner;

            sessionStates = new Stack<SessionState>();
        }

        public void Push(SessionState state)
        {
            lock (sessionStates)
            {
                SessionState previousState = Peek();

                if (previousState != null)
                    previousState.Pause();

                state.Owner = owner;
                sessionStates.Push(state);
                state.Start();
            }
        }

        public SessionState Pop()
        {
            lock (sessionStates)
            {
                if (sessionStates.Count == 0)
                    return null;

                SessionState currentState = sessionStates.Pop();
                currentState.Stop();
                currentState.Owner = null;

                SessionState previousState = sessionStates.Peek();

                if (previousState != null)
                    previousState.Resume();

                return currentState;
            }
        }

        public SessionState Peek()
        {
            lock (sessionStates)
            {
                if (sessionStates.Count == 0)
                    return null;

                return sessionStates.Peek();
            }
        }
    }
}
