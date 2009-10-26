using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace uTorrentTalker
{
    class uTorrentTalker
    {
        public static string[][] GetTorrents(string address, string username, string password)
        {
            WebClient wc = new WebClient();
            wc.Credentials = new NetworkCredential(username, password);
            string fullAddress = "http://" + address + "/gui/?list=1";
            //Fetch a raw listing of the current stuff from uTorrent==============================================
            byte[] rawResponse = wc.DownloadData(fullAddress);
            string response = Encoding.ASCII.GetString(rawResponse);
            Regex arrayBuilder = new Regex("\n");
            char[] delimiters = new char[] { '"', '[', ']' };
            int start = response.IndexOf('"' + "torrents" + '"' + ": [") + 16;
            int end = response.Length - (start + 29);
            response = response.Substring(start, end);
            //Clean the list text so its just the torrents=======================================================
            string[] rawTorrents = arrayBuilder.Split(response);
            string[][] torrents = new string[rawTorrents.Count()][];
            if (rawTorrents.Count() > 0)                //check for active torrents
            {
                for (int i = 0; i < rawTorrents.Count(); ++i)
                {
                    string rawTorrent = rawTorrents[i];
                    string[] tempTorrent = rawTorrent.Split(new Char[] { ',' });
                    //now we fill the array torrents with: 0 = Hash, 1 = Status, 2 = Label, and 3 = Queue
                    torrents[i] = new string[] { tempTorrent[0].ToString().Trim(delimiters), tempTorrent[1].ToString().Trim(delimiters), tempTorrent[11].ToString().Trim(delimiters), tempTorrent[17].ToString().Trim(delimiters), tempTorrent[2].ToString().Trim(delimiters) };
                }
            }
            return torrents;
        }
        public static void RemoveTorrent(string address, string username, string password, string hash)
        {
            WebClient wc = new WebClient();
            wc.Credentials = new NetworkCredential(username, password);
            string fullAddress = "http://" + address + "/gui/?action=remove&hash=" + hash;
            wc.DownloadData(fullAddress);
        }

        public static void LabelTorrent(string address, string username, string password, string hash, string label)
        {
            WebClient wc = new WebClient();
            wc.Credentials = new NetworkCredential(username, password);
            string fullAddress = "http://" + address + "/gui/?action=setprops&hash=" + hash + "&s=label&v="+label;
            wc.DownloadData(fullAddress);
        }
    }
}
