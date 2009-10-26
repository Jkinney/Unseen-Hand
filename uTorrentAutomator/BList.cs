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
    /// Represents a BEncoding list.
    /// </summary>
    public class BList : IBValue
    {
        /// <summary>
        /// Private member used to store the items represented by this object.
        /// </summary>
        private List<IBValue> items;

        /// <summary>
        /// Initializes a new instance of BList.
        /// </summary>
        public BList()
        {
            this.items = new List<IBValue>();
        }

        /// <summary>
        /// Gets a collection of items contained in this object.
        /// </summary>
        public List<IBValue> Items
        {
            get
            {
                return items;
            }
        }

        /// <summary>
        /// Adds a new item to this list object.
        /// </summary>
        /// <param name="value">The IBValue object to add.</param>
        public void Add(IBValue value)
        {
            this.items.Add(value);
        }

        /// <summary>
        /// Adds a new item to this list object.
        /// </summary>
        /// <param name="value">The integer number to add.</param>
        public void Add(decimal value)
        {
            this.items.Add(new BInteger(value));
        }

        /// <summary>
        /// Adds a new item to this list object.
        /// </summary>
        /// <param name="value">The string to add.</param>
        public void Add(string value)
        {
            this.items.Add(new BString(value));
        }

        /// <summary>
        /// Adds a new item to this list object.
        /// </summary>
        /// <param name="value">The byte array to add.</param>
        public void Add(byte[] value)
        {
            this.items.Add(new BString(value));
        }

        /// <summary>
        /// Serializes this list to its bencoding representation.
        /// </summary>
        /// <returns>A string containing this list bencoded.</returns>
        string IBValue.ToBEncodedString()
        {
            StringBuilder sb = new StringBuilder(1024);

            sb.Append('l');
            foreach (IBValue value in items)
            {
                sb.Append(value.ToBEncodedString());
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
            return "Count = " + items.Count.ToString();
        }
    }
}
