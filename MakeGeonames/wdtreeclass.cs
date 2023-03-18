using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web;
using System.Net;
using DotNetWikiBot;

namespace MakeGeonames
{

    public class wdtreeclass
    {
        public static Dictionary<string, int> propdict = new Dictionary<string, int>(); //from wikidata property name to property id


        public List<int> uplinks = new List<int>();
        public List<int> downlinks = new List<int>();

        public static void fill_propdict()
        {
            propdict.Add("country", 17);
            propdict.Add("capital", 36);
            propdict.Add("commonscat", 373);
            propdict.Add("coat of arms", 94);
            propdict.Add("locatormap", 242);
            propdict.Add("flag", 41);
            propdict.Add("timezone", 421);
            propdict.Add("kids", 150);
            propdict.Add("parent", 131);
            propdict.Add("iso", 300);
            propdict.Add("borders", 47);
            propdict.Add("coordinates", 625);
            propdict.Add("inception", 571);
            propdict.Add("head of government", 6);
            propdict.Add("gnid", 1566);
            propdict.Add("follows", 155);
            propdict.Add("category dead", 1465);
            propdict.Add("category born", 1464);
            propdict.Add("category from", 1792);
            propdict.Add("image", 18);
            propdict.Add("banner", 948);
            //propdict.Add("sister city",190);
            propdict.Add("postal code", 281);
            propdict.Add("position", 625);
            propdict.Add("population", 1082);
            propdict.Add("instance", 31);
            propdict.Add("subclass", 279);
            propdict.Add("nexttowater", 206);

            //propdict.Add("",);

        }

        public static String stripNonValidXMLCharacters(string textIn)
        {
            StringBuilder textOut = new StringBuilder(); // Used to hold the output.
            char current; // Used to reference the current character.


            if (textIn == null || textIn == string.Empty) return string.Empty; // vacancy test.
            for (int i = 0; i < textIn.Length; i++)
            {
                current = textIn[i];


                if ((current == 0x9 || current == 0xA || current == 0xD) ||
                    ((current >= 0x20) && (current <= 0xD7FF)) ||
                    ((current >= 0xE000) && (current <= 0xFFFD)))
                //||                ((current >= 0x10000) && (current <= 0x10FFFF)))
                {
                    textOut.Append(current);
                }
            }
            return textOut.ToString();
        }



        public static string get_webpage(string url)
        {
            WebClient client = new WebClient();

            // Add a user agent header in case the
            // requested URI contains a query.

            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            try
            {
                Stream data = client.OpenRead(url);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                return s;
            }
            catch (WebException e)
            {
                string message = e.Message;
                Console.Error.WriteLine(message);
            }
            return "";
        }


        public static string getxmlpar(string par, XmlNode node)
        {

            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == par)
                {
                    string s = xn.InnerXml;
                    return s;
                }


            }

