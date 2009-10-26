using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace DeleteEmptyFolders
{
    class Program
    {
        static void Main(string[] args)
        {
            string target = @".\";
            if (args.Length > 0)
            {
                target = args[0];
            }
            Console.WriteLine("Checking " + target + " for empty folders to be deleted");
            string[] folders = FileStuff.GetFolders(target);
            for (int i = 0; i < folders.Length; ++i)
            {
                DirectoryInfo dir = new DirectoryInfo(folders[i]);
                if (dir.GetFiles().Length == 0)
                {
                    Console.WriteLine("Now Deleting: " + folders[i]);
                    try
                    {
                        Directory.Delete(folders[i]);
                    }
                    catch
                    {
                        Console.WriteLine("Deletion failed.");
                    }
                }
            }
        }
    }
}
