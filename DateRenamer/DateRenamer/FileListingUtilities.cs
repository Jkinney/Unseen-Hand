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

public class FileStuff
{
    public static Stack GetFilesAndFolders(string path, Stack filelist)
    {
        DirectoryInfo root = new DirectoryInfo(path);
        DirectoryInfo[] folders = root.GetDirectories("*");
        foreach (DirectoryInfo d in folders)
        {
             filelist = GetFilesAndFolders(d.FullName.ToString(), filelist);
        }
        FileInfo[] files = root.GetFiles("*.*");
        foreach (FileInfo f in files)
        {
            filelist.Push(f.FullName.ToString());
        }
        return filelist;
    }
        public static string[] GetFilesAndFolders(string path)
        {
            Stack filesTemp = new Stack();
            filesTemp = FileStuff.GetFilesAndFolders(path, filesTemp);
            int fileCount = filesTemp.Count;
            string[] files = new string[fileCount];
            for (int i = 0; i < fileCount; i++)
            {
                files[i] = filesTemp.Pop().ToString();
            }
            return files;
        }
        public static string[] SplitFilename(string filename)
        {
            string[] pieces = new string[3];
            //containing folder
            int lastSlash = filename.LastIndexOf('\\');
            int lastDot = filename.LastIndexOf('.');
            pieces[0] = filename.Substring(0, lastSlash + 1);
            //no extension
            int finalPart = lastDot - lastSlash - 1;
            pieces[1] = filename.Substring(lastSlash+1,finalPart);
            //Just extension
            pieces[2] = filename.Substring(lastDot);
            return pieces;
        }
        public static string CleanFilename(string filename)
        {
            filename = filename.Replace(".", " ");
            filename = filename.Replace("{", " ");
            filename = filename.Replace("}", " ");
            filename = filename.Replace("[", " ");
            filename = filename.Replace("]", " ");
            filename = filename.Replace("-", " ");
            filename = filename.Replace("_", " ");
            filename = filename.Replace("   ", " ");
            filename = filename.Replace("  ", " ");
            return filename;
        }
        public static string FormatFileSize(int byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824)
                size = String.Format("{0:##.##}", byteCount / 1073741824) + " GB";
            else if (byteCount >= 1048576)
            {
                size = String.Format("{0:##.##}", byteCount / 1048576) + " MB";
            }
            else if (byteCount >= 1024)
            {
                size = String.Format("{0:##.##}", byteCount / 1024) + " KB";
            }
            else
            {
                size = String.Format("{0:##.##}", byteCount ) + " Bytes";
            }
                return size;
        }
        public static string SafeFileWrite(string inputFilename, string FileExtension)
        {
            string testFilename = inputFilename;
            string finalFilename = "";
            if (File.Exists(testFilename))
            {
                testFilename = testFilename.Substring(0, testFilename.Length - FileExtension.Length);
                testFilename = testFilename + "#2" + FileExtension;
                testFilename = SafeFileWrite(testFilename, FileExtension);
            }
            if (!File.Exists(testFilename))
            {
                finalFilename = testFilename;
            }
            return finalFilename;
        }
}

public class StringEx
{
    public static string ProperCase(string s)
    {
        s = s.ToLower();//lowercase the entire input string
        string sProper = ""; //instantiate the destination variable
        char[] seps = new char[] { ' ' }; //establish single space as the seperator
        string [] ss = s.Split(seps);
        for (int i = 0; i < ss.Length; ++i )
        {
            string word = ss[i];
            word = word.Substring(0, 1);
            word = word.ToUpper() + ss[i].Substring(1, ss[i].Length - 1);
            sProper = sProper + " " + word;
        }
        sProper = sProper.TrimEnd();
        return sProper;
    }
}