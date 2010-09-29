using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Marcidia.Commands
{
    public class CommandArguments
    {
        const char EOSMarker = '\0';
        const int BOS = -1;

        int position;
        StringReader stringReader;
        char current;

        public CommandArguments(string commandLine)
        {
            position = BOS;
            current = EOSMarker;
            
            CommandLine = commandLine;
            
            stringReader = new StringReader(commandLine);

            ParseCommandLine();
        }

        public string CommandName { get; private set; }

        public string CommandLine { get; private set; }

        public string ArgumentLine { get; private set; }

        public string[] Arguments { get; private set; }

        private char GetNext()
        {
            int character = stringReader.Read();

            char c = EOSMarker;

            if (character != -1)
            {
                c = (char)character;
            }

            current = c;

            position++;

            return c;
        }

        private char GetCurrent()
        {
            if (position == BOS)
                return GetNext();

            return current;
        }

        private void ParseCommandLine()
        {
            CommandName = ReadCommandLineArgument();
            SkipWhiteSpace();
            ArgumentLine = CommandLine.Substring(position);

            List<string> arguments = new List<string>();

            while (GetCurrent() != EOSMarker)
            {
                arguments.Add(ReadCommandLineArgument());
            }

            Arguments = arguments.ToArray();
        }

        private string ReadCommandLineArgument()
        {            
            SkipWhiteSpace();

            char c = GetCurrent();

            if (c == '"' || c == '\'')
            {
                Func<char, bool> seperatorPredicate = character => character == c;                
                
                // skip the seperator start
                GetNext();
                
                return ReadCommandLineWithSeperator(seperatorPredicate);
            }
            else
                return ReadCommandLineWithSeperator(char.IsWhiteSpace);

        }

        private string ReadCommandLineWithSeperator(Func<char, bool> seperatorPredicate)
        {
            StringBuilder argumentBuilder = new StringBuilder();

            char c = GetCurrent();

            while (!seperatorPredicate(c) && c != EOSMarker)
            {
                argumentBuilder.Append(c);
                c = GetNext();
            }

            // Skip seperator character, we know it's worthless
            GetNext();

            return argumentBuilder.ToString();
        }        

        private void SkipWhiteSpace()
        {
            char c = GetCurrent();

            while (char.IsWhiteSpace(c))
            {
                c = GetNext();
            }
        }
    }
}
