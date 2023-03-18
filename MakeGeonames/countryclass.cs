using DotNetWikiBot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    public class countryclass
    {
        public string Name = ""; //main name
        public string Name_ml = ""; //name in makelang language 
        public string asciiname = ""; //name in plain ascii
        public List<string> altnames = new List<string>(); //alternative names
        public string iso = "XX";
        public string iso3 = "XXX";
        public int isonumber = 0;
        public string fips = "XX";
        public string capital = "";
        public int capital_gnid = 0;
        public double area = 0.0;
        public long population = 0;
        public string continent = "EU";
        public string tld = ".xx";
        public string currencycode = "USD";
        public string currencyname = "Dollar";
        public string phone = "1-999";
        public string postalcode = "#####";
        public string nativewiki = "";
        public List<int> languages = new List<int>();
        public List<string> bordering = new List<string>();
#if (DBGEOFLAG)
            public shapeclass shape = null; //country shape(s); null means no shapefile. Shapes unreliable for Antarctica, Fiji, Norway, Russia
#endif
        public double clat = 9999; //lat, long of centroid of country shape
        public double clon = 9999;

        public static void fill_motherdict() //mother countries of colonies
        {
            if (Form1.motherdict.Count > 0)
                return;
            Form1.motherdict.Add("AN", "NL");

            Form1.motherdict.Add("AS", "US");
            Form1.motherdict.Add("AW", "NL");
            Form1.motherdict.Add("AX", "FI");
            Form1.motherdict.Add("BL", "FR");
            Form1.motherdict.Add("BV", "NO");
            Form1.motherdict.Add("CK", "NZ");
            Form1.motherdict.Add("CX", "AU");
            Form1.motherdict.Add("FK", "GB");
            Form1.motherdict.Add("FM", "US");
            Form1.motherdict.Add("FO", "DK");
            Form1.motherdict.Add("GF", "FR");
            Form1.motherdict.Add("GG", "GB");
            Form1.motherdict.Add("GI", "GB");
            Form1.motherdict.Add("GL", "DK");
            Form1.motherdict.Add("GP", "FR");
            Form1.motherdict.Add("GU", "US");
            Form1.motherdict.Add("HK", "CN");
            Form1.motherdict.Add("HM", "AU");
            Form1.motherdict.Add("IM", "GB");
            Form1.motherdict.Add("IO", "GB");
            Form1.motherdict.Add("JE", "GB");
            Form1.motherdict.Add("MF", "FR");
            Form1.motherdict.Add("MH", "US");
            Form1.motherdict.Add("MO", "CN");
            Form1.motherdict.Add("MP", "US");
            Form1.motherdict.Add("MQ", "FR");
            Form1.motherdict.Add("MS", "GB");
            Form1.motherdict.Add("NU", "NZ");
            Form1.motherdict.Add("PF", "FR");
            Form1.motherdict.Add("PM", "FR");
            Form1.motherdict.Add("PN", "GB");
            Form1.motherdict.Add("PR", "US");
            Form1.motherdict.Add("RE", "FR");
            Form1.motherdict.Add("SH", "GB");
            Form1.motherdict.Add("SJ", "NO");
            Form1.motherdict.Add("TC", "GB");
            Form1.motherdict.Add("TF", "FR");
            Form1.motherdict.Add("TK", "NZ");
            Form1.motherdict.Add("UM", "US");
            Form1.motherdict.Add("VG", "GB");
            Form1.motherdict.Add("VI", "US");
            Form1.motherdict.Add("WF", "FR");
            Form1.motherdict.Add("YT", "FR");
            Form1.motherdict.Add("SX", "NL");
            Form1.motherdict.Add("CC", "AU");
            Form1.motherdict.Add("BM", "GB");
            Form1.motherdict.Add("CW", "NL");
            Form1.motherdict.Add("GS", "GB");
            Form1.motherdict.Add("KY", "GB");
            Form1.motherdict.Add("NC", "FR");
            Form1.motherdict.Add("NF", "AU");
            Console.WriteLine("Form1.motherdict: " + Form1.motherdict.Count.ToString());
        }

        public static void read_country_info()
        {
            int n = 0;


            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "countryInfo.txt"))
            {
                int makelangcol = -1;
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();

                    if (line[0] == '#')
                        continue;

                    //if (n > 250)
                    //    Console.WriteLine(line);

                    string[] words = line.Split('\t');

                    //foreach (string s in words)
                    //    Console.WriteLine(s);

                    //Console.WriteLine(words[0] + "|" + words[1]);

                    if (words[0] == "ISO") //headline
                    {
                        for (int i = 1; i < words.Length; i++)
                        {
                            if (words[i] == Form1.makelang)
                                makelangcol = i;
                        }
                        continue;
                    }

                    int geonameid = -1;

                    countryclass country = new countryclass();

                    country.Name = words[4];
                    geonameid = util.tryconvert(words[16]);
                    country.iso = words[0];
                    country.iso3 = words[1];
                    country.isonumber = util.tryconvert(words[2]);
                    country.fips = words[3];
                    country.capital = words[5];
                    country.area = util.tryconvertdouble(words[6]);
                    country.population = util.tryconvertlong(words[7]);
                    country.continent = words[8];
                    country.tld = words[9];
                    country.currencycode = words[10];
                    country.currencyname = words[11];
                    country.phone = words[12];
                    country.postalcode = words[13];
                    foreach (string ll in words[15].Split(','))
                    {
                        //Console.WriteLine("ll.Split('-')[0] = " + ll.Split('-')[0]);
                        string lcode = ll.Split('-')[0];
                        if (String.IsNullOrEmpty(country.nativewiki))
                            country.nativewiki = lcode;
                        if (Form1.langtoint.ContainsKey(lcode))
                            country.languages.Add(Form1.langtoint[lcode]);
                    }
                    foreach (string ll in words[17].Split(','))
                        country.bordering.Add(ll);

                    if (makelangcol > 0)
                    {
                        country.Name_ml = words[makelangcol];
                    }
                    else
                    {
                        country.Name_ml = country.Name;
                    }
                    Form1.countryml.Add(country.Name, country.Name_ml);
                    Form1.countryiso.Add(country.Name, country.iso);

                    if (geonameid > 0)
                    {
                        Form1.countryid.Add(country.iso, geonameid);

                        Form1.countrydict.Add(geonameid, country);
                        //Console.WriteLine(country.iso+":"+geonameid.ToString());
                    }

                    n++;
                    if ((n % 10) == 0)
                    {
                        Console.WriteLine("n (country_info)   = " + n.ToString());

                    }

                }

                Console.WriteLine("n    (country_info)= " + n.ToString());

                if (Form1.savewikilinks)
                {
                    Page pt = new Page(Form1.makesite, util.mp(13) + Form1.botname + "/countrylinks");
                    pt.text = "Country links used by Lsjbot\n\n";
                    foreach (string cn in Form1.countryml.Keys)
                        pt.text += "*  [[" + Form1.countryml[cn] + "]]\n";
                    util.trysave(pt, 1, "Bot saving countrylinks");
                }



            }

            fill_motherdict();
            fill_nocapital();

