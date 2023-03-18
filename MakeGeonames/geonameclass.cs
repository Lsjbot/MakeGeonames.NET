using DotNetWikiBot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    public class geonameclass
    {
        public static string geonamesfolder = @"G:\dotnwb3\Geonames\"; //will be modified according to computer name

        public int id = 0;
        public string Name = ""; //main name
        public string Name_ml = ""; //name in makelang language 
        public string name_lower = ""; //name in lower case
        public string asciiname = ""; //name in plain ascii
        public List<string> altnames = new List<string>(); //alternative names
        public double latitude = 0.0;
        public double longitude = 0.0;
        public char featureclass;
        public string featurecode;
        //public string countrycode = "XX"; //GeonameID of country in adm[0]!
        public List<int> altcountry = new List<int>();
        public List<int> children = new List<int>(); //index of children
        public List<int> features = new List<int>(); //index of features in adm-unit
        public int[] adm = new int[5]; // adm[0] = country, adm[1] = province, etc.
        public long population = 0; //pop according to geonames
        public long population_wd = 0; //pop according to wikipedia
        public string population_wd_iw = ""; //language from which pop is taken
        public int elevation = 0;
        public int elevation_vp = 0;
        public int prominence = 0;
        public double width = 0; //largest horizontal dimension in km
        public int island = 0; //if located on island, gnid of island
        public int inlake = 0; //if located in lake, gnid of lake
        public int inrange = 0; //if mountain part of range, gnid of range
        public List<int> atlakes = new List<int>(); //near the shore of lake(s) in list 
        public double area = 0.0; //area from wikipedia

        public int dem = 0;
        public string tz = "";
        public string moddate = "1901-01-01";

        public string articlename = ""; //article name, with disambiguation
        public string unfixedarticlename = ""; //article name before fixing oddities
        public string oldarticlename = ""; //article name in previous bot run
        public string artname2 = ""; //article name in other bot language, for interwiki linking
        public int wdid = -1; //wikidata id
        public bool roundminute = false; //coordinates rounded to whole minutes?


        public static Dictionary<int, geonameclass> get_gndict(string countrycode, Site makesite)
        {
            Dictionary<int, geonameclass> localgndict = new Dictionary<int, geonameclass>();
            int n = 0;
            int nbad = 0;
            int provinceless = 0;
            Console.WriteLine("read_geonames " + countrycode);

            string filename = geonamesfolder;
            if (countrycode == "")
                filename += "allCountries.txt";
            else
                filename += "Countries//" + countrycode + ".txt";

            Form1.gnfiledate = File.GetLastWriteTime(@filename);
            Form1.dumpdate = Form1.gnfiledate.ToString("yyyy-MM-dd");

            //for checkminutes:
            int ntot = 0;
            int nclosex = 0;
            int nclosey = 0;
            int ncloseboth = 0;
            Dictionary<string, int> ntotcountry = new Dictionary<string, int>();
            Dictionary<string, int> ncbcountry = new Dictionary<string, int>();
            Dictionary<string, int> ntotfcode = new Dictionary<string, int>();
            Dictionary<string, int> ncbfcode = new Dictionary<string, int>();
            double minutedist = 0.01;
            Page pmin = new Page(makesite, "Användare:Lsjbot/Minutavrundade");


            using (StreamReader sr = new StreamReader(filename))
            {
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();

                    if (line[0] == '#')
                        continue;

                    //if (n > 250)
                    //    Console.WriteLine(line);

                    string[] words = line.Split('\t');

                    bool badone = false;

                    //foreach (string s in words)
                    //    Console.WriteLine(s);

                    //Console.WriteLine(words[0] + "|" + words[1]);

                    int geonameid = -1;

                    geonameclass gn = new geonameclass();

                    words[1] = util.initialcap(words[1]);

                    gn.Name = words[1];
                    gn.Name_ml = words[1];
                    gn.name_lower = words[1].ToLower();
                    gn.articlename = "XXX";
                    geonameid = util.tryconvert(words[0]);
                    if (geonameid <= 0)
                        continue;
                    gn.id = geonameid;
                    gn.asciiname = words[2];
                    foreach (string ll in words[3].Split(','))
                        gn.altnames.Add(util.initialcap(ll));
                    gn.latitude = util.tryconvertdouble(words[4]);
                    gn.longitude = util.tryconvertdouble(words[5]);

                    if (words[6].Length > 0)
                        gn.featureclass = words[6][0];
                    gn.featurecode = words[7];

                    if (!Form1.featuredict.ContainsKey(gn.featurecode))
                        badone = true;

                    for (int ii = 0; ii < 4; ii++)
                        gn.adm[ii] = -1;
                    if (Form1.countryid.ContainsKey(words[8]))
                        gn.adm[0] = Form1.countryid[words[8]];
                    if (!Form1.altnamesonly)
                        foreach (string ll in words[9].Split(','))
                        {
                            if ((ll != words[8]) && (Form1.countryid.ContainsKey(ll)))
                                gn.altcountry.Add(Form1.countryid[ll]);
                        }
                    if (admclass.adm1dict.ContainsKey(words[8]))
                    {
                        if (admclass.adm1dict[words[8]].ContainsKey(words[10]))
                            gn.adm[1] = admclass.adm1dict[words[8]][words[10]];
                        else if (admclass.adm1dict[words[8]].ContainsKey("0" + words[10]))
                            gn.adm[1] = admclass.adm1dict[words[8]]["0" + words[10]];
                    }
                    if (gn.adm[1] <= 0)
                        provinceless++;

                    if (admclass.adm2dict.ContainsKey(words[8]))
                        if (admclass.adm2dict[words[8]].ContainsKey(words[10]))
                            if (admclass.adm2dict[words[8]][words[10]].ContainsKey(words[11]))
                                gn.adm[2] = admclass.adm2dict[words[8]][words[10]][words[11]];
                    gn.adm[3] = util.tryconvert(words[12]);
                    gn.adm[4] = util.tryconvert(words[13]);
                    for (int ii = 1; ii < 4; ii++)
                        if (gn.adm[ii] == geonameid)
                            gn.adm[ii] = -1;

                    gn.population = util.tryconvertlong(words[14]);
                    gn.elevation = util.tryconvert(words[15]);
                    gn.dem = util.tryconvert(words[16]);
                    if ((gn.elevation <= 0) && (gn.dem > 0))
                        gn.elevation = gn.dem;
                    gn.tz = words[17];
                    gn.moddate = words[18];

                    //if ((gn.featureclass == 'P') && (gn.population <= 0) && (!checkwikidata))
                    //    badone = true;

                    bool skipnoheight = false;
                    if (skipnoheight)
                    {
                        if ((gn.featureclass == 'T') && (gn.elevation <= 0) && (gn.dem <= 0))
                        {
                            switch (gn.featurecode)
                            {
                                case "HLL":
                                    badone = true;
                                    break;
                                case "HLLS":
                                    badone = true;
                                    break;
                                case "MT":
                                    badone = true;
                                    break;
                                case "MTS":
                                    badone = true;
                                    break;
                                case "MESA":
                                    badone = true;
                                    break;
                                case "MND":
                                    badone = true;
                                    break;
                                case "PK":
                                    badone = true;
                                    break;
                                case "PKS":
                                    badone = true;
                                    break;
                                case "RDGE":
                                    badone = true;
                                    break;
                                default:
                                    break;
                            }

                        }
                    }

                    if (gn.featurecode == "PPLC") //Capital
                    {
                        if (Form1.countrydict.ContainsKey(gn.adm[0]))
                        {
                            Form1.countrydict[gn.adm[0]].capital_gnid = geonameid;
                        }
                    }

                    if (Form1.statisticsonly)
                    {
                        //Console.WriteLine(gn.featurecode);
                        Form1.fchist.Add(gn.featurecode);
                        if (badone)
                            Form1.fcbad.Add(gn.featurecode);
                        Form1.fclasshist.Add(gn.featureclass);
                        if (!badone && Form1.categorydict.ContainsKey(gn.featurecode))
                            Form1.fcathist.Add(Form1.categorydict[gn.featurecode]);
                    }

                    //    public static Dictionary<string, List<int>> namefork = new Dictionary<string, List<int>>(); //names with list of corresponding geonameid(s)

                    if (!badone)
                    {
                        //if (altnamesonly)
                        //{
                        //    addnamefork(geonameid, gn.Name);
                        //    addnamefork(geonameid, gn.asciiname);
                        //    foreach (string nm in gn.altnames)
                        //        addnamefork(geonameid, nm);
                        //}

                        if (!Form1.altnamesonly)
                            Form1.addlatlong(gn.latitude, gn.longitude, geonameid);


                        double dlatmin = 60 * gn.latitude;
                        double dlonmin = 60 * gn.longitude;
                        double latdiff = Math.Abs(dlatmin - Convert.ToInt32(dlatmin));
                        double londiff = Math.Abs(dlonmin - Convert.ToInt32(dlonmin));
                        if (Form1.checkminutes)
                        {
                            ntot++;
                            if (!ntotcountry.ContainsKey(words[8]))
                            {
                                ntotcountry.Add(words[8], 0);
                                ncbcountry.Add(words[8], 0);
                            }
                            if (!ntotfcode.ContainsKey(words[7]))
                            {
                                ntotfcode.Add(words[7], 0);
                                ncbfcode.Add(words[7], 0);
                            }

                            ntotcountry[words[8]]++;
                            ntotfcode[words[7]]++;

                            if (latdiff < minutedist)
                                nclosey++;
                        }
                        if (londiff < minutedist)
                        {
                            nclosex++;
                            if (latdiff < minutedist)
                            {
                                if (Form1.checkminutes)
                                {
                                    ncloseboth++;
                                    ncbcountry[words[8]]++;
                                    ncbfcode[words[7]]++;
                                }
                                gn.roundminute = true;
                            }
                        }


                        if ((geonameid > 0) && (!badone))// && (!statisticsonly))
                        {
                            localgndict.Add(geonameid, gn);
                        }


                    }
                    else
                        nbad++;

                    n++;
                    if ((n % 10000) == 0)
                    {
                        Console.WriteLine("n    (geonames)   = " + n.ToString());
                        Console.WriteLine("nbad (geonames)   = " + nbad.ToString());
                        Console.WriteLine("provinceless = " + provinceless.ToString());
                        if (n >= Form1.maxread)
                            break;
                    }

                }

                Console.WriteLine("n    (geonames) = " + n.ToString());
                Console.WriteLine("nbad (geonames) = " + nbad.ToString());
                Console.WriteLine("provinceless = " + provinceless.ToString());


            }

            if (Form1.checkminutes)
            {
                Console.WriteLine("nclosex = " + nclosex);
                Console.WriteLine("nclosey = " + nclosey);
                Console.WriteLine("nclosebot = " + ncloseboth);
                Console.WriteLine("ntot = " + ntot);
                double ratio = ncloseboth;
                ratio = ratio / ntot;
                Console.WriteLine("ncloseboth/ntot (%) = " + 100 * ratio);

                pmin.text = "== Länder ==\n";
                foreach (string cc in ntotcountry.Keys)
                {
                    ratio = ncbcountry[cc];
                    ratio = ratio / ntotcountry[cc];
                    Console.WriteLine(cc + ": ncloseboth/ntot (%) = " + 100 * ratio);
                    pmin.text += "*" + cc + ": Andel minutavrundade (%) = " + (100 * ratio).ToString("N1") + "\n";
                }
                pmin.text += "\n== Platstyper ==\n";
                foreach (string cc in ntotfcode.Keys)
                {
                    ratio = ncbfcode[cc];
                    ratio = ratio / ntotfcode[cc];
                    if (ntotfcode[cc] > 100)
                        Console.WriteLine(cc + ": ncloseboth/ntot (%) = " + 100 * ratio);
                    pmin.text += "*" + cc + ": Andel minutavrundade (%) = " + (100 * ratio).ToString("N1") + "\n";
                }
                pmin.text += "\n== Vanligaste platstyperna ==\n";
                foreach (string cc in ntotfcode.Keys)
                {
                    if (ntotfcode[cc] < 10000)
                        continue;
                    ratio = ncbfcode[cc];
                    ratio = ratio / ntotfcode[cc];
                    Console.WriteLine(cc + ": ncloseboth/ntot (%) = " + 100 * ratio);
                    pmin.text += "*" + Form1.linkfeature(cc, -1) + ": Andel minutavrundade (%) = " + (100 * ratio).ToString("N1") + "\n";
                }
                util.trysave(pmin, 1, util.mp(302, null));
                Console.ReadLine();

            }

            return localgndict;

        }
        public static void read_geonames(string countrycode, Site makesite)
        {

            foreach (KeyValuePair<int,geonameclass> kp in get_gndict(countrycode,makesite))
            {
                Form1.gndict.Add(kp.Key, kp.Value);
            }
            if ((!Form1.verifywikidata) && (!Form1.checkwikidata))
                Form1.read_wd_files(countrycode); //files for individual countries, with pop and area

            if (Form1.checkwikidata)
                Form1.read_good_wd_file(); //file for all countries, with id match only

            if (Form1.makecountry == "CN")
                Form1.chinese_special();

            //clear away villages without population:
            if ((!Form1.verifywikidata) && (!Form1.checkwikidata))
            {
                List<int> ghosts = new List<int>();
                foreach (int gnid in Form1.gndict.Keys)
                {
                    if ((Form1.gndict[gnid].featureclass == 'P') && (Form1.gndict[gnid].population <= Form1.minimum_population) && (Form1.gndict[gnid].population_wd <= Form1.minimum_population))
                    {
                        if ((Form1.makecountry == "CN") && (Form1.chinese_pop_dict2.ContainsKey(gnid)))
                        {
                            //public class forkclass //class for entries in a fork page
                            //{
                            //    public int geonameid = 0;
                            //    public string featurecode = "";
                            //    public string[] admname = new string[3];
                            //    public double latitude = 0.0;
                            //    public double longitude = 0.0;
                            //    public string realname = "*"; 
                            //    public int wdid = -1;    //wikidata id
                            //    public string iso = "XX"; //country iso code
                            //    public string featurename = "";
                            //}

                            //public class Disambigclass //class for disambiguation in article names
                            //{
                            //    public bool existsalready = false;
                            //    public bool country = false;
                            //    public bool adm1 = false;
                            //    public bool adm2 = false;
                            //    public bool latlong = false;
                            //    public bool fcode = false;
                            //    public forkclass fork = new forkclass();
                            //}

                            Disambigclass da = new Disambigclass();
                            da.country = true;
                            da.adm1 = true;
                            da.adm2 = true;
                            da.latlong = true;
                            da.fcode = true;
                            da.fork.geonameid = gnid;
                            da.fork.featurecode = Form1.gndict[gnid].featurecode;
                            string countryname = Form1.countrydict[Form1.gndict[gnid].adm[0]].Name;
                            string countrynameml = countryname;
                            if (Form1.countryml.ContainsKey(countryname))
                                countrynameml = Form1.countryml[countryname];

                            da.fork.admname[0] = countrynameml;
                            if (Form1.gndict.ContainsKey(Form1.gndict[gnid].adm[1]))
                                da.fork.admname[1] = Form1.gndict[Form1.gndict[gnid].adm[1]].Name_ml;
                            else
                                da.fork.admname[1] = "";
                            if (Form1.gndict.ContainsKey(Form1.gndict[gnid].adm[2]))
                                da.fork.admname[2] = Form1.gndict[Form1.gndict[gnid].adm[2]].Name_ml;
                            else
                                da.fork.admname[2] = "";
                            da.fork.latitude = Form1.gndict[gnid].latitude;
                            da.fork.longitude = Form1.gndict[gnid].longitude;
                            da.fork.iso = Form1.makecountry;

                            Form1.gndict[gnid].articlename = Form1.gndict[gnid].Name_ml + " " + Form1.make_disambig(da, gnid);
                            Form1.gndict[gnid].population = Form1.chinese_pop_dict2[gnid].pop;
                            Form1.resurrected.Add(gnid);
                        }
                        else
                            ghosts.Add(gnid);
                    }
                    else if ((Form1.gndict[gnid].featurecode == "PPLQ") && (Form1.gndict[gnid].population > 0))
                        ghosts.Add(gnid);
                }

                foreach (int gnid in ghosts)
                {
                    existingclass gh = new existingclass();
                    gh.articlename = Form1.gndict[gnid].Name_ml;
                    gh.latitude = Form1.gndict[gnid].latitude;
                    gh.longitude = Form1.gndict[gnid].longitude;
                    if (!Form1.ghostdict.ContainsKey(gnid))
                        Form1.ghostdict.Add(gnid, gh);

                    Form1.gndict.Remove(gnid);
                }

                if (Form1.resurrected.Count > 0)
                {
                    using (StreamWriter sw = new StreamWriter("resurrected-" + Form1.makelang + ".txt"))
                    {

                        foreach (int gnid in Form1.resurrected)
                        {
                            sw.WriteLine(gnid.ToString() + "\t" + Form1.gndict[gnid].articlename);
                        }
                    }
                    Console.WriteLine(Form1.resurrected.Count + " resurrected.");
                    //Console.ReadLine();
                }
            }

            read_islands(countrycode); //which place is on which island?

            read_lakes(countrycode); //which place is in or near which lake?

            read_ranges(countrycode); //which mountain is in which mountain range?

#if (DBGEOFLAG)

            read_river_file(countrycode); //data for rivers
#endif

            read_altitudes(countrycode);

            //get proper country names
            foreach (int gnid in Form1.countrydict.Keys)
                if (Form1.gndict.ContainsKey(gnid))
                    Form1.gndict[gnid].Name_ml = Form1.countrydict[gnid].Name_ml;

            Console.WriteLine("read_geonames done");

        }

        public static void add_nameforks()
        {
            int ntot = Form1.gndict.Count;

            Console.WriteLine("Add_nameforks: " + ntot.ToString() + " to do.");

            foreach (int gnid in Form1.gndict.Keys)
            {
                Form1.addnamefork(gnid, Form1.gndict[gnid].Name);
                Form1.addnamefork(gnid, Form1.gndict[gnid].Name_ml);
                Form1.addnamefork(gnid, Form1.gndict[gnid].asciiname);

                if (Form1.altdict.ContainsKey(gnid))
                {
                    foreach (altnameclass ac in Form1.altdict[gnid])
                    {
                        Form1.addnamefork(gnid, ac.altname);
                    }
                }
                //else
                //{
                //    foreach (string nm in Form1.gndict[gnid].altnames)
                //        addnamefork(gnid, nm);
                //}

                //if (Form1.gndict[gnid].wdid > 0)
                //{
                //    XmlDocument cx = get_wd_xml(wdid);
                //    if (cx != null)
                //    {
                //        Dictionary<string, string> rd = get_wd_sitelinks(cx);
                //        foreach (string sw in rd.Keys)
                //            addnamefork(gnid, rd[sw]);
                //    }
                //}

                ntot--;
                if ((ntot % 1000) == 0)
                    Console.WriteLine("=== " + ntot.ToString() + " left ===");
            }
        }

        public static void read_island_file(string wdcountry)
        {
            string filename = geonamesfolder + @"islands\islands-" + wdcountry + ".txt";
            Form1.islanddict.Clear();
            try
            {
                int nislands = 0;
                using (StreamReader sr = new StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        string[] words = line.Split('\t');
                        //sw.Write(gnid.ToString() + "\t" + area.ToString() + "\t" + kmew.ToString() + "\t" + kmns.ToString());
                        //foreach (int oi in onisland)
                        //    sw.Write("\t" + oi.ToString());
                        //sw.WriteLine();

                        if (words.Length < 4)
                            continue;

                        int gnid = util.tryconvert(words[0]);
                        if (!Form1.gndict.ContainsKey(gnid))
                            continue;

                        double area = util.tryconvertdouble(words[1]);
                        if ((area > 0) && (Form1.gndict[gnid].area <= 0))
                            Form1.gndict[gnid].area = area;

                        islandclass isl = new islandclass();
                        isl.area = area;

                        double scale = Math.Cos(Form1.gndict[gnid].latitude * 3.1416 / 180);
                        double pixkmx = scale * 40000 / (360 * 1200);
                        double pixkmy = 40000.0 / (360.0 * 1200.0);

                        isl.kmew = util.tryconvertdouble(words[2]) + pixkmx;
                        isl.kmns = util.tryconvertdouble(words[3]) + pixkmy;
                        for (int i = 4; i < words.Length; i++)
                        {
                            int oi = util.tryconvert(words[i]);
                            if (Form1.gndict.ContainsKey(oi))
                            {
                                isl.onisland.Add(oi);
                                //if (Form1.gndict[oi].island <= 0)
                                //{
                                //    Form1.gndict[oi].island = gnid;
                                //}
                                //else //on two islands - error
                                //{
                                //    otherisland = Form1.gndict[oi].island;
                                //    Form1.gndict[oi].island = 0;
                                //}
                            }
                        }
                        //if (islanddict.ContainsKey(otherisland))
                        //    islanddict.Remove(otherisland);
                        //else
                        Form1.islanddict.Add(gnid, isl);
                        nislands++;
                    }
                }

                Console.WriteLine("# islands = " + nislands.ToString());

                Dictionary<int, int> oindex = new Dictionary<int, int>();
                //List<int> badlist = new List<int>();
                Dictionary<int, List<int>> badlist = new Dictionary<int, List<int>>();

                //identify stuff that's listed as on two islands:
                foreach (int gnid in Form1.islanddict.Keys)
                {
                    //first add island itself as "on" itself...
                    if (oindex.ContainsKey(gnid))
                    {
                        if (!badlist.ContainsKey(oindex[gnid]))
                        {
                            List<int> bl = new List<int>();
                            badlist.Add(oindex[gnid], bl);
                        }
                        if (!badlist[oindex[gnid]].Contains(gnid))
                            badlist[oindex[gnid]].Add(gnid);
                    }
                    else
                        oindex.Add(gnid, gnid);
                    //... then add everything else on the island.
                    foreach (int oi in Form1.islanddict[gnid].onisland)
                    {
                        if (oindex.ContainsKey(oi))
                        {
                            if (!badlist.ContainsKey(oindex[oi]))
                            {
                                List<int> bl = new List<int>();
                                badlist.Add(oindex[oi], bl);
                            }
                            if (!badlist[oindex[oi]].Contains(gnid))
                                badlist[oindex[oi]].Add(gnid);
                        }
                        else
                            oindex.Add(oi, gnid);

                    }
                }

                if (Form1.verifyislands) //Go through and find best island for stuff with double location,
                {                  //then make new island file.
                    int nbad = 0;

                    foreach (int badi in badlist.Keys)
                    {
                        long bestdist = Form1.seed_center_dist(badi, false);
                        long best2dist = 99999999;
                        int best = badi;
                        foreach (int badg in badlist[badi])
                        {
                            long scdist = Form1.seed_center_dist(badg, false);
                            if (scdist < bestdist)
                            {
                                best2dist = bestdist;
                                bestdist = scdist;
                                best = badg;
                            }
                        }

                        if (bestdist * 3 > best2dist) //require 3 times better to "promote" one of the islands
                            best = -1;

                        if (badi != best)
                        {
                            Form1.islanddict.Remove(badi);
                            nbad++;
                        }
                        foreach (int badg in badlist[badi])
                            if (badg != best)
                            {
                                Form1.islanddict.Remove(badg);
                                nbad++;
                            }
                    }

                    Console.WriteLine("# islands = " + nislands.ToString());
                    Console.WriteLine("# bad islands = " + nbad.ToString());
                    using (StreamWriter sw = new StreamWriter("islands-" + Form1.makecountry + ".txt"))
                    {

                        foreach (int gnid in Form1.islanddict.Keys)
                        {
                            sw.Write(gnid.ToString() + "\t" + Form1.islanddict[gnid].area.ToString() + "\t" + Form1.islanddict[gnid].kmew.ToString() + "\t" + Form1.islanddict[gnid].kmns.ToString());
                            foreach (int oi in Form1.islanddict[gnid].onisland)
                                sw.Write("\t" + oi.ToString());
                            sw.WriteLine();
                        }
                    }
                }
                else //just remove the duplicate islands
                {
                    foreach (int badi in badlist.Keys)
                    {
                        Form1.islanddict.Remove(badi);
                        foreach (int badg in badlist[badi])
                            Form1.islanddict.Remove(badg);
                    }
                }

                foreach (int gnid in Form1.islanddict.Keys)
                {
                    foreach (int oi in Form1.islanddict[gnid].onisland)
                    {
                        Form1.gndict[oi].island = gnid;
                    }
                }


            }
            catch (IOException e)
            {
                string message = e.Message;
                Console.Error.WriteLine(message);
            }
        }

        public static void read_range_file(string wdcountry)
        {
            string filename = geonamesfolder + @"ranges\ranges-" + wdcountry + ".txt";
            Form1.rangedict.Clear();
            try
            {
                int nranges = 0;
                using (StreamReader sr = new StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        string[] words = line.Split('\t');
                        //sw.Write(gnid.ToString() + "\t" + area.ToString() + "\t" + kmew.ToString() + "\t" + kmns.ToString());
                        //foreach (int oi in onrange)
                        //    sw.Write("\t" + oi.ToString());
                        //sw.WriteLine();

                        if (words.Length < 6)
                            continue;

                        int gnid = util.tryconvert(words[0]);
                        if (!Form1.gndict.ContainsKey(gnid))
                            continue;

                        double length = util.tryconvertdouble(words[1]);
                        rangeclass isl = new rangeclass();
                        isl.length = length;

                        double scale = Math.Cos(Form1.gndict[gnid].latitude * 3.1416 / 180);
                        double pixkmx = scale * 40000 / (360 * 1200);
                        //double pixkmy = 40000.0 / (360.0 * 1200.0);

                        //public class rangeclass //data for each MTS/HLLS
                        //{
                        //    public double length = 0;
                        //    public string orientation = "....";
                        //    public double angle = 0; //polar angle of long axis (radians). 0 or pi = EW, pi/2 or 3pi/2 = NS etc.
                        //    public double kmew = 0;
                        //    public double kmns = 0;
                        //    public int maxheight = 0; //highest point; gnid of peak if negative, height if positive
                        //    public double hlat = 999; //latitude/longitude of highest point
                        //    public double hlon = 999;
                        //    public List<int> inrange = new List<int>(); //list of GeoNames id of mountains in the range.
                        //}


                        isl.kmew = util.tryconvertdouble(words[2]);
                        isl.kmns = util.tryconvertdouble(words[3]);
                        isl.angle = util.tryconvertdouble(words[4]);
                        isl.maxheight = util.tryconvert(words[5]);
                        isl.hlat = util.tryconvertdouble(words[6]);
                        isl.hlon = util.tryconvertdouble(words[7]);

                        for (int i = 8; i < words.Length; i++)
                        {
                            int oi = util.tryconvert(words[i]);
                            if (Form1.gndict.ContainsKey(oi))
                            {
                                isl.inrange.Add(oi);
                            }
                        }
                        //if (rangedict.ContainsKey(otherrange))
                        //    rangedict.Remove(otherrange);
                        //else
                        Form1.rangedict.Add(gnid, isl);
                        nranges++;
                    }
                }

                Console.WriteLine("# ranges = " + nranges.ToString());


                foreach (int gnid in Form1.rangedict.Keys)
                {
                    foreach (int oi in Form1.rangedict[gnid].inrange)
                    {
                        Form1.gndict[oi].inrange = gnid;
                    }
                }


            }
            catch (IOException e)
            {
                string message = e.Message;
                Console.Error.WriteLine(message);
            }
        }

        public static void read_islands(string countrycode)
        {
            if (countrycode == "")
            {
                foreach (int gnid in Form1.countrydict.Keys)
                    read_island_file(Form1.countrydict[gnid].iso);
            }
            else
                read_island_file(countrycode);
        }

        public static void read_ranges(string countrycode)
        {
            if (countrycode == "")
            {
                foreach (int gnid in Form1.countrydict.Keys)
                    read_range_file(Form1.countrydict[gnid].iso);
            }
            else
                read_range_file(countrycode);
        }


        public static void read_lake_file(string wdcountry)
        {
            string filename = geonamesfolder + @"lakes\lakes-" + wdcountry + ".txt";
            Form1.lakedict.Clear();
            try
            {
                int nlakes = 0;
                using (StreamReader sr = new StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        string[] words = line.Split('\t');
                        //sw.Write(gnid.ToString() + "\t" + area.ToString() + "\t" + kmew.ToString() + "\t" + kmns.ToString());
                        //foreach (int oi in onisland)
                        //    sw.Write("\t" + oi.ToString());
                        //sw.WriteLine();

                        if (words.Length < 6)
                            continue;

                        int gnid = util.tryconvert(words[0]);
                        if (!Form1.gndict.ContainsKey(gnid))
                            continue;

                        double area = util.tryconvertdouble(words[1]);
                        if ((area > 0) && (Form1.gndict[gnid].area <= 0))
                            Form1.gndict[gnid].area = area;

                        lakeclass lake = new lakeclass();
                        lake.area = area;

                        //double scale = Math.Cos(Form1.gndict[gnid].latitude * 3.1416 / 180);
                        //double pixkmx = scale * 40000 / (360 * 1200);
                        //double pixkmy = 40000.0 / (360.0 * 1200.0);

                        lake.kmew = util.tryconvertdouble(words[2]);
                        lake.kmns = util.tryconvertdouble(words[3]);
                        //if (verifylakes)
                        //{
                        //    lake.kmew += pixkmx;
                        //    lake.kmns += pixkmy;
                        //}

                        lake.higher = util.tryconvert(words[4]);
                        lake.lower = util.tryconvert(words[5]);

                        if (words.Length < 9)
                            continue;
                        lake.overlaps_with = util.tryconvert(words[6]);

                        lake.glwd_id = util.tryconvert(words[7]);
                        lake.glwd_area = util.tryconvertdouble(words[8]);

                        int iw = 8;
                        while ((iw < words.Length) && (words[iw] != "around"))
                        {
                            int ii = util.tryconvert(words[iw]);
                            if (Form1.gndict.ContainsKey(ii))
                                lake.inlake.Add(ii);
                            iw++;
                        }
                        while (iw < words.Length)
                        {
                            int ii = util.tryconvert(words[iw]);
                            if (Form1.gndict.ContainsKey(ii))
                                lake.atlake.Add(ii);
                            iw++;
                        }

                        Form1.lakedict.Add(gnid, lake);
                        nlakes++;
                    }
                }

                Console.WriteLine("# lakes = " + nlakes.ToString());

                Dictionary<int, int> oindex = new Dictionary<int, int>();
                //List<int> badlist = new List<int>();
                Dictionary<int, List<int>> badlist = new Dictionary<int, List<int>>();

                //identify overlapping lakes, or stuff that's listed as in two lakes:
                foreach (int gnid in Form1.lakedict.Keys)
                {
                    //if (Form1.lakedict.ContainsKey(Form1.lakedict[gnid].overlaps_with))
                    //    if (Form1.lakedict[Form1.lakedict[gnid].overlaps_with].overlaps_with < 0)
                    //        Form1.lakedict[Form1.lakedict[gnid].overlaps_with].overlaps_with = gnid;

                    if (Form1.lakedict.ContainsKey(Form1.lakedict[gnid].overlaps_with))
                    {
                        if (!badlist.ContainsKey(gnid))
                        {
                            List<int> bl = new List<int>();
                            badlist.Add(gnid, bl);
                        }
                        if (!badlist[gnid].Contains(Form1.lakedict[gnid].overlaps_with))
                            badlist[gnid].Add(Form1.lakedict[gnid].overlaps_with);
                    }

                    //first add lake itself as "in" itself...
                    if (oindex.ContainsKey(gnid))
                    {
                        if (!badlist.ContainsKey(oindex[gnid]))
                        {
                            List<int> bl = new List<int>();
                            badlist.Add(oindex[gnid], bl);
                        }
                        if (!badlist[oindex[gnid]].Contains(gnid))
                            badlist[oindex[gnid]].Add(gnid);
                    }
                    else
                        oindex.Add(gnid, gnid);
                    //... then add everything else in the lake.
                    foreach (int oi in Form1.lakedict[gnid].inlake)
                    {
                        if (oindex.ContainsKey(oi))
                        {
                            if (!badlist.ContainsKey(oindex[oi]))
                            {
                                List<int> bl = new List<int>();
                                badlist.Add(oindex[oi], bl);
                            }
                            if (!badlist[oindex[oi]].Contains(gnid))
                                badlist[oindex[oi]].Add(gnid);
                        }
                        else
                            oindex.Add(oi, gnid);

                    }


                }

                if (Form1.verifylakes) //Go through and find best lake for stuff with double location,
                {                  //then make new lake file.
                    int nbad = 0;

                    foreach (int badi in badlist.Keys)
                    {
                        long bestdist = Form1.seed_center_dist(badi, true);
                        long best2dist = 99999999;
                        int best = badi;
                        foreach (int badg in badlist[badi])
                        {
                            long scdist = Form1.seed_center_dist(badg, true);
                            if (scdist < bestdist)
                            {
                                best2dist = bestdist;
                                bestdist = scdist;
                                best = badg;
                            }
                        }

                        if (bestdist * 3 > best2dist) //require 3 times better to "promote" one of the lakes
                            best = -1;

                        if (badi != best)
                        {
                            Form1.lakedict.Remove(badi);
                            nbad++;
                        }
                        foreach (int badg in badlist[badi])
                            if (badg != best)
                            {
                                Form1.lakedict.Remove(badg);
                                nbad++;
                            }
                    }

                    Console.WriteLine("# lakes = " + nlakes.ToString());
                    Console.WriteLine("# bad lakes = " + nbad.ToString());
                    using (StreamWriter sw = new StreamWriter("lakes-" + Form1.makecountry + ".txt"))
                    {

                        foreach (int gnid in Form1.lakedict.Keys)
                        {
                            Console.WriteLine(Form1.gndict[gnid].Name + "; " + Form1.lakedict[gnid].area.ToString() + "; " + Form1.lakedict[gnid].kmew.ToString() + "; " + Form1.lakedict[gnid].kmns.ToString() + "; " + Form1.lakedict[gnid].inlake.Count.ToString() + "; " + Form1.lakedict[gnid].atlake.Count.ToString());
                            //sw.Write(gnid.ToString() + "\t" + Form1.lakedict[gnid].area.ToString() + "\t" + Form1.lakedict[gnid].kmew.ToString() + "\t" + Form1.lakedict[gnid].kmns.ToString() + "\t" + "in");
                            sw.Write(gnid.ToString() + "\t" + Form1.lakedict[gnid].area.ToString() + "\t" + Form1.lakedict[gnid].kmew.ToString() + "\t" + Form1.lakedict[gnid].kmns.ToString() + "\t" + Form1.lakedict[gnid].higher.ToString() + "\t" + Form1.lakedict[gnid].lower.ToString() + "\t" + Form1.lakedict[gnid].overlaps_with + "\t" + Form1.lakedict[gnid].glwd_id.ToString() + "\t" + Form1.lakedict[gnid].glwd_area.ToString() + "\t" + "in");

                            foreach (int il in Form1.lakedict[gnid].inlake)
                            {
                                sw.Write("\t" + il.ToString());
                                //Console.WriteLine(Form1.gndict[il].Name + " in lake");
                            }
                            sw.Write("\t" + "around");
                            foreach (int al in Form1.lakedict[gnid].atlake)
                            {
                                sw.Write("\t" + al.ToString());
                                //Console.WriteLine(Form1.gndict[al].Name + " around lake");
                            }
                            sw.WriteLine();
                        }
                    }
                }
                else //just remove the duplicate lakes
                {
                    foreach (int badi in badlist.Keys)
                    {
                        Form1.lakedict.Remove(badi);
                        foreach (int badg in badlist[badi])
                            Form1.lakedict.Remove(badg);
                    }
                }


                foreach (int gnid in Form1.lakedict.Keys)
                {
                    foreach (int ii in Form1.lakedict[gnid].inlake)
                    {
                        Form1.gndict[ii].inlake = gnid;
                    }
                    foreach (int ai in Form1.lakedict[gnid].atlake)
                    {
                        Form1.gndict[ai].atlakes.Add(gnid);
                    }
                }


            }
            catch (IOException e)
            {
                string message = e.Message;
                Console.Error.WriteLine(message);
            }
        }

        public static void read_lakes(string countrycode)
        {
            if (countrycode == "")
            {
                foreach (int gnid in Form1.countrydict.Keys)
                    read_lake_file(Form1.countrydict[gnid].iso);
            }
            else
                read_lake_file(countrycode);
        }

        public static void read_altitude_file(string wdcountry)
        {
            string filename = geonamesfolder + @"altitudes\altitude-" + wdcountry + ".txt";
            try
            {
                int nalt = 0;
                using (StreamReader sr = new StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        string[] words = line.Split('\t');

                        if (words.Length < 2)
                            continue;

                        int gnid = util.tryconvert(words[0]);
                        if (!Form1.gndict.ContainsKey(gnid))
                            continue;

                        int altitude = util.tryconvert(words[1]);

                        if (altitude < 9000)
                        {
                            Form1.gndict[gnid].elevation_vp = altitude;
                            if (Form1.gndict[gnid].elevation <= 0)
                                Form1.gndict[gnid].elevation = altitude;
                        }
                        nalt++;
                    }
                }

                Console.WriteLine("# altitudes = " + nalt.ToString());



            }
            catch (IOException e)
            {
                string message = e.Message;
                Console.Error.WriteLine(message);
            }
        }

        public static void read_altitudes(string countrycode)
        {
            if (countrycode == "")
            {
                foreach (int gnid in Form1.countrydict.Keys)
                    read_altitude_file(Form1.countrydict[gnid].iso);
            }
            else
                read_altitude_file(countrycode);
        }

        public static void read_altnames()
        {
            int n = 0;
            int nbad = 0;
            Console.WriteLine("read_altnames");
            string filename = geonamesfolder + "alternateNames.txt";

            using (StreamReader sr = new StreamReader(filename))
            {

                //public class altnameclass
                //{
                //    public int altid = 0;
                //    public string altname = "";
                //    public int ilang = 0;
                //    public string wikilink = "";
                //    public bool official = false;
                //    public bool shortform = false;
                //    public bool colloquial = false;
                //    public bool historic = false;
                //}

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();

                    bool goodname = false;

                    string[] words = line.Split('\t');

                    altnameclass an = new altnameclass();

                    an.altid = util.tryconvert(words[0]);

                    int gnid = util.tryconvert(words[1]);

                    if (!Form1.checkdoubles && !Form1.gndict.ContainsKey(gnid))
                        continue;

                    int country = -1;
                    if (Form1.gndict.ContainsKey(gnid))
                        country = Form1.gndict[gnid].adm[0];

                    //if (gnid == 3039154)
                    //    Console.WriteLine(line);

                    if (words[2] == "link")
                    {
                        an.wikilink = words[3];
                        goodname = true;
                    }
                    else if (words[2] == "iata")
                    {
                        if (!Form1.iatadict.ContainsKey(gnid))
                            Form1.iatadict.Add(gnid, words[3]);
                    }
                    else if (words[2] == "icao")
                    {
                        if (!Form1.icaodict.ContainsKey(gnid))
                            Form1.icaodict.Add(gnid, words[3]);
                    }
                    else
                    {
                        an.altname = util.initialcap(words[3]);

                        if (Form1.langtoint.ContainsKey(words[2]))
                            an.ilang = Form1.langtoint[words[2]];
                        else if (Form1.langtoint.ContainsKey(words[2].Split('-')[0]))
                            an.ilang = Form1.langtoint[words[2].Split('-')[0]];
                        else if (words[2].Length > 3)
                        {
                            an.ilang = -1;
                            continue;
                        }
                        //Console.WriteLine("lang:" + an.ilang.ToString() + ", " + words[2]);


                        if (words[2] == Form1.makelang)
                        {
                            if (Form1.gndict.ContainsKey(gnid))
                                if (!Form1.gndict[gnid].articlename.Contains("*"))
                                    Form1.gndict[gnid].Name_ml = words[3];
                        }

                        if (Form1.countrydict.ContainsKey(country))
                            if (Form1.countrydict[country].languages.Contains(an.ilang))
                                goodname = true;

                        string script = util.get_alphabet(an.altname);

                        if ((an.ilang < 0) && (script == "latin"))
                            goodname = true;

                        if (((Form1.makecountry == "CN") || (Form1.makecountry == "JP") || (Form1.makecountry == "TW")) && (script == "chinese/japanese"))
                            goodname = true;

                    }




                    if ((words.Length > 4) && (words[4] == "1"))
                        an.official = true;
                    if ((words.Length > 5) && (words[5] == "1"))
                        an.shortform = true;
                    if ((words.Length > 6) && (words[6] == "1"))
                        an.colloquial = true;
                    if ((words.Length > 7) && (words[7] == "1"))
                        an.historic = true;

                    //if (an.official || an.shortform || an.colloquial || an.historic)
                    //    goodname = true;

                    //if (langdict.ContainsKey(an.ilang))
                    //    Console.Write(langdict[an.ilang].iso2 + ":");
                    //else
                    //    Console.Write("--:");
                    //Console.WriteLine(an.altname + ": is_latin = " + is_latin(an.altname).ToString()+", goodname = "+goodname.ToString());

                    if (goodname)
                    {
                        if (!Form1.altdict.ContainsKey(gnid))
                        {
                            List<altnameclass> anl = new List<altnameclass>();
                            Form1.altdict.Add(gnid, anl);
                        }
                        Form1.altdict[gnid].Add(an);
                        if (!String.IsNullOrEmpty(an.altname))
                            n++;
                    }
                    else
                        nbad++;


                    if ((n % 1000000) == 0)
                    {
                        Console.WriteLine("n (altnames)   = " + n.ToString());
                        if (n >= 1000000000)
                            break;
                    }

                }

                Console.WriteLine("n    (altnames)= " + n.ToString());
                Console.WriteLine("nbad (altnames)= " + nbad.ToString());

                if (Form1.statisticsonly)
                {
                    hbookclass althist = new hbookclass("Alternate names");
                    hbookclass altuniquehist = new hbookclass("Unique alternate names");

                    foreach (int gnid in Form1.altdict.Keys)
                    {
                        althist.Add(Form1.altdict[gnid].Count);
                        List<string> al = new List<string>();
                        foreach (altnameclass ac in Form1.altdict[gnid])
                        {
                            if (!al.Contains(ac.altname))
                                al.Add(ac.altname);
                        }
                        altuniquehist.Add(al.Count);
                        if (al.Count > 100)
                        {
                            Console.Write(al.Count.ToString() + ": ");
                            foreach (string s in al)
                                Console.Write(s + " | ");
                            Console.WriteLine();
                        }
                    }

                    althist.PrintIHist();
                    altuniquehist.PrintIHist();

                    Console.ReadLine();
                }
            }

            if ((Form1.makelang == "sv") || (Form1.makelang == "no"))
            {
                if (Form1.makecountry == "")
                {
                    transliterationclass.read_translit("BG");
                    transliterationclass.read_translit("BY");
                    transliterationclass.read_translit("KZ");
                    transliterationclass.read_translit("MK");
                    transliterationclass.read_translit("RS");
                    transliterationclass.read_translit("RU");
                    transliterationclass.read_translit("UA");
                    transliterationclass.read_translit("MN");
                    transliterationclass.read_translit("KG");
                    transliterationclass.read_translit("TJ");
                    transliterationclass.read_translit("RS");

                }
                else
                    transliterationclass.read_translit(Form1.makecountry);
            }

            read_handtranslated();

        }

        public static void read_handtranslated()
        {

            string filename = geonamesfolder + @"handtranslated-" + Form1.makelang + ".txt";
            int n = 0;
            if (File.Exists(filename))
            {
                Console.WriteLine("read_handtranslated " + filename);
                using (StreamReader sr = new StreamReader(filename))
                {

                    //public class altnameclass
                    //{
                    //    public int altid = 0;
                    //    public string altname = "";
                    //    public int ilang = 0;
                    //    public string wikilink = "";
                    //    public bool official = false;
                    //    public bool shortform = false;
                    //    public bool colloquial = false;
                    //    public bool historic = false;
                    //}

                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();

                        //bool goodname = false;

                        string[] words = line.Split('\t');

                        if (words.Length < 2)
                            continue;

                        altnameclass an = new altnameclass();

                        an.altid = -1;

                        int gnid = util.tryconvert(words[0]);

                        if (!Form1.checkdoubles && !Form1.gndict.ContainsKey(gnid))
                            continue;

                        an.altname = util.remove_disambig(words[1].Replace("*", ""));

                        an.ilang = Form1.langtoint[Form1.makelang];

                        if (Form1.gndict.ContainsKey(gnid))
                        {
                            Form1.gndict[gnid].Name_ml = an.altname;
                            if (words[1].Contains("*"))
                                Form1.gndict[gnid].articlename = words[1];
                        }

                        //else
                        {
                            if (!Form1.artnamedict.ContainsKey(gnid))
                                Form1.artnamedict.Add(gnid, words[1]);
                            else if (!Form1.artnamedict[gnid].Contains("*"))
                                Form1.artnamedict[gnid] = words[1];

                        }




                        if (!Form1.altdict.ContainsKey(gnid))
                        {
                            List<altnameclass> anl = new List<altnameclass>();
                            Form1.altdict.Add(gnid, anl);
                        }
                        Form1.altdict[gnid].Add(an);
                        if (!String.IsNullOrEmpty(an.altname))
                            n++;

                    }
                }
                Console.WriteLine("Names found in handtranslated: " + n.ToString());
            }
            else
                Console.WriteLine("File not found! " + filename);
        }


    }
}
