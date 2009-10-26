/*
 * Created by Bruno Piovan - http://weblogs.asp.net/brunopiovan/default.aspx
 * Sunday, May 03, 2009 7:31:36 PM
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;
using DotNetTorrent.Exceptions;

namespace DotNetTorrent.BEncoding
{
    /// <summary>
    /// Helper class with methods related to torrent files.
    /// </summary>
    public static class Torrent
    {
        /// <summary>
        /// Represents the info_hash of a torrent file.
        /// </summary>
        public struct InfoHash
        {
            /// <summary>
            /// Gets the hash as an array of bytes.
            /// </summary>
            public byte[] Bytes { get; private set; }

            /// <summary>
            /// Creates a new InfoHash object from an array of bytes.
            /// </summary>
            /// <param name="bytes">The array of bytes used to create this object from.</param>
            /// <returns>A new InfoHash object.</returns>
            public static InfoHash FromByteArray(byte[] bytes)
            {
                InfoHash infohash = new InfoHash();
                infohash.Bytes = bytes;

                return infohash;
            }

            /// <summary>
            /// Converts this InfoHash object to it's string representation.
            /// </summary>
            /// <returns>A string representing this object.</returns>
            public override string ToString()
            {
                return BitConverter.ToString(this.Bytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Parses a torrent file and returns a new instance of BDictionary representing the parsed file upon successful parsing.
        /// </summary>
        /// <param name="path">The path of the file to parse.</param>
        /// <returns>A new instance of BDicionary if the parsing is successful.</returns>
        public static BDictionary ParseTorrentFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    return ParseTorrentFile(fs);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// Parses a torrent file and returns a new instance of BDictionary representing the parsed file upon successful parsing.
        /// </summary>
        /// <param name="input">The stream containing the the torrent file to parse.</param>
        /// <returns>A new instance of BDicionary if the parsing is successful.</returns>
        public static BDictionary ParseTorrentFile(Stream input)
        {
            if (input.Position != 0) input.Position = 0;
            BinaryReader br = new BinaryReader(input);

            char c = (char)br.PeekChar();
            if (c != 'd')
                throw new InvalidCharacterException(c, "'d'"); // .torrent files should start with a 'd' char

            return Parse(br) as BDictionary;
        }

        /// <summary>
        /// Private method that parses the the current position of the stream and returns the appropriate object.
        /// </summary>
        /// <param name="br">The BinaryReader with the data to parse.</param>
        /// <returns>The appropriate object representing the item parsed.</returns>
        private static IBValue Parse(BinaryReader br)
        {
            char c = (char)br.PeekChar();

            if (char.IsDigit(c))
            {
                return new BString(ParseString(br));
            }
            else if (c == 'i')
            {
                return ParseInteger(br);
            }
            else if (c == 'l')
            {
                return ParseList(br);
            }
            else if (c == 'd')
            {
                return ParseDictionary(br);
            }

            throw new InvalidCharacterException(c, "a number, 'i', 'l' or 'd'");
        }

        /// <summary>
        /// Private method that parses and returns the string in the current position of the stream.
        /// </summary>
        /// <param name="br">The BinaryReader with the data to parse.</param>
        /// <returns>The string parsed.</returns>
        /// <remarks>If the current position does not contain data similar to "6:string", which is a bencoded string, an exception will be thrown.</remarks>
        private static string ParseString(BinaryReader br)
        {
            StringBuilder buffer = new StringBuilder(10);

            while (true)
            {
                char c = (char)br.ReadByte();

                if (char.IsDigit(c))
                {
                    if (buffer.Length > 10)
                        throw new InvalidLengthException(buffer.ToString()); //2147483647

                    buffer.Append(c);
                }
                else if (c == ':')
                {
                    if (buffer.Length == 0)
                        throw new InvalidCharacterException(c, "a number"); //this exception is thrown if the first character is not a number, which is true if the buffer's length is 0

                    int length;
                    if (!int.TryParse(buffer.ToString(), NumberStyles.None, CultureInfo.InvariantCulture, out length))
                        throw new InvalidLengthException(buffer.ToString());

                    buffer = new StringBuilder(length);
                    for (int i = 0; i < length; i++)
                    {
                        buffer.Append((char)br.ReadByte());
                    }

                    return buffer.ToString();
                }
                else
                {
                    throw new InvalidCharacterException(c, "a number or ':'");
                }
            }
        }

        /// <summary>
        /// Private method that parses and returns the integer number in the current position of the stream.
        /// </summary>
        /// <param name="br">The BinaryReader with the data to parse.</param>
        /// <returns>The integer number parsed.</returns>
        /// <remarks>If the current position does not contain data similar to "i123e" or "i-123e", which is a bencoded integer number, an exception will be thrown.</remarks>
        private static BInteger ParseInteger(BinaryReader br)
        {
            StringBuilder buffer = new StringBuilder(30);

            char c = (char)br.ReadByte();
            if (c != 'i')
                throw new InvalidCharacterException(c, "'i'");

            while (true)
            {
                c = (char)br.ReadByte();
                if (char.IsDigit(c))
                {
                    if (buffer.Length > 30)
                        throw new InvalidLengthException(buffer.ToString()); //-79228162514264337593543950335

                    buffer.Append(c);
                }
                else if (c == '-')
                {
                    if (buffer.Length > 0)
                        throw new InvalidCharacterException(c, "a number or 'e'"); //the minus sign can only be appended if it's the first char, for negative numbers...

                    buffer.Append(c);
                }
                else if (c == 'e')
                {
                    decimal integer;
                    if (!decimal.TryParse(buffer.ToString(), NumberStyles.None, CultureInfo.InvariantCulture, out integer))
                        throw new InvalidIntegerException(buffer.ToString());

                    return new BInteger(integer);
                }
                else
                {
                    if (buffer.Length == 0)
                        throw new InvalidCharacterException(c, "a number, '-' or 'e'");
                    else
                        throw new InvalidCharacterException(c, "a number or 'e'"); //if buffer contains any char, it means that the minus char cannot be appended anymore
                }
            }
        }

        /// <summary>
        /// Private method that parses and returns the list in the current position of the stream.
        /// </summary>
        /// <param name="br">The BinaryReader with the data to parse.</param>
        /// <returns>The list parsed.</returns>
        /// <remarks>If the current position does not contain data similar to "lvaluese", which is a bencoded list, an exception will be thrown.</remarks>
        private static BList ParseList(BinaryReader br)
        {
            StringBuilder buffer = new StringBuilder(1024);

            char c = (char)br.ReadByte();
            if (c != 'l')
                throw new InvalidCharacterException(c, "'l'");

            BList list = new BList();

            do
            {
                list.Add(Parse(br));
            } while ((char)br.PeekChar() != 'e');

            br.ReadByte(); //skips the 'e' char

            return list;
        }

        /// <summary>
        /// Private method that parses and returns the dictionary in the current position of the stream.
        /// </summary>
        /// <param name="br">The BinaryReader with the data to parse.</param>
        /// <returns>The dictionary parsed.</returns>
        /// <remarks>If the current position does not contain data similar to "dBencoded_stringBencoded_elemente", which is a bencoded dictionary, an exception will be thrown.</remarks>
        private static BDictionary ParseDictionary(BinaryReader br)
        {
            StringBuilder buffer = new StringBuilder(1024);

            char c = (char)br.ReadByte();
            if (c != 'd')
                throw new InvalidCharacterException(c, "'d'");

            BDictionary dictionary = new BDictionary();

            do
            {
                string key = ParseString(br);
                IBValue value = Parse(br);

                dictionary.Add(key, value);
            } while ((char)br.PeekChar() != 'e');

            br.ReadByte(); //skips the 'e' char

            return dictionary;
        }

        /// <summary>
        /// Serializes an IBvalue to the provided stream.
        /// </summary>
        /// <param name="value">The IBValue to be serialized.</param>
        /// <param name="stream">The Stream to receive the serialized data.</param>
        public static void Save(IBValue value, Stream stream)
        {
            BinaryWriter bw = new BinaryWriter(stream);

            foreach (char c in value.ToBEncodedString())
            {
                bw.Write((byte)c);
            }
        }

        /// <summary>
        /// Serializes an IBValue to the provided file path.
        /// </summary>
        /// <param name="value">The IBValue to be serialized.</param>
        /// <param name="path">The destination path.</param>
        public static void Save(IBValue value, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                Save(value, fs);
                fs.Close();
            }
        }

        /// <summary>
        /// Saves a BDicionary to the provided Stream.
        /// </summary>
        /// <param name="value">The BDicionary to ba saved.</param>
        /// <param name="stream">The Stream to receive the saved data.</param>
        /// <remarks>This method is the same as the method Save, but receives a BDicionary as the value parameter because a torrent file is bencoded dictionary.</remarks>
        public static void SaveTorrent(BDictionary value, Stream stream)
        {
            Save(value, stream);
        }

        /// <summary>
        /// Saves a BDicionary to the provided file path.
        /// </summary>
        /// <param name="value">The BDicionary to ba saved.</param>
        /// <param name="path">The destination path.</param>
        /// <remarks>This method is the same as the method Save, but receives a BDicionary as the value parameter because a torrent file is bencoded dictionary.</remarks>
        public static void SaveTorrent(BDictionary value, string path)
        {
            Save(value, path);
        }

        /// <summary>
        /// Computes the "info_hash" of the provided torrent (BDictionary).
        /// </summary>
        /// <param name="torrent">The BDicionary containing the torrent file.</param>
        /// <returns>An InfoHash object with the SHA1 hash.</returns>
        public static InfoHash ComputeInfoHash(BDictionary torrent)
        {
            IBValue info = null;

            //looks for the "info" dictionary
            foreach (KeyValuePair<string, IBValue> item in torrent.Items)
            {
                if (item.Key == "info" && item.Value is BDictionary)
                {
                    info = item.Value;
                    break;
                }
            }

            //if found, then computes the SHA1 hash and returns it
            if (info != null)
            {
                //the info_hash is the sha1 hash of the bencoded "info" dictionary, so gets it
                string bencoded = info.ToBEncodedString();
                List<byte> bytes = new List<byte>(bencoded.Length);

                //adds its bytes to a list to be used in the ComputeHash function
                foreach (char c in info.ToBEncodedString())
                {
                    bytes.Add((byte)c);
                }

                SHA1 sha1 = SHA1.Create();
                return InfoHash.FromByteArray(sha1.ComputeHash(bytes.ToArray()));
            }

            //if the "info" dictionary is not found, then returns an empty InfoHash to avoid exceptions...
            return InfoHash.FromByteArray(new byte[] { });
        }
    }
}
