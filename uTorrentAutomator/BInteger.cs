/*
 * Created by Bruno Piovan - http://weblogs.asp.net/brunopiovan/default.aspx
 * Sunday, May 03, 2009 7:31:36 PM
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace DotNetTorrent.BEncoding
{
    /// <summary>
    /// Represents a BEncoding integer number.
    /// </summary>
    public class BInteger : IBValue
    {
        /// <summary>
        /// Private member used to store the number represented by this object.
        /// </summary>
        private decimal value;

        /// <summary>
        /// Initializes a new instance of BInteger.
        /// </summary>
        /// <param name="value">An integer number to be represented.</param>
        public BInteger(decimal value)
        {
            this.value = value;
        }

        /// <summary>
        /// Sets a new integer to be represented by this object.
        /// </summary>
        /// <param name="value">The new integer number.</param>
        /// <remarks>Only the integer part of the value is stored.</remarks>
        public void SetValue(decimal value)
        {
            this.value = decimal.Truncate(value);
        }

        /// <summary>
        /// Returns the integer number represented by this object.
        /// </summary>
        /// <returns>A System.Decimal value.</returns>
        public decimal GetValue()
        {
            return this.value;
        }

        /// <summary>
        /// Serializes this object to its bencoding representation.
        /// </summary>
        /// <returns>A string containing the bencoding data.</returns>
        string IBValue.ToBEncodedString()
        {
            return "i" + value.ToString(CultureInfo.InvariantCulture) + "e";
        }

        /// <summary>
        /// Returns a System.String that represents the current System.Object.
        /// </summary>
        /// <returns>A System.String that represents the current System.Object.</returns>
        public override string ToString()
        {
            BDictionary d = new BDictionary();
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
