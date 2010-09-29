using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Net;

namespace Marcidia.Output
{
    class DefaultConnectionWriter : ConnectionWriterBase
    {
        private const string NewLine = "\r\n";

        public DefaultConnectionWriter(IConnection connection)
            : base(connection)
        {            
        }

        public override void Write(string str)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(str);

            Connection.Write(bytes, 0, bytes.Length);
        }

        public override void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }
        
        public override void WriteLine(string str)
        {
            Write(str + NewLine);
        }

        public override void WriteLine()
        {
            WriteLine(string.Empty);
        }

        public override void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }
    }
}
