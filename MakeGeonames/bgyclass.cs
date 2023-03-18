using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace MakeGeonames
{
    public class bgyclass
    {
        public static Dictionary<string, bgyclass> bgydict = new Dictionary<string, bgyclass>();
        public static Dictionary<string, int> levelcode = new Dictionary<string, int>() {
            {"Reg",0 },
            {"Prov",1 },
            {"",1 },
            {"Dist",1 },
            {"Mun",2 },
            {"SubMun",2 },
            {"SGU",2 },
            {"City",2 },
            {"Bgy",3 }
        };

        public string psgc10 = "";
        public string name = "";
        public string origname = "";
        public string oldname = "";
        public string corrcode = "";
        public string level = "";
        public int intlevel = -1;
        public string cityclass = "";
        public int incomeclass = -1;
        public bool rural = true;
        public int pop2015 = -1;
        public int pop2020 = -1;
        public string status = "";
        //string region = ""; //psgc code of region
        //string province = ""; //psgc code of province
        //string municipality = ""; //psgc code of municipality
        public string[] parent = new string[3] { "", "", "" }; //0=region, 1=province, 2 = municipality
        public int gnid = -1;
        public string language = "?";
        public List<string> dirlanguage = new List<string>();
        public List<string> category = new List<string>();

        public static string saintstring = "Sacral name";
        public static string personstring = "Person";
        public static string spainstring = "Placename from Spain";
        public static string englandstring = "Placename from UK";
        public static string eswordstring = "Word in Spanish";
        public static string tlwordstring = "Word in Tagalog";
        public static string cebwordstring = "Word in Cebuano";
        public static string enwordstring = "Word in English";

        public static int maxaffix = 6;
        public static Dictionary<string, int>[] prefixdict = new Dictionary<string, int>[maxaffix];
        public static Dictionary<string, int>[] suffixdict = new Dictionary<string, int>[maxaffix];

        public Tuple<double,double> latlong(Dictionary<int,geonameclass> gndict)
        {
            if (gnid > 0)
            {
                return new Tuple<double, double>(gndict[gnid].latitude, gndict[gnid].longitude);
            }
            else
            {
                if (intlevel == 0)
                    return null;
                else
                {
                    var ll = bgydict[this.parent[intlevel-1]].latlong(gndict);
                    if (ll != null)
                    {
                        ll = latlongjitter(ll);
                    }
                    return ll;
                }
            }
        }

        static Random rnd = new Random();

        public Tuple<double,double> latlongjitter(Tuple<double, double> ll)
        {
            double[] deglevel = new double[] { 2, 0.5, 0.1, 0.03 };
            double deg = deglevel[intlevel];

            double jlat = rnd.NextDouble() - 0.5;
            double jlon = rnd.NextDouble() - 0.5;

            return new Tuple<double,double>(ll.Item1 + jlat * deg,ll.Item2+jlon*deg);
        }

        public string getregion()
        {
            return bgydict[parent[0]].name;
        }

        public static void MatchGeoNames(Dictionary<int,geonameclass> gndict)
        {
            string regrex = @"\((.+) region\)";
            string regrex2 = @"\((.+)\)";
            int maxlevel = 2;
            for (int jlevel = 0;jlevel<=maxlevel;jlevel++)
            {
                int nfail = 0;
                var q = from c in bgydict.Values where c.intlevel == jlevel select c;
                string fc = "ADM" + (jlevel + 1);
                var qgn = from c in gndict.Values where c.featurecode == fc select c;
                foreach (bgyclass bc in q)
                {
                    string gname = bc.name;
                    if (jlevel == 0)
                    {
                        foreach (Match m in Regex.Matches(gname, regrex))
                            gname = m.Groups[1].Value;
                        if (gname == bc.name)
                        {
                            foreach (Match m in Regex.Matches(gname, regrex2))
                            {
                                if (m.Groups[1].Value.Length > 3)
                                    gname = m.Groups[1].Value;
                                else
                                    gname = gname.Replace(m.Value, "").Trim();
                            }
                        }
                        if (gname == bc.name)
                            gname = gname.Replace("region", "").Trim();
                        if (gname == "barmm")
                        {
                            bc.gnid = 8367019;
                            continue;
                        }
                    }
                    else if (jlevel == 1)
                    {
                        gname = "province of " + gname;
                    }
                    else if (jlevel == 2)
                    {
                        if (gname == "city of cotabato")
                            gname = "cotabato city";
                        else if (gname == "sultan kudarat")
                            gname += " (nuling)";
                        else if (gname == "datu odin sinsuat")
                            gname += " (dinaig)";
                        else if (gname == "shariff aguak")
                            gname += " (maganoy)";
                        else if (gname == "sultan sa barongis")
                            gname += " (lambayong)";
                        else if (gname == "parang")
                        {
                            bc.gnid = 1694777;
                            continue;
                        }
                        else if (gname == "datu saudi ampatuan")
                        {
                            bc.gnid = 11778375;
                            continue;
                        }
                    }
                    string gnameic = util.initialcap(gname);
                    var qm = from c in qgn where (c.name_lower == gname || c.name_lower == bc.name) select c;
                    if (qm.Count() == 0)
                    {
                        qm = from c in qgn where c.altnames.Contains(gnameic) select c;
                    }
                    if (qm.Count() == 1)
                    {
                        bc.gnid = qm.First().id;
                    }
                    else if (qm.Count() > 1)
                    {
                        if (jlevel < 2)
                            Console.WriteLine("Multiple " + bc.name);
                        nfail++;
                    }
                    else if (qm.Count() == 0)
                    {
                        if (jlevel < 2)
                            Console.WriteLine("Not found " + gname);
                        nfail++;
                    }
                }
                Console.WriteLine("jlevel=" + jlevel + ", nfail = " + nfail);
            }
        }

        public static void fillaffix(string name)
        {
            if (prefixdict[0] == null)
            {
                for (int i = 0; i < maxaffix; i++)
                    prefixdict[i] = new Dictionary<string, int>();
            }
            char[] cc = (name+"#").ToCharArray();
            for (int i = 0; i < Math.Min(maxaffix, name.Length); i++)
            {
                string pf = name.Substring(0, i+1);
                if (!prefixdict[i].ContainsKey(pf))
                    prefixdict[i].Add(pf, 1);
                else
                    prefixdict[i][pf]++;
            }
            if (suffixdict[0] == null)
            {
                for (int i = 0; i < maxaffix; i++)
                    suffixdict[i] = new Dictionary<string, int>();
            }
            //char[] cc = (name + "#").ToCharArray();
            for (int i = 0; i < Math.Min(maxaffix, name.Length); i++)
            {
                string pf = name.Substring(name.Length-i-1, i + 1);
                if (!suffixdict[i].ContainsKey(pf))
                    suffixdict[i].Add(pf, 1);
                else
                    suffixdict[i][pf]++;
            }
        }

        public static List<string> topaffix(int afflength, int naffix)
        {
            List<string> ls = new List<string>();
            var q = (from c in prefixdict[afflength-1] select c).OrderByDescending(c => c.Value);

            int nw = 0;
            foreach (var cq in q)
            {
                string s1 = cq.Key.Substring(0, cq.Key.Length - 1);
                string frac = "";
                if (prefixdict[afflength - 2].ContainsKey(s1))
                    frac = ((100*cq.Value) / prefixdict[afflength - 2][s1]).ToString();
                ls.Add(cq.Key + "-\t" + cq.Value+"\t"+frac);
                nw++;
                if (nw > naffix)
                    break;
            }

            ls.Add("-------\t'-------");
            var qs = (from c in suffixdict[afflength - 1] select c).OrderByDescending(c => c.Value);

            nw = 0;
            foreach (var cq in qs)
            {
                string s1 = cq.Key.Substring(1, cq.Key.Length - 1);
                string frac = "";
                if (suffixdict[afflength - 2].ContainsKey(s1))
                    frac = ((100 * cq.Value) / suffixdict[afflength - 2][s1]).ToString();
                ls.Add("-"+cq.Key + "\t" + cq.Value + "\t" + frac);
                nw++;
                if (nw > naffix)
                    break;
            }

            return ls;
        }
        public static void read_bgyfile()
        {
            string fn = geonameclass.geonamesfolder + "PSGC-3Q-2022-Publication-Datafile.txt";
            Console.WriteLine("Reading " + fn);
            using (StreamReader sr = new StreamReader(fn))
            {
                sr.ReadLine();
                int nline = 0;

                string[] currentparent = new string[3] { "", "", "" };
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    nline++;

                    bgyclass bgy = new bgyclass();
                    bgy.psgc10 = words[0];
                    bgy.origname = words[1];
                    bgy.name = bgy.origname.ToLower().Trim();
                    bgy.oldname = words[4];
                    bgy.corrcode = words[2];
                    if (String.IsNullOrEmpty(bgy.psgc10))
                        bgy.psgc10 = bgy.corrcode;
                    bgy.level = words[3];
                    bgy.intlevel = levelcode[bgy.level];
                    if (bgy.intlevel < 3)
                    {
                        currentparent[bgy.intlevel] = bgy.psgc10;
                    }
                    if (bgy.intlevel > 0)
                    {
                        for (int i = 0; i < bgy.intlevel; i++)
                        {
                            bgy.parent[i] = currentparent[i];
                        }
                    }
                    bgy.cityclass = words[5];
                    if (!String.IsNullOrEmpty(words[6]))
                        bgy.incomeclass = util.tryconvert(words[6].Substring(0, 1));
                    bgy.rural = (words[7].Trim() == "R");
                    bgy.pop2015 = util.tryconvert(words[8].Replace(" ",""));
                    bgy.pop2020 = util.tryconvert(words[10].Replace(" ", ""));
                    if (words.Length > 12)
                        bgy.status = words[12];

                    bgydict.Add(bgy.psgc10,bgy);
                }

                Console.WriteLine(nline + " lines fread from " + fn);
            }

        }

        public static Dictionary<string, int> dirlist = new Dictionary<string, int>();
        public static Dictionary<string, string> dirlangdict = new Dictionary<string, string>();
        public static Dictionary<string, int> herolist = new Dictionary<string, int>();
        public static Dictionary<string, int> saintlist = new Dictionary<string, int>();
        public static Dictionary<string, int> titlelist = new Dictionary<string, int>();
        public static Dictionary<string, int> poblist = new Dictionary<string, int>();
        public static Dictionary<string, string> dirlang = new Dictionary<string, string>();
        public static void fill_lists()
        {
            dirlist.Add("north", 0);
            dirlist.Add("south", 0);
            dirlist.Add("east", 0);
            dirlist.Add("west", 0);
            dirlist.Add("northern", 0);
            dirlist.Add("southern", 0);
            dirlist.Add("eastern", 0);
            dirlist.Add("western", 0);
            dirlist.Add("norte", 0);
            dirlist.Add("sur", 0);
            dirlist.Add("este", 0);
            dirlist.Add("oeste", 0);
            dirlist.Add("upper", 0);
            dirlist.Add("lower", 0);
            dirlist.Add("central", 0);
            dirlist.Add("centro", 0);
            dirlist.Add("occidental", 0);
            dirlist.Add("oriental", 0);
            dirlist.Add("mataas", 0); //hög tagalog
            dirlist.Add("itaas", 0); //övre tagalog
            dirlist.Add("ibaba", 0); //nedre tagalog
            dirlist.Add("labak", 0); //dal tagalog
            dirlist.Add("ilaya", 0); //norra/inre/övre/uppströms tagalog
            dirlist.Add("iraya", 0); //norra/inre/övre/uppströms bikol
            dirlist.Add("silangan", 0); //östra tagalog
            dirlist.Add("kanluran", 0); //västra tagalog
            dirlist.Add("ilawod", 0); //yttre/nedströms bikol
            dirlist.Add("malaki", 0); //stor tagalog
            dirlist.Add("maliit", 0); //liten tagalog
            dirlist.Add("munti", 0); //liten tagalog
            dirlist.Add("dako", 0); //stor cebuano
            //dirlist.Add("Dakong", 0); //stor cebuano
            dirlist.Add("gamay", 0); //liten cebuano
            dirlist.Add("bago", 0); //ny tagalog
            dirlist.Add("bagong", 0); //ny tagalog
            dirlist.Add("bag-o", 0); //ny cebuano
            dirlist.Add("bag-ong", 0); //ny cebuano
            //dirlist.Add("Bag-Ong", 0); //ny tagalog
            dirlist.Add("new", 0);
            dirlist.Add("old", 0);
            dirlist.Add("nueva", 0);
            dirlist.Add("nuevo", 0);
            dirlist.Add("vieja", 0);
            dirlist.Add("viejo", 0);
            dirlist.Add("dacu", 0); //dako?
            dirlist.Add("cerca", 0);
            dirlist.Add("lejos", 0);
            dirlist.Add("grande", 0);
            dirlist.Add("pequeño", 0);
            dirlist.Add("pequeña", 0);
            dirlist.Add("chica", 0);
            dirlist.Add("chico", 0);
            dirlist.Add("mar", 0);
            dirlist.Add("monte", 0);
            dirlist.Add("ubos", 0); //nedre cebuano
            //dirlist.Add("purok", 0); //distrikt, tl?
            //dirlist.Add("zone", 0);
            dirlist.Add("mayor", 0);
            dirlist.Add("menor", 0);
            //dirlist.Add("cerca", 0);
            //dirlist.Add("lejos", 0);
            //dirlist.Add("central", 0);
            dirlist.Add("baja", 0);
            dirlist.Add("alta", 0);
            dirlist.Add("bajo", 0);
            dirlist.Add("abajo", 0);
            dirlist.Add("abaja", 0);
            dirlist.Add("alto", 0);
            dirlist.Add("la", 0);
            dirlist.Add("las", 0);
            dirlist.Add("los", 0);
            dirlist.Add("el", 0);
            dirlist.Add("de", 0);
            dirlist.Add("del", 0);
            dirlist.Add("ni", 0);
            dirlist.Add("sa", 0);
            dirlist.Add("ng", 0);
            dirlist.Add("of", 0);
            dirlist.Add("exterior", 0);
            dirlist.Add("interior", 0);
            dirlist.Add("coastal", 0);
            dirlist.Add("pulo", 0);
            dirlist.Add("island", 0);
            dirlist.Add("isla", 0);
            dirlist.Add("large", 0);
            dirlist.Add("big", 0);
            dirlist.Add("small", 0);
            dirlist.Add("little", 0);
            //dirlist.Add("", 0);
            //dirlist.Add("", 0);


            herolist.Add("rizal", 0);
            herolist.Add("aguinaldo", 0);
            herolist.Add("quezon", 0);
            herolist.Add("laurel", 0);
            herolist.Add("osmeña", 0);
            herolist.Add("roxas", 0);
            herolist.Add("quirino", 0);
            herolist.Add("magsaysay", 0);
            herolist.Add("garcia", 0);
            herolist.Add("garcía", 0);
            herolist.Add("macapagal", 0);
            herolist.Add("marcos", 0);
            herolist.Add("aquino", 0);
            herolist.Add("ramos", 0);
            herolist.Add("estrada", 0);
            herolist.Add("arroyo", 0);
            herolist.Add("duterte", 0);
            herolist.Add("bonifacio", 0);
            herolist.Add("macabulos", 0);
            herolist.Add("malvar", 0);
            //herolist.Add("sakay", 0);
            //herolist.Add("santos", 0);
            //herolist.Add("luna", 0);
            herolist.Add("legaspi", 0);
            herolist.Add("rivera", 0);
            herolist.Add("magallanes", 0);
            herolist.Add("magellan", 0);
            herolist.Add("columbus", 0);
            herolist.Add("colon", 0);
            herolist.Add("cervantes", 0);
            herolist.Add("peñarada", 0);
            herolist.Add("urdaneta", 0);
            herolist.Add("cortes", 0);
            herolist.Add("macarthur", 0);
            herolist.Add("washington", 0);
            herolist.Add("roosevelt", 0);
            herolist.Add("mckinley", 0);
            //spanish missionaries etc
            herolist.Add("hernandez", 0);
            herolist.Add("jimenez", 0);
            herolist.Add("fermin", 0);
            herolist.Add("polanco", 0);
            herolist.Add("loyola", 0);
            herolist.Add("villaverde", 0);
            //spanish royalty
            herolist.Add("isabela", 0);
            herolist.Add("alfonso", 0);
            herolist.Add("amadeo", 0);
            herolist.Add("mercedes", 0);
            herolist.Add("borbon", 0);
            herolist.Add("ramirez", 0);
            //first ladies
            herolist.Add("imelda", 0);
            herolist.Add("alicia", 0);
            herolist.Add("remedios", 0);
            herolist.Add("josefina", 0);
            herolist.Add("victoria", 0);
            //governor-generals
            herolist.Add("alaminos", 0);
            herolist.Add("dasmariñas", 0);
            herolist.Add("muñoz", 0);
            herolist.Add("aguilar", 0);
            herolist.Add("alcala", 0);
            herolist.Add("anda", 0);
            herolist.Add("barbaza", 0);
            herolist.Add("basco", 0);
            herolist.Add("claveria", 0);
            herolist.Add("echague", 0);
            herolist.Add("gandara", 0);
            herolist.Add("jovellar", 0);
            herolist.Add("lavezares", 0);
            herolist.Add("obando", 0);
            herolist.Add("solano", 0);
            herolist.Add("valderrama", 0);
            herolist.Add("villasis", 0);
            //national heroes
            herolist.Add("agoncillo", 0);
            herolist.Add("garchitorena", 0);
            herolist.Add("mamerto", 0);
            herolist.Add("alzarez", 0);
            herolist.Add("tinio", 0);
            herolist.Add("trias", 0);
            herolist.Add("llanera", 0);
            herolist.Add("dalman", 0);
            herolist.Add("jaena", 0);
            //herolist.Add("mendez", 0);
            //herolist.Add("plaridel", 0);
            //herolist.Add("bugallon", 0);
            //herolist.Add("", 0);
            //herolist.Add("", 0);
            //herolist.Add("", 0);

            saintlist.Add("santo", 0);
            saintlist.Add("santa", 0);
            saintlist.Add("san", 0);
            saintlist.Add("sto.", 0);
            saintlist.Add("sta.", 0);
            saintlist.Add("virgen", 0);
            saintlist.Add("santisima", 0);
            saintlist.Add("saint", 0);
            saintlist.Add("nstra.", 0);
            saintlist.Add("angeles", 0);
            saintlist.Add("martires", 0);
            saintlist.Add("milagros", 0);
            saintlist.Add("salvador", 0);
            saintlist.Add("natividad", 0);
            saintlist.Add("trinidad", 0);
            saintlist.Add("consolacion", 0);
            //saintlist.Add("", 0);

            titlelist.Add("padre", 0);
            titlelist.Add("general", 0);
            titlelist.Add("gen.", 0);
            titlelist.Add("president", 0);
            titlelist.Add("pres.", 0);
            titlelist.Add("don", 0);
            titlelist.Add("doña", 0);
            titlelist.Add("capitan", 0);
            titlelist.Add("dean", 0);
            titlelist.Add("datu", 0);
            //titlelist.Add("", 0);

            poblist.Add("poblacion", 0);
            poblist.Add("pob.", 0);
            //poblist.Add("Centro", 0);
            poblist.Add("district", 0);
            poblist.Add("village", 0);
            poblist.Add("zone", 0);
            poblist.Add("purok", 0);
            poblist.Add("proper", 0);
            poblist.Add("barangay", 0);
            poblist.Add("bgy.", 0);
            poblist.Add("no.", 0);
            poblist.Add("villa", 0);
            poblist.Add("subdivision", 0);
            poblist.Add("subd.", 0);
            //poblist.Add("", 0);

            dirlang.Add("north", "en");
            dirlang.Add("south", "en");
            dirlang.Add("east", "en");
            dirlang.Add("west", "en");
            dirlang.Add("northern", "en");
            dirlang.Add("southern", "en");
            dirlang.Add("eastern", "en");
            dirlang.Add("western", "en");
            dirlang.Add("norte", "es");
            dirlang.Add("sur", "es");
            dirlang.Add("este", "es");
            dirlang.Add("oeste", "es");
            dirlang.Add("upper", "en");
            dirlang.Add("lower", "en");
            dirlang.Add("central", "es");
            dirlang.Add("centro", "es");
            dirlang.Add("occidental", "la");
            dirlang.Add("oriental", "la");
            dirlang.Add("mataas", "tl");
            dirlang.Add("itaas", "tl");
            dirlang.Add("ibaba", "tl");
            dirlang.Add("labak", "tl");
            dirlang.Add("ilaya", "tl");
            dirlang.Add("iraya", "bcl");
            dirlang.Add("silangan", "tl");
            dirlang.Add("kanluran", "tl");
            dirlang.Add("ilawod", "bcl");
            dirlang.Add("malaki", "tl");
            dirlang.Add("maliit", "tl");
            dirlang.Add("munti", "tl");
            dirlang.Add("dako", "ceb");
            dirlang.Add("gamay", "ceb");
            dirlang.Add("bago", "tl");
            dirlang.Add("bag-o", "ceb");
            dirlang.Add("bagong", "tl");
            dirlang.Add("bag-ong", "ceb");
            dirlang.Add("new", "en");
            dirlang.Add("old", "en");
            dirlang.Add("nueva", "es");
            dirlang.Add("nuevo", "es");
            dirlang.Add("vieja", "es");
            dirlang.Add("viejo", "es");
            dirlang.Add("dacu", "ceb");
            dirlang.Add("cerca", "es");
            dirlang.Add("lejos", "es");
            dirlang.Add("grande", "es");
            dirlang.Add("pequeño", "es");
            dirlang.Add("pequeña", "es");
            dirlang.Add("chica", "es");
            dirlang.Add("chico", "es");
            dirlang.Add("mar", "es");
            dirlang.Add("monte", "es");
            dirlang.Add("baja", "es");
            dirlang.Add("alta", "es");
            dirlang.Add("bajo", "es");
            dirlang.Add("abajo", "es");
            dirlang.Add("abaja", "es");
            dirlang.Add("alto", "es");
            dirlang.Add("poblacion", "es");
            dirlang.Add("pob.", "es");
            dirlang.Add("district", "en");
            dirlang.Add("village", "en");
            dirlang.Add("zone", "en");
            dirlang.Add("proper", "en");
            dirlang.Add("barangay", "tl");
            dirlang.Add("bgy.", "tl");
            dirlang.Add("no.", "es");
            dirlang.Add("villa", "es");
            dirlang.Add("ubos", "ceb");
            dirlang.Add("purok", "tl");
            //dirlang.Add("zone", "en");
            dirlang.Add("mayor", "es");
            dirlang.Add("menor", "es");
            //dirlang.Add("cerca", "es");
            //dirlang.Add("lejos", "es");
            //dirlang.Add("central", "en");
            dirlang.Add("la", "es");
            dirlang.Add("de", "es");
            dirlang.Add("del", "es");
            dirlang.Add("las", "es");
            dirlang.Add("los", "es");
            dirlang.Add("el", "es");
            dirlang.Add("ni", "tl");
            dirlang.Add("sa", "tl");
            dirlang.Add("ng", "tl");
            dirlang.Add("of", "en");
            dirlang.Add("exterior", "en");
            dirlang.Add("interior", "en");
            dirlang.Add("coastal", "en");
            dirlang.Add("pulo", "tl");
            dirlang.Add("island", "en");
            dirlang.Add("isla", "es");
            dirlang.Add("large", "en");
            dirlang.Add("big", "en");
            dirlang.Add("small", "en");
            dirlang.Add("little", "en");
            dirlang.Add("subdivision", "en");
            dirlang.Add("subd.", "en");


            fill_numberlist();
        }

        public static bool checklists(string name)
        {
            bool found = false;
            if (dirlist.ContainsKey(name))
            {
                dirlist[name]++;
                found= true;
            }
            if (herolist.ContainsKey(name))
            {
                herolist[name]++;
                found = true;
            }
            if (saintlist.ContainsKey(name))
            {
                saintlist[name]++;
                found = true;
            }
            if (titlelist.ContainsKey(name))
            {
                titlelist[name]++;
                found = true;
            }
            if (poblist.ContainsKey(name))
            {
                poblist[name]++;
                found = true;
            }
            return found;
        }

        public static bool is_saint(string name)
        {
            foreach (string w in name.Split())
                if (saintlist.ContainsKey(w))
                    return true;
            return false;
        }

        public static bool is_person(string name)
        {
            foreach (string w in name.Split())
            {
                if (titlelist.ContainsKey(w))
                    return true;
                if (herolist.ContainsKey(w))
                    return true;
            }
            return false;
        }


        public static char[] trimcharsclean = new char[] { ',', '.', '-', ':' };
        public static string cleanname(string name)
        {
            string[] ww = name.Split();
            string clean = "";

            foreach (string w in ww)
            {
                if (w == "-")
                    continue;
                if (saintlist.ContainsKey(w))
                    return "";
                if (titlelist.ContainsKey(w))
                    return "";
                if (herolist.ContainsKey(w))
                    return "";
                if (dirlist.ContainsKey(w))
                    continue;
                if (dirlist.ContainsKey(w.Trim(trimcharsclean)))
                    continue;
                if (numberlist.Contains(w))
                    continue;
                if (numberlist.Contains(w.Trim(trimcharsclean)))
                    continue;
                if (poblist.ContainsKey(w))
                    continue;
                clean += " " + w.Trim();
            }
            return clean.Trim();
        }

        public static string remove_dirnumberpob(string name)
        {
            string[] ww = name.Split();
            string clean = "";

            foreach (string w in ww)
            {
                if (dirlist.ContainsKey(w))
                    continue;
                if (numberlist.Contains(w))
                    continue;
                if (poblist.ContainsKey(w))
                    continue;
                clean += " " + w;
            }
            return clean.Trim();
        }

        public static List<string> numberlist = new List<string>();
        public static List<string> fill_numberlist()
        {
            numberlist.Add("a");
            numberlist.Add("b");
            numberlist.Add("c");
            numberlist.Add("d");
            numberlist.Add("e");
            numberlist.Add("f");
            numberlist.Add("g");
            numberlist.Add("h");
            numberlist.Add("j");
            numberlist.Add("k");
            numberlist.Add("l");
            numberlist.Add("m");
            numberlist.Add("n");
            numberlist.Add("o");
            numberlist.Add("uno");
            numberlist.Add("dos");
            numberlist.Add("tres");
            numberlist.Add("1st");
            numberlist.Add("2nd");
            numberlist.Add("3rd");
            numberlist.Add("1");
            numberlist.Add("2");
            numberlist.Add("3");
            numberlist.Add("4");
            numberlist.Add("5");
            numberlist.Add("6");
            numberlist.Add("7");
            numberlist.Add("8");
            numberlist.Add("9");
            numberlist.Add("10");
            numberlist.Add("11");
            numberlist.Add("12");
            numberlist.Add("13");
            numberlist.Add("14");
            numberlist.Add("15");
            numberlist.Add("27");
            numberlist.Add("24");
            numberlist.Add("19");
            numberlist.Add("25");
            numberlist.Add("23");
            numberlist.Add("16");
            numberlist.Add("22");
            numberlist.Add("17");
            numberlist.Add("21");
            numberlist.Add("18");
            numberlist.Add("20");
            numberlist.Add("26");
            numberlist.Add("28");
            numberlist.Add("29");
            numberlist.Add("30");
            numberlist.Add("31");
            numberlist.Add("32");
            numberlist.Add("33");
            numberlist.Add("34");
            numberlist.Add("35");
            numberlist.Add("36");
            numberlist.Add("37");
            numberlist.Add("38");
            numberlist.Add("39");
            numberlist.Add("40");
            numberlist.Add("41");
            numberlist.Add("42");
            numberlist.Add("43");
            numberlist.Add("44");
            numberlist.Add("45");
            numberlist.Add("46");
            numberlist.Add("47");
            numberlist.Add("48");
            numberlist.Add("49");
            numberlist.Add("50");
            numberlist.Add("51");
            numberlist.Add("52");
            numberlist.Add("53");
            numberlist.Add("54");
            numberlist.Add("55");
            numberlist.Add("56");
            numberlist.Add("57");
            numberlist.Add("58");
            numberlist.Add("59");
            numberlist.Add("60");
            numberlist.Add("61");
            numberlist.Add("62");
            numberlist.Add("64");
            numberlist.Add("66");
            numberlist.Add("67");
            numberlist.Add("69");
            numberlist.Add("70");
            numberlist.Add("65");
            numberlist.Add("63");
            numberlist.Add("68");
            numberlist.Add("98");
            numberlist.Add("100");
            numberlist.Add("101");
            numberlist.Add("102");
            numberlist.Add("103");
            numberlist.Add("104");
            numberlist.Add("105");
            numberlist.Add("106");
            numberlist.Add("107");
            numberlist.Add("108");
            numberlist.Add("71");
            numberlist.Add("72");
            numberlist.Add("73");
            numberlist.Add("74");
            numberlist.Add("75");
            numberlist.Add("76");
            numberlist.Add("77");
            numberlist.Add("78");
            numberlist.Add("79");
            numberlist.Add("80");
            numberlist.Add("81");
            numberlist.Add("82");
            numberlist.Add("83");
            numberlist.Add("84");
            numberlist.Add("85");
            numberlist.Add("86");
            numberlist.Add("87");
            numberlist.Add("88");
            numberlist.Add("89");
            numberlist.Add("90");
            numberlist.Add("91");
            numberlist.Add("92");
            numberlist.Add("93");
            numberlist.Add("94");
            numberlist.Add("95");
            numberlist.Add("96");
            numberlist.Add("97");
            numberlist.Add("99");
            numberlist.Add("109");
            numberlist.Add("110");
            numberlist.Add("111");
            numberlist.Add("112");
            numberlist.Add("116");
            numberlist.Add("117");
            numberlist.Add("118");
            numberlist.Add("119");
            numberlist.Add("120");
            numberlist.Add("121");
            numberlist.Add("122");
            numberlist.Add("123");
            numberlist.Add("124");
            numberlist.Add("125");
            numberlist.Add("126");
            numberlist.Add("127");
            numberlist.Add("128");
            numberlist.Add("129");
            numberlist.Add("130");
            numberlist.Add("131");
            numberlist.Add("132");
            numberlist.Add("133");
            numberlist.Add("134");
            numberlist.Add("135");
            numberlist.Add("136");
            numberlist.Add("137");
            numberlist.Add("138");
            numberlist.Add("139");
            numberlist.Add("140");
            numberlist.Add("141");
            numberlist.Add("142");
            numberlist.Add("143");
            numberlist.Add("144");
            numberlist.Add("145");
            numberlist.Add("146");
            numberlist.Add("147");
            numberlist.Add("148");
            numberlist.Add("149");
            numberlist.Add("150");
            numberlist.Add("151");
            numberlist.Add("152");
            numberlist.Add("153");
            numberlist.Add("154");
            numberlist.Add("155");
            numberlist.Add("156");
            numberlist.Add("157");
            numberlist.Add("158");
            numberlist.Add("159");
            numberlist.Add("160");
            numberlist.Add("161");
            numberlist.Add("162");
            numberlist.Add("163");
            numberlist.Add("164");
            numberlist.Add("165");
            numberlist.Add("166");
            numberlist.Add("167");
            numberlist.Add("168");
            numberlist.Add("169");
            numberlist.Add("170");
            numberlist.Add("171");
            numberlist.Add("172");
            numberlist.Add("173");
            numberlist.Add("174");
            numberlist.Add("175");
            numberlist.Add("176");
            numberlist.Add("177");
            numberlist.Add("178");
            numberlist.Add("179");
            numberlist.Add("180");
            numberlist.Add("181");
            numberlist.Add("182");
            numberlist.Add("183");
            numberlist.Add("184");
            numberlist.Add("185");
            numberlist.Add("186");
            numberlist.Add("187");
            numberlist.Add("188");
            numberlist.Add("189");
            numberlist.Add("190");
            numberlist.Add("191");
            numberlist.Add("192");
            numberlist.Add("193");
            numberlist.Add("194");
            numberlist.Add("195");
            numberlist.Add("196");
            numberlist.Add("197");
            numberlist.Add("198");
            numberlist.Add("199");
            numberlist.Add("200");
            numberlist.Add("201");
            numberlist.Add("202");
            numberlist.Add("203");
            numberlist.Add("204");
            numberlist.Add("205");
            numberlist.Add("206");
            numberlist.Add("207");
            numberlist.Add("208");
            numberlist.Add("209");
            numberlist.Add("210");
            numberlist.Add("211");
            numberlist.Add("212");
            numberlist.Add("213");
            numberlist.Add("214");
            numberlist.Add("215");
            numberlist.Add("216");
            numberlist.Add("217");
            numberlist.Add("218");
            numberlist.Add("219");
            numberlist.Add("220");
            numberlist.Add("221");
            numberlist.Add("222");
            numberlist.Add("223");
            numberlist.Add("224");
            numberlist.Add("225");
            numberlist.Add("226");
            numberlist.Add("227");
            numberlist.Add("228");
            numberlist.Add("229");
            numberlist.Add("230");
            numberlist.Add("231");
            numberlist.Add("232");
            numberlist.Add("233");
            numberlist.Add("234");
            numberlist.Add("235");
            numberlist.Add("236");
            numberlist.Add("237");
            numberlist.Add("238");
            numberlist.Add("239");
            numberlist.Add("240");
            numberlist.Add("241");
            numberlist.Add("242");
            numberlist.Add("243");
            numberlist.Add("244");
            numberlist.Add("245");
            numberlist.Add("246");
            numberlist.Add("247");
            numberlist.Add("248");
            numberlist.Add("249");
            numberlist.Add("250");
            numberlist.Add("251");
            numberlist.Add("252");
            numberlist.Add("253");
            numberlist.Add("254");
            numberlist.Add("255");
            numberlist.Add("256");
            numberlist.Add("257");
            numberlist.Add("258");
            numberlist.Add("259");
            numberlist.Add("260");
            numberlist.Add("261");
            numberlist.Add("262");
            numberlist.Add("263");
            numberlist.Add("264");
            numberlist.Add("265");
            numberlist.Add("266");
            numberlist.Add("267");
            numberlist.Add("287");
            numberlist.Add("288");
            numberlist.Add("289");
            numberlist.Add("290");
            numberlist.Add("291");
            numberlist.Add("292");
            numberlist.Add("293");
            numberlist.Add("294");
            numberlist.Add("295");
            numberlist.Add("296");
            numberlist.Add("383");
            numberlist.Add("384");
            numberlist.Add("385");
            numberlist.Add("386");
            numberlist.Add("387");
            numberlist.Add("388");
            numberlist.Add("389");
            numberlist.Add("390");
            numberlist.Add("391");
            numberlist.Add("392");
            numberlist.Add("393");
            numberlist.Add("394");
            numberlist.Add("306");
            numberlist.Add("307");
            numberlist.Add("308");
            numberlist.Add("309");
            numberlist.Add("268");
            numberlist.Add("269");
            numberlist.Add("270");
            numberlist.Add("271");
            numberlist.Add("272");
            numberlist.Add("273");
            numberlist.Add("274");
            numberlist.Add("275");
            numberlist.Add("276");
            numberlist.Add("281");
            numberlist.Add("282");
            numberlist.Add("283");
            numberlist.Add("284");
            numberlist.Add("285");
            numberlist.Add("286");
            numberlist.Add("297");
            numberlist.Add("298");
            numberlist.Add("299");
            numberlist.Add("300");
            numberlist.Add("301");
            numberlist.Add("302");
            numberlist.Add("303");
            numberlist.Add("304");
            numberlist.Add("305");
            numberlist.Add("310");
            numberlist.Add("311");
            numberlist.Add("312");
            numberlist.Add("313");
            numberlist.Add("314");
            numberlist.Add("315");
            numberlist.Add("316");
            numberlist.Add("317");
            numberlist.Add("318");
            numberlist.Add("319");
            numberlist.Add("320");
            numberlist.Add("321");
            numberlist.Add("322");
            numberlist.Add("323");
            numberlist.Add("324");
            numberlist.Add("325");
            numberlist.Add("326");
            numberlist.Add("327");
            numberlist.Add("328");
            numberlist.Add("329");
            numberlist.Add("330");
            numberlist.Add("331");
            numberlist.Add("332");
            numberlist.Add("333");
            numberlist.Add("334");
            numberlist.Add("335");
            numberlist.Add("336");
            numberlist.Add("337");
            numberlist.Add("338");
            numberlist.Add("339");
            numberlist.Add("340");
            numberlist.Add("341");
            numberlist.Add("342");
            numberlist.Add("343");
            numberlist.Add("344");
            numberlist.Add("345");
            numberlist.Add("346");
            numberlist.Add("347");
            numberlist.Add("348");
            numberlist.Add("349");
            numberlist.Add("350");
            numberlist.Add("351");
            numberlist.Add("352");
            numberlist.Add("353");
            numberlist.Add("354");
            numberlist.Add("355");
            numberlist.Add("356");
            numberlist.Add("357");
            numberlist.Add("358");
            numberlist.Add("359");
            numberlist.Add("360");
            numberlist.Add("361");
            numberlist.Add("362");
            numberlist.Add("363");
            numberlist.Add("364");
            numberlist.Add("365");
            numberlist.Add("366");
            numberlist.Add("367");
            numberlist.Add("368");
            numberlist.Add("369");
            numberlist.Add("370");
            numberlist.Add("371");
            numberlist.Add("372");
            numberlist.Add("373");
            numberlist.Add("374");
            numberlist.Add("375");
            numberlist.Add("376");
            numberlist.Add("377");
            numberlist.Add("378");
            numberlist.Add("379");
            numberlist.Add("380");
            numberlist.Add("381");
            numberlist.Add("382");
            numberlist.Add("395");
            numberlist.Add("396");
            numberlist.Add("397");
            numberlist.Add("398");
            numberlist.Add("399");
            numberlist.Add("400");
            numberlist.Add("401");
            numberlist.Add("402");
            numberlist.Add("403");
            numberlist.Add("404");
            numberlist.Add("405");
            numberlist.Add("406");
            numberlist.Add("407");
            numberlist.Add("408");
            numberlist.Add("409");
            numberlist.Add("410");
            numberlist.Add("411");
            numberlist.Add("412");
            numberlist.Add("413");
            numberlist.Add("414");
            numberlist.Add("415");
            numberlist.Add("416");
            numberlist.Add("417");
            numberlist.Add("418");
            numberlist.Add("419");
            numberlist.Add("420");
            numberlist.Add("421");
            numberlist.Add("422");
            numberlist.Add("423");
            numberlist.Add("424");
            numberlist.Add("425");
            numberlist.Add("426");
            numberlist.Add("427");
            numberlist.Add("428");
            numberlist.Add("429");
            numberlist.Add("430");
            numberlist.Add("431");
            numberlist.Add("432");
            numberlist.Add("433");
            numberlist.Add("434");
            numberlist.Add("435");
            numberlist.Add("436");
            numberlist.Add("437");
            numberlist.Add("438");
            numberlist.Add("439");
            numberlist.Add("440");
            numberlist.Add("441");
            numberlist.Add("442");
            numberlist.Add("443");
            numberlist.Add("444");
            numberlist.Add("445");
            numberlist.Add("446");
            numberlist.Add("447");
            numberlist.Add("448");
            numberlist.Add("449");
            numberlist.Add("450");
            numberlist.Add("451");
            numberlist.Add("452");
            numberlist.Add("453");
            numberlist.Add("454");
            numberlist.Add("455");
            numberlist.Add("456");
            numberlist.Add("457");
            numberlist.Add("458");
            numberlist.Add("459");
            numberlist.Add("460");
            numberlist.Add("461");
            numberlist.Add("462");
            numberlist.Add("463");
            numberlist.Add("464");
            numberlist.Add("465");
            numberlist.Add("466");
            numberlist.Add("467");
            numberlist.Add("468");
            numberlist.Add("469");
            numberlist.Add("470");
            numberlist.Add("471");
            numberlist.Add("472");
            numberlist.Add("473");
            numberlist.Add("474");
            numberlist.Add("475");
            numberlist.Add("476");
            numberlist.Add("477");
            numberlist.Add("478");
            numberlist.Add("479");
            numberlist.Add("480");
            numberlist.Add("481");
            numberlist.Add("482");
            numberlist.Add("483");
            numberlist.Add("484");
            numberlist.Add("485");
            numberlist.Add("486");
            numberlist.Add("487");
            numberlist.Add("488");
            numberlist.Add("489");
            numberlist.Add("490");
            numberlist.Add("491");
            numberlist.Add("492");
            numberlist.Add("493");
            numberlist.Add("494");
            numberlist.Add("495");
            numberlist.Add("496");
            numberlist.Add("497");
            numberlist.Add("498");
            numberlist.Add("499");
            numberlist.Add("500");
            numberlist.Add("501");
            numberlist.Add("502");
            numberlist.Add("503");
            numberlist.Add("504");
            numberlist.Add("505");
            numberlist.Add("506");
            numberlist.Add("507");
            numberlist.Add("508");
            numberlist.Add("509");
            numberlist.Add("510");
            numberlist.Add("511");
            numberlist.Add("512");
            numberlist.Add("513");
            numberlist.Add("514");
            numberlist.Add("515");
            numberlist.Add("516");
            numberlist.Add("517");
            numberlist.Add("518");
            numberlist.Add("519");
            numberlist.Add("520");
            numberlist.Add("521");
            numberlist.Add("522");
            numberlist.Add("523");
            numberlist.Add("524");
            numberlist.Add("525");
            numberlist.Add("526");
            numberlist.Add("527");
            numberlist.Add("528");
            numberlist.Add("529");
            numberlist.Add("530");
            numberlist.Add("531");
            numberlist.Add("532");
            numberlist.Add("533");
            numberlist.Add("534");
            numberlist.Add("535");
            numberlist.Add("536");
            numberlist.Add("537");
            numberlist.Add("538");
            numberlist.Add("539");
            numberlist.Add("540");
            numberlist.Add("541");
            numberlist.Add("542");
            numberlist.Add("543");
            numberlist.Add("544");
            numberlist.Add("545");
            numberlist.Add("546");
            numberlist.Add("547");
            numberlist.Add("548");
            numberlist.Add("549");
            numberlist.Add("550");
            numberlist.Add("551");
            numberlist.Add("552");
            numberlist.Add("553");
            numberlist.Add("554");
            numberlist.Add("555");
            numberlist.Add("556");
            numberlist.Add("557");
            numberlist.Add("558");
            numberlist.Add("559");
            numberlist.Add("560");
            numberlist.Add("561");
            numberlist.Add("562");
            numberlist.Add("563");
            numberlist.Add("564");
            numberlist.Add("565");
            numberlist.Add("566");
            numberlist.Add("567");
            numberlist.Add("568");
            numberlist.Add("569");
            numberlist.Add("570");
            numberlist.Add("571");
            numberlist.Add("572");
            numberlist.Add("573");
            numberlist.Add("574");
            numberlist.Add("575");
            numberlist.Add("576");
            numberlist.Add("577");
            numberlist.Add("578");
            numberlist.Add("579");
            numberlist.Add("580");
            numberlist.Add("581");
            numberlist.Add("582");
            numberlist.Add("583");
            numberlist.Add("584");
            numberlist.Add("585");
            numberlist.Add("586");
            numberlist.Add("587");
            numberlist.Add("588");
            numberlist.Add("589");
            numberlist.Add("590");
            numberlist.Add("591");
            numberlist.Add("592");
            numberlist.Add("593");
            numberlist.Add("594");
            numberlist.Add("595");
            numberlist.Add("596");
            numberlist.Add("597");
            numberlist.Add("598");
            numberlist.Add("599");
            numberlist.Add("600");
            numberlist.Add("601");
            numberlist.Add("602");
            numberlist.Add("603");
            numberlist.Add("604");
            numberlist.Add("605");
            numberlist.Add("606");
            numberlist.Add("607");
            numberlist.Add("608");
            numberlist.Add("609");
            numberlist.Add("610");
            numberlist.Add("611");
            numberlist.Add("612");
            numberlist.Add("613");
            numberlist.Add("614");
            numberlist.Add("615");
            numberlist.Add("616");
            numberlist.Add("617");
            numberlist.Add("618");
            numberlist.Add("619");
            numberlist.Add("620");
            numberlist.Add("621");
            numberlist.Add("622");
            numberlist.Add("623");
            numberlist.Add("624");
            numberlist.Add("625");
            numberlist.Add("626");
            numberlist.Add("627");
            numberlist.Add("628");
            numberlist.Add("629");
            numberlist.Add("630");
            numberlist.Add("631");
            numberlist.Add("632");
            numberlist.Add("633");
            numberlist.Add("634");
            numberlist.Add("635");
            numberlist.Add("636");
            numberlist.Add("637");
            numberlist.Add("638");
            numberlist.Add("639");
            numberlist.Add("640");
            numberlist.Add("641");
            numberlist.Add("642");
            numberlist.Add("643");
            numberlist.Add("644");
            numberlist.Add("645");
            numberlist.Add("646");
            numberlist.Add("647");
            numberlist.Add("648");
            numberlist.Add("659");
            numberlist.Add("660");
            numberlist.Add("661");
            numberlist.Add("666");
            numberlist.Add("667");
            numberlist.Add("668");
            numberlist.Add("669");
            numberlist.Add("670");
            numberlist.Add("663");
            numberlist.Add("664");
            numberlist.Add("654");
            numberlist.Add("655");
            numberlist.Add("656");
            numberlist.Add("657");
            numberlist.Add("658");
            numberlist.Add("689");
            numberlist.Add("690");
            numberlist.Add("691");
            numberlist.Add("692");
            numberlist.Add("693");
            numberlist.Add("694");
            numberlist.Add("695");
            numberlist.Add("696");
            numberlist.Add("697");
            numberlist.Add("698");
            numberlist.Add("699");
            numberlist.Add("700");
            numberlist.Add("701");
            numberlist.Add("702");
            numberlist.Add("703");
            numberlist.Add("704");
            numberlist.Add("705");
            numberlist.Add("706");
            numberlist.Add("707");
            numberlist.Add("708");
            numberlist.Add("709");
            numberlist.Add("710");
            numberlist.Add("711");
            numberlist.Add("712");
            numberlist.Add("713");
            numberlist.Add("714");
            numberlist.Add("715");
            numberlist.Add("716");
            numberlist.Add("717");
            numberlist.Add("718");
            numberlist.Add("719");
            numberlist.Add("720");
            numberlist.Add("721");
            numberlist.Add("722");
            numberlist.Add("723");
            numberlist.Add("724");
            numberlist.Add("725");
            numberlist.Add("726");
            numberlist.Add("727");
            numberlist.Add("728");
            numberlist.Add("729");
            numberlist.Add("730");
            numberlist.Add("731");
            numberlist.Add("732");
            numberlist.Add("733");
            numberlist.Add("738");
            numberlist.Add("739");
            numberlist.Add("740");
            numberlist.Add("741");
            numberlist.Add("742");
            numberlist.Add("743");
            numberlist.Add("744");
            numberlist.Add("688");
            numberlist.Add("735");
            numberlist.Add("736");
            numberlist.Add("737");
            numberlist.Add("734");
            numberlist.Add("662");
            numberlist.Add("671");
            numberlist.Add("672");
            numberlist.Add("673");
            numberlist.Add("674");
            numberlist.Add("675");
            numberlist.Add("676");
            numberlist.Add("677");
            numberlist.Add("678");
            numberlist.Add("679");
            numberlist.Add("680");
            numberlist.Add("681");
            numberlist.Add("682");
            numberlist.Add("683");
            numberlist.Add("684");
            numberlist.Add("685");
            numberlist.Add("809");
            numberlist.Add("810");
            numberlist.Add("811");
            numberlist.Add("812");
            numberlist.Add("813");
            numberlist.Add("814");
            numberlist.Add("815");
            numberlist.Add("816");
            numberlist.Add("817");
            numberlist.Add("818");
            numberlist.Add("819");
            numberlist.Add("820");
            numberlist.Add("821");
            numberlist.Add("822");
            numberlist.Add("823");
            numberlist.Add("824");
            numberlist.Add("825");
            numberlist.Add("826");
            numberlist.Add("827");
            numberlist.Add("828");
            numberlist.Add("829");
            numberlist.Add("830");
            numberlist.Add("831");
            numberlist.Add("832");
            numberlist.Add("686");
            numberlist.Add("687");
            numberlist.Add("833");
            numberlist.Add("834");
            numberlist.Add("835");
            numberlist.Add("836");
            numberlist.Add("837");
            numberlist.Add("838");
            numberlist.Add("839");
            numberlist.Add("840");
            numberlist.Add("841");
            numberlist.Add("842");
            numberlist.Add("843");
            numberlist.Add("844");
            numberlist.Add("845");
            numberlist.Add("846");
            numberlist.Add("847");
            numberlist.Add("848");
            numberlist.Add("849");
            numberlist.Add("850");
            numberlist.Add("851");
            numberlist.Add("852");
            numberlist.Add("853");
            numberlist.Add("855");
            numberlist.Add("856");
            numberlist.Add("857");
            numberlist.Add("858");
            numberlist.Add("859");
            numberlist.Add("860");
            numberlist.Add("861");
            numberlist.Add("862");
            numberlist.Add("863");
            numberlist.Add("864");
            numberlist.Add("865");
            numberlist.Add("867");
            numberlist.Add("868");
            numberlist.Add("870");
            numberlist.Add("871");
            numberlist.Add("872");
            numberlist.Add("869");
            numberlist.Add("649");
            numberlist.Add("650");
            numberlist.Add("651");
            numberlist.Add("652");
            numberlist.Add("653");
            numberlist.Add("745");
            numberlist.Add("746");
            numberlist.Add("747");
            numberlist.Add("748");
            numberlist.Add("749");
            numberlist.Add("750");
            numberlist.Add("751");
            numberlist.Add("752");
            numberlist.Add("753");
            numberlist.Add("755");
            numberlist.Add("756");
            numberlist.Add("757");
            numberlist.Add("758");
            numberlist.Add("759");
            numberlist.Add("760");
            numberlist.Add("761");
            numberlist.Add("762");
            numberlist.Add("763");
            numberlist.Add("764");
            numberlist.Add("765");
            numberlist.Add("766");
            numberlist.Add("767");
            numberlist.Add("768");
            numberlist.Add("769");
            numberlist.Add("770");
            numberlist.Add("771");
            numberlist.Add("772");
            numberlist.Add("773");
            numberlist.Add("774");
            numberlist.Add("775");
            numberlist.Add("776");
            numberlist.Add("777");
            numberlist.Add("778");
            numberlist.Add("779");
            numberlist.Add("780");
            numberlist.Add("781");
            numberlist.Add("782");
            numberlist.Add("783");
            numberlist.Add("784");
            numberlist.Add("785");
            numberlist.Add("786");
            numberlist.Add("787");
            numberlist.Add("788");
            numberlist.Add("789");
            numberlist.Add("790");
            numberlist.Add("791");
            numberlist.Add("792");
            numberlist.Add("793");
            numberlist.Add("794");
            numberlist.Add("795");
            numberlist.Add("796");
            numberlist.Add("797");
            numberlist.Add("798");
            numberlist.Add("799");
            numberlist.Add("800");
            numberlist.Add("801");
            numberlist.Add("802");
            numberlist.Add("803");
            numberlist.Add("804");
            numberlist.Add("805");
            numberlist.Add("806");
            numberlist.Add("807");
            numberlist.Add("866");
            numberlist.Add("873");
            numberlist.Add("874");
            numberlist.Add("875");
            numberlist.Add("876");
            numberlist.Add("877");
            numberlist.Add("878");
            numberlist.Add("879");
            numberlist.Add("880");
            numberlist.Add("881");
            numberlist.Add("882");
            numberlist.Add("883");
            numberlist.Add("884");
            numberlist.Add("885");
            numberlist.Add("886");
            numberlist.Add("887");
            numberlist.Add("888");
            numberlist.Add("889");
            numberlist.Add("890");
            numberlist.Add("891");
            numberlist.Add("892");
            numberlist.Add("893");
            numberlist.Add("894");
            numberlist.Add("895");
            numberlist.Add("896");
            numberlist.Add("897");
            numberlist.Add("898");
            numberlist.Add("899");
            numberlist.Add("900");
            numberlist.Add("901");
            numberlist.Add("902");
            numberlist.Add("903");
            numberlist.Add("904");
            numberlist.Add("905");
            numberlist.Add("754");
            numberlist.Add("808");
            numberlist.Add("113");
            numberlist.Add("114");
            numberlist.Add("115");
            numberlist.Add("i");
            numberlist.Add("ii");
            numberlist.Add("iii");
            numberlist.Add("iv");
            numberlist.Add("v");
            numberlist.Add("vi");
            numberlist.Add("vii");
            numberlist.Add("viii");
            numberlist.Add("ix");
            numberlist.Add("x");
            numberlist.Add("xi");
            numberlist.Add("xii");
            numberlist.Add("xiii");
            numberlist.Add("xiv");
            numberlist.Add("xv");
            numberlist.Add("xvi");
            numberlist.Add("xvii");
            numberlist.Add("xviii");
            numberlist.Add("xix");
            numberlist.Add("xx");
            numberlist.Add("xxi");

            List<string> dummy = new List<string>();
            foreach (string s in numberlist)
            {
                dummy.Add(s + "-a");
                dummy.Add(s + "-b");
                dummy.Add(s + "-c");
                dummy.Add(s + "-d");
                dummy.Add(s + "-e");
                dummy.Add(s + "-f");
                dummy.Add(s + "-g");
                dummy.Add(s + "-h");
            }
            numberlist.AddRange(dummy);
            return numberlist;
        }


        public static List<string> cleanwolff(List<string> raw)
        {

            char[] trimcharswolff = new char[] { '*', '†', '-', '1', '2', '3', '4', '5' };
            List<string> ls = new List<string>();

            foreach (string s in raw)
            {
                string ss = s.Trim(trimcharswolff);
                if (!ls.Contains(ss))
                    ls.Add(ss);
            }

            return ls;
        }

        public static bool wolfftest(string w,List<string> cebwords)
        {
            return cebwords.Contains(w.Replace('e', 'i').Replace('o', 'u'));
        }
    }
}
