using DotNetWikiBot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    class tzclass //time zones
    {
        public string offset = "0";
        public string summeroffset = "0";
        public string rawoffset = "0";
        public string tzname = "";
        public string tzsummer = "";
        public string tzfull = "";
        public string tzfullsummer = "";

        public static Dictionary<string, tzclass> tzdict = new Dictionary<string, tzclass>(); //Time zone dictionary
        public static Dictionary<string, string> tznamedict = new Dictionary<string, string>(); //from timezone offset to timezone acronym (standard time)
        public static Dictionary<string, string> tzsummerdict = new Dictionary<string, string>(); //from timezone offset to timezone acronym (summer time)
        public static Dictionary<string, string> tzfulldict = new Dictionary<string, string>(); //from timezone offset to timezone full name (standard time)
        public static Dictionary<string, string> tzfullsummerdict = new Dictionary<string, string>(); //from timezone offset to timezone full name (summer time)

        public static void read_timezone()
        {
            int n = 0;

            tzdict.Clear();
            tznamedict.Clear();
            tzsummerdict.Clear();
            tzfulldict.Clear();
            tzfullsummerdict.Clear();

            string filename = "timezonenames.txt";

            //first look if timezone name file exists for specific country...
            filename = "timezonenames-" + Form1.makecountry + ".txt";
            //Console.WriteLine(filename);

            //... then for continent...
            if (!File.Exists(geonameclass.geonamesfolder + filename))
                if ((!String.IsNullOrEmpty(Form1.makecountry)) && Form1.countryid.ContainsKey(Form1.makecountry))
                    filename = "timezonenames-" + Form1.countrydict[Form1.countryid[Form1.makecountry]].continent + ".txt";

            //...otherwise default names.
            if (!File.Exists(geonameclass.geonamesfolder + filename))
                filename = "timezonenames.txt";

            Console.WriteLine(filename);
            //Console.ReadLine();

            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + filename))
            {
                //String line = sr.ReadLine();
                //line = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    //Console.WriteLine(line);
                    n++;
                    string[] words = line.Split('\t');

                    if (!String.IsNullOrEmpty(words[1]))
                        tznamedict.Add(words[0], words[1]);
                    if (!String.IsNullOrEmpty(words[2]))
                        tzsummerdict.Add(words[0], words[2]);
                    if (!String.IsNullOrEmpty(words[3]))
                        tzfulldict.Add(words[0], words[3]);
                    if ((words.Length > 4) && (!String.IsNullOrEmpty(words[4])))
                        tzfullsummerdict.Add(words[0], words[4]);
                }
            }

            n = 0;
            Console.WriteLine("tznamedict.Count = " + tznamedict.Count);
            //Console.ReadLine();

            if (Form1.savewikilinks)
            {
                Page pt = new Page(Form1.makesite, util.mp(13) + Form1.botname + "/timezonelinks");
                pt.text = "Timezone links used by Lsjbot\n\n";
                foreach (string tz in tznamedict.Keys)
                    pt.text += "* UTC" + tz + " [[" + tzfulldict[tz] + "|" + tznamedict[tz] + "]]\n";
                foreach (string tz in tzsummerdict.Keys)
                    pt.text += "* UTC" + tz + " [[" + tzfullsummerdict[tz] + "|" + tzsummerdict[tz] + "]]\n";
                util.trysave(pt, 1, "Bot saving timezonelinks");
            }

            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "timeZones.txt"))
            {
                String line = sr.ReadLine();
                line = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();


                    //if (n > 250)
                    //    Console.WriteLine(line);

                    string[] words = line.Split('\t');

                    //foreach (string s in words)
                    //    Console.WriteLine(s);

                    //Console.WriteLine(words[0] + "|" + words[1]);

                    //    public static Dictionary<string,Dictionary<string,int>> adm1dict = new Dictionary<string,Dictionary<string,int>>();

                    tzclass tzz = new tzclass();

                    for (int ii = 2; ii < 5; ii++)
                    {
                        if (!words[ii].Contains("+") && !words[ii].Contains("-"))
                            words[ii] = "+" + words[ii];
                        words[ii] = words[ii].Replace(".0", "");
                        words[ii] = words[ii].Replace(".5", ":30");
                        words[ii] = words[ii].Replace(".75", ":45");

                    }

                    tzz.offset = words[2];
                    tzz.summeroffset = words[3];
                    tzz.rawoffset = words[4];


                    if (tznamedict.ContainsKey(tzz.offset))
                        tzz.tzname = tznamedict[tzz.offset];
                    else
                        Console.WriteLine("No tzname for |" + tzz.offset + "|");
                    if (tzfulldict.ContainsKey(tzz.offset))
                        tzz.tzfull = tzfulldict[tzz.offset];

                    if ((tzz.summeroffset != tzz.offset) && (tzsummerdict.ContainsKey(tzz.summeroffset)))
                    {
                        tzz.tzsummer = tzsummerdict[tzz.summeroffset];
                        tzz.tzfullsummer = tzfullsummerdict[tzz.summeroffset];
                    }

                    tzdict.Add(words[1], tzz);

                    n++;
                    if ((n % 1000) == 0)
                    {
                        Console.WriteLine("n (timezone)   = " + n.ToString());

                    }

                }

                Console.WriteLine("n    (timezone)= " + n.ToString());
                //Console.ReadLine();

            }
        }



    }
}
