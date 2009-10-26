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
    /// Represents a BEncoding dictionary.
    /// </summary>
    public sealed class BDictionary : IBValue
    {
        /// <summary>
        /// Private list used to store the items contained in this dictionary.
        /// </summary>
        private List<KeyValuePair<string, IBValue>> items;

        /// <summary>
        /// Initializes a new instance of the BDictionary.
        /// </summary>
        public BDictionary()
        {
            items = new List<KeyValuePair<string, IBValue>>();
        }

        /// <summary>
        /// Gets a collection of items contained in this object.
        /// </summary>
        public List<KeyValuePair<string, IBValue>> Items
        {
            get
            {
                return this.items;
            }
        }

        /// <summary>
        /// Adds an object that implements the IBValue interface to the end of this dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The IBValue element to add.</param>
        public void Add(string key, IBValue value)
        {
            this.items.Add(new KeyValuePair<string, IBValue>(key, value));
        }

        /// <summary>
        /// Adds a BInteger to the end of this dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The integer number to add.</param>
        public void Add(string key, decimal value)
        {
            this.Add(key, new BInteger(value));
        }

        /// <summary>
        /// Adds a BString to the end of this dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The string to add.</param>
        public void Add(string key, string value)
        {
            this.Add(key, new BString(value));
        }

        /// <summary>
        /// Adds a BString as a byte array to the end of this dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The byte array to add.</param>
        public void Add(string key, byte[] value)
        {
            this.Add(key, new BString(value));
        }

        /// <summary>
        /// Serializes this dictionary to its bencoding representation.
        /// </summary>
        /// <returns>A string containing this dictionary bencoded.</returns>
        string IBValue.ToBEncodedString()
        {
            StringBuilder sb = new StringBuilder(1024);

            sb.Append('d');
            foreach (KeyValuePair<string, IBValue> value in items)
            {
                sb.Append((new BString(value.Key) as IBValue).ToBEncodedString());
                sb.Append(value.Value.ToBEncodedString());
            }
            sb.Append('e');

            return sb.ToString();
        }

        /// <summary>
        /// Returns a System.String that represents the current System.Object.
        /// </summary>
        /// <returns>A System.String that represents the current System.Object.</returns>
        public override string ToString()
        {
            return "Count = " + items.Count.ToString(CultureInfo.InvariantCulture);
        }
    }
}
