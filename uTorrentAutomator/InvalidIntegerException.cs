using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetTorrent.Exceptions
{
    public class InvalidIntegerException : Exception
    {
        private string integer;

        public InvalidIntegerException(string integer)
        {
            this.integer = integer;
        }

        public override string Message
        {
            get
            {
                return string.Format("The integer value '{0}' is invalid.", integer);
            }
        }
    }
}
