using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Data.SqlClient;
using System.Web.Mail;
using System.IO;
using System.Xml;
using System.Timers;

namespace Folderwatcher
{
    public class FolderWatcher : System.ServiceProcess.ServiceBase
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        FileSystemWatcher watcher1 = new FileSystemWatcher();
        FileSystemWatcher watcher2 = new FileSystemWatcher();
        Timer timer1 = new Timer();
        Timer timer2 = new Timer();
		public FolderWatcher()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
		}

		// The main entry point for the process
		static void Main()
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;
	
			// More than one user Service may run within the same process. To add
			// another service to this process, change the following line to
			// create a second service object. For example,
			//
			//   ServicesToRun = new System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
			//
            ServicesToRun = new System.ServiceProcess.ServiceBase[] { new FolderWatcher() };

			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// Service1
			// 
            this.ServiceName = "FolderWatcher";

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
            string currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
			// TODO: Add code here to start your service.
            //ADD MY CODE HERE
            string xmlFile = currentPath+@"\settings.xml";
            if (args.Length != 0)
            {
                xmlFile = args[0];
            }
            XmlReader xml = XmlReader.Create(xmlFile);
            xml.ReadToFollowing("folder1");
            string folder1 = xml.ReadElementContentAsString("folder1", "");
            xml.ReadToFollowing("program1");
            string program1 = xml.ReadElementContentAsString("program1", "");
            xml.ReadToFollowing("folder2");
            string folder2 = xml.ReadElementContentAsString("folder2", "");
            xml.ReadToFollowing("program2");
            string program2 = xml.ReadElementContentAsString("program2", "");
            xml.ReadToFollowing("timer1");
            int timer1sec = Convert.ToInt32(xml.ReadElementContentAsString("timer1", ""));
            xml.ReadToFollowing("timer2");
            int timer2sec = Convert.ToInt32(xml.ReadElementContentAsString("timer2", ""));
            EventLog.WriteEntry("FolderWatcher has been started. Executable running from " + currentPath + ". Watcher 1 folder is: " + folder1 + ", which will execute " + program1 + ". Watcher 2 folder is: " + folder2 + ", which will execute " + program2);
            timer1.Elapsed += new ElapsedEventHandler(Timer1Tick);
            timer2.Elapsed += new ElapsedEventHandler(Timer2Tick);
            timer1.Interval = timer1sec * 1000;
            timer2.Interval = timer2sec * 1000;
            watcher1.Path = Path.GetDirectoryName(folder1);
            watcher1.Filter = "*.*";
            watcher1.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.DirectoryName;
            watcher1.Created += new FileSystemEventHandler(Go1);
            watcher1.EnableRaisingEvents = true;
            watcher2.Path = Path.GetDirectoryName(folder2);
            watcher2.Filter = "*.*";
            watcher2.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.DirectoryName;
            watcher2.Created += new FileSystemEventHandler(Go2);
            watcher2.EnableRaisingEvents = true;
            Console.ReadLine();
            //END MY CODE	
		}
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			// TODO: Add code here to perform any tear-down necessary to stop your service.
		}
        public void Go1(object source, FileSystemEventArgs e)
        {
            string currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            // TODO: Add code here to start your service.
            //ADD MY CODE HERE
            string xmlFile = currentPath + @"\settings.xml";
            XmlReader xml = XmlReader.Create(xmlFile);
            xml.ReadToFollowing("folder1");
            string folder1 = xml.ReadElementContentAsString("folder1", "");
            xml.ReadToFollowing("program1");
            string program1 = xml.ReadElementContentAsString("program1", "");
            xml.ReadToFollowing("folder2");
            string folder2 = xml.ReadElementContentAsString("folder2", "");
            xml.ReadToFollowing("program2");
            string program2 = xml.ReadElementContentAsString("program2", "");
            xml.ReadToFollowing("timer1");
            string timer1seconds = xml.ReadElementContentAsString("timer1", "");
            xml.ReadToFollowing("timer2");
            string timer2seconds = xml.ReadElementContentAsString("timer2", "");
            string progFolder = program1.Substring(0, program1.LastIndexOf('\\')+1);
            progFolder = string.Format(progFolder);
            string progFile = program1.Substring(program1.LastIndexOf('\\')+1);
            EventLog.WriteEntry("Activity seen in " + folder1 + ", running " + program1);
            Process p = new Process();
            p.StartInfo.WorkingDirectory = progFolder;
            p.StartInfo.FileName = progFile;
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            watcher1.EnableRaisingEvents = false;
            EventLog.WriteEntry("Starting " + timer1seconds + "-second clock on Watcher 1");
            timer1.Enabled = true;
         }

        public void Go2(object source, FileSystemEventArgs e)
        {
            string currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            // TODO: Add code here to start your service.
            //ADD MY CODE HERE
            string xmlFile = currentPath + @"\settings.xml";
            XmlReader xml = XmlReader.Create(xmlFile);
            xml.ReadToFollowing("folder1");
            string folder1 = xml.ReadElementContentAsString("folder1", "");
            xml.ReadToFollowing("program1");
            string program1 = xml.ReadElementContentAsString("program1", "");
            xml.ReadToFollowing("folder2");
            string folder2 = xml.ReadElementContentAsString("folder2", "");
            xml.ReadToFollowing("program2");
            string program2 = xml.ReadElementContentAsString("program2", "");
            xml.ReadToFollowing("timer1");
            string timer1seconds = xml.ReadElementContentAsString("timer1", "");
            xml.ReadToFollowing("timer2");
            string timer2seconds = xml.ReadElementContentAsString("timer2", "");
            EventLog.WriteEntry("Activity seen in " + folder2+", running "+program2);
            string progFolder = program2.Substring(0, program2.LastIndexOf('\\') + 1);
            progFolder = string.Format(progFolder);
            string progFile = program2.Substring(program2.LastIndexOf('\\') + 1);
            Process p = new Process();
            p.StartInfo.WorkingDirectory = progFolder;
            p.StartInfo.FileName = progFile;
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            watcher2.EnableRaisingEvents = false;
            EventLog.WriteEntry("Starting "+ timer2seconds+"-second clock on Watcher 2");
            timer2.Enabled = true;
        }
        	void Timer1Tick(object sender, EventArgs e)
	        {
                timer1.Stop();
                EventLog.WriteEntry("Timer1 has expired");
                watcher1.EnableRaisingEvents = true;
	        }
            void Timer2Tick(object sender, EventArgs e)
	        {
                timer2.Stop();
                EventLog.WriteEntry("Timer2 has expired");
                watcher2.EnableRaisingEvents = true;
	        }
        }
}
