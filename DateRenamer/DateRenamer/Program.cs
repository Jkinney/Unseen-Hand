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
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Web;
using System.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Security.Cryptography;

public class MainClass
{
    public static void Main(string[] args)
    {
        string APIKEY = "FD84D5DAB5547298";
        string source = "";
        string destRegular = "";
        string destDaily = "";
        bool matched = false;
           if (args.Length != 0)
        {
            source = args[0];
            try
            {
                destRegular = args[1];
            }
            catch
            {
                destRegular = "";
            }
            try
            {
                destDaily = args[2];
            }
            catch
            {
                destDaily = "";
            }
        }
        if (source == "")
        {
            source = Directory.GetCurrentDirectory().ToString();
        }
        if (destRegular != "")
        {
            if (destRegular.Substring(destRegular.Length - 1) != "\\")
            {
                destRegular = destRegular + "\\";
            }
        }
        if (destDaily != "")
        {
            if (destDaily.Substring(destDaily.Length - 1) != "\\")
            {
                destDaily = destDaily + "\\";
            }
        }
        Console.WriteLine("Renamer launched!");
        Console.WriteLine("Source directory is: " + source);
        Console.WriteLine("Normal output directory is: " + destRegular);
        Console.WriteLine("Daily show output directory is: " + destDaily);
        string RSSoutputFile = @".\MyTorrents.xml";
        string date = DateTime.Now.ToString("ddd") + ", " + DateTime.Now.Day + " " + DateTime.Now.ToString("MMM") + " " + DateTime.Now.ToString("yyyy") + " " + DateTime.Now.ToString("HH") + ':' + DateTime.Now.ToString("mm") + ':' + DateTime.Now.ToString("ss") + " GMT";
        if (!File.Exists(RSSoutputFile))
        {
            FileStream fileStream = new FileStream(RSSoutputFile, FileMode.Create);
            StreamWriter textWriter = new StreamWriter(fileStream);
            textWriter.WriteLine("<?xml version=\"1.0\"?>");
            textWriter.WriteLine("<rss version=\"2.0\" xmlns:atom=\"http://www.w3.org/2005/Atom\">");
            textWriter.WriteLine("<channel>");
            textWriter.WriteLine("<title>My Automated Torrents</title>");
            textWriter.WriteLine("<link>http://www.thetvdb.com</link>");
            textWriter.WriteLine("<atom:link href="+'"'+"http://appliedcuriosity.cc/RSS/MyTorrents.xml"+'"'+" rel="+'"'+"self"+'"'+" type="+'"'+"application/rss+xml"+'"'+" />");
            textWriter.WriteLine("<description>What have you been downloading today? ;)</description>");
            textWriter.WriteLine("<pubDate>" + date + "</pubDate>");
            textWriter.WriteLine("<lastBuildDate>" + date + "</lastBuildDate>");
            textWriter.WriteLine("<language>en-us</language>");
            textWriter.WriteLine("</channel>");
            textWriter.WriteLine("</rss>");
            textWriter.Close();
        }
           Hashtable preCachedIDs = new Hashtable();
           preCachedIDs.Add("13 Fear Is Real", "84468");
           preCachedIDs.Add("24", "76290");
           preCachedIDs.Add("30 rock", "79488");
           preCachedIDs.Add("90210", "82716");
           preCachedIDs.Add("According to Jim", "75926");
           preCachedIDs.Add("Age of Love", "80279");
           preCachedIDs.Add("The Amazing Race", "77666");
           preCachedIDs.Add("America's Got Talent", "79490");
           preCachedIDs.Add("America's Most Wanted", "74943");
           preCachedIDs.Add("America's Next Top Model", "71721");
           preCachedIDs.Add("American Chopper", "73104");
           preCachedIDs.Add("American Dad!", "73141");
           preCachedIDs.Add("American Gladiators", "71483");
           preCachedIDs.Add("American Idol", "70814");
           preCachedIDs.Add("American Inventor", "79288");
           preCachedIDs.Add("Anthony Bourdain No Reservations", "79668");
           preCachedIDs.Add("The Armstrong and Miller Show", "76916");
           preCachedIDs.Add("Armstrong and Miller Show", "76916");
           preCachedIDs.Add("Armstrong and Miller", "76916");
           preCachedIDs.Add("As The World Turns", "71984");
           preCachedIDs.Add("Ashes to Ashes", "81253");
           preCachedIDs.Add("Avatar - The Last Airbender", "74852");
           preCachedIDs.Add("Avatar The Last Airbender", "74852");
           preCachedIDs.Add("The Bachelor", "70869");
           preCachedIDs.Add("Batman The Brave and the Bold", "82824");
           preCachedIDs.Add("Batman Brave and the Bold", "82824");
           preCachedIDs.Add("Battlestar Galactica", "71173");
           preCachedIDs.Add("The Beast", "78266");
           preCachedIDs.Add("Ben 10 Alien Force", "81841");
           preCachedIDs.Add("The Big Bang Theory", "80379");
           preCachedIDs.Add("Big Brother", "76706");
           preCachedIDs.Add("Big Love", "74156");
           preCachedIDs.Add("Big Shots", "80570");
           preCachedIDs.Add("The Biggest Loser", "75166");
           preCachedIDs.Add("Bones", "75682");
           preCachedIDs.Add("The Border", "81114");
           preCachedIDs.Add("Boston Legal", "74058");
           preCachedIDs.Add("Breaking Bad", "81189");
           preCachedIDs.Add("Brothers & Sisters", "71477");
           preCachedIDs.Add("Burn Notice", "80270");
           preCachedIDs.Add("The Business", "79375");
           preCachedIDs.Add("Californication", "80349");
           preCachedIDs.Add("Castle", "82607");
           preCachedIDs.Add("Charlie Brooker's Screenwipe", "79818");
           preCachedIDs.Add("Chasers war on everything", "79706");
           preCachedIDs.Add("Chuck", "80348");
           preCachedIDs.Add("Cities Of The Underworld", "80373");
           preCachedIDs.Add("The Cleaner", "82492");
           preCachedIDs.Add("The Closer", "74875");
           preCachedIDs.Add("Colbert Report", "79274");
           preCachedIDs.Add("Cold Case", "72167");
           preCachedIDs.Add("Cops", "74946");
           preCachedIDs.Add("Criminal Minds", "75710");
           preCachedIDs.Add("Criss Angel: Mindfreak", "78841");
           preCachedIDs.Add("Crusoe", "83226");
           preCachedIDs.Add("CSI", "83898");
           preCachedIDs.Add("CSI Miami", "78310");
           preCachedIDs.Add("CSI NY", "73696");
           preCachedIDs.Add("Cupid", "76612");
           preCachedIDs.Add("Curb Your Enthusiasm", "76203");
           preCachedIDs.Add("The Daily Show", "71256");
           preCachedIDs.Add("Damages", "80367");
           preCachedIDs.Add("Dancing With The Stars US", "79590");
           preCachedIDs.Add("Dancing With The Stars", "79590");
           preCachedIDs.Add("DEA", "76722");
           preCachedIDs.Add("Demons", "84347");
           preCachedIDs.Add("Desperate Housewives", "73800");
           preCachedIDs.Add("Destination Truth", "80251");
           preCachedIDs.Add("Dexter", "79349");
           preCachedIDs.Add("Dirt", "79679");
           preCachedIDs.Add("Dirty Jobs", "78904");
           preCachedIDs.Add("Dirty Sexy Money", "80593");
           preCachedIDs.Add("Do Not Disturb", "82304");
           preCachedIDs.Add("Doctor Who", "76107");
           preCachedIDs.Add("Doctor Who Confidential", "79298");
           preCachedIDs.Add("Dollhouse", "82046");
           preCachedIDs.Add("Eleventh Hour US", "83066");
           preCachedIDs.Add("Eleventh Hour", "83066");
           preCachedIDs.Add("Eli Stone", "81025");
           preCachedIDs.Add("Entourage", "74543");
           preCachedIDs.Add("ER", "70761");
           preCachedIDs.Add("Eureka", "79334");
           preCachedIDs.Add("Everybody Hates Chris", "75914");
           preCachedIDs.Add("Extras", "75660");
           preCachedIDs.Add("Extreme Engineering", "72702");
           preCachedIDs.Add("Family Guy", "75978");
           preCachedIDs.Add("Fear Itself", "82120");
           preCachedIDs.Add("Feasting on Asphalt", "79393");
           preCachedIDs.Add("Fifth Gear", "78998");
           preCachedIDs.Add("The Fixer", "81499");
           preCachedIDs.Add("Flashpoint", "82438");
           preCachedIDs.Add("Flight of the Conchords", "80252");
           preCachedIDs.Add("Frank TV", "80873");
           preCachedIDs.Add("Free Radio", "81558");
           preCachedIDs.Add("Friday Night Lights", "79337");
           preCachedIDs.Add("Fringe", "82066");
           preCachedIDs.Add("Frisky Dingo", "79930");
           preCachedIDs.Add("The Gadget Show", "80747");
           preCachedIDs.Add("Gary Unmarried", "82916");
           preCachedIDs.Add("Generation Kill", "82109");
           preCachedIDs.Add("Ghost Hunters", "77949");
           preCachedIDs.Add("Ghost Whisperer", "78817");
           preCachedIDs.Add("Good Eats", "73067");
           preCachedIDs.Add("Gossip Girl", "80547");
           preCachedIDs.Add("Greek", "80301");
           preCachedIDs.Add("Greys Anatomy", "73762");
           preCachedIDs.Add("Hannah Montana", "79317");
           preCachedIDs.Add("Harper's Island", "84084");
           preCachedIDs.Add("Heartland", "70598");
           preCachedIDs.Add("Hell's Kitchen", "74897");
           preCachedIDs.Add("Heroes", "79501");
           preCachedIDs.Add("The Hollowmen", "82460");
           preCachedIDs.Add("House", "73255");
           preCachedIDs.Add("How I Met Your Mother", "75760");
           preCachedIDs.Add("Howie Do It", "84550");
           preCachedIDs.Add("Human Weapon", "80353");
           preCachedIDs.Add("Hurl", "82500");
           preCachedIDs.Add("Hustle", "73028");
           preCachedIDs.Add("Hyperdrive", "79182");
           preCachedIDs.Add("I Hate My 30's", "80413");
           preCachedIDs.Add("I Survived a Japanese Game Show", "82228");
           preCachedIDs.Add("In Plain Sight", "82155");
           preCachedIDs.Add("In Treatment", "81248");
           preCachedIDs.Add("Inked", "78805");
           preCachedIDs.Add("The IT Crowd", "79216");
           preCachedIDs.Add("It's Always Sunny in Philadelphia", "75805");
           preCachedIDs.Add("Jon and Kate Plus 8", "100611");
           preCachedIDs.Add("Kenny vs. Spenny", "73062");
           preCachedIDs.Add("King of the Hill", "73903");
           preCachedIDs.Add("Kings", "84068");
           preCachedIDs.Add("Kitchen Nightmares", "80552");
           preCachedIDs.Add("Knight Rider 2008", "81318");
           preCachedIDs.Add("Kyle XY", "76143");
           preCachedIDs.Add("The L Word", "72477");
           preCachedIDs.Add("LA Ink", "80469");
           preCachedIDs.Add("Last Comic Standing", "72673");
           preCachedIDs.Add("Last of The Summer Wine", "77945");
           preCachedIDs.Add("Late Night with Conan O'Brien", "76737");
           preCachedIDs.Add("The Late Late Show with Craig Ferguson", "73387");
           preCachedIDs.Add("Law And Order", "91651");
           preCachedIDs.Add("Law & Order", "91651");
           preCachedIDs.Add("Law and Order Criminal Intent", "71489");
           preCachedIDs.Add("Law & Order Criminal Intent", "71489");
           preCachedIDs.Add("Law and Order Special Victims Unit", "75692");
           preCachedIDs.Add("Law & Order Special Victims Unit", "75692");
           preCachedIDs.Add("Legend of the Seeker", "82672");
           preCachedIDs.Add("Leverage", "82339");
           preCachedIDs.Add("Lie To Me", "83602");
           preCachedIDs.Add("Life", "80352");
           preCachedIDs.Add("Life On Mars", "79177");
           preCachedIDs.Add("Life On Mars US", "82289");
           preCachedIDs.Add("Life With Derek", "86611");
           preCachedIDs.Add("Lipstick Jungle", "80392");
           preCachedIDs.Add("Little Britain USA", "83232");
           preCachedIDs.Add("Little Mosque on the Prairie", "79701");
           preCachedIDs.Add("Little People, Big World", "80112");
           preCachedIDs.Add("Lost", "73739");
           preCachedIDs.Add("The Loop", "75275");
           preCachedIDs.Add("Lucy, The Daughter of the Devil ", "70695");
           preCachedIDs.Add("Mad Men", "80337");
           preCachedIDs.Add("Man vs. Wild", "79545");
           preCachedIDs.Add("McLeod's Daughters", "76532");
           preCachedIDs.Add("Medium", "73265");
           preCachedIDs.Add("The Mentalist", "82459");
           preCachedIDs.Add("Merlin", "83269");
           preCachedIDs.Add("Metalocalypse", "79563");
           preCachedIDs.Add("Miami Ink", "78816");
           preCachedIDs.Add("Mind of mencia", "78958");
           preCachedIDs.Add("The Middleman", "82230");
           preCachedIDs.Add("Monk", "78490");
           preCachedIDs.Add("My Boys", "79600");
           preCachedIDs.Add("My Family", "74438");
           preCachedIDs.Add("My Name is Earl", "75397");
           preCachedIDs.Add("My Own Worst Enemy", "82122");
           preCachedIDs.Add("Mythbusters", "73388");
           preCachedIDs.Add("Never Mind the Buzzcocks", "78051");
           preCachedIDs.Add("The New Adventures of Old Christine", "75756");
           preCachedIDs.Add("NCIS", "72108");
           preCachedIDs.Add("Nip Tuck", "72201");
           preCachedIDs.Add("NipTuck", "72201");
           preCachedIDs.Add("No Heroics", "83108");
           preCachedIDs.Add("Numb3rs", "73918");
           preCachedIDs.Add("The Office USA", "73244");
           preCachedIDs.Add("The Office (US)", "73244");
           preCachedIDs.Add("The Office", "73244");
           preCachedIDs.Add("One Tree Hill", "72158");
           preCachedIDs.Add("Paranormal State", "81408");
           preCachedIDs.Add("Penn and Teller Bullshit", "	72301");
           preCachedIDs.Add("Penn & Teller Bullshit", "	72301");
           preCachedIDs.Add("Penn and Teller's Bullshit", "	72301");
           preCachedIDs.Add("Penn & Teller's Bullshit", "	72301");
           preCachedIDs.Add("Peep Show", "71656");
           preCachedIDs.Add("The Pickup Artist", "80458");
           preCachedIDs.Add("Primeval", "79809");
           preCachedIDs.Add("Prison Break", "75340");
           preCachedIDs.Add("Private Practice", "80542");
           preCachedIDs.Add("Psych", "79335");
           preCachedIDs.Add("Pushing Daisies", "80351");
           preCachedIDs.Add("QI", "72716");
           preCachedIDs.Add("Quarterlife", "81424");
           preCachedIDs.Add("Raising The Bar", "82340");
           preCachedIDs.Add("Reno 911", "72336");
           preCachedIDs.Add("Rescue Me", "73741");
           preCachedIDs.Add("Real Time With Bill Maher", "72231");
           preCachedIDs.Add("Reaper", "80345");
           preCachedIDs.Add("The Riches", "79876");
           preCachedIDs.Add("Robin Hood", "79479");
           preCachedIDs.Add("Robot Chicken", "75734");
           preCachedIDs.Add("Rules of Engagement", "79842");
           preCachedIDs.Add("Rush", "82881");
           preCachedIDs.Add("Samantha Who?", "80680");
           preCachedIDs.Add("Sanctuary", "80159");
           preCachedIDs.Add("The Sarah Jane Adventures", "79708");
           preCachedIDs.Add("The Sarah Silverman Program", "79789");
           preCachedIDs.Add("Saturday Night Live", "76177");
           preCachedIDs.Add("Saving Grace", "80381");
           preCachedIDs.Add("Scrubs", "76156");
           preCachedIDs.Add("Sea Patrol", "80546");
           preCachedIDs.Add("Secret Diary of a Call Girl", "80621");
           preCachedIDs.Add("The Secret Life of the American Teenager", "82422");
           preCachedIDs.Add("The Shield", "78261");
           preCachedIDs.Add("The Simpsons", "71663");
           preCachedIDs.Add("Skins", "79773");
           preCachedIDs.Add("Smallville", "72218");
           preCachedIDs.Add("Smash Lab", "81047");
           preCachedIDs.Add("So You Think You Can Dance", "78956");
           preCachedIDs.Add("Sons of Anarchy", "82696");
           preCachedIDs.Add("Sordid Lives the Series", "84503");
           preCachedIDs.Add("The Soup", "82483");
           preCachedIDs.Add("South Park", "75897");
           preCachedIDs.Add("Spooks", "78890");
           preCachedIDs.Add("The Starter Wife", "80207");
           preCachedIDs.Add("Storm Chasers", "83498");
           preCachedIDs.Add("Stargate", "70852");
           preCachedIDs.Add("Stargate Atlantis", "70851");
           preCachedIDs.Add("Star Wars The Clone Wars", "83268");
           preCachedIDs.Add("The Suite Life of Zack and Cody", "79316");
           preCachedIDs.Add("Supernatural", "78901");
           preCachedIDs.Add("Survivor", "76733");
           preCachedIDs.Add("Tapout", "82979");
           preCachedIDs.Add("Tell Me You Love Me", "80462");
           preCachedIDs.Add("Terminator The Sarah Connor Chronicles", "80344");
           preCachedIDs.Add("Testees", "83339");
           preCachedIDs.Add("The City", "71402");
           preCachedIDs.Add("The Life and Times of Tim", "97391");
           preCachedIDs.Add("The Tonight Show with Jay Leno", "70336");
           preCachedIDs.Add("Til Death", "79384");
           preCachedIDs.Add("Top Chef", "79313");
           preCachedIDs.Add("Top Gear", "74608");
           preCachedIDs.Add("Top Gear Australia", "83234");
           preCachedIDs.Add("Torchwood", "79511");
           preCachedIDs.Add("True Blood", "82283");
           preCachedIDs.Add("Trust Me", "72560");
           preCachedIDs.Add("Two And A Half Men", "72227");
           preCachedIDs.Add("UFO Hunters", "81265");
           preCachedIDs.Add("Ugly Betty", "79352");
           preCachedIDs.Add("The Ultimate Fighter", "75382");
           preCachedIDs.Add("Unhitched", "81452");
           preCachedIDs.Add("The Unit", "75707");
           preCachedIDs.Add("The Universe", "80198");
           preCachedIDs.Add("The Unusuals", "83661");
           preCachedIDs.Add("Valentine", "83028");
           preCachedIDs.Add("The Venture Brothers", "72306");
           preCachedIDs.Add("Venture Brothers", "72306");
           preCachedIDs.Add("The Venture Bros", "72306");
           preCachedIDs.Add("Venture Bros", "72306");
           preCachedIDs.Add("The Venture Bros.", "72306");
           preCachedIDs.Add("Venture Bros.", "72306");
           preCachedIDs.Add("Without a Trace", "77963");
           preCachedIDs.Add("Weeds", "74845");
           preCachedIDs.Add("Wolverine and the X-Men", "82870");
           preCachedIDs.Add("Worst Week", "82895");
           preCachedIDs.Add("WWE", "70649");
//END SHOW CODES============================================================

        Console.WriteLine("Now scanning " + source + " for files.");
        string[] files = FileStuff.GetFilesAndFolders(source);

        //Begin the per-file processing loop here
        for (int i = 0; i < files.Length; ++i)
        {
            Console.WriteLine("Now examining " + files[i]);
            string[] filenameParts = FileStuff.SplitFilename(files[i]);
            string filename = FileStuff.CleanFilename(filenameParts[1]);
            //Setup individual show variables
            string showName = "";
            Boolean daily = false;
            string years = "";
            string months = "";
            string days = "";
            string episodeTitle = "";
            string season = "";
            string episodeNumber = "";
            string episodeID = "";
            string seasonID = "";
            string episodeSynopsis = "";
            string showID = "";
            string episodeThumbnail = "";
            //Daily Show 1
            string pattern = "20[0-9][0-9][ ][0-9]?[0-9][ ][0-9]?[0-9]";
            //Handle dated shows, year first format.
            if (!matched && Regex.IsMatch(filename, pattern))
            {
                matched = true;
                daily = true;
                Console.WriteLine("The file matches the "+'"'+"Year First"+'"'+" pattern");
                string[] pieces = Regex.Split(filename, pattern);
                showName = pieces[0].Trim();
                showName = StringEx.ProperCase(showName);
                years = filename.Substring(pieces[0].Length);
                months = years.Substring(5);
                years = years.Substring(0, 4);
                if (pieces[1].Length > 0)
                {
                    int junk = months.Length - pieces[1].Length;
                    months = months.Substring(0, junk);
                }
                days = months.Substring(months.LastIndexOf(" "));
                days = days.Trim();
                months = months.Substring(0,months.LastIndexOf(" "));
                months = months.Trim();
                if (days.Length == 1)
                {
                    days = "0" + days;
                }
                if (months.Length == 1)
                {
                    months = "0" + months;
                }
                showName = showName.Trim();
            }
            //Month Day Year format
            pattern = "[0-9]?[0-9][ ][0-9]?[0-9][ ]20[0-9][0-9]";
            if (!matched && Regex.IsMatch(filename, pattern))
            {
                matched = true;
                daily = true;
                Console.WriteLine("The file matches the " + '"' + "Month Date Year" + '"' + " pattern");
                string[] pieces = Regex.Split(filename, pattern);
                showName = pieces[0].Trim();
                showName = StringEx.ProperCase(showName);
                months = filename.Substring(pieces[0].Length);
                if (pieces[1].Length > 0)
                {
                    int junk = months.Length - pieces[1].Length;
                    months = months.Substring(0, junk);
                }
                years = months.Substring(months.Length-4);
                months = months.Substring(0, months.Length - 4);
                months = months.TrimEnd();
                days = months.Substring(months.LastIndexOf(" "));
                days = days.Trim();
                months = months.Substring(0, months.LastIndexOf(" "));
                months = months.Trim();
                if (days.Length == 1)
                {
                   days = "0" + days;
                }
                if (months.Length == 1)
                {
                    months = "0" + months;
                }
                showName = showName.Trim();
            }
            //Check for "Show Name-SxxExxx" format
            pattern = "[s|S][ ]?[0-9][0-9]?[ ]?[e|E][ ]?[0-9][0-9][0-9]";
            if (!matched && Regex.IsMatch(filename, pattern))
            {
                matched = true;
                daily = false;
                Console.WriteLine("The file matches the " + '"' + "SxxExxx" + '"' + " pattern");
                string[] pieces = Regex.Split(filename, pattern);
                showName = pieces[0].Trim();
                showName = StringEx.ProperCase(showName);
                months = filename.Substring(pieces[0].Length);
                if (pieces[1].Length > 0)
                {
                    months = months.Substring(0, months.Length - pieces[1].Length);
                }
                months = months.Trim();
                months = months.ToLower();
                season = months.Substring(0, months.LastIndexOf('e'));
                episodeNumber = months.Substring(months.LastIndexOf('e'));
                season = season.Trim();
                season = season.Substring(1);
                season = season.Trim();
                if (season.Length == 1)
                {
                    season = "0" + season;
                }
                episodeNumber = episodeNumber.Trim();
                episodeNumber = episodeNumber.Substring(1);
                episodeNumber = episodeNumber.Trim();
                if (episodeNumber.Length == 1)
                {
                    episodeNumber = "0" + episodeNumber;
                }
                showName = showName.Trim();
            }

            //Check for "Show Name-SxxExx" format
            pattern = "[s|S][ ]?[0-9][0-9]?[ ]?[e|E][ ]?[0-9][0-9]?";
            if (!matched && Regex.IsMatch(filename, pattern))
            {
                matched = true;
                daily = false;
                Console.WriteLine("The file matches the " + '"' + "SxxExx" + '"' + " pattern");
                string[] pieces = Regex.Split(filename, pattern);
                showName = pieces[0].Trim();
                showName = StringEx.ProperCase(showName);
                months = filename.Substring(pieces[0].Length);
                if(pieces[1].Length>0)
                {
                    months = months.Substring(0,months.Length-pieces[1].Length);
                }
                months = months.Trim();
                months = months.ToLower();
                season = months.Substring(0, months.LastIndexOf('e'));
                episodeNumber = months.Substring(months.LastIndexOf('e'));
                season = season.Trim();
                season = season.Substring(1);
                season = season.Trim();
                if (season.Length == 1)
                {
                    season = "0" + season;
                }
                episodeNumber = episodeNumber.Trim();
                episodeNumber = episodeNumber.Substring(1);
                episodeNumber = episodeNumber.Trim();
                if (episodeNumber.Length == 1)
                {
                    episodeNumber = "0" + episodeNumber;
                }
                showName = showName.Trim();
            }
            //Handle the #x## format
            pattern = "[0-9][0-9]?[ ]?[x|X][ ]?[0-9][0-9]?";
            if (!matched && Regex.IsMatch(filename, pattern))
            {
                matched = true;
                daily = false;
                Console.WriteLine("The file matches the " + '"' + "#x#"+ '"' + " pattern");
                string[] pieces = Regex.Split(filename, pattern);
                showName = pieces[0].Trim();
                showName = StringEx.ProperCase(showName);
                season = filename.Substring(pieces[0].Length);
                season = season.Substring(0, season.Length - pieces[1].Length);
                season = season.ToLower();
                episodeNumber = season.Substring(season.LastIndexOf('x')+1);
                season = season.Substring(0,season.LastIndexOf('x'));
                season = season.Trim();
                if (season.Length == 1)
                {
                    season = "0" + season;
                }
                episodeNumber = episodeNumber.Trim();
                if (episodeNumber.Length == 1)
                {
                    episodeNumber = "0" + episodeNumber;
                }
                showName = showName.Trim();
         }
            //Handle the #### format
            pattern = "[^0-9][0-9][0-9][0-9][0-9][^0-9]";
            if (!matched && Regex.IsMatch(filename, pattern))
            {
                matched = true;
                daily = false;
                Console.WriteLine("The file matches the " + '"' + "####" + '"' + " pattern");
                string[] pieces = Regex.Split(filename, pattern);
                showName = pieces[0].Trim();
                showName = StringEx.ProperCase(showName);
                season = filename.Substring(pieces[0].Length);
                season = season.Substring(0, season.Length - pieces[1].Length);
                season = season.ToLower();
                season = season.Trim();
                if (season.Length == 4)
                {
                    episodeNumber = season.Substring(2);
                    season = season.Substring(0, 2);
                }
                if (season.Length == 1)
                {
                    season = "0" + season;
                }
                episodeNumber = episodeNumber.Trim();
                if (episodeNumber.Length == 1)
                {
                    episodeNumber = "0" + episodeNumber;
                }
                showName = showName.Trim();
            }

            //Handle the ### format
            pattern = "[^0-9][0-9][0-9][0-9][^0-9]";
            if (!matched && Regex.IsMatch(filename, pattern))
            {
                matched = true;
                daily = false;
                Console.WriteLine("The file matches the " + '"' + "###" + '"' + " pattern");
                string[] pieces = Regex.Split(filename, pattern);
                showName = pieces[0].Trim();
                showName = StringEx.ProperCase(showName);
                season = filename.Substring(pieces[0].Length);
                season = season.Substring(0, season.Length - pieces[1].Length);
                season = season.ToLower();
                season = season.Trim();
                if (season.Length == 3)
                {
                    episodeNumber = season.Substring(1);
                    season = season.Substring(0, 1);
                }
                if (season.Length == 1)
                {
                    season = "0" + season;
                }
                episodeNumber = episodeNumber.Trim();
                if (episodeNumber.Length == 1)
                {
                    episodeNumber = "0" + episodeNumber;
                }
                showName = showName.Trim();
            }
            if (preCachedIDs.ContainsKey(showName))
            {
                showID = preCachedIDs[showName].ToString();
                Console.WriteLine("ID for " + showName + " is already cached");
            }
            if (showID == "" && matched)
            {
                Console.WriteLine("Getting ID for " + showName + " from TheTVDB");
                showID = GetTVDBID(showName);
            }
            if (!daily && showID != "")
            {
                Console.WriteLine("Found: " + showName + ", ID#: " + showID + ", Season "+season+", Episode "+episodeNumber);
                Console.WriteLine("Attempting TheTVDB lookup...");
                string webShowName = HttpUtility.HtmlEncode(showName);
                //BEGIN THETVDB.COM LOOKUP CODE HERE--------------------------------
                string lookupSeason = Convert.ToInt32(season).ToString();
                string lookupEpisode = Convert.ToInt32(episodeNumber).ToString();
                Console.WriteLine("http://thetvdb.com/api/" + APIKEY + "/series/" + showID + "/default/" + lookupSeason + "/" + lookupEpisode + "/en.xml");
                XmlDocument fetchedEpisode = new XmlDocument();
                try
                {
                    XmlReader tvdbFetch = new XmlTextReader("http://thetvdb.com/api/" + APIKEY + "/series/" + showID + "/default/" + lookupSeason + "/" + lookupEpisode + "/en.xml");
                    
                    fetchedEpisode.Load(tvdbFetch);
                }
                catch
                {
                    fetchedEpisode = null;
                }
                if (fetchedEpisode != null)
                {
                    XmlNode xmlSID = fetchedEpisode.SelectSingleNode("Data/Episode/EpisodeName");
                    try
                    {
                        episodeTitle = xmlSID.InnerText;
                        Console.WriteLine("Episode Title is " + episodeTitle);
                    }
                    catch
                    {
                        Console.WriteLine("Episode Title not retrieved");
                    }
                    try
                    {
                        xmlSID = fetchedEpisode.SelectSingleNode("Data/Episode/id");
                        episodeID = xmlSID.InnerText;
                        Console.WriteLine("Episode ID is " + episodeID);
                    }
                    catch
                    {
                        Console.WriteLine("Episode ID not retrieved");
                    }
                    try
                    {
                        xmlSID = fetchedEpisode.SelectSingleNode("Data/Episode/seasonid");
                        seasonID = xmlSID.InnerText;
                        Console.WriteLine("Season ID is " + seasonID);
                    }
                    catch
                    {
                        Console.WriteLine("Season ID not retrieved");
                    }
                    try
                    {
                        xmlSID = fetchedEpisode.SelectSingleNode("Data/Episode/Overview");
                        episodeSynopsis = xmlSID.InnerText;
                        Console.WriteLine("Episode Synopsis is " + episodeSynopsis);
                    }
                    catch
                    {
                        Console.WriteLine("Episode Synopsis not retrieved");
                    }
                    try
                    {
                        xmlSID = fetchedEpisode.SelectSingleNode("Data/Episode/filename");
                        episodeThumbnail = xmlSID.InnerText;
                        Console.WriteLine("Episode Thumbnail is " + episodeThumbnail);
                    }
                    catch
                    {
                        Console.WriteLine("Episode Thumbnail not retrieved");
                    }
                }
                string fileSize = "";
                FileInfo getSize = new FileInfo(files[i]);
                int sizeNumber = (int)getSize.Length;
                string guid = CalculateMD5Hash(files[i] + DateTime.Now.ToString("ddd") + ", " + DateTime.Now.Day + " " + DateTime.Now.ToString("MMM") + " " + DateTime.Now.ToString("yyyy") + " " + DateTime.Now.ToString("HH") + ':' + DateTime.Now.ToString("mm") + ':' + DateTime.Now.ToString("ss") + " GMT");
                fileSize = FileStuff.FormatFileSize(sizeNumber);
                string episodeLink = "";
                if (showID != "" && seasonID != "" && episodeID != "")
                {
                    episodeLink = "http://thetvdb.com/?tab=episode&seriesid=" + showID + "&seasonid=" + seasonID + "&id=" + episodeID + "&lid=7";
                }
                //Check to see if TheTVDB missed any details, and fill those from TV.com
                string TVepisodeNumber = "";
                string TVseasonNumber = "";
                string TVepLink = "";
                string TVepTitle = "";
                string TVepSynopsis = "";
                //Begin TV.Com Daily Show Per-Episode Scraper
                int numberTemp = Convert.ToInt32(episodeNumber);
                TVepisodeNumber = numberTemp.ToString();
                numberTemp = Convert.ToInt32(season);
                TVseasonNumber = numberTemp.ToString();
                Console.WriteLine("Now attempting TV.Com lookup");
                string episodeGuide = GetTVDBEpisodeGuide(showName);
                if (episodeGuide != "")
                {
                    WebClient wc = new WebClient();
                    byte[] rawResponse;
                    try
                    {
                        rawResponse = wc.DownloadData(episodeGuide);
                    }
                    catch
                    {
                        rawResponse = null;
                    }
                    if (rawResponse != null)
                    {
                        Console.WriteLine("Episode guide retrieved, attempting to scrape for episode info...");
                        string[] TVresults = Encoding.ASCII.GetString(rawResponse).Split('<');
                        Regex scanner = new Regex("Season " + season + ", Episode " + episodeNumber);
                        int found = -1;
                        for (int counto = 0; counto < TVresults.Length; counto++)
                        {
                            string tempcheckText = TVresults[counto];
                            tempcheckText = CleanScrapeResults(tempcheckText);
                            if (scanner.IsMatch(tempcheckText))
                            {
                                found = counto;
                                counto = TVresults.Length;
                            }
                        }
                        string epLinkAndTitle = CleanScrapeResults(TVresults[found + 3]);
                        TVepLink = epLinkAndTitle.Substring(8);
                        TVepLink = TVepLink.Substring(0, TVepLink.LastIndexOf('"'));
                        TVepTitle = epLinkAndTitle.Substring(epLinkAndTitle.LastIndexOf('>') + 1);
                        Console.WriteLine("Retrieved episode title: " + TVepTitle);
                        TVepSynopsis = "";
                        TVepSynopsis = "<" + TVresults[found + 6] + "<" + TVresults[found + 7] + "<" + TVresults[found + 8] + "<" + TVresults[found + 9] + "<" + TVresults[found + 10];
                        TVepSynopsis = CleanString(TVepSynopsis);
                        TVepSynopsis = CleanScrapeResults(TVepSynopsis);
                        TVepSynopsis = TVepSynopsis.Replace("<p class=" + '"' + "synopsis" + '"' + ">", "");
                        TVepSynopsis = TVepSynopsis.Replace("<strong>", "");
                        TVepSynopsis = TVepSynopsis.Replace("</strong>", "");
                        TVepSynopsis = TVepSynopsis.Replace("Segments: - - ", "");
                        TVepSynopsis = TVepSynopsis.Replace(" </p> ", "");
                        TVepSynopsis = TVepSynopsis.Replace("<ul class=" + '"' + "TAB_LINKS various_links" + '"' + ">", "");
                        TVepSynopsis = TVepSynopsis.Replace("<li class=" + '"' + "first" + '"' + ">", "");
                        TVepSynopsis = TVepSynopsis.Replace("<a href=" + '"', " ");
                        Regex linkfinder = new Regex("http://");
                        if (linkfinder.IsMatch(TVepSynopsis))
                        {
                            string[] synops = linkfinder.Split(TVepSynopsis);
                            TVepSynopsis = synops[0];
                        }

                        TVepSynopsis = TVepSynopsis.Trim();
                        Console.WriteLine(TVepSynopsis);
                        if (episodeTitle == "")
                        {
                            episodeTitle = TVepTitle;
                        }
                        if (episodeSynopsis == "")
                        {
                            episodeSynopsis = TVepSynopsis;
                        }
                        if (episodeLink == "")
                        {
                            episodeLink = TVepLink;
                        }
                    }
                }                
                //END TV.COM SCRAPER CODE
                //BEGIN RSS GENERATOR CODE
                if (File.Exists(RSSoutputFile))
                {
                    string itemTitle = showName + "-Season " + season + ", Episode " + episodeNumber;
                    if (episodeTitle != "")
                    {
                        itemTitle = itemTitle + ": " + episodeTitle;
                    }
                    string itemDescription = "";
                    if (episodeThumbnail != "")
                    {
                        itemDescription = "<img src=" + '"' + "http://images.thetvdb.com/banners/" + episodeThumbnail + '"' + "><BR><BR>";
                    }
                    itemDescription = itemDescription + "<B><U>Episode Synopsis</U></B><BR>";
                    if (episodeSynopsis == "")
                    {
                        itemDescription = itemDescription + "No episode synopsis available.<BR><BR><B>Downloaded File:</B> " + filenameParts[1] + filenameParts[2] + ", " + fileSize;
                    }
                    if (episodeSynopsis != "")
                    {
                        itemDescription = itemDescription + episodeSynopsis + "<BR><BR><B>Downloaded File:</B>" + filenameParts[1] + filenameParts[2] + ", " + fileSize;
                    }
                    //get the current XMLcode
                    XmlReader xmlReader = XmlReader.Create(RSSoutputFile);
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(xmlReader);
                    xmlReader.Close();
                    //Navigate to the channel element
                    XmlNode UpdatePubDate = xmlDocument.SelectSingleNode("rss/channel/lastBuildDate");
                    UpdatePubDate.InnerText = "";
                    UpdatePubDate.InnerText = DateTime.Now.ToString("ddd") + ", " + DateTime.Now.Day + " " + DateTime.Now.ToString("MMM") + " " + DateTime.Now.ToString("yyyy") + " " + DateTime.Now.ToString("HH") + ':' + DateTime.Now.ToString("mm") + ':' + DateTime.Now.ToString("ss") + " GMT";
                    XmlNode channel = xmlDocument.SelectSingleNode("rss/channel");
                    //Ready to create individual items
                    XmlNode item = xmlDocument.CreateElement("item");
                    channel.AppendChild(item);

                    XmlNode title = xmlDocument.CreateElement("title");
                    item.AppendChild(title);
                    title.InnerText = itemTitle;

                    XmlNode link = xmlDocument.CreateElement("link");
                    item.AppendChild(link);
                    link.InnerText = episodeLink;

                    XmlElement itemGuid = xmlDocument.CreateElement("guid");
                    itemGuid.SetAttribute("isPermaLink", "false");
                    item.AppendChild(itemGuid);
                    itemGuid.InnerText = guid;

                    XmlCDataSection descriptionData;
                    XmlElement description = xmlDocument.CreateElement("description");
                    item.AppendChild(description);
                    descriptionData = xmlDocument.CreateCDataSection("description");
                    descriptionData.InnerText = itemDescription;
                    description.AppendChild(descriptionData);

                    XmlNode pubDate = xmlDocument.CreateElement("pubDate");
                    item.AppendChild(pubDate);
                    pubDate.InnerText = DateTime.Now.ToString("ddd") + ", " + DateTime.Now.Day + " " + DateTime.Now.ToString("MMM") + " " + DateTime.Now.ToString("yyyy") + " " + DateTime.Now.ToString("HH") + ':' + DateTime.Now.ToString("mm") + ':' + DateTime.Now.ToString("ss") + " GMT";
                    //Write the changes out to the finished XML file
                    if (season != "" && episodeNumber != "")
                    {
                        XmlWriter write = XmlWriter.Create(RSSoutputFile);
                        xmlDocument.Save(write);
                        write.Close();
                        Console.WriteLine("RSS file updated");
                    }
                }
                //END RSS CODE    
            }
            //Assuming we do have a daily show and we can convert it to Season / Episode format, we handle that now
            if (daily && showID != "")
            {
                Console.WriteLine("Found: " + showName + ", ID#: " + showID + ", episode aired " + months + "-" + days + "-" + years);
                Console.WriteLine("Attempting TheTVDB lookup...");
                string webShowName = HttpUtility.HtmlEncode(showName);
                XmlReader tvdbFetch = new XmlTextReader("http://thetvdb.com/api/GetEpisodeByAirDate.php?apikey=" + APIKEY + "&seriesid=" + showID + "&airdate=" + days + "-" + months + "-" + years);
                XmlDocument fetchedEpisode = new XmlDocument();
                fetchedEpisode.Load(tvdbFetch);
                XmlNode xmlSID = fetchedEpisode.SelectSingleNode("Data/Episode/EpisodeName");
                try
                {
                    episodeTitle = xmlSID.InnerText;
                    Console.WriteLine("Episode Title is " + episodeTitle);
                }
                catch
                {
                    Console.WriteLine("Episode Title not retrieved");
                }
                try
                {
                    xmlSID = fetchedEpisode.SelectSingleNode("Data/Episode/EpisodeNumber");
                    episodeNumber = xmlSID.InnerText;
                    Console.WriteLine("Episode Number is " + episodeNumber);
                }
                catch
                {
                    Console.WriteLine("Episode Number not retrieved");
                }
                try
                {
                    xmlSID = fetchedEpisode.SelectSingleNode("Data/Episode/SeasonNumber");
                    season = xmlSID.InnerText;
                    Console.WriteLine("Season Number is " + season);
                }
                catch
                {
                    Console.WriteLine("Season Number not retrieved");
                }
                try
                {
                    xmlSID = fetchedEpisode.SelectSingleNode("Data/Episode/id");
                    episodeID = xmlSID.InnerText;
                    Console.WriteLine("Episode ID is " + episodeID);
                }
                catch
                {
                    Console.WriteLine("Episode ID not retrieved");
                }
                try
                {
                    xmlSID = fetchedEpisode.SelectSingleNode("Data/Episode/seasonid");
                    seasonID = xmlSID.InnerText;
                    Console.WriteLine("Season ID is " + seasonID);
                }
                catch
                {
                    Console.WriteLine("Season ID not retrieved");
                }
                try
                {
                    xmlSID = fetchedEpisode.SelectSingleNode("Data/Episode/Overview");
                    episodeSynopsis = xmlSID.InnerText;
                    Console.WriteLine("Episode Synopsis is " + episodeSynopsis);
                }
                catch
                {
                    Console.WriteLine("Episode Synopsis not retrieved");
                }
                try
                {
                    xmlSID = fetchedEpisode.SelectSingleNode("Data/Episode/filename");
                    episodeThumbnail = xmlSID.InnerText;
                    Console.WriteLine("Episode Thumbnail is " + episodeThumbnail);
                }
                catch
                {
                    Console.WriteLine("Episode Thumbnail not retrieved");
                }
                string fileSize = "";
                FileInfo getSize = new FileInfo(files[i]);
                int sizeNumber = (int)getSize.Length;
                string guid = CalculateMD5Hash(files[i] + DateTime.Now.ToString("ddd") + ", " + DateTime.Now.Day + " " + DateTime.Now.ToString("MMM") + " " + DateTime.Now.ToString("yyyy") + " " + DateTime.Now.ToString("HH") + ':' + DateTime.Now.ToString("mm") + ':' + DateTime.Now.ToString("ss") + " GMT");
                fileSize = FileStuff.FormatFileSize(sizeNumber);
                string episodeLink = "";
                if (showID != "" && seasonID != "" && episodeID != "")
                {
                    episodeLink = "http://thetvdb.com/?tab=episode&seriesid=" + showID + "&seasonid=" + seasonID + "&id=" + episodeID + "&lid=7";
                }
                //Now we check to see if TheTVDB was missing any info and fill that with the TV.com scraper
                string TVepNumbers = "";
                string TVepisodeNumber = "";
                string TVseasonNumber = "";
                string TVepLink = "";
                string TVepTitle = "";
                string TVepSynopsis = "";
                int numberTemp = Convert.ToInt32(days);
                days = numberTemp.ToString();
                numberTemp = Convert.ToInt32(months);
                months = numberTemp.ToString();
                Console.WriteLine("Now attempting TV.Com lookup");
                string episodeGuide = GetTVDBEpisodeGuide(showName);
                if (episodeGuide != "")
                {
                    WebClient wc = new WebClient();
                    byte[] TVrawResponse;
                    try
                    {
                        TVrawResponse = wc.DownloadData(episodeGuide);
                    }
                    catch
                    {
                        TVrawResponse = null;
                    }
                    if (TVrawResponse != null)
                    {
                        Console.WriteLine("Episode guide retrieved, attempting to scrape for episode info...");
                        string catcher = Encoding.ASCII.GetString(TVrawResponse);
                        string[] TVresults = catcher.Split('<');
                        Regex scanner = new Regex("Aired: " + months + "/" + days + "/" + years);
                        int found = -1;
                        for (int counter = 0; counter < TVresults.Length; counter++)
                        {
                            string tempResults = CleanScrapeResults(TVresults[counter]);
                            if (scanner.IsMatch(tempResults))
                            {
                                found = counter;
                                counter = TVresults.Length;
                            }
                        }
                        try
                        {
                            TVepNumbers = CleanScrapeResults(TVresults[found]);
                            TVepNumbers = TVepNumbers.Substring(14);
                            TVepNumbers = TVepNumbers.Trim();
                            TVepNumbers = TVepNumbers.Substring(TVepNumbers.IndexOf('S'), TVepNumbers.IndexOf('&'));
                            TVepNumbers = TVepNumbers.Trim();
                            TVepNumbers = TVepNumbers.Substring(0, TVepNumbers.LastIndexOf(' '));
                            TVepisodeNumber = TVepNumbers.Substring(TVepNumbers.IndexOf(',') + 10);
                            Console.WriteLine("Retrieved episode number " + TVepisodeNumber);
                            TVseasonNumber = TVepNumbers.Substring(0, TVepNumbers.IndexOf(','));
                            TVseasonNumber = TVseasonNumber.Substring(7);
                            Console.WriteLine("Retrieved season number " + TVseasonNumber);
                            string epLinkAndTitle = CleanScrapeResults(TVresults[found + 3]);
                            TVepLink = epLinkAndTitle.Substring(8);
                            TVepLink = TVepLink.Substring(0, TVepLink.LastIndexOf('"'));
                            TVepTitle = epLinkAndTitle.Substring(epLinkAndTitle.LastIndexOf('>') + 1);
                            Console.WriteLine("Retrieved episde title: " + TVepTitle);
                            TVepSynopsis = "";
                            TVepSynopsis = "<" + TVresults[found + 6] + "<" + TVresults[found + 7] + "<" + TVresults[found + 8] + "<" + TVresults[found + 9] + "<" + TVresults[found + 10];
                            TVepSynopsis = CleanString(TVepSynopsis);
                            TVepSynopsis = CleanScrapeResults(TVepSynopsis);
                            TVepSynopsis = TVepSynopsis.Replace("<p class=" + '"' + "synopsis" + '"' + ">", "");
                            TVepSynopsis = TVepSynopsis.Replace("<strong>", "");
                            TVepSynopsis = TVepSynopsis.Replace("</strong>", "");
                            TVepSynopsis = TVepSynopsis.Replace("Segments: - - ", "");
                            TVepSynopsis = TVepSynopsis.Replace(" </p> ", "");
                            TVepSynopsis = TVepSynopsis.Replace("<ul class=" + '"' + "TAB_LINKS various_links" + '"' + ">", "");
                            TVepSynopsis = TVepSynopsis.Replace("<li class=" + '"' + "first" + '"' + ">", "");
                            TVepSynopsis = TVepSynopsis.Replace("<a href=" + '"', " ");
                            Regex linkfinder = new Regex("http://");
                            if (linkfinder.IsMatch(TVepSynopsis))
                            {
                                string[] synops = linkfinder.Split(TVepSynopsis);
                                TVepSynopsis = synops[0];
                            }

                            TVepSynopsis = TVepSynopsis.Trim();
                            Console.WriteLine(TVepSynopsis);
                            if (episodeTitle == "")
                            {
                                episodeTitle = TVepTitle;
                            }
                            if (season == "")
                            {
                                season = TVseasonNumber;
                            }
                            if (episodeNumber == "")
                            {
                                episodeNumber = TVepisodeNumber;
                            }
                            if (episodeSynopsis == "")
                            {
                                episodeSynopsis = TVepSynopsis;
                            }
                            if (episodeLink == "")
                            {
                                episodeLink = TVepLink;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                //END TV.COM DAILY CODE
                    //BEGIN RSS GENERATOR CODE
                if (File.Exists(RSSoutputFile)&& showName !="" && season !="" && episodeNumber !="")
                {
                    string itemTitle = showName + "-Season " + season + ", Episode " + episodeNumber;
                    if (episodeTitle != "")
                    {
                        itemTitle = itemTitle + ": " + episodeTitle;
                    }
                    string itemDescription = "";
                    if (episodeThumbnail != "")
                    {
                        itemDescription = "<img src=" + '"' + "http://images.thetvdb.com/banners/" + episodeThumbnail + '"' + "><BR><BR>";
                    }
                    itemDescription = itemDescription + "<B><U>Episode Synopsis</U></B><BR>";
                    if (episodeSynopsis == "")
                    {
                        itemDescription = itemDescription + "No episode synopsis available.<BR><BR><B>Downloaded File:</B> " + filenameParts[1] + filenameParts[2] + ", " + fileSize;
                    }
                    if (episodeSynopsis != "")
                    {
                        itemDescription = itemDescription + episodeSynopsis + "<BR><BR><B>Downloaded File:</B>" + filenameParts[1] + filenameParts[2] + ", " + fileSize;
                    }
                    //get the current XMLcode
                    XmlReader xmlReader = XmlReader.Create(RSSoutputFile);
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(xmlReader);
                    xmlReader.Close();
                    //Navigate to the channel element
                    XmlNode UpdatePubDate = xmlDocument.SelectSingleNode("rss/channel/lastBuildDate");
                    UpdatePubDate.InnerText = "";
                    UpdatePubDate.InnerText = DateTime.Now.ToString("ddd") + ", " + DateTime.Now.Day + " " + DateTime.Now.ToString("MMM") + " " + DateTime.Now.ToString("yyyy") + " " + DateTime.Now.ToString("HH") + ':' + DateTime.Now.ToString("mm") + ':' + DateTime.Now.ToString("ss") + " GMT";
                    XmlNode channel = xmlDocument.SelectSingleNode("rss/channel");
                    //Ready to create individual items
                    XmlNode item = xmlDocument.CreateElement("item");
                    channel.AppendChild(item);

                    XmlNode title = xmlDocument.CreateElement("title");
                    item.AppendChild(title);
                    title.InnerText = itemTitle;

                    XmlNode link = xmlDocument.CreateElement("link");
                    item.AppendChild(link);
                    link.InnerText = episodeLink;

                    XmlElement itemGuid = xmlDocument.CreateElement("guid");
                    itemGuid.SetAttribute("isPermaLink", "false");
                    item.AppendChild(itemGuid);
                    itemGuid.InnerText = guid;

                    XmlCDataSection descriptionData;
                    XmlElement description = xmlDocument.CreateElement("description");
                    item.AppendChild(description);
                    descriptionData = xmlDocument.CreateCDataSection("description");
                    descriptionData.InnerText = itemDescription;
                    description.AppendChild(descriptionData);
                    //XmlNode description = xmlDocument.CreateElement("description");
                    //item.AppendChild(description);
                    //description.InnerText = itemDescription;

                    XmlNode pubDate = xmlDocument.CreateElement("pubDate");
                    item.AppendChild(pubDate);
                    pubDate.InnerText = DateTime.Now.ToString("ddd") + ", " + DateTime.Now.Day + " " + DateTime.Now.ToString("MMM") + " " + DateTime.Now.ToString("yyyy") + " " + DateTime.Now.ToString("HH") + ':' + DateTime.Now.ToString("mm") + ':' + DateTime.Now.ToString("ss") + " GMT";
                    if (season !="" && episodeNumber !="")
                    {
                        //Write the changes out to the finished XML file
                        XmlWriter write = XmlWriter.Create(RSSoutputFile);
                        xmlDocument.Save(write);
                        write.Close();
                        Console.WriteLine("RSS file updated");
                    }

                }
                //END RSS CODE
            } 
            //BEGIN NFO writer
            if (File.Exists(@".\episode.txt"))
            {
                File.Delete(@".\episode.txt");
            }
            if (showName != "" && season != "" && episodeNumber != "")
            {
                FileStream nfoStream = new FileStream(@".\episode.txt", FileMode.Create);
                StreamWriter nfoWriter = new StreamWriter(nfoStream);
                nfoWriter.WriteLine("<?xml version=" + '"' + "1.0" + '"' + " encoding=" + '"' + "UTF-8" + '"' + " standalone=" + '"' + "yes" + '"' + " ?>");
                nfoWriter.WriteLine("<episodedetails>");
                    if(episodeTitle!="")
                    {
                        nfoWriter.WriteLine("<title>" + episodeTitle + "</title>");
                    }
                    nfoWriter.WriteLine("<season>" + season + "</season>");
                    nfoWriter.WriteLine("<episode>" + episodeNumber + "</episode>");
                    if (episodeSynopsis != "")
                    {
                        nfoWriter.WriteLine("<plot>" + episodeSynopsis + "</plot>");
                    }
                    nfoWriter.WriteLine("</episodedetails>");
                    nfoWriter.Flush();
                    nfoWriter.Close();
                    Console.WriteLine("NFO file created");
                
            }
                    //Begin mover section
                    //we have a daily destination populated AND we have a daily file, the most special case
                    if (destDaily != "" && daily && episodeNumber!="")
                    {
                        if (!Directory.Exists(destDaily + showName + '\\'))
                        {
                            Directory.CreateDirectory(destDaily + showName + '\\');
                        }
                        string finalName = destDaily + showName + '\\' + showName + "-S" + season + "E" + episodeNumber;
                        if (episodeTitle != "")
                        {
                            finalName = finalName+"-" + episodeTitle + filenameParts[2];
                        }
                        if(episodeTitle =="")
                        {
                            finalName = finalName + filenameParts[2];
                        }
                        Console.WriteLine("Moving " + files[i]);
                        Console.WriteLine("To: " + finalName);
                        if (File.Exists(finalName))
                        {
                            finalName = FileStuff.SafeFileWrite(finalName,filenameParts[2]);
                            Console.WriteLine("File already exists, changing to "+finalName);
                        }
                        string finalNFOname = finalName.Substring(0, finalName.LastIndexOf('.'));
                        finalNFOname = finalNFOname + ".nfo";
                        File.Move(files[i], finalName);
                        Console.WriteLine("Moving NFO file to " + finalNFOname);
                        File.Move(@".\episode.txt",finalNFOname);
                    }
                //this is if we get a daily show but do not have anywhere special to put it, which means we actually use the same steps
                //as a "standard" show
                    if (daily && destDaily == "" && destRegular != "" && episodeNumber != "")
                    {
                        if (!Directory.Exists(destRegular + showName + '\\'))
                        {
                            Directory.CreateDirectory(destRegular + showName + '\\');
                        }
                        string finalName = destRegular + showName + '\\' + showName + "-S" + season + "E" + episodeNumber;
                        if (episodeTitle != "")
                        {
                            finalName = finalName + "-" + episodeTitle + filenameParts[2];
                        }
                        if (episodeTitle == "")
                        {
                            finalName = finalName + filenameParts[2];
                        }
                        Console.WriteLine("Moving " + files[i]);
                        Console.WriteLine("To: " + finalName);
                        if (File.Exists(finalName))
                        {
                            finalName = FileStuff.SafeFileWrite(finalName, filenameParts[2]);
                            Console.WriteLine("File already exists, changing to " + finalName);
                        }
                        File.Move(files[i], finalName);
                        string finalNFOname = finalName.Substring(0, finalName.LastIndexOf('.'));
                        finalNFOname = finalNFOname + ".nfo";
                        File.Move(files[i], finalName);
                        Console.WriteLine("Moving NFO file to " + finalNFOname);
                        File.Move(@".\episode.txt", finalNFOname);
                    }
                    //This should handle the standard cases
                    if (!daily && destRegular != "" && episodeNumber != "")
                    {
                        if(!Directory.Exists(destRegular + showName + '\\'))
                        {
                            Directory.CreateDirectory(destRegular + showName + '\\');
                        }
                        string finalName = destRegular + showName+'\\' + showName + "-S" + season + "E" + episodeNumber;
                        if (episodeTitle != "")
                        {
                            finalName = finalName + "-" + episodeTitle + filenameParts[2];
                        }
                        if(episodeTitle =="")
                        {
                            finalName = finalName + filenameParts[2];
                        }
                        Console.WriteLine("Moving " + files[i]);
                        Console.WriteLine("To: " + finalName);
                        if (File.Exists(finalName))
                        {
                            finalName = FileStuff.SafeFileWrite(finalName, filenameParts[2]);
                            Console.WriteLine("File already exists, changing to " + finalName);
                        }
                        File.Move(files[i], finalName);
                        string finalNFOname = finalName.Substring(0, finalName.LastIndexOf('.'));
                        finalNFOname = finalNFOname + ".nfo";
                        Console.WriteLine("Moving NFO file to " + finalNFOname);
                        File.Move(@".\episode.txt", finalNFOname);
                   }
                //For the times when the exe is run with no destinations specified
                //we just leave the files in the folder they're already in
                    if (destRegular == "" && destDaily == "" && episodeNumber != "")
                    {
                        if (!Directory.Exists(filenameParts[0] + showName + '\\'))
                        {
                            Directory.CreateDirectory(filenameParts[0] + showName + '\\');
                        }
                        string finalName = filenameParts[0] + showName + '\\' + showName + "-S" + season + "E" + episodeNumber;
                        if (episodeTitle != "")
                        {
                            finalName = finalName + "-" + episodeTitle + filenameParts[2];
                        }
                        if (episodeTitle == "")
                        {
                            finalName = finalName + filenameParts[2];
                        }
                        Console.WriteLine("Moving " + files[i]);
                        Console.WriteLine("To: " + finalName);
                        if (File.Exists(finalName))
                        {
                            finalName = FileStuff.SafeFileWrite(finalName, filenameParts[2]);
                            Console.WriteLine("File already exists, changing to " + finalName);
                        }
                        File.Move(files[i], finalName);
                        string finalNFOname = finalName.Substring(0, finalName.LastIndexOf('.'));
                        finalNFOname = finalNFOname + ".nfo";
                        File.Move(files[i], finalName);
                        Console.WriteLine("Moving NFO file to " + finalNFOname);
                        File.Move(@".\episode.txt", finalNFOname);
                    }
            Console.WriteLine(" ");
            daily = false;
            matched = false;
            //This is the final part of the loop
        }
    }

    public static string GetTVDBID(string ShowName)
    {
        string seriesID ="";
        try
        {
            ShowName = HttpUtility.HtmlEncode(ShowName);
            XmlReader tvdbFetch = new XmlTextReader("http://thetvdb.com/api/GetSeries.php?seriesname=" + ShowName + "&language=en");
            XmlDocument fetchedShow = new XmlDocument();
            fetchedShow.Load(tvdbFetch);
            XmlNode xmlSID = fetchedShow.SelectSingleNode("Data/Series/seriesid");
            seriesID = xmlSID.InnerText;
        }
        catch
        {
            seriesID = "";
        }
        return seriesID;
    }
    public static string CalculateMD5Hash(string input)
    {
        // step 1, calculate MD5 hash from input
        MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);

        // step 2, convert byte array to hex string
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }
    static string GetTVDBEpisodeGuide(string showName)
    {
        string episodeGuideLink = "";
        string searchShowName = HttpUtility.UrlPathEncode(showName);
        string searchAddress = "http://www.tv.com/search.php?type=Search&stype=ajax_search&qs=" + showName + "&search_type=program&pg_results=0&sort=";
        WebClient wc = new WebClient();
        try
        {
            byte[] rawResponse = wc.DownloadData(searchAddress);
            string[] results = Encoding.ASCII.GetString(rawResponse).Split('\n');
            Regex scanner = new Regex("episode.html");
            int found = -1;
            for (int i = 0; i < results.Length; i++)
            {
                if (scanner.IsMatch(results[i]))
                {
                    found = i;
                    i = results.Length;
                }
            }
            string[] temp = results[found].Trim().Split('>');
            episodeGuideLink = temp[1];
            episodeGuideLink = episodeGuideLink.Trim();
            episodeGuideLink = episodeGuideLink.Replace('<', ' ');
            episodeGuideLink = episodeGuideLink.Replace('"', ' ');
            episodeGuideLink = episodeGuideLink.Trim();
            episodeGuideLink = episodeGuideLink.Substring(8);
        }
        catch
        {
            episodeGuideLink = "";
        }
        return episodeGuideLink;
    }
    static string CleanScrapeResults(string input)
    {
        input = input.TrimStart('\n');
        input = input.TrimEnd('\n');
        input = input.Replace('\n', ' ');
        input = input.Replace("          ", " ");
        input = input.Replace("         ", " ");
        input = input.Replace("        ", " ");
        input = input.Replace("       ", " ");
        input = input.Replace("      ", " ");
        input = input.Replace("     ", " ");
        input = input.Replace("    ", " ");
        input = input.Replace("   ", " ");
        input = input.Replace("  ", " ");
        input = input.Trim();
        input = input.TrimStart('\n');
        input = input.TrimEnd('\n');
        input = input.Replace('\n', ' ');
        input = input.Replace("    ", " ");
        input = input.Replace("   ", " ");
        input = input.Replace("  ", " ");
        input = input.Trim();
        return input;
    }
    static public string CleanString(string s)
    {
        if (s != null && s.Length > 0)
        {
            StringBuilder sb = new StringBuilder(s.Length);
            foreach (char c in s)
            {
                sb.Append(Char.IsControl(c) ? ' ' : c);
            }
            s = sb.ToString();
        }
        return s;
    }
}

