/*
 * Created by Bruno Piovan - http://weblogs.asp.net/brunopiovan/default.aspx
 * Sunday, May 03, 2009 7:31:36 PM
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetTorrent.BEncoding
{
    /// <summary>
    /// Represents a BEncoding byte string.
    /// </summary>
    public class BString : IBValue
    {
        /// <summary>
        /// Private member used to store the string represented by this object.
        /// </summary>
        private string value;

        /// <summary>
        /// Initializes a new instance of BString.
        /// </summary>
        /// <param name="value">A string to be represented.</param>
        public BString(string value)
        {
            this.SetValue(value);
        }

        /// <summary>
        /// Initializes a new instance of BString.
        /// </summary>
        /// <param name="value">A byte array to be represented.</param>
        public BString(byte[] value)
        {
            this.SetValue(value);
        }

        /// <summary>
        /// Sets a new string to be represented by this object.
        /// </summary>
        /// <param name="value">The new string value to set.</param>
        public void SetValue(string value)
        {
            //checks if the string contains any unicode char
            bool unicode = false;
            foreach (char c in value)
            {
                if (c > (char)byte.MaxValue)
                {
                    unicode = true;
                    break;
                }
            }

            //if it does, then get the bytes using UTF-8 and stores it.
            //I didn't find the specification very clear about this... it worked as expected with the tests I made
            //please, let me know if it's wrong...
            if (unicode)
            {
                this.SetValue(Encoding.UTF8.GetBytes(value));
            }
            else
            {
                //there is no unicode chars, so store the value as it is
                this.value = value;
            }
        }

        /// <summary>
        /// Sets a new byte array to be represented by this object.
        /// </summary>
        /// <param name="value">The byte array to set.</param>
        public void SetValue(byte[] value)
        {
            StringBuilder buffer = new StringBuilder(value.Length);
            foreach (byte b in value)
            {
                buffer.Append((char)b);
            }

            this.value = buffer.ToString();
        }

        /// <summary>
        /// Gets the value represented by this object.
        /// </summary>
        /// <returns>A string represented by this object.</returns>
        public string GetValue()
        {
            return this.value;
        }

        /// <summary>
        /// Serializes this object to its bencoding representation.
        /// </summary>
        /// <returns>A string containing the bencoding data.</returns>
        string IBValue.ToBEncodedString()
        {
            return this.value.Length.ToString() + ":" + this.value;
        }

        /// <summary>
        /// Returns a System.String that represents the current System.Object.
        /// </summary>
        /// <returns>A System.String that represents the current System.Object.</returns>
        public override string ToString()
        {
            return this.value;
        }
    }
}
