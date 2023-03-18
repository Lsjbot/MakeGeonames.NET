using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DotNetWikiBot;

namespace MakeGeonames
{
    public class admclass //names of administrative entities by level, one admclass for each country
    {
        public string[] label = new string[5];
        public string[] det = new string[5];
        public int maxadm = 5;

        public static Dictionary<string, admclass> admdict = new Dictionary<string, admclass>();
        public static Dictionary<string, string> admtodet = new Dictionary<string, string>(); //from base form to determinate form of adm labels

        public static Dictionary<string, List<string>> existing_adm1 = new Dictionary<string, List<string>>();
        public static Dictionary<string, Dictionary<string, int>> adm1dict = new Dictionary<string, Dictionary<string, int>>();
        public static Dictionary<string, Dictionary<string, Dictionary<string, int>>> adm2dict = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
        public static Dictionary<string, string> admcap = new Dictionary<string, string>(); //names in Form1.makelang of capitals of various types of administrative units

        public static void read_adm1()
        {
            int n = 0;



            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "admin1CodesASCII.txt"))
            {
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

                    //    public static Dictionary<string,Dictionary<string,int>> adm1dict = new Dictionary<string,Dictionary<string,int>>();


                    int geonameid = -1;

                    geonameid = util.tryconvert(words[3]);

                    string[] ww = words[0].Split('.');
                    if (adm1dict.ContainsKey(ww[0]))
                        adm1dict[ww[0]].Add(ww[1], geonameid);
                    else
                    {
                        Dictionary<string, int> dd = new Dictionary<string, int>();
                        dd.Add(ww[1], geonameid);
                        adm1dict.Add(ww[0], dd);
                    }

                    if (ww[0] == Form1.makecountry)
                    {
                        Console.WriteLine("adm1:" + words[0] + ":" + geonameid.ToString());
                    }


                    n++;
                    if ((n % 1000) == 0)
                    {
                        Console.WriteLine("n (adm1)   = " + n.ToString());

                    }

                }