#if (DBGEOFLAG)

            List<shapeclass> shapelist = read_shapelist(geonameclass.geonamesfolder + "ne_10m_admin_0_countries.multipoly.txt");
            foreach (shapeclass sc in shapelist)
            {
                if (sc.metadict.ContainsKey("iso_a2") & sc.metadict.ContainsKey("name"))
                {
                    //Console.WriteLine(sc.metadict["iso_a2"] + " " + sc.metadict["name"] + " " + sc.metadict["Centroid latitude"] + " " + sc.metadict["Centroid longitude"] + " " + sc.metadict["Areasum"]);
                    if (countryid.ContainsKey(sc.metadict["iso_a2"]))
                    {
                        int gnid = countryid[sc.metadict["iso_a2"]];
                        if (countrydict.ContainsKey(gnid))
                            countrydict[gnid].shape = sc;
                    }
                }

            }

            //write_shapelist("ne_10m_admin_0_countries.multipoly.txt",shapelist);

            //Console.ReadLine();
#endif
        }

        public static void fill_nocapital()
        {
            Form1.nocapital.Add("HK");
            Form1.nocapital.Add("IO");
            Form1.nocapital.Add("GI");
            Form1.nocapital.Add("MC");
            Form1.nocapital.Add("MO");
            Form1.nocapital.Add("SG");
            Form1.nocapital.Add("VA");
        }

        public static string countrytitle(int gnid)
        {
            string rs = "";
            string iso = "";
            if (Form1.countrydict.ContainsKey(gnid))
                iso = Form1.countrydict[gnid].iso;

            if (Form1.makelang != "sv")
                return rs;

            switch (iso)
            {
                case "MT":
                case "IE":
                case "IS":
                    rs = "republiken ";
                    break;
                case "FK":
                    rs = "territoriet ";
                    break;
                default:
                    rs = "";
                    break;

            }

            return rs;

        }

        public static string linkcountry(int gnid)
        {
            if (Form1.countrydict.ContainsKey(gnid))
                return linkcountry(Form1.countrydict[gnid].iso);
            else
                return "";
        }

        public static string linkcountry(string ciso)
        {
            if (!Form1.countryid.ContainsKey(ciso))
                return ciso;
            int gnid = Form1.countryid[ciso];
            string rt = "[[" + Form1.countryml[Form1.countrydict[gnid].Name] + "]]";
            if (Form1.motherdict.ContainsKey(ciso))
            {
                //int mothergnid = countryid[motherdict[ciso]];
                string mama = linkcountry(Form1.motherdict[ciso]);
                if (((Form1.motherdict[ciso] == "DK") || (Form1.motherdict[ciso] == "NL")) && (Form1.makelang == "sv"))
                    mama = mama.Replace("[[", "[[Kungariket ");
                rt += " (" + mama + ")";
            }

            return rt;
        }

        public static void get_country_iw(string langcode)
        {
            //Console.WriteLine("get country iw " + langcode);
            //using (StreamWriter sw = new StreamWriter("countrynames-" + langcode + ".csv"))
            //{

            //    foreach (int gnid in countrydict.Keys)
            //    {
            //        string langname = countrydict[gnid].Name;
            //        List<string> iwlist = Interwiki(wdsite, countrydict[gnid].Name);
            //        foreach (string iws in iwlist)
            //        {
            //            string[] ss = iws.Split(':');
            //            string iwcode = ss[0];
            //            string iwtitle = ss[1];
            //            //Console.WriteLine("iw - " + iwcode + ":" + iwtitle);
            //            if (iwcode == langcode)
            //                langname = iwtitle;
            //        }
            //        sw.WriteLine(countrydict[gnid].Name + ";" + langname);
            //        Console.WriteLine(countrydict[gnid].Name + ";" + langname);


            //    }
            //}
        }


    }
}
