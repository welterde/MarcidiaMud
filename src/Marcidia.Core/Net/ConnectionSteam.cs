using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Marcidia.Net
{
    /// <summary>
    /// An adapter that wraps a connection as a stream
    /// </summary>
    public class ConnectionStream : Stream
    {        
        IConnection connection;

        public ConnectionStream(IConnection connection)
        {
            if (connection == null)
		        throw new ArgumentNullException("connection", "connection is null.");

            this.connection = connection;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            // Doesn't need to flush
        }

        public override long Length
        {
            get { throw new NotSupportedException("Stream does not support retrieval of length"); }
        }

        public override long Position
        {
            get
            {
                throw new NotSupportedException("Stream does not support seeking");
            }
            set
            {
                throw new NotSupportedException("Stream does not support seeking");
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return connection.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("Stream does not support seeking");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            connection.Write(buffer, offset, count);
        }
    }
}