                Console.WriteLine("n    (adm1)= " + n.ToString());
                //Console.WriteLine("<cr>");
                //Console.ReadLine();

            }
        }

        public static void read_adm2()
        {
            int n = 0;

            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "admin2Codes.txt"))
            {
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

                    //    public static Dictionary<string,Dictionary<string,int>> adm1dict = new Dictionary<string,Dictionary<string,int>>();

                    countryclass country = new countryclass();

                    int geonameid = -1;

                    geonameid = util.tryconvert(words[3]);

                    string[] ww = words[0].Split('.');
                    if (!adm2dict.ContainsKey(ww[0]))
                    {
                        Dictionary<string, Dictionary<string, int>> dd = new Dictionary<string, Dictionary<string, int>>();
                        adm2dict.Add(ww[0], dd);
                    }

                    if (adm2dict[ww[0]].ContainsKey(ww[1]))
                        adm2dict[ww[0]][ww[1]].Add(ww[2], geonameid);
                    else
                    {
                        Dictionary<string, int> ddd = new Dictionary<string, int>();
                        ddd.Add(ww[2], geonameid);
                        adm2dict[ww[0]].Add(ww[1], ddd);
                    }


                    n++;
                    if ((n % 10000) == 0)
                    {
                        Console.WriteLine("n (adm2)   = " + n.ToString());

                    }

                }

                Console.WriteLine("n    (adm2)= " + n.ToString());

            }
        }

        public static void read_existing_adm1(Site makesite)
        {
            //public static Dictionary<string, List<string>> existing_adm1 = new Dictionary<string, List<string>>();
            PageList pl = new PageList(makesite);
            pl.FillAllFromCategory("Kategori:Regionala politiska indelningar");

            int n = 0;
            foreach (Page p in pl)
            {
                string tit = util.remove_disambig(p.title.Replace("Kategori:", ""));

                string country = "";
                if (tit.Contains("s "))
                    country = tit.Substring(0, tit.IndexOf("s "));
                else
                    continue;

                PageList pl1 = new PageList(makesite);
                pl1.FillAllFromCategory(p.title);
                foreach (Page p1 in pl1)
                {
                    string tit1 = p1.title.Replace("Kategori:", "");
                    if (!existing_adm1.ContainsKey(country))
                    {
                        List<string> ll = new List<string>();
                        existing_adm1.Add(country, ll);
                        Console.WriteLine("country = " + country);
                    }
                    existing_adm1[country].Add(tit1);
                    n++;
                }
            }
            Console.WriteLine("n(adm1) = " + n.ToString());

        }

        public static void fill_admcap()
        {
            if (admcap.Count != 0)
                return;

            if (Form1.makelang == "ceb")
            {
                for (int i = 1; i < 5; i++)
                {
                    foreach (string mc in admdict.Keys)
                    {
                        if ((!String.IsNullOrEmpty(admdict[mc].label[i - 1])) && (!admcap.ContainsKey(admdict[mc].label[i - 1])))
                            admcap.Add(admdict[mc].label[i - 1], "kapital sa " + admdict[mc].label[i - 1]);
                    }
                }
            }
            else if (Form1.makelang == "no")
            {
                for (int i = 1; i < 5; i++)
                {
                    foreach (string mc in admdict.Keys)
                    {
                        if ((!String.IsNullOrEmpty(admdict[mc].label[i - 1])) && (!admcap.ContainsKey(admdict[mc].label[i - 1])))
                            admcap.Add(admdict[mc].label[i - 1], "administrasjonssenter i " + admdict[mc].label[i - 1]);
                    }
                }
            }
            else if (Form1.makelang == "sv")
            {
                admcap.Add("administrativ atoll", "atollhuvudort");
                admcap.Add("administrativ by", "byhuvudort");
                admcap.Add("administrativ enhet", "enhetshuvudort");
                admcap.Add("administrativ ö", "öhuvudort");
                admcap.Add("administrativt område", "områdeshuvudort");
                admcap.Add("arrondissement", "arrondissementhuvudort");
                admcap.Add("barrio", "barriohuvudort");
                admcap.Add("byutvecklingskommitté", "byutvecklingskommittéhuvudort");
                admcap.Add("community", "communityhuvudort");
                admcap.Add("constituency", "constituencyhuvudort");
                admcap.Add("corregimiento", "corregimientohuvudort");
                admcap.Add("county", "countyhuvudort");
                admcap.Add("delegation", "delegationshuvudort");
                admcap.Add("delstat", "delstatshuvudstad");
                admcap.Add("departement", "departementshuvudort");
                admcap.Add("distrikt", "distriktshuvudort");
                admcap.Add("division", "divisionshuvudort");
                admcap.Add("emirat", "emirathuvudstad");
                admcap.Add("entitet", "entitetshuvudort");
                admcap.Add("fylke", "fylkeshuvudort");
                admcap.Add("förbundsland", "förbundslandshuvudstad");
                admcap.Add("församling", "församlingshuvudort");
                admcap.Add("gemenskap", "gemenskapshuvudort");
                admcap.Add("gewog", "gewoghuvudort");
                admcap.Add("grannskap", "grannskapshuvudort");
                admcap.Add("grevskap", "grevskapshuvudort");
                admcap.Add("guvernement", "guvernementshuvudort");
                admcap.Add("härad", "häradshuvudort");
                admcap.Add("hövdingadöme", "hövdingadömeshuvudort");
                admcap.Add("hövdingaråd", "hövdingarådshuvudort");
                admcap.Add("kabupaten", "kabupatenhuvudort");
                admcap.Add("kanton", "kantonhuvudstad");
                admcap.Add("klan", "klanhuvudort");
                admcap.Add("kommun", "kommunhuvudort");
                admcap.Add("krets", "kretshuvudort");
                admcap.Add("kungadöme", "kungadömeshuvudstad");
                admcap.Add("kvarter", "kvartershuvudort");
                admcap.Add("lokalstyresområde", "lokalstyresområdeshuvudort");
                admcap.Add("län", "länshuvudort");
                admcap.Add("mahaliyya", "mahaliyyahuvudort");
                admcap.Add("mukim", "mukimhuvudort");
                admcap.Add("oblast", "oblasthuvudort");
                admcap.Add("oblyst", "oblysthuvudort");
                admcap.Add("område", "områdeshuvudort");
                admcap.Add("opština", "opštinahuvudort");
                admcap.Add("parroqui", "parroquihuvudort");
                admcap.Add("powiat", "powiathuvudort");
                admcap.Add("prefektur", "prefekturhuvudort");
                admcap.Add("provins", "provinshuvudstad");
                admcap.Add("rajon", "rajonhuvudort");
                admcap.Add("region", "regionhuvudort");
                admcap.Add("autonom region", "regionhuvudort");
                admcap.Add("regeringsdistrikt", "regeringsdistriktshuvudort");
                admcap.Add("riksdel", "riksdelshuvudstad");
                admcap.Add("rote", "rotehuvudort");
                admcap.Add("rådsområde", "rådsområdeshuvudort");
                admcap.Add("samhällsutvecklingsråd", "samhällsutvecklingsrådshuvudort");
                admcap.Add("sektor", "sektorshuvudort");
                admcap.Add("shehia", "shehiahuvudort");
                admcap.Add("socken", "sockenhuvudort");
                admcap.Add("stad", "stadshuvudort");
                admcap.Add("stadsdel", "stadsdelshuvudort");
                admcap.Add("subbarrio", "subbarriohuvudort");
                admcap.Add("subdistrikt", "subdistriktshuvudort");
                admcap.Add("subprefektur", "subprefekturhuvudort");
                admcap.Add("sýsla", "sýslahuvudort");
                admcap.Add("tehsil", "tehsilhuvudort");
                admcap.Add("territorium", "territoriehuvudort");
                admcap.Add("tidigare kommun", "huvudort för tidigare kommun");
                admcap.Add("underdistrikt", "underdistriktshuvudort");
                admcap.Add("utvecklingsregion", "utvecklingsregionshuvudort");
                admcap.Add("ward", "wardhuvudort");
                admcap.Add("voblast", "voblasthuvudort");
                admcap.Add("vojvodskap", "vojvodskapshuvudort");
                admcap.Add("zon", "zonhuvudort");
                admcap.Add("åldermannaskap", "åldermannaskapshuvudort");
                admcap.Add("ö och specialkommun", "öhuvudort");
                admcap.Add("ögrupp", "ögruppshuvudort");
                admcap.Add("öområde", "öområdeshuvudort");
                admcap.Add("öråd", "örådshuvudort");
                admcap.Add("parish", "parishhuvudort");
                admcap.Add("parroquia", "parroquiahuvudort");
                admcap.Add("freguesia", "freguesiahuvudort");
                admcap.Add("kraj", "krajhuvudort");
                admcap.Add("delrepublik", "delrepublikhuvudstad");
                admcap.Add("autonomt distrikt", "distriktshuvudort");
                admcap.Add("köping", "köpinghuvudort");
            }


        }

        public static void read_adm(Site makesite)
        {
            int n = 0;

            List<string> uniquelabels = new List<string>();

            string lf = "";

            if (Form1.firstround)
            {
                using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "adm-" + Form1.makelang + ".txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();

                        //if (n > 250)
                        //Console.WriteLine(line);

                        string[] words = line.Split('\t');
                        while (words.Length < 11)
                        {
                            line += "\t";
                            words = line.Split('\t');
                        }

                        //Console.WriteLine("wl = " + words.Length.ToString());

                        admclass ad = new admclass();

                        int maxlabel = 0;

                        for (int i = 0; i < 5; i++)
                        {
                            ad.label[i] = words[i + 1];
                            if (!String.IsNullOrEmpty(words[i + 1]))
                                maxlabel = i + 1;
                            if (!uniquelabels.Contains(words[i + 1]))
                                uniquelabels.Add(words[i + 1]);
                            ad.det[i] = words[i + 6];

                            if (!admtodet.ContainsKey(ad.label[i]))
                                admtodet.Add(ad.label[i], ad.det[i]);
                        }

                        ad.maxadm = maxlabel;
                        if (words[0] == Form1.makecountry)
                            Console.WriteLine(words[0] + ": maxadm = " + maxlabel.ToString());

                        admdict.Add(words[0], ad);

                        if (Form1.saveadmlinks)
                        {
                            lf += "* " + countryclass.linkcountry(words[0]) + "\n";
                            for (int i = 0; i < 5; i++)
                            {
                                if (!String.IsNullOrEmpty(ad.label[i]))
                                    lf += ":* ADM" + (i + 1).ToString() + ": [[" + ad.label[i] + "]]\n";
                            }

                        }


                        n++;
                        if ((n % 10) == 0)
                        {
                            Console.WriteLine("n (adm)   = " + n.ToString());

                        }

                    }

                    if (Form1.makelang == "sv")
                    {
                        if (!admtodet.ContainsKey("kraj"))
                            admtodet.Add("kraj", "krajen");
                        if (!admtodet.ContainsKey("köping"))
                            admtodet.Add("köping", "köpingen");
                        if (!admtodet.ContainsKey("socken"))
                            admtodet.Add("socken", "socknen");
                        if (!admtodet.ContainsKey("autonomt distrikt"))
                            admtodet.Add("autonomt distrikt", "det autonoma distriktet");
                        if (!admtodet.ContainsKey("delrepublik"))
                            admtodet.Add("delrepublik", "delrepubliken");
                    }

                    Console.WriteLine("n    (adm)= " + n.ToString());
                    if (Form1.saveadmlinks)
                    {
                        Page pf = new Page(makesite, util.mp(13) + Form1.botname + "/linkadmin");
                        pf.text = lf;
                        util.trysave(pf, 1, "Bot saving linkadmin");
                    }
                }

                //using (StreamWriter sw = new StreamWriter("uniquelabels.txt"))
                //{
                //    foreach (string ul in uniquelabels)
                //        sw.WriteLine(ul);
                //}
                //Console.WriteLine("unique labels written");
                //Console.ReadLine();
            }

            fill_admcap();

            if (Form1.makecountry != "")
            {
                if (admdict.ContainsKey(Form1.makecountry))
                {
                    string[] p167 = new string[1] { Form1.countryml[Form1.countrydict[Form1.countryid[Form1.makecountry]].Name] };
                    string admlink = util.mp(167, p167);
                    for (int i = 1; i < 5; i++)
                    {
                        if (!String.IsNullOrEmpty(admdict[Form1.makecountry].label[i - 1]))
                        {
                            if (!Form1.featurearticle.ContainsKey(admdict[Form1.makecountry].label[i - 1]))
                                Form1.featurearticle.Add(admdict[Form1.makecountry].label[i - 1], admlink);
                            else
                                Form1.featurearticle[admdict[Form1.makecountry].label[i - 1]] = admlink;
                        }
                        string fc = "PPLA";
                        if (i > 1)
                            fc += i.ToString();
                        if (admcap.ContainsKey(admdict[Form1.makecountry].label[i - 1]))
                        {
                            Form1.featuredict[fc] = admcap[admdict[Form1.makecountry].label[i - 1]];
                            if (!Form1.featurearticle.ContainsKey(admcap[admdict[Form1.makecountry].label[i - 1]]))
                                Form1.featurearticle.Add(admcap[admdict[Form1.makecountry].label[i - 1]], admlink);
                        }
                    }

                }
                else
                {

                }
            }





            //Console.ReadLine();

        }


    }

}
