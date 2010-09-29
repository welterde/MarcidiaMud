using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Net;

namespace Marcidia.Output
{
    public abstract class ConnectionWriterBase : IConnectionWriter
    {
        protected IConnection Connection { get; private set; }

        public ConnectionWriterBase(IConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection", "connection is null.");

            Connection = connection;
        }


        private bool colorEnabled;
        public bool ColorEnabled
        {
            get
            {
                return colorEnabled;
            }
            set
            {

                if (colorEnabled == value)
                    return;

                colorEnabled = value;
                OnColorEnabledChanged();
            }
        }

        protected virtual void OnColorEnabledChanged()
        {
        }

        public abstract void Write(string str);
        public abstract void Write(string format, params object[] args);
        public abstract void WriteLine();
        public abstract void WriteLine(string str);
        public abstract void WriteLine(string format, params object[] args);
    }
}
