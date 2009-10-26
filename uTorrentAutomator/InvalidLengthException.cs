using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetTorrent.Exceptions
{
    public class InvalidLengthException : Exception
    {
        private string length;

        public InvalidLengthException(string length)
        {
            this.length = length;
        }

        public override string Message
        {
            get
            {
                return string.Format("The length value '{0}' is invalid.", length);
            }
        }
    }
}