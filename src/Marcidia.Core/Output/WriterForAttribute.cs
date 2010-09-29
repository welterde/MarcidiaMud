using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Net;

namespace Marcidia.Output
{
    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=false)]
    public class WriterForAttribute : Attribute
    {
        public Type ConnectionType { get; private set; }

        public WriterForAttribute(Type connectionType)
        {
            if (connectionType == null)
                throw new ArgumentNullException("connectionType", "connectionType is null.");

            if (!typeof(IConnection).IsAssignableFrom(connectionType))
                throw new ArgumentException(
                    string.Format(
                        "WriterFor attribute is invalid. {0} is not an implementation of {1}",
                        connectionType.Name,
                    typeof(IConnection).Name));

            this.ConnectionType = connectionType;
        }
    }
}
