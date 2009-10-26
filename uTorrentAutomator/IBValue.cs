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
    /// This interface defines a method that serializes an object to its bencoding representation.
    /// </summary>
    public interface IBValue
    {
        /// <summary>
        /// Serializes the object to its bencoding representation.
        /// </summary>
        /// <returns>A string containing the bencoding representation of the object.</returns>
        string ToBEncodedString();
    }
}
