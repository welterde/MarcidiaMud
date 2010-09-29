using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Output
{
    public class NullConnectionWriter : IConnectionWriter
    {
        public void Write(string str)
        {
            
        }

        public void Write(string format, params object[] args)
        {
            
        }

        public void WriteLine(string str)
        {
            
        }

        public void WriteLine(string format, params object[] args)
        {
            
        }

        public bool ColorEnabled
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }
    }
}
