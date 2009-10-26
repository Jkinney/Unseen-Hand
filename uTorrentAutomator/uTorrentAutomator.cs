using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Specialized;
using DotNetTorrent;
using DotNetTorrent.BEncoding;
using uTorrentTalker;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string address = "127.0.0.1:5000";
            string userName = "admin";
            string password = "password";
            string label = "";
            string torrentFile = "";
            if (args.Count() != 0)
            {
                try
                {
                    address = args[0];
                    userName = args[1];
                    password = args[2];
                    if (args.Count() == 5)
                    {
                        label = args[3];
                        torrentFile = args[4];
                    }
                }
                catch
                {
                }

            }
            Regex arrayBuilder = new Regex("\n");
            char[] delimiters = new char[] { '"', '[', ']' };
            //Removal Mode
            if (label == "" && torrentFile == "")
            {
                try
                {
                    string[][] torrents = uTorrentTalker.uTorrentTalker.GetTorrents(address, userName, password);
                    //0 = Hash, 1 = Status, 2 = Label, 3 = Queue, 4 = Name
                    for (int i = 0; i < torrents.Count(); ++i)
                    {
                        if (torrents[i][1] == "136")
                        {
                            uTorrentTalker.uTorrentTalker.RemoveTorrent(address, userName, password, torrents[i][0]);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Something went wrong");
                }
            }
            //Add Mode
            if (label != "" && torrentFile != "")
            {
                //===========================================================================================
                Stream TorrentStream = File.OpenRead(torrentFile);
                CredentialCache uTorrentCredentials = new CredentialCache();
                string addAddress = "http://" + address + "/gui/";
                uTorrentCredentials.Add(new Uri(addAddress), "Basic", new NetworkCredential(userName, password));
                HttpWebRequest PostFileRequest = (HttpWebRequest)(HttpWebRequest.Create(String.Format("{0}?action=add-file", addAddress)));
                PostFileRequest.KeepAlive = false;
                PostFileRequest.Credentials = uTorrentCredentials;
                string BoundaryString = Guid.NewGuid().ToString("N");
                PostFileRequest.ContentType = "multipart/form-data; boundary=" + BoundaryString;
                PostFileRequest.Method = "POST";
                using (BinaryWriter Writer = new BinaryWriter(PostFileRequest.GetRequestStream()))
                {
                    byte[] FileBytes = new byte[TorrentStream.Length];
                    TorrentStream.Read(FileBytes, 0, FileBytes.Length);
                    Writer.Write(Encoding.ASCII.GetBytes(String.Format("--{0}\r\nContent-Disposition: form-data; name=\"torrent_file\"; filename=\"{0}\"\r\nContent-Type: application/x-bittorrent\r\n\r\n", BoundaryString, torrentFile)));
                    Writer.Write(FileBytes, 0, FileBytes.Length);
                    Writer.Write(Encoding.ASCII.GetBytes(String.Format("\r\n--{0}--\r\n", BoundaryString)));
                }
                HttpWebResponse Response = (HttpWebResponse)PostFileRequest.GetResponse();
                Response.Close();
                BDictionary dictionary = DotNetTorrent.BEncoding.Torrent.ParseTorrentFile(torrentFile);
                DotNetTorrent.BEncoding.Torrent.InfoHash newTorrentHashObject = DotNetTorrent.BEncoding.Torrent.ComputeInfoHash(dictionary);
                string newTorrentHash = newTorrentHashObject.ToString();
                uTorrentTalker.uTorrentTalker.LabelTorrent(address, userName, password, newTorrentHash, label);
            }
        }
    }
}