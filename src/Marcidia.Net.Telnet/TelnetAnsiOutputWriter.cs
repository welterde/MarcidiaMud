using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Output;
using Marcidia.Net;

namespace Marcidia.Net.Telnet
{
    [WriterFor(typeof(TelnetConnection))]
    public class TelnetAnsiOutputWriter : ConnectionWriterBase
    {
        private const string NewLine = "\r\n";
        private const char esacpeCharacter = '`';
        private Dictionary<char, string> colorToAnsiMap = new Dictionary<char,string>()
        {
            { 'k', "\u001B[0;30m" },
            { 'r', "\u001B[0;31m" },
            { 'g', "\u001B[0;32m" },
            { 'y', "\u001B[0;33m" },
            { 'b', "\u001B[0;34m" },
            { 'm', "\u001B[0;35m" },
            { 'c', "\u001B[0;36m" },
            { 'w', "\u001B[0;37m" },
            { 'K', "\u001B[1;30m" },
            { 'R', "\u001B[1;31m" },
            { 'G', "\u001B[1;32m" },
            { 'Y', "\u001B[1;33m" },
            { 'B', "\u001B[1;34m" },
            { 'M', "\u001B[1;35m" },
            { 'C', "\u001B[1;36m" },
            { 'W', "\u001B[1;37m" }
        };

        public TelnetAnsiOutputWriter(IConnection connection)
            : base(connection)
        {
            
        }
          
        public override void Write(string str)
        {
            if (str == null)
                throw new ArgumentException("str is null or empty.", "str");

            if (str == String.Empty)
                return;

            string outputString = ParseColorCodes(str);

            byte[] bytes = Encoding.ASCII.GetBytes(outputString);

            Connection.Write(bytes, 0, bytes.Length);
        }

        public override void Write(string format, params object[] args)
        {
            if (String.IsNullOrEmpty(format))
                throw new ArgumentException("format is null or empty.", "format");

            Write(string.Format(format, args));
        }

        public override void WriteLine()
        {
            Write(NewLine);
        }

        public override void WriteLine(string str)
        {
            Write(str + NewLine);
        }

        public override void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        private string ParseColorCodes(string str)
        {
            StringBuilder finalString = new StringBuilder();

            int finalIndex = str.Length - 1;

            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                if (c == esacpeCharacter)
                {
                    if (i == finalIndex)
                        finalString.Append(esacpeCharacter);

                    i++;
                    c = str[i];

                    if (c == esacpeCharacter)
                    {
                        finalString.Append(c);
                        continue;
                    }
                    else if (colorToAnsiMap.ContainsKey(c))
                    {
                        finalString.Append(colorToAnsiMap[c]);
                    }
                    else
                    {
                        finalString.Append(esacpeCharacter);
                        finalString.Append(c);
                    }
                }
                else
                {
                    finalString.Append(c);
                }
            }

            return finalString.ToString();
        }
    }
}
