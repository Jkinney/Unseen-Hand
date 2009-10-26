using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetTorrent.Exceptions
{
    public class InvalidCharacterException : Exception
    {
        private char found;
        private string expected;

        public InvalidCharacterException(char found, string expected)
        {
            this.found = found;
            this.expected = expected;
        }

        public override string Message
        {
            get
            {
                return string.Format("Character {0} found but {1} was expected.", found, expected);
            }
        }
    }
}