            return "";
        }

        public static List<int> get_wd_prop_idlist(int propid, XmlDocument cx)
        {
            List<int> rl = new List<int>();
            XmlDocument propx = new XmlDocument();
            XmlNode propnode = get_property_node(propid, cx);
            if (propnode == null)
                return rl;

            propx.AppendChild(propx.ImportNode(propnode, true));
            //  teamDoc.AppendChild(teamDoc.ImportNode(teamNode, true));
            XmlNodeList mslist = propx.GetElementsByTagName("mainsnak");
            foreach (XmlNode msn in mslist)
            {
                XmlDocument msx = new XmlDocument();
                msx.AppendChild(msx.ImportNode(msn, true));
                XmlNodeList elemlist = msx.GetElementsByTagName("value");
                foreach (XmlNode ee in elemlist)
                {
                    try
                    {
                        int rs = util.tryconvert(ee.Attributes.GetNamedItem("numeric-id").Value);
                        if (rs > 0)
                            rl.Add(rs);
                    }
                    catch (NullReferenceException e)
                    {
                        Console.Error.WriteLine(e.Message);
                    }
                }
            }

            return rl;

        }


        public static string get_wd_prop(int propid, XmlDocument cx)
        {
            XmlNode propnode = get_property_node(propid, cx);
            if (propnode == null)
                return "";

            //Console.WriteLine("propnode = " + propnode.ToString());

            XmlDocument propx = new XmlDocument();
            propx.AppendChild(propx.ImportNode(propnode, true));

            XmlNodeList elemlist = propx.GetElementsByTagName("datavalue");
            string rs = "";
            foreach (XmlNode ee in elemlist)
            {
                try
                {
                    rs = ee.Attributes.GetNamedItem("value").Value;
                }
                catch (NullReferenceException e)
                {
                    Console.Error.WriteLine(e.Message);
                }
            }

            Console.WriteLine("get_wd_prop:rs: " + rs);
            return rs;

        }

        public static double[] get_wd_position(XmlDocument cx)
        {
            double[] rl = { 9999.0, 9999.0 };

            XmlNode propnode = get_property_node(propdict["position"], cx);
            if (propnode == null)
                return rl;

            //Console.WriteLine("propnode = " + propnode.ToString());

            XmlDocument propx = new XmlDocument();
            propx.AppendChild(propx.ImportNode(propnode, true));

            XmlNodeList elemlist = propx.GetElementsByTagName("value");
            //string rs = "";
            foreach (XmlNode ee in elemlist)
            {
                try
                {
                    rl[0] = util.tryconvertdouble(ee.Attributes.GetNamedItem("latitude").Value);
                    rl[1] = util.tryconvertdouble(ee.Attributes.GetNamedItem("longitude").Value);
                }
                catch (NullReferenceException e)
                {
                    Console.Error.WriteLine(e.Message);
                }
            }

            Console.WriteLine("get_wd_prop:rl: " + rl[0].ToString() + " | " + rl[1].ToString());
            return rl;

        }


        public static string wdlink(string prop)
        {
            if (!propdict.ContainsKey(prop))
            {
                Console.WriteLine("Invalid property: " + prop);
                return "";
            }

            string s = "{{#property:P" + propdict[prop].ToString() + "}}";
            return s;
        }

        public static XmlDocument get_wd_xml(int wdid)
        {
            string url = "https://www.wikidata.org/w/api.php?action=wbgetentities&format=xml&ids=Q" + wdid.ToString() + "&redirects=yes";
            XmlDocument xd = new XmlDocument();
            string s = get_webpage(url);
            if (String.IsNullOrEmpty(s))
                return null;
            //Console.WriteLine(s);
            try
            {
                xd.LoadXml(s);
            }
            catch (XmlException e)
            {
                string message = e.Message;
                Console.Error.WriteLine("tl we " + message);
                return null;
            }

            return xd;
        }

        public static int get_wd_gnid(int wdid)
        {
            XmlDocument cx = get_wd_xml(wdid);
            if (cx == null)
                return -1;
            else
                return util.tryconvert(get_wd_prop(propdict["gnid"], cx));

        }

        public static XmlNode get_property_node(int propid, XmlDocument cx)
        {
            XmlNodeList elemlist = cx.GetElementsByTagName("property");
            //Console.WriteLine("get_property_node: elemlist: " + elemlist.Count.ToString());
            foreach (XmlNode ee in elemlist)
            {
                try
                {
                    string id = ee.Attributes.GetNamedItem("id").Value;
                    //Console.WriteLine("id = " + id);
                    if (id == "P" + propid.ToString())
                    {
                        //Console.WriteLine("get_property_node: found!");
                        return ee;
                    }
                }
                catch (NullReferenceException e)
                {
                    Console.Error.WriteLine(e.Message);
                }
            }

            //Console.WriteLine("get_property_node: not found");
            return null;

        }


        public static List<int> get_wd_kids(XmlDocument cx)
        {
            List<int> rl = new List<int>();
            List<int> rldouble = new List<int>();
            XmlDocument ee = new XmlDocument();
            XmlNode propnode = get_property_node(150, cx);
            if (propnode == null)
                return rl;

            ee.AppendChild(ee.ImportNode(propnode, true));

            //ee.DocumentElement.AppendChild(propnode);

            //XmlNode ee = get_property_node(150, cx);
            if (ee == null)
                return rl;

            XmlNodeList elemlist = ee.GetElementsByTagName("value");
            Console.WriteLine("get-wd_kids: elemlist " + elemlist.Count.ToString());

            foreach (XmlNode eee in elemlist)
            {
                Console.WriteLine("eee.Attributes: " + eee.Attributes.ToString());
                try
                {
                    string etype = eee.Attributes.GetNamedItem("entity-type").Value;
                    Console.WriteLine("etype = " + etype);
                    if (etype == "item")
                    {
                        string id = eee.Attributes.GetNamedItem("numeric-id").Value;
                        Console.WriteLine("id = " + id);
                        int iid = util.tryconvert(id);
                        if (iid > 0)
                        {
                            if (!rl.Contains(iid))
                                rl.Add(iid);
                            else if (!rldouble.Contains(iid))
                                rldouble.Add(iid);
                        }
                    }

                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e);
                }
            }

            foreach (int ii in rldouble)
                rl.Remove(ii);

            return rl;
        }

        public static Dictionary<string, string> get_wd_sitelinks(XmlDocument cx)
        {
            Dictionary<string, string> rd = new Dictionary<string, string>();

            XmlNodeList elemlist = cx.GetElementsByTagName("sitelink");
            foreach (XmlNode ee in elemlist)
            {
                try
                {
                    string lang = ee.Attributes.GetNamedItem("site").Value;
                    string value = ee.Attributes.GetNamedItem("title").Value;
                    //Console.WriteLine("get_wd_sitelinks: lang,value : " + lang + " " + value);
                    if (!rd.ContainsKey(lang))
                    {
                        rd.Add(lang, value);
                    }
                }
                catch (NullReferenceException e)
                {
                    Form1.eglob = e;
                }
            }

            return rd;
        }

        public static string iwlinks(XmlDocument cx)
        {
            Dictionary<string, string> rd = get_wd_sitelinks(cx);

            string iws = "\n\n";

            foreach (string sw in rd.Keys)
            {
                string s = sw.Replace("wiki", "");
                if (s == Form1.makelang)
                    return "Exists already:" + rd[sw];
                if ((s.Length == 2) || (s.Length == 3))
                    iws += "[[" + s + ":" + rd[sw] + "]]\n";
            }
            //Console.WriteLine("iwlinks: " + iws);

            return iws;
        }


        public static Dictionary<string, string> get_wd_name_dictionary(XmlDocument cx)
        {

            Dictionary<string, string> rd = new Dictionary<string, string>();

            XmlNodeList elemlist = cx.GetElementsByTagName("label");
            foreach (XmlNode ee in elemlist)
            {
                try
                {
                    string lang = ee.Attributes.GetNamedItem("language").Value;
                    string value = ee.Attributes.GetNamedItem("value").Value;
                    if (!rd.ContainsKey(lang))
                    {
                        rd.Add(lang, value);
                    }
                }
                catch (NullReferenceException e)
                {
                    Console.Error.WriteLine(e.Message);
                }
            }

            return rd;
        }

        public static string get_wd_name_from_xml(XmlDocument cx)
        {
            return get_wd_name_from_xml(cx, Form1.makelang);
        }

        public static string get_wd_name_from_xml(XmlDocument cx, string mainlang)
        {

            Dictionary<string, string> rd = new Dictionary<string, string>();

            if (cx == null)
                return "";

            XmlNodeList elemlist = cx.GetElementsByTagName("label");
            Console.WriteLine("elemlist " + elemlist.Count.ToString());
            foreach (XmlNode ee in elemlist)
            {
                try
                {
                    string lang = ee.Attributes.GetNamedItem("language").Value;
                    string value = ee.Attributes.GetNamedItem("value").Value;
                    //Console.WriteLine("lang,value = " + lang +"|"+ value);
                    if (!rd.ContainsKey(lang))
                    {
                        if (lang == mainlang)
                            return value;
                        else
                            rd.Add(lang, value);
                    }
                }
                catch (NullReferenceException e)
                {
                    //Console.Error.WriteLine(e.Message);
                    Form1.eglob = e;
                }
            }

            //Pick the most common form:
            Dictionary<string, int> namestats = new Dictionary<string, int>();
            foreach (string lang in rd.Keys)
            {
                if (!namestats.ContainsKey(rd[lang]))
                    namestats.Add(rd[lang], 0);
                namestats[rd[lang]]++;
            }
            string name = "";
            int maxuse = 0;
            foreach (string nn in namestats.Keys)
            {
                Console.WriteLine(nn);
                if (namestats[nn] > maxuse)
                {
                    maxuse = namestats[nn];
                    name = nn;
                    Console.WriteLine("maxuse = " + maxuse.ToString());
                }
            }

            Console.WriteLine("get_wd_name_from_xml " + name);

            return name;

        }

        public static string get_wd_name(int wdid)
        {
            string url = "https://www.wikidata.org/w/api.php?action=wbgetentities&ids=Q" + wdid.ToString() + "&props=labels&format=xml";
            string xmlitem = get_webpage(url);
            //Console.WriteLine(xmlitem);
            if (String.IsNullOrEmpty(xmlitem))
                return "";

            XmlDocument cx = new XmlDocument();
            cx.LoadXml(xmlitem);

            return get_wd_name_from_xml(cx);
        }

        public static int get_wd_item_direct(int gnid)
        {

            //http://wdq.wmflabs.org/api?q=string[1566:"2715459"]&format=xml
            string url0 = "http://wdq.wmflabs.org/api?q=string[1566:\"" + gnid.ToString() + "\"]";
            string hit = get_webpage(url0);
            string s = "";
            if (hit.IndexOf("items\":[") > 0)
            {
                s = hit.Substring(hit.IndexOf("items\":[") + 8);
                if (s.IndexOf("]") > 0)
                {
                    s = s.Substring(0, s.IndexOf("]"));
                    //Console.WriteLine("get_wd_item; s = " + s);
                    string[] items0 = s.Split(',');
                    //Console.WriteLine("Direct gnid query");
                    foreach (string item in items0)
                    {
                        if (util.tryconvert(item) > 0)
                        {
                            Form1.nwdhist.Add("direct gnid");
                            if (!Form1.wdgniddict.ContainsKey(util.tryconvert(item)))
                                Form1.wdgniddict.Add(util.tryconvert(item), gnid);
                            return util.tryconvert(item);
                        }
                    }
                }
            }
            return -1;
        }

        public static int get_wd_item(int gnid)
        {
            int wdid = get_wd_item_direct(gnid);
            if (wdid > 0)
                return wdid;


            string url1 = "http://wdq.wmflabs.org/api?q=around[625," + Form1.gndict[gnid].latitude.ToString(Form1.culture_en) + "," + Form1.gndict[gnid].longitude.ToString(Form1.culture_en) + ",2]";
            string around = get_webpage(url1);
            string s = "";
            if (around.IndexOf("items\":[") >= 0)
            {
                s = around.Substring(around.IndexOf("items\":[") + 8);
                if (s.IndexOf("]") > 0)
                {
                    s = s.Substring(0, s.IndexOf("]"));
                    string[] items = s.Split(',');

                    //List<string> withgnid = new List<string>();
                    //Console.WriteLine("Search by location and gnid");
                    //foreach (string item in items)
                    //{
                    //    Console.WriteLine("item = " + item);
                    //    string url2 = "https://www.wikidata.org/w/api.php?action=wbgetclaims&entity=Q" + item + "&format=xml&property=p1566";
                    //    string xmlitem = get_webpage(url2);

                    //    if (String.IsNullOrEmpty(xmlitem))
                    //        continue;

                    //    XmlDocument cx = new XmlDocument();
                    //    cx.LoadXml(xmlitem);

                    //    XmlNodeList elemlist = cx.GetElementsByTagName("datavalue");
                    //    foreach (XmlNode ee in elemlist)
                    //    {
                    //        try
                    //        {
                    //            string value = ee.Attributes.GetNamedItem("value").Value;
                    //            Console.WriteLine("value = " + value);
                    //            if (util.tryconvert(value) == gnid)
                    //            {
                    //                nwdhist.Add("loc&gnid");
                    //                if (!wdgniddict.ContainsKey(wdid))
                    //                    wdgniddict.Add(util.tryconvert(item), gnid);

                    //                return util.tryconvert(item);
                    //            }
                    //            else if (util.tryconvert(value) > 0)
                    //                withgnid.Add(item);
                    //        }
                    //        catch (NullReferenceException e)
                    //        {
                    //        }
                    //    }


                    //}

                    Console.WriteLine("Search by name at location");
                    foreach (string item in items)
                    {
                        //if (withgnid.Contains(item))
                        //    continue;

                        XmlDocument cx = get_wd_xml(util.tryconvert(item));
                        if (cx == null)
                            continue;
                        XmlNodeList elemlist = cx.GetElementsByTagName("label");
                        foreach (XmlNode ee in elemlist)
                        {
                            try
                            {
                                string value = ee.Attributes.GetNamedItem("value").Value;
                                //Console.WriteLine("value = " + value);
                                if (value == Form1.gndict[gnid].Name)
                                {
                                    Form1.nwdhist.Add("name at loc");
                                    if (!Form1.wdgniddict.ContainsKey(util.tryconvert(item)))
                                        Form1.wdgniddict.Add(util.tryconvert(item), gnid);
                                    return util.tryconvert(item);
                                }
                            }
                            catch (NullReferenceException e)
                            {
                                Console.Error.WriteLine(e.Message);
                            }
                        }

                    }
                }
            }

            Console.WriteLine("Search by article name in iwlangs");

            string sites = "";
            foreach (string iws in Form1.iwlang)
            {
                if (!String.IsNullOrEmpty(sites))
                    sites += "|";
                if (iws != Form1.makelang)
                    sites += iws + "wiki";
                else if (!String.IsNullOrEmpty(Form1.countrydict[Form1.countryid[Form1.makecountry]].nativewiki))
                    sites += Form1.countrydict[Form1.countryid[Form1.makecountry]].nativewiki + "wiki";
            }

            List<string> titlist = new List<string>();
            titlist.Add(Form1.gndict[gnid].Name);
            if (!titlist.Contains(Form1.gndict[gnid].Name_ml))
                titlist.Add(Form1.gndict[gnid].Name_ml);
            foreach (string an in Form1.gndict[gnid].altnames)
                if (!titlist.Contains(an))
                    titlist.Add(an);
            string titles = "";
            foreach (string tit in titlist)
            {
                if (!String.IsNullOrEmpty(tit))
                {
                    if (!String.IsNullOrEmpty(titles))
                        titles += "|";
                    titles += tit;
                }
            }

            //https://www.wikidata.org/w/api.php?action=wbgetentities&format=xml&sites=enwiki&titles=Austurland&redirects=yes&props=labels

            int entid = get_wdid_by_name(sites, titles, gnid);

            if (entid > 0)
                return entid;

            //string url3 = "https://www.wikidata.org/w/api.php?action=wbgetentities&format=xml&sites="+sites+"&titles="+titles+"&redirects=yes&props=claims";

            ////Console.WriteLine(url3);

            //string xmlitem3 = get_webpage(url3);

            //if (!String.IsNullOrEmpty(xmlitem3))
            //{
            //    XmlDocument cx = new XmlDocument();
            //    cx.LoadXml(xmlitem3);

            //    XmlNodeList elemlist = cx.GetElementsByTagName("entity");
            //    foreach (XmlNode ee in elemlist)
            //    {
            //        try
            //        {
            //            int entid = util.tryconvert(ee.Attributes.GetNamedItem("id").Value.Replace("Q",""));
            //            Console.WriteLine("entid = " + entid.ToString());
            //            if (entid > 0)
            //            {
            //                XmlDocument ex = new XmlDocument();
            //                ex.AppendChild(ex.ImportNode(ee, true));
            //                double[] latlong = get_wd_position(ex);
            //                if (latlong[0] + latlong[1] > 360.0)
            //                    continue;

            //                double dist = get_distance_latlong(latlong[0], latlong[1], Form1.gndict[gnid].latitude, Form1.gndict[gnid].longitude);

            //                Console.WriteLine("dist = " + dist.ToString());
            //                Console.WriteLine("gnid-latlong = " + Form1.gndict[gnid].latitude.ToString() + " | " + Form1.gndict[gnid].longitude.ToString());
            //                //Console.WriteLine("<ret>");
            //                //Console.ReadLine();

            //                if (dist < 100.0)
            //                {
            //                    nwdhist.Add("iw");
            //                    if (!wdgniddict.ContainsKey(entid))
            //                        wdgniddict.Add(entid, gnid);
            //                    return entid;
            //                }

            //            }

            //        }
            //        catch (NullReferenceException e)
            //        {
            //            Console.Error.WriteLine(e.Message);
            //        }
            //    }

            //}

            Console.WriteLine("Nothing found");
            return -1;
        }

        public static int get_wdid_by_name(string sites, string titles, int gnid)
        {
            string url3 = "https://www.wikidata.org/w/api.php?action=wbgetentities&format=xml&sites=" + sites + "&titles=" + titles + "&redirects=yes&props=claims";

            //Console.WriteLine(url3);

            string xmlitem3 = get_webpage(url3);

            if (!String.IsNullOrEmpty(xmlitem3))
            {
                XmlDocument cx = new XmlDocument();
                cx.LoadXml(xmlitem3);

                XmlNodeList elemlist = cx.GetElementsByTagName("entity");
                foreach (XmlNode ee in elemlist)
                {
                    try
                    {
                        int entid = util.tryconvert(ee.Attributes.GetNamedItem("id").Value.Replace("Q", ""));
                        Console.WriteLine("entid = " + entid.ToString());
                        if (entid > 0)
                        {
                            XmlDocument ex = new XmlDocument();
                            ex.AppendChild(ex.ImportNode(ee, true));
                            double[] latlong = get_wd_position(ex);
                            if (latlong[0] + latlong[1] > 360.0)
                                continue;

                            double dist = util.get_distance_latlong(latlong[0], latlong[1], Form1.gndict[gnid].latitude, Form1.gndict[gnid].longitude);

                            Console.WriteLine("dist = " + dist.ToString());
                            Console.WriteLine("gnid-latlong = " + Form1.gndict[gnid].latitude.ToString() + " | " + Form1.gndict[gnid].longitude.ToString());
                            //Console.WriteLine("<ret>");
                            //Console.ReadLine();

                            if (dist < 100.0)
                            {
                                Form1.nwdhist.Add("iw");
                                if (!Form1.wdgniddict.ContainsKey(entid))
                                    Form1.wdgniddict.Add(entid, gnid);
                                return entid;
                            }

                        }

                    }
                    catch (NullReferenceException e)
                    {
                        Console.Error.WriteLine(e.Message);
                    }
                }

            }
            return -1;
        }

        public static string get_name_from_wdid(int wdid)
        {
            int nbgnid = -1;
            if (Form1.wdgniddict.ContainsKey(wdid))
                nbgnid = Form1.wdgniddict[wdid];
            else
                nbgnid = get_wd_gnid(wdid);

            if (Form1.gndict.ContainsKey(nbgnid))
                return Form1.makegnidlink(nbgnid);
            else
                return get_wd_name(wdid);

        }

        public static bool check_wd_instance(int gnid, int wdid)
        {
            XmlDocument cx = get_wd_xml(wdid);
            bool foundtarget = false;

            if (cx != null)
            {
                List<int> instances = get_wd_prop_idlist(propdict["instance"], cx);

                Console.WriteLine("check_wd_instance " + instances.Count);

                //foreach (int inst in instances)
                {
                    string fcode = Form1.gndict[gnid].featurecode;

                    int target = 0;
                    string category = "default";
                    if (Form1.categorydict.ContainsKey(fcode))
                        category = Form1.categorydict[fcode];
                    if (category == "subdivisions")
                    {
                        target = Form1.catwdclass["subdivision1"];
                        foreach (int inst in instances)
                            if (Form1.search_rdf_tree(target, inst, 0))
                            {
                                foundtarget = true;
                                break;
                            }
                        if (!foundtarget)
                        {
                            target = Form1.catwdclass["subdivision2"];
                            foreach (int inst in instances)
                                if (Form1.search_rdf_tree(target, inst, 0))
                                {
                                    foundtarget = true;
                                    break;
                                }
                            if (!foundtarget)
                            {
                                target = Form1.catwdclass["subdivision3"];
                                foreach (int inst in instances)
                                    if (Form1.search_rdf_tree(target, inst, 0))
                                    {
                                        foundtarget = true;
                                        break;
                                    }
                            }
                        }
                    }
                    else if (category == "populated places")
                    {
                        //First do regular search...
                        target = Form1.catwdclass[category];
                        foreach (int inst in instances)
                            if (Form1.search_rdf_tree(target, inst, 0))
                            {
                                foundtarget = true;
                                break;
                            }

                        //...then use VETO on subdivision!
                        if (foundtarget)
                        {
                            bool foundsub = false;
                            target = Form1.catwdclass["subdivision1"];
                            foreach (int inst in instances)
                                if (Form1.search_rdf_tree(target, inst, 0))
                                {
                                    foundsub = true;
                                    break;
                                }
                            if (!foundsub)
                            {
                                target = Form1.catwdclass["subdivision2"];
                                foreach (int inst in instances)
                                    if (Form1.search_rdf_tree(target, inst, 0))
                                    {
                                        foundsub = true;
                                        break;
                                    }
                                if (!foundsub)
                                {
                                    target = Form1.catwdclass["subdivision3"];
                                    foreach (int inst in instances)
                                        if (Form1.search_rdf_tree(target, inst, 0))
                                        {
                                            foundsub = true;
                                            break;
                                        }
                                }
                            }
                            if (foundsub)
                                foundtarget = false;
                        }
                    }
                    else if (category == "mountains")
                    {
                        target = Form1.catwdclass["mountains1"];
                        foreach (int inst in instances)
                            if (Form1.search_rdf_tree(target, inst, 0))
                            {
                                foundtarget = true;
                                break;
                            }
                        if (!foundtarget)
                        {
                            target = Form1.catwdclass["mountains2"];
                            foreach (int inst in instances)
                                if (Form1.search_rdf_tree(target, inst, 0))
                                {
                                    foundtarget = true;
                                    break;
                                }
                        }
                    }
                    else
                    {
                        if (!Form1.catwdclass.ContainsKey(category))
                            category = "default";
                        target = Form1.catwdclass[category];
                        foreach (int inst in instances)
                            if (Form1.search_rdf_tree(target, inst, 0))
                            {
                                foundtarget = true;
                                break;
                            }
                    }


                }
            }

            return foundtarget;
        }

        public static void verify_wd()
        {
            Form1.read_rdf_tree();

            Dictionary<string, int> latlongobjects = new Dictionary<string, int>();
            List<int> withprop = new List<int>();
            int maxreadrdf = 10000000;

            int n = 0;
            //int n1566 = 0;
            int n625 = 0;

            //public class wdminiclass //minimal wikidata entry needed for verifying Geonames-links
            //{
            //    public int gnid = 0;
            //    public double latitude = 9999.9;
            //    public double longitude = 9999.9;
            //    public List<int> instance_of = new List<int>();
            //public double dist = 9999.9;
            //public bool okdist = false;
            //public bool okclass = false;
            //public bool goodmatch = false;

            //}

            Dictionary<int, wdminiclass> wdminidict = new Dictionary<int, wdminiclass>();

            //Console.WriteLine("First pass");
            //using (StreamReader sr = new StreamReader("wikidata-simple-statements.nt"))
            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "wikidata-only1566.nt"))
            {
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    if (line.Contains("P1566"))
                        Console.WriteLine(line);
                    rdfclass rc = Form1.rdf_parse(line);
                    n++;
                    if ((n % 10000) == 0)
                        Console.WriteLine("n = " + n.ToString());
                    if (n > maxreadrdf)
                        break;

                    if (rc.obj > 0)
                    {
                        if (!wdminidict.ContainsKey(rc.obj))
                        {
                            wdminiclass wdm = new wdminiclass();
                            wdminidict.Add(rc.obj, wdm);
                        }
                        if (rc.prop == propdict["gnid"])
                        {
                            wdminidict[rc.obj].gnid = util.tryconvert(rc.value);
                            Console.WriteLine("gnid = " + wdminidict[rc.obj].gnid.ToString());
                        }
                        else if (rc.prop == propdict["coordinates"])
                        {
                            if (!latlongobjects.ContainsKey(rc.value))
                            {
                                latlongobjects.Add(rc.value, rc.obj);
                                n625++;
                            }
                            //else
                            //    Console.WriteLine("Repeated latlong " + rc.value);
                        }
                        else if (rc.prop == propdict["instance"])
                        {
                            if (!wdminidict[rc.obj].instance_of.Contains(rc.objlink))
                                wdminidict[rc.obj].instance_of.Add(rc.objlink);
                        }


                    }
                    else
                    {
                        if (latlongobjects.ContainsKey(rc.objstring))
                        {
                            if (!wdminidict.ContainsKey(latlongobjects[rc.objstring]))
                            {
                                wdminiclass wdm = new wdminiclass();
                                wdminidict.Add(latlongobjects[rc.objstring], wdm);
                            }
                            if (rc.prop == 6250001)
                            {
                                wdminidict[latlongobjects[rc.objstring]].latitude = util.tryconvertdouble(rc.value);
                            }
                            else if (rc.prop == 6250002)
                            {
                                wdminidict[latlongobjects[rc.objstring]].longitude = util.tryconvertdouble(rc.value);
                            }
                        }
                    }

                }
            }

            Console.WriteLine("wdminidict: " + wdminidict.Count.ToString());

            Dictionary<int, List<int>> wddoubles = new Dictionary<int, List<int>>();
            Dictionary<int, int> gnidwddict = new Dictionary<int, int>();

            int nmountains = 0;
            int nranges = 0;
            int nrangesgood = 0;

            foreach (int wdid in wdminidict.Keys)
            {
                Console.WriteLine(wdid.ToString() + "; " + wdminidict[wdid].gnid.ToString() + "; " + wdminidict[wdid].latitude.ToString() + "; " + wdminidict[wdid].longitude.ToString());
                int gnid = wdminidict[wdid].gnid;
                if (Form1.gndict.ContainsKey(gnid))
                {
                    if (!Form1.wdgniddict.ContainsKey(wdid))
                        Form1.wdgniddict.Add(wdid, gnid);
                    else if (Form1.wdgniddict[wdid] != gnid)
                    {// negative numbers count how many duplicates
                        if (Form1.wdgniddict[wdid] > 0)
                            Form1.wdgniddict[wdid] = -2;
                        else
                            Form1.wdgniddict[wdid]--;
                    }

                    if (!gnidwddict.ContainsKey(gnid))
                        gnidwddict.Add(gnid, wdid);
                    else if (gnidwddict[gnid] != wdid)
                    {
                        if (!wddoubles.ContainsKey(gnid))
                        {
                            List<int> dlist = new List<int>();
                            wddoubles.Add(gnid, dlist);
                        }
                        if (gnidwddict[gnid] > 0)
                            wddoubles[gnid].Add(gnidwddict[gnid]);
                        wddoubles[gnid].Add(wdid);
                        Console.WriteLine("Double!");
                        if (gnidwddict[gnid] > 0)
                            gnidwddict[gnid] = -2;
                        else
                            gnidwddict[gnid]--;
                    }

                    wdminidict[wdid].dist = util.get_distance_latlong(Form1.gndict[gnid].latitude, Form1.gndict[gnid].longitude, wdminidict[wdid].latitude, wdminidict[wdid].longitude);
                    Console.WriteLine("dist = " + wdminidict[wdid].dist.ToString("F2"));

                    string fcode = Form1.gndict[gnid].featurecode;
                    double maxdist = 10.0; //maximum acceptable distance for a match
                    if (!Form1.featurepointdict[fcode]) //... hundred times larger for non-point features
                        maxdist = 300 * maxdist;

                    wdminidict[wdid].okdist = (wdminidict[wdid].dist < maxdist);
                    Console.WriteLine("dist = " + wdminidict[wdid].dist.ToString("F2") + ", " + wdminidict[wdid].okdist.ToString());

                    int target = 0;
                    bool foundtarget = false;
                    string category = "default";
                    if (Form1.categorydict.ContainsKey(fcode))
                        category = Form1.categorydict[fcode];
                    Console.WriteLine("category = " + category + ", instances: " + wdminidict[wdid].instance_of.Count.ToString());
                    if (category == "subdivisions")
                    {
                        target = Form1.catwdclass["subdivision1"];
                        foreach (int inst in wdminidict[wdid].instance_of)
                            if (Form1.search_rdf_tree(target, inst, 0))
                            {
                                foundtarget = true;
                                break;
                            }
                        if (!foundtarget)
                        {
                            target = Form1.catwdclass["subdivision2"];
                            foreach (int inst in wdminidict[wdid].instance_of)
                                if (Form1.search_rdf_tree(target, inst, 0))
                                {
                                    foundtarget = true;
                                    break;
                                }
                            if (!foundtarget)
                            {
                                target = Form1.catwdclass["subdivision3"];
                                foreach (int inst in wdminidict[wdid].instance_of)
                                    if (Form1.search_rdf_tree(target, inst, 0))
                                    {
                                        foundtarget = true;
                                        break;
                                    }
                            }
                        }
                    }
                    else if (category == "mountains")
                    {
                        Console.WriteLine("mountains " + fcode);
                        nmountains++;
                        if (fcode == "MTS")
                            nranges++;
                        target = Form1.catwdclass["mountains1"];
                        foreach (int inst in wdminidict[wdid].instance_of)
                            if (Form1.search_rdf_tree(target, inst, 0))
                            {
                                foundtarget = true;
                                break;
                            }
                        if (!foundtarget)
                        {
                            target = Form1.catwdclass["mountains2"];
                            foreach (int inst in wdminidict[wdid].instance_of)
                                if (Form1.search_rdf_tree(target, inst, 0))
                                {
                                    foundtarget = true;
                                    break;
                                }
                        }
                        if (foundtarget)
                            nrangesgood++;
                    }
                    else
                    {
                        if (!Form1.catwdclass.ContainsKey(category))
                            category = "default";
                        target = Form1.catwdclass[category];
                        foreach (int inst in wdminidict[wdid].instance_of)
                            if (Form1.search_rdf_tree(target, inst, 0))
                            {
                                foundtarget = true;
                                break;
                            }
                    }

                    wdminidict[wdid].okclass = foundtarget;
                }
                else
                    Console.WriteLine("gnid not found");
            }

            int ngood = 0;
            foreach (int wdid in wdminidict.Keys)
            {
                wdminidict[wdid].goodmatch = (wdminidict[wdid].okclass && wdminidict[wdid].okdist);
            }

            foreach (int gnid in wddoubles.Keys)
            {
                int nokclass = 0;
                int idok = -1;
                double bestdist = 9999.9;
                int idbestdist = -1;
                foreach (int wdid in wddoubles[gnid])
                {
                    if (wdminidict[wdid].okclass)
                    {
                        nokclass++;
                        idok = wdid;
                        if (wdminidict[wdid].okdist && (wdminidict[wdid].dist < bestdist))
                        {
                            idbestdist = wdid;
                            bestdist = wdminidict[wdid].dist;
                        }
                    }
                }
                if (nokclass > 1)
                    idok = idbestdist;
                foreach (int wdid in wddoubles[gnid])
                {
                    wdminidict[wdid].goodmatch = (wdid == idok);
                }
            }

            Console.WriteLine("wdminidict: " + wdminidict.Count.ToString());

            using (StreamWriter sw = new StreamWriter("wikidata-good.nt"))
            {

                foreach (int wdid in wdminidict.Keys)
                {
                    if (wdminidict[wdid].goodmatch)
                    {
                        sw.WriteLine(wdminidict[wdid].gnid.ToString() + "\t" + wdid.ToString());
                        ngood++;
                    }
                }
                Console.WriteLine("ngood = " + ngood.ToString());
            }

            Page pbad = new Page(Form1.makesite, "Användare:Lsjbot/Bad P1566 in Wikidata");
            pbad.text = "This is a list of wikidata items with dubious links to GeoNames (P1566)\n\n";

            pbad.text += "== Wrong type of object ==\n";
            pbad.text += "Feature code on GeoNames does not match InstanceOf (P31) on Wikidata\n\n";

            int nwrongtype = 0;
            foreach (int wdid in wdminidict.Keys)
            {
                if ((!wdminidict[wdid].okclass) && (wdminidict[wdid].okdist))
                {
                    pbad.text += "* [[:d:Q" + wdid.ToString() + "]]\n";
                    nwrongtype++;
                }
            }
            Console.WriteLine("nwrongtype = " + nwrongtype.ToString());

            pbad.text += "== Position mismatch ==\n";
            pbad.text += "Latitude/longitude on GeoNames does not match latitude/longitude (P625) on Wikidata\n\n";

            int nwrongpos = 0;
            foreach (int wdid in wdminidict.Keys)
            {
                if ((!wdminidict[wdid].okdist) && (wdminidict[wdid].okclass))
                {
                    pbad.text += "* [[:d:Q" + wdid.ToString() + "]]\n";
                    nwrongpos++;
                }

            }
            Console.WriteLine("nwrongpos = " + nwrongpos.ToString());

            pbad.text += "== Both position and type mismatch ==\n";
            pbad.text += "Latitude/longitude on GeoNames does not match latitude/longitude (P625) on Wikidata ''and'' ";
            pbad.text += "feature code on GeoNames does not match InstanceOf (P31) on Wikidata\n\n";

            int nwrongboth = 0;
            foreach (int wdid in wdminidict.Keys)
            {
                if ((!wdminidict[wdid].okdist) && (!wdminidict[wdid].okclass))
                {
                    pbad.text += "* [[:d:Q" + wdid.ToString() + "]]\n";
                    nwrongboth++;
                }
            }
            Console.WriteLine("nwrongboth = " + nwrongboth.ToString());

            pbad.text += "== Duplicate entries ==\n";
            pbad.text += "Several Wikidata objects have P1566 pointing to same GeoNames entry\n\n";

            int ndup = 0;
            foreach (int gnid in wddoubles.Keys)
            {
                pbad.text += "* GeoNames ID " + gnid.ToString() + "\n";
                foreach (int wdid in wddoubles[gnid])
                {
                    pbad.text += "** [[:d:Q" + wdid.ToString() + "]]\n";
                }
                ndup++;
            }
            Console.WriteLine("ndup = " + ndup.ToString());

            Console.WriteLine("nmountains = " + nmountains.ToString());
            Console.WriteLine("nranges = " + nranges.ToString());
            Console.WriteLine("nrangesgood = " + nrangesgood.ToString());

            //trysave(pbad,3);
            using (StreamWriter sw = new StreamWriter("wikidata-bad.txt"))
            {
                sw.WriteLine(pbad.text);
            }


        }



        public static void verify_wd_online()
        {
            Dictionary<string, Dictionary<string, int>> wdclass = new Dictionary<string, Dictionary<string, int>>();

            foreach (int gnid in Form1.gndict.Keys)
            {
                if (Form1.gndict[gnid].wdid < 0)
                    continue;

                XmlDocument cx = get_wd_xml(Form1.gndict[gnid].wdid);

                int gnidwd = util.tryconvert(get_wd_prop(propdict["gnid"], cx));

                if ((gnidwd > 0) && (gnidwd != gnid))
                {
                    Console.WriteLine("Different gnid in wikidata");
                    continue;
                }

                double[] latlong = get_wd_position(cx);
                double dist = util.get_distance_latlong(Form1.gndict[gnid].latitude, Form1.gndict[gnid].longitude, latlong[0], latlong[1]);
                Console.WriteLine("distance = " + dist.ToString());
                string wdname = get_wd_name_from_xml(cx);
                Console.WriteLine(Form1.gndict[gnid].Name + " / " + wdname);

                List<int> instances = get_wd_prop_idlist(propdict["instance"], cx);

                foreach (int inst in instances)
                {
                    XmlDocument cxi = get_wd_xml(inst);
                    string instancename = get_wd_name_from_xml(cxi, "en");
                    if (!wdclass.ContainsKey(Form1.gndict[gnid].featurecode))
                    {
                        Dictionary<string, int> dd = new Dictionary<string, int>();
                        wdclass.Add(Form1.gndict[gnid].featurecode, dd);
                    }
                    if (!wdclass[Form1.gndict[gnid].featurecode].ContainsKey(instancename))
                        wdclass[Form1.gndict[gnid].featurecode].Add(instancename, 0);
                    wdclass[Form1.gndict[gnid].featurecode][instancename]++;

                    List<int> subclassof = new List<int>();
                    int k = 0;
                    do
                    {
                        subclassof.Clear();
                        subclassof = get_wd_prop_idlist(propdict["subclass"], cxi);
                        foreach (int sc in subclassof)
                        {
                            cxi = get_wd_xml(sc);
                            string scname = get_wd_name_from_xml(cxi, "en");
                            Console.WriteLine("scname = " + scname);
                            if (!wdclass.ContainsKey(Form1.gndict[gnid].featurecode))
                            {
                                Dictionary<string, int> dd = new Dictionary<string, int>();
                                wdclass.Add(Form1.gndict[gnid].featurecode, dd);
                            }
                            if (!wdclass[Form1.gndict[gnid].featurecode].ContainsKey(scname))
                                wdclass[Form1.gndict[gnid].featurecode].Add(scname, 0);
                            wdclass[Form1.gndict[gnid].featurecode][scname]++;
                        }
                        k++;
                    }
                    while ((k < 7) && (subclassof.Count > 0));
                }
            }

            using (StreamWriter sw = new StreamWriter("gnvswiki-instance.txt"))
            {

                foreach (string fc in wdclass.Keys)
                {
                    sw.WriteLine(fc);
                    foreach (string sc in wdclass[fc].Keys)
                        sw.WriteLine("   " + sc + " " + wdclass[fc][sc].ToString());
                }
            }

            //propdict.Add("instance", 31);
            //propdict.Add("subclass", 279);

        }

        public static bool climb_wd_class_tree(int wdid, string targetclass, int depth)
        {
            if (depth > 7)
                return false;

            //bool found = false;

            XmlDocument cxi = get_wd_xml(wdid);
            string scname = get_wd_name_from_xml(cxi, "en");
            if (scname == targetclass)
                return true;
            else
            {
                List<int> subclassof = get_wd_prop_idlist(propdict["subclass"], cxi);
                foreach (int sc in subclassof)
                {
                    if (climb_wd_class_tree(sc, targetclass, depth + 1))
                    {
                        return true;
                    }
                }
            }

            return false;


        }


    }

}
