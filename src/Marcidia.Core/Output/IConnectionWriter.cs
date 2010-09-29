using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Output
{
    /// <summary>
    /// Provides a friendly interface for writing to a connection
    /// </summary>
    public interface IConnectionWriter
    {
        bool ColorEnabled { get; set; }
        
        void Write(string str);
        void Write(string format, params object[] args);
        void WriteLine();
        void WriteLine(string str);
        void WriteLine(string format, params object[] args);
    }
}
