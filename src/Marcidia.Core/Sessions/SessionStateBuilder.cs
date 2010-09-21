using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Marcidia.Sessions
{
    public class SessionStateBuilder
    {
        public SessionState Build(Type sessionStateType)
        {
            if (sessionStateType == null)
                throw new ArgumentNullException("sessionStateType", "sessionStateType is null.");

            if (!typeof(SessionState).IsAssignableFrom(sessionStateType))
                throw new ArgumentException("Type passed is not a decendent of SessionState", "sessionStateType");

            ConstructorInfo constructor = sessionStateType.GetConstructor(Type.EmptyTypes);

            if (constructor == null)
                throw new ArgumentException(
                    string.Format("Type {0} does not have a default constructor", sessionStateType.Name),
                    "sessionStateType");

            return (SessionState)constructor.Invoke(null);
        }
    }
}
