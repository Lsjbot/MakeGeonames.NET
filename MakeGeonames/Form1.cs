//#define DBGEOFLAG

using System;
using System.IO;
using DotNetWikiBot;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Threading;
using System.Web;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Data;
using static hbookclass;


#if (DBGEOFLAG)
using System.Data.Entity.Spatial;
#endif


using System.Windows.Forms;

namespace MakeGeonames
{
    public partial class Form1 : Form
    {

        public static string extractdir = @"O:\dotnwb3\extract\";      //will be modified according to computer name

        public static string botname = "Lsjbot";
        public static string makelang = "ceb";
        public static string makewiki = "ceb";
        //public static string makecountry =    "AD,AE,AF,AG,AI,AL,AM,AO,AQ,AR,AS,AT,AU,AW,AX,AZ,BA,BB,BD,BE,BF,BG,BH,BI,BJ,BL,BM,BN,BO,BQ,BR,BS,BT,BV,BW,BY,BZ,CA,CC,CD,CF,CG,CH,CI,CK,CL,CM,CN,CO,CR,CU,CV,CW,CX,CY,CZ,DE,DJ,DK,DM,DO,DZ,EC,EE,EG,EH,ER,ES,ET,FI,FJ,FK,FM,FO,FR,GA,GB,GD,GE,GF,GG,GH,GI,GL,GM,GN,GP,GQ,GR,GS,GT,GU,GW,GY,HK,HM,HN,HR,HT,HU,ID,IE,IL,IM,IN,IO,IQ,IR,IS,IT,JE,JM,JO,JP,KE,KG,KH,KI,KM,KN,KP,KR,XK,KW,KY,KZ,LA,LB,LC,LI,LK,LR,LS,LT,LU,LV,LY,MA,MC,MD,ME,MF,MG,MH,MK,ML,MM,MN,MO,MP,MQ,MR,MS,MT,MU,MV,MW,MX,MY,MZ,NA,NC,NE,NF,NG,NI,NL,NO,NP,NR,NU,NZ,OM,PA,PE,PF,PG,PH,PK,PL,PM,PN,PR,PS,PT,PW,PY,QA,RE,RO,RS,RU,RW,SA,SB,SC,SD,SS,SE,SG,SH,SI,SJ,SK,SL,SM,SN,SO,SR,ST,SV,SX,SY,SZ,TC,TD,TF,TG,TH,TJ,TK,TL,TM,TN,TO,TR,TT,TV,TW,TZ,UA,UG,UM,US,UY,UZ,VA,VC,VE,VG,VI,VN,VU,WF,WS,YE,YT,ZA,ZM,ZW"; //Can be comma-separated list. Must be same number of components in the following three strings.
        //public static string makecountry =    "AG,MT,MK,SS,BH,BT,LU,AD,AE,AF,AI,AL,AM,AO,AQ,AR,AS,AT,AU,AW,AX,AZ,BA,BB,BD,BE,BF,BG,BI,BJ,BL,BM,BN,BO,BQ,BR,BS,BV,BW,BY,BZ,CA,CC,CD,CF,CG,CH,CI,CK,CL,CM,CN,CO,CR,CU,CV,CW,CX,CY,CZ,DE,DJ,DK,DM,DO,DZ,EC,EE,EG,EH,ER,ES,ET,FI,FJ,FK,FM,FO,FR,GA,GB,GD,GE,GF,GG,GH,GI,GL,GM,GN,GP,GQ,GR,GS,GT,GU,GW,GY,HK,HM,HN,HR,HT,HU,ID,IE,IL,IM,IN,IO,IQ,IR,IS,IT,JE,JM,JO,JP,KE,KG,KH,KI,KM,KN,KP,KR,XK,KW,KY,KZ,LA,LB,LC,LI,LK,LR,LS,LT,LU,LV,LY,MA,MC,MD,ME,MF,MG,MH,ML,MM,MN,MO,MP,MQ,MR,MS,MU,MV,MW,MX,MY,MZ,NA,NC,NE,NF,NG,NI,NL,NO,NP,NR,NU,NZ,OM,PA,PE,PF,PG,PH,PK,PL,PM,PN,PR,PS,PT,PW,PY,QA,RE,RO,RS,RU,RW,SA,SB,SC,SD,SE,SG,SH,SI,SJ,SK,SL,SM,SN,SO,SR,ST,SV,SX,SY,SZ,TC,TD,TF,TG,TH,TJ,TK,TL,TM,TN,TO,TR,TT,TV,TW,TZ,UA,UG,UM,US,UY,UZ,VA,VC,VE,VG,VI,VN,VU,WF,WS,YE,YT,ZA,ZM,ZW"; //Can be comma-separated list. Must be same number of components in the following three strings.
        public static string makecountry = "";//,AU,AW,AX,AZ";//"RU,UA,BY,RS,MK,KZ,KG,MN,BG,TJ";
        public static string makecountryname = "";//"Afghanistan,Albania,Antarctica,Armenia,Andorra,Burundi,Nicaragua,Bahamas,Macedonia,South Sudan,Bhutan,Luxembourg,Malta,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr,fr"; //Används av Makefork, samma engelska namnform som i namefork-filen
        //public static string makecountrywiki = "en";//"ru,ua,be,sr,mk,kk,ky,mn,bg,tg";
        //public static string makecountrywiki ="ca,ar,fa,en,en,sq,hy,pt,en,es,en,de,en,nl,fi,az,bs,en,bn,nl,fr,bg,ar,fr,fr,fr,en,ms,es,nl,pt,en,dz,en,en,be,en,en,ms,fr,fr,fr,de,fr,en,es,en,zh,es,es,es,pt,nl,en,el,cs,de,fr,da,en,es,ar,es,et,ar,ar,aa,es,am,fi,en,en,en,fo,fr,fr,en,en,ka,fr,en,en,en,kl,en,fr,fr,es,el,en,es,en,pt,en,zh,en,es,hr,ht,hu,id,en,he,en,en,en,ar,fa,is,it,en,en,ar,ja,en,ky,km,en,ar,en,ko,ko,sq,ar,en,kk,lo,ar,en,de,si,en,en,lt,lb,lv,ar,ar,fr,ro,sr,fr,fr,mh,mk,fr,my,mn,zh,fi,fr,ar,en,mt,en,dv,ny,es,ms,pt,en,fr,fr,en,en,es,nl,no,ne,na,ni,en,ar,es,es,fr,en,tl,ur,pl,fr,en,en,ar,pt,pa,es,ar,fr,ro,sr,ru,rw,ar,en,en,ar,en,sv,cm,en,sl,no,sk,en,it,fr,so,nl,pt,es,nl,ar,en,en,fr,fr,fr,th,tg,tk,te,tk,ar,to,tr,en,tv,zh,sw,uk,en,en,en,es,uz,la,en,es,en,en,vi,bi,wl,sm,ar,fr,zu,en,en"; //Används för iw
        public static int resume_at = -1; //-1: run from start; gnid to resume at.
        public static int stop_at = -1; //-1: run to end; gnid to stop at.
        public static string resume_at_fork = ""; //resume at for fork pages; empty string to run from start
        public static string resume_at_map = ""; //resume at for map pages; empty string to run from start NOT WORKING
        public static string testprefix = "";
        public static string kmlprefix = "Wikipedia:KML/";
        //public static string nameforkdate = "20150803";
        public static char createclass = ' '; //create only articles with this feature class
        public static char createexceptclass = ' '; //create articles EXCEPT with this feature class
        public static string createfeature = ""; //create only articles with this feature code
        public static string createexceptfeature = "RK,RKS,AIRF,CSTL"; //create all articles EXCEPT with this feature code
        public static string createcategory = ""; //create only articles in this category
        public static string createexceptcategory = ""; //create all articles EXCEPT in this category
        public static int createunit = -1; //create only articles for place in admin.unit with gnid=createunit; <0 to make all; =0 to make only provinceless
        public static int createexceptunit = -1; //create only articles for place NOT in admin.unit with gnid=createexceptunit; <0 to make all

        //======================================================
        // Flags setting the main task for a run.
        // Unpredictable results if multiple flags are true.
        //======================================================
        public static bool makearticles = false; //create wiki pages
        public static bool makespecificarticles = false; //create specific wiki pages, one by one
        public static bool remakearticleset = false; //remake a selected set of articles; overwrite must be true
        public static bool altnamesonly = false; //create namefork file
        public static bool makefork = false; //create fork pages on wiki
        public static bool checkdoubles = false; //create artname file
        public static bool checkwikidata = false; //create wikidata-XX files
        public static bool makeislands = false; //create islands-XX files
        public static bool makelakes = false; //create lakes-XX files
        public static bool makerivers = false; //create rivers-XX files
        public static bool makeranges = false; //create ranges-XX files
        public static bool verifygeonames = false; //doublecheck geonames against wiki
        public static bool verifywikidata = false; //doublecheck wikidata
        public static bool verifyislands = false; //verify island files, removing duplicates
        public static bool verifylakes = false; //verify island files, removing duplicates
        public static bool makealtitude = false;    //make altitude_XX files
        public static bool maketranslit = false;    //make translit_XX files
        public static bool makeworldmaponly = false; //create world map
        public static bool statisticsonly = false; //gather statistics without actually making anything; can be combined with other options
        public static bool savefeaturelink = false; //save a list of what feature codes link to
        public static bool savewikilinks = false; //save a list of what the bot wikilinks to
        public static bool saveadmlinks = false; //save a list of what the bot wikilinks to
        public static bool manualcheck = false; //check name matches manually, if true
        public static bool listnative = false; //list article names from native wiki of a country
        public static bool forkduplicates = false; //make namefork-duplicates file
        public static bool fixsizecats = false; //fix size categories
        public static bool testnasa = false; //test nasa data
        public static bool retrofitnasa = false; // retrofit existing articles with nasa data
        public static int resurrection = 0; //if >0, make only gnid in resurrected; if <0 skip resurrected; if 0, disregard.
        public static bool checkminutes = false; //check coordinates if rounded to whole minutes
        public static bool countrycenters = false;
        public static bool wdthread = true; //check wd on the fly, in separate thread
        public static bool listfeature = false; //list all features of type createfeature
        public static bool fixppla = false; //fix bug in PPLA links.
        public static bool fixmapedges = false; //fix names too close to edge in location maps.

        public static bool prefergeonamespop = true; //use population figures from GeoNames rather than wiki
        public static bool makedoubles = true;      //if suspected double, make article as a copy in doubleprefix folder
        public static bool overwrite = false;       //if article already exists, overwrite with a new version (if not human-edited)

        public static bool reallymake = true;  //if false, do dry run without actually loading from or saving to wiki; can be combined with other options
        public static bool pauseaftersave = false;  //if true, wait for keypress after saving each article
        public static bool makehtml = true;

        public static bool firstround = true;

        public static int maxread = 100000000; //Set to a small number for a limited test run, or to 100000000 for a full run

        public static bool threadstop = false;
        public static bool threadrunning = false;
        public static int threadmax = 5;

#if (DBGEOFLAG)
        public class shapeclass
        {
            public Dictionary<string, string> metadict = new Dictionary<string, string>();
            public List<DbGeography> shapes = new List<DbGeography>();
            public List<DbGeometry> mshapes = new List<DbGeometry>();
        }
#endif

        public static Dictionary<int, geonameclass> gndict = new Dictionary<int, geonameclass>();
        public static Dictionary<int, countryclass> countrydict = new Dictionary<int, countryclass>(); //from geoname ID to country info
        public static Dictionary<string, int> countryid = new Dictionary<string, int>(); //from ISO code to geoname ID for countries
        public static Dictionary<string, string> countryml = new Dictionary<string, string>(); // from English name to makelang name
        public static Dictionary<string, string> countryiso = new Dictionary<string, string>(); // from English name to ISO
        public static Dictionary<string, locatorclass> locatordict = new Dictionary<string, locatorclass>(); // from English name to locator map name
        //public static Dictionary<string, string> locatorimage = new Dictionary<string, string>(); //from English name to locator map image
        public static Dictionary<string, List<int>> namefork = new Dictionary<string, List<int>>(); //names with list of corresponding geonameid(s)
        public static Dictionary<int, Dictionary<int, List<int>>> latlong = new Dictionary<int, Dictionary<int, List<int>>>(); //List of all places in same square degree
        public static Dictionary<int, Dictionary<int, List<int>>> exlatlong = new Dictionary<int, Dictionary<int, List<int>>>(); //List of all places in same square degree
        public static Dictionary<string, string> featuredict = new Dictionary<string, string>(); //From featurecode to feature name in makelang
        public static Dictionary<string, char> featureclassdict = new Dictionary<string, char>(); //From featurecode to feature class
        public static Dictionary<string, string> geoboxdict = new Dictionary<string, string>(); //From featurecode to geobox type
        public static Dictionary<string, string> geoboxtemplates = new Dictionary<string, string>(); //From geobox type to geobox template
        public static Dictionary<string, string> categorydict = new Dictionary<string, string>(); //from featurecode to category
        public static Dictionary<string, string> parentcategory = new Dictionary<string, string>(); //from category to parent category
        public static Dictionary<string, string> categoryml = new Dictionary<string, string>(); //from category to category name in makelang
        public static Dictionary<string, int> catstatdict = new Dictionary<string, int>(); //category statistics
        public static Dictionary<string, double> catnormdict = new Dictionary<string, double>(); //category statistics
        public static Dictionary<string, bool> featurepointdict = new Dictionary<string, bool>(); //true if feature is pointlike, false if extended
        public static Dictionary<string, bool> minutesensitivedict = new Dictionary<string, bool>(); //true if feature is sensitive to coordinate rounding errors
        public static List<string> noclimatelist = new List<string>(); //list of feature codes that should NOT have climate data in their articles
        public static Dictionary<int, islandclass> islanddict = new Dictionary<int, islandclass>();
        public static Dictionary<int, rangeclass> rangedict = new Dictionary<int, rangeclass>();
        public static Dictionary<int, lakeclass> lakedict = new Dictionary<int, lakeclass>();
        public static Dictionary<int, int> wdgniddict = new Dictionary<int, int>(); // from wdid to gnid
        //public static Dictionary<int, int> wdgnid = new Dictionary<int, int>(); //from wikidata id to geonames id; negative counts duplicates
        public static Dictionary<string, int> catwdclass = new Dictionary<string, int>(); //from category to appropriate wd top class
        public static Dictionary<string, List<string>> catwdinstance = new Dictionary<string, List<string>>(); //from category to list of appropriate wd instance_of
        public static Dictionary<int, existingclass> existingdict = new Dictionary<int, existingclass>(); //already existing articles
        public static Dictionary<int, existingclass> ghostdict = new Dictionary<int, existingclass>(); //towns with no known population
        public static Dictionary<string, string> motherdict = new Dictionary<string, string>(); //for overseas possessions: from territory ISO to mother country ISO
        public static Dictionary<int, string> specialfeaturedict = new Dictionary<int, string>(); //for places that are exceptions to the usual feature labels
        public static Dictionary<string, List<chinese_pop_class>> chinese_pop_dict = new Dictionary<string, List<chinese_pop_class>>();
        public static Dictionary<int, chinese_pop_class> chinese_pop_dict2 = new Dictionary<int, chinese_pop_class>();
        public static Dictionary<int, string> iatadict = new Dictionary<int, string>();
        public static Dictionary<int, string> icaodict = new Dictionary<int, string>();
        public static List<int> resurrected = new List<int>(); //gnid of articles "resurrected" from ghosts with new population data; mostly used in China.

        public static Dictionary<int, List<altnameclass>> altdict = new Dictionary<int, List<altnameclass>>();
        public static List<string> geoboxlist = new List<string>(); //List of geobox types used
        public static Dictionary<int, langclass> langdict = new Dictionary<int, langclass>(); //main language table
        public static Dictionary<string, int> langtoint = new Dictionary<string, int>(); //from iso to integer code. Both iso2 and iso3 used as keys to the same int
        public static Dictionary<int, string> artnamedict = new Dictionary<int, string>();
        public static Dictionary<int, string> oldartnamedict = new Dictionary<int, string>();
        //public static Dictionary<string, int> propdict = new Dictionary<string, int>(); //from wikidata property name to property id
        public static string[] iwlang = { "en", "fr", "de", "es" };
        public static Dictionary<string, Site> iwsites = new Dictionary<string, Site>();
        public static long minimum_population = 100;
        public static double minimum_area = 0.1;
        public static int minimum_prominence = 50;
        public static DateTime wdtime = new DateTime();
        public static DateTime gnfiledate = new DateTime();
        public static string dumpdate = "";
        public static bool hasnotes = false;
        public static Dictionary<long, long> popvspop = new Dictionary<long, long>(); //comparing population for same place, wd vs gn
        public static Dictionary<double, double> areavsarea = new Dictionary<double, double>(); //comparing area for same place, wd vs gn
        public static int nwdtot = 0;
        public static Exception eglob;
        public static bool locatoringeobox = false;  //only works in Swedish!
        public static List<string> forktemplates = new List<string>();
        public static Dictionary<string, string> featurearticle = new Dictionary<string, string>();//from feature name to article name for feature
        public static Dictionary<string, string> funny_quotes = new Dictionary<string, string>();
        public static List<string> donecountries = new List<string>();
        public static List<string> donecats = new List<string>();
        public static Dictionary<int, nasaclass> nasadict = new Dictionary<int, nasaclass>();
        public static Dictionary<string, int> climatemismatchdict = new Dictionary<string, int>();
        public static int pausetime = 5; //time between saves, modified depending on task
        public static List<string> nocapital = new List<string>(); //countries with no capital or capital filling country
        public static List<int> blacklist = new List<int>(); //list of geonames id NOT to create

#if (DBGEOFLAG)
        public static Dictionary<int, shapeclass> lakeshapedict = new Dictionary<int, shapeclass>();
        public static Dictionary<int, List<int>> countrylakedict = new Dictionary<int, List<int>>();
        public static Dictionary<string, shapeclass> rivershapedict = new Dictionary<string, shapeclass>();
        public static Dictionary<string, shapeclass> drainageshapedict = new Dictionary<string, shapeclass>();
        public static Dictionary<string, drainageclass> drainagedict = new Dictionary<string, drainageclass>();
        public static Dictionary<int, riverclass> rivernamedict = new Dictionary<int, riverclass>();
        public static Dictionary<int, riverclass> riverdict = new Dictionary<int, riverclass>();
        public static Dictionary<string, string> oceanstringdict = new Dictionary<string, string>();
        public static Dictionary<string, string> oceannamedict = new Dictionary<string, string>();
        public static Dictionary<string, DbGeography> oceandict = new Dictionary<string, DbGeography>();
#endif
        public static Dictionary<int, int> wdid_buffer = new Dictionary<int, int>();
        public static int resume_at_wdid = -1;

        public static string mapfilecache = "NO CACHE";
        public static int[,] mapcache = new int[3603, 3603];

        public static string doubleprefix = "Användare/Lsjbot/Dubletter";


        public static Page pconflict = null;
        public static Page panomaly = null;
        public static bool conflictheadline = false;
        public static bool anomalyheadline = false;

        public static hbookclass featurehist = new hbookclass("Feature hist");

        public static int nedit = 0;
        public static DateTime oldtime = DateTime.Now;

        public static List<string> refnamelist = new List<string>();
        public static string reflist = "<references>";
        public static string password = "";
        public static string tabstring = "\t";
        public static char tabchar = '\t';


        public static XmlDocument currentxml = new XmlDocument();
        //public static XmlNode currentnode;
        public static int wdid = -1;

        public static Site makesite;
        public static Site ensite;
        public static Site cmsite;
        //public static Site wdsite;
        public static Page pstats;

        public static int badelevation = -99999;

        public static CultureInfo culture = CultureInfo.CreateSpecificCulture("sv-SE");
        public static CultureInfo culture_en = CultureInfo.CreateSpecificCulture("en-US");
        public static NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        public static NumberFormatInfo nfi_en = new CultureInfo("en-US", false).NumberFormat;
        public static NumberFormatInfo nfi_space = new CultureInfo("en-US", false).NumberFormat;

        public static Dictionary<int, wdtreeclass> wdtree = new Dictionary<int, wdtreeclass>();

        public static hbookclass fchist = new hbookclass("fchist");
        public static hbookclass fcbad = new hbookclass("fcbad");
        public static hbookclass fcathist = new hbookclass("fcathist");
        public static hbookclass fclasshist = new hbookclass("fclasshist");
        public static hbookclass evarhist = new hbookclass("evarhist");
        public static hbookclass slope1hist = new hbookclass("slope1hist");
        public static hbookclass slope5hist = new hbookclass("slope5hist");
        public static hbookclass slopermshist = new hbookclass("slopermshist");
        public static hbookclass ndirhist = new hbookclass("ndirhist");
        public static hbookclass nsameterrhist = new hbookclass("nsameterrhist");
        public static hbookclass terrainhist = new hbookclass("terrainhist");
        public static hbookclass terraintexthist = new hbookclass("terraintexthist");
        public static hbookclass elevdiffhist = new hbookclass("elevdiffhist");
        public static hbookclass nwdhist = new hbookclass("nwdhist");
        public static hbookclass foverrephist = new hbookclass("foverrephist");

        public static statclass stats = new statclass();



        public static transliterationclass cyrillic = new transliterationclass();

        public static void make_translit()
        {
            int icountry = countryid[makecountry];
            using (StreamWriter sw = new StreamWriter("translit-" + makecountry + ".txt"))
            {
                foreach (int gnid in altdict.Keys)
                {
                    bool found = false;
                    string langtouse = countrydict[icountry].nativewiki;
                    foreach (altnameclass ac in altdict[gnid])
                    {
                        //if (countrydict[icountry].languages.Contains(ac.ilang))
                        {
                            if (langdict.ContainsKey(ac.ilang))
                            {
                                string langname = langdict[ac.ilang].iso2;
                                if (langname != langtouse)
                                    continue;
                                string alphabet = util.get_alphabet(ac.altname);
                                //Console.WriteLine(ac.altname + ": " + alphabet);
                                if (alphabet == "cyrillic")
                                {
                                    //Console.WriteLine("Transliteration: " + cyrillic.Transliterate(ac.altname, "ru"));
                                    sw.WriteLine(gnid.ToString() + tabstring + ac.altname + tabstring + cyrillic.Transliterate(ac.altname, langname));
                                    found = true;
                                }
                            }
                        }
                    }
                    if ((!found) && ((makecountry == "UA") || (makecountry == "BY") || (makecountry == "KZ") || (makecountry == "KG") || (makecountry == "TJ")))
                    {
                        langtouse = "ru";
                        foreach (altnameclass ac in altdict[gnid])
                        {
                            //if (countrydict[icountry].languages.Contains(ac.ilang))
                            {
                                if (langdict.ContainsKey(ac.ilang))
                                {
                                    string langname = langdict[ac.ilang].iso2;
                                    if (langname != langtouse)
                                        continue;
                                    string alphabet = util.get_alphabet(ac.altname);
                                    //Console.WriteLine(ac.altname + ": " + alphabet);
                                    if (alphabet == "cyrillic")
                                    {
                                        //Console.WriteLine("Transliteration: " + cyrillic.Transliterate(ac.altname, "ru"));
                                        sw.WriteLine(gnid.ToString() + tabstring + ac.altname + tabstring + cyrillic.Transliterate(ac.altname, langname));
                                        found = true;
                                    }
                                }
                            }
                        }
                    }
                }
                sw.Write("Badlist:");
                foreach (char c in cyrillic.badlist)
                    sw.Write(c);
                sw.WriteLine();
            }
            altdict.Clear();
        }

        public static string getgnidname(int gnid)
        {
            if (!Form1.gndict.ContainsKey(gnid))
                return null;
            else
                return Form1.gndict[gnid].Name_ml;

        }

        public static string getartname(int gnid)
        {
            if (!Form1.gndict.ContainsKey(gnid))
                return null;
            else
                return Form1.gndict[gnid].articlename.Replace("*", "");

        }

        public static void read_blacklist()
        {
            Page pblack = new Page(makesite, util.mp(13) + botname + "/svartlista");
            util.tryload(pblack, 2);
            foreach (string s in pblack.text.Split('\n'))
            {
                if (util.tryconvert(s) > 0)
                    blacklist.Add(util.tryconvert(s));
            }
        }

        public static void addnamefork(int geonameid, string Name)
        {
            string nn = Name.Trim();
            if (String.IsNullOrEmpty(nn))
                return;

            //Skip numbers and acronyms:
            if (util.tryconvert(Name) > 0)
                return;
            if ((Name.Length <= 3) && (Name == Name.ToUpper()))
                return;


            if (namefork.ContainsKey(Name))
            {
                if (!namefork[Name].Contains(geonameid))
                    namefork[Name].Add(geonameid);
            }
            else
            {
                List<int> ll = new List<int>();
                ll.Add(geonameid);
                namefork.Add(Name, ll);
            }
        }

        public static void read_existing_coord()
        {
            int n = 0;
            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "coord-" + makelang + ".txt"))
            {
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    //Console.WriteLine(line);
                    n++;
                    string[] words = line.Split('\t');

                    if (words.Length < 3)
                        continue;

                    existingclass ex = new existingclass();

                    ex.articlename = words[0];
                    ex.latitude = util.tryconvertdouble(words[1]);
                    ex.longitude = util.tryconvertdouble(words[2]);
                    existingdict.Add(n, ex);
                    addexistinglatlong(ex.latitude, ex.longitude, n);
                }
            }
        }

        public static void addexistinglatlong(double lat, double lon, int exid)
        {
            int ilat = Convert.ToInt32(Math.Truncate(lat));
            int ilong = Convert.ToInt32(Math.Truncate(lon));

            if (!exlatlong.ContainsKey(ilat))
            {
                Dictionary<int, List<int>> dd = new Dictionary<int, List<int>>();
                exlatlong.Add(ilat, dd);
            }
            if (!exlatlong[ilat].ContainsKey(ilong))
            {
                List<int> ll = new List<int>();
                exlatlong[ilat].Add(ilong, ll);
            }
            if (!exlatlong[ilat][ilong].Contains(exid))
                exlatlong[ilat][ilong].Add(exid);
        }

        public static List<int> getexisting(double lat, double lon, double radius)
        {
            List<int> ll = new List<int>();
            double kmdeg = 40000 / 360; //km per degree at equator
            double r2 = radius * radius / (kmdeg * kmdeg);
            double scale = Math.Cos(lat * 3.1416 / 180); //latitude-dependent longitude scale

            int ilat = Convert.ToInt32(Math.Truncate(lat));
            int ilong = Convert.ToInt32(Math.Truncate(lon));

            int cells = Convert.ToInt32(radius / kmdeg + 1);
            for (int u = -cells; u < (cells + 1); u++)
                for (int v = -cells; v < (cells + 1); v++)
                {
                    if (exlatlong.ContainsKey(ilat + u))
                        if (exlatlong[ilat + u].ContainsKey(ilong + v))
                            foreach (int gnn in exlatlong[ilat + u][ilong + v])
                            {
                                if (!existingdict.ContainsKey(gnn))
                                    continue;
                                //if ((existingdict[gnn].latitude == lat) && (existingdict[gnn].longitude == lon))
                                //    continue;
                                double dlat = existingdict[gnn].latitude - lat;
                                double dlon = (existingdict[gnn].longitude - lon) * scale;
                                if ((dlat * dlat + dlon * dlon) < r2)
                                    ll.Add(gnn);
                            }
                }
            return ll;
        }

        public static void addlatlong(double lat, double lon, int gnid)
        {
            int ilat = Convert.ToInt32(Math.Truncate(lat));
            int ilong = Convert.ToInt32(Math.Truncate(lon));

            if (!latlong.ContainsKey(ilat))
            {
                Dictionary<int, List<int>> dd = new Dictionary<int, List<int>>();
                latlong.Add(ilat, dd);
            }
            if (!latlong[ilat].ContainsKey(ilong))
            {
                List<int> ll = new List<int>();
                latlong[ilat].Add(ilong, ll);
            }
            if (!latlong[ilat][ilong].Contains(gnid))
                latlong[ilat][ilong].Add(gnid);
        }

        public static bool getghostneighbors(double lat, double lon, double radius)
        {
            double kmdeg = 40000 / 360; //km per degree at equator
            double r2 = radius * radius / (kmdeg * kmdeg);
            double scale = Math.Cos(lat * 3.1416 / 180); //latitude-dependent longitude scale

            foreach (int gnn in ghostdict.Keys)
            {
                double dlat = ghostdict[gnn].latitude - lat;
                double dlon = (ghostdict[gnn].longitude - lon) * scale;
                if ((dlat * dlat + dlon * dlon) < r2)
                    return true;
            }

            return false;
        }

        public static List<int> getneighbors(double lat, double lon, double radius)
        {
            List<int> ll = new List<int>();
            double kmdeg = 40000 / 360; //km per degree at equator
            double r2 = radius * radius / (kmdeg * kmdeg);
            double scale = Math.Cos(lat * 3.1416 / 180); //latitude-dependent longitude scale

            int ilat = Convert.ToInt32(Math.Truncate(lat));
            int ilong = Convert.ToInt32(Math.Truncate(lon));

            int cells = Convert.ToInt32(radius / kmdeg + 1);
            for (int u = -cells; u < (cells + 1); u++)
                for (int v = -cells; v < (cells + 1); v++)
                {
                    if (latlong.ContainsKey(ilat + u))
                        if (latlong[ilat + u].ContainsKey(ilong + v))
                            foreach (int gnn in latlong[ilat + u][ilong + v])
                            {
                                if (!Form1.gndict.ContainsKey(gnn))
                                    continue;
                                if ((Form1.gndict[gnn].latitude == lat) && (Form1.gndict[gnn].longitude == lon))
                                    continue;
                                double dlat = Form1.gndict[gnn].latitude - lat;
                                double dlon = (Form1.gndict[gnn].longitude - lon) * scale;
                                if ((dlat * dlat + dlon * dlon) < r2)
                                    ll.Add(gnn);
                            }
                }
            return ll;
        }

        public static List<int> getneighbors(int gnid, double radius) //radius in km!
        {
            List<int> ll = new List<int>();

            if (!Form1.gndict.ContainsKey(gnid))
                return ll;
            double lat = Form1.gndict[gnid].latitude;
            double lon = Form1.gndict[gnid].longitude;
            ll = getneighbors(lat, lon, radius);
            return ll;
        }

        public static void read_geoboxes()
        {
            int n = 0;

            foreach (string geotype in geoboxlist)
            {
                string filename = geonameclass.geonamesfolder + "geobox-" + geotype + "-" + makelang + ".txt";

                using (StreamReader sr = new StreamReader(filename))
                {
                    geoboxtemplates.Add(geotype, sr.ReadToEnd());
                    n++;
                }
            }

            Console.WriteLine("Read " + n.ToString() + " geoboxes.");

        }


        public static void read_categories()
        {
            int n = 0;

            string filename = geonameclass.geonamesfolder + "categories.txt";

            using (StreamReader sr = new StreamReader(filename))
            {
                String headline = "";
                headline = sr.ReadLine();

                int icol = 0;
                string[] langs = headline.Split('\t');
                for (icol = 0; icol < langs.Length; icol++)
                {
                    if (langs[icol] == makelang)
                    {
                        break;
                    }
                }

                //public static Dictionary<string, string> parentcategory = new Dictionary<string, string>(); //from category to parent category
                //public static Dictionary<string, string> categoryml = new Dictionary<string, string>(); //from category to category name in makelang


                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();

                    string[] words = line.Split('\t');

                    if (words.Length < icol + 1)
                        continue;

                    parentcategory.Add(words[0], words[1]);
                    categoryml.Add(words[0], words[icol]);

                    n++;
                    if ((n % 100) == 0)
                    {
                        Console.WriteLine("n (categories)   = " + n.ToString());
                        if (n >= 100000000)
                            break;
                    }

                }

                Console.WriteLine("n    (categories)= " + n.ToString());

            }

        }

        public static void read_catstat()
        {
            int n = 0;
            double nctot = 0;

            string filename = geonameclass.geonamesfolder + "catstat.txt";

            using (StreamReader sr = new StreamReader(filename))
            {

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();

                    string[] words = line.Split(':');

                    if (words.Length < 2)
                        continue;

                    int nc = util.tryconvert(words[1].Trim());
                    catstatdict.Add(words[0], nc);
                    nctot += nc;

                    n++;
                    if ((n % 100) == 0)
                    {
                        Console.WriteLine("n (categories)   = " + n.ToString());
                        if (n >= 100000000)
                            break;
                    }

                }

                Console.WriteLine("n    (categories)= " + n.ToString());

            }


            foreach (string s in catstatdict.Keys)
                catnormdict.Add(s, catstatdict[s] / nctot);


        }


        public static void read_featurecodes()
        {
            int n = 0;
            int nbad = 0;

            string filename = geonameclass.geonamesfolder + "featureCodes.txt";
            string lf = "";

            using (StreamReader sr = new StreamReader(filename))
            {
                String headline = "";
                headline = sr.ReadLine();

                int icol = 0;
                string[] langs = headline.Split('\t');
                for (icol = 0; icol < langs.Length; icol++)
                {
                    if (langs[icol] == makelang)
                    {
                        break;
                    }
                }

                string oldfc0 = "X";

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();

                    string[] words = line.Split('\t');

                    string[] fc = words[0].Split('.');

                    if (words[1] == "0")
                    {
                        nbad++;
                        continue;
                    }

                    //Console.WriteLine(fc[1]);
                    featuredict.Add(fc[1], words[icol]);

                    char fchar = fc[0].ToCharArray()[0];
                    featureclassdict.Add(fc[1], fchar);

                    string geotype = words[2];
                    if (String.IsNullOrEmpty(geotype))
                        geotype = "alla";
                    geoboxdict.Add(fc[1], geotype);
                    if (!geoboxlist.Contains(geotype))
                        geoboxlist.Add(geotype);

                    string catname = words[3];
                    if (String.IsNullOrEmpty(catname))
                        catname = "landforms";
                    categorydict.Add(fc[1], catname);

                    featurepointdict.Add(fc[1], ((words[4] != "0") && (words[4] != "3")));
                    minutesensitivedict.Add(fc[1], (words[4] == "2"));
                    if (words[4] == "3")
                        noclimatelist.Add(fc[1]);

                    if (savefeaturelink)
                    {
                        if ((!fc[1].Contains("ADM")) && (!fc[1].Contains("PPLA")))
                        {
                            if (fc[0] != oldfc0)
                            {
                                oldfc0 = fc[0];
                                lf += "\n\n== " + fc[0] + " ==\n\n";
                            }
                            lf += "* " + words[0] + ": " + linkfeature(fc[1], -1);
                            if (words.Length > 8)
                                lf += " (" + words[8] + ")";
                            lf += "\n";
                        }
                    }

                    n++;
                    if ((n % 100) == 0)
                    {
                        Console.WriteLine("n (featurecodes)   = " + n.ToString());
                        if (n >= 100000000)
                            break;
                    }

                }

                Console.WriteLine("n    (featurecodes)= " + n.ToString());
                Console.WriteLine("nbad (featurecodes)= " + nbad.ToString());

                if (savefeaturelink)
                {
                    Console.WriteLine(lf);

                    Page plf = new Page(makesite, util.mp(13) + botname + "/linkfeatures");
                    plf.text = lf;
                    util.trysave(plf, 1, "Bot saving linkfeatures");
                    Console.ReadLine();
                }
            }

            read_specialfeatures();
        }

        public static void read_specialfeatures()
        {
            int n = 0;

            string filename = geonameclass.geonamesfolder + "specialfeatures-" + makelang + ".txt";
            if (!File.Exists(filename))
                return;

            using (StreamReader sr = new StreamReader(filename))
            {

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();

                    string[] words = line.Split('\t');

                    if (words.Length < 2)
                        continue;

                    int gnid = util.tryconvert(words[0]);
                    if (gnid < 0)
                        continue;

                    if (!specialfeaturedict.ContainsKey(gnid))
                        specialfeaturedict.Add(gnid, words[1]);

                    if (words.Length >= 3)
                    {
                        if (!admclass.admtodet.ContainsKey(words[1]))
                            admclass.admtodet.Add(words[1], words[2]);
                    }

                    n++;
                    if ((n % 100) == 0)
                    {
                        Console.WriteLine("n (specialfeatures)   = " + n.ToString());
                        if (n >= 100000000)
                            break;
                    }

                }

                Console.WriteLine("n    (specialfeatures)= " + n.ToString());

            }

        }


        public static void make_altitude_files()
        {
            elevdiffhist.SetBins(-1000.0, 1000.0, 200);

            using (StreamWriter sw = new StreamWriter("altitude-" + makecountry + ".txt"))
            {

                int ngnid = Form1.gndict.Count;

                foreach (int gnid in Form1.gndict.Keys)
                {
                    Console.WriteLine("=====" + makecountry + "======== " + ngnid.ToString() + " remaining. ===========");
                    ngnid--;
                    if ((ngnid % 1000) == 0)
                    {
                        Console.WriteLine("Garbage collection:");
                        GC.Collect();
                    }

                    if ((resume_at > 0) && (resume_at != gnid))
                    {
                        stats.Addskip();
                        continue;
                    }
                    else
                        resume_at = -1;


                    int altitude = get_altitude(gnid);

                    Console.WriteLine(Form1.gndict[gnid].Name + ", " + Form1.gndict[gnid].featureclass + "." + Form1.gndict[gnid].featurecode + ": " + altitude.ToString());

                    if (Form1.gndict[gnid].elevation > 0)
                        elevdiffhist.Add(1.0 * (Form1.gndict[gnid].elevation - altitude));

                    if (altitude != 0)
                        sw.WriteLine(gnid.ToString() + tabstring + altitude.ToString());

                }
            }
            elevdiffhist.PrintDHist();
            //Console.ReadLine();

        }



        public static void read_chinese_pop()
        {
            Console.WriteLine("read_chinese_pop");
            string filepath = geonameclass.geonamesfolder + @"\China population\";
            string filekeyname = filepath + "filekey.txt";
            Dictionary<int, int> filekeys = new Dictionary<int, int>();
            int nkeys = 0;
            using (StreamReader sr = new StreamReader(filekeyname))
            {
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    int fn = util.tryconvert(words[0]);
                    if (fn > 0)
                    {
                        int gnid = util.tryconvert(words[2]);
                        if (gnid > 0)
                        {
                            nkeys++;
                            filekeys.Add(fn, gnid);
                        }
                    }
                }
            }
            Console.WriteLine("nkeys = " + nkeys);

            //public class chinese_pop_class
            //{
            //public int adm1 = -1;
            //public long pop = -1;
            //public long malepop = -1;
            //public long femalepop = -1;
            //public long households = -1;
            //public long pop014 = -1;
            //public long pop1564 = -1;
            //public long pop65 = -1;
            //}

            foreach (int fn in filekeys.Keys)
            {
                string filename = filepath + "China" + fn.ToString() + ".txt";
                Console.WriteLine(filename);
                if (!File.Exists(filename))
                    continue;

                int npop = 0;
                using (StreamReader sr = new StreamReader(filename))
                {
                    bool started = false;
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        string[] words = line.Split('\t');
                        if (!started) //skip preamble in file
                        {
                            if (words[0] == "start")
                                started = true;
                            continue;
                        }
                        chinese_pop_class cc = new chinese_pop_class();
                        cc.adm1 = filekeys[fn];
                        if (words.Length <= 1)
                            continue;
                        cc.pop = util.tryconvertlong(words[1]);
                        if (cc.pop < 0)
                            continue;
                        if (words.Length > 9)
                        {
                            cc.malepop = util.tryconvertlong(words[2]);
                            cc.femalepop = util.tryconvertlong(words[3]);
                            cc.households = util.tryconvertlong(words[4]);
                            cc.pop014 = util.tryconvertlong(words[7]);
                            cc.pop1564 = util.tryconvertlong(words[8]);
                            cc.pop65 = util.tryconvertlong(words[9]);
                        }
                        if (!chinese_pop_dict.ContainsKey(words[0]))
                        {
                            List<chinese_pop_class> cl = new List<chinese_pop_class>();
                            chinese_pop_dict.Add(words[0], cl);
                        }
                        chinese_pop_dict[words[0]].Add(cc);
                        npop++;
                    }
                }
                Console.WriteLine("npop = " + npop);
            }

        }

        public static void chinese_special()
        {
            read_chinese_pop();

            int nfdouble = 0;
            foreach (int gnid in Form1.gndict.Keys)
            {
                if ((Form1.gndict[gnid].featureclass == 'A') || (Form1.gndict[gnid].featureclass == 'P'))
                {
                    int nfcc = 0;
                    foreach (string an in Form1.gndict[gnid].altnames)
                    {
                        if (chinese_pop_dict.ContainsKey(an))
                        {
                            foreach (chinese_pop_class cc in chinese_pop_dict[an])
                            {
                                if (cc.adm1 == Form1.gndict[gnid].adm[1])
                                {
                                    nfcc++;
                                    if (nfcc == 1)
                                        chinese_pop_dict2.Add(gnid, cc);
                                    else if (nfcc == 2)
                                    {
                                        //Console.WriteLine("pop1 = " + cc.pop);
                                        //Console.WriteLine("pop2 = " + chinese_pop_dict2[gnid].pop);
                                        chinese_pop_dict2.Remove(gnid);
                                        nfdouble++;
                                    }
                                }
                            }
                            //if ( nfcc > 0 )
                            //    Console.WriteLine("nfcc = " + nfcc);
                        }
                    }

                }
            }

            Console.WriteLine("chinese pop found: " + chinese_pop_dict2.Count);
            Console.WriteLine("nfdouble = " + nfdouble);
            //Console.ReadLine();
        }

        public static void list_nameforks()
        {
            int nfork = 0;
            int nfork2 = 0;
            int nfork10 = 0;
            int nfork100 = 0;
            int nfork500 = 0;
            int nfork1000 = 0;
            int maxfork = 0;
            string maxforkname = "xxx";
            //string datestring = util.getdatestring();

            Console.WriteLine("list_nameforks");

            using (StreamWriter sw = new StreamWriter("namefork-" + util.getdatestring() + ".csv"))
            {

                foreach (string s in namefork.Keys)
                {
                    if (namefork[s].Count > 0)
                    {
                        nfork++;
                        if (namefork[s].Count > maxfork)
                        {
                            maxfork = namefork[s].Count;
                            maxforkname = s;
                        }
                        if (namefork[s].Count >= 2)
                            nfork2++;
                        if (namefork[s].Count >= 10)
                            nfork10++;
                        if (namefork[s].Count >= 100)
                            nfork100++;
                        if (namefork[s].Count >= 500)
                            nfork500++;
                        if (namefork[s].Count >= 1000)
                            nfork1000++;

                        sw.WriteLine(s);
                        foreach (int i in namefork[s])
                        {
                            if (!Form1.gndict.ContainsKey(i))
                            {
                                sw.WriteLine("Bad geonameid " + i.ToString());
                                continue;
                            }
                            sw.Write(i.ToString() + ";" + Form1.gndict[i].featurecode + ";");
                            if (countrydict.ContainsKey(Form1.gndict[i].adm[0]))
                            {
                                //Console.WriteLine(Form1.gndict[i].adm[0].ToString() + " " + countrydict[Form1.gndict[i].adm[0]].Name);
                                sw.Write(countrydict[Form1.gndict[i].adm[0]].Name);
                            }
                            else if (Form1.gndict.ContainsKey(Form1.gndict[i].adm[0]))
                                sw.Write(Form1.gndict[Form1.gndict[i].adm[0]].Name);
                            sw.Write(";");
                            if (Form1.gndict.ContainsKey(Form1.gndict[i].adm[1]))
                                sw.Write(Form1.gndict[Form1.gndict[i].adm[1]].Name_ml);
                            sw.Write(";");
                            if (Form1.gndict.ContainsKey(Form1.gndict[i].adm[2]))
                                sw.Write(Form1.gndict[Form1.gndict[i].adm[2]].Name_ml);
                            sw.Write(";");
                            sw.Write(Form1.gndict[i].latitude.ToString() + ";" + Form1.gndict[i].longitude.ToString() + ";");
                            if (Form1.gndict[i].Name_ml == s)
                                sw.Write("*");
                            else
                                sw.Write(Form1.gndict[i].Name_ml);
                            sw.Write(";" + Form1.gndict[i].wdid.ToString());
                            sw.WriteLine();
                        }
                        sw.WriteLine("#");
                        //sw.WriteLine();
                    }
                }

            }
            Console.WriteLine("nfork = " + nfork.ToString());
            Console.WriteLine("maxfork = " + maxfork.ToString());
            Console.WriteLine("maxforkname = |" + maxforkname + "|");
            Console.WriteLine("nfork2 = " + nfork2.ToString());
            Console.WriteLine("nfork10 = " + nfork10.ToString());
            Console.WriteLine("nfork100 = " + nfork100.ToString());
            Console.WriteLine("nfork500 = " + nfork100.ToString());
            Console.WriteLine("nfork1000 = " + nfork100.ToString());
        }

        public static PageList get_geotemplates()
        {
            PageList pl = new PageList(makesite);
            pl.FillAllFromCategoryTree(util.mp(74));
            //Skip all namespaces except templates (ns = 10):
            Console.WriteLine("pl.Count = " + pl.Count().ToString());
            //pl.RemoveNamespaces(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 12, 13, 14, 15, 100, 101 });
            Console.WriteLine("pl.Count = " + pl.Count().ToString());

            util.coordparams.Add("coord");
            util.coordparams.Add("Coord");
            util.coordparams.Add("lat_d");
            util.coordparams.Add("lat_g");
            util.coordparams.Add("latitude");
            util.coordparams.Add("latitud");

            return pl;
        }

        public static void fill_featurearticle()
        {
            if (featurearticle.ContainsKey("vik"))
                return;
            if (makelang == "sv")
            {
                featurearticle.Add("vik", "havsvik");
                featurearticle.Add("samhälle", "samhälle (geografi)");
                featurearticle.Add("udde", "halvö");
                featurearticle.Add("ö", "ö (landområde)");
                featurearticle.Add("kulle", "kulle (landform)");
                featurearticle.Add("del av lagun", "lagun");
                //featurearticle.Add("periodisk sjö", "sjö");
                //featurearticle.Add("periodisk saltsjö", "saltsjö");
                //featurearticle.Add("periodisk korvsjö", "korvsjö");
                featurearticle.Add("periodiska sjöar", "periodisk sjö");
                featurearticle.Add("periodiska saltsjöar", "periodisk saltsjö");
                featurearticle.Add("saltsjöar", "saltsjö");
                featurearticle.Add("del av en sjö", "sjö");
                featurearticle.Add("del av ett rev", "rev");
                featurearticle.Add("trångt sund", "sund");
                //featurearticle.Add("saltträsk", "våtmark");
                featurearticle.Add("fors", "fors (vattendrag)");
                featurearticle.Add("periodisk reservoar", "vattenmagasin");
                featurearticle.Add("sabkha", "saltöken");
                featurearticle.Add("grund", "grund (sjöfart)");
                featurearticle.Add("källa", "vattenkälla");
                featurearticle.Add("periodiskt vattendrag", "vattendrag");
                featurearticle.Add("wadier", "wadi");
                featurearticle.Add("periodisk våtmark", "våtmark");
                featurearticle.Add("reservat", "indianreservat");
                featurearticle.Add("övergiven gruva", "gruva");
                featurearticle.Add("kitteldalar", "kitteldal");
                featurearticle.Add("sänka", "bäcken (geografi)");
                featurearticle.Add("klippöken", "öken");
                featurearticle.Add("del av en ö", "ö (landområde)");
                featurearticle.Add("karstområde", "karst");
                featurearticle.Add("höjd", "kulle (landform)");
                featurearticle.Add("undervattenshöjd", "kulle (landform)");
                featurearticle.Add("morän", "morän (landform)");
                featurearticle.Add("nunataker", "nunatak");
                featurearticle.Add("sänkor", "bäcken (geografi)");
                featurearticle.Add("del av halvö", "halvö");
                featurearticle.Add("bergstoppar", "bergstopp");
                featurearticle.Add("klippa", "klippa (geologi)");
                featurearticle.Add("åmynning", "flodmynning");
                featurearticle.Add("militärt övningsområde", "övningsfält");
                featurearticle.Add("mangroveö", "mangrove");
                featurearticle.Add("del av en halvö", "halvö");
                featurearticle.Add("del av en platå", "platå");
                featurearticle.Add("del av en slätt", "slätt");
                featurearticle.Add("uddar", "halvö");
                featurearticle.Add("stenöken", "öken");
                featurearticle.Add("bankar", "sandbank");
                featurearticle.Add("bank", "sandbank");
                featurearticle.Add("sandbankar", "sandbank");
                featurearticle.Add("dalar", "dal");
                //featurearticle.Add("sadelpass", "bergspass");
                featurearticle.Add("del av en lagun", "lagun");
                featurearticle.Add("del av en ort", "stadsdel");
                featurearticle.Add("delta", "floddelta");
                featurearticle.Add("plattform", "massrörelse (geologi)");
                featurearticle.Add("veckterräng", "veckning");
                featurearticle.Add("bassäng", "bassäng (geologi)");
                featurearticle.Add("kanjoner", "kanjon");
                featurearticle.Add("fossil skog", "förstenat trä");
                featurearticle.Add("åsar", "ås");
                featurearticle.Add("undervattensåsar", "ås");
                featurearticle.Add("undervattensås", "ås");
                featurearticle.Add("orter", "ort");
                featurearticle.Add("hög udde", "halvö");
                featurearticle.Add("del av en dal", "dal");
                featurearticle.Add("liten undervattenskulle", "kulle (landform)");
                featurearticle.Add("små undervattenskullar", "kulle (landform)");
                featurearticle.Add("undervattenskulle", "kulle (landform)");
                featurearticle.Add("undervattenskullar", "kulle (landform)");
                featurearticle.Add("tröskel", "tröskelfjord");
                featurearticle.Add("kontinentalsluttning", "kontinentalbranten");
                featurearticle.Add("undervattensdal", "dal");
                featurearticle.Add("undervattensdalar", "dal");
                featurearticle.Add("utlöpare", "utlöpare (landform)");
                featurearticle.Add("guyoter", "guyot");
                featurearticle.Add("terass", "terass (landform)");
                featurearticle.Add("å", "å (vattendrag)");
                featurearticle.Add("klint", "klint (landform)");
                //featurearticle.Add("ekonomisk region", "ekonomisk region (Finland)");
                if (makecountry == "CN")
                {
                    featurearticle.Add("köping", "Kinas köpingar");
                    featurearticle.Add("socken", "Kinas socknar");
                }

                featurearticle.Add("stup", "stup (berg)");
                featurearticle.Add("", "");

            }
        }

        public static string linkfeature(string fcode, int gnid)
        {
            fill_featurearticle();
            string s = getfeaturelabelindet(makecountry, fcode, gnid);
            Console.WriteLine("linkfeature " + fcode + " " + s);

            //if (fcode.Contains("ADM"))
            //{
            //    //Console.WriteLine("linkfeature ADM");
            //    int admlevel = -1;
            //    if (fcode.Length >= 4)
            //        admlevel = util.tryconvert(fcode.Substring(3, 1));
            //    if (admlevel > 0)
            //        s = getadmindet(makecountry, admlevel,gnid);
            //}

            string rs = s;

            if (makelang == "sv")
            {
                if (s.IndexOf("en ") == 0)
                    rs = s.Insert(3, "[[");
                //rs = s.Replace("en ", "en [[");
                else if (s.IndexOf("ett ") == 0)
                    rs = s.Insert(4, "[[");
                //rs = s.Replace("ett ", "ett [[");
            }
            else if (makelang == "no")
            {
                if (s.IndexOf("en ") == 0)
                    rs = s.Insert(3, "[[");
                //rs = s.Replace("en ", "en [[");
                else if (s.IndexOf("et ") == 0)
                    rs = s.Insert(3, "[[");
                //rs = s.Replace("ett ", "ett [[");
            }
            else if (makelang == "ceb")
            {
                if (s.IndexOf("ang ") == 0)
                    rs = s.Insert(4, "[[");
                if (s.IndexOf("mga ") == 0)
                    rs = s.Insert(4, "[[");

            }

            if (!rs.Contains("[["))
                rs = "[[" + rs;

            rs = rs + "]]";

            Console.WriteLine("rs = " + rs);
            string gw = util.getwikilink(rs);
            Console.WriteLine("gw = " + gw);
            if (featurearticle.ContainsKey(gw))
                rs = rs.Replace(gw, featurearticle[gw] + "|" + gw);

            rs = util.comment(featureclassdict[fcode].ToString() + "." + fcode) + rs;

            if (rs.Contains("{{")) //Don't link if label contains template
            {
                rs = rs.Replace("[[", "").Replace("]]", "");
                if (rs.Contains(util.mp(174, null)))
                    hasnotes = true;
            }

            return rs;
        }

        public static string fix_artname(string artname)
        {
            string rs = artname;
            if (funny_quotes.Count == 0)
            {
                funny_quotes.Add("„", "“");//„...“ (makedonska m.m.)
                funny_quotes.Add("“", "”");//“…” (engelska m.m.) 
                funny_quotes.Add("«", "»");//«…» (franska m.m.)

            }

            foreach (string q1 in funny_quotes.Keys)
            {
                if (rs.Contains(q1) && rs.Contains(funny_quotes[q1]))
                    rs = rs.Replace(q1, "\"").Replace(funny_quotes[q1], "\"");
            }

            rs = rs.Replace("’", "'");
            rs = rs.Replace("[", "").Replace("]", "");
            rs = rs.Replace("{", "").Replace("}", "");

            bool hascomma = rs.Contains(",");
            rs = Regex.Replace(rs, @"[\u0000-\u001F]|[\u00AD]", string.Empty); //Ta bort nonprintable. \u00AD är mjukt bindestreck.
            bool stillhascomma = rs.Contains(",");
            if (hascomma != stillhascomma)
            {
                Console.WriteLine(rs);
                Console.WriteLine("Comma removed <cr>");
                Console.ReadLine();
            }
            return rs;
        }

        public static void fix_names()
        {
            foreach (int gnid in Form1.gndict.Keys)
            {
                Form1.gndict[gnid].Name = fix_artname(Form1.gndict[gnid].Name);
                Form1.gndict[gnid].Name_ml = fix_artname(Form1.gndict[gnid].Name_ml);
            }
        }

        public static void read_artname2_file(string filename)
        {
            int nartname = 0;
            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + filename))
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    string[] words = s.Split('\t');
                    if (words.Length < 2)
                        continue;
                    nartname++;
                    int gnid = util.tryconvert(words[0]);
                    string aname = fix_artname(words[1]);

                    if ((Form1.gndict.Count > 0) && !checkdoubles)
                    {
                        if (Form1.gndict.ContainsKey(gnid))
                        {
                            Form1.gndict[gnid].artname2 = aname.Replace("*", "");
                        }
                    }
                    if ((nartname % 1000000) == 0)
                        Console.WriteLine("nartname2 = " + nartname.ToString());
                }

            }
            Console.WriteLine("nartname2 = " + nartname.ToString());
        }

        public static void add_drainagecats()
        {
#if (DBGEOFLAG)

            //drainagedict[riverdict[gnid].drainage_name].main_river_artname + " " + util.mp(320)
            foreach (string dd in drainagedict.Keys)
            {
                string dcatname = util.mp(1) + drainagedict[dd].main_river_artname + " " + util.mp(320);
                Page dcat = new Page(makesite, dcatname);
                util.tryload(dcat, 2);
                if (!dcat.Exists())
                {
                    dcat.text = "";
                }
                dcat.AddToCategory("Mga tubig-saluran");
                util.trysave(dcat, 2);
            }
#endif
        }

        public static void read_drainagedict(string drainfilename)
        {
#if (DBGEOFLAG)

            Console.WriteLine("drainname: " + drainfilename);
            int ndrain = 0;
            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + drainfilename))
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    string[] words = s.Split('\t');
                    if (words.Length < 8)
                        continue;
                    drainageclass dc = new drainageclass();
                    dc.drainage_name = words[0];
                    dc.main_river_artname = words[4];
                    if (words.Length >= 9)
                    {
                        dc.main_river = util.tryconvert(words[8]);
                    }
                    ndrain++;
                    drainagedict.Add(words[0], dc);
                }
            }
            Console.WriteLine("ndrain = " + ndrain);
            //add_drainagecats();
#endif
        }

        public static void read_drainage_artname_file(string filename, string drainfilename)
        {
            //public class drainageclass //data for each drainage
            //{
            //    public double area = 0; //area of watershed
            //    public double kmew = 0;
            //    public double kmns = 0;
            //    public double length = 0;
            //    public List<int> inarea = new List<int>(); //list of GeoNames id of entities located in the catchment area of the river.
            //    public string drainage_name = ""; //name of drainage basin, index to drainageshapedict
            //    public int main_river; //gnid of main river; index to riverdict
            //    public string main_river_artname; //gnid of main river; index to riverdict
            //}

#if (DBGEOFLAG)

            int nartname = 0;
            int ndrain = 0;
            List<int> draingnid = new List<int>();
            Console.WriteLine("drainname: " + drainfilename);
            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + drainfilename))
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    string[] words = s.Split('\t');
                    if (words.Length < 8)
                        continue;
                    drainageclass dc = new drainageclass();
                    dc.drainage_name = words[0];
                    dc.main_river_artname = words[4];
                    if (words.Length >= 9)
                    {
                        dc.main_river = util.tryconvert(words[8]);
                        if (dc.main_river > 0)
                            draingnid.Add(dc.main_river);
                    }
                    ndrain++;
                    drainagedict.Add(words[0], dc);
                }
            }

            Console.WriteLine("ndrain = " + ndrain);

            Console.WriteLine("artname: " + filename);
            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + filename))
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    string[] words = s.Split('\t');
                    if (words.Length < 2)
                        continue;
                    nartname++;
                    int gnid = util.tryconvert(words[0]);
                    string aname = fix_artname(words[1]);

                    if ( draingnid.Contains(gnid))
                    {
                        string drain = "";
                        foreach (string dd in drainagedict.Keys)
                            if (drainagedict[dd].main_river == gnid)
                                drain = dd;
                        if (!String.IsNullOrEmpty(drain))
                            drainagedict[drain].main_river_artname = aname;
                    }
                    else if ( drainagedict.ContainsKey(aname))
                    {
                        drainagedict[aname].main_river = gnid;
                    }

                    if ((nartname % 1000000) == 0)
                        Console.WriteLine("ndrainartname = " + nartname.ToString());
                }

            }
            Console.WriteLine("nartname = " + nartname.ToString());
            using (StreamWriter sw = new StreamWriter(geonameclass.geonamesfolder + drainfilename + util.getdatestring()))
            {
                foreach (string dd in drainagedict.Keys)
                {
                    sw.WriteLine(drainagedict[dd].drainage_name + "\t" + drainagedict[dd].main_river.ToString() + "\t" + drainagedict[dd].main_river_artname);
                }
            }
            Console.WriteLine("Done");
            Console.ReadLine();
#endif
        }

        public static void read_oldartname_file(string filename)
        {
            int nartname = 0;
            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + filename))
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    string[] words = s.Split('\t');
                    if (words.Length < 2)
                        continue;
                    nartname++;
                    int gnid = util.tryconvert(words[0]);
                    string aname = fix_artname(words[1]);

                    if ((Form1.gndict.Count > 0) && !checkdoubles)
                    {
                        if (Form1.gndict.ContainsKey(gnid))
                        {
                            if (Form1.gndict[gnid].articlename != aname)
                                Form1.gndict[gnid].oldarticlename = aname;
                        }
                    }
                    if (checkdoubles)
                    {
                        if (!oldartnamedict.ContainsKey(gnid))
                            oldartnamedict.Add(gnid, aname);
                    }

                    if ((nartname % 1000000) == 0)
                        Console.WriteLine("noldartname = " + nartname.ToString());
                }

            }
            Console.WriteLine("noldartname = " + nartname.ToString());
        }


        public static void read_artname_file(string filename)
        {
            int nartname = 0;
            int nparish = 0;
            Console.WriteLine("artname: " + filename);
            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + filename))
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    string[] words = s.Split('\t');
                    if (words.Length < 2)
                        continue;
                    nartname++;
                    int gnid = util.tryconvert(words[0]);
                    string aname = fix_artname(words[1]);

                    //if ((makecountry == "AZ") && (filename.Contains("missing")) && (makelang == "sv")) //kludge to get around weird Azerbadjan bug
                    //{
                    //    Page pz = new Page(makesite, aname.Replace("*", ""));
                    //    util.tryload(pz, 1);
                    //    if (is_fork(pz) && pz.text.Contains("Azerbajdzjan"))
                    //    {
                    //        aname += " (distrikt i Azerbajdzjan)";
                    //        Console.WriteLine(aname);
                    //    }
                    //}


                    if ((Form1.gndict.Count > 0) && !checkdoubles)
                    {
                        if (Form1.gndict.ContainsKey(gnid))
                        {
                            if (makecountry == "BE")
                            {
                                if (aname.Contains("(församling"))
                                {
                                    aname = aname.Replace("(församling)", "(kommun i Belgien)");
                                    aname = aname.Replace("(församling i", "(kommun i");
                                    aname = aname.Replace("(församling,", "(kommun i Belgien,");
                                    aname = aname.Replace("(församlingshuvudort", "(kommunhuvudort");
                                    if (aname.Contains("(församling"))
                                    {
                                        nparish++;
                                        Console.WriteLine(aname);
                                    }
                                }
                            }

                            if (makecountry == "CW")
                            {
                                if (aname.Contains("Curacao"))
                                {
                                    aname = aname.Replace("Curacao", "Curaçao");
                                }
                            }

                            if (makecountry == "FI")
                            {
                                if (aname.Contains("Åboland-Turunmaa"))
                                    aname = aname.Replace("Åboland-Turunmaa", "Åboland");
                            }

                            if (makecountry == "DE")
                            {
                                if (aname.Contains("Upper Palatinate"))
                                    aname = aname.Replace("Upper Palatinate", "Oberpfalz");
                                if (aname.Contains("Upper Bavaria"))
                                    aname = aname.Replace("Upper Bavaria", "Oberbayern");
                                if (aname.Contains("Upper Franconia"))
                                    aname = aname.Replace("Upper Franconia", "Oberfranken");
                                if (aname.Contains(" Swabia"))
                                    aname = aname.Replace(" Swabia", " Schwaben");
                            }


                            if ((!Form1.gndict[gnid].articlename.Contains("*")) || (aname.Contains("*")) || filename.Contains("missing"))
                            {
                                Form1.gndict[gnid].articlename = aname;
                                if (words[1] != aname)
                                    Form1.gndict[gnid].unfixedarticlename = words[1];
                            }
                            if ((aname.Contains("*")) || (Form1.gndict[gnid].Name == Form1.gndict[gnid].Name_ml) || filename.Contains("missing"))
                            {
                                Form1.gndict[gnid].Name_ml = util.remove_disambig(aname.Replace("*", ""));
                            }
                            if (aname.Contains("Östprovinsen"))
                                Console.WriteLine(gnid.ToString() + ": " + aname);
                        }
                    }
                    //else 
                    {
                        if (!artnamedict.ContainsKey(gnid))
                            artnamedict.Add(gnid, aname);
                        else if (!artnamedict[gnid].Contains("*"))
                            artnamedict[gnid] = aname;

                    }
                    if ((nartname % 1000000) == 0)
                        Console.WriteLine("nartname = " + nartname.ToString());
                }

            }
            Console.WriteLine("nartname = " + nartname.ToString());
            Console.WriteLine("nparish = " + nparish.ToString());
        }

        public static void read_artname()
        {
            Console.WriteLine("read_artname");
            string filename = "artname-" + makelang + ".txt"; //use current file
            if (checkdoubles)
                filename = "artname-" + makelang + "-checkwiki.txt"; //use the file that really was checked against wiki
            read_artname_file(filename);

            //fil med diverse handfixade namn
            read_artname_file("missing-adm1-toartname-" + makelang + ".txt");

            //Console.ReadLine();

            if (makelang == "ceb")
                filename = "artname-sv.txt";
            else
                filename = "artname-ceb.txt";

            //Console.WriteLine("Skip artname2");
            read_artname2_file(filename);

            //filename = "artname-"+makelang+"-old.txt";
            //read_oldartname_file(filename);
        }


        public static void fill_donecountries()
        {
            if (makelang == "sv")
            {
                donecountries.Add("BT");
                donecountries.Add("AG");
                donecountries.Add("BH");
                donecountries.Add("MK");
                donecountries.Add("MT");
                donecountries.Add("SS");
                donecountries.Add("NI");
                donecountries.Add("LU");
            }
        }

        public static void check_doubles()
        {
            int ndouble = 0;
            int ndouble_ow = 0;
            int ndouble_cw = 0;
            int ndouble_coord = 0;
            int ndouble_iw = 0;
            int nfuzzy = 0;
            int nadm1 = 0;
            int nalldisambig = 0;
            List<string> fuzzylist = new List<string>();
            bool checkwiki = false;

            hbookclass scripthist = new hbookclass("scripthist");


            Console.WriteLine("Check doubles");
            //PageList geolist = get_geotemplates();
            //foreach (Page p in geolist)
            //    Console.WriteLine("Template " + p.title);

            Dictionary<string, int> geotempdict = new Dictionary<string, int>();

            int np = 0;

            using (StreamWriter swfuzz = new StreamWriter("manualmatch-ADM1-" + makelang + "-" + util.getdatestring() + ".txt"))
            using (StreamWriter sw = new StreamWriter("artname-" + makelang + "-" + util.getdatestring() + ".txt"))
            {
                using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "namefork-" + makelang + ".csv"))
                {
                    while (!sr.EndOfStream)
                    {
                        string s = sr.ReadLine();
                        s = s.Trim(';');

                        //scripthist.Add(util.get_alphabet(s));
                        if (util.get_alphabet(s) == "none")
                        {
                            Console.WriteLine("none:" + s + "|");
                            //Console.ReadLine();
                        }
                        List<forkclass> fl = new List<forkclass>();

                        string countryname = "";
                        string adm1name = "";
                        string adm2name = "";
                        string fcode = "";


                        int nrealnames = 0;
                        int nnames = 0;
                        int imakelang = langtoint[makelang];

                        while (true)
                        {
                            string line = sr.ReadLine();
                            if (line[0] == '#')
                                break;
                            string[] words = line.Split(';');

                            //public class forkclass
                            //{
                            //    public int geonameid = 0;
                            //    public string featurecode = "";
                            //    public string[] admname = new string[3];
                            //    public double latitude = 0.0;
                            //    public double longitude = 0.0;

                            //}

                            forkclass fc = new forkclass();
                            fc.geonameid = util.tryconvert(words[0]);
                            fc.featurecode = words[1];
                            fc.admname[0] = words[2];
                            if (countryml.ContainsKey(words[2]))
                                fc.admname[0] = countryml[words[2]];
                            if (countryiso.ContainsKey(words[2]))
                                fc.iso = countryiso[words[2]];
                            fc.admname[1] = words[3];
                            fc.admname[2] = words[4];
                            fc.latitude = util.tryconvertdouble(words[5]);
                            fc.longitude = util.tryconvertdouble(words[6]);
                            fc.realname = words[7];
                            fc.wdid = util.tryconvert(words[8]);
                            fc.featurename = getfeaturelabel(fc.iso, fc.featurecode, fc.geonameid);
                            if (altdict.ContainsKey(fc.geonameid))
                            {
                                foreach (altnameclass ac in altdict[fc.geonameid])
                                {
                                    if (ac.ilang == imakelang)
                                    {
                                        if (ac.altname == s)
                                            fc.realname = "*";
                                        else
                                            fc.realname = ac.altname;
                                    }
                                }
                            }
                            nnames++;
                            if (fc.realname == "*")
                            {
                                nrealnames++;
                            }
                            fl.Add(fc);
                            countryname = words[2];
                            adm1name = words[3];
                            adm2name = words[4];
                            fcode = words[1];
                        }

                        bool allsamecountry = true;
                        //bool allsamefcode = true;
                        bool allsameadm1 = true;
                        bool allsameadm2 = true;
                        bool somesamecountry = false;
                        bool somesamefcode = false;
                        bool somesameadm1 = false;
                        bool somesameadm2 = false;


                        foreach (forkclass ff in fl)
                        {
                            if (ff.realname == "*")
                            {
                                if (ff.admname[0] != countryname)
                                    allsamecountry = false;
                                if (ff.admname[1] != adm1name)
                                    allsameadm1 = false;
                                if (ff.admname[2] != adm2name)
                                    allsameadm2 = false;
                                //if (ff.featurecode != fcode)
                                //    allsamefcode = false;

                                if (String.IsNullOrEmpty(ff.admname[1]))
                                    somesameadm1 = true;
                                if (String.IsNullOrEmpty(ff.admname[2]))
                                    somesameadm2 = true;

                                foreach (forkclass ff2 in fl)
                                {
                                    if ((ff2.realname == "*") && (ff.geonameid != ff2.geonameid))
                                    {
                                        if (ff.admname[0] == ff2.admname[0])
                                            somesamecountry = true;
                                        if (ff.admname[1] == ff2.admname[1])
                                            somesameadm1 = true;
                                        if (ff.admname[2] == ff2.admname[2])
                                            somesameadm2 = true;
                                        if (ff.featurecode == ff2.featurecode)
                                            somesamefcode = true;
                                        if (ff.featurename == ff2.featurename)
                                            somesamefcode = true;
                                    }
                                }
                            }
                        }

                        if (nrealnames == 0)
                            continue;

                        //bool geotemplatefound = false;
                        bool coordfound = false;
                        bool pagefound = false;
                        string namefound = "";
                        double lat = 999.0;
                        double lon = 999.0;

                        //foreach (forkclass fc in fl)
                        //    if (fc.realname == "*")
                        //    {
                        //        List<int> enb = getexistingneighbors(fc.latitude, fc.longitude, 10.0);
                        //        foreach (int nb in enb)
                        //        {
                        //            if (existingdict.ContainsKey(nb))
                        //            {
                        //                if ( util.remove_disambig(existingdict[nb].articlename) == s)
                        //                {
                        //                    pagefound = true;
                        //                    coordfound = true;
                        //                    namefound = existingdict[nb].articlename;
                        //                    lat = fc.latitude;
                        //                    lon = fc.longitude;
                        //                    break;
                        //                }
                        //            }
                        //        }
                        //    }


                        if (checkwiki)
                        {
                            Page oldpage = new Page(makesite, s);
                            //Page forkpage = new Page(makesite, testprefix + s + " (" + util.mp(67) + ")");
                            if (util.tryload(oldpage, 1))
                            {
                                if (oldpage.Exists())
                                {
                                    pagefound = true;
                                    if (oldpage.IsRedirect())
                                    {
                                        oldpage.title = oldpage.RedirectsTo();
                                        util.tryload(oldpage, 1);
                                        pagefound = oldpage.Exists();
                                    }

                                    if (util.is_fork(oldpage))
                                        pagefound = false;

                                    if (pagefound)
                                    {
                                        //geotemplatefound = false;
                                        coordfound = false;
                                        double[] latlong = util.get_article_coord(oldpage);
                                        if (latlong[0] + latlong[1] < 720.0)
                                        {
                                            coordfound = true;
                                            Console.WriteLine(latlong[0].ToString() + "|" + latlong[1].ToString());
                                            lat = latlong[0];
                                            lon = latlong[1];
                                            namefound = oldpage.title;
                                            ndouble_cw++;
                                        }
                                    }
                                }
                            }
                        }

                        else //check against old artname-file
                        {
                            bool alldisambig = true;
                            foreach (forkclass fc in fl)
                                if (fc.realname == "*")
                                {
                                    if (artnamedict.ContainsKey(fc.geonameid))
                                    {
                                        if (artnamedict[fc.geonameid].Contains("*"))
                                        {
                                            pagefound = true;
                                            coordfound = true;
                                            namefound = artnamedict[fc.geonameid].Replace("*", "");
                                            lat = fc.latitude;
                                            lon = fc.longitude;
                                            alldisambig = false;
                                            ndouble_ow++;
                                            break;
                                        }
                                        else if (!artnamedict[fc.geonameid].Contains("("))
                                            alldisambig = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("gnid missing in artnamedict! " + s);
                                        //Console.ReadLine();
                                    }
                                }
                            if ((alldisambig) && (nrealnames == 1)) //page with that name exists but doesn't match any place
                            {
                                pagefound = true;
                                coordfound = false;
                                nalldisambig++;
                            }
                        }




                        Dictionary<int, Disambigclass> dadict = new Dictionary<int, Disambigclass>();
                        //public class disambigclass
                        //{
                        //    bool existsalready = false;
                        //    bool country = false;
                        //    bool adm1 = false;
                        //    bool adm2 = false;
                        //    bool latlong = false;
                        //    bool fcode = false;
                        //}

                        //Now we know if page exists:

                        if (pagefound)
                        {
                            //ndouble++;
                            if (nrealnames == 1)
                            {
                                foreach (forkclass fc in fl)
                                    if (fc.realname == "*")
                                    {
                                        Disambigclass da = new Disambigclass();
                                        da.fork = fc;

                                        if (coordfound)
                                        {
                                            double dist = util.get_distance_latlong(lat, lon, fc.latitude, fc.longitude);
                                            if (dist < 5.0) //Probably same object
                                            {
                                                da.existsalready = true;
                                                //sw.WriteLine(fc.geonameid + tabstring + "*"+s);
                                            }
                                            else
                                            {
                                                da.fcode = true;
                                                da.country = true;
                                                //sw.WriteLine(fc.geonameid + tabstring + s + " (" + util.removearticle(featuredict[fc.featurecode]) + " " + util.mp(75) + " " + fc.admname[0] + ")");
                                            }
                                        }
                                        else //no coordinates
                                        {
                                            da.fcode = true;
                                            da.country = true;
                                            //sw.WriteLine(fc.geonameid + tabstring + s + " (" + util.removearticle(featuredict[fc.featurecode]) + " " + util.mp(75) + " " + fc.admname[0] + ")");
                                        }
                                        dadict.Add(fc.geonameid, da);

                                    }
                            }
                            else //several realnames, pagefound
                            {
                                if (coordfound)
                                {
                                    foreach (forkclass fc in fl)
                                        if (fc.realname == "*")
                                        {
                                            Disambigclass da = new Disambigclass();
                                            da.fork = fc;

                                            double dist = util.get_distance_latlong(lat, lon, fc.latitude, fc.longitude);
                                            if (dist < 5.0) //Probably same object
                                            {
                                                da.existsalready = true;
                                                //sw.WriteLine(fc.geonameid + tabstring + "X");
                                            }
                                            else
                                            {
                                                //sw.Write(fc.geonameid + tabstring + s + " (");
                                                //sw.Write(util.removearticle(featuredict[fc.featurecode]));
                                                da.fcode = true;
                                                if (somesamefcode)
                                                {
                                                    //sw.Write(" " + util.mp(75) + " " + fc.admname[0]);
                                                    da.country = !allsamecountry && !String.IsNullOrEmpty(da.fork.admname[0]);
                                                    if (somesamecountry)
                                                    {

                                                        //sw.Write(", ");
                                                        //if (!allsameadm1)
                                                        da.adm1 = !allsameadm1 && !String.IsNullOrEmpty(da.fork.admname[1]); // sw.Write(fc.admname[1]);
                                                        if (somesameadm1)
                                                        {
                                                            //if (!allsameadm1 && !String.IsNullOrEmpty(fc.admname[1]))
                                                            //    sw.Write(", ");
                                                            //if (!allsameadm2)
                                                            //    sw.Write(fc.admname[2]);
                                                            da.adm2 = !allsameadm2 && !String.IsNullOrEmpty(da.fork.admname[2]);
                                                            if (somesameadm2)
                                                            {
                                                                da.latlong = true;
                                                                //if (!allsameadm2 && !String.IsNullOrEmpty(fc.admname[2]))
                                                                //    sw.Write(", ");
                                                                //sw.Write("lat " + fc.latitude.ToString("F2") + ", long " + fc.longitude.ToString("F2"));
                                                            }
                                                        }

                                                    }
                                                }
                                                //sw.WriteLine(")");
                                            }
                                            dadict.Add(fc.geonameid, da);

                                        }
                                }
                                else //no coordinates, several realnames, page found
                                {
                                    foreach (forkclass fc in fl)
                                        if (fc.realname == "*")
                                        {
                                            Disambigclass da = new Disambigclass();
                                            da.fork = fc;

                                            //sw.Write(fc.geonameid + tabstring + s + " (");
                                            //sw.Write(util.removearticle(featuredict[fc.featurecode]));
                                            da.fcode = true;

                                            if (somesamefcode)
                                            {
                                                //sw.Write(" " + util.mp(75) + " " + fc.admname[0]);
                                                da.country = !allsamecountry && !String.IsNullOrEmpty(da.fork.admname[0]);
                                                if (somesamecountry)
                                                {

                                                    //sw.Write(", ");
                                                    //if (!allsameadm1)
                                                    da.adm1 = !allsameadm1 && !String.IsNullOrEmpty(da.fork.admname[1]); // sw.Write(fc.admname[1]);
                                                    if (somesameadm1)
                                                    {
                                                        //if (!allsameadm1 && !String.IsNullOrEmpty(fc.admname[1]))
                                                        //    sw.Write(", ");
                                                        //if (!allsameadm2)
                                                        //    sw.Write(fc.admname[2]);
                                                        da.adm2 = !allsameadm2 && !String.IsNullOrEmpty(da.fork.admname[2]);
                                                        if (somesameadm2)
                                                        {
                                                            da.latlong = true;
                                                            //if (!allsameadm2 && !String.IsNullOrEmpty(fc.admname[2]))
                                                            //    sw.Write(", ");
                                                            //sw.Write("lat " + fc.latitude.ToString("F2") + ", long " + fc.longitude.ToString("F2"));
                                                        }
                                                    }

                                                }

                                            }
                                            //sw.WriteLine(")");
                                            dadict.Add(fc.geonameid, da);
                                        }


                                }
                            }
                        }
                        else //page not found
                        {
                            if (nrealnames == 1)
                            {
                                foreach (forkclass fc in fl)
                                {
                                    if (fc.realname == "*")
                                    {
                                        //sw.WriteLine(fc.geonameid + tabstring + s);
                                        Disambigclass da = new Disambigclass();
                                        da.fork = fc;
                                        if (nnames > 1)
                                        {
                                            da.fcode = true;
                                            da.country = true;
                                        }
                                        dadict.Add(fc.geonameid, da);
                                    }
                                }
                            }
                            else //several realnames, page not found
                            {
                                foreach (forkclass fc in fl)
                                    if (fc.realname == "*")
                                    {
                                        Disambigclass da = new Disambigclass();
                                        da.fork = fc;

                                        //sw.Write(fc.geonameid + tabstring + s + " (");
                                        //sw.Write(util.removearticle(featuredict[fc.featurecode]));
                                        da.fcode = true;

                                        if (somesamefcode)
                                        {
                                            //sw.Write(" " + util.mp(75) + " " + fc.admname[0]);
                                            da.country = !allsamecountry && !String.IsNullOrEmpty(da.fork.admname[0]);
                                            if (somesamecountry)
                                            {

                                                //sw.Write(", ");
                                                //if (!allsameadm1)
                                                da.adm1 = !allsameadm1 && !String.IsNullOrEmpty(da.fork.admname[1]); // sw.Write(fc.admname[1]);
                                                if (somesameadm1)
                                                {
                                                    //if (!allsameadm1 && !String.IsNullOrEmpty(fc.admname[1]))
                                                    //    sw.Write(", ");
                                                    //if (!allsameadm2)
                                                    //    sw.Write(fc.admname[2]);
                                                    da.adm2 = !allsameadm2 && !String.IsNullOrEmpty(da.fork.admname[2]);
                                                    if (somesameadm2)
                                                    {
                                                        da.latlong = true;
                                                        //if (!allsameadm2 && !String.IsNullOrEmpty(fc.admname[2]))
                                                        //    sw.Write(", ");
                                                        //sw.Write("lat " + fc.latitude.ToString("F2") + ", long " + fc.longitude.ToString("F2"));
                                                    }
                                                }

                                            }

                                        }
                                        //sw.WriteLine(")");
                                        dadict.Add(fc.geonameid, da);

                                    }
                            }
                        }

                        foreach (int gnid in dadict.Keys)
                        {
                            if (nrealnames > 1)
                            {
                                bool uniquecountry = !String.IsNullOrEmpty(dadict[gnid].fork.admname[0]);
                                bool uniqueadm1 = !String.IsNullOrEmpty(dadict[gnid].fork.admname[1]);
                                bool uniqueadm2 = !String.IsNullOrEmpty(dadict[gnid].fork.admname[2]);
                                bool uniquefcode = true;

                                foreach (forkclass ff2 in fl)
                                {
                                    if ((ff2.realname == "*") && (ff2.geonameid != gnid))
                                    {
                                        if (dadict[gnid].fork.admname[0] == ff2.admname[0])
                                            uniquecountry = false;
                                        if (dadict[gnid].fork.admname[1] == ff2.admname[1])
                                            uniqueadm1 = false;
                                        if (dadict[gnid].fork.admname[2] == ff2.admname[2])
                                            uniqueadm2 = false;
                                        if (dadict[gnid].fork.featurecode == ff2.featurecode)
                                            uniquefcode = false;
                                    }

                                }

                                if (dadict[gnid].fcode && uniquefcode)
                                {
                                    dadict[gnid].country = false;
                                    dadict[gnid].adm1 = false;
                                    dadict[gnid].adm2 = false;
                                    dadict[gnid].latlong = false;
                                }
                                else if (dadict[gnid].country && uniquecountry)
                                {
                                    dadict[gnid].adm1 = false;
                                    dadict[gnid].adm2 = false;
                                    dadict[gnid].latlong = false;
                                }
                                else if (dadict[gnid].adm1 && uniqueadm1)
                                {
                                    dadict[gnid].adm2 = false;
                                    dadict[gnid].latlong = false;
                                }
                                else if (dadict[gnid].adm2 && uniqueadm2)
                                {
                                    dadict[gnid].latlong = false;
                                }
                            }

                            //if (!Form1.gndict.ContainsKey(gnid))
                            //    continue;
                            string artname = "";

                            if (countrydict.ContainsKey(gnid))
                            {
                                artname = "*" + countrydict[gnid].Name_ml;
                            }

                            if (checkwiki && String.IsNullOrEmpty(artname)) //Look for interwiki matches
                            {
                                if (dadict[gnid].fork.wdid > 0)
                                {
                                    XmlDocument cx = wdtreeclass.get_wd_xml(wdid);
                                    if (cx != null)
                                    {
                                        Dictionary<string, string> rd = wdtreeclass.get_wd_sitelinks(cx);
                                        foreach (string wiki in rd.Keys)
                                        {
                                            string ssw = wiki.Replace("wiki", "");
                                            if (ssw == makelang)
                                            {
                                                artname = "*" + rd[wiki];
                                                ndouble_iw++;
                                                break;
                                            }
                                        }
                                    }

                                }
                            }

                            if ((String.IsNullOrEmpty(artname)) && (dadict[gnid].fork.featurecode == "ADM1")) //Look for ADM1 in category:
                            {
                                Console.WriteLine("Checking for ADM1 match " + s + ", " + dadict[gnid].fork.admname[0]);
                                if (admclass.existing_adm1.ContainsKey(dadict[gnid].fork.admname[0]))
                                {
                                    Console.WriteLine("Country found; count = " + admclass.existing_adm1[dadict[gnid].fork.admname[0]].Count.ToString());
                                    if (admclass.existing_adm1[dadict[gnid].fork.admname[0]].Contains(s))
                                        artname = "*" + s;
                                    else
                                    {
                                        foreach (string es in admclass.existing_adm1[dadict[gnid].fork.admname[0]])
                                            if (util.remove_disambig(es) == "s")
                                            {
                                                artname = "*" + es;
                                                nadm1++;
                                                break;
                                            }

                                        if (String.IsNullOrEmpty(artname)) //Look for fuzzy matches:
                                        {
                                            int mindist = 999;
                                            if (s.Length < 4)
                                                mindist = 1;
                                            else if (s.Length < 7)
                                                mindist = 3;
                                            else if (s.Length < 20)
                                                mindist = 4;
                                            else
                                                mindist = 5;
                                            int distmax = mindist;
                                            mindist = 999;
                                            string mindistname = "";

                                            foreach (string es in admclass.existing_adm1[dadict[gnid].fork.admname[0]])
                                            {
                                                string tit = util.remove_disambig(es);
                                                int dist = util.LevenshteinDistance(s, tit);
                                                //Console.WriteLine(s+" | "+tit + ": "+dist.ToString());
                                                if (dist < mindist)
                                                {
                                                    mindist = dist;
                                                    mindistname = es;
                                                }

                                            }
                                            if (mindist < distmax)
                                            {
                                                Console.WriteLine("Fuzzy match: " + s + " | " + mindistname + ": " + mindist.ToString());
                                                //Console.ReadLine();
                                                nadm1++;
                                                fuzzylist.Add(gnid.ToString() + ": " + s + " | " + mindistname + ": " + mindist.ToString());
                                                artname = "*" + mindistname;
                                            }
                                            else if (manualcheck && (!String.IsNullOrEmpty(mindistname)))
                                            {
                                                Console.WriteLine("Fuzzy match: " + s + " | " + mindistname + ": " + mindist.ToString());
                                                Console.Write("OK? (y/n)");
                                                char yn = Console.ReadKey().KeyChar;
                                                if (yn == 'y')
                                                {
                                                    nadm1++;
                                                    fuzzylist.Add(gnid.ToString() + ": " + s + " | " + mindistname + ": " + mindist.ToString());
                                                    artname = "*" + mindistname;
                                                    Console.WriteLine("Saving " + artname);
                                                    swfuzz.WriteLine(gnid.ToString() + tabstring + artname);

                                                }
                                                else
                                                    Console.WriteLine(yn.ToString());
                                            }

                                        }
                                    }
                                }
                                //Console.ReadLine();
                            }

                            if (String.IsNullOrEmpty(artname)) //Look for fuzzy matches:
                            {
                                int mindist = 999;
                                if (s.Length < 4)
                                    mindist = 0;
                                else if (s.Length < 7)
                                    mindist = 2;
                                else if (s.Length < 20)
                                    mindist = 3;
                                else
                                    mindist = 4;
                                int distmax = mindist;
                                string mindistname = "";
                                List<int> enb = getexisting(dadict[gnid].fork.latitude, dadict[gnid].fork.longitude, 10.0);
                                foreach (int nb in enb)
                                {
                                    if (existingdict.ContainsKey(nb))
                                    {
                                        string cleanart = util.remove_disambig(existingdict[nb].articlename);
                                        if (cleanart == s)
                                        {
                                            artname = "*" + existingdict[nb].articlename;
                                            ndouble_coord++;
                                            break;
                                        }
                                        else
                                        {
                                            int dist = util.LevenshteinDistance(s, cleanart);
                                            //Console.WriteLine(s+" | "+cleanart + ": "+dist.ToString());
                                            if (dist < mindist)
                                            {
                                                mindist = dist;
                                                mindistname = existingdict[nb].articlename;
                                            }
                                        }

                                    }
                                }
                                if (String.IsNullOrEmpty(artname))
                                {
                                    if (mindist < distmax)
                                    {
                                        Console.WriteLine("Fuzzy match: " + s + " | " + mindistname + ": " + mindist.ToString());
                                        //Console.ReadLine();
                                        nfuzzy++;
                                        fuzzylist.Add(gnid.ToString() + ": " + s + " | " + mindistname + ": " + mindist.ToString());
                                        artname = "*" + mindistname;
                                    }
                                }
                            }

                            if (String.IsNullOrEmpty(artname))
                            {

                                if (dadict[gnid].existsalready)
                                    artname = "*" + s;
                                else
                                {
                                    bool daneeded = dadict[gnid].fcode || dadict[gnid].country || dadict[gnid].adm1 || dadict[gnid].adm2 || dadict[gnid].latlong;
                                    if (!daneeded)
                                        artname = s;
                                    else
                                    {
                                        artname = s + " " + make_disambig(dadict[gnid], gnid);
                                    }

                                }
                            }

                            if (donecountries.Contains(dadict[gnid].fork.iso))
                            {

                                if (oldartnamedict.ContainsKey(gnid))
                                {
                                    string fname = util.removearticle(getfeaturelabel(dadict[gnid].fork.iso, dadict[gnid].fork.featurecode, gnid));
                                    if (!oldartnamedict[gnid].Contains("(") || oldartnamedict[gnid].Contains(fname))
                                        artname = oldartnamedict[gnid];
                                }
                            }

                            sw.WriteLine(gnid.ToString() + tabstring + artname);
                            np++;
                            if (artname.Contains("*"))
                                ndouble++;
                            if ((np % 1000) == 0)
                            {
                                Console.WriteLine("np  = " + np.ToString() + ", " + countryname);
                            }
                        }

                        if ((ndouble % 100) == 0)
                        {
                            Console.WriteLine("n (doubles)   = " + ndouble.ToString());
                        }


                        //while (s[0] != '#')
                        //    s = sr.ReadLine();
                        //continue;
                    }

                    foreach (string ss in fuzzylist)
                        Console.WriteLine(ss);
                    Console.WriteLine("n    (doubles)     = " + ndouble.ToString());
                    Console.WriteLine("n    (checkwiki)   = " + ndouble_cw.ToString());
                    Console.WriteLine("n    (oldwiki)     = " + ndouble_ow.ToString());
                    Console.WriteLine("n    (coord)       = " + ndouble_coord.ToString());
                    Console.WriteLine("n    (wikidata)    = " + ndouble_iw.ToString());
                    Console.WriteLine("n    (fuzzy match) = " + nfuzzy.ToString());
                    Console.WriteLine("n    (ADM1-match)  = " + nadm1.ToString());
                    Console.WriteLine("n    (alldisambig) = " + nalldisambig.ToString());

                    //scripthist.PrintSHist();
                    //foreach (string gt in geotempdict.Keys)
                    //    Console.WriteLine(gt + ":" + geotempdict[gt].ToString());

                }
            }
        }

        public static string getfeaturelabel(string countrycode, string fcode, int gnid)
        {
            return util.removearticle(getfeaturelabelindet(countrycode, fcode, gnid));
        }

        public static string getfeaturelabelindet(string countrycode, string fcode, int gnid)
        {
            string rs = "";

            if (!featuredict.ContainsKey(fcode))
                return "unknown feature";

            if (specialfeaturedict.ContainsKey(gnid))
                return specialfeaturedict[gnid];

            if (fcode.Contains("PPLA"))
            {
                int level = 1;
                if (fcode != "PPLA")
                    level = util.tryconvert(fcode.Replace("PPLA", ""));
                if ((level > 0) && (level <= 5))
                {
                    string admlabel = getadmlabel(countrycode, level, gnid);
                    if (admclass.admcap.ContainsKey(admlabel))
                    {
                        if (makelang == "sv")
                            rs = "en " + admclass.admcap[admlabel];
                        else if (makelang == "no")
                            rs = "et " + admclass.admcap[admlabel];
                        else
                            rs = admclass.admcap[admlabel];
                    }
                    else
                        rs = featuredict[fcode];
                }
                else
                    rs = featuredict[fcode];
            }
            else if (fcode.Contains("ADM"))
            {
                int level = util.tryconvert(fcode.Replace("ADM", ""));
                if ((level > 0) && (level <= 5))
                {
                    rs = getadmindet(countrycode, level, gnid);
                }
            }

            if (String.IsNullOrEmpty(rs))
                rs = featuredict[fcode];

            Console.WriteLine("getfeaturelabelindet = " + rs);
            return rs;

        }

        public static bool is_zhen(int gnid)
        {
            bool zhenfound = false;
            if (Form1.gndict[gnid].Name.ToLower().Contains(" zhen"))
                zhenfound = true;
            else
            {
                if (altdict.ContainsKey(gnid))
                {
                    foreach (altnameclass ac in altdict[gnid])
                    {
                        if (ac.altname.ToLower().Contains(" zhen"))
                        {
                            zhenfound = true;
                            break;
                        }
                    }
                }
            }
            Console.WriteLine("is_zhen = " + zhenfound.ToString());
            return zhenfound;
        }

        public static string getadmlabel(string countrycode, int level, int gnid)
        {
            string rs = "";
            if (specialfeaturedict.ContainsKey(gnid))
                rs = specialfeaturedict[gnid];
            else if (admclass.admdict.ContainsKey(countrycode))
            {
                if (level <= admclass.admdict[countrycode].maxadm)
                    rs = admclass.admdict[countrycode].label[level - 1];
            }
            else
            {
                switch (countrycode)
                {
                    case "MY":
                        if ((Form1.gndict.ContainsKey(gnid)) && (Form1.gndict[gnid].longitude > 106.0))
                            rs = getadmlabel("MY2", level, gnid);
                        else
                            rs = getadmlabel("MY1", level, gnid);
                        break;
                    case "GB": //different for different kingdoms in United Kingdom
                        int kingdom = 6269131;
                        if (Form1.gndict.ContainsKey(gnid))
                            kingdom = Form1.gndict[gnid].adm[1];
                        switch (kingdom)
                        {
                            case 6269131: //England
                                rs = getadmlabel("GB1", level, gnid);
                                break;
                            case 2641364: //Northern Ireland
                                rs = getadmlabel("GB2", level, gnid);
                                break;
                            case 2638360: //Scotland
                                rs = getadmlabel("GB3", level, gnid);
                                break;
                            case 2634895: //Wales
                                rs = getadmlabel("GB4", level, gnid);
                                break;
                            default:
                                rs = getadmlabel("GB1", level, gnid);
                                break;

                        }
                        break;
                    case "RU":
                        if (level == 1)
                        {
                            if (Form1.gndict.ContainsKey(gnid))
                            {
                                if (Form1.gndict[gnid].Name.Contains(" Oblast"))
                                    rs = "oblast";
                                else if ((gndict[gnid].Name.Contains(" Krai")) || (gndict[gnid].Name.Contains(" Kray")))
                                    rs = "kraj";
                                else if (gndict[gnid].Name.Contains(" Okrug"))
                                    rs = "autonomt distrikt";
                                else
                                    rs = "delrepublik";
                            }
                            else
                            {
                                rs = "oblast";
                            }
                        }
                        else
                        {
                            rs = (admclass.admdict["RU1"].label[level - 1]);
                        }
                        break;
                    case "CN":
                        if (level == 4)
                        {
                            if (gndict.ContainsKey(gnid))
                            {
                                bool zhenfound = is_zhen(gnid);
                                if (zhenfound)
                                {
                                    rs = util.mp(297);
                                }
                                else
                                    rs = (admclass.admdict["CN1"].label[level - 1]);
                            }
                            else
                            {
                                rs = (admclass.admdict["CN1"].label[level - 1]);
                            }
                        }
                        else
                        {
                            rs = (admclass.admdict["CN1"].label[level - 1]);
                        }
                        break;

                    default:
                        rs = (admclass.admdict["default"].label[level - 1]);
                        break;
                }
            }

            Console.WriteLine("getadmlabel = " + rs);
            return rs;
        }

        public static string getadmindet(string countrycode, int level, int gnid)
        {
            string rs = getadmlabel(countrycode, level, gnid);

            if (makelang == "sv")
            {
                if (getadmdet(countrycode, level, gnid).EndsWith("t"))
                    rs = "ett " + rs;
                else
                    rs = "en " + rs;
            }
            else if (makelang == "no")
            {
                if (getadmdet(countrycode, level, gnid).EndsWith("t"))
                    rs = "et " + rs;
                else
                    rs = "en " + rs;
            }

            return rs;
        }

        public static string getadmdet(string countrycode, int level, int gnid)
        {
            string rs = getadmlabel(countrycode, level, gnid);
            if (admclass.admtodet.ContainsKey(rs))
                rs = admclass.admtodet[rs];

            if (makelang == "ceb")
                rs += " sa";

            return rs;

        }

        public static double get_distance(int gnidfrom, int gnidto)
        {
            double gnidlat = gndict[gnidto].latitude;
            double gnidlong = gndict[gnidto].longitude;
            double countrylat = gndict[gnidfrom].latitude;
            double countrylong = gndict[gnidfrom].longitude;

            return util.get_distance_latlong(countrylat, countrylong, gnidlat, gnidlong);

        }

        public static int get_direction(int gnidfrom, int gnidto)
        {
            double tolat = gndict[gnidto].latitude;
            double tolong = gndict[gnidto].longitude;
            double fromlat = gndict[gnidfrom].latitude;
            double fromlong = gndict[gnidfrom].longitude;
            return util.get_direction_latlong(fromlat, fromlong, tolat, tolong);

        }

        public static void fill_kids_features()
        {
            foreach (int gnid in gndict.Keys)
            {
                int parent = 0;
                for (int i = 0; i < 5; i++)
                    if ((gndict[gnid].adm[i] > 0) && (gndict[gnid].adm[i] != gnid))
                        parent = gndict[gnid].adm[i];
                if ((gndict[gnid].featureclass == 'A') && (gndict[gnid].featurecode.Contains("ADM")) && (!gndict[gnid].featurecode.Contains("ADMD")) && (!gndict[gnid].featurecode.Contains("H")))
                {
                    if (gndict.ContainsKey(parent))
                        gndict[parent].children.Add(gnid);
                }
                else
                {
                    if (gndict.ContainsKey(parent))
                        gndict[parent].features.Add(gnid);
                }

            }
        }

        public static void read_languageiso()
        {
            //public class langclass
            //{
            //    public string iso3 = "";
            //    public string iso2 = "";
            //    public Dictionary<string,string> name = new Dictionary<string,string>(); //name of language in different language. Iso -> name.
            //}

            //public static Dictionary<int,langclass> langdict = new Dictionary<int,langclass>(); //main language table
            //public static Dictionary<string, int> langtoint = new Dictionary<string, int>(); //from iso to integer code. Both iso2 and iso3 used as keys to the same int
            int n = 0;


            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "language-iso.txt"))
            {
                //int makelangcol = -1;

                String headline = sr.ReadLine();
                string[] heads = headline.Split('\t');
                Dictionary<int, string> ld = new Dictionary<int, string>();
                for (int i = 0; i < heads.Length; i++)
                {
                    if ((heads[i].Length == 2) || (heads[i].Length == 3))
                        ld.Add(i, heads[i]);

                }

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();

                    if (line[0] == '#')
                        continue;


                    //if (n > 250)
                    //Console.WriteLine(line);

                    string[] words = line.Split('\t');

                    if (words.Length < 3)
                        continue;

                    n++;

                    //foreach (string s in words)
                    //    Console.WriteLine(s);

                    //Console.WriteLine(words[0] + "|" + words[1]);

                    langclass lc = new langclass();
                    lc.iso3 = words[0].Trim();
                    lc.iso2 = words[1].Trim();

                    for (int i = 2; i < words.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(words[i].Trim()))
                        {
                            if (ld.ContainsKey(i))
                            {
                                if (!lc.name.ContainsKey(ld[i]))
                                    lc.name.Add(ld[i], words[i].Trim());

                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(lc.iso3))
                        if (!langtoint.ContainsKey(lc.iso3))
                            langtoint.Add(lc.iso3, n);
                    if (!String.IsNullOrEmpty(lc.iso2))
                        if (!langtoint.ContainsKey(lc.iso2))
                            langtoint.Add(lc.iso2, n);
                    langdict.Add(n, lc);

                    if ((n % 100) == 0)
                    {
                        Console.WriteLine("n (language-iso)   = " + n.ToString());
                    }

                }

                Console.WriteLine("n    (language-iso)= " + n.ToString());




            }

            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "langnames-" + makelang + ".txt"))
            {
                n = 0;
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();

                    string[] words = line.Split('\t');

                    if (words.Length < 2)
                        continue;

                    string iso = words[0];
                    string langname = words[1];
                    if (makelang == "sv")
                        langname = langname.ToLower();
                    if (langtoint.ContainsKey(iso))
                    {
                        int langcode = langtoint[iso];
                        if ((langdict.ContainsKey(langcode)) && (!langdict[langcode].name.ContainsKey(makelang)))
                            langdict[langcode].name.Add(makelang, langname);
                    }

                    n++;
                    if ((n % 100) == 0)
                    {
                        Console.WriteLine("n (langname-makelang)   = " + n.ToString());

                    }


                }
                Console.WriteLine("n    (langname-makelang)= " + n.ToString());
            }

            if (savewikilinks)
            {
                Page pt = new Page(makesite, util.mp(13) + botname + "/languagelinks");
                pt.text = "Language links used by Lsjbot\n\n";
                foreach (int ilang in langdict.Keys)
                    if (langdict[ilang].name.ContainsKey(makelang))
                        pt.text += "* " + langdict[ilang].iso3 + " [[" + langdict[ilang].name[makelang] + "]]\n";
                    else
                        pt.text += "* " + langdict[ilang].iso3 + "\n";
                util.trysave(pt, 1, "Bot saving language links");
            }


        }


        public static string addref(string rn, string rstring)
        {
            if (String.IsNullOrEmpty(rn) || String.IsNullOrEmpty(rstring))
                return "";

            string refname = "\"" + rn + "\"";
            if (!refnamelist.Contains(refname))
            {
                refnamelist.Add(refname);

                string refref = "<ref name = " + refname + ">" + rstring + "</ref>";
                reflist += "\n" + refref;
            }
            string shortref = "<ref name = " + refname + "/>";
            return shortref;

        }

        public static string addnote(string notestring)
        {
            //if (makelang != "sv")
            //    return "";
            hasnotes = true;
            //Console.WriteLine("addnote:" + notestring);
            return util.mp(174) + notestring + "}}";

        }


        public static string fill_geobox(int gnid)
        {
            string fc = gndict[gnid].featurecode;
            int icountry = gndict[gnid].adm[0];
            List<string> allnames = new List<string>();

            string boxtype = "alla";
            if (geoboxdict.ContainsKey(fc))
                boxtype = geoboxdict[fc];
            if (!geoboxtemplates.ContainsKey(boxtype))
            {
                Console.WriteLine("XXXXXXXXX Bad box type: " + boxtype);
                return "";
            }

            //creates dummy page, in order to use DNWB tools for template handling
            Page dummy = new Page(makesite, "dummy");
            dummy.text = geoboxtemplates[boxtype];

            //Console.WriteLine("Före:"+dummy.text.Substring(0,30));

            dummy.SetTemplateParameter("geobox", "name", gndict[gnid].Name_ml, true);
            //Console.WriteLine("1:" + dummy.text.Substring(0, 30));
            allnames.Add(gndict[gnid].Name_ml);

            int othernames = 0;
            if (gndict[gnid].Name != gndict[gnid].Name_ml)
            {
                dummy.SetTemplateParameter("geobox", "other_name", gndict[gnid].Name, true);
                allnames.Add(gndict[gnid].Name);
                othernames++;
            }

            if (altdict.ContainsKey(gnid))
            {
                int nativenames = 0;
                foreach (altnameclass ac in altdict[gnid])
                {
                    if (!allnames.Contains(ac.altname))
                    {
                        if (countrydict[icountry].languages.Contains(ac.ilang))
                        {
                            nativenames++;
                            if (ac.official)
                            {
                                dummy.SetTemplateParameter("geobox", "official_name", ac.altname, true);
                                allnames.Add(ac.altname);
                            }
                        }
                        if (ac.colloquial)
                        {
                            dummy.SetTemplateParameter("geobox", "nickname", ac.altname, true);
                            allnames.Add(ac.altname);
                        }
                    }
                }

                if (nativenames > 0)
                {
                    //bool nativeset = false;
                    foreach (altnameclass ac in altdict[gnid])
                    {
                        if (!allnames.Contains(ac.altname))
                        {

                            if (countrydict[icountry].languages.Contains(ac.ilang))
                            {
                                dummy.SetTemplateParameter("geobox", "native_name", ac.altname, true);
                                allnames.Add(ac.altname);
                                break; //set only once
                            }
                        }
                    }
                    foreach (altnameclass ac in altdict[gnid])
                    {

                        if (!allnames.Contains(ac.altname))

                        //if ( !countrydict[icountry].languages.Contains(ac.ilang))
                        {
                            string order = "";
                            if (othernames > 0)
                                order = othernames.ToString();
                            if ((!String.IsNullOrEmpty(ac.altname)) && (!allnames.Contains(ac.altname)))
                            {
                                dummy.SetTemplateParameter("geobox", "other_name" + order, ac.altname, true);
                                allnames.Add(ac.altname);
                                othernames++;
                            }
                        }
                    }
                }
                else
                {
                    foreach (altnameclass ac in altdict[gnid])
                    {
                        if (!allnames.Contains(ac.altname))
                        {

                            string order = "";
                            if (othernames > 0)
                                order = othernames.ToString();
                            if (!allnames.Contains(ac.altname))
                            {
                                dummy.SetTemplateParameter("geobox", "other_name" + order, ac.altname, true);
                                allnames.Add(ac.altname);
                                othernames++;
                            }
                        }
                    }

                }


            }


            //Console.WriteLine("2:" + dummy.text.Substring(0, 30));

            string latstring = gndict[gnid].latitude.ToString(culture_en);
            if (!latstring.Contains("."))
                latstring += ".0";
            string lonstring = gndict[gnid].longitude.ToString(culture_en);
            if (!lonstring.Contains("."))
                lonstring += ".0";
            dummy.SetTemplateParameter("geobox", "lat_d", latstring, true);
            dummy.SetTemplateParameter("geobox", "long_d", lonstring, true);

            string cat = util.initialcap(getfeaturelabel(countrydict[icountry].iso, gndict[gnid].featurecode, gnid));
            dummy.SetTemplateParameter("geobox", "category", cat, true);

            string countrynameml = countrydict[icountry].Name;
            if (countryml.ContainsKey(countrynameml))
                countrynameml = countryml[countrynameml];

            if (makelang == "sv")
            {

                dummy.SetTemplateParameter("geobox", "country", countrynameml, true);
                dummy.SetTemplateParameter("geobox", "country_flag", "true", true);
            }
            else
            {
                dummy.SetTemplateParameter("geobox", "country", "{{flag|" + countrynameml + "}}", true);
            }

            int nc = 0;

            int mamagnid = -1;
            if (motherdict.ContainsKey(makecountry))
            {
                string mama = motherdict[makecountry];
                mamagnid = countryid[mama];
                nc++;
                string acml = countrydict[mamagnid].Name;
                if (countryml.ContainsKey(acml))
                    acml = countryml[acml];
                if (makelang == "sv")
                {
                    dummy.SetTemplateParameter("geobox", "country" + nc.ToString(), acml, true);
                    dummy.SetTemplateParameter("geobox", "country_flag" + nc.ToString(), "true", true);

                }
                else
                {
                    dummy.SetTemplateParameter("geobox", "country" + nc.ToString(), "{{flag|" + acml + "}}", true);
                }
                dummy.SetTemplateParameter("geobox", "country_type", util.mp(294), true);
            }

            foreach (int ic in gndict[gnid].altcountry)
            {
                nc++;
                if (ic == mamagnid)
                    continue;
                string acml = countrydict[ic].Name;
                if (countryml.ContainsKey(acml))
                    acml = countryml[acml];
                if (makelang == "sv")
                {
                    dummy.SetTemplateParameter("geobox", "country" + nc.ToString(), acml, true);
                    dummy.SetTemplateParameter("geobox", "country_flag" + nc.ToString(), "true", true);
                }
                else
                {
                    dummy.SetTemplateParameter("geobox", "country" + nc.ToString(), "{{flag|" + acml + "}}", true);
                }

            }

            if (!string.IsNullOrEmpty(getgnidname(gndict[gnid].adm[1])))
            {
                dummy.SetTemplateParameter("geobox", "state", makegnidlink(gndict[gnid].adm[1]), true);
                dummy.SetTemplateParameter("geobox", "state_type", util.initialcap(util.removearticle(getadmlabel(makecountry, 1, gndict[gnid].adm[1]))), true);
            }
            if (!string.IsNullOrEmpty(getgnidname(gndict[gnid].adm[2])))
            {
                dummy.SetTemplateParameter("geobox", "region", makegnidlink(gndict[gnid].adm[2]), true);
                dummy.SetTemplateParameter("geobox", "region_type", util.initialcap(getadmlabel(makecountry, 2, gndict[gnid].adm[2])), true);
            }
            if (!string.IsNullOrEmpty(getgnidname(gndict[gnid].adm[3])))
            {
                dummy.SetTemplateParameter("geobox", "district", makegnidlink(gndict[gnid].adm[3]), true);
                dummy.SetTemplateParameter("geobox", "district_type", util.initialcap(getadmlabel(makecountry, 3, gndict[gnid].adm[3])), true);
            }
            if (!string.IsNullOrEmpty(getgnidname(gndict[gnid].adm[4])))
            {
                dummy.SetTemplateParameter("geobox", "municipality", makegnidlink(gndict[gnid].adm[4]), true);
                dummy.SetTemplateParameter("geobox", "municipality_type", util.initialcap(getadmlabel(makecountry, 4, gndict[gnid].adm[4])), true);
            }

            int elev = gndict[gnid].elevation;
            if (elev < 0)
                elev = gndict[gnid].dem;
            string category = "";
            if (categorydict.ContainsKey(fc))
                category = categorydict[fc];
            if ((category == "oceans") || (category == "seabed") || (category == "reefs") || (category == "bays") || (category == "navigation"))
                elev = -9999;


            if (elev > 0)
            {
                dummy.SetTemplateParameter("geobox", "elevation", elev.ToString("N0", nfi_en), true);
                if (is_height(gndict[gnid].featurecode))
                {
                    double width = 0;
                    int prom = get_prominence(gnid, out width);

                    if (prom <= 0)
                    {
                        //int nearhigh = -1;
                        int altitude = -1;
                        double nbradius = 3.0;
                        List<int> farlist = getneighbors(gnid, nbradius);
                        bool otherpeak = false;
                        Console.WriteLine("farlist = " + farlist.Count.ToString());
                        foreach (int nbgnid in farlist)
                            if (nbgnid != gnid)
                                if (is_height(gndict[nbgnid].featurecode) && (gndict[nbgnid].elevation > gndict[gnid].elevation))
                                {
                                    otherpeak = true;
                                    Console.WriteLine(gndict[nbgnid].Name);
                                }

                        if (!otherpeak)
                        {
                            Console.WriteLine("No other peak");
                            nbradius = 2.0;
                            double slat = 9999.9;
                            double slon = 9999.9;
                            altitude = get_summit(gnid, out slat, out slon);

                            Console.WriteLine("get summit " + slat.ToString() + " " + slon.ToString() + ": " + altitude.ToString());

                            double nhdist = util.get_distance_latlong(gndict[gnid].latitude, gndict[gnid].longitude, slat, slon);
                            Console.WriteLine("nhdist = " + nhdist.ToString());
                            if ((nhdist < nbradius) && (nhdist > 0.1) && (altitude > elev))
                            {
                                gndict[gnid].latitude = slat;
                                gndict[gnid].longitude = slon;
                                gndict[gnid].elevation = altitude;
                                gndict[gnid].elevation_vp = altitude;
                                elev = altitude;

                                latstring = gndict[gnid].latitude.ToString("F4", culture_en);
                                if (!latstring.Contains("."))
                                    latstring += ".0";
                                lonstring = gndict[gnid].longitude.ToString("F4", culture_en);
                                if (!lonstring.Contains("."))
                                    lonstring += ".0";
                                dummy.SetTemplateParameter("geobox", "elevation", elev.ToString("N0", nfi_en), true);
                                dummy.SetTemplateParameter("geobox", "lat_d", latstring, true);
                                dummy.SetTemplateParameter("geobox", "long_d", lonstring, true);
                                dummy.SetTemplateParameter("geobox", "coordinates_note", addnote(util.mp(213) + addref("vp", viewfinder_ref()) + " " + util.mp(200)), true);

                                prom = get_prominence(gnid, out width);
                            }
                        }

                    }

                    if (prom > minimum_prominence)
                    {
                        dummy.SetTemplateParameter("geobox", "height", prom.ToString("N0", nfi_en), true);
                        dummy.SetTemplateParameter("geobox", "width", util.fnum(width), true);
                        dummy.SetTemplateParameter("geobox", "width_unit", "km", true);
                        dummy.SetTemplateParameter("geobox", "highest_elevation", elev.ToString("N0", nfi_en), true);
                        dummy.SetTemplateParameter("geobox", "elevation", (elev - prom).ToString("N0", nfi_en), true);
                        gndict[gnid].prominence = prom;
                        gndict[gnid].width = width;
                        if (gndict.ContainsKey(gndict[gnid].inrange))
                            dummy.SetTemplateParameter("geobox", "range", makegnidlink(gndict[gnid].inrange), true);
                    }
                }
            }

            bool haspop = false;

            if (prefergeonamespop)
            {
                if ((makecountry == "CN") && (chinese_pop_dict2.ContainsKey(gnid)))
                {
                    dummy.SetTemplateParameter("geobox", "population", chinese_pop_dict2[gnid].pop.ToString(), true);
                    haspop = true;
                    dummy.SetTemplateParameter("geobox", "population_note", chinapopref(), true);
                    dummy.SetTemplateParameter("geobox", "population_date", "2010", true);

                }
                else if (gndict[gnid].population > minimum_population)
                {
                    dummy.SetTemplateParameter("geobox", "population", gndict[gnid].population.ToString(), true);
                    haspop = true;
                    if (gndict[gnid].population == gndict[gnid].population_wd)
                        dummy.SetTemplateParameter("geobox", "population_note", "<sup>" + util.mp(131) + " " + gndict[gnid].population_wd_iw + "wiki</sup>", true);
                    else
                    {
                        dummy.SetTemplateParameter("geobox", "population_note", geonameref(gnid), true);
                        dummy.SetTemplateParameter("geobox", "population_date", gndict[gnid].moddate, true);
                    }

                }
                else if ((wdid > 0) && (!String.IsNullOrEmpty(wdtreeclass.get_wd_prop(wdtreeclass.propdict["population"], currentxml))))
                {
                    dummy.SetTemplateParameter("geobox", "population", wdtreeclass.wdlink("population"), true);
                    haspop = true;
                    dummy.SetTemplateParameter("geobox", "population_note", "<sup>" + util.mp(131) + " Wikidata</sup>", true);
                    long wdpop = util.tryconvert(wdtreeclass.get_wd_prop(wdtreeclass.propdict["population"], currentxml));
                    if (wdpop > 0)
                    {
                        gndict[gnid].population_wd = wdpop;
                        if ((gndict[gnid].population < minimum_population) || (!prefergeonamespop))
                            gndict[gnid].population = wdpop;
                        gndict[gnid].population_wd_iw = "Wikidata";
                    }
                }
                else if (gndict[gnid].population_wd > minimum_population)
                {
                    dummy.SetTemplateParameter("geobox", "population", gndict[gnid].population_wd.ToString(), true);
                    haspop = true;
                    //dummy.SetTemplateParameter("geobox", "population_date", gndict[gnid].moddate, true);
                    dummy.SetTemplateParameter("geobox", "population_note", "<sup>" + util.mp(131) + " " + gndict[gnid].population_wd_iw + "wiki</sup>", true);
                }
            }
            else
            {
                if ((wdid > 0) && (!String.IsNullOrEmpty(wdtreeclass.get_wd_prop(wdtreeclass.propdict["population"], currentxml))))
                {
                    dummy.SetTemplateParameter("geobox", "population", wdtreeclass.wdlink("population"), true);
                    haspop = true;
                    dummy.SetTemplateParameter("geobox", "population_note", "<sup>" + util.mp(131) + " Wikidata</sup>", true);
                    long wdpop = util.tryconvert(wdtreeclass.get_wd_prop(wdtreeclass.propdict["population"], currentxml));
                    if (wdpop > 0)
                    {
                        gndict[gnid].population_wd = wdpop;
                        if ((gndict[gnid].population < minimum_population) || (!prefergeonamespop))
                            gndict[gnid].population = wdpop;
                        gndict[gnid].population_wd_iw = "Wikidata";
                    }
                }
                else if (gndict[gnid].population_wd > minimum_population)
                {
                    dummy.SetTemplateParameter("geobox", "population", gndict[gnid].population_wd.ToString(), true);
                    haspop = true;
                    //dummy.SetTemplateParameter("geobox", "population_date", gndict[gnid].moddate, true);
                    dummy.SetTemplateParameter("geobox", "population_note", "<sup>" + util.mp(131) + " " + gndict[gnid].population_wd_iw + "wiki</sup>", true);
                }
                else if (gndict[gnid].population > minimum_population)
                {
                    dummy.SetTemplateParameter("geobox", "population", gndict[gnid].population.ToString(), true);
                    haspop = true;
                    dummy.SetTemplateParameter("geobox", "population_date", gndict[gnid].moddate, true);
                    dummy.SetTemplateParameter("geobox", "population_note", geonameref(gnid), true);
                }
            }

            if (gndict[gnid].area > minimum_area)
            {
                dummy.SetTemplateParameter("geobox", "area", gndict[gnid].area.ToString("N2", nfi_en), true);
                if (haspop)
                    dummy.SetTemplateParameter("geobox", "population_density", "auto", true);
            }

            if (tzclass.tzdict.ContainsKey(gndict[gnid].tz))
            {
                dummy.SetTemplateParameter("geobox", "timezone_label", gndict[gnid].tz, true);
                dummy.SetTemplateParameter("geobox", "utc_offset", tzclass.tzdict[gndict[gnid].tz].offset, true);
                if (tzclass.tzdict[gndict[gnid].tz].summeroffset != tzclass.tzdict[gndict[gnid].tz].offset)
                    dummy.SetTemplateParameter("geobox", "utc_offset_DST", tzclass.tzdict[gndict[gnid].tz].summeroffset, true);
                if (!String.IsNullOrEmpty(tzclass.tzdict[gndict[gnid].tz].tzname))
                {
                    dummy.SetTemplateParameter("geobox", "timezone", "[[" + tzclass.tzdict[gndict[gnid].tz].tzfull + "|" + tzclass.tzdict[gndict[gnid].tz].tzname + "]]", true);
                    if (tzclass.tzdict[gndict[gnid].tz].summeroffset != tzclass.tzdict[gndict[gnid].tz].offset)
                        dummy.SetTemplateParameter("geobox", "timezone_DST", "[[" + tzclass.tzdict[gndict[gnid].tz].tzfullsummer + "|" + tzclass.tzdict[gndict[gnid].tz].tzsummer + "]]", true);
                }
            }

            if (wdid > 0) //get various stuff from Wikidata:
            {
                Console.WriteLine("Filling geobox from wikidata");
                if (!String.IsNullOrEmpty(wdtreeclass.get_wd_prop(wdtreeclass.propdict["coat of arms"], currentxml)))
                {
                    string imagename = wdtreeclass.get_wd_prop(wdtreeclass.propdict["coat of arms"], currentxml);
                    if (exists_at_commons(imagename))
                        dummy.SetTemplateParameter("geobox", "symbol", imagename, true);
                }
                if (!String.IsNullOrEmpty(wdtreeclass.get_wd_prop(wdtreeclass.propdict["flag"], currentxml)))
                    dummy.SetTemplateParameter("geobox", "flag", wdtreeclass.get_wd_prop(wdtreeclass.propdict["flag"], currentxml), true);
                //if (!String.IsNullOrEmpty(wdtreeclass.get_wd_prop(wdtreeclass.propdict("capital", currentxml))))
                //    dummy.SetTemplateParameter("geobox", "capital", wdtreeclass.wdlink("capital"));
                if (!String.IsNullOrEmpty(wdtreeclass.get_wd_prop(wdtreeclass.propdict["locatormap"], currentxml)))
                    dummy.SetTemplateParameter("geobox", "map2", wdtreeclass.get_wd_prop(wdtreeclass.propdict["locatormap"], currentxml), true);
                if (!String.IsNullOrEmpty(wdtreeclass.get_wd_prop(wdtreeclass.propdict["iso"], currentxml)))
                    dummy.SetTemplateParameter("geobox", "iso_code", wdtreeclass.wdlink("iso"), true);
                //if (!String.IsNullOrEmpty(wdtreeclass.get_wd_prop(wdtreeclass.propdict("head of government", currentxml))))
                //    dummy.SetTemplateParameter("geobox", "leader", wdtreeclass.wdlink("head of government"));
                if (!String.IsNullOrEmpty(wdtreeclass.get_wd_prop(wdtreeclass.propdict["postal code"], currentxml)))
                    dummy.SetTemplateParameter("geobox", "postal_code", wdtreeclass.wdlink("postal code"), true);
                if (!String.IsNullOrEmpty(wdtreeclass.get_wd_prop(wdtreeclass.propdict["image"], currentxml)))
                {
                    string imagename = wdtreeclass.get_wd_prop(wdtreeclass.propdict["image"], currentxml);
                    Console.WriteLine("Imagename = " + imagename);
                    if (exists_at_commons(imagename))
                        dummy.SetTemplateParameter("geobox", "image", imagename, true);
                }
                else if (!String.IsNullOrEmpty(wdtreeclass.get_wd_prop(wdtreeclass.propdict["banner"], currentxml)))
                {
                    string imagename = wdtreeclass.get_wd_prop(wdtreeclass.propdict["banner"], currentxml);
                    if (exists_at_commons(imagename))
                        dummy.SetTemplateParameter("geobox", "image", imagename, true);
                }
                foreach (int ic in wdtreeclass.get_wd_prop_idlist(wdtreeclass.propdict["capital"], currentxml))
                    dummy.SetTemplateParameter("geobox", "capital", wdtreeclass.get_wd_name(ic), true);
                foreach (int ic in wdtreeclass.get_wd_prop_idlist(wdtreeclass.propdict["head of government"], currentxml))
                    dummy.SetTemplateParameter("geobox", "leader", wdtreeclass.get_wd_name(ic), true);

                if (String.IsNullOrEmpty(wdtreeclass.get_wd_prop(wdtreeclass.propdict["gnid"], currentxml))) //if gnid NOT in wikidata, set it manually
                    dummy.SetTemplateParameter("geobox", "geonames", gnid.ToString(), true);

            }
            else //wdid missing, set geonames ID in template
            {
                dummy.SetTemplateParameter("geobox", "geonames", gnid.ToString(), true);
            }

            if (gndict[gnid].inrange > 0)
            {
                dummy.SetTemplateParameter("geobox", "range", makegnidlink(gndict[gnid].inrange), true);
            }
            else if (rangedict.ContainsKey(gnid))
            {
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

                dummy.SetTemplateParameter("geobox", "length", rangedict[gnid].length.ToString("N0", nfi_en), true);
                dummy.SetTemplateParameter("geobox", "length_orientation", get_nsew(rangedict[gnid].angle), true);
                int hmax = rangedict[gnid].maxheight;
                int hgnid = -1;
                if (hmax < 0)
                    if (gndict.ContainsKey(-hmax))
                    {
                        hgnid = -hmax;
                        hmax = gndict[hgnid].elevation;
                    }
                if (hmax > 0)
                {
                    string hlatstring = rangedict[gnid].hlat.ToString(culture_en);
                    if (!hlatstring.Contains("."))
                        hlatstring += ".0";
                    string hlonstring = rangedict[gnid].hlon.ToString(culture_en);
                    if (!hlonstring.Contains("."))
                        hlonstring += ".0";
                    if (hgnid > 0)
                        dummy.SetTemplateParameter("geobox", "highest_location", makegnidlink(hgnid), true);
                    dummy.SetTemplateParameter("geobox", "highest_elevation", hmax.ToString("N0", nfi_en), true);
                    dummy.SetTemplateParameter("geobox", "highest_lat_d", hlatstring, true);
                    dummy.SetTemplateParameter("geobox", "highest_long_d", hlonstring, true);
                }
            }

            //Airport codes:
            if (makelang == "sv")
            {
                if (iatadict.ContainsKey(gnid))
                    dummy.SetTemplateParameter("geobox", "IATA-kod", iatadict[gnid], true);
                if (icaodict.ContainsKey(gnid))
                    dummy.SetTemplateParameter("geobox", "ICAO-kod", icaodict[gnid], true);
            }
            else
            {
                if (iatadict.ContainsKey(gnid))
                {
                    dummy.SetTemplateParameter("geobox", "free", iatadict[gnid], true);
                    dummy.SetTemplateParameter("geobox", "free_type", "[[IATA]]", true);
                }
                if (icaodict.ContainsKey(gnid))
                {
                    dummy.SetTemplateParameter("geobox", "free1", icaodict[gnid], true);
                    dummy.SetTemplateParameter("geobox", "free1_type", "[[ICAO]]", true);
                }
            }


            //Console.WriteLine("3:" + dummy.text.Substring(0, 30));

            if (locatoringeobox) //only works in Swedish!
            {
                string countryname = countrydict[icountry].Name;

                //if (String.IsNullOrEmpty(locatordict[countryname].locatorimage))
                //{
                //string templatename = util.mp(63,null)+util.mp(72).Replace("{{", "") + " " + locatordict[countryname];
                //Console.WriteLine(templatename);
                //string imagename = "";
                //Page ltp = new Page(makesite, templatename);
                //util.tryload(ltp, 2);
                //if (ltp.Exists())
                //{
                //    imagename = get_pictureparam(ltp);
                //}

                //if (!String.IsNullOrEmpty(imagename))
                //{
                //    locatordict[countryname].locatorimage = imagename;
                //}
                //}

                if (locatordict.ContainsKey(countryname))
                {
                    string templatestart = util.mp(72) + " " + locatordict[countryname].get_locator(gndict[gnid].latitude, gndict[gnid].longitude);
                    string imagename = "{{xxx|reliefkarta_om_den_finns}}".Replace("{{xxx", templatestart).Replace("bild", util.mp(171));
                    //string imagename = "{{#if:xxx|bild1}}|xxx|bild1}}|xxx|bild}}}}".Replace("xxx", templatestart).Replace("bild", util.mp(171));
                    string[] p143 = new string[1] { countrynameml };
                    dummy.SetTemplateParameter("geobox", "map", imagename, true);
                    dummy.SetTemplateParameter("geobox", "map_locator", locatordict[countryname].get_locator(gndict[gnid].latitude, gndict[gnid].longitude), true);
                    dummy.SetTemplateParameter("geobox", "map_caption", util.mp(143, p143), true);
                }
            }

            //Console.WriteLine("Efter:" + dummy.text.Substring(0, 30));

            if (makehtml)
                return wikitohtml.make_html_box(dummy, makesite);
            else
                return dummy.text;

        }

        public static string get_pictureparam(Page ltp)
        {
            string imagename = "";
            string[] param = ltp.text.Split('|');
            foreach (string par in param)
            {
                if (((par.Trim().ToLower().IndexOf("bild") == 0) || (par.Trim().ToLower().IndexOf("image") == 0)) && (par.Contains("=")))
                {
                    imagename = par.Split('=')[1].Trim();
                    if (imagename.Contains("}}"))
                        imagename = imagename.Remove(imagename.IndexOf("}}")).Trim();
                    //Console.WriteLine("imagename = " + imagename);
                    break;
                }
            }
            return imagename;
        }

        public static double get_edgeparam(Page ltp, string edgepar)
        {
            string imagename = "";
            string[] param = ltp.text.Split('|');
            foreach (string par in param)
            {
                if ((par.Trim().ToLower().IndexOf(edgepar) == 0) && (par.Contains("=")))
                {
                    imagename = par.Split('=')[1].Trim();
                    if (imagename.Contains("}}"))
                        imagename = imagename.Remove(imagename.IndexOf("}}")).Trim();
                    //Console.WriteLine("imagename = " + imagename);
                    break;
                }
            }
            double x = util.tryconvertdouble(imagename);
            if (x == -1)
                x = -999;
            return x;
        }

        public static bool exists_at_commons(string imagename)
        {
            string res = cmsite.indexPath + "?title=" +
                        WebUtility.UrlEncode("File:" + imagename);
            //Console.WriteLine("commonsres = " + res);
            string src = "";
            try
            {
                src = cmsite.GetWebPage(res); // cmsite.GetPageHTM(res);
                Console.WriteLine("Found at Commons: " + imagename);
                return true;
            }
            catch (WebException e)
            {
                //newpix[newpic] = "/// NOT FOUND ON COMMONS";
                string message = e.Message;
                if (message.Contains(": (404) "))
                {		// Not Found
                    Console.Error.WriteLine(Bot.Msg("Page \"{0}\" doesn't exist."), imagename);
                    Console.WriteLine("Image not found: " + imagename);
                    //continue;
                }
                else
                {
                    Console.Error.WriteLine(message);
                    //continue;
                }
                return false;
            }

        }

        public static void fix_positionmaps()
        {
            //https://tools.wmflabs.org/magnus-toolserver/commonsapi.php?image=Bhutan_location_map.svg

            Dictionary<string, string> replacedict1 = new Dictionary<string, string>();
            Dictionary<string, string> replacedict2 = new Dictionary<string, string>();
            replacedict1.Add("|topp", "| topp");
            replacedict1.Add(" topp=", " topp =");
            replacedict1.Add("|botten", "| botten");
            replacedict1.Add(" botten=", " botten =");
            replacedict1.Add("|vänster", "| vänster");
            replacedict1.Add(" vänster=", " vänster =");
            replacedict1.Add("|höger", "| höger");
            replacedict1.Add(" höger=", " höger =");
            replacedict1.Add("|bild", "| bild");
            replacedict1.Add(" bild=", " bild =");

            replacedict2.Add("| topp ", "| topp|top ");
            replacedict2.Add("| botten ", "| botten|bottom ");
            replacedict2.Add("| vänster ", "| vänster|left ");
            replacedict2.Add("| höger ", "| höger|right ");


            foreach (string countryname in locatordict.Keys)
            {
                Console.WriteLine("countryname = " + countryname);
                string templatename = util.mp(63) + util.mp(72).Replace("{{", "") + " " + locatordict[countryname].locatorname;
                Console.WriteLine(templatename);
                string imagename = "";
                Page ltp = new Page(makesite, templatename);
                util.tryload(ltp, 2);
                if (ltp.Exists())
                {
                    if (ltp.text.Contains("topp|top"))
                        continue;
                    imagename = get_pictureparam(ltp);
                    if (!String.IsNullOrEmpty(imagename))
                    {

                        foreach (KeyValuePair<string, string> replacepair in replacedict1)
                        {
                            ltp.text = ltp.text.Replace(replacepair.Key, replacepair.Value);
                        }
                        foreach (KeyValuePair<string, string> replacepair in replacedict2)
                        {
                            ltp.text = ltp.text.Replace(replacepair.Key, replacepair.Value);
                        }

                        XmlDocument xd = new XmlDocument();
                        string cmurl = "https://tools.wmflabs.org/magnus-toolserver/commonsapi.php?image=" + imagename;
                        string s = wdtreeclass.get_webpage(cmurl);
                        if (!String.IsNullOrEmpty(s))
                        {
                            //Console.WriteLine(s);
                            xd.LoadXml(s);
                            XmlNodeList elemlist1 = xd.GetElementsByTagName("width");
                            double width = -1;
                            foreach (XmlNode ee in elemlist1)
                                width = util.tryconvertdouble(ee.InnerXml);
                            XmlNodeList elemlist2 = xd.GetElementsByTagName("height");
                            double height = -1;
                            foreach (XmlNode ee in elemlist2)
                                height = util.tryconvertdouble(ee.InnerXml);
                            Console.WriteLine("w,h = " + width.ToString() + ", " + height.ToString());
                            double ratio = height / width;
                            if (!ltp.text.Contains("ratio"))
                                ltp.text = ltp.text.Replace("| bild ", "| ratio = " + ratio.ToString(culture_en) + "\n| bild ");

                        }

                    }
                    util.trysave(ltp, 2, util.mp(60, null) + " " + util.mp(63, null));
                    string redirectname = util.mp(63) + "Geobox locator " + locatordict[countryname].locatorname;
                    make_redirect(redirectname, templatename, "Geobox locator|" + locatordict[countryname].locatorname, -1);

                    //Mall:Geobox locator Andorra
                    //#OMDIRIGERING[[Mall:Kartposition Andorra]]
                    //[[Kategori:Geobox locator|Andorra]]


                    //Console.WriteLine("<ret>");
                    //Console.ReadLine();
                }

            }
            Console.WriteLine("Done!");
            Console.ReadLine();

        }

        public static void get_page_area_pop_height(Page p, out double areaout, out long popout, out int heightout)
        {
            areaout = -1;
            popout = -1;
            heightout = -1;

            List<string> popparams = new List<string>();
            List<string> urbanparams = new List<string>();
            List<string> areaparams = new List<string>();
            List<string> heightparams = new List<string>();

            popparams.Add("population");
            popparams.Add("population_total");
            popparams.Add("population_urban");
            popparams.Add("population_metro");
            popparams.Add("befolkning");

            urbanparams.Add("population_urban");
            urbanparams.Add("population_metro");
            urbanparams.Add("población_urb");

            areaparams.Add("area_total_km2");
            areaparams.Add("area");
            areaparams.Add("fläche");
            areaparams.Add("superficie");
            areaparams.Add("yta");

            heightparams.Add("highest_elevation");
            heightparams.Add("elevation");

            bool foundpop = false;
            bool foundurban = false;
            bool foundarea = false;
            //bool foundheight = false;
            bool foundhighest = false;
            bool preferurban = true;

            long popwdurban = -1;

            foreach (string ttt in p.GetTemplates(true, true))
            {
                //Console.WriteLine(ttt);
                Dictionary<string, string> pdict = Page.ParseTemplate(ttt);
                foreach (string param in pdict.Keys)
                {
                    if (popparams.Contains(param) && !foundpop)
                    {
                        long popwd = util.tryconvertlong(pdict[param]);
                        if (popwd <= 0)
                        {
                            popwd = util.tryconvertlong(pdict[param].Split()[0]);
                        }
                        if (popwd > 0)
                        {
                            foundpop = true;
                            popout = popwd;
                        }
                    }
                    if (urbanparams.Contains(param) && !foundurban)
                    {
                        long popwd = util.tryconvertlong(pdict[param]);
                        if (popwd > 0)
                        {
                            foundurban = true;
                            popwdurban = popwd;
                        }
                    }
                    if (areaparams.Contains(param) && !foundarea)
                    {
                        double areawd = util.tryconvertdouble(pdict[param]);
                        if (areawd <= 0)
                        {
                            areawd = util.tryconvertdouble(pdict[param].Split()[0]);
                        }
                        Console.WriteLine("areaparam = " + pdict[param]);
                        Console.WriteLine("areawd = " + areawd.ToString());
                        if (areawd > 0)
                        {
                            foundarea = true;
                            areaout = areawd;
                        }
                    }

                    if (heightparams.Contains(param) && !foundhighest)
                    {
                        int heightwd = util.tryconvert(pdict[param]);
                        if (heightwd <= 0)
                        {
                            heightwd = util.tryconvert(pdict[param].Split()[0]);
                        }
                        Console.WriteLine("heightparam = " + param);
                        Console.WriteLine("heightwd = " + heightwd.ToString());
                        if (heightwd > 0)
                        {
                            //foundheight = true;
                            heightout = heightwd;
                            if (param.Contains("highest"))
                                foundhighest = true;
                        }
                    }


                }

            }

            if (preferurban && foundurban)
            {
                popout = popwdurban;
            }
        }

        public static void get_wd_area_pop(int wdid, XmlDocument cx, out double areawdout, out long popwdout, out string iwsout, bool preferurban)
        {
            areawdout = -1.0;
            popwdout = -1;
            iwsout = "";
            long popwdurban = -1;
            string iwsurban = "";

            List<string> popparams = new List<string>();
            List<string> urbanparams = new List<string>();
            List<string> areaparams = new List<string>();
            List<string> paramlangs = new List<string>();

            popparams.Add("population");
            popparams.Add("population_total");
            popparams.Add("population_urban");
            popparams.Add("population_metro");
            popparams.Add("poblacion");
            popparams.Add("población");
            popparams.Add("población_urb");
            popparams.Add("einwohner");

            urbanparams.Add("population_urban");
            urbanparams.Add("population_metro");
            urbanparams.Add("población_urb");

            areaparams.Add("area_total_km2");
            areaparams.Add("area");
            areaparams.Add("fläche");
            areaparams.Add("superficie");

            paramlangs.Add("en");
            paramlangs.Add("de");
            paramlangs.Add("fr");
            paramlangs.Add("es");
            if (!paramlangs.Contains(countrydict[countryid[makecountry]].nativewiki) && !String.IsNullOrEmpty(countrydict[countryid[makecountry]].nativewiki))
                paramlangs.Add(countrydict[countryid[makecountry]].nativewiki);

            string badlang = "";
            foreach (string iwl in paramlangs)
            {
                if ((!iwsites.ContainsKey(iwl)) && (iwl != makelang))
                {
                    Console.WriteLine(iwl);
                    try
                    {
                        Site ssite = new Site("https://" + iwl + ".wikipedia.org", botname, password);
                        iwsites.Add(iwl, ssite);
                    }
                    catch (WebException e)
                    {
                        string message = e.Message;
                        Console.Error.WriteLine(message);
                        badlang = iwl;
                    }
                    catch (WikiBotException e)
                    {
                        string message = e.Message;
                        Console.Error.WriteLine(message);
                        badlang = iwl;
                    }
                }
            }
            if (!String.IsNullOrEmpty(badlang))
                paramlangs.Remove(badlang);

            if (cx == null)
                cx = wdtreeclass.get_wd_xml(wdid);
            if (cx == null)
            {
                Console.WriteLine("cx = null");
                return;
            }
            Dictionary<string, string> iwdict = wdtreeclass.get_wd_sitelinks(cx);

            bool foundpop = false;
            bool foundurban = false;
            bool foundarea = false;

            foreach (string iws in iwdict.Keys)
            {
                string iwss = iws.Replace("wiki", "");
                Console.WriteLine(iwss + ":" + iwdict[iws]);
                if ((paramlangs.Contains(iwss)) && (iwsites.ContainsKey(iwss)))
                {
                    Console.WriteLine(iws + ":" + iwdict[iws] + " Paramlang!");
                    Page iwpage = new Page(iwsites[iwss], iwdict[iws]);
                    if (util.tryload(iwpage, 1))
                        if (iwpage.Exists())
                        {
                            foreach (string ttt in iwpage.GetTemplates(true, true))
                            {
                                //Console.WriteLine(ttt);
                                Dictionary<string, string> pdict = Page.ParseTemplate(ttt);// iwsites[iwss].ParseTemplate(ttt);
                                foreach (string param in pdict.Keys)
                                {
                                    if (popparams.Contains(param) && !foundpop)
                                    {
                                        long popwd = util.tryconvertlong(pdict[param]);
                                        if (popwd > 0)
                                        {
                                            foundpop = true;
                                            popwdout = popwd;
                                            iwsout = iwss;
                                        }
                                    }
                                    if (urbanparams.Contains(param) && !foundurban)
                                    {
                                        long popwd = util.tryconvertlong(pdict[param]);
                                        if (popwd > 0)
                                        {
                                            foundurban = true;
                                            popwdurban = popwd;
                                            iwsurban = iwss;
                                        }
                                    }
                                    if (areaparams.Contains(param) && !foundarea)
                                    {
                                        double areawd = util.tryconvertdouble(pdict[param]);
                                        if (areawd <= 0)
                                        {
                                            areawd = util.tryconvertdouble(pdict[param].Split()[0]);
                                        }
                                        Console.WriteLine("areaparam = " + pdict[param]);
                                        Console.WriteLine("areawd = " + areawd.ToString());
                                        if (areawd > 0)
                                        {
                                            foundarea = true;
                                            areawdout = areawd;
                                        }
                                    }
                                }

                            }
                            if (foundarea && foundpop && (foundurban || !preferurban))
                                break;

                        }
                }

            }

            if (preferurban && foundurban)
            {
                popwdout = popwdurban;
                iwsout = iwsurban;
            }

        }


        public static void check_wikidata()
        {
            int nwd = 0;
            int npop = 0;
            int narea = 0;


            using (StreamWriter sw = new StreamWriter("wikidata-" + makecountry + ".txt"))
            {

                int ngnid = gndict.Count;

                foreach (int gnid in gndict.Keys)
                {
                    Console.WriteLine("=====" + makecountry + "======== " + ngnid.ToString() + " remaining. ===========");
                    ngnid--;
                    if ((ngnid % 1000) == 0)
                    {
                        Console.WriteLine("Garbage collection:");
                        GC.Collect();
                    }

                    //wdid = wdtreeclass.get_wd_item(gnid);
                    if (gndict[gnid].wdid <= 0)
                        continue;
                    else
                        wdid = gndict[gnid].wdid;

                    Console.WriteLine(gndict[gnid].Name + ": " + wdid.ToString());
                    if (wdid > 0)
                    {
                        nwd++;

                        double areawd = -1.0;
                        long popwd = -1;
                        string iwss = "";

                        bool preferurban = (gndict[gnid].featureclass == 'P');
                        get_wd_area_pop(wdid, null, out areawd, out popwd, out iwss, preferurban);

                        if (popwd > 0)
                        {
                            Console.WriteLine("popwd = " + popwd.ToString());
                            gndict[gnid].population_wd = popwd;
                            gndict[gnid].population_wd_iw = iwss;
                            npop++;
                        }

                        if (areawd > 0)
                        {
                            gndict[gnid].area = areawd;
                            narea++;
                        }


                        //Console.WriteLine("<ret>");
                        //Console.ReadLine();
                        sw.WriteLine(gnid.ToString() + tabstring + wdid.ToString() + tabstring + gndict[gnid].area.ToString() + tabstring + gndict[gnid].population_wd.ToString() + tabstring + gndict[gnid].population_wd_iw);
                    }
                }
                Console.WriteLine("nwd = " + nwd.ToString());
                Console.WriteLine("npop = " + npop.ToString());
                Console.WriteLine("narea = " + narea.ToString());
                Console.WriteLine("gndict = " + gndict.Count.ToString());
                nwdhist.PrintSHist();

            }
        }

        public static void read_good_wd_file()
        {
            Console.WriteLine("read_good_wd_file");
            int nwdtot = 0;
            if (!File.Exists("wikidata-good.nt"))
                return;

            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "wikidata-good.nt"))
            {
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    string[] words = line.Split('\t');

                    if (words.Length < 2)
                        continue;

                    nwdtot++;

                    int gnid = util.tryconvert(words[0]);
                    if (!gndict.ContainsKey(gnid))
                        continue;

                    int wdid = util.tryconvert(words[1]);
                    if (wdid <= 0)
                        continue;

                    gndict[gnid].wdid = wdid;

                    if (!wdgniddict.ContainsKey(wdid))
                        wdgniddict.Add(wdid, gnid);
                    else if (wdgniddict[wdid] != gnid)
                    {// negative numbers count how many duplicates
                        if (wdgniddict[wdid] > 0)
                            wdgniddict[wdid] = -2;
                        else
                            wdgniddict[wdid]--;
                    }
                }
                if ((nwdtot % 10000) == 0)
                    Console.WriteLine("nwdtot = " + nwdtot.ToString());
            }

            Console.WriteLine("nwdtot = " + nwdtot.ToString());

        }



        public static Dictionary<int, int> read_wd_dict(string wdcountry)
        {
            Dictionary<int, int> rdict = new Dictionary<int, int>();
            string filename = geonameclass.geonamesfolder + "wikidata\\wikidata-" + wdcountry + ".txt";
            string filename_override = geonameclass.geonamesfolder + "wikidata\\wikidata-" + wdcountry + "-override.txt";

            if (!File.Exists(filename))
            {
                Console.WriteLine("No file " + filename);
                return rdict;
            }

            Dictionary<int, int> overridedict = new Dictionary<int, int>();
            if (File.Exists(filename_override)) //use to override wd assignments in case of systematic errors in main wd run
            {
                int nover = 0;
                using (StreamReader sr = new StreamReader(filename_override))
                {
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        string[] words = line.Split('\t');
                        if (words.Length < 2)
                            continue;
                        int gnid0 = util.tryconvert(words[0]);
                        int gnid1 = util.tryconvert(words[1]);
                        if ((gnid0 <= 0) || (gnid1 <= 0))
                            continue;

                        if (overridedict.ContainsKey(gnid0))
                            continue;

                        overridedict.Add(gnid0, gnid1);
                        nover++;
                    }
                }
                Console.WriteLine("noverride = " + nover.ToString());
            }

            try
            {
                List<int> withwd = new List<int>();

                //first pass, in order to doublecheck overridedict
                if (overridedict.Count > 0)
                {
                    Console.WriteLine("First pass...");
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        while (!sr.EndOfStream)
                        {
                            String line = sr.ReadLine();
                            string[] words = line.Split('\t');
                            //sw.WriteLine(gnid.ToString() + tabstring + wdid.ToString() + tabstring + gndict[gnid].area.ToString() + tabstring + gndict[gnid].population_wd.ToString() + tabstring + gndict[gnid].population_wd_iw);

                            if (words.Length < 4)
                                continue;

                            int gnid = util.tryconvert(words[0]);
                            withwd.Add(gnid);
                        }
                    }
                    foreach (int gnid in withwd) //remove from overridedict those where both have wd match
                    {
                        if (overridedict.ContainsKey(gnid))
                            if (withwd.Contains(overridedict[gnid]))
                                overridedict.Remove(gnid);
                    }
                    Console.WriteLine("Remaining in overridedict: " + overridedict.Count.ToString());
                }


                using (StreamReader sr = new StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        string[] words = line.Split('\t');
                        //sw.WriteLine(gnid.ToString() + tabstring + wdid.ToString() + tabstring + gndict[gnid].area.ToString() + tabstring + gndict[gnid].population_wd.ToString() + tabstring + gndict[gnid].population_wd_iw);

                        if (words.Length < 4)
                            continue;

                        int gnid = util.tryconvert(words[0]);

                        if (overridedict.ContainsKey(gnid))
                        {
                            Console.WriteLine("Overriding " + gnid.ToString() + " with " + overridedict[gnid].ToString());
                            gnid = overridedict[gnid];
                        }

                        int wdid = util.tryconvert(words[1]);
                        if (wdid <= 0)
                            continue;

                        nwdtot++;
                        rdict.Add(gnid, wdid);
                    }
                }
            }
            catch (IOException e)
            {
                string message = e.Message;
                Console.Error.WriteLine(message);
            }

            return rdict;
        }


        public static void read_wd_file(string wdcountry)
        {
            string filename = geonameclass.geonamesfolder + "wikidata\\wikidata-" + wdcountry + ".txt";
            string filename_override = geonameclass.geonamesfolder + "wikidata\\wikidata-" + wdcountry + "-override.txt";

            if (!File.Exists(filename))
            {
                Console.WriteLine("No file " + filename);
                return;
            }

            Dictionary<int, int> overridedict = new Dictionary<int, int>();
            if (File.Exists(filename_override)) //use to override wd assignments in case of systematic errors in main wd run
            {
                int nover = 0;
                using (StreamReader sr = new StreamReader(filename_override))
                {
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        string[] words = line.Split('\t');
                        if (words.Length < 2)
                            continue;
                        int gnid0 = util.tryconvert(words[0]);
                        int gnid1 = util.tryconvert(words[1]);
                        if ((gnid0 <= 0) || (gnid1 <= 0))
                            continue;

                        if (overridedict.ContainsKey(gnid0))
                            continue;

                        overridedict.Add(gnid0, gnid1);
                        nover++;
                    }
                }
                Console.WriteLine("noverride = " + nover.ToString());
            }

            try
            {
                wdtime = File.GetCreationTime(@filename);
                //Console.WriteLine("wdtime = "+wdtime.ToString());
                //Console.WriteLine("<ret>");
                //Console.ReadLine();

                List<int> withwd = new List<int>();

                //first pass, in order to doublecheck overridedict
                if (overridedict.Count > 0)
                {
                    Console.WriteLine("First pass...");
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        while (!sr.EndOfStream)
                        {
                            String line = sr.ReadLine();
                            string[] words = line.Split('\t');
                            //sw.WriteLine(gnid.ToString() + tabstring + wdid.ToString() + tabstring + gndict[gnid].area.ToString() + tabstring + gndict[gnid].population_wd.ToString() + tabstring + gndict[gnid].population_wd_iw);

                            if (words.Length < 4)
                                continue;

                            int gnid = util.tryconvert(words[0]);
                            withwd.Add(gnid);
                        }
                    }
                    foreach (int gnid in withwd) //remove from overridedict those where both have wd match
                    {
                        if (overridedict.ContainsKey(gnid))
                            if (withwd.Contains(overridedict[gnid]))
                                overridedict.Remove(gnid);
                    }
                    Console.WriteLine("Remaining in overridedict: " + overridedict.Count.ToString());
                }


                using (StreamReader sr = new StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        string[] words = line.Split('\t');
                        //sw.WriteLine(gnid.ToString() + tabstring + wdid.ToString() + tabstring + gndict[gnid].area.ToString() + tabstring + gndict[gnid].population_wd.ToString() + tabstring + gndict[gnid].population_wd_iw);

                        if (words.Length < 4)
                            continue;

                        int gnid = util.tryconvert(words[0]);

                        if (overridedict.ContainsKey(gnid))
                        {
                            Console.WriteLine("Overriding " + gnid.ToString() + " with " + overridedict[gnid].ToString());
                            gnid = overridedict[gnid];
                        }

                        if (!gndict.ContainsKey(gnid))
                            continue;

                        int wdid = util.tryconvert(words[1]);
                        if (wdid <= 0)
                            continue;

                        nwdtot++;
                        gndict[gnid].wdid = wdid;


                        if (!wdgniddict.ContainsKey(wdid))
                            wdgniddict.Add(wdid, gnid);
                        else if (wdgniddict[wdid] != gnid)
                        {// negative numbers count how many duplicates
                            if (wdgniddict[wdid] > 0)
                                wdgniddict[wdid] = -2;
                            else
                                wdgniddict[wdid]--;
                        }


                        double area = util.tryconvertdouble(words[2]);
                        if (area > 0)
                        {
                            //Console.WriteLine("area>0");
                            if (verifygeonames && (gndict[gnid].area > 0))
                            {
                                //Console.WriteLine("gnid-area>0");
                                double eps = 0;
                                while (areavsarea.ContainsKey(area + eps))
                                    eps += 0.00001;
                                areavsarea.Add(area + eps, gndict[gnid].area + eps);
                            }
                            gndict[gnid].area = area;
                        }

                        long pop = util.tryconvertlong(words[3]);
                        if (pop > 0)
                        {
                            if (verifygeonames && (gndict[gnid].population > 10))
                            {
                                Console.WriteLine("pop.vs.pop:" + gndict[gnid].population.ToString() + tabstring + pop.ToString());
                                int j = 0;
                                while (popvspop.ContainsKey(pop + j))
                                    j++;
                                popvspop.Add(pop + j, gndict[gnid].population + j);
                            }
                            if (((gndict[gnid].population < minimum_population) || !prefergeonamespop) && !verifygeonames)
                                gndict[gnid].population = pop;
                            gndict[gnid].population_wd = pop;
                            if (words.Length >= 5)
                                gndict[gnid].population_wd_iw = words[4];

                            //Console.WriteLine(gndict[gnid].Name + ": " + gndict[gnid].population_wd.ToString() + gndict[gnid].population_wd_iw);
                        }
                        //public static Dictionary<int, int> wdgnid = new Dictionary<int, int>(); //from wikidata id to geonames id; negative counts duplicates
                        //public static Dictionary<long, long> popvspop = new Dictionary<long, long>(); //comparing population for same place, wd vs gn
                        //public static Dictionary<double, double> areavsarea = new Dictionary<double, double>(); //comparing area for same place, wd vs gn


                    }
                }
            }
            catch (IOException e)
            {
                string message = e.Message;
                Console.Error.WriteLine(message);
            }


        }

        public static void read_wd_files(string countrycode)
        {
            if (countrycode == "")
            {
                foreach (int gnid in countrydict.Keys)
                    read_wd_file(countrydict[gnid].iso);
            }
            else
                read_wd_file(countrycode);
        }


        public static string get_terrain_type2(List<int> farlist, int gnid)
        {
            string terrain_type = "unknown";

            //int n = 0;
            //int nelev = 0;
            //double elevationsum = 0.0;
            //double elevationvar = 0.0;
            //double elevationsquare = 0.0;
            //double elevationmean = 0.0;

            //double[] elevdirsum = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            //double[] elevdirmean = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            //int[] nelevdir = { 0, 0, 0, 0, 0, 0, 0, 0 };

            //foreach (int nb in farlist)
            //{
            //    if (!gndict.ContainsKey(nb))
            //        continue;

            //    n++;

            //    if (gndict[nb].elevation > 0)
            //    {
            //        nelev++;
            //        elevationsum += gndict[nb].elevation;
            //        elevationsquare += gndict[nb].elevation * gndict[nb].elevation;
            //        int dir = get_direction(gnid, nb);
            //        if (dir > 0)
            //        {
            //            nelevdir[dir - 1]++;
            //            elevdirsum[dir - 1] += gndict[nb].elevation;
            //        }
            //    }

            //}


            //if (nelev > 10)
            //{
            //    elevationmean = elevationsum / nelev;
            //    elevationvar = elevationsquare / nelev - elevationmean * elevationmean;


            //    Console.WriteLine("elevation mean, var = " + elevationmean.ToString() + ", " + elevationvar.ToString());

            //    if (statisticsonly)
            //        evarhist.Add(elevationvar);

            //    terrain_type = terrain_label(elevationvar,elevationmean);



            //    if (gndict[gnid].elevation > 0)
            //    {
            //        if (elevationmean - gndict[gnid].elevation > 500)
            //            terrain_type += " valley";
            //        else if (elevationmean - gndict[gnid].elevation < -500)
            //            terrain_type += " peak";
            //    }

            //    //      1
            //    //   7    5
            //    //  3      4
            //    //   8    6
            //    //      2

            //    int ndir = 0;
            //    if (nelev > 20)
            //    {
            //        for (int i = 0; i < 8; i++)
            //        {
            //            if (nelevdir[i] > 0)
            //            {
            //                elevdirmean[i] = elevdirsum[i] / nelevdir[i];
            //                ndir++;
            //            }
            //            else
            //                elevdirmean[i] = -1.0;
            //        }
            //    }

            //    if (statisticsonly)
            //        ndirhist.Add(ndir);


            //}

            //if (statisticsonly)
            //{
            //    Console.WriteLine(gndict[gnid].Name + ": " + terrain_type);
            //    terrainhist.Add(terrain_type);
            //}
            return terrain_type;
        }

        public static int get_altitude(int gnid)
        {
            string dir = extractdir;
            double lat = gndict[gnid].latitude;
            double lon = gndict[gnid].longitude;
            string filename = make_hgt_filename(lat, lon);

            int[,] map = get_hgt_array(dir + filename);
            int mapsize = map.GetLength(0);

            double xfraction = lon - Math.Floor(lon);
            int pixx = Convert.ToInt32(mapsize * xfraction); //x counted in positive longitude direction
            double yfraction = lat - Math.Floor(lat);
            int pixy = mapsize - Convert.ToInt32(mapsize * yfraction); //y counted in negative latitude direction
            if (pixx >= mapsize)
                pixx = mapsize - 1;
            if (pixy >= mapsize)
                pixy = mapsize - 1;
            //Console.WriteLine(gnid.ToString() + ": " + pixx.ToString() + ", " + pixy.ToString());
            int alt = map[pixx, pixy];
            if (alt == 32768) //bad pixel
                alt = 0;
            return alt;
        }

        public static string classify_terrain(double elevationvar, double elevationmean)
        {
            string terrain_type = "";

            if (elevationvar < 2500.0) //rms < 50
            {
                terrain_type = "flat";
                if (elevationvar < 100) //rms < 10
                    terrain_type = "very " + terrain_type;
                if (elevationmean > 1500) //average higher than 1500
                    terrain_type += " high";
            }
            else if (elevationvar < 62500.0) //rms < 250
            {
                terrain_type = "hilly";
                if (elevationvar < 20000) //rms < 140
                    terrain_type = "somewhat " + terrain_type;
                if (elevationmean > 1500) //average higher than 1500
                    terrain_type += " high";
            }
            else if (elevationvar < 122500.0) //rms < 350
                terrain_type = "low-mountains";
            else if (elevationvar < 250000.0) //rms < 500
                terrain_type = "medium-mountains";
            else
                terrain_type = "high-mountains";

            return terrain_type;
        }

        public static int next_dir(int dir)
        {
            //      1
            //   7    5
            //  3      4
            //   8    6
            //      2

            switch (dir)
            {
                case 1:
                    return 5;
                case 5:
                    return 4;
                case 4:
                    return 6;
                case 6:
                    return 2;
                case 2:
                    return 8;
                case 8:
                    return 3;
                case 3:
                    return 7;
                case 7:
                    return 1;
                default:
                    return -1;
            }

        }

        public static void put_mountains_on_map(ref int[,] map, double lat, double lon)
        {
            int mapsize = map.GetLength(0);
            List<int> farlist = getneighbors(lat, lon, 20.0);
            foreach (int nb in farlist)
            {
                if (is_height(gndict[nb].featurecode))
                {
                    int xnb = get_x_pixel(gndict[nb].longitude, lon);
                    if ((xnb < 0) || (xnb >= mapsize))
                        continue;
                    int ynb = get_y_pixel(gndict[nb].latitude, lat);
                    if ((ynb < 0) || (ynb >= mapsize))
                        continue;
                    map[xnb, ynb] = nb;
                }
            }
        }


        public static void put_category_on_map(ref int[,] map, double lat, double lon, string category, double marksize) //puts gnid on map pixel
        {
            int mapsize = map.GetLength(0);

            double scale = Math.Cos(lat * 3.1416 / 180);
            double pixkmx = scale * 40000 / (360 * 1200);
            double pixkmy = 40000.0 / (360.0 * 1200.0);
            int markx = Convert.ToInt32(marksize / pixkmx);
            int marky = Convert.ToInt32(marksize / pixkmy);
            Console.WriteLine("markx,marky = " + markx + " " + marky);

            int nput = 0;
            Console.WriteLine(gndict.Count + " in gndict");
            foreach (int gnid in gndict.Keys)
            {
                //Console.WriteLine("gnid = " + gnid);
                if (categorydict.ContainsKey(gndict[gnid].featurecode) && (categorydict[gndict[gnid].featurecode] == category))
                {
                    int xgnid = get_x_pixel(gndict[gnid].longitude, lon, mapsize / 3);
                    int ygnid = get_y_pixel(gndict[gnid].latitude, lat, mapsize / 3);
                    if (in_map(xgnid, ygnid, mapsize))
                    {
                        for (int u = -markx; u < markx; u++)
                            for (int v = -marky; v < marky; v++)
                            {
                                if (in_map(xgnid + u, ygnid + v, mapsize))
                                {
                                    map[xgnid + u, ygnid + v] = gnid;
                                }
                            }
                        nput++;
                        //Console.WriteLine("put gnid " + gnid);
                    }

                }
            }
            Console.WriteLine("nput = " + nput);
        }

        public static List<int> list_category_in_map(int mapsize, double lat, double lon, string category) //puts gnid on map pixel, and returns list of put items
        {
            List<int> gnidlist = new List<int>();

            foreach (int gnid in gndict.Keys)
            {
                if (categorydict.ContainsKey(gndict[gnid].featurecode) && (categorydict[gndict[gnid].featurecode] == category))
                {
                    int xgnid = get_x_pixel(gndict[gnid].longitude, lon, mapsize / 3);
                    int ygnid = get_y_pixel(gndict[gnid].latitude, lat, mapsize / 3);
                    if (in_map(xgnid, ygnid, mapsize))
                        gnidlist.Add(gnid);
                }
            }

            return gnidlist;
        }





        public static int get_summit(int gnid, out double slat, out double slon) //seeks proper DEM summit of a mountain.
        {
            Console.WriteLine("get_summit");
            double lat = gndict[gnid].latitude;
            double lon = gndict[gnid].longitude;
            int[,] map = get_3x3map(lat, lon);
            int mapsize = map.GetLength(0);
            //double scale = Math.Cos(lat * 3.1416 / 180);
            //double pixkmx = scale * 40000 / (360 * 1200);
            //double pixkmy = 40000.0 / (360.0 * 1200.0);

            int x0 = get_x_pixel(lon, lon);
            int y0 = get_y_pixel(lat, lat);

            int[,] donemap = new int[mapsize, mapsize];

            for (int i = 0; i < mapsize; i++)
                for (int j = 0; j < mapsize; j++)
                    donemap[i, j] = 0;
            put_mountains_on_map(ref donemap, lat, lon);

            donemap[x0, y0] = -1; //negative for done, positive for mountain gnid

            int tolerance = -1; //maximum dip before going up

            int xhout = -1;
            int yhout = -1;

            //Console.WriteLine("x0,yo = " + x0.ToString() + " " + y0.ToString());
            int mtgnid = seek_highest(ref map, ref donemap, x0, y0, tolerance, out xhout, out yhout);
            //Console.WriteLine("xh,yh = " + xhout.ToString() + " " + yhout.ToString());

            double one1200 = 1.0 / 1200.0;
            double dlon = (xhout - x0) * one1200;
            double dlat = -(yhout - y0) * one1200; //reverse sign because higher pixel number is lower latitude
            slat = gndict[gnid].latitude + dlat;
            slon = gndict[gnid].longitude + dlon;

            if (mtgnid > 0) //Another mountain on summit
                return -1;
            else if (xhout < 0)
                return -1;
            else
                return map[xhout, yhout];

        }


        public static int seek_highest(ref int[,] map, ref int[,] donemap, int x0, int y0, int tolerance, out int xhout, out int yhout)
        {
            //Find nearest mountain from x0,y0. 
            //Assumes donemap contains gnid of mountains in appropriate cells.
            //Donemap =  0: untouched empty cell
            //Donemap = -1: cell touched here
            //Donemap = -2: cell to skip
            //Tolerance = permitted dip before terrain turns upwards (negative!)

            Console.WriteLine("seek_highest");

            int mapsize = map.GetLength(0);

            int maxx = x0;
            int maxy = y0;
            int minx = x0;
            int miny = y0;
            int xhigh = x0;
            int yhigh = y0;
            int xhighest = -1;
            int yhighest = -1;
            xhout = 0;
            yhout = 0;

            int x = x0;
            int y = y0;
            int newhigh = map[x0, y0];
            int highesthigh = newhigh;
            int nsame = 0;
            int nround = 0;
            int maxround = 1000;

            int maxsame = 1000;
            int[] xh = new int[maxsame];
            int[] yh = new int[maxsame];


            while (((newhigh - map[x0, y0]) >= tolerance) && (newhigh - highesthigh >= 5 * tolerance))
            {
                nround++;
                //Console.WriteLine("nround = " + nround.ToString());
                if (nround > maxround)
                    break;

                newhigh = tolerance - 1;
                for (int i = minx; i <= maxx; i++)
                    for (int j = miny; j <= maxy; j++)
                    {
                        if (donemap[i, j] == -1)
                        {
                            //Console.WriteLine("i,j=" + i.ToString() +","+ j.ToString());
                            for (int u = -1; u <= 1; u++)
                                if ((i + u > 0) && (i + u < mapsize))
                                    for (int v = -1; v <= 1; v++)
                                        if ((j + v > 0) && (j + v < mapsize))
                                            if (donemap[i + u, j + v] >= 0)
                                            {
                                                if (map[i + u, j + v] > newhigh)
                                                {
                                                    newhigh = map[i + u, j + v];
                                                    xhigh = i + u;
                                                    yhigh = j + v;
                                                    nsame = 0;
                                                }
                                                else if (map[i + u, j + v] == newhigh)
                                                {
                                                    if (nsame < maxsame)
                                                    {
                                                        xh[nsame] = i + u;
                                                        yh[nsame] = j + v;
                                                        nsame++;
                                                        //xyh.Add(Tuple.Create(i + u,j+v));
                                                    }
                                                }

                                            }
                        }
                    }

                //Console.WriteLine("newhigh = " + newhigh.ToString());
                if (newhigh > highesthigh)
                {
                    highesthigh = newhigh;
                    xhighest = xhigh;
                    yhighest = yhigh;
                    Console.WriteLine("seek_highest: highesthigh " + highesthigh.ToString() + " xh,yh = " + xhighest.ToString() + ", " + yhighest.ToString());
                }

                if ((newhigh - map[x0, y0]) > tolerance)
                {
                    if (donemap[xhigh, yhigh] > 0)
                        break;

                    donemap[xhigh, yhigh] = -1;

                    if (nsame > 0)
                    {
                        //Console.WriteLine("seek_highest: nsame = " + nsame.ToString());
                        //foreach (Tuple xy in xyh)
                        //    donemap[xy.Item1,xy.Item2] = nround;
                        for (int isame = 0; isame < nsame; isame++)
                            donemap[xh[isame], yh[isame]] = -1;
                    }


                    if (xhigh > maxx)
                    {
                        maxx = xhigh;
                        Console.WriteLine("maxx = " + maxx.ToString());
                        if (maxx >= mapsize)
                            newhigh = -9999;
                        if (maxx >= x0 + 500)
                            newhigh = -9999;
                    }
                    if (xhigh < minx)
                    {
                        minx = xhigh;
                        Console.WriteLine("minx = " + minx.ToString());
                        if (minx <= 0)
                            newhigh = -9999;
                        if (minx <= x0 - 500)
                            newhigh = -9999;
                    }
                    if (yhigh > maxy)
                    {
                        maxy = yhigh;
                        Console.WriteLine("maxy = " + maxy.ToString());
                        if (maxy >= mapsize)
                            newhigh = -9999;
                        if (maxy >= y0 + 500)
                            newhigh = -9999;
                    }
                    if (yhigh < miny)
                    {
                        miny = yhigh;
                        Console.WriteLine("miny = " + miny.ToString());
                        if (miny <= 0)
                            newhigh = -9999;
                        if (miny <= y0 - 500)
                            newhigh = -9999;
                    }
                }

                if (newhigh <= 0)
                    break;

                //Console.WriteLine("xhigh,yhigh = " + xhigh.ToString() + ", " + yhigh.ToString());
            }

            xhout = xhighest;
            yhout = yhighest;
            return donemap[xhigh, yhigh];

        }



        public static int seek_mountain(ref int[,] map, ref int[,] donemap, int x0, int y0, int tolerance)
        {
            //Find nearest mountain from x0,y0. 
            //Assumes donemap contains gnid of mountains in appropriate cells.
            //Donemap =  0: untouched empty cell
            //Donemap = -1: cell touched here
            //Donemap = -2: cell to skip
            //Tolerance = permitted dip before terrain turns upwards (negative!)

            Console.WriteLine("seek_mountain");

            int mapsize = map.GetLength(0);

            int maxx = x0;
            int maxy = y0;
            int minx = x0;
            int miny = y0;
            int xhigh = 0;
            int yhigh = 0;

            int x = x0;
            int y = y0;
            int newhigh = map[x0, y0];
            int highesthigh = newhigh;
            int nsame = 0;
            int nround = 0;
            int maxround = 10000;

            int maxsame = 1000;
            int[] xh = new int[maxsame];
            int[] yh = new int[maxsame];


            while (donemap[x, y] <= 0 && ((newhigh - map[x0, y0]) > tolerance) && (newhigh - highesthigh > 5 * tolerance))
            {
                nround++;
                if (nround > maxround)
                    break;

                newhigh = tolerance - 1;
                for (int i = minx; i <= maxx; i++)
                    for (int j = miny; j <= maxy; j++)
                    {
                        if (donemap[i, j] == -1)
                        {
                            for (int u = -1; u <= 1; u++)
                                if ((i + u > 0) && (i + u < mapsize))
                                    for (int v = -1; v <= 1; v++)
                                        if ((j + v > 0) && (j + v < mapsize))
                                            if (donemap[i + u, j + v] >= 0)
                                            {
                                                if (map[i + u, j + v] > newhigh)
                                                {
                                                    newhigh = map[i + u, j + v];
                                                    xhigh = i + u;
                                                    yhigh = j + v;
                                                    nsame = 0;
                                                }
                                                else if (map[i + u, j + v] == newhigh)
                                                {
                                                    if (nsame < maxsame)
                                                    {
                                                        xh[nsame] = i + u;
                                                        yh[nsame] = j + v;
                                                        nsame++;
                                                        //xyh.Add(Tuple.Create(i + u,j+v));
                                                    }
                                                }

                                            }
                        }
                    }

                if (newhigh > highesthigh)
                    highesthigh = newhigh;

                if ((newhigh - map[x0, y0]) > tolerance)
                {
                    if (donemap[xhigh, yhigh] > 0)
                        break;

                    donemap[xhigh, yhigh] = -1;

                    if (nsame > 0)
                    {
                        Console.WriteLine("seek_mountain: nsame = " + nsame.ToString());
                        //foreach (Tuple xy in xyh)
                        //    donemap[xy.Item1,xy.Item2] = nround;
                        for (int isame = 0; isame < nsame; isame++)
                            donemap[xh[isame], yh[isame]] = -1;
                    }


                    if (xhigh > maxx)
                    {
                        maxx = xhigh;
                        Console.WriteLine("maxx = " + maxx.ToString());
                        if (maxx >= mapsize)
                            newhigh = -9999;
                        if (maxx >= x0 + 500)
                            newhigh = -9999;
                    }
                    if (xhigh < minx)
                    {
                        minx = xhigh;
                        Console.WriteLine("minx = " + minx.ToString());
                        if (minx <= 0)
                            newhigh = -9999;
                        if (minx <= x0 - 500)
                            newhigh = -9999;
                    }
                    if (yhigh > maxy)
                    {
                        maxy = yhigh;
                        Console.WriteLine("maxy = " + maxy.ToString());
                        if (maxy >= mapsize)
                            newhigh = -9999;
                        if (maxy >= y0 + 500)
                            newhigh = -9999;
                    }
                    if (yhigh < miny)
                    {
                        miny = yhigh;
                        Console.WriteLine("miny = " + miny.ToString());
                        if (miny <= 0)
                            newhigh = -9999;
                        if (miny <= y0 - 500)
                            newhigh = -9999;
                    }
                }

                if (newhigh <= 0)
                    break;

                //Console.WriteLine("xhigh,yhigh = " + xhigh.ToString() + ", " + yhigh.ToString());
            }

            return donemap[xhigh, yhigh];

        }


        public static bool between_mountains(int gnid, out int mgnid1, out int mgnid2) //calculates whether gnid is part of a mountain. Intended for use with spurs etc.
        {
            Console.WriteLine("Between_mountains");
            double lat = gndict[gnid].latitude;
            double lon = gndict[gnid].longitude;
            int[,] map = get_3x3map(lat, lon);
            int mapsize = map.GetLength(0);
            //double scale = Math.Cos(lat * 3.1416 / 180);
            //double pixkmx = scale * 40000 / (360 * 1200);
            //double pixkmy = 40000.0 / (360.0 * 1200.0);
            mgnid1 = -1;
            mgnid2 = -1;

            int x0 = get_x_pixel(lon, lon);
            int y0 = get_y_pixel(lat, lat);

            int[,] donemap = new int[mapsize, mapsize];

            for (int i = 0; i < mapsize; i++)
                for (int j = 0; j < mapsize; j++)
                    donemap[i, j] = 0;
            put_mountains_on_map(ref donemap, lat, lon);

            if (donemap[x0, y0] > 0)
                return false;

            donemap[x0, y0] = -1; //negative for done, positive for mountain gnid

            int tolerance = -10; //maximum dip before going up

            mgnid1 = seek_mountain(ref map, ref donemap, x0, y0, tolerance);

            if (!gndict.ContainsKey(mgnid1))
                return false;

            double dlat = gndict[mgnid1].latitude - lat;
            double dlon = gndict[mgnid1].longitude - lon;

            if (Math.Abs(dlat) > Math.Abs(dlon))
            {
                if (dlat > 0) //North (smaller y!)
                {
                    for (int i = 0; i < y0; i++)
                        for (int j = 0; j < mapsize; j++)
                            donemap[j, i] = -2;
                }
                else //South (larger y!)
                {
                    for (int i = y0 + 1; i < mapsize; i++)
                        for (int j = 0; j < mapsize; j++)
                            donemap[j, i] = -2;
                }
            }
            else
            {
                if (dlon > 0) //East
                {
                    for (int i = x0 + 1; i < mapsize; i++)
                        for (int j = 0; j < mapsize; j++)
                            donemap[i, j] = -2;
                }
                else //West
                {
                    for (int i = 0; i < x0; i++)
                        for (int j = 0; j < mapsize; j++)
                            donemap[i, j] = -2;
                }
            }
            mgnid2 = seek_mountain(ref map, ref donemap, x0, y0, tolerance);

            if (!gndict.ContainsKey(mgnid2))
                return false;

            return true;
        }




        public static int attach_to_mountain(int gnid) //calculates whether gnid is part of a mountain. Intended for use with spurs etc.
        {
            Console.WriteLine("attach_to_mountain");
            double lat = gndict[gnid].latitude;
            double lon = gndict[gnid].longitude;
            int[,] map = get_3x3map(lat, lon);
            int mapsize = map.GetLength(0);
            //double scale = Math.Cos(lat * 3.1416 / 180);
            //double pixkmx = scale * 40000 / (360 * 1200);
            //double pixkmy = 40000.0 / (360.0 * 1200.0);

            int x0 = get_x_pixel(lon, lon);
            int y0 = get_y_pixel(lat, lat);

            int[,] donemap = new int[mapsize, mapsize];

            for (int i = 0; i < mapsize; i++)
                for (int j = 0; j < mapsize; j++)
                    donemap[i, j] = 0;
            put_mountains_on_map(ref donemap, lat, lon);
            if (donemap[x0, y0] > 0)
                return donemap[x0, y0];
            donemap[x0, y0] = -1; //negative for done, positive for mountain gnid

            int tolerance = -10; //maximum dip before going up

            return seek_mountain(ref map, ref donemap, x0, y0, tolerance);

        }


        public static int get_prominence(int gnid, out double width) //calculates the topographic prominence of a mountain
        {
            Console.WriteLine("get_prominence");

            double lat = gndict[gnid].latitude;
            double lon = gndict[gnid].longitude;
            //Console.WriteLine("lat, lon = " + lat.ToString() + " " + lon.ToString());
            int[,] map = get_3x3map(lat, lon);
            int mapsize = map.GetLength(0);
            double scale = Math.Cos(lat * 3.1416 / 180);
            double pixkmx = scale * 40000 / (360 * 1200);
            double pixkmy = 40000.0 / (360.0 * 1200.0);
            width = 0;


            int x0 = get_x_pixel(lon, lon);
            int y0 = get_y_pixel(lat, lat);

            int[,] donemap = new int[mapsize, mapsize];

            bool higherfound = false;
            for (int i = 0; i < mapsize; i++)
                for (int j = 0; j < mapsize; j++)
                {
                    donemap[i, j] = 0;
                    if (map[i, j] > gndict[gnid].elevation)
                        higherfound = true;
                }

            if (!higherfound) //if highest point in map, algorithm won't work
                return -1;

            donemap[x0, y0] = 1;
            int maxx = x0;
            int maxy = y0;
            int minx = x0;
            int miny = y0;

            int lowesthigh = 9999;
            int newhigh = -1;
            int badhigh = 9999;
            int xhigh = 0;
            int yhigh = 0;
            int nround = 1;
            int nroundlow = -1;
            int nsame = 0;
            int ntotal = 0;
            int maxtotal = 100000;
            //Dictionary<int,int> xyhdict = new Dictionary<int,int>();
            //List<Tuple<int, int>> xyh = new List<Tuple<int, int>>();
            int maxsame = 1000;
            int[] xh = new int[maxsame];
            int[] yh = new int[maxsame];

            while ((newhigh < gndict[gnid].elevation) || (nround < 6)) //disregards the first 5 pixels, in case of slight position mismatch
            {
                newhigh = -1;
                for (int i = minx; i <= maxx; i++)
                    for (int j = miny; j <= maxy; j++)
                    {
                        if (donemap[i, j] > 0)
                        {
                            for (int u = -1; u <= 1; u++)
                                if ((i + u > 0) && (i + u < mapsize))
                                    for (int v = -1; v <= 1; v++)
                                        if ((j + v > 0) && (j + v < mapsize))
                                            if (donemap[i + u, j + v] == 0)
                                            {
                                                if (map[i + u, j + v] > newhigh)
                                                {
                                                    newhigh = map[i + u, j + v];
                                                    xhigh = i + u;
                                                    yhigh = j + v;
                                                    nsame = 0;
                                                }
                                                else if (map[i + u, j + v] == newhigh)
                                                {
                                                    if (nsame < maxsame)
                                                    {
                                                        xh[nsame] = i + u;
                                                        yh[nsame] = j + v;
                                                        nsame++;
                                                        //xyh.Add(Tuple.Create(i + u,j+v));
                                                    }
                                                }
                                            }
                        }
                    }
                nround++;
                //Console.WriteLine("get_prominence: nround,ntotal,newhigh = " + nround.ToString() + ", " + ntotal + ", " + newhigh);

                ntotal += nsame + 1;
                if (ntotal > maxtotal)
                    newhigh = badhigh;

                donemap[xhigh, yhigh] = nround;
                if (nsame > 0)
                {
                    //Console.WriteLine("get_prominence: nsame = " + nsame.ToString());
                    //foreach (Tuple xy in xyh)
                    //    donemap[xy.Item1,xy.Item2] = nround;
                    for (int isame = 0; isame < nsame; isame++)
                        donemap[xh[isame], yh[isame]] = nround;
                }

                if (newhigh < lowesthigh)
                {
                    lowesthigh = newhigh;
                    nroundlow = nround;
                }

                if (xhigh > maxx)
                {
                    maxx = xhigh;
                    if (maxx >= mapsize)
                        newhigh = badhigh;
                }
                if (xhigh < minx)
                {
                    minx = xhigh;
                    if (minx <= 0)
                        newhigh = badhigh;
                }
                if (yhigh > maxy)
                {
                    maxy = yhigh;
                    if (maxy >= mapsize)
                        newhigh = badhigh;
                }
                if (yhigh < miny)
                {
                    miny = yhigh;
                    if (miny <= 0)
                        newhigh = badhigh;
                }

                if (newhigh <= 0)
                    newhigh = badhigh;
            }

            double r2max = 0;
            int xr2max = 0;
            int yr2max = 0;
            int npix = 0;
            for (int i = minx; i <= maxx; i++)
                for (int j = miny; j <= maxy; j++)
                {
                    if ((donemap[i, j] > 0) && (donemap[i, j] < nroundlow))
                    {
                        npix++;
                        double r2 = scale * scale * (i - x0) * (i - x0) + (j - y0) * (j - y0);
                        if (r2 > r2max)
                        {
                            r2max = r2;
                            xr2max = i;
                            yr2max = j;
                        }
                    }
                }

            Console.WriteLine("get_promince: npix = " + npix.ToString());

            if (npix <= 1)
                return -1;

            if (newhigh == badhigh)
                return -1;

            r2max = 0;
            int xw = 0;
            int yw = 0;

            for (int i = minx; i <= maxx; i++)
                for (int j = miny; j <= maxy; j++)
                {
                    if ((donemap[i, j] > 0) && (donemap[i, j] < nroundlow))
                    {

                        double r2 = scale * scale * (i - xr2max) * (i - xr2max) + (j - yr2max) * (j - yr2max);
                        if (r2 > r2max)
                        {
                            r2max = r2;
                            xw = i;
                            yw = j;
                        }
                    }
                }

            width = Math.Sqrt(r2max) * pixkmy;

            Console.WriteLine("get_promince: nroundfinal = " + nround.ToString());

            if (lowesthigh < gndict[gnid].elevation)
                return gndict[gnid].elevation - lowesthigh;
            else
                return -1;
        }

        public static string get_terrain_type3(int gnid, double radius)
        {
            try
            {
                string terrain_type = get_terrain_type_latlong(ref gndict[gnid].elevation, gndict[gnid].latitude, gndict[gnid].longitude, radius);
                if (statisticsonly)
                {
                    Console.WriteLine(gndict[gnid].Name + ": " + terrain_type);
                    string[] tp = terrain_type.Split('|');
                    foreach (string ttp in tp)
                        terrainhist.Add(ttp);
                }
                return terrain_type;
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return "";
            }

        }

        public static string get_terrain_type_latlong(ref int elevation, double lat, double lon, double radius)
        {

            string terrain_type = "unknown";

            Console.WriteLine("get_terrain_type3");

            //int n = 0;
            int nelev = 0;
            int ndry = 0;
            int nocean = 0;
            int ncentral = 0;
            double elevationsum = 0.0;
            double elevationvar = 0.0;
            double elevationsquare = 0.0;
            double elevationmean = 0.0;
            double centralsum = 0.0;
            double centralvar = 0.0;
            double centralsquare = 0.0;
            double centralmean = 0.0;
            double r2ocean = 9999.9;
            int oceanmindir = -1;

            int[,] map = get_3x3map(lat, lon);
            int mapsize = map.GetLength(0);

            int x0 = get_x_pixel(lon, lon);
            int y0 = get_y_pixel(lat, lat);
            Console.WriteLine(lat.ToString() + " " + lon.ToString() + " " + x0.ToString() + " " + y0.ToString());

            if (elevation <= 0)
                elevation = map[x0, y0];
            else if (statisticsonly)
                elevdiffhist.Add(1.0 * (elevation - map[x0, y0]));


            double scale = Math.Cos(lat * 3.1416 / 180);
            double pixkmx = scale * 40000 / (360 * 1200);
            double pixkmy = 40000.0 / (360.0 * 1200.0);


            double[] elevdirsum = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] elevdirmean = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] elevdirsquare = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] elevdirvar = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };

            int[] nelevdir = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] noceandir = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] ndrydir = { 0, 0, 0, 0, 0, 0, 0, 0 };
            double slope1sum = 0;
            double slope5sum = 0;
            double slope1mean = 0;
            double slope5mean = 0;
            int nslope = 0;

            int r = Convert.ToInt32(radius / pixkmx);
            double r2max = radius / pixkmy * radius / pixkmy;
            double r2central = r2max / 16; //central part is one quarter the radius

            for (int x = x0 - r; x < x0 + r; x++)
                if ((x > 0) && (x < mapsize - 1))
                    for (int y = y0 - r; y < y0 + r; y++)
                        if ((y > 0) && (y < mapsize - 1))
                        {
                            double r2 = scale * scale * (x - x0) * (x - x0) + (y - y0) * (y - y0);
                            if (r2 < r2max)
                            {
                                int weight = 1;
                                if (4 * r2 < r2max)
                                    weight = 4;
                                else if (3 * r2 < r2max)
                                    weight = 3;
                                else if (2 * r2 < r2max)
                                    weight = 2;
                                int dir = util.get_pix_direction(x, y, x0, y0, scale);
                                if (map[x, y] != 0) //not ocean
                                {
                                    if (map[x, y] != 32768) //bad pixel
                                    {
                                        nelev++;
                                        ndry += weight;
                                        elevationsum += map[x, y];
                                        elevationsquare += map[x, y] * map[x, y];
                                        if (dir > 0)
                                            ndrydir[dir - 1] += weight;
                                        if (r2 < r2central)
                                        {
                                            centralsum += map[x, y];
                                            centralsquare += map[x, y] * map[x, y];
                                            ncentral++;
                                        }
                                        else if (dir > 0)
                                        {
                                            nelevdir[dir - 1]++;
                                            elevdirsum[dir - 1] += map[x, y];
                                            elevdirsquare[dir - 1] += map[x, y] * map[x, y];
                                        }
                                        slope1sum += Math.Abs(0.001 * (map[x, y] - map[x, y - 1])) / pixkmy; //0.001 because vertical meters and horizontal km
                                        slope1sum += Math.Abs(0.001 * (map[x, y] - map[x - 1, y])) / pixkmx;
                                        if (y > 5)
                                            slope5sum += Math.Abs(0.0002 * (map[x, y] - map[x, y - 5])) / pixkmy; //0.0002 = 0.001/5 bec
                                        if (x > 5)
                                            slope5sum += Math.Abs(0.0002 * (map[x, y] - map[x - 5, y])) / pixkmx;
                                        nslope += 2;
                                    }
                                }
                                else
                                {
                                    nocean += weight;
                                    if (dir > 0)
                                    {
                                        noceandir[dir - 1] += weight;
                                    }
                                    if (r2 < r2ocean)
                                    {
                                        r2ocean = r2;
                                        oceanmindir = dir;
                                    }
                                }
                            }
                        }


            if (nelev > 10)
            {
                elevationmean = elevationsum / nelev;
                elevationvar = elevationsquare / nelev - elevationmean * elevationmean;

                slope1mean = slope1sum / nslope;
                slope5mean = slope5sum / nslope;

                if (statisticsonly)
                {
                    double sloperms = 10000 * slope1mean / (Math.Sqrt(elevationvar) + 20);
                    Console.WriteLine(sloperms.ToString());
                    slopermshist.Add(sloperms);
                }

                Console.WriteLine("elevation mean, var = " + elevationmean.ToString() + ", " + elevationvar.ToString());
                Console.WriteLine("slope mean1,mean5 = " + slope1mean.ToString() + ", " + slope5mean.ToString());

                if (statisticsonly)
                {
                    evarhist.Add(elevationvar);
                    if (elevation > 0)
                    {
                        evarhist.Add(1.0 * (elevation - map[x0, y0]));
                    }
                    slope1hist.Add(100.0 * slope1mean);
                    slope5hist.Add(100.0 * slope5mean);
                }

                terrain_type = classify_terrain(elevationvar, elevationmean);


                //      1
                //   7    5
                //  3      4
                //   8    6
                //      2

                int ndir = 0;
                string[] terrtype_sector = new string[9];

                if (ncentral > 10)
                {
                    centralmean = centralsum / ncentral;
                    centralvar = centralsquare / ncentral - centralmean * centralmean;
                    Console.WriteLine("Central elevation mean, var = " + centralmean.ToString() + ", " + centralvar.ToString());
                    terrtype_sector[8] = classify_terrain(centralvar, centralmean);

                    terrain_type += "|central " + terrtype_sector[8];
                }


                if (nelev > 20)
                {
                    //Dictionary<string, int> terrtype = new Dictionary<string, int>();
                    for (int i = 0; i < 8; i++)
                    {
                        terrtype_sector[i] = "";
                        if (nelevdir[i] > 10)
                        {
                            elevdirmean[i] = elevdirsum[i] / nelevdir[i];
                            //elevationvar = elevationsquare / nelev - elevationmean * elevationmean;
                            elevdirvar[i] = elevdirsquare[i] / nelevdir[i] - elevdirmean[i] * elevdirmean[i];
                            terrtype_sector[i] = classify_terrain(elevdirvar[i], elevdirmean[i]);
                            //if (!terrtype.ContainsKey(terrtype_sector[i]))
                            //    terrtype.Add(terrtype_sector[i], 0);
                            //terrtype[terrtype_sector[i]]++;
                            terrain_type += "|dir" + (i + 1).ToString() + " " + terrtype_sector[i];
                            ndir++;
                        }
                        else
                            elevdirmean[i] = -99999.0;
                    }
                    //if (!terrtype.ContainsKey(terrtype_sector[8]))
                    //    terrtype.Add(terrtype_sector[8], 0);
                    //terrtype[terrtype_sector[8]]++;
                    //Console.WriteLine("Types in sectors: " + terrtype.Count.ToString());
                    //if (statisticsonly)
                    //    nsameterrhist.Add(terrtype.Count.ToString());
                }



                //int[] util.getdircoord(int dir)

                if (statisticsonly)
                    ndirhist.Add(ndir);


            }

            int nwet = 0;
            int nbitwet = 0;
            if (nocean > 10)
            {
                int iwet = -1;
                for (int i = 0; i < 8; i++)
                {
                    if (noceandir[i] > ndrydir[i])
                    {
                        nwet++;
                        iwet = i;
                    }
                    else if (2 * noceandir[i] > ndrydir[i])
                    {
                        nbitwet++;
                    }

                }
                if (nwet > 0) //at least one sector has mostly ocean
                {
                    terrain_type += "|ocean ";
                    if ((nwet == 1) && (nbitwet == 0))
                    {
                        terrain_type += "bay " + (iwet + 1).ToString();
                    }
                    else
                    {
                        int xsum = 0;
                        int ysum = 0;
                        for (int i = 0; i < 8; i++)
                        {
                            if (noceandir[i] > ndrydir[i])
                            {
                                int[] cdir = util.getdircoord(i + 1);
                                xsum += cdir[0];
                                ysum += cdir[1];
                            }
                        }

                        terrain_type += "coast" + get_NSEW_from_xysum(xsum, ysum);

                    }
                }
            }

            if (nwet == 0)
            {
                double triggerdiff = Math.Sqrt(elevationvar); //enough height difference to call it a terrain feature; more needed in rugged terrain
                if (triggerdiff < 20)
                    triggerdiff = 20;
                Console.WriteLine("triggerdiff = " + triggerdiff.ToString());
                if (elevation > 0)
                {
                    if (elevationmean - elevation > triggerdiff)
                        terrain_type += "|valley ";
                    else if (elevationmean - elevation < -triggerdiff)
                        terrain_type += "|peak ";
                }

                double xsum = 0; //measures east-west slope
                double ysum = 0; //measure north-south slope
                double x0sum = 0; //centerline NS altitude
                double y0sum = 0; //centerline EW altitude
                double x2sum = 0; //periphery NS altitude
                double y2sum = 0; //periphery EW altitude
                double xysum = 0;

                for (int i = 0; i < 8; i++)
                {
                    int[] cdir = util.getdircoord(i + 1);
                    xsum += cdir[0] * (elevdirmean[i] - elevationmean) / 6;
                    ysum += cdir[1] * (elevdirmean[i] - elevationmean) / 6;
                    x0sum += (1 - cdir[0] * cdir[0]) * (elevdirmean[i] - elevationmean) / 2;
                    y0sum += (1 - cdir[1] * cdir[1]) * (elevdirmean[i] - elevationmean) / 2;
                    x2sum += cdir[0] * cdir[0] * (elevdirmean[i] - elevationmean) / 6;
                    y2sum += cdir[1] * cdir[1] * (elevdirmean[i] - elevationmean) / 6;
                    xysum += cdir[0] * cdir[1] * (elevdirmean[i] - elevationmean) / 4;
                }

                //xsum -= elevationmean;
                //ysum -= elevationmean;
                //x0sum -= elevationmean;
                //y0sum -= elevationmean;
                //x2sum -= elevationmean;
                //y2sum -= elevationmean;
                //xysum -= elevationmean;

                double nsridge = x0sum - x2sum; // centerline > periphery
                double ewridge = y0sum - y2sum; // centerline > periphery

                if (terrain_type.Contains("valley"))
                {
                    Console.WriteLine("xsum = " + xsum.ToString());
                    Console.WriteLine("ysum = " + ysum.ToString());
                    Console.WriteLine("x0sum = " + x0sum.ToString());
                    Console.WriteLine("y0sum = " + y0sum.ToString());
                    Console.WriteLine("x2sum = " + x2sum.ToString());
                    Console.WriteLine("y2sum = " + y2sum.ToString());
                    Console.WriteLine("xysum = " + xysum.ToString());
                    if ((nsridge < -triggerdiff) && (ewridge > -triggerdiff / 2))
                        terrain_type += "NS.."; //North-south valley
                    else if ((ewridge < -triggerdiff) && (nsridge > -triggerdiff / 2))
                        terrain_type += "EW..";
                    else if (xysum > triggerdiff)
                        terrain_type += "SWNE";
                    else if (xysum < -triggerdiff)
                        terrain_type += "SENW";
                    //Console.WriteLine(terrain_type);
                    //Console.ReadLine();
                }
                else if (terrain_type.Contains("peak"))
                {
                    if ((nsridge > triggerdiff) && (ewridge < triggerdiff / 2))
                        terrain_type += "NS.."; //North-south ridge
                    else if ((ewridge > triggerdiff) && (nsridge < triggerdiff / 2))
                        terrain_type += "EW..";
                    else if (xysum > triggerdiff)
                        terrain_type += "SENW";
                    else if (xysum < -triggerdiff)
                        terrain_type += "SWNE";
                }
                else if (Math.Abs(elevationmean - elevation) < triggerdiff / 2)
                {
                    if (Math.Abs(xsum) > 3 * Math.Abs(ysum) + triggerdiff / 5)
                    {
                        if (xsum > 0)
                            terrain_type += "|slope E"; //upwards to the East
                        else
                            terrain_type += "|slope W";
                        if (Math.Abs(xsum) > triggerdiff)
                            terrain_type += " steep";
                    }
                    else if (Math.Abs(ysum) > 3 * Math.Abs(xsum) + triggerdiff / 5)
                    {
                        if (ysum > 0)
                            terrain_type += "|slope N";
                        else
                            terrain_type += "|slope S";
                        if (Math.Abs(ysum) > triggerdiff + 100)
                            terrain_type += " steep";
                    }
                }
            }

            return terrain_type;
        }

        public static string get_terrain_type(int gnid, double radius)
        {
            //List<int> farlist = getneighbors(gnid, 20.0);
            //return get_terrain_type2(farlist,gnid);
            return get_terrain_type3(gnid, radius);
        }

        public static string get_terrain_type_island(int gnid)
        {
            string terrain_type = "unknown";

            Console.WriteLine("get_terrain_type_island");

            //int n = 0;
            int nelev = 0;
            //int nocean = 0;
            double elevationsum = 0.0;
            double elevationvar = 0.0;
            double elevationsquare = 0.0;
            double elevationmean = 0.0;
            double elevationmax = 0.0;
            //double r2ocean = 9999.9;
            //int oceanmindir = -1;

            int[,] map = get_3x3map(gndict[gnid].latitude, gndict[gnid].longitude);
            int mapsize = map.GetLength(0);

            int x0 = get_x_pixel(gndict[gnid].longitude, gndict[gnid].longitude);
            int y0 = get_y_pixel(gndict[gnid].latitude, gndict[gnid].latitude);

            if (gndict[gnid].elevation <= 0)
                gndict[gnid].elevation = map[x0, y0];
            else if (statisticsonly)
                elevdiffhist.Add(1.0 * (gndict[gnid].elevation - map[x0, y0]));

            byte[,] fillmap = new byte[mapsize, mapsize];


            for (int x = 0; x < mapsize; x++)
                for (int y = 0; y < mapsize; y++)
                    fillmap[x, y] = 1;

            floodfill(ref fillmap, ref map, x0, y0, 0, 0, false);

            if (fillmap[0, 0] == 3) //fill failure
                return terrain_type;

            double scale = Math.Cos(gndict[gnid].latitude * 3.1416 / 180);
            //double pixkmx = scale * 40000 / (360 * 1200);
            //double pixkmy = 40000.0 / (360.0 * 1200.0);


            double[] elevdirsum = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] elevdirmean = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            int[] nelevdir = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] noceandir = { 0, 0, 0, 0, 0, 0, 0, 0 };

            //int r = Convert.ToInt32(radius / pixkmx);
            //double r2max = radius / pixkmy * radius / pixkmy;

            for (int x = 0; x < mapsize; x++)
                for (int y = 0; y < mapsize; y++)
                {
                    if (fillmap[x, y] == 2)
                    {
                        if (map[x, y] != 32768) //bad pixel
                        {
                            int dir = util.get_pix_direction(x, y, x0, y0, scale);
                            nelev++;
                            elevationsum += map[x, y];
                            elevationsquare += map[x, y] * map[x, y];
                            if (map[x, y] > elevationmax)
                                elevationmax = map[x, y];
                            if (dir > 0)
                            {
                                nelevdir[dir - 1]++;
                                elevdirsum[dir - 1] += map[x, y];
                            }
                        }
                    }
                }


            if (nelev > 10)
            {
                elevationmean = elevationsum / nelev;
                elevationvar = elevationsquare / nelev - elevationmean * elevationmean;


                Console.WriteLine("elevation mean, var = " + elevationmean.ToString() + ", " + elevationvar.ToString());

                if (statisticsonly)
                {
                    evarhist.Add(elevationvar);
                    if (gndict[gnid].elevation > 0)
                    {
                        evarhist.Add(1.0 * (gndict[gnid].elevation - map[x0, y0]));
                    }
                }

                //Inflate variance for very small areas, to make terrain more intuitive on islets
                if (nelev < 50)
                    elevationvar *= 16;
                else if (nelev < 600)
                {
                    double inflator = 800 / Convert.ToDouble(nelev);
                    elevationvar *= inflator;
                }

                terrain_type = classify_terrain(elevationvar, elevationmean);

                //      1
                //   7    5
                //  3      4
                //   8    6
                //      2

                int ndir = 0;
                if (nelev > 20)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (nelevdir[i] > 0)
                        {
                            elevdirmean[i] = elevdirsum[i] / nelevdir[i];
                            ndir++;
                        }
                        else
                            elevdirmean[i] = -99999.0;
                    }
                }


                //int[] getdircoord(int dir)

                if (statisticsonly)
                    ndirhist.Add(ndir);


            }

            if (elevationmax > 0)
                terrain_type += "|elevation_max " + elevationmax.ToString();


            if (statisticsonly)
            {
                Console.WriteLine(gndict[gnid].Name + ": " + terrain_type);
                terrainhist.Add(terrain_type);
            }
            return terrain_type;
        }




        public static void get_lang_iw(string langcode)
        {
            Console.WriteLine("get lang iw " + langcode);

            using (StreamWriter sw = new StreamWriter("langnames-" + langcode + ".csv"))
            {

                foreach (int gnid in langdict.Keys)
                {

                    string langname = langdict[gnid].name["en"];
                    string[] names = langname.Split(';');
                    foreach (string ln in names)
                    {
                        Page ep = new Page(ensite, ln);
                        util.tryload(ep, 2);
                        if (!ep.Exists())
                            continue;
                        if (ep.IsRedirect())
                        {
                            ep.title = ep.RedirectsTo();
                            util.tryload(ep, 2);
                            if (!ep.Exists())
                                continue;
                        }
                        langname = ln;

                        List<string> iwlist = ep.GetInterLanguageLinks();
                        foreach (string iws in iwlist)
                        {
                            string[] ss = iws.Split(':');
                            string iwcode = ss[0];
                            string iwtitle = ss[1];
                            //Console.WriteLine("iw - " + iwcode + ":" + iwtitle);
                            if (iwcode == langcode)
                                langname = iwtitle;
                        }
                        sw.WriteLine(langdict[gnid].iso3 + ";" + langname);
                        Console.WriteLine(ln + ";" + langname);
                    }

                }
            }
        }

        public static List<string> Interwiki(Site site, string title)

        //Borrowed from http://sv.wikipedia.org/wiki/Wikipedia:Projekt_DotNetWikiBot_Framework/Innocent_bot/Addbotkopia
        {
            List<string> r = new List<string>();
            XmlDocument doc = new XmlDocument();

            string url = "action=wbgetentities&sites=enwiki&titles=" + WebUtility.UrlEncode(title) + "&languages=en&format=xml";
            //string tmpStr = site.PostDataAndGetResultHTM(site.site+"/w/api.php", url);
            try
            {
                //string tmpStr = site.PostDataAndGetResultHTM(site.site + "/w/api.php", url);
                string tmpStr = site.PostDataAndGetResult(site.address + "/w/api.php", url);
                doc.LoadXml(tmpStr);
                for (int i = 0; i < doc.GetElementsByTagName("sitelink").Count; i++)
                {
                    string s = doc.GetElementsByTagName("sitelink")[i].Attributes.GetNamedItem("site").Value;
                    string t = doc.GetElementsByTagName("sitelink")[i].Attributes.GetNamedItem("title").Value;
                    s = s.Replace("_", "-");
                    string t2 = s.Substring(0, s.Length - 4) + ":" + t;
                    //Console.WriteLine(t2);
                    r.Add(t2);
                }
            }
            catch (WebException e)
            {
                string message = e.Message;
                Console.Error.WriteLine(message);
            }

            return r;
        }


        public static string make_disambig(Disambigclass da, int gnid)
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

            if (String.IsNullOrEmpty(da.fork.admname[0]))
                da.country = false;
            if (String.IsNullOrEmpty(da.fork.admname[1]))
                da.adm1 = false;
            if (String.IsNullOrEmpty(da.fork.admname[2]))
                da.adm2 = false;

            string artname = "(";
            bool needscomma = false;
            if (da.fcode)
            {
                artname += util.removearticle(getfeaturelabel(da.fork.iso, da.fork.featurecode, gnid));
                if (da.adm1 || da.adm2 || da.country)
                    artname += " " + util.mp(75) + " ";
                else
                    needscomma = true;
            }
            if (da.country)
            {
                if (needscomma)
                    artname += ", ";
                artname += da.fork.admname[0];
                needscomma = true;
            }
            if (da.adm1)
            {
                if (needscomma)
                    artname += ", ";
                artname += da.fork.admname[1];
                needscomma = true;
            }
            if (da.adm2)
            {
                if (needscomma)
                    artname += ", ";
                artname += da.fork.admname[2];
                needscomma = true;
            }
            if (da.latlong)
            {
                if (needscomma)
                    artname += ", ";
                artname += "lat " + da.fork.latitude.ToString("F2") + ", long " + da.fork.longitude.ToString("F2");
                needscomma = true;
            }

            artname += ")";

            return artname;
        }

        public static string saveconflict(string thisname, string othername)
        {
            if (thisname == othername)
                return "";

            string rs = "";
            string[] p9 = new string[2] { othername, getmonthstring() };
            rs = util.mp(9, p9) + "\n";
            if (pconflict == null)
            {
                pconflict = new Page(makesite, util.mp(13) + botname + "/Namnkonflikter-PRIVAT");
            }
            util.tryload(pconflict, 1);

            string ptt = "*[[" + thisname + "]] [[" + othername + "]]\n";
            if (!pconflict.text.Contains(ptt))
            {
                if (!conflictheadline)
                {
                    pconflict.text += "\n\n== " + countryml[makecountryname] + " ==\n";
                    conflictheadline = true;
                }

                pconflict.text += "\n" + ptt;

                util.trysave(pconflict, 1, util.mp(304, null));
            }
            return rs;
        }

        public static string saveanomaly(string thisname, string reason)
        {
            string rs = "";
            string[] p196 = new string[1] { reason };
            rs = util.mp(196, p196) + "\n";
            if (panomaly == null)
            {
                panomaly = new Page(makesite, util.mp(13) + botname + "/Anomalier-PRIVAT");
                util.tryload(panomaly, 1);
            }

            string ptt = "*[[" + thisname + "]] " + reason;
            if (!panomaly.text.Contains(ptt))
            {
                if (!anomalyheadline)
                {
                    panomaly.text += "\n\n== " + countryml[makecountryname] + " ==\n";
                    anomalyheadline = true;
                }
                panomaly.text += "\n" + ptt;
                util.trysave(panomaly, 1, util.mp(304, null));
            }
            return rs;
        }

        public static void find_duplicate_forks()
        {
            Dictionary<string, List<string>> forkdict = new Dictionary<string, List<string>>();

            using (StreamReader sw = new StreamReader(geonameclass.geonamesfolder + "namefork-" + makelang + ".csv"))
            {
                while (!sw.EndOfStream)
                {
                    string s = sw.ReadLine();
                    s = s.Trim(';');

                    List<forkclass> fl = new List<forkclass>();

                    string countryname = "";

                    string gnidstring = "";
                    int nreal = 0;

                    while (true)
                    {
                        string line = sw.ReadLine();
                        if (line[0] == '#')
                            break;
                        string[] words = line.Split(';');

                        //public class forkclass
                        //{
                        //    public int geonameid = 0;
                        //    public string featurecode = "";
                        //    public string[] admname = new string[3];
                        //    public double latitude = 0.0;
                        //    public double longitude = 0.0;

                        //}

                        forkclass fc = new forkclass();
                        fc.geonameid = util.tryconvert(words[0]);
                        gnidstring += " " + fc.geonameid;
                        fc.featurecode = words[1];
                        fc.admname[0] = words[2];
                        fc.admname[1] = words[3];
                        fc.admname[2] = words[4];
                        fc.latitude = util.tryconvertdouble(words[5]);
                        fc.longitude = util.tryconvertdouble(words[6]);
                        fc.realname = words[7];

                        if (fc.realname == "*")
                        {
                            fc.realname = s;
                            nreal++;
                        }
                        //if (artnamedict.ContainsKey(fc.geonameid))
                        //    fc.realname = artnamedict[fc.geonameid];
                        fl.Add(fc);
                        countryname = words[2];
                    }

                    if (fl.Count < 2)
                        continue;

                    gnidstring = gnidstring.Trim();

                    if (forkdict.ContainsKey(gnidstring))
                    {
                        Console.WriteLine(s + ";" + nreal.ToString());
                        forkdict[gnidstring].Add(s + ";" + nreal.ToString());
                    }
                    else
                    {
                        List<string> ll = new List<string>();
                        ll.Add(s + ";" + nreal.ToString());
                        forkdict.Add(gnidstring, ll);
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter("namefork-duplicates-" + makelang + util.getdatestring() + ".txt"))
            {
                foreach (string gnidstring in forkdict.Keys)
                {
                    if (forkdict[gnidstring].Count > 1)
                    {
                        int nrmax = -1;
                        string srmax = "";
                        foreach (string ss in forkdict[gnidstring])
                        {
                            string[] sss = ss.Split(';');
                            int nreal = util.tryconvert(sss[1]);
                            if (nreal > nrmax)
                            {
                                nrmax = nreal;
                                srmax = sss[0];
                            }
                        }
                        if (nrmax > 0)
                        {
                            sw.Write(srmax);
                            foreach (string ss in forkdict[gnidstring])
                            {
                                string[] sss = ss.Split(';');
                                if (sss[0] != srmax)
                                    sw.Write(";" + sss[0]);
                            }
                            sw.WriteLine();

                        }
                    }
                }
            }

        }

        public static void makeforkpages()
        {
            int nfork = 0;
            List<string> forkdoubles = new List<string>();
            int nu2019 = 0;

            makesite.defaultEditComment = util.mp(69);
            if (!String.IsNullOrEmpty(makecountry))
                makesite.defaultEditComment += " " + countryml[makecountryname];

            if (pstats == null)
            {
                pstats = new Page(makesite, util.mp(13) + botname + "/Statistik");
                pstats.Load();
            }
            pstats.text += "\n\n== [[" + countryml[makecountryname] + "]] grensidor ==\n\n";
            util.trysave(pstats, 1, util.mp(302) + " " + countryml[makecountryname]);

            Dictionary<string, string> forkduplicatedict = new Dictionary<string, string>();
            using (StreamReader sw = new StreamReader(geonameclass.geonamesfolder + "namefork-duplicates-" + makelang + ".txt"))
            {
                while (!sw.EndOfStream)
                {
                    string s = sw.ReadLine();
                    string[] ss = s.Split(';');
                    if (ss.Length < 2)
                        continue;
                    for (int i = 1; i < ss.Length; i++)
                    {
                        if (!forkduplicatedict.ContainsKey(ss[i]))
                            forkduplicatedict.Add(ss[i], ss[0]); //dictionary from duplicate name to proper name
                    }
                }
            }

            using (StreamReader sw = new StreamReader(geonameclass.geonamesfolder + "namefork-" + makelang + ".csv"))
            {
                while (!sw.EndOfStream)
                {
                    string s = sw.ReadLine();
                    s = s.Trim(';');

                    int nbranches = 0;

                    List<forkclass> fl = new List<forkclass>();

                    string countryname = "";

                    int nsartname = 0;
                    while (true)
                    {
                        string line = sw.ReadLine();

                        if (line[0] == '#')
                            break; //normal exit from the loop

                        string[] words = line.Split(';');

                        //public class forkclass
                        //{
                        //    public int geonameid = 0;
                        //    public string featurecode = "";
                        //    public string[] admname = new string[3];
                        //    public double latitude = 0.0;
                        //    public double longitude = 0.0;

                        //}

                        forkclass fc = new forkclass();
                        fc.geonameid = util.tryconvert(words[0]);
                        fc.featurecode = words[1];
                        if (!featuredict.ContainsKey(fc.featurecode))
                            continue;

                        fc.admname[0] = words[2];
                        fc.admname[1] = words[3];
                        fc.admname[2] = words[4];
                        fc.latitude = util.tryconvertdouble(words[5]);
                        fc.longitude = util.tryconvertdouble(words[6]);
                        fc.realname = words[7];
                        if (fc.realname == "*")
                            fc.realname = s;
                        //if (artnamedict.ContainsKey(fc.geonameid))
                        //    fc.realname = artnamedict[fc.geonameid];
                        fl.Add(fc);
                        countryname = words[2];
                        if (artnamedict.ContainsKey(fc.geonameid))
                            if (s == artnamedict[fc.geonameid].Replace("*", ""))
                                nsartname++;
                    }

                    //bool allsamecountry = true;
                    bool hasmakecountry = false;
                    bool hasanomaly = false;
                    string anomalytext = "";

                    //Console.WriteLine("# names in fork page = " + fl.Count.ToString());

                    if (fl.Count < 2)
                        continue;

                    foreach (forkclass ff in fl)
                    {
                        //if (ff.admname[0] != countryname)
                        //    allsamecountry = false;
                        if (ff.admname[0] == makecountryname)
                            hasmakecountry = true;
                    }

                    if (!hasmakecountry)
                    {
                        Console.WriteLine("No place in makecountry");
                        continue;
                    }

                    if (!String.IsNullOrEmpty(resume_at_fork))
                        if (s != resume_at_fork)
                        {
                            stats.Addskip();
                            continue;
                        }
                        else
                            resume_at_fork = "";


                    Console.WriteLine("nsartname = " + nsartname.ToString());
                    if (nsartname >= 2)
                    {
                        Console.WriteLine(s + " Too many places link to same!");
                        //Console.ReadLine();
                    }

                    bool alreadyfork = false;
                    bool willoverwrite = false;

                    string alreadyforktitle = "";

                    string forkpagename = testprefix + s;
                    Page forkpage = new Page(makesite, forkpagename);
                    if (util.tryload(forkpage, 2))
                    {
                        if (forkpage.Exists() || (nsartname > 0))
                        {
                            if (!forkpage.Exists())
                                forkpage.text = "";

                            if (forkpage.text.Contains(util.mp(69))) //botmade fork; don't make again unless overwrite is set
                            {
                                if (!overwrite || human_touched(forkpage, makesite))
                                    continue;
                                else
                                {
                                    alreadyfork = false;
                                    willoverwrite = true;
                                }
                            }
                            else
                            {
                                if (util.is_fork(forkpage))
                                {
                                    alreadyfork = true;
                                    alreadyforktitle = forkpage.title;
                                }



                                Page fp2 = new Page(makesite, forkpage.title + " (" + util.mp(67) + ")");

                                if (util.tryload(fp2, 1))
                                {
                                    if (fp2.Exists())
                                    {
                                        alreadyfork = true;
                                        alreadyforktitle = forkpage.title;

                                        if (fp2.text.Contains(util.mp(69))) //botmade fork; don't make again
                                        {
                                            if (!overwrite || human_touched(fp2, makesite))
                                                continue;
                                            else
                                            {
                                                alreadyfork = false;
                                                forkpage = fp2;
                                                forkpage.text = "";
                                                willoverwrite = true;
                                            }
                                        }
                                        else
                                        {
                                            Page fp3 = new Page(makesite, fp2.title.Replace(")", " 2)"));

                                            if (util.tryload(fp3, 1))
                                            {
                                                if (fp3.Exists())
                                                    continue;
                                                else
                                                {
                                                    forkpage = fp3;
                                                    forkpage.text = "";
                                                }

                                            }
                                            else
                                            {
                                                forkpage = fp3;
                                                forkpage.text = "";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        forkpage = fp2;
                                        forkpage.text = "";
                                    }
                                }
                                else
                                {
                                    forkpage = fp2;
                                    forkpage.text = "";
                                }
                            }
                        }
                    }
                    Console.WriteLine("forkpage.title = " + forkpage.title);

                    if (forkduplicatedict.ContainsKey(s))
                    {
                        if (forkpage.title == util.remove_disambig(forkpage.title))
                            make_redirect_override(forkpage, forkduplicatedict[s], "", -1);
                        Console.WriteLine("duplicate fork " + forkpage.title + " - " + forkduplicatedict[s]);
                        //Console.ReadLine();
                        continue;
                    }

                    string origtext = forkpage.text;
                    forkpage.text = "";
                    if (alreadyfork)
                    {
                        forkpage.text += saveconflict(forkpage.title, alreadyforktitle);
                    }

                    forkpage.text += util.mp(120) + "\n\n";


                    string[] p68 = new string[1] { s };
                    forkpage.text += util.mp(68, p68) + ":\n";

                    forkpage.text += "== " + util.mp(307) + " ==\n";
                    if (makelang == "sv")
                    {
                        forkpage.text = util.mp(142) + "\n" + forkpage.text;
                        forkpage.text += "\n" + util.comment("NOTERA: Om platser läggs till, tas bort eller ordningen på platserna ändras, bör också mallen Kartposition under samma rubrik korrigeras för att kartan ska förbli rättvisande.") + "\n";
                    }

                    string[] p71 = new string[1] { countryname };

                    //if (allsamecountry)
                    //{
                    //    if (fl.Count > 2)
                    //        forkpage.text += util.mp(71, p71);
                    //    else
                    //        forkpage.text += util.mp(78, p71);
                    //}

                    forkpage.text += "\n\n";

                    //if (allsamecountry)
                    //{
                    //    string countrynameml = countryname;
                    //    if (countryml.ContainsKey(countryname))
                    //        countrynameml = countryml[countryname];
                    //    string[] p73 = new string[2] { countrynameml, s };
                    //    if (locatordict.ContainsKey(countryname))
                    //    {
                    //        forkpage.text += util.mp(72) + "+|" + locatordict[countryname] + "\n |caption = " + util.mp(73, p73) + "\n  |float = right\n  |width=300\n  | places =";
                    //        int inum = 0;
                    //        foreach (forkclass ff in fl)
                    //        {
                    //            inum++;
                    //            forkpage.text += util.mp(72) + "~|" + locatordict[countryname] + "| label = " + inum.ToString() + "| mark =Blue_pog.svg|position=right|background=white|lat=" + ff.latitude.ToString(culture_en) + "|long=" + ff.longitude.ToString(culture_en) + "|caption=|float=}}\n";
                    //        }
                    //        forkpage.text += "}}\n";
                    //    }
                    //    foreach (forkclass ff in fl)
                    //    {
                    //        string artname = s;
                    //        if (artnamedict.ContainsKey(ff.geonameid))
                    //        {
                    //            if (artnamedict[ff.geonameid] != "X")
                    //                artname = artnamedict[ff.geonameid];
                    //        }
                    //        string ss = "# [[" + artname + "]], ";
                    //        if (!artname.Contains("(" + featuredict[ff.featurecode]))
                    //            ss += featuredict[ff.featurecode];
                    //        if (!String.IsNullOrEmpty(ff.admname[1]) && !artname.Contains(ff.admname[1]))
                    //            ss += ", " + ff.admname[1];
                    //        if (!String.IsNullOrEmpty(ff.admname[2]) && !artname.Contains(ff.admname[2]))
                    //            ss += ", " + ff.admname[2];
                    //        if (!artname.Contains(" lat "))
                    //        {
                    //            ss += ", lat. " + ff.latitude.ToString("F1", culture);
                    //            ss += ", long. " + ff.longitude.ToString("F1", culture);
                    //        }
                    //        forkpage.text += ss + "\n";
                    //        nbranches++;
                    //    }
                    //}
                    //else
                    //{
                    Dictionary<string, List<forkclass>> fd = new Dictionary<string, List<forkclass>>();
                    SortedDictionary<string, string> scountry = new SortedDictionary<string, string>();


                    foreach (forkclass ff in fl)
                    {
                        string sortcountry = ff.admname[0];
                        if (countryml.ContainsKey(ff.admname[0]))
                            sortcountry = countryml[ff.admname[0]];
                        string locatorkey = ff.admname[0];
                        if ((String.IsNullOrEmpty(sortcountry)) || (countrydict.ContainsKey(ff.geonameid)))
                        {
                            sortcountry = util.mp(166);
                            locatorkey = "";
                        }
                        if (!scountry.ContainsKey(sortcountry))
                            scountry.Add(sortcountry, locatorkey);
                        if (!fd.ContainsKey(locatorkey))
                        {
                            List<forkclass> ffl = new List<forkclass>();
                            fd.Add(locatorkey, ffl);
                        }
                        fd[locatorkey].Add(ff);
                    }

                    int ncountries = 0;
                    int maxpercountry = 0;
                    int nplaces = 0;
                    foreach (string cs in fd.Keys)
                    {
                        ncountries++;
                        nplaces += fd[cs].Count;
                        if (fd[cs].Count > maxpercountry)
                            maxpercountry = fd[cs].Count;
                    }

                    bool worldmaponly = ((ncountries > 4) && (maxpercountry < 4));

                    if (makeworldmaponly & !worldmaponly)
                        continue;

                    int inum = 0;

                    if (worldmaponly)
                    {
                        int mapsize = 450;

                        string caption = "";
                        if (makelang == "sv")
                        {
                            string ifcollapsed = "";//" mw-collapsed";
                            string collapseintro = "{| class=\"mw-collapsible" + ifcollapsed + "\" data-expandtext=\"Visa karta\" data-collapsetext=\"Dölj karta\" style=\"float:right; clear:right;\"\n|-\n!\n|-\n|\n";
                            forkpage.text += collapseintro;
                        }
                        forkpage.text += util.mp(72) + "+|" + locatordict[""].locatorname + "\n |caption = " + caption + "\n  |float = right\n  |width=" + mapsize.ToString() + "\n  | places =";
                        inum = 0;
                        foreach (string csl in scountry.Keys)
                        {
                            string cs = scountry[csl];
                            foreach (forkclass ff in fd[cs])
                            {
                                inum++;
                                forkpage.text += util.mp(72) + "~|" + locatordict[""].locatorname + "| label = " + inum.ToString() + "| mark =Blue_pog.svg|position=right|background=white|lat=" + ff.latitude.ToString(culture_en) + "|long=" + ff.longitude.ToString(culture_en) + "}}\n";
                            }
                        }
                        forkpage.text += "}}\n";
                        if (makelang == "sv")
                            forkpage.text += "|}\n"; //collapse-end
                    }

                    inum = 0;

                    foreach (string csl in scountry.Keys)
                    {
                        forkpage.text += "=== " + csl + " ===\n";
                        string cs = scountry[csl];

                        string ciso = "";
                        string locname = csl;
                        if (countryiso.ContainsKey(cs))
                        {
                            ciso = countryiso[cs];
                            locname = countryclass.linkcountry(ciso);
                        }

                        if (locatordict.ContainsKey(cs) && !worldmaponly)
                        {
                            int mapsize = 300;
                            if (fd[cs].Count > 40)
                                mapsize = 600;
                            else if (fd[cs].Count > 8)
                                mapsize = 450;
                            else if (fd[cs].Count == 1)
                                mapsize = 150;

                            string[] p73 = new string[2] { locname, s };
                            string caption = util.mp(73, p73);
                            if (csl == util.mp(166))
                                caption = csl;
                            if (makelang == "sv")
                            {
                                string ifcollapsed = "";//" mw-collapsed";
                                string collapseintro = "{| class=\"mw-collapsible" + ifcollapsed + "\" data-expandtext=\"Visa karta\" data-collapsetext=\"Dölj karta\" style=\"float:right; clear:right;\"\n|-\n!\n|-\n|\n";
                                forkpage.text += collapseintro;
                            }
                            forkpage.text += util.mp(72) + "+|" + locatordict[cs].locatorname + "\n |caption = " + caption + "\n  |float = right\n  |width=" + mapsize.ToString() + "\n  | places =";
                            inum = 0;
                            foreach (forkclass ff in fd[cs])
                            {
                                inum++;
                                forkpage.text += util.mp(72) + "~|" + locatordict[cs].locatorname + "| label = " + inum.ToString() + "| mark =Blue_pog.svg|position=right|background=white|lat=" + ff.latitude.ToString(culture_en) + "|long=" + ff.longitude.ToString(culture_en) + "}}\n";
                            }
                            forkpage.text += "}}\n";
                            if (makelang == "sv")
                                forkpage.text += "|}\n"; //collapse-end
                        }

                        List<string> artnames = new List<string>();
                        foreach (forkclass ff in fd[cs])
                        {
                            string artname = s;
                            if (artnamedict.ContainsKey(ff.geonameid))
                            {
                                //if (artnamedict[ff.geonameid] != "X")
                                //    artname = artnamedict[ff.geonameid];
                                artname = artnamedict[ff.geonameid].Replace("*", "");
                            }
                            if (artnames.Contains(artname))
                            {
                                string existing = "";
                                if (artnamedict[ff.geonameid].Contains("*"))
                                    existing = "*";
                                if (!forkdoubles.Contains(existing + s))
                                {
                                    forkdoubles.Add(existing + s);
                                    hasanomaly = true;
                                    //forkpage.text = saveanomaly(forkpage.title, util.mp(201)) + forkpage.text;
                                    if (!String.IsNullOrEmpty(anomalytext))
                                        anomalytext += " ";
                                    anomalytext += util.mp(201) + " [[" + artname + "]]";
                                }

                            }
                            else
                                artnames.Add(artname);
                            foreach (forkclass ff2 in fd[cs])
                            {
                                if (ff2 != ff)
                                {
                                    if (util.get_distance_latlong(ff.latitude, ff.longitude, ff2.latitude, ff2.longitude) < 1.0)
                                    {
                                        Console.WriteLine("featurecodes potential anomaly: " + ff.featurecode + " " + ff2.featurecode);
                                        if (!(((ff.featurecode.Contains("PPL")) && (ff2.featurecode.Contains("ADM"))) || ((ff.featurecode.Contains("ADM")) && (ff2.featurecode.Contains("PPL")))))
                                        {
                                            //forkpage.text = saveanomaly(forkpage.title, util.mp(202)) + forkpage.text;
                                            hasanomaly = true;
                                            Console.WriteLine("Has anomaly");
                                            string artname2 = "";
                                            if (artnamedict.ContainsKey(ff.geonameid))
                                            {
                                                artname2 = artnamedict[ff2.geonameid].Replace("*", "");
                                            }

                                            if (!String.IsNullOrEmpty(anomalytext))
                                                anomalytext += " ";
                                            anomalytext += util.mp(202) + " [[" + artname + "]]" + " [[" + artname2 + "]]";
                                            //Console.ReadLine();
                                            break;
                                        }
                                        //Console.ReadLine();
                                    }
                                }
                                else
                                    break;
                            }
                            string fstart = "#";
                            if (worldmaponly)
                            {
                                inum++;
                                fstart = "*" + inum.ToString();
                            }
                            string ss = fstart + " [[" + artname.Replace("*", "") + "]], ";
                            if (!artname.Contains("(" + getfeaturelabel(ciso, ff.featurecode, ff.geonameid)))
                                ss += getfeaturelabel(ciso, ff.featurecode, ff.geonameid) + ", ";
                            if (!String.IsNullOrEmpty(ff.admname[1]) && !artname.Contains(ff.admname[1]))
                                ss += ff.admname[1] + ", ";
                            if (!String.IsNullOrEmpty(ff.admname[2]) && !artname.Contains(ff.admname[2]))
                                ss += ff.admname[2] + ", ";
                            //if (!artname.Contains(" lat "))
                            //{
                            //    ss += ", lat. " + ff.latitude.ToString("F1", culture);
                            //    ss += ", long. " + ff.longitude.ToString("F1", culture);
                            //}

                            //Console.WriteLine(make_coord_template(ciso, ff.featurecode, ff.latitude, ff.longitude));
                            //Console.ReadLine();

                            ss += util.make_coord_template(ciso, ff.featurecode, ff.latitude, ff.longitude, artname.Replace("*", ""));

                            ss += util.comment("Geonames ID " + ff.geonameid.ToString());

                            forkpage.text += ss + "\n";
                            nbranches++;
                        }
                    }
                    //}


                    forkpage.text += "\n{{" + util.mp(69) + "}}\n";
                    //forkpage.text += "[[" + util.mp(70) + "]]\n";

                    if ((makelang == "sv") && (!util.is_latin(forkpage.title)))
                    {
                        string alph_sv = util.get_alphabet_sv(util.get_alphabet(util.remove_disambig(forkpage.title)));
                        if (!alph_sv.Contains("okänd"))
                            forkpage.text += "{{Sidnamn annan skrift|" + alph_sv + "}}\n";
                        else
                        {
                            Console.WriteLine(forkpage.title);
                            Console.WriteLine(util.remove_disambig(forkpage.title));
                            Console.WriteLine(alph_sv);
                            //Console.ReadLine();
                        }

                    }

                    string[] p215 = new string[] { "", getmonthstring() };
                    forkpage.AddToCategory(util.mp(215, p215).Trim());
                    p215[1] = "";
                    foreach (string csl in scountry.Keys)
                    {
                        p215[0] = csl;
                        forkpage.AddToCategory(util.mp(215, p215).Trim());
                    }


                    if (nbranches > 1)
                    {
                        if (hasanomaly)
                            forkpage.text = saveanomaly(forkpage.title, anomalytext) + forkpage.text;

                        forkpage.text = util.cleanup_text(forkpage.text);

                        if (forkpage.text != origtext)
                        {
                            if (willoverwrite)
                                util.trysave(forkpage, 2, util.mp(303) + " " + makesite.defaultEditComment);
                            else
                                util.trysave(forkpage, 2);
                        }

                        nfork++;
                        //if ( s == "Andorra" )
                        //    Console.ReadLine();
                        Console.WriteLine("nfork = " + nfork.ToString());
                        romanian_redirect(forkpage.title);

                    }

                }


            }
            Console.WriteLine(stats.GetStat());
            if (pstats == null)
            {
                pstats = new Page(makesite, util.mp(13) + botname + "/Statistik");
                pstats.Load();
            }
            //pstats.text += "\n\n== [[" + countryml[makecountryname] + "]] grensidor ==\n\n";
            pstats.text += stats.GetStat();
            util.trysave(pstats, 1, util.mp(302) + " " + countryml[makecountryname]);
            stats.ClearStat();

            Console.WriteLine("nfork = " + nfork.ToString());
            foreach (string fd in forkdoubles)
                Console.WriteLine(fd);
            using (StreamWriter sw = new StreamWriter("forkdoubles.txt"))
            {
                foreach (string ul in forkdoubles)
                    sw.WriteLine(ul);
            }

            Console.WriteLine("forkdoubles = " + forkdoubles.Count.ToString());

            Console.WriteLine("nu2019 = " + nu2019.ToString());
        }

        public static void print_geonameid(int id)
        {
            if (gndict.ContainsKey(id))
            {
                Console.WriteLine("Name = " + gndict[id].Name);
                Console.WriteLine("Country = " + gndict[gndict[id].adm[0]].Name);
                Console.WriteLine("Province = " + gndict[gndict[id].adm[1]].Name);
            }
        }


        public static int get_direction_from_NSEW(string NSEW)
        {
            switch (NSEW.Trim())
            {
                case "N.":
                    return 1;
                case "S.":
                    return 2;
                case ".W":
                    return 3;
                case ".E":
                    return 4;
                case "NE":
                    return 5;
                case "SE":
                    return 6;
                case "NW":
                    return 7;
                case "SW":
                    return 8;
                default:
                    return -1;
            }
        }

        public static string get_NSEW_from_xysum(int xsum, int ysum)
        {
            string rs = "";
            if (xsum > 0)
            {
                if (ysum > 0)
                    rs = " NE";
                else if (ysum < 0)
                    rs = " SE";
                else
                    rs = " .E";
            }
            else if (xsum < 0)
            {
                if (ysum > 0)
                    rs = " NW";
                else if (ysum < 0)
                    rs = " SW";
                else
                    rs = " .W";
            }
            else if (ysum > 0)
                rs = " N.";
            else if (ysum < 0)
                rs = " S.";
            else
                rs = " C.";
            return rs;
        }

        public static string terrain_label(string terr)
        {
            string rt = "";
            if (terr.Contains("flat"))
            {
                if (terr.Contains("high"))
                    rt = util.mp(183);
                else if (terr.Contains("very "))
                    rt = util.mp(182);
                else
                    rt = util.mp(109);
            }
            else if (terr.Contains("hilly"))
            {
                if (terr.Contains("somewhat"))
                    rt = util.mp(184);
                else
                    rt = util.mp(111);
            }
            else if (terr.Contains("high-mountains"))
                rt = util.mp(112);
            else if (terr.Contains("low-mountains"))
                rt = util.mp(185);
            else if (terr.Contains("mountains"))
                rt = util.mp(110);
            else
                rt = util.mp(186);
            return rt;
        }

        public static bool is_height(string fcode)
        {
            if (fcode == "MTS")
                return false;
            else if (fcode == "HLLS")
                return false;
            else if (fcode == "NTKS")
                return false;
            else if (fcode == "PKS")
                return false;
            else if (categorydict[fcode] == "mountains")
                return true;
            else if (categorydict[fcode] == "hills")
                return true;
            else if (categorydict[fcode] == "volcanoes")
                return true;
            else
                return false;
        }

        public static int imp_mountainpart(string fcode)
        {
            switch (fcode)
            {
                case "SPUR":
                case "PROM":
                    return 197;
                case "BNCH":
                case "CLF":
                case "RKFL":
                case "SLID":
                case "TAL":
                case "CRQ":
                case "CRQS":
                    return 203;
                default:
                    return -1;
            }
        }

        public static bool human_touched(Page p, Site site) //determines if an article has been edited by a human user with account (not ip or bot)
        {
            string xmlSrc;
            bool ht = false;
            try
            {
                xmlSrc = site.PostDataAndGetResult(site.address + "/w/api.php", "action=query&format=xml&prop=revisions&titles=" + WebUtility.UrlEncode(p.title) + "&rvlimit=20&rvprop=user");
            }
            catch (WebException e)
            {
                string message = e.Message;
                Console.Error.WriteLine(message);
                return true;
            }

            XmlDocument xd = new XmlDocument();
            xd.LoadXml(xmlSrc);

            XmlNodeList elemlist = xd.GetElementsByTagName("rev");

            Console.WriteLine("elemlist.Count = " + elemlist.Count);
            //Console.WriteLine(xmlSrc);

            foreach (XmlNode ee in elemlist)
            {

                try
                {

                    string username = ee.Attributes.GetNamedItem("user").Value;
                    Console.WriteLine(username);
                    if (!username.ToLower().Contains("bot") && (util.get_alphabet(username) != "none"))
                    {
                        ht = true;
                        break;
                    }

                }
                catch (NullReferenceException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine(message);
                }
            }

            return ht;
        }


        public static string terrain_text(string terrain_type, int gnid)
        {
            if (terrain_type == "")
                return "";
            if (terrain_type == "unknown")
                return "";
            string rt = "";
            string[] p98 = { gndict[gnid].Name_ml };

            string[] words = terrain_type.Split('|');
            string main_terrain = words[0];
            Dictionary<string, int> terrsector = new Dictionary<string, int>();
            List<string> maintype = new List<string>();
            maintype.Add("flat");
            maintype.Add("hilly");
            maintype.Add("mountains");
            int nsector = 0;
            string centralterrain = "";
            string majorterrain = "";
            string minorterrain = "";
            foreach (string w in words)
            {
                if ((w.IndexOf("dir") == 0) || (w.IndexOf("central") == 0))
                {
                    foreach (string ttype in maintype)
                    {
                        if (w.Contains(ttype))
                        {
                            if (!terrsector.ContainsKey(ttype))
                                terrsector.Add(ttype, 0);
                            terrsector[ttype]++;
                            nsector++;
                            if (w.IndexOf("central") == 0)
                                centralterrain = ttype;
                        }
                    }
                }
            }

            bool allsame = false;
            bool varied = false;
            bool singlediff = false;
            int tmax = -1;
            string tmaxtype = "";
            string tmintype = "";
            //string dirterrain = "";
            if (terrsector.Count <= 1)
            {
                allsame = true;
            }
            else if (terrsector.Count == 2)
            {
                foreach (string ttype in terrsector.Keys)
                {
                    if (terrsector[ttype] > tmax)
                    {
                        tmax = terrsector[ttype];
                        tmaxtype = ttype;
                    }
                }
                if (2 * tmax >= nsector) //at least half are same type
                {
                    int xsum = 0;
                    int ysum = 0;
                    foreach (string ttype in terrsector.Keys)
                    {
                        if (ttype != tmaxtype)
                        {
                            tmintype = ttype;
                        }
                    }
                    if (nsector - tmax == 1) //a single sector different
                    {
                        singlediff = true;
                        foreach (string w in words)
                        {
                            if ((w.IndexOf("dir") == 0) && (w.Contains(tmintype)))
                            {
                                int i = util.tryconvert(w.Substring(3, 1));
                                int[] cdir = util.getdircoord(i);
                                xsum += cdir[0];
                                ysum += cdir[1];
                            }
                        }
                        minorterrain = tmintype + get_NSEW_from_xysum(xsum, ysum);
                        majorterrain = tmaxtype;
                    }
                    else //minority more than a single sector
                    {
                        xsum = 0;
                        ysum = 0;
                        foreach (string w in words)
                        {
                            if ((w.IndexOf("dir") == 0) && (w.Contains(tmaxtype)))
                            {
                                int i = util.tryconvert(w.Substring(3, 1));
                                int[] cdir = util.getdircoord(i);
                                xsum += cdir[0];
                                ysum += cdir[1];
                            }
                        }
                        string major_NSEW = get_NSEW_from_xysum(xsum, ysum);
                        int majordir = get_direction_from_NSEW(major_NSEW);
                        if (majordir <= 0)
                        {
                            varied = true;
                            main_terrain = "mixed";
                        }
                        else
                        {
                            bool rightmajor = false;
                            foreach (string w in words)
                            {
                                if (w.Contains("dir" + majordir.ToString()))
                                {
                                    if (w.Contains(tmaxtype))
                                        rightmajor = true;
                                }
                            }
                            if (!rightmajor)
                            {
                                varied = true;
                                main_terrain = "mixed";
                            }
                            else
                            {
                                majorterrain = tmaxtype + major_NSEW;
                                xsum = 0;
                                ysum = 0;
                                foreach (string w in words)
                                {
                                    if ((w.IndexOf("dir") == 0) && (w.Contains(tmintype)))
                                    {
                                        int i = util.tryconvert(w.Substring(3, 1));
                                        int[] cdir = util.getdircoord(i);
                                        xsum += cdir[0];
                                        ysum += cdir[1];
                                    }
                                }
                                minorterrain = tmintype + get_NSEW_from_xysum(xsum, ysum);
                            }
                        }
                    }
                }
                else
                {
                    varied = true;
                    main_terrain = "mixed";
                }
            }
            else
            {
                varied = true;
                main_terrain = "mixed";
            }

            Console.WriteLine("majorterrain=" + majorterrain);
            Console.WriteLine("minorterrain=" + minorterrain);

            //terrain header:

            bool peakvalley = true; //true if it should be written that something is on a peak or in a valley

            if (categorydict[gndict[gnid].featurecode] == "peninsulas")
            {
                rt = util.mp(170, p98) + " "; //terrain landwards from a peninsula
                peakvalley = false;
            }
            else if (categorydict[gndict[gnid].featurecode] == "islands")
            {
                rt = util.mp(194, p98) + " "; //terrain ON an island
                peakvalley = false;
            }
            else if (featurepointdict[gndict[gnid].featurecode])
            {
                rt = util.mp(98, p98) + " "; //terrain around a point
                peakvalley = !is_height(gndict[gnid].featurecode);

            }
            else
            {
                rt = util.mp(141, p98) + " "; //terrain in an area
                peakvalley = false;
            }

            //terrain label:

            if (allsame)
            {
                if (terrain_type.Contains("peak") || terrain_type.Contains("valley")) //add "mostly" if peak or valley. Sounds funny if combined with "flat" otherwise.
                    rt += util.mp(187) + " ";
                rt += terrain_label(main_terrain); //main terrain
            }
            else if (singlediff)
            {
                rt += util.mp(187) + " " + terrain_label(tmaxtype); //mostly
                if (minorterrain.Contains(centralterrain))
                {
                    rt += ", " + util.mp(188) + " " + terrain_label(centralterrain);
                }
                else
                {
                    string NSEW = minorterrain.Replace(tmintype, "").Trim();
                    int dir = get_direction_from_NSEW(NSEW);
                    if (dir > 0)
                    {
                        string[] p189 = new string[] { util.mp(120 + dir) };
                        rt += ", " + util.mp(189, p189) + " " + terrain_label(tmintype);
                    }
                }
            }
            else if (varied)
                rt += terrain_label("mixed");
            else
            {
                string major_NSEW = majorterrain.Replace(tmaxtype, "").Trim();
                string minor_NSEW = minorterrain.Replace(tmintype, "").Trim();
                int majordir = get_direction_from_NSEW(major_NSEW);
                int minordir = get_direction_from_NSEW(minor_NSEW);

                if (majordir <= 0)
                    rt += terrain_label("mixed");
                else
                {
                    rt += terrain_label(majorterrain) + " " + util.mp(120 + majordir); //main terrain

                    if (minordir > 0)
                    {
                        string[] p189 = new string[] { util.mp(120 + minordir) };
                        rt += ", " + util.mp(189, p189) + " " + terrain_label(minorterrain);
                    }

                }
            }

            //coast, peak/valley:

            if (featurepointdict[gndict[gnid].featurecode])
            {

                foreach (string w in words)
                {
                    if (w.Contains("ocean"))
                    {
                        if (w.Contains("coast"))
                        {
                            string NSEW = w.Substring(w.IndexOf("coast") + 6, 2);
                            int dir = get_direction_from_NSEW(NSEW);
                            if (dir > 0)
                            {
                                if (!String.IsNullOrEmpty(rt))
                                    rt += ". ";
                                string[] p144 = new string[2] { gndict[gnid].Name_ml, util.mp(120 + dir) };
                                rt += util.initialcap(util.mp(144, p144));
                            }
                        }
                        else if (w.Contains("bay"))
                        {
                            string NSEW = w.Substring(w.IndexOf("bay") + 4, 1);
                            int dir = util.tryconvert(NSEW);
                            if (dir > 0)
                            {
                                if (!String.IsNullOrEmpty(rt))
                                    rt += ". ";
                                string[] p144 = new string[2] { gndict[gnid].Name_ml, util.mp(120 + dir) };
                                rt += util.initialcap(util.mp(145) + " " + util.mp(144, p144));
                            }
                        }
                    }
                    else if ((w.Contains("peak")) || w.Contains("valley"))
                    {
                        string[] p205 = new string[1] { get_nsew(w) };
                        if (peakvalley || !String.IsNullOrEmpty(p205[0]))
                        {
                            if (w.Contains("peak"))
                            {
                                if (!String.IsNullOrEmpty(rt))
                                    rt += ". ";

                                rt += util.mp(154, p98);


                                if (!String.IsNullOrEmpty(p205[0]))
                                    rt += " " + util.mp(205, p205);

                            }
                            else if (w.Contains("valley"))
                            {
                                //if ((nsridge < -triggerdiff) && (ewridge > -triggerdiff / 2))
                                //    terrain_type += "NS.."; //North-south valley
                                //else if ((ewridge < -triggerdiff) && (nsridge > -triggerdiff / 2))
                                //    terrain_type += "EW..";
                                //else if (xysum > triggerdiff)
                                //    terrain_type += "SWNE";
                                //else if (xysum < -triggerdiff)
                                //    terrain_type += "SENW";

                                if (!String.IsNullOrEmpty(rt))
                                    rt += ". ";

                                rt += util.mp(149, p98);
                                //string[] p205 = new string[1] { get_nsew(w) };

                                if (!String.IsNullOrEmpty(p205[0]))
                                    rt += " " + util.mp(205, p205);
                            }
                        }
                    }
                    else if (w.Contains("slope"))
                    {
                        if (!String.IsNullOrEmpty(rt) && (allsame || varied))
                            rt += ", " + util.mp(147);
                        else
                        {
                            if (!String.IsNullOrEmpty(rt))
                                rt += ". ";
                            rt += util.mp(146, p98);
                        }
                        if (w.Contains("steep"))
                            rt += " " + util.mp(148);
                        if (w.Contains("N"))
                            rt += " " + util.mp(122); //reverse because NSEW-coding is upwards and text should be downwards slope
                        else if (w.Contains("S"))
                            rt += " " + util.mp(121);
                        else if (w.Contains("E"))
                            rt += " " + util.mp(123);
                        else if (w.Contains("W"))
                            rt += " " + util.mp(124);


                    }
                }
            }

            if (!String.IsNullOrEmpty(rt))
                rt += ".";
            return rt;
        }

        public static string get_nsew(double angle) //compass angle in radians. East = 0.
        {
            double ang = angle;
            if (ang < 0)
                ang += Math.PI;
            ang *= 180.0 / Math.PI; //convert to degrees

            if ((ang < 25) || (ang > 155))
                return get_nsew("EW..");
            else if ((ang >= 25) && (ang <= 65))
                return get_nsew("SWNE");
            else if ((ang > 65) && (ang < 115))
                return get_nsew("NS..");
            else if ((ang >= 115) && (ang <= 155))
                return get_nsew("SENW");
            else
                return "";
        }

        public static string get_nsew(string nsew)
        {
            string rt = "";
            if (nsew.Contains("NS.."))
                rt = util.mp(150);
            else if (nsew.Contains("EW.."))
                rt = util.mp(151);
            else if (nsew.Contains("SWNE"))
                rt = util.mp(152);
            else if (nsew.Contains("SENW"))
                rt = util.mp(153);
            return rt;
        }

        public static string get_overrep(double lat, double lon, double nbradius)
        {
            //double nbradius = 20.0;
            List<int> farlist = getneighbors(lat, lon, nbradius);

            int nnb = 0;
            Dictionary<string, int> nbcount = new Dictionary<string, int>();

            foreach (int nb in farlist)
            {
                if (!gndict.ContainsKey(nb))
                    continue;


                if (catnormdict.ContainsKey(categorydict[gndict[nb].featurecode]))
                {
                    nnb++;

                    if (!nbcount.ContainsKey(categorydict[gndict[nb].featurecode]))
                        nbcount.Add(categorydict[gndict[nb].featurecode], 0);
                    nbcount[categorydict[gndict[nb].featurecode]]++;
                }
            }

            Console.WriteLine("nnb = " + nnb.ToString());


            List<string> overrep = new List<string>();
            //int nbsum = 0;

            foreach (string scat in nbcount.Keys)
            {
                //nbsum += nbcount[scat];
                //Console.WriteLine(scat + ": " + (nbcount[scat] / (1.0*nnb)).ToString("F", culture) + " (" + catnormdict[scat].ToString("F", culture) + ")");
                if ((nbcount[scat] > 3 * catnormdict[scat] * nnb) && (nbcount[scat] > (catnormdict[scat] * nnb + 5)))
                {
                    Console.WriteLine("Overrepresented! " + scat);
                    overrep.Add(categoryml[scat]);
                    foverrephist.Add(scat);
                }
            }
            //Console.WriteLine("nbsum = " + nbsum);

            string overlist = "";
            if (overrep.Count > 0)
            {
                int noo = 0;
                foreach (string oo in overrep)
                {
                    noo++;
                    if (noo > 1)
                    {
                        if (noo == overrep.Count)
                            overlist += util.mp(97);
                        else
                            overlist += ",";
                    }
                    overlist += " " + oo;
                }
            }

            return overlist.Trim();
        }

        public static string get_overrep(int gnid, double nbradius)
        {

            string overlist = get_overrep(gndict[gnid].latitude, gndict[gnid].longitude, nbradius);
            Console.WriteLine("overlist = " + overlist);
            if (String.IsNullOrEmpty(overlist))
                return "";

            string[] p133 = { gndict[gnid].Name_ml, overlist };
            string[] p138 = { nbradius.ToString("F0") };
            string text = " " + util.mp(133, p133) + addnote(util.mp(138, p138) + geonameref(gnid));
            return text;
        }

        public static double[] get_nearhigh(int gnid, List<int> farlist, double radius, double minradius, out int nearhigh, out int altitude)
        {
            nearhigh = -1;
            altitude = 0;
            double[] latlong = { 9999.9, 9999.9 };

            if (!gndict.ContainsKey(gnid))
                return latlong;

            double emax = 0.0;
            double emin = 9999.9;
            int nmax = 0;
            int nmin = 0;

            double maxelevation = 0.0;
            double maxheight = 0.0;
            int nbmaxh = -1;
            double elevation = 0.0;

            //double minradius = 1.0; 

            foreach (int nb in farlist)
            {
                if (!gndict.ContainsKey(nb))
                    continue;


                if (gndict[nb].elevation > 0)
                {
                    if (gndict[nb].elevation > emax)
                    {
                        emax = gndict[nb].elevation;
                        nmax = nb;
                    }
                    if (gndict[nb].elevation < emin)
                    {
                        emin = gndict[nb].elevation;
                        nmin = nb;
                    }

                    if ((gndict[gnid].elevation > 0) && (gndict[gnid].elevation < gndict[nb].elevation))
                    {
                        double dist = get_distance(gnid, nb);
                        if (dist > minradius)
                        {
                            elevation = (gndict[nb].elevation - gndict[gnid].elevation) / dist;
                            if (elevation > maxelevation)
                            {
                                maxelevation = elevation;
                                nearhigh = nb;
                            }
                        }
                        else if (gndict[nb].elevation > maxheight)
                        {
                            maxheight = gndict[nb].elevation;
                            nbmaxh = nb;
                        }

                    }

                }

            }

            int[,] map = get_3x3map(gndict[gnid].latitude, gndict[gnid].longitude);
            int mapsize = map.GetLength(0);

            int x0 = get_x_pixel(gndict[gnid].longitude, gndict[gnid].longitude);
            int y0 = get_y_pixel(gndict[gnid].latitude, gndict[gnid].latitude);

            double scale = Math.Cos(gndict[gnid].latitude * 3.1416 / 180);
            double pixkmx = scale * 40000 / (360 * 1200);
            double pixkmy = 40000.0 / (360.0 * 1200.0);


            double[] elevdirsum = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] elevdirmean = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            int[] nelevdir = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] noceandir = { 0, 0, 0, 0, 0, 0, 0, 0 };

            int r = Convert.ToInt32(radius / pixkmx);
            double r2max = radius / pixkmy * radius / pixkmy;
            double r2min = minradius / pixkmy * minradius / pixkmy;
            double maxelevationdem = 0;
            double maxheightdem = 0.0;

            for (int x = x0 - r; x < x0 + r; x++)
                if ((x > 0) && (x < mapsize))
                    for (int y = y0 - r; y < y0 + r; y++)
                        if ((y > 0) && (y < mapsize))
                        {
                            double r2 = scale * scale * (x - x0) * (x - x0) + (y - y0) * (y - y0);
                            if (r2 < r2max)
                            {
                                if (map[x, y] != 0) //not ocean
                                {
                                    if (map[x, y] != 32768) //bad pixel
                                    {

                                        if (r2 > r2min)
                                        {
                                            double dist = Math.Sqrt(r2) * pixkmy;
                                            elevation = (map[x, y] - gndict[gnid].elevation) / dist;
                                            if (elevation > maxelevationdem)
                                            {
                                                maxelevationdem = elevation;
                                                double one1200 = 1.0 / 1200.0;
                                                double dlon = (x - x0) * one1200;
                                                double dlat = -(y - y0) * one1200; //reverse sign because higher pixel number is lower latitude
                                                latlong[0] = gndict[gnid].latitude + dlat;
                                                latlong[1] = gndict[gnid].longitude + dlon;
                                                altitude = map[x, y];
                                            }
                                        }
                                        else if (map[x, y] > maxheightdem)
                                        {
                                            maxheightdem = map[x, y];
                                        }

                                    }
                                }
                            }
                        }


            if (maxelevationdem > 1.1 * maxelevation)
            {
                nearhigh = -1;
                if (maxheightdem > altitude)
                {
                    latlong[0] = 9999;
                    latlong[1] = 9999;
                }

            }
            else if (gndict.ContainsKey(nearhigh))
            {
                latlong[0] = gndict[nearhigh].latitude;
                latlong[1] = gndict[nearhigh].longitude;
                altitude = gndict[nearhigh].elevation;
                if ((maxheightdem > altitude) || (maxheight > altitude))
                {
                    latlong[0] = 9999;
                    latlong[1] = 9999;
                }
            }

            return latlong;
        }

        public static double[] get_highest(int gnid, double radius, out int altitude)
        {
            //Find highest DEM point within radius
            altitude = 0;
            double[] latlong = { 9999.9, 9999.9 };

            if (!gndict.ContainsKey(gnid))
                return latlong;

            double elevation = 0.0;

            int[,] map = get_3x3map(gndict[gnid].latitude, gndict[gnid].longitude);
            int mapsize = map.GetLength(0);

            int x0 = get_x_pixel(gndict[gnid].longitude, gndict[gnid].longitude);
            int y0 = get_y_pixel(gndict[gnid].latitude, gndict[gnid].latitude);

            double scale = Math.Cos(gndict[gnid].latitude * 3.1416 / 180);
            double pixkmx = scale * 40000 / (360 * 1200);
            double pixkmy = 40000.0 / (360.0 * 1200.0);


            double[] elevdirsum = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] elevdirmean = { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            int[] nelevdir = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] noceandir = { 0, 0, 0, 0, 0, 0, 0, 0 };

            int r = Convert.ToInt32(radius / pixkmx);
            double r2max = radius / pixkmy * radius / pixkmy;
            //double r2min = minradius / pixkmy * minradius / pixkmy;
            double maxelevationdem = 0;


            Console.WriteLine("pixkmx = " + pixkmx.ToString());
            Console.WriteLine("r = " + r.ToString());
            Console.WriteLine("r2max = " + r2max.ToString());

            for (int x = x0 - r; x < x0 + r; x++)
                if ((x > 0) && (x < mapsize))
                    for (int y = y0 - r; y < y0 + r; y++)
                        if ((y > 0) && (y < mapsize))
                        {
                            double r2 = scale * scale * (x - x0) * (x - x0) + (y - y0) * (y - y0);
                            if (r2 < r2max)
                            {
                                if (map[x, y] != 0) //not ocean
                                {
                                    if (map[x, y] != 32768) //bad pixel
                                    {
                                        //double dist = Math.Sqrt(r2) * pixkmy;
                                        elevation = map[x, y];
                                        if (elevation > maxelevationdem)
                                        {
                                            maxelevationdem = elevation;
                                            double one1200 = 1.0 / 1200.0;
                                            double dlon = (x - x0) * one1200;
                                            double dlat = -(y - y0) * one1200; //reverse sign because higher pixel number is lower latitude
                                            latlong[0] = gndict[gnid].latitude + dlat;
                                            latlong[1] = gndict[gnid].longitude + dlon;
                                            altitude = map[x, y];
                                        }
                                    }
                                }
                            }
                        }

            return latlong;
        }

        public static string make_town(int gnid)
        {
            string text = "";

            //List<int> nearlist = getneighbors(gnid, 10.0);
            double nbradius = 20.0;
            List<int> farlist = getneighbors(gnid, nbradius);

            double ttradius = 10.0;
            string terrain_type = get_terrain_type3(gnid, ttradius);

            string[] p158 = { ttradius.ToString("F0") };
            text += "\n\n" + terrain_text(terrain_type, gnid) + addnote(util.mp(158, p158) + addref("vp", viewfinder_ref()) + " " + util.mp(200));

            //Dictionary<string, int> nbcount = new Dictionary<string, int>();
            //Dictionary<string, int> catcount = new Dictionary<string, int>();

            //double nnb = 0;
            long popmax = gndict[gnid].population;
            long pop3 = 3 * gndict[gnid].population;
            long totalpop = gndict[gnid].population;
            int npopmax = 0;
            int npopnear = 0;
            int nppl = 0;
            double popmindist = 9999.9;

            foreach (int nb in farlist)
            {
                if (!gndict.ContainsKey(nb))
                    continue;

                //if (catnormdict.ContainsKey(categorydict[gndict[nb].featurecode]))
                //{
                //    nnb += 1;

                //    if (!nbcount.ContainsKey(categorydict[gndict[nb].featurecode]))
                //        nbcount.Add(categorydict[gndict[nb].featurecode], 0);
                //    nbcount[categorydict[gndict[nb].featurecode]]++;
                //}

                if (gndict[nb].featureclass == 'P')
                {
                    nppl++;
                    totalpop += gndict[nb].population;

                    if (gndict[nb].population > popmax)
                    {
                        popmax = gndict[nb].population;
                        npopmax = nb;
                    }
                    if (gndict[nb].population > pop3)
                    {
                        double dist = get_distance(gnid, nb);
                        if (dist < popmindist)
                        {
                            popmindist = dist;
                            npopnear = nb;
                        }
                    }


                }


            }

            string[] p113 = { gndict[gnid].Name_ml };

            int nhalt = 0;
            int nearhigh = -1;
            double[] nhlatlong = get_nearhigh(gnid, farlist, nbradius, city_radius(gndict[gnid].population), out nearhigh, out nhalt);

            if (nearhigh > 0)
            {
                string[] p116 = { makegnidlink(nearhigh), util.fnum(gndict[nearhigh].elevation) };
                text += " " + util.mp(116, p116) + ", " + util.fnum(get_distance(gnid, nearhigh)) + " " + util.mp(308) + " " + util.mp(100 + get_direction(gnid, nearhigh)) + " " + gndict[gnid].Name_ml + "." + addnote(util.mp(137) + geonameref(gnid));
            }
            else if (gndict[gnid].elevation > nhalt)
            {
                text += " " + util.mp(165, p113) + ".";
            }
            else if (nhlatlong[0] + nhlatlong[1] < 720.0)
            {
                string[] p172 = { util.fnum(nhalt) };
                text += " " + util.mp(172, p172) + ", " + util.fnum(util.get_distance_latlong(gndict[gnid].latitude, gndict[gnid].longitude, nhlatlong[0], nhlatlong[1])) + " " + util.mp(308) + " " + util.mp(100 + util.get_direction_latlong(gndict[gnid].latitude, gndict[gnid].longitude, nhlatlong[0], nhlatlong[1])) + " " + gndict[gnid].Name_ml + "." + addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));

            }


            //double popdensity = totalpop / (3.14 * nbradius * nbradius);
            //if (popdensity < 10.0)
            //{
            //    text += " " + util.mp(117) + ".";
            //}
            //else if (popdensity > 100.0)
            //{
            //    text += " " + util.mp(118) + ".";
            //}
            //else if (popdensity > 400.0)
            //{
            //    text += " " + util.mp(119) + ".";
            //}

            text += make_popdensity(gnid);

            if (!nocapital.Contains(makecountry))
            {
                if (nppl == 0)
                {
                    if (getghostneighbors(gndict[gnid].latitude, gndict[gnid].longitude, 0.5 * nbradius))
                    {
                        text += " " + util.mp(115) + ".";
                    }
                }
                else if (npopmax > 0)
                {
                    int nbig = npopmax;
                    if (npopnear > 0)
                    {
                        if (gndict[nbig].population < 3 * gndict[npopnear].population)
                            nbig = npopnear;
                    }
                    string[] p114 = { makegnidlink(nbig) };
                    text += " " + util.mp(114, p114) + ", " + util.fnum(get_distance(gnid, nbig)) + " " + util.mp(308) + " " + util.mp(100 + get_direction(gnid, nbig)) + " " + gndict[gnid].Name_ml + ".";
                }
                else
                {
                    text += " " + util.mp(113, p113) + ".";
                }
            }

            text += make_landcover(gnid);

            text += get_overrep(gnid, 20.0);

            //public static double get_distance(int gnidfrom, int gnidto)
            //public static int get_direction(int gnidfrom, int gnidto)

            //public static Dictionary<string, string> categorydict = new Dictionary<string, string>(); //from featurecode to category
            //public static Dictionary<string, string> parentcategory = new Dictionary<string, string>(); //from category to parent category
            //public static Dictionary<string, string> categoryml = new Dictionary<string, string>(); //from category to category name in makelang

            return text;
        }

        public static string viewfinder_ref()
        {
            if (makelang == "sv")
                return "{{Webbref |url= {{Viewfinderlänk}}|titel= Viewfinder Panoramas Digital elevation Model|hämtdatum= 2015-06-21|format= |verk= }}";
            else
                return "{{Cite web |url= {{Viewfinderlink}}|title= Viewfinder Panoramas Digital elevation Model|date= 2015-06-21|format= }}";

        }

        public static string make_point(int gnid) //make any pointlike place, from mountains to oases 
        {
            string text = "";

            //List<int> nearlist = getneighbors(gnid, 10.0);
            double nbradius = 20.0;
            List<int> farlist = getneighbors(gnid, nbradius);

            if (!(categorydict[gndict[gnid].featurecode] == "seabed") && !(categorydict[gndict[gnid].featurecode] == "navigation") && !(categorydict[gndict[gnid].featurecode] == "bays") && !(categorydict[gndict[gnid].featurecode] == "reefs"))
            {
                double ttradius = 10.0;
                string terrain_type = get_terrain_type3(gnid, ttradius);

                string[] p158 = { ttradius.ToString("F0") };
                text += "\n\n" + terrain_text(terrain_type, gnid) + addnote(util.mp(158, p158) + addref("vp", viewfinder_ref()) + " " + util.mp(200));
            }
            //Dictionary<string, int> nbcount = new Dictionary<string, int>();
            //Dictionary<string, int> catcount = new Dictionary<string, int>();

            //double nnb = 0;
            long popmax = 0;
            long pop3 = 3000;
            long totalpop = 0;
            int npopmax = 0;
            int npopnear = 0;
            int nppl = 0;
            int nbpop = -1;
            double popmindist = 9999.9;

            foreach (int nb in farlist)
            {
                if (!gndict.ContainsKey(nb))
                    continue;

                //if (catnormdict.ContainsKey(categorydict[gndict[nb].featurecode]))
                //{
                //    nnb += 1;

                //    if (!nbcount.ContainsKey(categorydict[gndict[nb].featurecode]))
                //        nbcount.Add(categorydict[gndict[nb].featurecode], 0);
                //    nbcount[categorydict[gndict[nb].featurecode]]++;
                //}


                if ((gndict[nb].featureclass == 'P') || (gndict[nb].featurecode == "STNB"))
                {
                    nppl++;
                    nbpop = nb;
                    totalpop += gndict[nb].population;

                    if (gndict[nb].population > popmax)
                    {
                        popmax = gndict[nb].population;
                        npopmax = nb;
                    }
                    if (gndict[nb].population > pop3)
                    {
                        double dist = get_distance(gnid, nb);
                        if (dist < popmindist)
                        {
                            popmindist = dist;
                            npopnear = nb;
                        }
                    }


                }



            }

            string[] p113 = { gndict[gnid].Name_ml };

            int nhalt = 0;
            int nearhigh = -1;
            double[] nhlatlong = get_nearhigh(gnid, farlist, nbradius, 1.0, out nearhigh, out nhalt);

            if (nearhigh > 0)
            {
                string[] p116 = { makegnidlink(nearhigh), util.fnum(gndict[nearhigh].elevation) };
                text += " " + util.mp(116, p116) + ", " + util.fnum(get_distance(gnid, nearhigh)) + " " + util.mp(308) + " " + util.mp(100 + get_direction(gnid, nearhigh)) + " " + gndict[gnid].Name_ml + "." + addnote(util.mp(137) + geonameref(gnid));
            }
            else if (gndict[gnid].elevation > nhalt)
            {
                text += " " + util.mp(165, p113) + ".";
            }
            else if (nhlatlong[0] + nhlatlong[1] < 720.0)
            {
                string[] p172 = { util.fnum(nhalt) };
                text += " " + util.mp(172, p172) + ", " + util.fnum(util.get_distance_latlong(gndict[gnid].latitude, gndict[gnid].longitude, nhlatlong[0], nhlatlong[1])) + " " + util.mp(308) + " " + util.mp(100 + util.get_direction_latlong(gndict[gnid].latitude, gndict[gnid].longitude, nhlatlong[0], nhlatlong[1])) + " " + gndict[gnid].Name_ml + "." + addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));
                Console.WriteLine("nhlatlong = " + nhlatlong[0].ToString() + "   " + nhlatlong[1].ToString());
            }


            //double popdensity = totalpop / (3.14 * nbradius * nbradius);
            //if (popdensity < 10.0)
            //{
            //    text += " " + util.mp(117) + ".";
            //}
            //else if (popdensity > 100.0)
            //{
            //    text += " " + util.mp(118) + ".";
            //}
            //else if (popdensity > 400.0)
            //{
            //    text += " " + util.mp(119) + ".";
            //}

            text += make_popdensity(gnid);

            if (nppl == 0)
            {
                if (!getghostneighbors(gndict[gnid].latitude, gndict[gnid].longitude, 0.5 * nbradius))
                {
                    text += " " + util.mp(169) + ".";
                }
            }
            else if (npopmax > 0)
            {
                int nbig = npopmax;
                if (npopnear > 0)
                {
                    if (gndict[nbig].population < 3 * gndict[npopnear].population)
                        nbig = npopnear;
                }
                string[] p114 = { makegnidlink(nbig) };
                text += " " + util.mp(114, p114) + ", " + util.fnum(get_distance(gnid, nbig)) + " " + util.mp(308) + " " + util.mp(100 + get_direction(gnid, nbig)) + " " + gndict[gnid].Name_ml + ".";
            }
            else if (nbpop > 0)
            {
                string[] p212 = { makegnidlink(nbpop) };
                text += " " + util.mp(212, p212) + ", " + util.fnum(get_distance(gnid, nbpop)) + " " + util.mp(308) + " " + util.mp(100 + get_direction(gnid, nbpop)) + " " + gndict[gnid].Name_ml + ".";
            }
            else
            {
                Console.WriteLine("Should never come here. make_point <ret>");
                Console.ReadLine();
            }

            text += make_landcover(gnid);

            int imp = imp_mountainpart(gndict[gnid].featurecode);
            if (imp > 0)
            {
                int mtgnid = attach_to_mountain(gnid);
                if (gndict.ContainsKey(mtgnid))
                {
                    text += " " + gndict[gnid].Name_ml + " " + util.mp(imp) + " " + makegnidlink(mtgnid) + "." + addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));
                }
            }

            if (categorydict[gndict[gnid].featurecode] == "passes")
            {
                int mtgnid1 = -1;
                int mtgnid2 = -1;
                if (between_mountains(gnid, out mtgnid1, out mtgnid2))
                {
                    string[] p199 = { makegnidlink(mtgnid1), makegnidlink(mtgnid2) };
                    text += " " + util.mp(199, p199) + "." + addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));
                }
            }

            text += get_overrep(gnid, 20.0);



            //public static double get_distance(int gnidfrom, int gnidto)
            //public static int get_direction(int gnidfrom, int gnidto)

            //public static Dictionary<string, string> categorydict = new Dictionary<string, string>(); //from featurecode to category
            //public static Dictionary<string, string> parentcategory = new Dictionary<string, string>(); //from category to parent category
            //public static Dictionary<string, string> categoryml = new Dictionary<string, string>(); //from category to category name in makelang

            return text;
        }

        public static string make_channel(int gnid) //make a channel 
        {
            string text = "";

            //List<int> nearlist = getneighbors(gnid, 10.0);
            double nbradius = 20.0;
            List<int> farlist = getneighbors(gnid, nbradius);

            //double nnb = 0;
            long popmax = 0;
            long pop3 = 3000;
            long totalpop = 0;
            int npopmax = 0;
            int npopnear = 0;
            int nppl = 0;
            int nbpop = -1;
            double popmindist = 9999.9;

            foreach (int nb in farlist)
            {
                if (!gndict.ContainsKey(nb))
                    continue;

                if ((gndict[nb].featureclass == 'P') || (gndict[nb].featurecode == "STNB"))
                {
                    nppl++;
                    nbpop = nb;
                    totalpop += gndict[nb].population;

                    if (gndict[nb].population > popmax)
                    {
                        popmax = gndict[nb].population;
                        npopmax = nb;
                    }
                    if (gndict[nb].population > pop3)
                    {
                        double dist = get_distance(gnid, nb);
                        if (dist < popmindist)
                        {
                            popmindist = dist;
                            npopnear = nb;
                        }
                    }
                }
            }

            string[] p113 = { gndict[gnid].Name_ml };

            int nhalt = 0;
            int nearhigh = -1;
            double[] nhlatlong = get_nearhigh(gnid, farlist, nbradius, 1.0, out nearhigh, out nhalt);

            if (nearhigh > 0)
            {
                string[] p116 = { makegnidlink(nearhigh), util.fnum(gndict[nearhigh].elevation) };
                text += " " + util.mp(116, p116) + ", " + util.fnum(get_distance(gnid, nearhigh)) + " " + util.mp(308) + " " + util.mp(100 + get_direction(gnid, nearhigh)) + " " + gndict[gnid].Name_ml + "." + addnote(util.mp(137) + geonameref(gnid));
            }
            else if (gndict[gnid].elevation > nhalt)
            {
                text += " " + util.mp(165, p113) + ".";
            }
            else if (nhlatlong[0] + nhlatlong[1] < 720.0)
            {
                string[] p172 = { util.fnum(nhalt) };
                text += " " + util.mp(172, p172) + ", " + util.fnum(util.get_distance_latlong(gndict[gnid].latitude, gndict[gnid].longitude, nhlatlong[0], nhlatlong[1])) + " " + util.mp(308) + " " + util.mp(100 + util.get_direction_latlong(gndict[gnid].latitude, gndict[gnid].longitude, nhlatlong[0], nhlatlong[1])) + " " + gndict[gnid].Name_ml + "." + addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));
                Console.WriteLine("nhlatlong = " + nhlatlong[0].ToString() + "   " + nhlatlong[1].ToString());
            }

            text += make_popdensity(gnid);

            if (nppl == 0)
            {
                if (!getghostneighbors(gndict[gnid].latitude, gndict[gnid].longitude, 0.5 * nbradius))
                {
                    text += " " + util.mp(169) + ".";
                }
            }
            else if (npopmax > 0)
            {
                int nbig = npopmax;
                if (npopnear > 0)
                {
                    if (gndict[nbig].population < 3 * gndict[npopnear].population)
                        nbig = npopnear;
                }
                string[] p114 = { makegnidlink(nbig) };
                text += " " + util.mp(114, p114) + ", " + util.fnum(get_distance(gnid, nbig)) + " " + util.mp(308) + " " + util.mp(100 + get_direction(gnid, nbig)) + " " + gndict[gnid].Name_ml + ".";
            }
            else if (nbpop > 0)
            {
                string[] p212 = { makegnidlink(nbpop) };
                text += " " + util.mp(212, p212) + ", " + util.fnum(get_distance(gnid, nbpop)) + " " + util.mp(308) + " " + util.mp(100 + get_direction(gnid, nbpop)) + " " + gndict[gnid].Name_ml + ".";
            }
            else
            {
                Console.WriteLine("Should never come here. make_channel <ret>");
                Console.ReadLine();
            }

            text += make_landcover(gnid);

            text += get_overrep(gnid, 20.0);

            return text;
        }

        public static string make_adm(int gnid)
        {
            string text = "";
            Dictionary<char, int> fc = new Dictionary<char, int>();
            fc.Add('H', 0);
            fc.Add('L', 0);
            fc.Add('A', 0);
            fc.Add('P', 0);
            fc.Add('S', 0);
            fc.Add('T', 0);
            fc.Add('U', 0);
            fc.Add('V', 0);

            string[] p79 = new string[1] { gndict[gnid].Name_ml };

            //bordering:

            if (wdid > 0)
            {
                List<int> neighbors = wdtreeclass.get_wd_prop_idlist(wdtreeclass.propdict["borders"], currentxml);

                if (neighbors.Count > 1)
                {
                    text += " " + util.mp(96, p79);
                    int i = 0;
                    foreach (int wdnb in neighbors)
                    {
                        i++;
                        if (i == neighbors.Count)
                            text += util.mp(97);
                        else if (i > 1)
                            text += ",";
                        text += " " + wdtreeclass.get_name_from_wdid(wdnb);
                    }
                    text += ". ";
                }
                else if (neighbors.Count == 1)
                {
                    text += " " + util.mp(96, p79);
                    text += " " + wdtreeclass.get_name_from_wdid(neighbors[0]);
                    text += ".";
                }
            }

            //terrain type

            if (gndict[gnid].area > 0)
            {
                double nbradius = Math.Sqrt(gndict[gnid].area);

                string terrain_type = get_terrain_type3(gnid, nbradius);

                //string[] p136 = { nbradius.ToString("F0") };
                text += "\n\n" + terrain_text(terrain_type, gnid);
                //if ( makelang == "sv" )
                text += addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));

            }



            //administrative subdivisions:

            if (gndict[gnid].children.Count > 1)
            {
                Console.WriteLine("Subdivisions from gnid-kids");
                text += "\n\n" + util.mp(79, p79) + "\n";
                foreach (int kid in gndict[gnid].children)
                    text += "* " + makegnidlink(kid) + "\n";
            }
            else if (wdid > 0)
            {

                List<int> kidlist = wdtreeclass.get_wd_kids(currentxml);
                Console.WriteLine("Subdivisions from wikidata-kids, " + kidlist.Count.ToString());
                if (kidlist.Count > 0)
                {
                    text += "\n\n" + util.mp(79, p79) + "\n";
                    foreach (int kid in kidlist)
                    {
                        text += "* " + wdtreeclass.get_name_from_wdid(kid) + "\n";
                    }
                }
            }



            //feature lists:
            if (gndict[gnid].features.Count > 0)
            {
                foreach (int ff in gndict[gnid].features)
                {
                    fc[gndict[ff].featureclass]++;
                }
                // H: vatten
                // L: diverse människoskapade områden 
                // P: samhällen
                // S: Byggnadsverk
                // T: diverse naturfenomen; berg, dalar, öar...
                // U: undervattensfenomen
                // V: växtlighet; skogar, hedar...

                SortedDictionary<long, int> flist = new SortedDictionary<long, int>();

                if (fc['P'] > 0)
                {
                    text += "\n\n" + util.mp(80, p79) + "\n";
                    foreach (int ff in gndict[gnid].features)
                        if (gndict[ff].featureclass == 'P')
                        {
                            long pop = gndict[ff].population;
                            while (flist.ContainsKey(pop))
                                pop++;
                            flist.Add(pop, ff);
                        }
                    string sorted = "";
                    foreach (int fpop in flist.Keys)
                    {
                        //sorted = "\n* " + makegnidlink(flist[fpop]) + " (" + fpop.ToString("N0",nfi) + " " + util.mp(81) + ")" + sorted; //With pop
                        sorted = "\n* " + makegnidlink(flist[fpop]) + sorted; //Without pop
                    }
                    text += sorted;
                }

                SortedDictionary<string, int> fl2 = new SortedDictionary<string, int>();

                if (fc['H'] + fc['T'] + fc['U'] + fc['V'] > 10)
                {
                    //public static Dictionary<string, string> categorydict = new Dictionary<string, string>(); //from featurecode to category
                    //public static Dictionary<string, string> parentcategory = new Dictionary<string, string>(); //from category to parent category
                    //public static Dictionary<string, string> categoryml = new Dictionary<string, string>(); //from category to category name in makelang

                    text += "\n\n" + util.mp(91, p79) + "\n";

                    foreach (string cat in categoryml.Keys)
                    {
                        fl2.Clear();
                        foreach (int ff in gndict[gnid].features)
                            if ((gndict[ff].featureclass == 'H') || (gndict[ff].featureclass == 'T') || (gndict[ff].featureclass == 'U') || (gndict[ff].featureclass == 'V'))
                            {
                                if (categorydict[gndict[ff].featurecode] == cat)
                                {
                                    string ffname = gndict[ff].Name_ml;
                                    while (fl2.ContainsKey(ffname))
                                        ffname += " ";
                                    fl2.Add(ffname, ff);
                                }
                            }

                        if (fl2.Count > 0)
                        {
                            text += "\n\n* " + util.initialcap(categoryml[cat]) + ":\n";
                            string sorted = "";
                            foreach (string fname in fl2.Keys)
                            {
                                sorted += "\n:* " + makegnidlink(fl2[fname]) + " (" + linkfeature(gndict[fl2[fname]].featurecode, fl2[fname]) + ")";
                            }
                            text += sorted;
                        }
                    }

                }
                else if (fc['H'] + fc['T'] + fc['U'] + fc['V'] > 0)
                {
                    fl2.Clear();

                    text += "\n\n" + util.mp(91, p79) + "\n";
                    foreach (int ff in gndict[gnid].features)
                        if ((gndict[ff].featureclass == 'H') || (gndict[ff].featureclass == 'T') || (gndict[ff].featureclass == 'U') || (gndict[ff].featureclass == 'V'))
                        {
                            string ffname = gndict[ff].Name_ml;
                            while (fl2.ContainsKey(ffname))
                                ffname += " ";
                            fl2.Add(ffname, ff);
                        }
                    string sorted = "";
                    foreach (string fname in fl2.Keys)
                    {
                        sorted += "\n* " + makegnidlink(fl2[fname]) + " (" + linkfeature(gndict[fl2[fname]].featurecode, fl2[fname]) + ")";
                    }
                    text += sorted;
                }



            }

            return text;

        }


        public static string make_island(int gnid, Page p)
        {
            string text = "";
            Dictionary<char, int> fc = new Dictionary<char, int>();
            fc.Add('H', 0);
            fc.Add('L', 0);
            fc.Add('A', 0);
            fc.Add('P', 0);
            fc.Add('S', 0);
            fc.Add('T', 0);
            fc.Add('U', 0);
            fc.Add('V', 0);

            string[] p79 = new string[1] { gndict[gnid].Name_ml };


            //terrain type

            if (gndict[gnid].area > 0)
            {
                //double nbradius = Math.Sqrt(gndict[gnid].area);

                string terrain_type = get_terrain_type_island(gnid);

                //string[] p136 = { nbradius.ToString("F0") };
                string tt = terrain_text(terrain_type, gnid);
                if (!String.IsNullOrEmpty(tt))
                {
                    text += "\n\n" + tt;
                    //if (makelang == "sv")
                    text += addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));
                }

                string heightmarker = "|elevation_max";
                if (terrain_type.Contains(heightmarker))
                {
                    string elmax = terrain_type.Substring(terrain_type.IndexOf(heightmarker) + heightmarker.Length + 1);
                    int maxheight = util.tryconvert(elmax);
                    if (maxheight > gndict[gnid].elevation)
                    {
                        string[] p163 = { util.fnum(maxheight) };
                        text += " " + util.mp(163, p163);
                        p.SetTemplateParameter("geobox", "highest_elevation", elmax, true);
                    }

                }

                if ((islanddict.ContainsKey(gnid)) && ((islanddict[gnid].kmns + islanddict[gnid].kmew) > 1.0))
                {
                    string[] p164 = { islanddict[gnid].kmns.ToString("F1", culture), islanddict[gnid].kmew.ToString("F1", culture) };
                    text += " " + util.mp(164, p164);
                    //if (makelang == "sv")
                    text += addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));

                    if (islanddict[gnid].kmew > 2 * islanddict[gnid].kmns)
                    {
                        p.SetTemplateParameter("geobox", "length", islanddict[gnid].kmew.ToString(), true);
                        p.SetTemplateParameter("geobox", "width", islanddict[gnid].kmns.ToString(), true);
                        p.SetTemplateParameter("geobox", "length_orientation", "EW", true);
                        p.SetTemplateParameter("geobox", "width_orientation", "NS", true);
                    }
                    else if (islanddict[gnid].kmns > 2 * islanddict[gnid].kmew)
                    {
                        p.SetTemplateParameter("geobox", "length", islanddict[gnid].kmns.ToString(), true);
                        p.SetTemplateParameter("geobox", "width", islanddict[gnid].kmew.ToString(), true);
                        p.SetTemplateParameter("geobox", "length_orientation", "NS", true);
                        p.SetTemplateParameter("geobox", "width_orientation", "EW", true);
                    }
                }

                if (gndict[gnid].area < 300)
                    text += " " + make_landcover(gnid);
            }




            //feature lists:
            if (islanddict.ContainsKey(gnid))
                if (islanddict[gnid].onisland.Count > 0)
                {
                    foreach (int ff in islanddict[gnid].onisland)
                    {
                        if (!categorydict.ContainsKey(gndict[ff].featurecode))
                        {
                            Console.WriteLine(gndict[ff].featurecode + " missing in categorydict");
                            Console.ReadLine();
                            continue;
                        }
                        if (categorydict[gndict[ff].featurecode] != "islands") //Not island on island
                            fc[gndict[ff].featureclass]++;
                    }
                    // H: vatten
                    // L: diverse människoskapade områden 
                    // P: samhällen
                    // S: Byggnadsverk
                    // T: diverse naturfenomen; berg, dalar, öar...
                    // U: undervattensfenomen
                    // V: växtlighet; skogar, hedar...

                    SortedDictionary<long, int> flist = new SortedDictionary<long, int>();

                    if (fc['P'] > 0)
                    {
                        text += "\n\n" + util.mp(159, p79) + "\n";
                        foreach (int ff in islanddict[gnid].onisland)
                            if (gndict[ff].featureclass == 'P')
                            {
                                long pop = gndict[ff].population;
                                while (flist.ContainsKey(pop))
                                    pop++;
                                flist.Add(pop, ff);
                            }
                        string sorted = "";
                        foreach (int fpop in flist.Keys)
                        {
                            //sorted = "\n* " + makegnidlink(flist[fpop]) + " (" + fpop.ToString("N0",nfi) + " " + util.mp(81) + ")" + sorted;
                            sorted = "\n* " + makegnidlink(flist[fpop]) + sorted;
                        }
                        text += sorted;
                    }

                    SortedDictionary<string, int> fl2 = new SortedDictionary<string, int>();

                    if (fc['H'] + fc['T'] + fc['U'] + fc['V'] > 10)
                    {
                        //public static Dictionary<string, string> categorydict = new Dictionary<string, string>(); //from featurecode to category
                        //public static Dictionary<string, string> parentcategory = new Dictionary<string, string>(); //from category to parent category
                        //public static Dictionary<string, string> categoryml = new Dictionary<string, string>(); //from category to category name in makelang

                        text += "\n\n" + util.mp(160, p79) + "\n";

                        foreach (string cat in categoryml.Keys)
                        {
                            if (cat == "islands") //not island on island
                                continue;

                            fl2.Clear();
                            foreach (int ff in islanddict[gnid].onisland)
                                if ((gndict[ff].featureclass == 'H') || (gndict[ff].featureclass == 'T') || (gndict[ff].featureclass == 'U') || (gndict[ff].featureclass == 'V'))
                                {
                                    if (categorydict[gndict[ff].featurecode] == cat)
                                    {
                                        string ffname = gndict[ff].Name_ml;
                                        while (fl2.ContainsKey(ffname))
                                            ffname += " ";
                                        fl2.Add(ffname, ff);
                                    }
                                }

                            if (fl2.Count > 0)
                            {
                                text += "\n* " + util.initialcap(categoryml[cat]) + ":\n";
                                string sorted = "";
                                foreach (string fname in fl2.Keys)
                                {
                                    sorted += "\n** " + makegnidlink(fl2[fname]) + " (" + linkfeature(gndict[fl2[fname]].featurecode, fl2[fname]) + ")";
                                }
                                text += sorted;
                            }
                        }

                    }
                    else if (fc['H'] + fc['T'] + fc['U'] + fc['V'] > 0)
                    {
                        fl2.Clear();

                        foreach (int ff in islanddict[gnid].onisland)
                            if ((gndict[ff].featureclass == 'H') || (gndict[ff].featureclass == 'T') || (gndict[ff].featureclass == 'U') || (gndict[ff].featureclass == 'V'))
                            {
                                if (categorydict[gndict[ff].featurecode] != "islands") //Not island on island
                                    continue;
                                string ffname = gndict[ff].Name_ml;
                                while (fl2.ContainsKey(ffname))
                                    ffname += " ";
                                fl2.Add(ffname, ff);
                            }

                        if (fl2.Count > 0)
                        {
                            text += "\n\n" + util.mp(160, p79) + "\n";
                            string sorted = "";
                            foreach (string fname in fl2.Keys)
                            {
                                sorted += "\n* " + makegnidlink(fl2[fname]) + " (" + linkfeature(gndict[fl2[fname]].featurecode, fl2[fname]) + ")";
                            }
                            text += sorted;
                        }
                    }



                }

            return text;

        }


        public static string make_range(int gnid, Page p)
        {
            string text = "";
            Dictionary<char, int> fc = new Dictionary<char, int>();
            fc.Add('H', 0);
            fc.Add('L', 0);
            fc.Add('A', 0);
            fc.Add('P', 0);
            fc.Add('S', 0);
            fc.Add('T', 0);
            fc.Add('U', 0);
            fc.Add('V', 0);

            if (rangedict.ContainsKey(gnid))
            {
                string[] p207 = new string[3] { gndict[gnid].Name_ml, util.fnum(rangedict[gnid].length), get_nsew(rangedict[gnid].angle) };

                text += "\n\n" + util.mp(207, p207) + addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));
                if (rangedict[gnid].maxheight > 0)
                {
                    string[] p208 = new string[1] { util.fnum(rangedict[gnid].maxheight) };
                    text += " " + util.mp(208, p208);
                }
                else if (gndict.ContainsKey(-rangedict[gnid].maxheight))
                {
                    int hgnid = -rangedict[gnid].maxheight;
                    string[] p209 = new string[2] { makegnidlink(hgnid), util.fnum(gndict[hgnid].elevation) };
                    text += " " + util.mp(209, p209);
                }

                string[] p79 = new string[1] { gndict[gnid].Name_ml };

                //feature lists:
                if (rangedict[gnid].inrange.Count > 0)
                {
                    foreach (int ff in rangedict[gnid].inrange)
                    {
                        fc[gndict[ff].featureclass]++;
                    }
                    // H: vatten
                    // L: diverse människoskapade områden 
                    // P: samhällen
                    // S: Byggnadsverk
                    // T: diverse naturfenomen; berg, dalar, öar...
                    // U: undervattensfenomen
                    // V: växtlighet; skogar, hedar...

                    SortedDictionary<string, int> fl2 = new SortedDictionary<string, int>();

                    if (fc['H'] + fc['T'] + fc['U'] + fc['V'] > 0)
                    {

                        text += "\n\n" + util.mp(206, p79) + "\n";
                        foreach (int ff in rangedict[gnid].inrange)
                            if ((gndict[ff].featureclass == 'H') || (gndict[ff].featureclass == 'T') || (gndict[ff].featureclass == 'U') || (gndict[ff].featureclass == 'V'))
                            {
                                string ffname = gndict[ff].Name_ml;
                                while (fl2.ContainsKey(ffname))
                                    ffname += " ";
                                fl2.Add(ffname, ff);
                            }
                        string sorted = "";
                        foreach (string fname in fl2.Keys)
                        {
                            sorted += "\n* " + makegnidlink(fl2[fname]);
                        }
                        text += sorted;
                    }



                }
            }
            return text;

        }

        public static void fill_ocean_shapes()
        {
#if (DBGEOFLAG)

            string pacific = "POLYGON ((-67 -55.15,-72 -51.5,-62 0,-79.2 9.33,-81.8 8.5,-91.6 17.5,-100.4 19.45,-125 66,110 69,98.75 8.62,106.08 -5.88,114.1 -8.2,117.5 -8.8,118.94 -8.52,122.46 -8.65,131.4 -7.62,133.38 -2.86,142.1 -5.5,147 -48.5,147 -80, -66 -80,-67 -55.15))";
            string atlantic = "POLYGON ((-67 -55.15,-72 -51.5,-62 0,-79.2 9.33,-81.8 8.5,-91.6 17.5,-100.4 19.45,-125 66,-100 64,-50 80,16.8 79,23.5 69.35,13.46 57.14,-5.88 36.74,21.5 1.8,15 -80, -67 -80,-67 -55.15))";
            string baltic = "POLYGON ((9.24 54.7,13.63 56.1,19.26 67.83, 30.3 67.9, 36.5 54,9.77 53.15,9.24 54.7))";
            string mediterranean = "POLYGON ((-5.88 35.94,4 48,21.55 46.8,30.86 39.14,40.5 39.3,37 31.1,-2.5 29.9,-5.88 35.94))";
            string blacksea = "POLYGON ((29.4 39.8,22.2 49,43.5 49,44.4 39.5,29.4 39.8))";
            string indianocean = "POLYGON ((98.75 8.62,106.08 -5.88,114.1 -8.2,117.5 -8.8,118.94 -8.52,122.46 -8.65,131.4 -7.62,133.38 -2.86,142.1 -5.5,147 -48.5,147 -80,19.5 -80,19.4 -34.4, 29 29.2,98 44,98.75 8.62))";
            string arcticocean = "POLYGON ((180 67,81 62,27.5 63.6,25.3 70.8,18.3 78.6,-40 81,-83 77,-83.66 68.11,-89.1 66.4,-167 65.8,-180 66,180 67))";
            string westphil = "POLYGON ((102.75 -0.25, 104.63 0.77,98.69 9.19,99.6 24,116.84 24.15, 120.77 18.06,121.62 14.16,119.44 10.58,118.713 9.955,117.205 8.405,116.46 4.15,110.9 -2.2,103.9 -2.6,102.75 -0.25))";
            string hudsonbay = "POLYGON ((-80 49,-111.9 63.9,-89.1 66.8,-71.7 60.25,-75.1 49.9,-80 49))";
            string gulfofmexico = "POLYGON ((-80.94 25.5,-82.1 23,-89.6 20.57,-90.4 17,-100 18.8,-98 33,-83 32,-80.94 25.5))";
            string caribbean = "POLYGON ((-77.2 7.3,-78.3 9.16,-79.55 9.42,-81.4 8.29,-89.41 16.08,-87.59 21,-82.07 23.05,-74.645 20.25,-68.62 18.55,-61.99 17.59,-61.804 17.065,-61.32 15.35,-61.1 14.74,-60.85 14.5,-61 13.79,-61.2 13.16,-61.45 12.48,-61.7 12.08,-61.3 10.735,-62.9 10.66,-67.9 7.4,-77.2 7.3))";
            string persiangulf = "POLYGON ((56.78 27.76,53.6 19.73,44.1 30.15,50.5 32.9,56.78 27.76))";
            string bengalbay = "POLYGON ((77.9 10.8,77.4 24.8,98.3 24.8,99.1 11.74,77.9 10.8))";
            string japansea = "POLYGON ((128.3 35.2,125.3 41.65,139.3 52.3,142.6 52.3,142.23 46.7,141.87 43.27,140 42.54,140.9 39.1,136.5 35.36,131.89 34.6,131 34.1,130.4 33.3,128.3 35.2))";
            string northsea = "POLYGON ((0 51.5,-5.26 57.2,-4.15 58.25,-1.28 60,7.5 60,9.3 56,9.17 54.4,10.8 53.2,3.1 50.45,0 51.5))";
            string englishchannel = "POLYGON ((0 51.5,3.1 50.45,-1.6 48,-4.26 48.52,-5.26 50.19,-3.8 51,0 51.5))";
            string beringsea = "POLYGON ((-155 65,-155.9 58.4,-159.7 56.1,-160.57 55.64,-161.68 55.77,-164.63 54.56,-176.85 51.7,177.46 51.94,172.9 52.9,159.2 57.8,163.5 60.4,176.8 68.3,-174.3 65.9,-171.6 66.3,-155 65))";
            string tasmansea = "POLYGON ((150 -27,173.33 -35.1,174.59 -36.23,174.73 -36.88,175.1 -37,175.4 -41,172.9 -41.8,168.1 -46,147.56 -42.15,150 -27))";
            string philsea = "POLYGON ((121.2 14.38,120.7 23.75,125.33 24.77,127.7 26.2,127.8 26.4,128.2 26.76,129.43 28.33,130.5 30.38,131 33.1,135.75 34.3,136.8 36,138.7 36,145.8 15.26,144.8 13.5,134.55 7.45,126.1 7.5,125.45 9.72,124.6 11.2,125.4 11.46,124.5 12.5,124 12.6,124.12 13,123.56 13.05,122.67 14.13,122.17 13.85,121.2 14.38))";
            string sulusea = "POLYGON ((116.6 6.5, 117 8,117.25 8.5,118.7 9.93,119 10.16,119.5 10.7,119.44 11.3,120 12.15,121.16 12.47,122.97 11.25,123 9.27,123.2 8,122.3 7.9,122 6.56,120.16 5.26,119.84 5.13,116.5 5.4,116.6 6.5))";
            string boholsea = "POLYGON ((123.1 9.25,124.5 10,125 10.56,125.15 10.08,126.13 8.88,124.18 7.7,123.19 8.1,123.1 9.25))";
            string adriatic = "POLYGON ((18.22 40.28,11 44,12.5 48.2,22 41.3,18.22 40.28))";
            string camotes = "POLYGON ((123.89 10.39,123.96 11.06,124.86 11.17,124.87 10.2,124.22 10.03,123.89 10.39))";
            string visayan = "POLYGON ((123.97 11.06,123.2 10.8,123 11.4,123.12 11.56,123.17 11.94,123.44 12.4,124 11.9,124.4 11.4,124.4 11.1,123.97 11.06))";
            string sibuyan = "POLYGON ((122 11.8,121.1 12.4,121.2 13.2,122 13.4,122.48 14.18,123.5 13.3,123.35 12.2,122.7 11.26,122 11.8))";
            //string samarsea = "POLYGON ((123.3 12.5,123.4 13.2,123.8 13,124.05 13,124.07 12.65,125.15 12.2,125.15 11.4,124.5 11.14,124.3 11.54,123.57 12.3,123.3 12.4,123.3 12.5))";
            string inlandsea = "POLYGON ((130.8 33.7,131 34.3,135.1 35.3,135.03 34.63,134.73 34.3,132.8 33.5,130.7 33,130.8 33.7))";
            string yellowsea = "POLYGON ((119.5 32.3,114 40.4,125 43.5,128 35.6,119.5 32.3))";
            string celebes = "POLYGON ((117 5.2,119.93 5.15,121 6,122.4 8,123.7 7.9,125 7.5,125.4 6,125.58 3.46,125.14 1.56,124 1.56,120.8 0.8,120 0.4,116 2.2,117 5.2))";
            string molucca = "POLYGON ((125.58 3.46,125.14 1.56,124 1.56,120.8 0.8,120 0.4,119.9 -0.53,120.4 -1.9,122.2 -1.1,124.44 -1.777,125.96 -1.85,127.95 -1.61,127.78 0.66,127.56 1.04,127.8 1.98,126.7 4,125.58 3.46))";
            string makassar = "POLYGON ((117 1.8,120.2 0.65,119.9 -0.46,120.2 -1.36,119 -3.5,115 -3.5,117 1.8))";
            string andaman = "POLYGON ((95.5 5.3,93.8 7,92.75 9.17,92.48 10.76,92.713 12.13,92.97 13.49,95 18,99 18,99.18 11,98.36 8.38,98.33 7.88,95.5 5.3))";
            string redsea = "POLYGON ((29.82 30.44,36.8 30.8,46 15,39 9,29.82 30.44))";
            string labradorsea = "POLYGON ((-44 61,-60 52,-64.7 59.3,-70 64,-68 67,-45 67,-44 61))";
            string baffinbay = "POLYGON ((-63.7 66.4,-83.1 72,-83.2 77.2,-81.4 78.6,-40 77,-44 67,-63.7 66.4))";
            string hudsonstrait = "POLYGON ((-64.8 60.1,-65 55,-77 62,-81 64,-76.6 65,-73.3 65,-64.9 61.55,-64.8 60.1))";
            string norwegian = "POLYGON ((7 61,-7.09 62.2,-18 65,-8.17 71.1,16.4 77.3,25.34 70.8,14 62,7 61))";
            string whitesea = "POLYGON ((30 69,51 63,31 63,30 69))";
            string barents = "POLYGON ((25.38 70.8,19 74.5,16.4 76.8,18.4 79,26 80,60 81,67 77,59 75.5,53.4 72,67 68,45 65,24 70,25.38 70.8))";
            string kattegat = "POLYGON ((10.5 57.7,13.5 58,13 56.3,12.3 55.9,12 55.4,11.2 55.66,10.35 55.38,9.2 55.7,10.26 57.57,10.5 57.7))";
            string california = "POLYGON ((-109.7 23.2,-111.11 24.6,-112.35 27.1,-116.1 31.3,-113 34.5,-107 24.5,-109.7 23.2))";
            string skagerrak = "POLYGON ((10.3 57.6,9.5 57.1,8.65 57,7.1 58.5,11.5 61,12.3 58,10.3 57.6))";
            string mozambique = "POLYGON ((49.2 -12.07,49 -17,45.18 -25.56,31.7 -23.5,37.7 -10.2,49.2 -12.07))";
            string malaccastrait = "POLYGON ((104.3 1.1,102 -0.75,97 5,100 7,104.3 1.1))";
            string javasea = "POLYGON ((106 -6.11,104 -3,114.8 -2,119.7 -5.3,118.53 -8.46,117.5 -8.9,115.13 -8.24,109.2 -7,106 -6.11))";
            string floressea = "POLYGON ((118.44 -8.44,119.9 -4.5,119.9 -1.1,123 -4.6,126.8 -8.5,125.3 -8.8,124.8 -8.25,122.66 -8.47,122.38 -8.67,118.44 -8.44))";
            string cebustrait = "POLYGON ((123.75 9.55,123.4 9.67,123.77 10.53,124.29 9.93,123.75 9.55))";
            string tanonstrait = "POLYGON ((123.34 9.49,122.96 9.2,123.23 10.9,123.92 10.89,123.34 9.49))";
            string verdeisland = "POLYGON ((120.1 13.81,120.8 14.2,121.35 13.7,121.2 13.3,120.4 13.5,120.4 13.63,120.1 13.81))";
            string carpentaria = "POLYGON ((136.5 -12.3,142.4 -10.9,142 -20,132 -16,135.6 -12.9,136.5 -12.3))";
            string timorsea = "POLYGON ((126 -15.4,123.17 -10.67,124.6 -9.3,127.2 -8.4,130.9 -11.4,132 -18,126 -15.4))";
            string arafura = "POLYGON ((130.7 -11.5,127.2 -8,131.5 -7.4,133 -4 -2.76,133.8 -2.6,135.6 -4,142 -6,142.5 -11.3,131 -14,130.7 -11.5))";
            string greataustralian = "POLYGON ((117 -33,141 -20,141 -38,127 -42,117 -33))";
            string bandasea = "POLYGON ((125.8 -8.88,122.7 -5.5,123 -4.1,121.3 -3.1,121.1 -1.6,123.2 -0.7,123.4 -1.3,124.6 -1.7,125.9 -1.9,126.6 -3.4,129 -3,130.4 -3.2,132.7 -5.7,131.5 -7.4,125.8 -8.88))";
            string arabiansea = "POLYGON ((50.7 11.4,48.9 16.8,59 22.3,61 27,78 27,77.4 8.6,50.7 11.4))";
            string oman = "POLYGON ((56.1 25.9,57 29,61.5 26,59.6 22.3,55.6 24.5,56.1 25.9))";
            string aden = "POLYGON ((37 9,51 17,50.5 10.6,37 9))";
            string karasea = "POLYGON ((59 70.2,54.2 71.5,56.3 73.9,59.5 75.5,68.3 76.7,92.7 79.9,103.2 78.6,105 75.6,82 65,61 64,63 69,59 70.2))";
            string okhotsk = "POLYGON ((141.75 45.11,142.1 46.5,142.77 50.5,142.3 52.2,129 54,150 68,171 63,162.8 60.2,158 55.6,156.9 56.2,155.4 50.2,153 47.75,150 46,147.3 44.8,142.8 42.9,141.75 45.11))";
            string bothnia = "POLYGON ((27 61,12.5 59,20 67,27 67,27 61))";
            string finskaviken = "POLYGON ((23.6 59,23 61,29.5 61,32 59,23.6 59))";
            string manilabay = "POLYGON ((120.5 14.5,120.5 15.3,121.4 14.6,120.7 14.2,120.5 14.5))";
            string ragaygulf = "POLYGON ((122.6 13.25,122.26 13.9,122.5 14.2,123.75 12.9,123.36 12.71,123.24 12.86,123.13 12.95,123 13.1,122.6 13.25))";
            string samarsea = "POLYGON ((124.3 11.5,124.04 11.77,123.5 12.3,124 12.8,125.3 11.7,124.6 11.1,124.3 11.5))";
            string aegean = "POLYGON ((23.1 36.5,22.3 37.6,22.96 37.93,23.5 38.2,21.3 39.3,22 42,27 41.3,26.9 40.56,26.4 40.3,29.4 38,27.9 36.1,26.1 35.1,23.7 35.35,23.1 36.5))";
            //string  = "POLYGON (())";

            //string  = "POLYGON (())";
            //string  = "POLYGON (())";

            //line = reorient_polygon(line);
            //line = close_polygon(line);
            //bool cw2 = clockwise(line, (double)dmc.XCoordinate, (double)dmc.YCoordinate);

            oceanstringdict.Add("pacific", pacific);
            oceanstringdict.Add("atlantic",atlantic);
            oceanstringdict.Add("baltic",baltic);
            oceanstringdict.Add("mediterranean",mediterranean);
            oceanstringdict.Add("blacksea",blacksea);
            oceanstringdict.Add("indianocean",indianocean);
            oceanstringdict.Add("arcticocean",arcticocean);
            oceanstringdict.Add("westphil", westphil);
            oceanstringdict.Add("hudsonbay", hudsonbay);
            oceanstringdict.Add("gulfofmexico", gulfofmexico);
            oceanstringdict.Add("caribbean", caribbean);
            oceanstringdict.Add("persiangulf",persiangulf);
            oceanstringdict.Add("bengalbay",bengalbay);
            oceanstringdict.Add("japansea",japansea);
            oceanstringdict.Add("northsea",northsea);
            oceanstringdict.Add("englishchannel",englishchannel);
            oceanstringdict.Add("beringsea",beringsea);
            oceanstringdict.Add("tasmansea",tasmansea);
            oceanstringdict.Add("philsea",philsea);
            oceanstringdict.Add("sulusea",sulusea);
            oceanstringdict.Add("boholsea",boholsea);
            oceanstringdict.Add("adriatic",adriatic);
            oceanstringdict.Add("camotes",camotes);
            oceanstringdict.Add("visayan",visayan);
            oceanstringdict.Add("sibuyan",sibuyan);
            oceanstringdict.Add("samarsea",samarsea);
            oceanstringdict.Add("inlandsea",inlandsea);
            oceanstringdict.Add("yellowsea",yellowsea);
            oceanstringdict.Add("celebes",celebes);
            oceanstringdict.Add("molucca",molucca);
            oceanstringdict.Add("makassar",makassar);
            oceanstringdict.Add("andaman",andaman);
            oceanstringdict.Add("redsea",redsea);
            oceanstringdict.Add("labradorsea",labradorsea);
            oceanstringdict.Add("baffinbay",baffinbay);
            oceanstringdict.Add("hudsonstrait",hudsonstrait);
            oceanstringdict.Add("norwegian",norwegian);
            oceanstringdict.Add("whitesea",whitesea);
            oceanstringdict.Add("barents",barents);
            oceanstringdict.Add("kattegat",kattegat);
            oceanstringdict.Add("california",california);
            oceanstringdict.Add("skagerrak",skagerrak);
            oceanstringdict.Add("mozambique",mozambique);
            oceanstringdict.Add("malaccastrait",malaccastrait);
            oceanstringdict.Add("javasea",javasea);
            oceanstringdict.Add("floressea",floressea);
            oceanstringdict.Add("cebustrait",cebustrait);
            oceanstringdict.Add("tanonstrait",tanonstrait);
            oceanstringdict.Add("verdeisland",verdeisland);
            oceanstringdict.Add("carpentaria",carpentaria);
            oceanstringdict.Add("timorsea",timorsea);
            oceanstringdict.Add("arafura",arafura);
            oceanstringdict.Add("greataustralian",greataustralian);
            oceanstringdict.Add("bandasea",bandasea);
            oceanstringdict.Add("arabiansea",arabiansea);
            oceanstringdict.Add("oman",oman);
            oceanstringdict.Add("aden",aden);
            oceanstringdict.Add("karasea",karasea);
            oceanstringdict.Add("okhotsk",okhotsk);
            oceanstringdict.Add("bothnia",bothnia);
            oceanstringdict.Add("finskaviken",finskaviken);
            oceanstringdict.Add("manilabay",manilabay);
            oceanstringdict.Add("ragaygulf",ragaygulf);
            //oceanstringdict.Add("samarsea",samarsea);
            oceanstringdict.Add("aegean",aegean);
            //oceanstringdict.Add("",);

            foreach (string ocean in oceanstringdict.Keys)
            {
                if ((ocean == "pacific") || (ocean == "molucca"))
                    oceandict.Add(ocean, DbGeography.FromText(oceanstringdict[ocean]));
                else
                {
                    oceandict.Add(ocean, DbGeography.FromText(reorient_polygon(oceanstringdict[ocean])));
                }
                Console.WriteLine(ocean + ": " + oceandict[ocean].Area/1000000);

            }

            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "oceans.txt"))
            {

                String headline = "";
                headline = sr.ReadLine();

                int icol = 0;
                string[] langs = headline.Split('\t');
                for (icol = 0; icol < langs.Length; icol++)
                {
                    if (langs[icol] == makelang)
                    {
                        break;
                    }
                }

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    Console.WriteLine(line);

                    string[] words = line.Split('\t');
                    if (words.Length < icol + 1)
                        continue;
                    oceannamedict.Add(words[0], words[icol]);
                }
            }

            //Console.ReadLine();
#endif
        }

        public static string get_ocean(coordclass ll)
        {
            string returnocean = "NONE";
#if (DBGEOFLAG)

            foreach (string ocean in oceandict.Keys)
            {
                double area = 1e20;
                DbGeography mouth = pointfromlatlong(ll.lat, ll.lon);
                double? dist = oceandict[ocean].Distance(mouth) / 1000;
                if (dist <= 1000)
                    Console.WriteLine(ocean + " " + dist);
                if (dist <= 0)
                {
                    if (oceandict[ocean].Area < area) //if several ocean matches, pick the smaller more specific one
                    {
                        returnocean = ocean;
                        area = (double)oceandict[ocean].Area;
                    }
                }
            }
#endif
            return returnocean;
        }

        public static string make_river(int gnid, Page p)
        {

            string text = "";
#if (DBGEOFLAG)

            if (riverdict.ContainsKey(gnid))
            {
                bool secondary = true;
                if (drainagedict.ContainsKey(riverdict[gnid].drainage_name))
                {
                    string[] p318 = new string[2] { gndict[gnid].Name_ml, makegnidlink(drainagedict[riverdict[gnid].drainage_name].main_river_artname) };
                    if ((riverdict[gnid].tributary_of > 0) && (rivernamedict.ContainsKey(riverdict[gnid].tributary_of)))
                    {
                        if (rivernamedict[riverdict[gnid].tributary_of].is_lake)
                        {
                            string[] p321 = new string[1] { makegnidlink(rivernamedict[riverdict[gnid].tributary_of].river_artname) };
                            text += " " + util.mp(318, p318) + ", " + util.mp(3) + " " + util.mp(321, p321) + ".";
                        }
                        else
                        {
                            string[] p319 = new string[1] { makegnidlink(rivernamedict[riverdict[gnid].tributary_of].river_artname) };
                            text += " " + util.mp(318, p318) + ", " + util.mp(3) + " " + util.mp(319, p319) + ".";
                        }
                    }
                    else //drainage only
                    {
                        text += " " + util.mp(318, p318) + ".";
                        if (drainagedict[riverdict[gnid].drainage_name].main_river == gnid)
                            secondary = false;
                    }
                }
                else
                    secondary = false;
                
                if ( !secondary)
                {
                    if (gndict[gnid].elevation < 10)
                    {
                        string terrain_type = get_terrain_type3(gnid, 3);
                        if (terrain_type.Contains("|ocean "))
                        {
                            string ocean = get_ocean(gnidcoord(gnid));
                            if (oceannamedict.ContainsKey(ocean))
                            {
                                string[] p321 = new string[1] { "[["+oceannamedict[ocean]+"]]" };
                                text += " "+ gndict[gnid].Name_ml + " " + util.mp(321, p321) + ".";
                            }
                        }
                    }
                }
            }
#endif
            return text;
        }
        public static string make_lake(int gnid, Page p)
        {
            string text = "";
            Dictionary<char, int> fc = new Dictionary<char, int>();
            fc.Add('H', 0);
            fc.Add('L', 0);
            fc.Add('A', 0);
            fc.Add('P', 0);
            fc.Add('S', 0);
            fc.Add('T', 0);
            fc.Add('U', 0);
            fc.Add('V', 0);

            if (lakedict.ContainsKey(gnid))
            {
                double lakesize = lakedict[gnid].kmns + lakedict[gnid].kmew;
                double nbradius = 30.0;
                if ((lakesize > 0) && (lakesize < 0.5 * nbradius))
                {
                    int nhalt = 0;
                    int nearhigh = -1;
                    List<int> farlist = getneighbors(gnid, nbradius);

                    double[] nhlatlong = get_nearhigh(gnid, farlist, nbradius, lakesize, out nearhigh, out nhalt);

                    if (nearhigh > 0)
                    {
                        string[] p116 = { makegnidlink(nearhigh), util.fnum(gndict[nearhigh].elevation) };
                        text += " " + util.mp(116, p116) + ", " + util.fnum(get_distance(gnid, nearhigh)) + " " + util.mp(308) + " " + util.mp(100 + get_direction(gnid, nearhigh)) + " " + gndict[gnid].Name_ml + "." + addnote(util.mp(137) + geonameref(gnid));
                    }
                    else if (nhlatlong[0] + nhlatlong[1] < 720.0)
                    {
                        string[] p172 = { util.fnum(nhalt) };
                        text += " " + util.mp(172, p172) + ", " + util.fnum(util.get_distance_latlong(gndict[gnid].latitude, gndict[gnid].longitude, nhlatlong[0], nhlatlong[1])) + " " + util.mp(308) + " " + util.mp(100 + util.get_direction_latlong(gndict[gnid].latitude, gndict[gnid].longitude, nhlatlong[0], nhlatlong[1])) + " " + gndict[gnid].Name_ml + "." + addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));

                    }
                }

                text += make_landcover(gnid);

                string[] p79 = new string[1] { gndict[gnid].Name_ml };

                if (lakedict[gnid].kmns + lakedict[gnid].kmew > 1.0)
                {
                    string[] p164 = { lakedict[gnid].kmns.ToString("F1", culture), lakedict[gnid].kmew.ToString("F1", culture) };
                    text += " " + util.mp(164, p164);

                    if (lakedict[gnid].kmew > 2 * lakedict[gnid].kmns)
                    {
                        p.SetTemplateParameter("geobox", "length", lakedict[gnid].kmew.ToString(), true);
                        p.SetTemplateParameter("geobox", "width", lakedict[gnid].kmns.ToString(), true);
                        p.SetTemplateParameter("geobox", "length_orientation", "EW", true);
                        p.SetTemplateParameter("geobox", "width_orientation", "NS", true);
                    }
                    else if (lakedict[gnid].kmns > 2 * lakedict[gnid].kmew)
                    {
                        p.SetTemplateParameter("geobox", "length", lakedict[gnid].kmns.ToString(), true);
                        p.SetTemplateParameter("geobox", "width", lakedict[gnid].kmew.ToString(), true);
                        p.SetTemplateParameter("geobox", "length_orientation", "NS", true);
                        p.SetTemplateParameter("geobox", "width_orientation", "EW", true);
                    }
                }

                //feature lists:
                if (lakedict[gnid].inlake.Count > 0)
                {
                    text += "\n\n" + util.mp(91, p79) + "\n";
                    foreach (int il in lakedict[gnid].inlake)
                        text += "\n* " + makegnidlink(il) + " (" + linkfeature(gndict[il].featurecode, il) + ")";
                    text += "\n\n";

                }
                if (lakedict[gnid].atlake.Count > 0)
                {
                    foreach (int ff in lakedict[gnid].atlake)
                    {
                        fc[gndict[ff].featureclass]++;
                    }
                    // H: vatten
                    // L: diverse människoskapade områden 
                    // P: samhällen
                    // S: Byggnadsverk
                    // T: diverse naturfenomen; berg, dalar, öar...
                    // U: undervattensfenomen
                    // V: växtlighet; skogar, hedar...

                    SortedDictionary<long, int> flist = new SortedDictionary<long, int>();

                    if (fc['P'] > 0)
                    {
                        text += "\n\n" + util.mp(161, p79) + "\n";
                        foreach (int ff in lakedict[gnid].atlake)
                            if (gndict[ff].featureclass == 'P')
                            {
                                long pop = gndict[ff].population;
                                while (flist.ContainsKey(pop))
                                    pop++;
                                flist.Add(pop, ff);
                            }
                        string sorted = "";
                        foreach (int fpop in flist.Keys)
                        {
                            sorted = "\n* " + makegnidlink(flist[fpop]) + " (" + fpop.ToString("N0", nfi) + " " + util.mp(81) + ")" + sorted;
                        }
                        text += sorted;
                    }

                    SortedDictionary<string, int> fl2 = new SortedDictionary<string, int>();

                    if (fc['H'] + fc['T'] + fc['U'] + fc['V'] > 10)
                    {
                        //public static Dictionary<string, string> categorydict = new Dictionary<string, string>(); //from featurecode to category
                        //public static Dictionary<string, string> parentcategory = new Dictionary<string, string>(); //from category to parent category
                        //public static Dictionary<string, string> categoryml = new Dictionary<string, string>(); //from category to category name in makelang

                        text += "\n\n" + util.mp(162, p79) + "\n";

                        foreach (string cat in categoryml.Keys)
                        {
                            fl2.Clear();
                            foreach (int ff in lakedict[gnid].atlake)
                                if ((gndict[ff].featureclass == 'H') || (gndict[ff].featureclass == 'T') || (gndict[ff].featureclass == 'U') || (gndict[ff].featureclass == 'V'))
                                {
                                    if (categorydict[gndict[ff].featurecode] == cat)
                                    {
                                        string ffname = gndict[ff].Name_ml;
                                        while (fl2.ContainsKey(ffname))
                                            ffname += " ";
                                        fl2.Add(ffname, ff);
                                    }
                                }

                            if (fl2.Count > 0)
                            {
                                text += "\n* " + util.initialcap(categoryml[cat]) + ":\n";
                                string sorted = "";
                                foreach (string fname in fl2.Keys)
                                {
                                    sorted += "\n** " + makegnidlink(fl2[fname]) + " (" + linkfeature(gndict[fl2[fname]].featurecode, fl2[fname]) + ")";
                                }
                                text += sorted;
                            }
                        }

                    }
                    else if (fc['H'] + fc['T'] + fc['U'] + fc['V'] > 0)
                    {
                        fl2.Clear();

                        text += "\n\n" + util.mp(168, p79) + "\n";
                        foreach (int ff in lakedict[gnid].atlake)
                            if ((gndict[ff].featureclass == 'H') || (gndict[ff].featureclass == 'T') || (gndict[ff].featureclass == 'U') || (gndict[ff].featureclass == 'V'))
                            {
                                string ffname = gndict[ff].Name_ml;
                                while (fl2.ContainsKey(ffname))
                                    ffname += " ";
                                fl2.Add(ffname, ff);
                            }
                        string sorted = "";
                        foreach (string fname in fl2.Keys)
                        {
                            sorted += "\n* " + makegnidlink(fl2[fname]) + " (" + linkfeature(gndict[fl2[fname]].featurecode, fl2[fname]) + ")";
                        }
                        text += sorted;
                    }



                }

                string kmlfilename = countryml[makecountryname] + "/" + gndict[gnid].articlename;
                Page pkml = new Page(makesite, kmlprefix + kmlfilename);
                util.tryload(pkml, 1);
                if (pkml.Exists())
                {
                    if (makelang == "sv")
                        kmlfilename = kmlprefix + kmlfilename;
                    kmlfilename = kmlfilename.Replace(" ", "_");
                    text += "\n{{KML|" + util.mp(310) + "=" + kmlfilename + "}}";
                }
            }

            return text;

        }


        public static string geonameref(int gnid)
        {

            //string gn = "[http://www.geonames.org/gnidgnid/asciiascii.html " + gndict[gnid].Name + "] at [http://www.geonames.org/about.html Geonames.Org (cc-by)]; updated "+gndict[gnid].moddate;
            string gn = "[{{" + util.mp(173) + "|gnid=" + gnid.ToString() + "|name=" + gndict[gnid].asciiname.ToLower().Replace(" ", "%20").Replace("\"", "%22") + "}} " + gndict[gnid].Name + "] " + util.mp(293) + " [{{Geonamesabout}} Geonames.org (cc-by)]; post " + util.mp(179) + " " + gndict[gnid].moddate + "; " + util.mp(180) + " " + dumpdate;
            //gn = gn.Replace("gnidgnid", gnid.ToString());
            //gn = gn.Replace("asciiascii", gndict[gnid].asciiname.ToLower().Replace(" ", "%20"));
            return addref("gn" + gnid.ToString(), gn);

        }

        public static string chinapopref()
        {

            string gn = "";
            string url = "http://pan.baidu.com/share/link?uk=2922733136&shareid=2553664090&third=0&adapt=pc&fr=ftw#path=%252F%25E3%2580%258A%25E4%25B8%25AD%25E5%259B%25BD2010%25E5%25B9%25B4%25E4%25BA%25BA%25E5%258F%25A3%25E6%2599%25AE%25E6%259F%25A5%25E5%2588%2586%25E4%25B9%25A1%25E3%2580%2581%25E9%2595%2587%25E3%2580%2581%25E8%25A1%2597%25E9%2581%2593%25E8%25B5%2584%25E6%2596%2599%25E3%2580%258Bxls";
            if (makelang == "sv")
                gn = "{{webbref |url= " + url + "|titel= 中国2010年人口普查分乡、镇、街道资料》xls (Folkräkning 2010 Kina)|hämtdatum=23 april 2016 |efternamn= |förnamn= |datum= |verk= |utgivare= Baidu.com}}";
            else
                gn = "{{Cite web |url= " + url + "|title= 中国2010年人口普查分乡、镇、街道资料》xls (China 2010 census)|access-date = 23 Abril 2016 |publisher= Baidu.com}}";
            //{{Webbref |url= |titel= |hämtdatum= |författare= |efternamn= |förnamn= |författarlänk= |efternamn2= |förnamn2= |datum= |år= |månad= |format= |verk= |utgivare= |sid= |språk= |doi= |arkivurl= |arkivdatum= |citat= |ref= }}
            return addref("chinapop", gn);
        }


        public static string nasaref()
        {

            string gn = "";
            if (makelang == "sv")
                gn = "{{webbref |url= http://neo.sci.gsfc.nasa.gov/dataset_index.php|titel= NASA Earth Observations Data Set Index|hämtdatum=30 januari 2016 |efternamn= |förnamn= |datum= |verk= |utgivare= NASA}} Temperaturuppgifter från satellitmätningar av markytans temperatur inom en ruta som är 0,1×0,1 grader.";
            else
                gn = "{{Cite web |url= http://neo.sci.gsfc.nasa.gov/dataset_index.php|title= NASA Earth Observations Data Set Index|access-date = 30 Enero 2016 |publisher= NASA}}";
            //{{Webbref |url= |titel= |hämtdatum= |författare= |efternamn= |förnamn= |författarlänk= |efternamn2= |förnamn2= |datum= |år= |månad= |format= |verk= |utgivare= |sid= |språk= |doi= |arkivurl= |arkivdatum= |citat= |ref= }}
            return addref("nasa", gn);
        }

        public static string nasapopref()
        {

            string gn = "";
            if (makelang == "sv")
                gn = "{{webbref |url= http://neo.sci.gsfc.nasa.gov/view.php?datasetId=SEDAC_POP|titel= NASA Earth Observations: Population Density|hämtdatum=30 januari 2016 |efternamn= |förnamn= |datum= |verk= |utgivare= NASA/SEDAC}}";
            else
                gn = "{{Cite web |url= http://neo.sci.gsfc.nasa.gov/view.php?datasetId=SEDAC_POP|title= NASA Earth Observations: Population Density|access-date = 30 Enero 2016 |publisher= NASA/SEDAC}}";
            //{{Webbref |url= |titel= |hämtdatum= |författare= |efternamn= |förnamn= |författarlänk= |efternamn2= |förnamn2= |datum= |år= |månad= |format= |verk= |utgivare= |sid= |språk= |doi= |arkivurl= |arkivdatum= |citat= |ref= }}
            return addref("nasapop", gn);
        }

        public static string nasarainref()
        {

            string gn = "";
            if (makelang == "sv")
                gn = "{{webbref |url= http://neo.sci.gsfc.nasa.gov/view.php?datasetId=TRMM_3B43M|titel= NASA Earth Observations: Rainfall (1 month - TRMM)|hämtdatum=30 januari 2016 |efternamn= |förnamn= |datum= |verk= |utgivare= NASA/Tropical Rainfall Monitoring Mission}} Medelvärde för åren 2012–2014 inom en ruta som är 0,1×0,1 grader.";
            else
                gn = "{{Cite web |url= http://neo.sci.gsfc.nasa.gov/view.php?datasetId=TRMM_3B43M|title= NASA Earth Observations: Rainfall (1 month - TRMM)|access-date = 30 Enero 2016 |publisher= NASA/Tropical Rainfall Monitoring Mission}}";
            //{{Webbref |url= |titel= |hämtdatum= |författare= |efternamn= |förnamn= |författarlänk= |efternamn2= |förnamn2= |datum= |år= |månad= |format= |verk= |utgivare= |sid= |språk= |doi= |arkivurl= |arkivdatum= |citat= |ref= }}
            return addref("nasarain", gn);
        }

        public static string nasalandcoverref()
        {

            string gn = "";
            if (makelang == "sv")
                gn = "{{webbref |url= http://neo.sci.gsfc.nasa.gov/view.php?datasetId=MCD12C1_T1|titel= NASA Earth Observations: Land Cover Classification|hämtdatum=30 januari 2016 |efternamn= |förnamn= |datum= |verk= |utgivare= NASA/MODIS}}";
            else
                gn = "{{Cite web |url= http://neo.sci.gsfc.nasa.gov/view.php?datasetId=MCD12C1_T1|title= NASA Earth Observations: Land Cover Classification|access-date = 30 Enero 2016 |publisher= NASA/MODIS}}";
            //{{Webbref |url= |titel= |hämtdatum= |författare= |efternamn= |förnamn= |författarlänk= |efternamn2= |förnamn2= |datum= |år= |månad= |format= |verk= |utgivare= |sid= |språk= |doi= |arkivurl= |arkivdatum= |citat= |ref= }}
            return addref("nasalandcover", gn);
        }

        public static string koppenref()
        {
            string gn = "";
            if (makelang == "sv")
                gn = "{{Tidskriftsref | författare = | redaktör = | rubrik = Updated world map of the Köppen-Geiger climate classification| url = http://www.hydrol-earth-syst-sci.net/11/1633/2007/hess-11-1633-2007.html| år = 2007| tidskrift = Hydrology and Earth System Sciences| volym = 11| utgivningsort = | utgivare = | nummer = | sid = 1633-1644| hämtdatum = 2016-01-30| id = | doi = 10.5194/hess-11-1633-2007| issn = | citat = | språk = | förnamn =M C| förnamn2 = B L| förnamn3 = T A| efternamn = Peel| efternamn2 = Finlayson| efternamn3 =McMahon| ref = }}";
            else
                gn = "{{cite journal |last= Peel|first= M C|last2= Finlayson|first2= B L|date= |title= Updated world map of the Köppen-Geiger climate classification| url = http://www.hydrol-earth-syst-sci.net/11/1633/2007/hess-11-1633-2007.html |journal= Hydrology and Earth System Sciences|publisher= |volume= 11|issue= |pages= 1633-1644|doi= 10.5194/hess-11-1633-2007|access-date=30 Enero 2016}}";
            return addref("koppen", gn);
        }

        public static string wikiref(string iwlang)
        {
            Console.WriteLine("wikiref " + iwlang);

            if ((iwlang == "Wikidata") || (iwlang == "wd"))
                return addref(iwlang, "(" + util.mp(130) + " " + iwlang + DateTime.Now.ToString("yyyy-MM-dd") + ")");
            else
                return addref(iwlang + "wiki", "(" + util.mp(130) + " " + iwlang + "wiki " + wdtime.ToString("yyyy-MM-dd") + ")");

        }

        public static string makegnidlink(int gnid)
        {
            string link = "[[";

            if (!gndict.ContainsKey(gnid))
            {
                Console.WriteLine("Bad gnid in makegnidlink " + gnid.ToString());
                return "";
            }

            string aname = gndict[gnid].articlename;
            if (aname == "XXX")
                return "[[" + gndict[gnid].Name_ml + "]]"; //no article name - return Name_ml linked!

            if (aname.Contains("*"))
                aname = aname.Replace("*", "");

            if (gndict[gnid].Name_ml == aname)
                link += aname;
            else
                link += aname + "|" + gndict[gnid].Name_ml;
            link += "]]";
            return link;
        }

        public static string makegnidlink(string aname)
        {
            string link = "[[";

            if (aname.Contains("*"))
                aname = aname.Replace("*", "");

            string nml = util.remove_disambig(aname);
            if (nml == aname)
                link += aname;
            else
                link += aname + "|" + nml;
            link += "]]";
            return link;

        }

        public static string make_catname(string catcode, string adm, bool countrylevel)
        {
            string catname = "";
            if (countrylevel)
            {
                catname = util.mp(1) + util.initialcap(categoryml[catcode]) + " " + util.mp(75) + " " + adm;
                if ((makelang == "sv") && ((catcode == "geography") || (catcode == "islands")))
                {
                    catname = util.mp(1) + adm + "s " + categoryml[catcode];
                    if (catname.Contains("ss " + categoryml[catcode]))
                        catname = catname.Replace("ss " + categoryml[catcode], "s " + categoryml[catcode]);
                    if (catname.Contains("zs " + categoryml[catcode]))
                        catname = catname.Replace("zs " + categoryml[catcode], "z " + categoryml[catcode]);

                }
            }
            else
            {
                catname = util.mp(1) + util.initialcap(categoryml[catcode]) + " " + util.mp(75) + " " + adm;
            }
            return catname;
        }

        public static void make_x_in_country(string catcode, string countrynameml)
        {
            string catname = make_catname(catcode, countrynameml, true);

            if (donecats.Contains(catname))
                return;
            else
            {
                stats.Adddonecat();
                donecats.Add(catname);
            }
            Page pc = new Page(makesite, catname);
            util.tryload(pc, 1);
            if (pc.Exists())
                return;

            string[] p95 = new string[1] { categoryml[catcode] };

            if (parentcategory[catcode] == "top")
            {
                pc.text = util.mp(120);
                pc.AddToCategory(util.initialcap(util.mp(95, p95)) + "|" + countrynameml);
                pc.AddToCategory(countrynameml);
                util.trysave(pc, 2, makesite.defaultEditComment + " " + util.mp(1));

                Page pc1 = new Page(makesite, util.mp(1) + util.mp(95, p95));
                util.tryload(pc1, 1);
                if (!pc1.Exists())
                {
                    pc1.text = util.mp(120);
                    pc1.AddToCategory(util.initialcap(categoryml[catcode]));
                    util.trysave(pc1, 2, makesite.defaultEditComment + " " + util.mp(1));
                }


            }
            else
            {
                pc.text = util.mp(120);
                pc.AddToCategory(make_catname(parentcategory[catcode], countrynameml, true));
                pc.AddToCategory(util.initialcap(util.mp(95, p95)) + "|" + countrynameml);
                util.trysave(pc, 2, makesite.defaultEditComment + " " + util.mp(1));
                make_x_in_country(parentcategory[catcode], countrynameml);

            }

        }

        public static void make_x_in_adm1(string catcode, int admgnid, string countrynameml)
        {
            string admname = getartname(admgnid);
            string catname = make_catname(catcode, admname, false);
            Console.WriteLine(catname);

            if (donecats.Contains(catname))
                return;
            else
            {
                stats.Adddonecat();
                donecats.Add(catname);
            }

            Page pc = new Page(makesite, catname);
            if (!util.tryload(pc, 1))
                return;
            if (pc.Exists())
                return;

            if (parentcategory[catcode] == "top")
            {
                pc.text = util.mp(120);
                string[] p93 = new string[3] { categoryml[catcode], countrynameml, getadmlabel(makecountry, 1, admgnid) };
                pc.AddToCategory(util.initialcap(util.mp(93, p93)));
                //pc.AddToCategory(getgnidname(admgnid));
                util.trysave(pc, 2, makesite.defaultEditComment + " " + util.mp(1));

                Page pc1 = new Page(makesite, util.mp(1) + util.mp(93, p93));
                util.tryload(pc1, 1);
                if (!pc1.Exists())
                {
                    pc1.text = util.mp(120);
                    pc1.AddToCategory(make_catname(catcode, countrynameml, true));
                    util.trysave(pc1, 2, makesite.defaultEditComment + " " + util.mp(1));
                    Page pc2 = new Page(makesite, make_catname(catcode, countrynameml, true));
                    util.tryload(pc2, 1);
                    if (!pc2.Exists())
                    {
                        pc2.text = util.mp(120);
                        pc2.AddToCategory(countrynameml);
                        util.trysave(pc2, 2, makesite.defaultEditComment + " " + util.mp(1));
                    }
                }


            }
            else
            {
                string parentcat = make_catname(parentcategory[catcode], admname, false);
                Console.WriteLine(parentcat);
                pc.text = util.mp(120);
                pc.AddToCategory(parentcat);
                pc.AddToCategory(make_catname(catcode, countrynameml, true));
                util.trysave(pc, 2, makesite.defaultEditComment + " " + util.mp(1));
                make_x_in_adm1(parentcategory[catcode], admgnid, countrynameml);
                make_x_in_country(catcode, countrynameml);
            }
        }

        public static void merge_refs(Page p, string refstring)
        {
            //Console.WriteLine(refstring);
            if (!p.text.Contains("</references>"))
            {
                Console.WriteLine("no </references>");
                return;
            }

            p.text = p.text.Replace("</references>", refstring.Replace("<references>", "") + "\n</references>");

            //Reference list:

            //if (hasnotes)
            //    p.text += util.mp(175).Replace("XX", "\n\n");

            //reflist += "\n</references>\n\n";
            //p.text += "\n\n== " + util.mp(51) + " ==\n\n" + reflist;


        }

        public static void retrofit_nasa(int gnid)
        {
            Console.WriteLine("============");

            hasnotes = false;

            if (!gndict.ContainsKey(gnid))
            {
                Console.WriteLine("Bad gnid in retrofit_nasa " + gnid.ToString());
                return;
            }

            Console.WriteLine(gndict[gnid].featureclass.ToString() + "." + gndict[gnid].featurecode);

            if (createclass != ' ')
                if (gndict[gnid].featureclass != createclass)
                {
                    Console.WriteLine("Wrong class in retrofit_nasa");
                    return;
                }
            if (createfeature != "")
                if (gndict[gnid].featurecode != createfeature)
                {
                    Console.WriteLine("Wrong feature in retrofit_nasa");
                    return;
                }

            if (createexceptfeature != "")
                if (gndict[gnid].featurecode == createexceptfeature)
                {
                    Console.WriteLine("Wrong feature in retrofit_nasa");
                    return;
                }

            if (createunit > 0)
            {
                bool admfound = false;
                for (int i = 0; i < 5; i++)
                    if (gndict[gnid].adm[i] == createunit)
                        admfound = true;
                if (!admfound)
                {
                    Console.WriteLine("Wrong adm-unit in retrofit_nasa");
                    return;
                }
            }

            if (createexceptunit > 0)
            {
                bool admfound = false;
                for (int i = 0; i < 5; i++)
                    if (gndict[gnid].adm[i] == createexceptunit)
                        admfound = true;
                if (admfound)
                {
                    Console.WriteLine("Wrong adm-unit in retrofit_nasa");
                    return;
                }
            }


            string prefix = testprefix;
            //string maintitle = "";

            if (gndict[gnid].articlename == "XXX")
            {
                Console.WriteLine("No articlename");
                gndict[gnid].articlename = gndict[gnid].Name_ml;
                //return;
            }

            if (String.IsNullOrEmpty(gndict[gnid].articlename))
            {
                Console.WriteLine("Null articlename");
                gndict[gnid].articlename = gndict[gnid].Name_ml;
                return;
            }

            Console.WriteLine("gnid = " + gnid);


            string countryname = "";
            int icountry = -1;
            if (countrydict.ContainsKey(gndict[gnid].adm[0]))
            {
                icountry = gndict[gnid].adm[0];
                countryname = countrydict[gndict[gnid].adm[0]].Name;
                //Console.WriteLine("country name = " + countryname);
                //Console.WriteLine("Native wiki = "+countrydict[icountry].nativewiki);
            }
            else
                Console.WriteLine("Invalid country " + gndict[gnid].adm[0].ToString());

            string countrynameml = countryname;
            if (countryml.ContainsKey(countryname))
                countrynameml = countryml[countryname];

            string cleanname = util.remove_disambig(gndict[gnid].articlename);
            string oldname = gndict[gnid].articlename.Replace("*", "");
            if (!util.is_latin(cleanname))
            {
                string latinname = gndict[gnid].asciiname;
                if ((util.get_alphabet(cleanname) == "cyrillic") && (makelang == "sv"))
                {
                    latinname = cyrillic.Transliterate(cleanname, countrydict[icountry].nativewiki);
                }
                gndict[gnid].articlename = gndict[gnid].articlename.Replace(cleanname, latinname);
                if (!util.is_latin(gndict[gnid].Name_ml))
                    gndict[gnid].Name_ml = latinname;
            }

            Page p = new Page(makesite, prefix + getartname(gnid));

            util.tryload(p, 3);

            string origtext = "";

            if (p.Exists())
            {
                if (!p.text.Contains(util.mp(195)))
                {
                    Console.WriteLine("Not botmade");
                    return;
                }

                if (!p.text.Contains(gnid.ToString()))
                {
                    Console.WriteLine("Wrong gnid in old article");
                    return;
                }

                if (p.text.Contains("NASA Earth"))
                {
                    Console.WriteLine("Already done");
                    return;
                }


                origtext = p.text;

                string mp117 = util.mp(117);
                string mp118 = util.mp(118);
                string mp119 = util.mp(119);

                string mpop = make_popdensity(gnid).Trim();
                bool popdone = false;
                if (!String.IsNullOrEmpty(mpop))
                {
                    if (p.text.Contains(mp117))
                    {
                        p.text = p.text.Replace(mp117, mpop);
                        popdone = true;
                    }
                    else if (p.text.Contains(mp118))
                    {
                        p.text = p.text.Replace(mp118, mpop);
                        popdone = true;
                    }
                    else if (p.text.Contains(mp119))
                    {
                        p.text = p.text.Replace(mp119, mpop);
                        popdone = true;
                    }

                }

                string climate = make_climate(gnid);
                string lc = make_landcover(gnid);

                string total = lc;
                if (!popdone && !String.IsNullOrEmpty(mpop))
                {
                    if (!String.IsNullOrEmpty(total))
                        total += " ";
                    total += mpop;
                }
                if (!String.IsNullOrEmpty(climate))
                {
                    if (!String.IsNullOrEmpty(total))
                        total += " ";
                    total += climate;
                }

                total = total.Trim();

                string mp175 = util.mp(175).Replace("XX", "").Substring(0, 11);

                string replacekey = "XXXX";
                if (p.text.Contains(util.mp(51)))
                    replacekey = "== " + util.mp(51);
                if (p.text.Contains(mp175))
                    replacekey = mp175;

                Console.WriteLine("replacekey = " + replacekey);

                p.text = p.text.Replace(replacekey, total + "\n\n" + replacekey);

                merge_refs(p, reflist);

                p.text = p.text.Replace("Mer om algoritmen finns här: [[Användare:Lsjbot/Algoritmer]].", "{{Lsjbot-algoritmnot}}").Replace("\n\n\n", "\n\n");
                p.text = p.text.Replace("at [{{Geonames", util.mp(293) + " [{{Geonames");

                //Reference list:

                //if (hasnotes)
                //    p.text += util.mp(175).Replace("XX", "\n\n");

                //reflist += "\n</references>\n\n";
                //p.text += "\n\n== " + util.mp(51) + " ==\n\n" + reflist;

                if (p.text != origtext)
                    util.trysave(p, 3);
            }
        }


        public static void retrofit_nasa()
        {
            makesite.defaultEditComment = util.mp(219) + " " + countryml[makecountryname];

            int iremain = gndict.Count;
            int iremain0 = iremain;

            foreach (int gnid in gndict.Keys)
            {
                iremain--;
                if ((resume_at > 0) && (resume_at != gnid))
                {
                    stats.Addskip();
                    continue;
                }
                else
                    resume_at = -1;

                if (stop_at == gnid)
                    break;

                reflist = "<references>";
                refnamelist.Clear();

                retrofit_nasa(gnid);

                Console.WriteLine(iremain.ToString() + " remaining.");

                if (firstround && (iremain0 - iremain < 5))
                {
                    Console.WriteLine("<cr>");
                    Console.ReadLine();
                }
            }

        }

        public static void test_nasa()
        {
            foreach (int gnid in gndict.Keys)
                make_climate(gnid);
            foreach (string s in climatemismatchdict.Keys)
                Console.WriteLine(s + ": " + climatemismatchdict[s]);

            while (true)
            {
                Console.WriteLine("Latitude:");
                double lat = util.tryconvertdouble(Console.ReadLine());
                Console.WriteLine("Longitude:");
                double lon = util.tryconvertdouble(Console.ReadLine());

                Console.WriteLine("Climate: " + make_climate(lat, lon));
                Console.WriteLine("Landcover: " + make_landcover(lat, lon));
                Console.WriteLine("Pop density: " + make_popdensity(lat, lon));
            }
        }

        public static string make_climate_chart(nasaclass nc, string name)
        {
            string s = util.mp(220);
            if (String.IsNullOrEmpty(s))
                return s;

            s = "{| border=\"1\"\n" + s + "\n|  " + name + "\n";
            int rainmax = 0;
            bool validrain = true;
            bool zerosuppress = (makelang == "sv");

            for (int i = 1; i < 13; i++)
            {
                if ((nc.month_temp_day[i] < -100) || (nc.month_temp_night[i] < -100))
                {
                    Console.WriteLine("Invalid temperature data in make_climate_chart");
                    return "";
                }
                else if (nc.month_rain[i] < 0)
                    validrain = false;
            }
            for (int i = 1; i < 13; i++)
            {
                s += "| " + nc.month_temp_night[i].ToString(culture_en) + "| " + nc.month_temp_day[i].ToString(culture_en);
                if ((validrain) || (zerosuppress && (nc.month_rain[i] >= 0)))
                    s += "| " + nc.month_rain[i].ToString() + "\n";
                else if (zerosuppress)
                    s += "|\n";
                else
                    s += "| 0\n";
                if (nc.month_rain[i] > rainmax)
                    rainmax = nc.month_rain[i];
            }
            if (rainmax > 750)
                s += "|maxprecip = " + rainmax.ToString() + "\n";

            s += "|float=left\n|clear=left\n|source = " + nasaref() + "\n}}\n|}";


            //|maxprecip = <!--- supply largest monthly precipitation, in case it's > 750 mm (30 in.) --->
            //|float     = <!--- left, right, or none --->
            //|clear     = <!--- left, right, both, or none --->
            //|units     = <!--- set to "imperial" if the values are in °F and inches --->
            //|source    = <!--- source of the data --->
            //}}
            return s;

        }

        public static void read_nasa()
        {
            string fname = geonameclass.geonamesfolder + @"nasa.txt";
            Console.WriteLine("read_nasa " + fname);
            if (!File.Exists(fname))
                return;

            //if (stats)
            //{
            //    pophist.SetBins(0.0, 10000.0, 100);
            //    rainhist.SetBins(0.0, 10000.0, 100);
            //    rainminmaxhist.SetBins(0.0, 2000.0, 20);
            //    daynighthist.SetBins(-20.0, 40.0, 30);
            //}

            using (StreamReader sr = new StreamReader(fname))
            {
                int n = 0;

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();

                    //if (n > 250)
                    //    Console.WriteLine(line);

                    string[] words = line.Split('\t');

                    //foreach (string s in words)
                    //    Console.WriteLine(s);

                    //Console.WriteLine(words[0] + "|" + words[1]);

                    //    public static Dictionary<string,Dictionary<string,int>> adm1dict = new Dictionary<string,Dictionary<string,int>>();

                    int code = util.tryconvert(words[0]);
                    //Console.WriteLine("code = " + code);
                    if (code < 0)
                        continue;

                    n++;

                    nasaclass nc = new nasaclass();
                    //public class nasaclass
                    //{
                    //    public int landcover = -1; //Landcover code 1-17 page 66 http://eospso.nasa.gov/sites/default/files/atbd/atbd_mod12.pdf
                    //    public int popdensity = -1; //people per square km
                    //    public int temp_average = -999; //average across months and day-night
                    //    public int temp_max = -999; //temp of hottest month
                    //    public int month_max = -999; //hottest month (1-12)
                    //    public int temp_min = -999; //temp of coldest month
                    //    public int month_min = -999; //coldest month
                    //    public int temp_daynight = -999; //average difference between day and night
                    //    public int rainfall = -999; //mm per year
                    //}

                    nc.landcover = util.tryconvert(words[1]);
                    nc.popdensity = util.tryconvert(words[2]);
                    nc.temp_average = util.tryconvert(words[3]);
                    nc.temp_max = util.tryconvert(words[4]);
                    nc.month_max = util.tryconvert(words[5]);
                    nc.temp_min = util.tryconvert(words[6]);
                    nc.month_min = util.tryconvert(words[7]);
                    nc.temp_daynight = util.tryconvert(words[8]);
                    nc.rainfall = util.tryconvert(words[9]);
                    nc.rain_max = util.tryconvert(words[10]);
                    nc.rain_month_max = util.tryconvert(words[11]);
                    nc.rain_min = util.tryconvert(words[12]);
                    nc.rain_month_min = util.tryconvert(words[13]);
                    nc.koppen = util.tryconvert(words[14]);
                    if (words.Length > 15)
                    {
                        for (int i = 0; i < 13; i++)
                            nc.month_temp_day[i] = util.tryconvert(words[15 + i]);
                        for (int i = 0; i < 13; i++)
                            nc.month_temp_night[i] = util.tryconvert(words[28 + i]);
                        for (int i = 0; i < 13; i++)
                            nc.month_rain[i] = util.tryconvert(words[41 + i]);
                    }

                    int ndaynight = 0;
                    double daynightdiff = 0;
                    for (int i = 1; i < 13; i++) //remove zeroes that might be invalid
                    {
                        //if ( Math.Abs(nc.month_temp_day[i]) < 0.01 )
                        //    nc.month_temp_day[i] = -999;
                        //if (Math.Abs(nc.month_temp_night[i]) < 0.01)
                        //    nc.month_temp_night[i] = -999;
                        if ((nc.month_temp_day[i] > -100) && (nc.month_temp_night[i] > -100) && (Math.Abs(nc.month_temp_day[i]) > 0.01) && (Math.Abs(nc.month_temp_night[i]) > 0.01))
                        {
                            daynightdiff += nc.month_temp_day[i] - nc.month_temp_night[i];
                            ndaynight++;
                        }
                    }
                    if (ndaynight > 0)
                    {
                        nc.temp_daynight = Convert.ToInt32(daynightdiff / (1.0 * ndaynight));

                        if (ndaynight < 12)
                        {
                            for (int i = 1; i < 13; i++) //remove zeroes that might be invalid, if expected temp outside +-2
                            {
                                if (Math.Abs(nc.month_temp_day[i]) < 0.01)
                                {
                                    if (nc.month_temp_night[i] > -100)
                                    {
                                        double expected = nc.month_temp_night[i] + nc.temp_daynight;
                                        if (Math.Abs(expected) > 2)
                                            nc.month_temp_day[i] = -999;
                                    }
                                    else
                                        nc.month_temp_day[i] = -999;
                                }
                                if (Math.Abs(nc.month_temp_night[i]) < 0.01)
                                {
                                    if (nc.month_temp_day[i] > -100)
                                    {
                                        double expected = nc.month_temp_day[i] - nc.temp_daynight;
                                        if (Math.Abs(expected) > 2)
                                            nc.month_temp_night[i] = -999;
                                    }
                                    else
                                        nc.month_temp_night[i] = -999;
                                }
                            }

                        }
                    }

                    int ntemp = 0;
                    double tempsum = 0;

                    nc.temp_max = -999;
                    nc.month_max = -1;
                    nc.temp_min = 999;
                    nc.month_min = -1;

                    for (int i = 1; i < 13; i++) //estimate missing values
                    {
                        if ((nc.month_temp_day[i] > -100) && (nc.month_temp_night[i] > -100))
                        {
                            tempsum += nc.month_temp_day[i];
                            tempsum += nc.month_temp_night[i];
                            ntemp += 2;
                        }
                        else if (nc.month_temp_day[i] > -100)
                        {
                            tempsum += nc.month_temp_day[i];
                            tempsum += nc.month_temp_day[i] - nc.temp_daynight;
                            ntemp += 2;
                        }
                        else if (nc.month_temp_night[i] > -100)
                        {
                            tempsum += nc.month_temp_night[i];
                            tempsum += nc.month_temp_night[i] + nc.temp_daynight;
                            ntemp += 2;
                        }
                    }

                    if (ntemp > 12)
                        nc.temp_average = Convert.ToInt32(tempsum / (1.0 * ntemp));

                    if (ntemp > 20)
                        for (int i = 1; i < 13; i++) //estimate missing values
                        {
                            double tmean = 0.5 * (nc.month_temp_day[i] + nc.month_temp_night[i]);
                            if (tmean > -100)
                            {
                                if (tmean > nc.temp_max)
                                {
                                    nc.temp_max = Convert.ToInt32(tmean);
                                    nc.month_max = i;
                                }
                                if (tmean < nc.temp_min)
                                {
                                    nc.temp_min = Convert.ToInt32(tmean);
                                    nc.month_min = i;
                                }
                            }
                        }

                    nasadict.Add(code, nc);

                    //if (stats)
                    //{
                    //    pophist.Add(Convert.ToDouble(nc.popdensity));
                    //    rainhist.Add(Convert.ToDouble(nc.rainfall));
                    //    rainminmaxhist.Add(Convert.ToDouble(nc.rain_max - nc.rain_min));
                    //    daynighthist.Add(Convert.ToDouble(nc.temp_daynight));
                    //}
                }
                Console.WriteLine("readnasa " + n);

                //if (stats)
                //{
                //    Console.WriteLine("Pophist:");
                //    pophist.PrintDHist();
                //    Console.ReadLine();
                //    Console.WriteLine("Rainhist:");
                //    rainhist.PrintDHist();
                //    Console.ReadLine();
                //    Console.WriteLine("Rainminmaxhist:");
                //    rainminmaxhist.PrintDHist();
                //    Console.ReadLine();
                //    Console.WriteLine("Daynighthist:");
                //    daynighthist.PrintDHist();
                //    Console.ReadLine();
                //}
            }
        }


        public static string make_popdensity(int gnid)
        {
            if (gndict.ContainsKey(gnid))
                return " " + make_popdensity(gndict[gnid].latitude, gndict[gnid].longitude).Replace("XXX", gndict[gnid].Name_ml);
            else
                return "";
        }

        public static string make_popdensity(double lat, double lon)
        {
            int row = Convert.ToInt32(10 * (90 - lat));
            int col = Convert.ToInt32(10 * (180 + lon));
            int code = 10000 * row + col;

            if (!nasadict.ContainsKey(code))
                return "";

            if (nasadict[code].popdensity >= 0)
            {
                if (nasadict[code].popdensity < 2)
                    return " " + util.mp(239) + nasapopref();
                else
                {
                    int[] poplevels = { 7, 20, 50, 250, 1000, 99999 };
                    int i = 0;
                    while (poplevels[i] < nasadict[code].popdensity)
                        i++;
                    string[] p238 = new string[] { util.mp(240 + i), util.fnum(nasadict[code].popdensity) };
                    return util.mp(238, p238) + nasapopref();
                }
            }

            return "";
        }

        public static string make_landcover(int gnid)
        {
            if (gndict.ContainsKey(gnid))
                return " " + make_landcover(gndict[gnid].latitude, gndict[gnid].longitude).Replace("XXX", gndict[gnid].Name_ml);
            else
                return "";
        }

        public static string make_landcover(double lat, double lon)
        {
            int row = Convert.ToInt32(10 * (90 - lat));
            int col = Convert.ToInt32(10 * (180 + lon));
            int code = 10000 * row + col;

            if (!nasadict.ContainsKey(code))
                return "";

            if ((nasadict[code].landcover > 0) && (nasadict[code].landcover <= 17))
            {
                return util.mp(220 + nasadict[code].landcover) + nasalandcoverref();
            }

            return "";
        }

        public static string make_climate(int gnid)
        {
            if (gndict.ContainsKey(gnid))
                return " " + make_climate(gndict[gnid].latitude, gndict[gnid].longitude, gndict[gnid].elevation, gndict[gnid].Name_ml).Replace("XXX", gndict[gnid].Name_ml);
            else
                return "";
        }

        public static string make_climate(double lat, double lon)
        {
            return make_climate(lat, lon, 0, "");
        }

        public static string make_climate(double lat, double lon, int altitude, string name)
        {
            Dictionary<int, string> koppendict = new Dictionary<int, string>();
            koppendict.Add(1, "rainforest");
            koppendict.Add(2, "monsoon");
            koppendict.Add(3, "savanna");
            koppendict.Add(4, "desert hot");
            koppendict.Add(5, "desert cold");
            koppendict.Add(6, "steppe hot");
            koppendict.Add(7, "steppe cold");
            koppendict.Add(8, "mediterranean");
            koppendict.Add(9, "mediterranean");
            koppendict.Add(10, "subalpine");
            koppendict.Add(11, "humid subtropical");
            koppendict.Add(12, "Cwb");
            koppendict.Add(13, "Cwc");
            koppendict.Add(14, "humid subtropical");
            koppendict.Add(15, "oceanic");
            koppendict.Add(16, "Cfc");
            koppendict.Add(17, "continental");
            koppendict.Add(18, "hemiboreal");
            koppendict.Add(19, "boreal");
            koppendict.Add(20, "continental subarctic");
            koppendict.Add(21, "continental");
            koppendict.Add(22, "hemiboreal");
            koppendict.Add(23, "boreal");
            koppendict.Add(24, "continental subarctic");
            koppendict.Add(25, "continental");
            koppendict.Add(26, "hemiboreal");
            koppendict.Add(27, "boreal");
            koppendict.Add(28, "continental subarctic");
            koppendict.Add(29, "tundra");
            koppendict.Add(30, "arctic");

            string s = "";
            //string dummyname = "XXX";

            int row = Convert.ToInt32(10 * (90 - lat));
            int col = Convert.ToInt32(10 * (180 + lon));
            int code = 10000 * row + col;

            Console.WriteLine("lat,lon = " + lat + ", " + lon);
            Console.WriteLine("row,col,code = " + row + ", " + col + ", " + code);

            if (!nasadict.ContainsKey(code))
                return "";

            string climate = "unknown";
            string koppenclimate = "";
            Console.WriteLine("koppen = " + nasadict[code].koppen);
            if (nasadict[code].koppen > 0)
                koppenclimate = koppendict[nasadict[code].koppen];

            if (koppenclimate.Contains("Cw"))
            {
                if (Math.Abs(lat) < 30)
                    koppenclimate = "tropical highland";
                else
                    koppenclimate = "oceanic";
            }

            if (koppenclimate == "Cfc")
            {
                if (Math.Abs(lat) > 60)
                    koppenclimate = "subarctic oceanic";
                else
                    koppenclimate = "oceanic";
            }

            if (koppenclimate == "tundra")
                if (nasadict[code].temp_max > 13)
                    koppenclimate = "unknown";


            if (koppenclimate == "subalpine")
                if (altitude < 500)
                    koppenclimate = "unknown";

            //{{cite journal | author=Peel, M. C. and Finlayson, B. L. and McMahon, T. A. | year=2007 | title= Updated world map of the Köppen-Geiger climate classification | journal=Hydrol. Earth Syst. Sci. | volume=11 | pages=1633-1644 | url=http://www.hydrol-earth-syst-sci.net/11/1633/2007/hess-11-1633-2007.html | issn = 1027-5606}}

            if (nasadict[code].rainfall >= 0)
            {
                if (nasadict[code].temp_average > -100)
                {
                    if (nasadict[code].temp_min >= 18) //Tropical
                    {
                        if (nasadict[code].rain_min > 60)
                            climate = "rainforest";
                        else if (nasadict[code].rain_min > 100 - nasadict[code].rainfall / 25)
                            climate = "monsoon";
                        else
                        {
                            int potevapo = 20 * nasadict[code].temp_average;
                            if (Math.Abs(nasadict[code].rain_month_max - nasadict[code].month_max) <= 5)
                                potevapo += 280 - 56 * Math.Abs(nasadict[code].rain_month_max - nasadict[code].month_max);
                            if (nasadict[code].rainfall < 0.5 * potevapo)
                                climate = "desert";
                            else if (nasadict[code].rainfall < potevapo)
                                climate = "steppe";
                            else
                            {
                                climate = "savanna";
                            }
                        }

                    }
                    else if (nasadict[code].temp_max < 0) //Arctic
                        climate = "arctic";
                    else if (nasadict[code].temp_max < 10) //Tundra
                        climate = "tundra";
                    else
                    {
                        int potevapo = 20 * nasadict[code].temp_average;
                        if (Math.Abs(nasadict[code].rain_month_max - nasadict[code].month_max) <= 5)
                            potevapo += 280 - 56 * Math.Abs(nasadict[code].rain_month_max - nasadict[code].month_max);
                        if (nasadict[code].rainfall < 0.5 * potevapo)
                            climate = "desert";
                        else if (nasadict[code].rainfall < potevapo)
                            climate = "steppe";
                        else
                        {
                            if (nasadict[code].temp_min > -3)
                                climate = "temperate";
                            else
                                climate = "continental";
                        }
                    }
                }
                else if (String.IsNullOrEmpty(koppenclimate)) //temperature and Köppen unknown
                {
                    int[] rainlevels = { 300, 1000, 99999 };
                    int i = 0;
                    while (rainlevels[i] < nasadict[code].rainfall)
                        i++;
                    //string[] p238 = new string[] { util.mp(240 + i), util.fnum(nasadict[code].popdensity) };
                    //s += " " + util.mp(238, p238);
                    if (i == 0)
                        climate = "dry";
                    else if (i >= 2)
                        climate = "wet";

                }
            }
            else if (nasadict[code].temp_average > -100)
            {
                if (nasadict[code].temp_min >= 18) //Tropical
                    climate = "tropical";
                else if (nasadict[code].temp_max < 0) //Arctic
                    climate = "arctic";
                else if (nasadict[code].temp_max < 10) //Tundra
                    climate = "tundra";
                else
                {
                    if (nasadict[code].temp_min > -3)
                        climate = "temperate";
                    else
                        climate = "continental";
                }

            }

            Console.WriteLine("koppen, climate 1 = " + koppenclimate + ", " + climate);

            if (!String.IsNullOrEmpty(koppenclimate)) //check consistency
            {
                if (koppenclimate.Contains(climate))
                {
                    climate = koppenclimate;
                }
                else if (climate == "unknown")
                {
                    climate = koppenclimate;
                }
                else
                {
                    if (climate == "temperate")
                    {
                        if (((nasadict[code].koppen == 5) || (nasadict[code].koppen >= 7)) && (nasadict[code].koppen <= 28))
                            climate = koppenclimate;
                        else
                        {
                            Console.WriteLine("Koppen = " + koppenclimate + ", climate = " + climate);
                            string kc = koppenclimate + " - " + climate;
                            if (!climatemismatchdict.ContainsKey(kc))
                                climatemismatchdict.Add(kc, 0);
                            climatemismatchdict[kc]++;

                            climate = "unknown";
                        }
                    }
                    else if (climate == "continental")
                    {
                        if ((nasadict[code].koppen == 5) || ((nasadict[code].koppen >= 7) && (nasadict[code].koppen <= 10)) || ((nasadict[code].koppen >= 15) && (nasadict[code].koppen <= 29)))
                            climate = koppenclimate;
                        else
                        {
                            Console.WriteLine("Koppen = " + koppenclimate + ", climate = " + climate);
                            string kc = koppenclimate + " - " + climate;
                            if (!climatemismatchdict.ContainsKey(kc))
                                climatemismatchdict.Add(kc, 0);
                            climatemismatchdict[kc]++;
                            climate = "unknown";
                        }
                    }
                    else if ((climate == "desert") || (climate == "steppe"))
                    {
                        if (koppenclimate.Contains("desert") || koppenclimate.Contains("steppe"))
                            climate = koppenclimate;
                        else if ((koppenclimate.Contains("cold")) && ((climate == "tundra") || (climate == "continental")))
                            climate = koppenclimate;
                        else if (koppenclimate == "mediterranean")
                            climate = koppenclimate;
                        else
                        {
                            Console.WriteLine("Koppen = " + koppenclimate + ", climate = " + climate);
                            string kc = koppenclimate + " - " + climate;
                            if (!climatemismatchdict.ContainsKey(kc))
                                climatemismatchdict.Add(kc, 0);
                            climatemismatchdict[kc]++;
                            climate = "unknown";
                        }
                    }
                    else if ((climate == "tropical") || (climate == "rainforest") || (climate == "monsoon"))
                    {
                        if (koppenclimate.Contains("tropical") || koppenclimate.Contains("rainforest") || koppenclimate.Contains("monsoon") || koppenclimate.Contains("savanna"))
                            climate = koppenclimate;
                        else
                        {
                            Console.WriteLine("Koppen = " + koppenclimate + ", climate = " + climate);
                            string kc = koppenclimate + " - " + climate;
                            if (!climatemismatchdict.ContainsKey(kc))
                                climatemismatchdict.Add(kc, 0);
                            climatemismatchdict[kc]++;
                            climate = "unknown";
                        }
                    }
                    else if (climate == "savanna")
                    {
                        if ((koppenclimate == "steppe hot") || (koppenclimate == "mediterranean") || (koppenclimate == "monsoon") || (koppenclimate == "humid subtropical"))
                            climate = koppenclimate;
                        else
                        {
                            Console.WriteLine("Koppen = " + koppenclimate + ", climate = " + climate);
                            string kc = koppenclimate + " - " + climate;
                            if (!climatemismatchdict.ContainsKey(kc))
                                climatemismatchdict.Add(kc, 0);
                            climatemismatchdict[kc]++;
                            climate = "unknown";
                        }
                    }
                    else if (climate == "tundra")
                    {
                        if (koppenclimate.Contains("boreal") || koppenclimate.Contains("suba"))
                            climate = koppenclimate;
                        else
                        {
                            Console.WriteLine("Koppen = " + koppenclimate + ", climate = " + climate);
                            string kc = koppenclimate + " - " + climate;
                            if (!climatemismatchdict.ContainsKey(kc))
                                climatemismatchdict.Add(kc, 0);
                            climatemismatchdict[kc]++;
                            climate = "unknown";
                        }
                    }
                    else if ((climate == "dry") || (climate == "wet"))
                        climate = koppenclimate;
                    else
                    {
                        Console.WriteLine("Koppen = " + koppenclimate + ", climate = " + climate);
                        string kc = koppenclimate + " - " + climate;
                        if (!climatemismatchdict.ContainsKey(kc))
                            climatemismatchdict.Add(kc, 0);
                        climatemismatchdict[kc]++;
                        climate = "unknown";
                    }
                }
            }

            Console.WriteLine("koppen, climate 2 = " + koppenclimate + ", " + climate);


            if (climate != "unknown")
            {

                switch (climate)
                {
                    case "rainforest":
                        s = util.mp(250);
                        break;
                    case "monsoon":
                        s = util.mp(251);
                        break;
                    case "savanna":
                        s = util.mp(252);
                        break;
                    case "desert":
                        s = util.mp(253);
                        break;
                    case "desert hot":
                        s = util.mp(254);
                        break;
                    case "desert cold":
                        s = util.mp(255);
                        break;
                    case "steppe":
                        s = util.mp(256);
                        break;
                    case "steppe hot":
                        s = util.mp(257);
                        break;
                    case "steppe cold":
                        s = util.mp(258);
                        break;
                    case "mediterranean":
                        s = util.mp(259);
                        break;
                    case "subalpine":
                        s = util.mp(260);
                        break;
                    case "humid subtropical":
                        s = util.mp(261);
                        break;
                    case "oceanic":
                        s = util.mp(262);
                        break;
                    case "subarctic oceanic":
                        s = util.mp(263);
                        break;
                    case "tropical highland":
                        s = util.mp(264);
                        break;
                    case "continental":
                        s = util.mp(265);
                        break;
                    case "hemiboreal":
                        s = util.mp(266);
                        break;
                    case "boreal":
                        s = util.mp(267);
                        break;
                    case "continental subarctic":
                        s = util.mp(268);
                        break;
                    case "tundra":
                        s = util.mp(269);
                        break;
                    case "arctic":
                        s = util.mp(270);
                        break;
                    case "tropical":
                        s = util.mp(271);
                        break;
                    case "dry":
                        s = util.mp(272);
                        break;
                    case "wet":
                        s = util.mp(273);
                        break;
                    case "temperate":
                        s = util.mp(274);
                        break;
                    default:
                        break;
                }
                if (climate == koppenclimate)
                    s += koppenref();

            }


            if (nasadict[code].temp_average > -100)
            {
                string[] p248 = new string[] { util.fnum(nasadict[code].temp_average) };
                s += " " + util.mp(248, p248);
                if (nasadict[code].month_max > 0)
                {
                    string[] p249 = new string[] { util.mp(280 + nasadict[code].month_max), util.fnum(nasadict[code].temp_max), util.mp(280 + nasadict[code].month_min), util.fnum(nasadict[code].temp_min) };
                    s += " " + util.mp(249, p249);
                }
                s += nasaref();

            }

            if (nasadict[code].rainfall >= 0)
            {
                string[] p246 = new string[] { util.fnum(nasadict[code].rainfall) };
                s += " " + util.mp(246, p246);
                if (nasadict[code].rain_month_max > 0)
                {
                    string[] p247 = new string[] { util.mp(280 + nasadict[code].rain_month_max), util.fnum(nasadict[code].rain_max), util.mp(280 + nasadict[code].rain_month_min), util.fnum(nasadict[code].rain_min) };
                    s += " " + util.mp(247, p247);
                }
                s += nasarainref();
            }


            if (nasadict[code].temp_average > -100)
            {
                s += "\n\n" + make_climate_chart(nasadict[code], name);
            }


            return s.Trim();
        }

        public static double city_radius(long population)
        {
            double radius = 1.5 + 0.004 * Math.Sqrt(population);

            return radius;
        }

        public static string from_capital(int gnid)
        {
            string fromcapital = ""; ;
            int capitalgnid = countrydict[gndict[gnid].adm[0]].capital_gnid;

            if (capitalgnid == gnid) //capital itself is not far from capital :)
                return fromcapital;

            if (nocapital.Contains(makecountry))
                return fromcapital;

            if (gndict.ContainsKey(capitalgnid))
            {
                double dist = get_distance(gnid, capitalgnid);
                double mindistcapital = city_radius(gndict[capitalgnid].population);

                if (dist > mindistcapital)
                {
                    int intdist = Convert.ToInt32(dist);
                    if (intdist > 300)
                    {
                        intdist = 100 * Convert.ToInt32(0.01 * dist);
                    }
                    else if (intdist > 30)
                    {
                        intdist = 10 * Convert.ToInt32(0.1 * dist);
                    }

                    fromcapital = ", " + util.fnum(intdist) + " " + util.mp(308) + " " + util.mp(100 + get_direction(capitalgnid, gnid)) + " " + util.mp(132) + " " + makegnidlink(capitalgnid);
                }
                else //coinciding with capital location
                {
                    if (gndict[gnid].featureclass == 'A')
                    {
                        fromcapital = ". " + util.initialcap(util.mp(132)) + " " + makegnidlink(capitalgnid) + " " + util.mp(77) + " " + gndict[gnid].Name_ml;
                    }
                    else if (featurepointdict[gndict[gnid].featurecode])
                    {
                        fromcapital = ", " + util.mp(198) + " " + util.mp(132) + " " + makegnidlink(capitalgnid);
                    }

                }
            }
            return fromcapital;

        }

        public static string getmonthstring()
        {
            DateTime thismonth = DateTime.Now;
            string monthstring = thismonth.Month.ToString();
            while (monthstring.Length < 2)
                monthstring = "0" + monthstring;
            return thismonth.Year.ToString() + "-" + monthstring;
        }

        public static void fill_wdid_buffer()
        {
            int ndone = 0;

            read_rdf_tree();

            foreach (int gnid in gndict.Keys)
            {
                if ((resume_at_wdid > 0) && (resume_at_wdid != gnid))
                {
                    continue;
                }
                else
                    resume_at_wdid = -1;

                if (!valid_article_type(gnid))
                    continue;

                int wdid = gndict[gnid].wdid;

                Console.WriteLine("wdid = " + wdid);

                if (wdid <= 0)
                {
                    //wdid = wdtreeclass.get_wd_item_direct(gnid);
                    wdid = wdtreeclass.get_wdid_by_name(makewiki + "wiki", gndict[gnid].artname2, gnid);
                    if (threadstop) //set to true when thread should abort
                        break;

                    Console.WriteLine("THREAD: gnid,wdid: " + gnid.ToString() + ", " + wdid.ToString());
                    if (!wdtreeclass.check_wd_instance(gnid, wdid))
                    {
                        Console.WriteLine("THREAD: bad instance");
                        wdid = -1;
                    }
                }

                if (gndict[gnid].wdid > 0)
                {
                    if (!wdtreeclass.check_wd_instance(gnid, gndict[gnid].wdid))
                    {
                        Console.WriteLine("THREAD: bad instance");
                        wdid = -2;
                    }

                }
                if (!wdid_buffer.ContainsKey(gnid))
                    wdid_buffer.Add(gnid, wdid);

                if (threadstop) //set to true when thread should abort
                    break;

                ndone++;
                if (ndone > threadmax)
                    break;
            }

            Console.WriteLine("End of fill_wdid_buffer");
            threadrunning = false;
        }

        public static bool valid_article_type(int gnid)
        {

            if (createclass != ' ')
                if (gndict[gnid].featureclass != createclass)
                {
                    Console.WriteLine("Wrong class in valid_article_type");
                    return false;
                }

            if (createexceptclass != ' ')
                if (gndict[gnid].featureclass == createclass)
                {
                    Console.WriteLine("Wrong class in valid_article_type");
                    return false;
                }


            if (createfeature != "")
                foreach (string cf in createfeature.Split(','))
                {
                    bool found = false;
                    if (gndict[gnid].featurecode == cf)
                    {
                        found = true;
                    }
                    if (!found)
                    {
                        Console.WriteLine("Wrong feature in valid_article_type");
                        return false;
                    }
                }

            if (createexceptfeature != "")
                foreach (string cf in createexceptfeature.Split(','))
                    if (gndict[gnid].featurecode == cf)
                    {
                        Console.WriteLine("Wrong feature in valid_article_type");
                        return false;
                    }

            if (createcategory != "")
                foreach (string cf in createcategory.Split(','))
                {
                    bool found = false;
                    if (categorydict[gndict[gnid].featurecode] == cf)
                    {
                        found = true;
                    }
                    if (!found)
                    {
                        Console.WriteLine("Wrong category in valid_article_type");
                        return false;
                    }
                }

            if (createexceptcategory != "")
                foreach (string cf in createexceptcategory.Split(','))
                    if (categorydict[gndict[gnid].featurecode] == cf)
                    {
                        Console.WriteLine("Wrong category in valid_article_type");
                        return false;
                    }



            if ((gndict[gnid].featureclass == 'A') && (gndict[gnid].featurecode.Length > 3) && (gndict[gnid].featurecode.Contains("ADM"))) //check so ADMx supported
            {
                int iadm = util.tryconvert(gndict[gnid].featurecode.Substring(3, 1));
                if (String.IsNullOrEmpty(getadmlabel(makecountry, iadm, gnid)))
                //if (iadm > admclass.admdict[makecountry].maxadm)
                {
                    Console.WriteLine("iadm = " + iadm.ToString());
                    Console.WriteLine("Unsupported ADM-label in valid_article_type");
                    //Console.ReadLine();
                    return false;
                }
            }

            if (createunit > 0)
            {
                bool admfound = false;
                for (int i = 0; i < 5; i++)
                    if (gndict[gnid].adm[i] == createunit)
                        admfound = true;
                if (!admfound)
                {
                    Console.WriteLine("Wrong adm-unit in valid_article_type");
                    return false;
                }
            }
            else if (createunit == 0)
            {
                if (gndict[gnid].adm[1] > 0)
                {
                    Console.WriteLine("Has adm-unit in retrofit_nasa");
                    return false;
                }
            }


            if (createexceptunit > 0)
            {
                bool admfound = false;
                for (int i = 0; i < 5; i++)
                    if (gndict[gnid].adm[i] == createexceptunit)
                        admfound = true;
                if (admfound)
                {
                    Console.WriteLine("Wrong adm-unit in valid_article_type");
                    return false;
                }
            }


            return true;
        }

        public static void make_article(int gnid)
        {

            Console.WriteLine("============");

            hasnotes = false;

            if (!gndict.ContainsKey(gnid))
            {
                Console.WriteLine("Bad gnid in make_article " + gnid.ToString());
                return;
            }

            if (blacklist.Contains(gnid))
            {
                Console.WriteLine("Blacklisted gnid in make_article " + gnid.ToString());
                return;
            }

            Console.WriteLine(gndict[gnid].featureclass.ToString() + "." + gndict[gnid].featurecode);

            if (!valid_article_type(gnid))
                return;

            if (resurrection > 0)
            {
                if (!resurrected.Contains(gnid))
                    return;
            }
            else if (resurrection < 0)
            {
                if (resurrected.Contains(gnid))
                {
                    Console.WriteLine("Skip resurrected");
                    return;
                }
            }

            if (gndict[gnid].roundminute && minutesensitivedict[gndict[gnid].featurecode])
            {
                Console.WriteLine("Rounded coordinates for sensitive article type");
                return;
            }


            string prefix = testprefix;
            string maintitle = "";

            if (gndict[gnid].articlename.Contains("*"))
            {
                Console.WriteLine("Exists already");
                if (makedoubles)
                {
                    prefix = doubleprefix;
                    maintitle = getartname(gnid);
                    Page pmain = new Page(makesite, maintitle);
                    if (util.tryload(pmain, 1))
                        if (!pmain.Exists())
                        {
                            prefix = testprefix;
                            maintitle = "";
                        }
                        else if (pmain.text.Contains(util.mp(195)) && !human_touched(pmain, makesite))
                        {
                            prefix = testprefix;
                            maintitle = "";
                        }

                }
                else
                    return;
            }

            if (gndict[gnid].articlename == "XXX")
            {
                Console.WriteLine("No articlename");
                gndict[gnid].articlename = gndict[gnid].Name_ml;
                //return;
            }

            if (String.IsNullOrEmpty(getartname(gnid)))
            //if (String.IsNullOrEmpty(gndict[gnid].articlename))
            {
                Console.WriteLine("Null articlename");
                gndict[gnid].articlename = gndict[gnid].Name_ml;
                return;
            }


            string countryname = "";
            int icountry = -1;
            if (countrydict.ContainsKey(gndict[gnid].adm[0]))
            {
                icountry = gndict[gnid].adm[0];
                countryname = countrydict[gndict[gnid].adm[0]].Name;
                //Console.WriteLine("country name = " + countryname);
                //Console.WriteLine("Native wiki = "+countrydict[icountry].nativewiki);
            }
            else
                Console.WriteLine("Invalid country " + gndict[gnid].adm[0].ToString());

            Console.WriteLine("gnid = " + gnid);

            string countrynameml = countryname;
            if (countryml.ContainsKey(countryname))
                countrynameml = countryml[countryname];

            string cleanname = util.remove_disambig(gndict[gnid].articlename);
            string oldname = gndict[gnid].articlename.Replace("*", "");
            if (!util.is_latin(cleanname))
            {
                Console.WriteLine("Fixing non-latin name " + getartname(gnid));
                string latinname = gndict[gnid].asciiname;
                if ((util.get_alphabet(cleanname) == "cyrillic") && (makelang == "sv"))
                {
                    latinname = cyrillic.Transliterate(cleanname, countrydict[icountry].nativewiki);
                }
                if (String.IsNullOrEmpty(latinname))
                {
                    Console.WriteLine("No latin name form for " + getartname(gnid));
                    return;
                }
                gndict[gnid].articlename = gndict[gnid].articlename.Replace(cleanname, latinname);
                if (!util.is_latin(gndict[gnid].Name_ml))
                    gndict[gnid].Name_ml = latinname;
            }

            if (gndict[gnid].articlename != oldname)
            {
                int ilang = -1;
                if (langtoint.ContainsKey(countrydict[icountry].nativewiki))
                    ilang = langtoint[countrydict[icountry].nativewiki];
                make_redirect(oldname, getartname(gnid), "", ilang);
            }

            if ((makecountry == "CN") && ((gndict[gnid].featurecode == "ADM4") || (gndict[gnid].featurecode == "PPLA4")))
            {
                if (is_zhen(gnid))
                {
                    gndict[gnid].oldarticlename = gndict[gnid].articlename;
                    gndict[gnid].articlename = gndict[gnid].articlename.Replace("(" + util.mp(298), "(" + util.mp(297));
                }
            }



            //TEMPORARY!
            //if (getartname(gnid).Contains(","))
            //{
            //    string namenocomma = getartname(gnid).Replace(",", "");
            //    Page pagenc = new Page(makesite, namenocomma);
            //    util.tryload(pagenc, 1);
            //    if ( pagenc.Exists())
            //        if (pagenc.text.Contains("obotskapad") && (pagenc.text.Contains(gnid.ToString())))
            //        {
            //            make_redirect_override(pagenc, getartname(gnid),"",-1);
            //            //Console.ReadLine();
            //        }
            //}
            //TEMPORARY!

            Console.WriteLine("prefix = " + prefix);
            Console.WriteLine("getartname = " + getartname(gnid));
            Page p = new Page(makesite, prefix + getartname(gnid));

            util.tryload(p, 3);

            bool ok_to_overwrite = false;
            string origtext = "";

            if (p.Exists() && !makehtml)
            {
                Console.WriteLine("Exists already 1: " + p.title);
                if ((overwrite && (p.text.Contains(util.mp(195)) && !p.text.Contains(util.mp(69))) || p.IsRedirect()) && !human_touched(p, makesite) && p.text.Contains(gnid.ToString()))
                {
                    //p.text = "";
                    ok_to_overwrite = true;
                }
                else if (makedoubles && !p.text.Contains(util.mp(195)))
                {
                    prefix = doubleprefix;
                    maintitle = p.title;
                    p.title = doubleprefix + p.title;
                    Console.WriteLine("Prefix 1: " + p.title);
                }
                else
                    return;
            }

            if ((!String.IsNullOrEmpty(gndict[gnid].oldarticlename)) && (gndict[gnid].oldarticlename.Replace("*", "") != gndict[gnid].articlename.Replace("*", "")))
            {
                Page pold = new Page(makesite, gndict[gnid].oldarticlename.Replace("*", ""));
                util.tryload(pold, 1);
                if (pold.Exists())
                {
                    if (human_touched(pold, makesite)) //old article exists and is edited; don't make new, redirect and return instead
                    {
                        make_redirect(getartname(gnid), pold.title, "", -1);
                        return;
                    }
                    else if (!util.is_fork(pold))
                        make_redirect_override(pold, getartname(gnid), "", -1); //redirect from old to new
                }
            }

            //if ( gndict[gnid].wdid <= 0 )
            //    gndict[gnid].wdid = wdtreeclass.get_wd_item_direct(gnid);

            if (gndict[gnid].wdid <= 0)
            {
                if (makespecificarticles || remakearticleset)
                {
                    gndict[gnid].wdid = wdtreeclass.get_wdid_by_name(makelang, p.title, gnid);
                    if (gndict[gnid].wdid < 0)
                        gndict[gnid].wdid = wdtreeclass.get_wd_item_direct(gnid);
                }
                else if (wdthread)
                {
                    int nwait = 0;
                    while (!wdid_buffer.ContainsKey(gnid) && (nwait < 5))
                    {
                        Console.WriteLine("Waiting for wdid thread.");
                        Thread.Sleep(10000);//milliseconds
                        nwait++;
                    }
                    if (wdid_buffer.ContainsKey(gnid))
                    {
                        gndict[gnid].wdid = wdid_buffer[gnid];
                        Console.WriteLine("Getting from wdid thread.");
                    }
                }
            }
            else
            {
                if (wdid_buffer.ContainsKey(gnid))
                {
                    if (wdid_buffer[gnid] == -2)
                    {
                        Console.WriteLine("VETO from wdid thread. Bad instance");
                        gndict[gnid].wdid = -2;
                    }
                }
            }

            wdid = gndict[gnid].wdid;

            if (wdid > 0)
            {
                currentxml = wdtreeclass.get_wd_xml(wdid);
                if (currentxml == null)
                    wdid = -1;

                if (wdid > 0)
                {
                    double areawd = -1.0;
                    long popwd = -1;
                    string iwss = "";

                    bool preferurban = (gndict[gnid].featureclass == 'P');
                    get_wd_area_pop(wdid, currentxml, out areawd, out popwd, out iwss, preferurban);
                    if (popwd > 0)
                    {
                        Console.WriteLine("popwd = " + popwd.ToString());
                        gndict[gnid].population_wd = popwd;
                        if ((gndict[gnid].population < minimum_population) || (!prefergeonamespop))
                            gndict[gnid].population = popwd;
                        gndict[gnid].population_wd_iw = iwss;
                        //npop++;
                    }

                    if (areawd > 0)
                    {
                        gndict[gnid].area = areawd;
                        //narea++;
                    }

                }
            }
            else
                currentxml = null;

            //Console.WriteLine(gndict[gnid].Name + ": " + gndict[gnid].population_wd.ToString() + gndict[gnid].population_wd_iw);


            Console.WriteLine("wdid = " + wdid.ToString());

            string commonscat = "";

            if ((wdid > 0) && !makehtml)
            {
                Console.WriteLine("wdtreeclass.get_wd_sitelinks");
                Dictionary<string, string> sitelinks = wdtreeclass.get_wd_sitelinks(currentxml);
                foreach (string lang in sitelinks.Keys)
                {
                    if (lang == makelang + "wiki")
                    {
                        Console.WriteLine("Already iw to makelang (1)");
                        if (String.IsNullOrEmpty(prefix))
                        {
                            make_redirect(getartname(gnid), sitelinks[lang], "", -1);

                            if (makedoubles)
                            {
                                Page psl = new Page(makesite, sitelinks[lang]);
                                util.tryload(psl, 2);
                                if (psl.Exists() && !psl.IsRedirect())
                                    if ((p.title != sitelinks[lang]) || !ok_to_overwrite)
                                    {
                                        Console.WriteLine("Setting double");
                                        prefix = doubleprefix;
                                        if (!p.title.Contains(doubleprefix))
                                            p.title = doubleprefix + p.title;
                                        maintitle = sitelinks[lang];
                                        Console.WriteLine("Prefix 2: " + p.title);

                                    }
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    if (lang == "commonswiki")
                        commonscat = sitelinks[lang];
                }
                Console.WriteLine("wdtreeclass.get_wd_commonscat");
                if (String.IsNullOrEmpty(commonscat))
                    commonscat = wdtreeclass.get_wd_prop(wdtreeclass.propdict["commonscat"], currentxml);
            }


            string[] p10 = new string[3] { botname, countrynameml, getmonthstring() };

            origtext = p.text;
            p.text = util.mp(10, p10) + "\n";

            if (util.is_disambig(p.title)) //top link to disambig page
            {
                string forktitle = "";
                Page pfork = new Page(makesite, util.remove_disambig(p.title));
                if (util.tryload(pfork, 1))
                {
                    if (util.is_fork(pfork))
                        forktitle = pfork.title;
                    else
                    {
                        Page pfork2 = new Page(makesite, pfork.title + " (" + util.mp(67) + ")");
                        if (util.tryload(pfork2, 1))
                        {
                            if (pfork2.Exists())
                            {
                                forktitle = pfork2.title;
                            }
                        }

                    }
                }

                if (!String.IsNullOrEmpty(forktitle))
                {
                    String[] p181 = { forktitle };
                    p.text += util.mp(181, p181) + "\n";
                    //pauseaftersave = true;
                }
            }

            if (p.title.Contains("&#"))
                saveanomaly(p.title, "Contains broken html?");

            if ((gndict[gnid].featureclass != 'P') && (gndict[gnid].featureclass != 'A'))
            {
                gndict[gnid].population = 0;
                gndict[gnid].population_wd = 0;
            }

            if (gndict[gnid].featureclass == 'P')
                gndict[gnid].area = 0;


            p.text += fill_geobox(gnid) + "\n\n";

            //Native names:
            Dictionary<string, int> nativenames = new Dictionary<string, int>();

            if ((icountry > 0) && (altdict.ContainsKey(gnid)))
            {
                foreach (altnameclass ac in altdict[gnid])
                {
                    if (ac.altname != gndict[gnid].Name_ml)
                    {
                        if (countrydict[icountry].languages.Contains(ac.ilang))
                        {
                            if (!nativenames.ContainsKey(ac.altname))
                                nativenames.Add(ac.altname, ac.ilang);
                        }
                    }
                }
            }

            string nativestring = "";
            if (nativenames.Count > 0)
            {
                int nname = 0;
                bool commaneeded = false;
                nativestring = "(";

                int prevlang = -1;

                foreach (string nn in nativenames.Keys)
                {
                    int ilang = nativenames[nn];
                    if (langdict.ContainsKey(ilang))
                    {
                        if (langdict[ilang].name.ContainsKey(makelang))
                        {
                            if (commaneeded)
                                nativestring += ", ";
                            if (ilang != prevlang)
                                nativestring += "[[" + langdict[ilang].name[makelang] + "]]: ";
                            nativestring += "'''" + nn + "'''";
                            nname++;
                            commaneeded = true;
                            prevlang = ilang;
                        }
                    }

                }
                if (nname > 0)
                    nativestring += ") ";
                else
                    nativestring = "";
            }

            bool namestart = false; //true if previous sentence started with article name; used to avoid repetitive language.
            string sent = "";
            string pronoun = gndict[gnid].Name_ml;
            string flabel = getfeaturelabelindet(makecountry, gndict[gnid].featurecode, gnid);
            if (makelang == "sv")
            {
                if (flabel.StartsWith("en "))
                    pronoun = "den";
                else if (flabel.StartsWith("ett "))
                    pronoun = "det";
                else
                    pronoun = "de";
            }
            else if (makelang == "no")
            {
                if (flabel.StartsWith("en "))
                    pronoun = "den";
                else if (flabel.StartsWith("et "))
                    pronoun = "det";
                else
                    pronoun = "de";
            }
            else if (makelang == "ceb")
            {
                if (featuredict[gndict[gnid].featurecode].StartsWith("mga "))
                    pronoun = "sila";
                else
                    pronoun = "siya";
            }


            /// X är en Y i landet Z
            sent += "'''" + gndict[gnid].Name_ml + "''' " + nativestring + util.mp(4) + " " + linkfeature(gndict[gnid].featurecode, gnid);
            if (countrydict.ContainsKey(gndict[gnid].adm[0]))
            {
                sent += " " + util.mp(75) + " " + countryclass.countrytitle(gndict[gnid].adm[0]) + countryclass.linkcountry(gndict[gnid].adm[0]);
            }
            if (gndict[gnid].altcountry.Count > 0)
            {
                string countrylist = "";
                int noo = 0;
                foreach (int oo in gndict[gnid].altcountry)
                {
                    if (countrydict.ContainsKey(oo))
                    {
                        if (motherdict.ContainsKey(makecountry))
                            if (oo == countryid[motherdict[makecountry]])
                                continue;
                        noo++;
                        if (noo > 1)
                        {
                            if (noo == gndict[gnid].altcountry.Count)
                                countrylist += util.mp(97);
                            else
                                countrylist += ",";
                        }
                        countrylist += " " + countrydict[oo].Name_ml;
                    }
                }
                if (noo > 0)
                {
                    sent += ", " + util.mp(134) + countrylist;
                }

            }
            p.text += sent + "." + geonameref(gnid);
            namestart = true;



            // X ligger i (kommun), (provins) och (region)

            sent = "";
            int maxadm = 0;
            int minadm = 999;

            for (int i = 4; i > 0; i--)
            {
                if (!String.IsNullOrEmpty(getgnidname(gndict[gnid].adm[i])) && (gndict[gnid].adm[i] != gnid))
                {
                    if (i > maxadm)
                        maxadm = i;
                    if (i < minadm)
                        minadm = i;
                }
            }

            //int capitalgnid = -1;
            string fromcapital = "";
            if (countrydict.ContainsKey(gndict[gnid].adm[0]))
            {
                fromcapital = from_capital(gnid);
            }


            string countrypart = util.mp(coordclass.getcountrypart(gnid));
            if ((makelang == "sv") && (motherdict.ContainsKey(makecountry)))
                countrypart = countrypart.Replace("delen av landet", "delen av " + countrynameml);

            if (maxadm > 0)
            {
                sent += " " + util.mp(135) + " " + gndict[gnid].Name_ml + " " + util.mp(77) + " ";
                for (int i = 4; i > 0; i--)
                {
                    if (!String.IsNullOrEmpty(getgnidname(gndict[gnid].adm[i])) && (gndict[gnid].adm[i] != gnid))
                    {
                        sent += getadmdet(makecountry, i, gndict[gnid].adm[i]) + " " + util.comment("ADM" + i.ToString()) + makegnidlink(gndict[gnid].adm[i]);
                        if (i > minadm)
                        {
                            if (i == minadm + 1)
                                sent += " " + util.mp(3) + " ";
                            else
                                sent += ", ";
                        }
                    }
                }
                sent += ", " + countrypart + fromcapital + ".";

            }
            else
            {
                sent += " " + gndict[gnid].Name_ml + " " + util.mp(92) + " " + countrypart + fromcapital + ".";
            }

            if (!String.IsNullOrEmpty(sent))
                if (sent.Trim().StartsWith(gndict[gnid].Name_ml))
                {
                    if (namestart)
                    {
                        sent = util.ReplaceOne(sent, gndict[gnid].Name_ml, util.initialcap(pronoun), 0);
                        namestart = false;
                    }
                    else
                        namestart = true;
                }
                else
                    namestart = false;

            p.text += sent;



            //population & elevation & area



            string[] p99 = new string[2] { gndict[gnid].Name_ml, util.fnum(gndict[gnid].elevation) };
            string[] p100 = new string[1] { util.comment("pop") + util.fnum(gndict[gnid].population) };
            string[] p100wd = new string[1] { util.comment("pop") + util.fnum(gndict[gnid].population_wd) };


            sent = "";

            Console.WriteLine("elevation/population");
            if ((gndict[gnid].elevation > 0) && ((categorydict[gndict[gnid].featurecode] != "peninsulas")) && (featurepointdict[gndict[gnid].featurecode] || (categorydict[gndict[gnid].featurecode] == "lakes")))
            {
                string heightref = geonameref(gnid);
                if ((gndict[gnid].elevation > 0) && (gndict[gnid].elevation == gndict[gnid].elevation_vp))
                    heightref = addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));

                if ((makecountry == "CN") && (chinese_pop_dict2.ContainsKey(gnid)))
                {
                    p100[0] = util.comment("pop") + util.fnum(chinese_pop_dict2[gnid].pop);
                    sent += " " + util.mp(99, p99) + heightref + util.mp(97) + " " + util.mp(100, p100) + "." + chinapopref();
                    //public class chinese_pop_class
                    //{
                    //    public int adm1 = -1;
                    //    public long pop = -1;
                    //    public long malepop = -1;
                    //    public long femalepop = -1;
                    //    public long households = -1;
                    //    public long pop014 = -1;
                    //    public long pop1564 = -1;
                    //    public long pop65 = -1;
                    //}
                    string[] p300 = new string[2] { util.fnum(chinese_pop_dict2[gnid].femalepop), util.fnum(chinese_pop_dict2[gnid].malepop) };
                    sent += " " + util.mp(300, p300);
                    double c014 = (100 * chinese_pop_dict2[gnid].pop014) / chinese_pop_dict2[gnid].pop;
                    double c1564 = (100 * chinese_pop_dict2[gnid].pop1564) / chinese_pop_dict2[gnid].pop;
                    double c65 = (100 * chinese_pop_dict2[gnid].pop65) / chinese_pop_dict2[gnid].pop;
                    string[] p301 = new string[3] { util.fnum(c014), util.fnum(c1564), util.fnum(c65) };
                    sent += " " + util.mp(301, p301) + chinapopref();
                }
                else if (gndict[gnid].population > minimum_population)
                {
                    if (gndict[gnid].population_wd == gndict[gnid].population)
                        sent += " " + util.mp(99, p99) + heightref + util.mp(97) + " " + util.mp(100, p100wd) + "." + wikiref(gndict[gnid].population_wd_iw);
                    else
                        sent += " " + util.mp(99, p99) + heightref + util.mp(97) + " " + util.mp(100, p100) + "." + geonameref(gnid);
                }
                else
                {
                    if (gndict[gnid].population_wd > minimum_population)
                    {
                        p100[0] = gndict[gnid].population_wd.ToString();
                        sent += " " + util.mp(99, p99) + heightref + util.mp(97) + " " + util.mp(100, p100wd) + "." + wikiref(gndict[gnid].population_wd_iw);
                    }
                    else
                    {
                        int imp = 99;
                        bool peak = is_height(gndict[gnid].featurecode);
                        if (peak)
                            imp = 178;
                        //switch (categorydict[gndict[gnid].featurecode])
                        //{
                        //    case "mountains":
                        //    case "hills":
                        //    case "volcanoes":
                        //        imp = 178;
                        //        break;
                        //    default:
                        //        imp = 99;
                        //        break;

                        //}
                        sent += " " + util.mp(imp, p99);
                        if ((peak) && (gndict[gnid].prominence > minimum_prominence))
                        {
                            sent += "," + heightref;
                            string[] p190 = new string[1] { util.fnum(gndict[gnid].prominence) };
                            sent += " " + util.mp(190, p190);
                            sent += addnote(util.mp(191) + addref("vp", viewfinder_ref()) + " " + util.mp(200));
                            string[] p192 = new string[1] { util.fnum(gndict[gnid].width) };
                            sent += ". " + util.mp(192, p192) + "." + addnote(util.mp(193));
                        }
                        else
                            sent += "." + heightref;
                    }
                }
            }
            else if ((makecountry == "CN") && (chinese_pop_dict2.ContainsKey(gnid)))
            {
                p100[0] = util.comment("pop") + util.fnum(chinese_pop_dict2[gnid].pop);
                sent += " " + util.initialcap(util.mp(100, p100)) + "." + chinapopref();
                //public class chinese_pop_class
                //{
                //    public int adm1 = -1;
                //    public long pop = -1;
                //    public long malepop = -1;
                //    public long femalepop = -1;
                //    public long households = -1;
                //    public long pop014 = -1;
                //    public long pop1564 = -1;
                //    public long pop65 = -1;
                //}
                string[] p300 = new string[2] { util.fnum(chinese_pop_dict2[gnid].femalepop), util.fnum(chinese_pop_dict2[gnid].malepop) };
                sent += " " + util.mp(300, p300);
                double c014 = (100 * chinese_pop_dict2[gnid].pop014) / chinese_pop_dict2[gnid].pop;
                double c1564 = (100 * chinese_pop_dict2[gnid].pop1564) / chinese_pop_dict2[gnid].pop;
                double c65 = (100 * chinese_pop_dict2[gnid].pop65) / chinese_pop_dict2[gnid].pop;
                string[] p301 = new string[3] { util.fnum(c014), util.fnum(c1564), util.fnum(c65) };
                sent += " " + util.mp(301, p301) + chinapopref();
            }
            else if (gndict[gnid].population > minimum_population)
            {
                sent += " " + util.initialcap(util.mp(100, p100)) + ".";
                if (gndict[gnid].population_wd == gndict[gnid].population)
                    sent += wikiref(gndict[gnid].population_wd_iw);
                else
                    sent += geonameref(gnid);
            }
            else if (gndict[gnid].population_wd > minimum_population)
            {
                //p100[0] = comment("pop") + util.fnum(gndict[gnid].population_wd);
                sent += " " + util.initialcap(util.mp(100, p100wd)) + "." + wikiref(gndict[gnid].population_wd_iw);
            }

            Console.WriteLine("area");
            if (gndict[gnid].area > minimum_area)
            {
                string[] p129 = new string[2] { gndict[gnid].Name_ml, util.fnum(gndict[gnid].area) };
                sent += " " + util.mp(129, p129);
            }

            if (!String.IsNullOrEmpty(sent))
                if (sent.Trim().StartsWith(gndict[gnid].Name_ml))
                {
                    if (namestart)
                    {
                        sent = util.ReplaceOne(sent, gndict[gnid].Name_ml, util.initialcap(pronoun), 0);
                        namestart = false;
                    }
                    else
                        namestart = true;

                }
                else
                    namestart = false;

            p.text += sent;
            sent = "";

            if ((gndict[gnid].island > 0) && (categorydict[gndict[gnid].featurecode] != "islands")) //On island; not island-on-island
            {
                if (gndict[gndict[gnid].island].area > gndict[gnid].area) //Only if island is bigger than gnid
                {
                    string[] p139 = new string[2] { gndict[gnid].Name_ml, makegnidlink(gndict[gnid].island) };
                    sent += " " + util.mp(139, p139);
                    //if (makelang == "sv")
                    sent += addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));
                }
            }

            if (!String.IsNullOrEmpty(sent))
                if (sent.Trim().StartsWith(gndict[gnid].Name_ml))
                {
                    if (namestart)
                    {
                        sent = util.ReplaceOne(sent, gndict[gnid].Name_ml, util.initialcap(pronoun), 0);
                        namestart = false;
                    }
                    else
                        namestart = true;
                }
                else
                    namestart = false;

            p.text += sent;
            sent = "";

            if ((gndict[gnid].inlake > 0) && (categorydict[gndict[gnid].featurecode] != "lakes"))//In a lake (mainly islands); not lake-in-lake
            {
                if (gndict[gndict[gnid].inlake].area > gndict[gnid].area) //Only if lake is bigger than gnid
                {
                    string[] p155 = new string[2] { gndict[gnid].Name_ml, makegnidlink(gndict[gnid].inlake) };
                    sent += " " + util.mp(155, p155);
                    if (makelang == "sv")
                        sent += addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));
                }
            }


            if (!String.IsNullOrEmpty(sent))
                if (sent.Trim().StartsWith(gndict[gnid].Name_ml))
                {
                    if (namestart)
                    {
                        sent = util.ReplaceOne(sent, gndict[gnid].Name_ml, util.initialcap(pronoun), 0);
                        namestart = false;
                    }
                    else
                        namestart = true;
                }
                else
                    namestart = false;

            p.text += sent;
            sent = "";


            if (gndict[gnid].atlakes.Count > 0) //Near one or more lakes
            {
                if (gndict[gnid].atlakes.Count == 1)
                {
                    if (gndict[gndict[gnid].atlakes[0]].area > gndict[gnid].area) //Only if lake is bigger than gnid
                    {
                        string[] p156 = new string[2] { gndict[gnid].Name_ml, makegnidlink(gndict[gnid].atlakes[0]) };
                        sent += " " + util.mp(156, p156);
                        if (makelang == "sv")
                            sent += addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));
                    }
                }
                else
                {
                    string lakes = "";
                    int ilakes = 0;
                    foreach (int lg in gndict[gnid].atlakes)
                    {
                        ilakes++;
                        if (ilakes == gndict[gnid].atlakes.Count)
                            lakes += util.mp(97);
                        lakes += " " + makegnidlink(lg);
                    }
                    string[] p157 = new string[2] { gndict[gnid].Name_ml, lakes };
                    sent += " " + util.mp(157, p157);
                    //if (makelang == "sv")
                    sent += addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));
                }
            }

            if (!String.IsNullOrEmpty(sent))
                if (sent.Trim().StartsWith(gndict[gnid].Name_ml))
                {
                    if (namestart)
                    {
                        sent = util.ReplaceOne(sent, gndict[gnid].Name_ml, util.initialcap(pronoun), 0);
                        namestart = false;
                    }
                    else
                        namestart = true;
                }
                else
                    namestart = false;

            p.text += sent;
            sent = "";

            string rangecat = "";
            if ((gndict[gnid].inrange > 0) && (gndict.ContainsKey(gndict[gnid].inrange)))//Part of a mountain range
            {
                sent = " " + gndict[gnid].Name_ml + " " + util.mp(204) + " " + makegnidlink(gndict[gnid].inrange) + ".";
                sent += addnote(util.mp(140) + addref("vp", viewfinder_ref()) + " " + util.mp(200));
                rangecat = getartname(gndict[gnid].inrange);
            }

            string drainagecat = "";
#if (DBGEOFLAG)

            if (riverdict.ContainsKey(gnid))
                if ( drainagedict.ContainsKey(riverdict[gnid].drainage_name))
                {
                    drainagecat = drainagedict[riverdict[gnid].drainage_name].main_river_artname + " " + util.mp(320);
                }
#endif
            if (!String.IsNullOrEmpty(sent))
                if (sent.Trim().StartsWith(gndict[gnid].Name_ml))
                {
                    if (namestart)
                    {
                        sent = util.ReplaceOne(sent, gndict[gnid].Name_ml, util.initialcap(pronoun), 0);
                        namestart = false;
                    }
                    else
                        namestart = true;
                }
                else
                    namestart = false;

            p.text += sent;
            sent = "";

            //p.text += "\n\n";

            //separate for different types of features:
            Console.WriteLine("Feature-specific");
            if ((gndict[gnid].featureclass == 'A') && (gndict[gnid].featurecode.Contains("ADM")) && (!gndict[gnid].featurecode.Contains("ADMD")))
            {
                p.text += make_adm(gnid);
            }
            else if (gndict[gnid].featureclass == 'P')
            {
                if ((gndict[gnid].population > minimum_population) || (chinese_pop_dict2.ContainsKey(gnid)))
                    p.text += make_town(gnid);
                else
                {
                    Console.WriteLine("Below minimum population.");
                    return;
                }
            }
            else if (categorydict[gndict[gnid].featurecode] == "islands")
            {
                p.text += make_island(gnid, p);
            }
            else if (categorydict[gndict[gnid].featurecode] == "lakes")
            {
                if (good_lake(gnid))
                    p.text += make_lake(gnid, p);
                else
                    return;
            }
            else if (categorydict[gndict[gnid].featurecode] == "streams")
            {
                p.text += make_river(gnid, p);
            }
            else if ((gndict[gnid].featurecode == "MTS") || (gndict[gnid].featurecode == "HLLS"))
            {
                p.text += make_range(gnid, p);
            }
            //else if (gndict[gnid].featurecode == "CHN")
            //{
            //    p.text += make_channel(gnid);
            //}
            else if (featurepointdict[gndict[gnid].featurecode])
            {
                if (is_height(gndict[gnid].featurecode))
                    if (gndict[gnid].elevation < minimum_prominence)
                    {
                        Console.WriteLine("Hill too low");
                        return;
                    }
                p.text += make_point(gnid);
            }
            else //Nothing type-specific to add
            {
            }

            if (!noclimatelist.Contains(gndict[gnid].featurecode))
            {
                p.text += "\n\n" + make_climate(gnid);
            }

            //locator map:
            if (!locatoringeobox)
            {
                string[] p73 = new string[2] { countrynameml, gndict[gnid].Name_ml };
                p.text += "\n\n" + util.mp(72) + "|" + locatordict[countryname].get_locator(gndict[gnid].latitude, gndict[gnid].longitude) + " |float = right  |width=300 |";
                if (makelang != "sv")
                    p.text += " caption = " + util.mp(73, p73) + " | ";
                p.text += util.mp(76) + " = " + gndict[gnid].Name_ml + "|position=right|background=white|lat=" + gndict[gnid].latitude.ToString(culture_en) + "|long=" + gndict[gnid].longitude.ToString(culture_en);
                p.text += "}}\n";
            }

            //Reference list:

            if (hasnotes)
                p.text += util.mp(175).Replace("XX", "\n\n");

            reflist += "\n</references>\n\n";
            p.text += "\n\n== " + util.mp(51) + " ==\n\n" + reflist;

            //External links:
            if (!String.IsNullOrEmpty(commonscat))
            {
                if (commonscat.Contains("Category:"))
                    commonscat = commonscat.Replace("Category:", "");
                p.text += "\n\n== " + util.mp(52) + "==\n\n{{commonscat|" + commonscat + "|" + gndict[gnid].Name_ml + "}}\n";
            }

            //If testrun only, inactivate categories and iw:

            if (!String.IsNullOrEmpty(prefix))
            {
                p.text += "<nowiki>\n";
            }

            //Categories:

            string catcode = categorydict[gndict[gnid].featurecode];

            string catname = "";
            if (!String.IsNullOrEmpty(getartname(gndict[gnid].adm[1])))
            {
                //catname = categoryml[catcode] + " " + util.mp(75) + " " + getgnidname(gndict[gnid].adm[1]);
                catname = make_catname(catcode, getartname(gndict[gnid].adm[1]), false);
                if (String.IsNullOrEmpty(prefix))
                    make_x_in_adm1(catcode, gndict[gnid].adm[1], countrynameml);
            }
            else
            {
                catname = make_catname(catcode, countrynameml, true);
                if (String.IsNullOrEmpty(prefix))
                    make_x_in_country(catcode, countrynameml);
            }

            if (!String.IsNullOrEmpty(rangecat))
            {
                p.AddToCategory(util.initialcap(rangecat));
                Page rcp = new Page(makesite, util.mp(1) + rangecat);
                util.tryload(rcp, 1);
                if (!rcp.Exists())
                {
                    rcp.text = util.mp(120) + "\n";
                    rcp.AddToCategory(util.initialcap(catname));
                    util.trysave(rcp, 2, makesite.defaultEditComment + " " + util.mp(1));
                }
                else if (!rcp.text.Contains(catname))
                {
                    rcp.AddToCategory(util.initialcap(catname));
                    util.trysave(rcp, 2, makesite.defaultEditComment + " " + util.mp(1));
                }

            }

            if (!String.IsNullOrEmpty(drainagecat))
            {
                p.AddToCategory(util.initialcap(drainagecat));
                Page rcp = new Page(makesite, util.mp(1) + drainagecat);
                util.tryload(rcp, 1);
                if (!rcp.Exists())
                {
                    rcp.text = util.mp(120) + "\n";
                    rcp.AddToCategory(util.initialcap(catname));
                    util.trysave(rcp, 2, makesite.defaultEditComment + " " + util.mp(1));
                }
                else if (!rcp.text.Contains(catname))
                {
                    rcp.AddToCategory(util.initialcap(catname));
                    util.trysave(rcp, 2, makesite.defaultEditComment + " " + util.mp(1));
                }

            }

            if (categorydict[gndict[gnid].featurecode] == "streams")
            {
#if (DBGEOFLAG)
                foreach (string ocean in oceannamedict.Keys)
                {
                    if ( p.text.Contains("[["+oceannamedict[ocean]+"]]"))
                    {
                        string[] p322 = new string[] { oceannamedict[ocean] };
                        p.AddToCategory(util.mp(322,p322));
                    }
                }
#endif
            }

            p.AddToCategory(util.initialcap(catname));

            switch (catcode)
            {
                case "lakes":
                case "islands":
                    cat_by_size(p, catcode, countrynameml, gndict[gnid].area);
                    break;
                case "mountains":
                case "hills":
                case "volcanoes":
                    cat_by_size(p, "mountains", countrynameml, gndict[gnid].elevation);
                    break;
                case "populated places":
                    double dpop = gndict[gnid].population;
                    cat_by_size(p, "populated places", countrynameml, dpop, false);
                    break;
                default:
                    break;
            }

            if ((makecountry == "CN") && (makelang == "sv"))
                p.AddToCategory("WP:Projekt Kina");

            p.text += "\n\n";

            //Interwiki:
            if (wdid > 0)
            {
                string iwl = wdtreeclass.iwlinks(currentxml);
                if (!iwl.Contains("Exists already"))
                    p.text += "\n" + iwl;
                //else
                //{
                //    string oldtit = iwl.Split(':')[1];
                //    if (!makedoubles)
                //    {
                //        make_redirect(prefix + gndict[gnid].articlename, oldtit, "", -1);
                //        return;
                //    }
                //    else
                //    {
                //        if ((p.title != oldtit) || !ok_to_overwrite)
                //        {
                //            if (!p.title.Contains(doubleprefix))
                //                p.title = doubleprefix + p.title;
                //            maintitle = oldtit;
                //        }
                //    }
                //}
            }
            else
            {
                if (!String.IsNullOrEmpty(gndict[gnid].artname2))
                {
                    string iwl = "\n[[";
                    if (makelang == "sv")
                        iwl += "ceb:";
                    else
                        iwl += "sv:";
                    iwl += gndict[gnid].artname2 + "]]\n";
                    p.text += iwl;
                }
            }

            if (!String.IsNullOrEmpty(prefix))
            {
                p.text += "</nowiki>\n";
                p.text += "[[" + util.mp(1) + countrynameml + " duplicates]]\n";
            }

            if (makedoubles && !String.IsNullOrEmpty(maintitle))
                p.text = saveconflict(p.title, maintitle) + p.text;

            if (p.text.Contains(util.mp(213)))
                p.AddToCategory(util.mp(214));

            countryspecials(p, gnid, catcode);

            //Clean and save:

            p.text = p.text.Replace("{{geobox\n| 0 ", "{{geobox\n| 1 ");
            p.text = p.text.Replace("= <!--", "=\n<!--");

            p.text = util.cleanup_text(p.text);

            if (p.text != origtext)
            {
                if (ok_to_overwrite)
                    util.trysave(p, 4, util.mp(303) + " " + makesite.defaultEditComment);
                else
                    util.trysave(p, 4);
            }

            //Redirects:

            //if (!String.IsNullOrEmpty(testprefix))
            //{
            //make_redirect(testprefix + gndict[gnid].Name, gndict[gnid].articlename, "");

            if (resurrection <= 0)
            {
                if (gndict[gnid].Name != getartname(gnid))
                    make_redirect(testprefix + gndict[gnid].Name, getartname(gnid), "", -1);
                if (gndict[gnid].asciiname != getartname(gnid))
                    make_redirect(testprefix + gndict[gnid].asciiname, getartname(gnid), "", -1);
            }

            if (altdict.ContainsKey(gnid))
            {
                foreach (altnameclass ac in altdict[gnid])
                {
                    if ((!String.IsNullOrEmpty(ac.altname)) && (util.tryconvert(ac.altname) <= 0) && (ac.altname != util.remove_disambig(getartname(gnid))))
                        make_redirect(testprefix + ac.altname, getartname(gnid), "", ac.ilang);
                }
            }
            //}

            romanian_redirect(getartname(gnid));

            if (!String.IsNullOrEmpty(gndict[gnid].unfixedarticlename))
                make_redirect(gndict[gnid].unfixedarticlename.Replace("*", ""), getartname(gnid), "", -1);

            //Console.WriteLine("<ret>");
            //Console.ReadLine();

        }

        public static void countryspecials(Page p, int gnid, string catcode)
        {
            if (makecountry == "AQ") //specials for Antarctica:
            {
                p.SetTemplateParameter("geobox", "timezone", "", true);
                p.SetTemplateParameter("geobox", "timezone_label", "", true);
                p.SetTemplateParameter("geobox", "utc_offset", "", true);
                p.SetTemplateParameter("geobox", "timezone_DST", "", true);
                p.SetTemplateParameter("geobox", "utc_offset_DST", "", true);

                string sectortext = antarctic_sector(gndict[gnid].longitude);

                if (makelang == "sv")
                {
                    p.text = p.text.Replace("Trakten är glest befolkad. Det finns inga samhällen i närheten.", "Trakten är obefolkad. Det finns inga samhällen i närheten.");
                    p.text = p.text.Replace("delen av landet.", "delen av kontinenten. " + sectortext);
                    p.text = p.text.Replace(" Närmaste större samhälle ", " Närmaste befolkade plats ");
                    p.text = p.text.Replace("den östra delen av kontinenten", "[[Östantarktis]]");
                    p.text = p.text.Replace("den västra delen av kontinenten", "[[Västantarktis]]");
                    p.text = p.text.Replace("den norra delen av kontinenten", "[[Sydshetlandsöarna]]");
                    p.text = p.text.Replace("den södra delen av kontinenten", "[[Sydorkneyöarna]]");
                    if (p.text.Contains("[[Östantarktis]]"))
                        p.AddToCategory(make_catname(catcode, "Östantarktis", false));
                    if (p.text.Contains("[[Västantarktis]]"))
                        p.AddToCategory(make_catname(catcode, "Västantarktis", false));
                    if (p.text.Contains("[[Sydshetlandsöarna]]"))
                        p.AddToCategory("Sydshetlandsöarna");
                    if (p.text.Contains("[[Sydorkneyöarna]]"))
                        p.AddToCategory("Sydorkneyöarna");
                    if (p.text.Contains("[[Norge]]"))
                        p.AddToCategory("Norges anspråk i Antarktis");
                    if (p.text.Contains("[[Storbritannien]]"))
                        p.AddToCategory("Storbritanniens anspråk i Antarktis");
                    if (p.text.Contains("[[Chile]]"))
                        p.AddToCategory("Chiles anspråk i Antarktis");
                    if (p.text.Contains("[[Argentina]]"))
                        p.AddToCategory("Argentinas anspråk i Antarktis");
                    if (p.text.Contains("[[Frankrike]]"))
                        p.AddToCategory("Frankrikes anspråk i Antarktis");
                    if (p.text.Contains("[[Australien]]"))
                        p.AddToCategory("Australiens anspråk i Antarktis");
                    if (p.text.Contains("[[Nya Zeeland]]"))
                        p.AddToCategory("Nya Zeelands anspråk i Antarktis");

                    if (p.text.Contains("Kategori:Landformer på havets botten") || p.text.Contains("Kategori:Antarktis ö"))
                    {
                        p.text = p.text.Replace("Den ligger i ", "Den ligger i havet utanför ");
                        p.text = p.text.Replace("Det ligger i ", "Det ligger i havet utanför ");
                    }

                    p.SetTemplateParameter("geobox", "country_type", "Kontinent", true);
                }
                else if (makelang == "ceb")
                {
                    p.text = p.text.Replace("bahin sa nasod.", "bahin sa kontinente. " + sectortext);
                    p.SetTemplateParameter("geobox", "country_type", "Kontinente", true);

                }
            }


        }

        public static string enumeration(List<string> namelist)
        {
            int n = namelist.Count;
            if (n == 0)
                return "";
            else if (n == 1)
                return namelist[0];
            else
            {
                string rs = "";
                foreach (string name in namelist)
                {
                    if (n == 1)
                        rs += util.mp(97) + " ";
                    else if (n < namelist.Count)
                        rs += ", ";
                    rs += name;
                    n--;
                }
                return rs;
            }
        }

        public static string antarctic_sector(double lon)
        {
            List<string> claims = new List<string>();
            if ((lon <= -150) || (lon >= 160))
                claims.Add("NZ");
            if ((lon <= -25) && (lon >= -74))
                claims.Add("AR");
            if ((lon >= 142) && (lon <= 160))
                claims.Add("AU");
            if ((lon >= 45) && (lon <= 136))
                claims.Add("AU");
            if ((lon >= 136) && (lon <= 142))
                claims.Add("FR");
            if ((lon >= -20) && (lon <= 45))
                claims.Add("NO");
            if ((lon <= -28) && (lon >= -53))
                claims.Add("BR");
            if ((lon <= -53) && (lon >= -90))
                claims.Add("CL");
            if ((lon <= -20) && (lon >= -80))
                claims.Add("GB");

            List<string> claimnames = new List<string>();
            foreach (string cc in claims)
            {
                string countrynameml = countrydict[countryid[cc]].Name;
                if (countryml.ContainsKey(countrynameml))
                    countrynameml = countryml[countrynameml];
                claimnames.Add("[[" + countrynameml + "]]");
            }

            string[] p210 = new string[1] { util.mp(211) };
            if (claimnames.Count > 0)
                p210[0] = enumeration(claimnames);

            return util.mp(210, p210);


        }

        public static void fix_sizecats()
        {
            int icountry = countryid[makecountry];
            string countrynameml = countrydict[icountry].Name;
            if (countryml.ContainsKey(countrynameml))
                countrynameml = countryml[countrynameml];

            string towncat = make_catname("populated places", countrynameml, true);
            PageList pl = new PageList(makesite);
            PageList pl1 = new PageList(makesite);
            pl.FillFromCategoryTree(towncat);

            foreach (Page p in pl)
            {
                util.tryload(p, 2);
                string origtext = p.text;

                double areaout = -1;
                long popout = -1;
                int heightout = -1;

                get_page_area_pop_height(p, out areaout, out popout, out heightout);

                double dpop = popout;
                cat_by_size(p, "populated places", countrynameml, dpop, false);

                if (origtext != p.text)
                {
                    util.trysave(p, 2, util.mp(305, null));

                    //Console.WriteLine("<ret>");
                    //Console.ReadLine();
                }
            }
        }

        public static void fix_sizecats2()
        {
            int icountry = countryid[makecountry];
            string countrynameml = countrydict[icountry].Name;
            if (countryml.ContainsKey(countrynameml))
                countrynameml = countryml[countrynameml];

            string towncat = make_catname("populated places", countrynameml, true);
            string tcbad1 = "Orter i " + countrynameml + " större än 100 kvadratkilometer";
            string tcbad2 = "Orter i " + countrynameml + " större än 1000 kvadratkilometer";
            PageList pl = new PageList(makesite);
            PageList pl1 = new PageList(makesite);
            pl.FillFromCategoryTree(towncat);

            foreach (Page p in pl)
            {
                util.tryload(p, 2);
                string origtext = p.text;

                double areaout = -1;
                long popout = -1;
                int heightout = -1;

                get_page_area_pop_height(p, out areaout, out popout, out heightout);

                p.RemoveFromCategory(tcbad1);
                p.RemoveFromCategory(tcbad2);

                double dpop = popout;
                cat_by_size(p, "populated places", countrynameml, dpop, false);

                if (origtext != p.text)
                {
                    util.trysave(p, 2, util.mp(305));

                    //Console.WriteLine("<ret>");
                    //Console.ReadLine();
                }
            }


        }

        public static string oldsizecat(Page p, string catcode, string countrynameml, double size, bool is_area)
        {
            double[] heightsize = { 200.0, 500.0, 1000.0, 2000.0, 4000.0, 6000.0 };
            int sizecat = -1;
            double catsize = -1;
            int imax = heightsize.Length;

            for (int i = 0; i < imax; i++)
            {
                if (size >= heightsize[i])
                {
                    sizecat = i;
                    catsize = heightsize[i];
                }
            }
            if (sizecat < 0)
                return "";

            //Console.WriteLine("catcode = " + catcode);
            string[] p176 = { categoryml[catcode], countrynameml, catsize.ToString("F0", nfi) };
            string catname = "";
            int imp = 177;
            catname = util.initialcap(util.mp(imp, p176));
            return catname;
        }

        public static string tostringsize(double size)
        {
            if (size > 9000.0)
                return size.ToString("N0", nfi_space);
            else
                return size.ToString("F0", nfi);

        }

        public static void cat_by_size(Page p, string catcode, string countrynameml, double size, bool is_area)
        {
            double[] areasize = { 1.0, 2.0, 5.0, 10.0, 100.0, 1000.0 };
            //double[] heightsize = { 200.0, 500.0, 1000.0, 2000.0, 4000.0, 6000.0 };
            double[] heightsize = { 200.0, 500.0, 1000.0, 2000.0, 3000.0, 4000.0, 5000.0, 6000.0, 7000.0, 8000.0 };
            double[] popsize = { 3000.0, 10000.0, 30000.0, 100000.0, 300000.0, 1000000.0 };

            //Console.WriteLine("popsize[5].ToString() "+popsize[5].ToString());
            //Console.WriteLine("popsize[5].ToString(nfi) " + popsize[5].ToString(nfi));
            //Console.WriteLine("popsize[5].ToString(F0,nfi) " + popsize[5].ToString("F0",nfi));
            //Console.WriteLine("popsize[5].ToString(F0,nfi_en) " + popsize[5].ToString("F0", nfi_en));
            //Console.WriteLine("popsize[5].ToString(N0,nfi) " + popsize[5].ToString("N0", nfi));
            //Console.WriteLine("popsize[5].ToString(N0,nfi_space) " + popsize[5].ToString("N0", nfi_space));
            //Console.WriteLine("popsize[0].ToString(N0,nfi_space) " + popsize[0].ToString("N0", nfi_space));
            //Console.ReadLine();


            int sizecat = -1;
            double catsize = -1;
            int imax = popsize.Length;
            if (is_area)
                imax = areasize.Length;
            else if (catcode == "mountains")
                imax = heightsize.Length;

            for (int i = 0; i < imax; i++)
            {
                if (is_area)
                {
                    if (size > areasize[i])
                    {
                        sizecat = i;
                        catsize = areasize[i];
                    }
                }
                else if (catcode == "mountains")
                {
                    if (size >= heightsize[i])
                    {
                        sizecat = i;
                        catsize = heightsize[i];
                    }
                }
                else
                {
                    if (size >= popsize[i])
                    {
                        sizecat = i;
                        catsize = popsize[i];
                    }
                }
            }
            if (sizecat < 0)
                return;

            string[] p176 = { categoryml[catcode], countrynameml, tostringsize(catsize) };
            string catname = "";
            int imp = 217;
            if (is_area)
                imp = 217;
            else if (catcode == "mountains")
                imp = 218;
            else
                imp = 216;
            catname = util.initialcap(util.mp(imp, p176));
            p.AddToCategory(catname);
            catname = util.mp(1) + catname;
            while (sizecat >= 0)
            {
                Page pcat = new Page(makesite, catname);
                util.tryload(pcat, 1);
                if (pcat.Exists())
                    break;
                string incat = "";
                if (sizecat > 0)
                {
                    if (is_area)
                    {
                        p176[2] = tostringsize(areasize[sizecat - 1]);
                    }
                    else if (catcode == "mountains")
                    {
                        p176[2] = tostringsize(heightsize[sizecat - 1]);
                    }
                    else
                    {
                        p176[2] = tostringsize(popsize[sizecat - 1]);
                    }
                    incat = util.mp(imp, p176);
                }
                else
                    incat = make_catname(catcode, countrynameml, true) + "| ";
                incat = util.initialcap(incat);
                pcat.text = util.mp(120);
                pcat.AddToCategory(incat);
                string worldcat = catname.Replace(util.mp(75) + " " + countrynameml, "") + "|" + countrynameml;
                pcat.AddToCategory(worldcat);
                util.trysave(pcat, 2, makesite.defaultEditComment + " " + util.mp(1));
                catname = util.mp(1) + incat;
                sizecat--;
            }

        }

        public static void cat_by_size(Page p, string catcode, string countrynameml, double area)
        {
            cat_by_size(p, catcode, countrynameml, area, true);
        }

        public static void cat_by_size(Page p, string catcode, string countrynameml, int elevation)
        {
            double elev = elevation;
            cat_by_size(p, catcode, countrynameml, elev, false);
        }



        public static void make_articles()
        {
            makesite.defaultEditComment = util.mp(60) + " " + countryml[makecountryname];

            if (!makehtml)
            {
                if (pstats == null)
                {
                    pstats = new Page(makesite, util.mp(13) + botname + "/Statistik");
                    pstats.Load();
                }
                pstats.text += "\n\n== [[" + countryml[makecountryname] + "]] ==\n\n";
                util.trysave(pstats, 1, util.mp(302) + " " + countryml[makecountryname]);
            }

            string[] p295 = new string[] { countryml[makecountryname] };
            Page pcat = new Page(makesite, util.mp(1) + util.mp(295, p295));
            util.tryload(pcat, 1);
            if (!pcat.Exists())
            {
                pcat.text = "[[" + util.mp(1) + util.mp(296, p295) + "]]";
                util.trysave(pcat, 2);
            }


            if (makecountry == "AQ") //Antarctica
                minimum_population = 5;
            else
                minimum_population = 100;

            int iremain = gndict.Count;
            int iremain0 = iremain;

            foreach (int gnid in gndict.Keys)
            {
                iremain--;
                if ((resume_at > 0) && (resume_at != gnid))
                {
                    stats.Addskip();
                    continue;
                }
                else
                    resume_at = -1;

                if (stop_at == gnid)
                    break;

                reflist = "<references>";
                refnamelist.Clear();
                try
                {
                    make_article(gnid);
                }
                catch (OutOfMemoryException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    return;
                }

                Console.WriteLine(iremain.ToString() + " remaining.");

                if (firstround && (iremain0 - iremain < 5))
                {
                    Console.WriteLine("<cr>");
                    Console.ReadLine();
                }
            }

            Console.WriteLine(stats.GetStat());

            if (!makehtml)
            {
                if (pstats == null)
                {
                    pstats = new Page(makesite, util.mp(13) + botname + "/Statistik");
                    pstats.Load();
                }
                //pstats.text += "\n\n== [[" + countryml[makecountryname] + "]] ==\n\n";
                pstats.text += stats.GetStat();
                util.trysave(pstats, 1, util.mp(302) + " " + countryml[makecountryname]);
            }
            stats.ClearStat();

        }

        public static void make_specific_articles()
        {
            makesite.defaultEditComment = util.mp(60) + " " + countryml[makecountryname];

            while (true)
            {
                Console.Write("Gnid: ");
                string gnidstring = Console.ReadLine();
                reflist = "<references>";
                refnamelist.Clear();

                make_article(util.tryconvert(gnidstring));

                //Console.WriteLine("<cr>");
                //Console.ReadLine();
            }
        }

        public static void remake_article_set()
        {
            Console.WriteLine("In remake_article_set");
            PageList pl = new PageList(makesite);
            PageList pl1 = new PageList(makesite);

            //Find articles from a category
            //pl.FillAllFromCategoryTree("Geografi i Goiás");
            //pl1.FillAllFromCategoryTree("Eufriesea");
            //foreach (Page p in pl1)
            //    pl.Add(p);
            //pl1.FillAllFromCategoryTree("Euglossa");
            //foreach (Page p in pl1)
            //    pl.Add(p);
            //pl1.FillAllFromCategoryTree("Eulaema");
            //foreach (Page p in pl1)
            //    pl.Add(p);
            //pl1.FillAllFromCategoryTree("Exaerete");
            //foreach (Page p in pl1)
            //    pl.Add(p);
            //pl.FillFromCategory("Robotskapade Finlandförgreningar");

            //Find subcategories of a category
            //pl.FillSubsFromCategory("Svampars vetenskapliga namn");

            //Find articles from all the links to an article, mostly useful on very small wikis
            //pl.FillFromLinksToPage("Användare:Lsjbot/Algoritmer");

            //Find articles containing a specific string
            pl.FillFromSearchResults("insource:/och [A-Z][a-z]+språkiga Wikipedia./", 4999);
            //pl.FillFromSearchResults("insource:\"http://www.itis.gov;http://\"", 4999);

            //Set specific article:
            //Page pp = new Page(site, "Citrontrogon");pl.Add(pp);

            //Skip all namespaces except articles:
            pl.RemoveNamespaces(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 100, 101 });

            Console.WriteLine("In remake_article_set. Pl.Count = " + pl.Count());

            foreach (Page p in pl)
            {
                util.tryload(p, 2);
                if (!p.Exists())
                    continue;

                if (!p.text.Contains("obotskapad"))
                    continue;

                int gnid = get_gnid_from_article(p);

                if (!gndict.ContainsKey(gnid))
                    continue;

                if (human_touched(p, makesite))
                    continue;

                reflist = "<references>";
                refnamelist.Clear();

                make_article(gnid);
            }

        }

        public static int get_gnid_from_article(Page p)
        {
            foreach (string gs in p.GetTemplateParameter(util.mp(173), "gnid"))
                if (util.tryconvert(gs) > 0)
                    return util.tryconvert(gs);

            return -1;
        }

        public static void verify_geonames()
        {

            int n = 0;
            int n1 = 0;
            int ngnid = 0;
            using (StreamWriter sw = new StreamWriter("gnvswiki_pop.txt"))
            {

                foreach (int gnid in gndict.Keys)
                {
                    ngnid++;
                    if ((gndict[gnid].population > 0) && (gndict[gnid].population_wd > 0))
                    {
                        n1++;
                        sw.WriteLine(gndict[gnid].Name + tabstring + gndict[gnid].population_wd.ToString() + tabstring + gndict[gnid].population.ToString());
                        if ((n1 % 1000) == 0)
                            Console.WriteLine("n1 = " + n1.ToString());
                    }
                }
                if ((n % 100000) == 0)
                    Console.WriteLine("n = " + n.ToString());
            }
            Console.WriteLine("n (pop) = " + n1.ToString());
            n = 0;
            using (StreamWriter sw = new StreamWriter("gnvswiki_area.txt"))
            {

                foreach (double wdarea in areavsarea.Keys)
                {
                    n++;
                    sw.WriteLine(wdarea.ToString() + tabstring + areavsarea[wdarea].ToString());
                }
                if ((n % 10000) == 0)
                    Console.WriteLine("n = " + n.ToString());
            }
            Console.WriteLine("n(area) = " + n.ToString());
            n = 0;
            using (StreamWriter sw = new StreamWriter("gnvswiki_duplicates.txt"))
            {

                foreach (int wdid in wdgniddict.Keys)
                {
                    if (wdgniddict[wdid] < 0)
                    {
                        n++;
                        sw.WriteLine(wdid.ToString() + tabstring + (-wdgniddict[wdid]).ToString());
                    }
                }
                if ((n % 10000) == 0)
                    Console.WriteLine("n = " + n.ToString());
            }
            Console.WriteLine("n(duplicate) = " + n.ToString());
            Console.WriteLine("nwdtot = " + nwdtot.ToString());
            Console.WriteLine("ngnid = " + ngnid.ToString());

            //verify_wd();

        }

        public static int[,] get_hgt_array(string filename)
        {

            Console.WriteLine("get_hgt_array: " + filename);
            int pixvalue = 0;
            int oldpix = 0;
            int mapsize = 1201;
            int[,] map = new int[mapsize, mapsize];

            try
            {
                FileInfo finfo = new FileInfo(filename);
                Console.WriteLine("File size = " + finfo.Length);
                if (finfo.Length > 3000000)
                {
                    Console.WriteLine("Weird file size <cr>");
                    Console.ReadLine();
                }
                //Console.ReadLine();
                byte[] pixels = File.ReadAllBytes(filename);

                Console.WriteLine("pixels = " + pixels.Length);
                int x = 0;
                int y = 0;
                bool odd = true;
                //bool negative = false;
                foreach (byte b in pixels)
                {
                    if (odd)
                    {
                        //if (b < 128)
                        pixvalue = b;
                        //else
                        //{
                        //    negative = true;
                        //    pixvalue = b - 128;
                        //}
                        odd = !odd;
                    }
                    else
                    {
                        //if ( b < 128 )
                        pixvalue = pixvalue * 256 + b;
                        //else
                        //    pixvalue = -(pixvalue * 256 + b - 128);
                        //if (negative)
                        //    pixvalue = -pixvalue;
                        if (pixvalue > 32768)
                            pixvalue = pixvalue - 65536;
                        else if (pixvalue > 9000)
                            pixvalue = oldpix;
                        //Console.WriteLine(pixvalue);
                        map[x, y] = pixvalue;
                        oldpix = pixvalue;
                        x++;
                        if (x >= mapsize)
                        {
                            x = 0;
                            y++;
                        }
                        odd = !odd;
                        //negative = false;
                    }
                }


            }
            catch (FileNotFoundException e)
            {
                Console.Error.WriteLine(e.Message);
                //Console.WriteLine("Not found!");
                for (int x = 0; x < mapsize; x++)
                    for (int y = 0; y < mapsize; y++)
                        map[x, y] = 0;
            }
            catch (OutOfMemoryException e)
            {
                Console.Error.WriteLine(e.Message);
                //Console.WriteLine("Not found!");
                for (int x = 0; x < mapsize; x++)
                    for (int y = 0; y < mapsize; y++)
                        map[x, y] = 0;
            }
            return map;
        }

        public static string padint(int n, int len)
        {
            string s = n.ToString();
            while (s.Length < len)
                s = "0" + s;
            return s;
        }

        public static string nexthgt(string filenamepar, string dir)
        {
            //Console.WriteLine("nexthgt: filename before = " + filenamepar);
            string filename = filenamepar;

            switch (dir)
            {
                case "north":
                    if (filename.Contains("N"))
                    {
                        int lat = util.tryconvert(filename.Substring(1, 2));
                        lat++;
                        filename = filename.Replace(filename.Substring(0, 3), "N" + padint(lat, 2));
                    }
                    else //"S"
                    {
                        int lat = util.tryconvert(filename.Substring(1, 2));
                        if (lat > 1)
                        {
                            lat--;
                            filename = filename.Replace(filename.Substring(0, 3), "S" + padint(lat, 2));
                        }
                        else
                        {
                            filename = filename.Replace(filename.Substring(0, 3), "N00");
                        }
                    }
                    break;
                case "south":
                    if (filename.Contains("S"))
                    {
                        int lat = util.tryconvert(filename.Substring(1, 2));
                        lat++;
                        filename = filename.Replace(filename.Substring(0, 3), "S" + padint(lat, 2));
                    }
                    else //"N"
                    {
                        int lat = util.tryconvert(filename.Substring(1, 2));
                        if (lat > 0)
                        {
                            lat--;
                            filename = filename.Replace(filename.Substring(0, 3), "N" + padint(lat, 2));
                        }
                        else
                        {
                            filename = filename.Replace(filename.Substring(0, 3), "S01");
                        }
                    }
                    break;
                case "east":
                    if (filename.Contains("E"))
                    {
                        int lon = util.tryconvert(filename.Substring(4, 3));
                        lon++;
                        if (lon >= 180)
                            filename = filename.Replace(filename.Substring(3, 4), "W180");
                        else
                            filename = filename.Replace(filename.Substring(3, 4), "E" + padint(lon, 3));
                    }
                    else //"W"
                    {
                        int lon = util.tryconvert(filename.Substring(4, 3));
                        if (lon > 1)
                        {
                            lon--;
                            filename = filename.Replace(filename.Substring(3, 4), "W" + padint(lon, 3));
                        }
                        else
                        {
                            filename = filename.Replace(filename.Substring(3, 4), "E000");
                        }
                    }
                    break;
                case "west":
                    if (filename.Contains("W"))
                    {
                        int lon = util.tryconvert(filename.Substring(4, 3));
                        lon++;
                        if (lon > 180)
                            filename = filename.Replace(filename.Substring(3, 4), "E179");
                        else
                            filename = filename.Replace(filename.Substring(3, 4), "W" + padint(lon, 3));
                    }
                    else //"E"
                    {
                        int lon = util.tryconvert(filename.Substring(4, 3));
                        if (lon > 0)
                        {
                            lon--;
                            filename = filename.Replace(filename.Substring(3, 4), "E" + padint(lon, 3));
                        }
                        else
                        {
                            filename = filename.Replace(filename.Substring(3, 4), "W001");
                        }
                    }
                    break;
            }

            //Console.WriteLine("nexthgt: filename after = " + filename);
            return filename;
        }

        public static string make_hgt_filename(double lat, double lon)
        {
            int intlat = Convert.ToInt32(Math.Abs(Math.Floor(lat)));
            int intlon = Convert.ToInt32(Math.Abs(Math.Floor(lon)));

            string filename = "N00E999.hgt";

            if (lat < 0)
                filename = filename.Replace('N', 'S');

            if (lon < 0)
                filename = filename.Replace('E', 'W');

            filename = filename.Replace("00", padint(intlat, 2));
            filename = filename.Replace("999", padint(intlon, 3));

            Console.WriteLine(filename);
            return filename;

        }

        public static int[,] get_9x9map(double lat, double lon)
        {
            //int[,] centermap = get_3x3map(lat, lon);

            int map3x3size = 3603; //centermap.GetLength(0);

            int[,] map = new int[3 * map3x3size, 3 * map3x3size];
            for (int x = 0; x < 3 * map3x3size; x++)
                for (int y = 0; y < 3 * map3x3size; y++)
                    map[x, y] = 0;

            int xoff = map3x3size;
            int yoff = map3x3size;

            for (int u = -1; u <= 1; u++)
                for (int v = -1; v <= 1; v++)
                {
                    int[,] map3x3 = get_3x3map(lat - 3 * u, lon + 3 * v);


                    for (int x = 0; x < map3x3size; x++)
                        for (int y = 0; y < map3x3size; y++)
                            map[x + (u + 1) * xoff, y + (v + 1) * yoff] = map3x3[x, y];

                }

            return map;

        }

        public static int[,] get_3x3map(double lat, double lon)
        {
            int mapsize = 1201;

            string dir = extractdir;
            string filename = make_hgt_filename(lat, lon);

            int[,] map;

            if (filename == mapfilecache)
                map = mapcache;
            else
            {
                mapfilecache = filename;
                Console.WriteLine("Garbage collection:");
                GC.Collect();
                Console.WriteLine("Making map array..." + mapsize);
                map = new int[3 * mapsize, 3 * mapsize];
                Console.WriteLine("Map array done.");
                for (int x = 0; x < 3 * mapsize; x++)
                    for (int y = 0; y < 3 * mapsize; y++)
                        map[x, y] = 0;


                // ...
                // .x.
                // ...

                Console.WriteLine("Getting first map square...");
                int[,] map0 = get_hgt_array(dir + filename);

                int xoff = mapsize;
                int yoff = mapsize;

                for (int x = 0; x < mapsize; x++)
                    for (int y = 0; y < mapsize; y++)
                        map[x + xoff, y + yoff] = map0[x, y];

                // ...
                // x..
                // ...

                Console.WriteLine("Getting 2nd map square...");
                filename = nexthgt(filename, "west");
                map0 = get_hgt_array(dir + filename);

                xoff = 0;
                yoff = mapsize;

                for (int x = 0; x < mapsize; x++)
                    for (int y = 0; y < mapsize; y++)
                        map[x + xoff, y + yoff] = map0[x, y];

                // x..
                // ...
                // ...

                filename = nexthgt(filename, "north");
                map0 = get_hgt_array(dir + filename);

                xoff = 0;
                yoff = 0;

                for (int x = 0; x < mapsize; x++)
                    for (int y = 0; y < mapsize; y++)
                        map[x + xoff, y + yoff] = map0[x, y];

                // .x.
                // ...
                // ...

                filename = nexthgt(filename, "east");
                map0 = get_hgt_array(dir + filename);

                xoff = mapsize;
                yoff = 0;

                for (int x = 0; x < mapsize; x++)
                    for (int y = 0; y < mapsize; y++)
                        map[x + xoff, y + yoff] = map0[x, y];

                // ..x
                // ...
                // ...

                filename = nexthgt(filename, "east");
                map0 = get_hgt_array(dir + filename);

                xoff = 2 * mapsize;
                yoff = 0;

                for (int x = 0; x < mapsize; x++)
                    for (int y = 0; y < mapsize; y++)
                        map[x + xoff, y + yoff] = map0[x, y];

                // ...
                // ..x
                // ...


                filename = nexthgt(filename, "south");
                map0 = get_hgt_array(dir + filename);

                xoff = 2 * mapsize;
                yoff = mapsize;

                for (int x = 0; x < mapsize; x++)
                    for (int y = 0; y < mapsize; y++)
                        map[x + xoff, y + yoff] = map0[x, y];

                // ...
                // ...
                // ..x

                filename = nexthgt(filename, "south");
                map0 = get_hgt_array(dir + filename);

                xoff = 2 * mapsize;
                yoff = 2 * mapsize;

                for (int x = 0; x < mapsize; x++)
                    for (int y = 0; y < mapsize; y++)
                        map[x + xoff, y + yoff] = map0[x, y];

                // ...
                // ...
                // .x.

                filename = nexthgt(filename, "west");
                map0 = get_hgt_array(dir + filename);

                xoff = mapsize;
                yoff = 2 * mapsize;

                for (int x = 0; x < mapsize; x++)
                    for (int y = 0; y < mapsize; y++)
                        map[x + xoff, y + yoff] = map0[x, y];

                // ...
                // ...
                // x..

                filename = nexthgt(filename, "west");
                map0 = get_hgt_array(dir + filename);

                xoff = 0;
                yoff = 2 * mapsize;

                for (int x = 0; x < mapsize; x++)
                    for (int y = 0; y < mapsize; y++)
                        map[x + xoff, y + yoff] = map0[x, y];

                mapcache = map;
            }

            return map;
        }

        public static int get_x_pixel(double lon, double orilon)
        {
            return get_x_pixel(lon, orilon, 1201);
        }

        public static int get_x_pixel(double lon, double orilon, int mapsize) //mapsize should be one third of actual mapsize!
        {
            double fraction = lon - Math.Floor(lon);
            int pix = Convert.ToInt32((Math.Floor(lon) - Math.Floor(orilon) + 1) * mapsize + mapsize * fraction);
            return pix;
        }

        public static int get_y_pixel(double lat, double orilat)
        {
            return get_y_pixel(lat, orilat, 1201);
        }

        public static int get_y_pixel(double lat, double orilat, int mapsize) //mapsize should be one third of actual mapsize!
        {
            double fraction = lat - Math.Floor(lat);
            int pix = 3 * mapsize - Convert.ToInt32((Math.Floor(lat) - Math.Floor(orilat) + 1) * mapsize + mapsize * fraction);
            return pix;
        }


        public static long seed_center_dist(int gnid, bool exactlevel) //verify island or lake by calculating distance between seed point and center of gravity
        {
            double lat = gndict[gnid].latitude;
            double lon = gndict[gnid].longitude;
            double scale = Math.Cos(lat * 3.1416 / 180);
            double pixkmx = scale * 40000 / (360 * 1200);
            double pixkmy = 40000.0 / (360.0 * 1200.0);

            Console.WriteLine("scale,pixkmx,pixkmy = " + scale.ToString() + "; " + pixkmx.ToString() + "; " + pixkmy.ToString());
            int[,] mainmap = get_3x3map(lat, lon);

            int mapsize = mainmap.GetLength(0);

            byte[,] fillmap = new byte[mapsize, mapsize];

            for (int x = 0; x < mapsize; x++)
                for (int y = 0; y < mapsize; y++)
                    fillmap[x, y] = 1;

            int x0 = get_x_pixel(lon, lon);
            int y0 = get_y_pixel(lat, lat);
            floodfill(ref fillmap, ref mainmap, x0, y0, 0, 0, exactlevel);

            if (fillmap[0, 0] == 3) //fill failure
                return 99999999;

            long xsum = 0;
            long ysum = 0;
            int nfill = 0;

            for (int x = 0; x < mapsize; x++)
                for (int y = 0; y < mapsize; y++)
                    if (fillmap[x, y] == 2)
                    {
                        nfill++;
                        xsum += x;
                        ysum += y;
                    }
            if (nfill == 0) //fill failure
                return 99999999;

            long xc = xsum / nfill;
            long yc = ysum / nfill;

            return (x0 - xc) * (x0 - xc) + (y0 - yc) * (y0 - yc);

        }


        public static bool inmap(int x, int y, int size, int margin)
        {
            if (x < margin)
                return false;
            if (y < margin)
                return false;
            if (x > size - margin - 1)
                return false;
            if (y > size - margin - 1)
                return false;

            return true;
        }

        public static bool filldirland(ref byte[,] fillmap, ref int[,] mainmap, int xx, int yy, int dirx, int diry, ref int nnew, int level)
        {
            bool atedge = false;
            if ((fillmap[xx + dirx, yy + diry] == 1) && (mainmap[xx + dirx, yy + diry] > level))
            {
                fillmap[xx + dirx, yy + diry] = 2;
                nnew++;
                int u = 2;
                while (inmap(xx + u * dirx, yy + u * diry, mainmap.GetLength(0), 2) && (mainmap[xx + u * dirx, yy + u * diry] > level))
                {
                    fillmap[xx + u * dirx, yy + u * diry] = 2;
                    nnew++;
                    u++;
                }
                if (!inmap(xx + u * dirx, yy + u * diry, mainmap.GetLength(0), 2))
                    atedge = true;
            }
            return atedge;
        }

        public static bool filldirlake(ref byte[,] fillmap, ref int[,] mainmap, int xx, int yy, int dirx, int diry, ref int nnew, int level)
        {
            bool atedge = false;
            if ((fillmap[xx + dirx, yy + diry] == 1) && (mainmap[xx + dirx, yy + diry] == level))
            {
                fillmap[xx + dirx, yy + diry] = 2;
                nnew++;
                int u = 2;
                while (inmap(xx + u * dirx, yy + u * diry, mainmap.GetLength(0), 2) && (mainmap[xx + u * dirx, yy + u * diry] == level))
                {
                    fillmap[xx + u * dirx, yy + u * diry] = 2;
                    nnew++;
                    u++;
                }
                if (!inmap(xx + u * dirx, yy + u * diry, mainmap.GetLength(0), 2))
                    atedge = true;
            }
            return atedge;
        }



        public static void floodfill(ref byte[,] fillmap, ref int[,] mainmap, int x, int y, int level, int depth, bool exactlevel)
        {
            Console.WriteLine("flood " + x.ToString() + " " + y.ToString() + " " + mainmap[x, y].ToString() + " " + depth);
            if ((x < 0) || (x >= mainmap.GetLength(0)))
            {
                fillmap[0, 0] = 3; //bad fill
                Console.WriteLine("Invalid x");
                return;
            }
            if ((y < 0) || (y >= mainmap.GetLength(1)))
            {
                fillmap[0, 0] = 3; //bad fill
                Console.WriteLine("Invalid y");
                return;
            }

            if (fillmap[x, y] != 1) //1 = unchecked
            {
                Console.WriteLine("Starting point checked.");
                return;
            }

            bool atedge = false;

            if (exactlevel) //find all at same level
            {
                if (mainmap[x, y] == level)
                {
                    fillmap[x, y] = 2; //2 = filled
                    int nnew = 0;
                    int nfill = 0;
                    do
                    {
                        nnew = 0;
                        nfill = 0;
                        for (int xx = 1; xx < mainmap.GetLength(0) - 1; xx++)
                            for (int yy = 1; yy < mainmap.GetLength(0) - 1; yy++)
                            {
                                if (fillmap[xx, yy] == 2)
                                {
                                    nfill++;
                                    if (!inmap(xx, yy, mainmap.GetLength(0), 2))
                                        atedge = true;

                                    if (!atedge)
                                    {
                                        atedge = atedge || filldirlake(ref fillmap, ref mainmap, xx, yy, 1, 0, ref nnew, level);
                                        atedge = atedge || filldirlake(ref fillmap, ref mainmap, xx, yy, -1, 0, ref nnew, level);
                                        atedge = atedge || filldirlake(ref fillmap, ref mainmap, xx, yy, 0, 1, ref nnew, level);
                                        atedge = atedge || filldirlake(ref fillmap, ref mainmap, xx, yy, 0, -1, ref nnew, level);

                                    }
                                }
                            }
                        Console.WriteLine("nnew = " + nnew.ToString() + ", nfill = " + nfill.ToString());
                    }
                    while ((nnew > 0) && (!atedge));

                }
                else
                    fillmap[x, y] = 0; //0 = checked but wrong level
            }
            else //find all ABOVE the given level
            {
                if (mainmap[x, y] > level)
                {
                    fillmap[x, y] = 2;
                    //floodfill(x - 1, y, level, depth + 1);
                    //floodfill(x + 1, y, level, depth + 1);
                    //floodfill(x, y - 1, level, depth + 1);
                    //floodfill(x, y + 1, level, depth + 1);
                    int nnew = 0;
                    int nfill = 0;
                    do
                    {
                        nnew = 0;
                        nfill = 0;
                        for (int xx = 1; xx < mainmap.GetLength(0) - 1; xx++)
                            for (int yy = 1; yy < mainmap.GetLength(0) - 1; yy++)
                            {
                                if (fillmap[xx, yy] == 2)
                                {
                                    nfill++;
                                    if (!inmap(xx, yy, mainmap.GetLength(0), 2))
                                        atedge = true;

                                    if (!atedge)
                                    {
                                        atedge = atedge || filldirland(ref fillmap, ref mainmap, xx, yy, 1, 0, ref nnew, level);
                                        atedge = atedge || filldirland(ref fillmap, ref mainmap, xx, yy, -1, 0, ref nnew, level);
                                        atedge = atedge || filldirland(ref fillmap, ref mainmap, xx, yy, 0, 1, ref nnew, level);
                                        atedge = atedge || filldirland(ref fillmap, ref mainmap, xx, yy, 0, -1, ref nnew, level);

                                    }
                                }
                            }
                        Console.WriteLine("nnew = " + nnew.ToString() + ", nfill = " + nfill.ToString());
                    }
                    while ((nnew > 0) && (!atedge));
                }
                else
                    fillmap[x, y] = 0;
            }

            if (atedge)
                fillmap[0, 0] = 3;
        }

        public static string kml_header(string name)
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<kml xmlns=\"http://www.opengis.net/kml/2.2\">\n<Document><Folder><name>#NAME#</name>\n".Replace("#NAME#", name);
        }

        public static string kml_placemark(List<coordclass> points, string description, string source, string name)
        {
            return kml_placemark(points, description, source, name, "green");
        }

        public static string kml_placemark(List<coordclass> points, string description, string source, string name, string colorparam)
        {
            string color = colorparam;
            switch (colorparam)
            {
                case "red":
                    color = "ff0000ff";
                    break;
                case "green":
                    color = "5014f000";
                    break;
                case "blue":
                    color = "50f01414";
                    break;
                case "yellow":
                    color = "5014f0ff";
                    break;
                case "orange":
                    color = "501478ff";
                    break;
                case "purple":
                    color = "50780078";
                    break;
                case "turqoise":
                    color = "50F0FF14";
                    break;
                case "black":
                    color = "50000000";
                    break;

            }
            string placeheader = ("<Placemark>\n	<description>#DESCRIPTION#</description>\n	<Style><LineStyle><color>" + color + "</color></LineStyle><PolyStyle><fill>0</fill></PolyStyle></Style>\n	<ExtendedData><SchemaData schemaUrl=\"#SCHEMA#\">\n		<SimpleData name=\"Source\">#SOURCE#</SimpleData>\n	</SchemaData></ExtendedData>").Replace("#DESCRIPTION#", description).Replace("#SCHEMA#", name).Replace("#SOURCE#", source);
            string polygonheader = "<Polygon><altitudeMode>relativeToGround</altitudeMode><outerBoundaryIs><LinearRing><altitudeMode>relativeToGround</altitudeMode><coordinates>";
            string polygonend = "</coordinates></LinearRing></outerBoundaryIs></Polygon>\n";
            string placeend = "</Placemark>";

            string coordstring = "";
            foreach (coordclass cc in points)
                coordstring += cc.lon.ToString(culture_en) + "," + cc.lat.ToString(culture_en) + " ";
            coordstring = coordstring.Trim();

            return placeheader + polygonheader + coordstring + polygonend + placeend;
        }

        public static string kml_end(string name, string category)
        {
            return "</Folder>\n<Schema name=\"#NAME#\" id=\"#NAME#\">\n\t<SimpleField name=\"Name\" type=\"string\"></SimpleField>\n	<SimpleField name=\"Description\" type=\"string\"></SimpleField>\n	<SimpleField name=\"Source\" type=\"string\"></SimpleField>\n</Schema>//[[" + util.mp(1) + "#KATEGORI#]]\n</Document></kml>".Replace("#NAME#", name).Replace("#KATEGORI#", category);
        }

        public static string make_kml_file(List<coordclass> points, string description, string source, string name, string category)
        {
            string kmlfilename = kmlprefix + countryml[makecountryname] + "/" + name;
            kmlfilename = kmlfilename.Replace(" ", "_");
            Page p = new Page(makesite, kmlfilename);
            p.text = kml_header(name) + kml_placemark(points, description, source, name) + kml_end(name, category);
            util.trysave(p, 2, util.mp(60, null) + " KML");
            return kmlfilename;
        }

        public static bool in_map(int x, int y, int mapsize)
        {
            if (x < 1)
                return false;
            if (x > mapsize - 2)
                return false;
            if (y < 1)
                return false;
            if (y > mapsize - 2)
                return false;

            return true;
        }

        public static void make_rivers_old() //create rivers-XX file for a country, old version that tries to follow valley in elevation model. Not producing useful results.
        {
            int nriver = 0;

            using (StreamWriter sw = new StreamWriter("rivers-" + makecountry + ".txt"))
            {

                int ngnid = gndict.Count;

                double minlat = 999;
                double maxlat = -999;
                double minlon = 999;
                double maxlon = -999;


                //Page pkr = new Page(makesite, "Användare:Lsjbot/Kartrutor");
                //util.tryload(pkr, 2);
                //pkr.text += "\n== Vattendrag i " + countryml[makecountryname] + " ==\n";

                foreach (int gnid in gndict.Keys)
                {
                    if (categorydict[gndict[gnid].featurecode] == "streams")
                    {
                        if (gndict[gnid].latitude < minlat)
                            minlat = gndict[gnid].latitude;
                        else if (gndict[gnid].latitude > maxlat)
                            maxlat = gndict[gnid].latitude;
                        if (gndict[gnid].longitude < minlon)
                            minlon = gndict[gnid].longitude;
                        else if (gndict[gnid].longitude > maxlon)
                            maxlon = gndict[gnid].longitude;
                    }
                }

                int ndone = 0;
                List<int> donelist = new List<int>();
                List<coordclass> mapcoord = new List<coordclass>();

                double slon = minlon;
                double slat = maxlat; //northeast corner

                while (slon < maxlon)
                {
                    while (slat > minlat)
                    {
                        coordclass cc = new coordclass();
                        cc.lon = slon;
                        cc.lat = slat;
                        mapcoord.Add(cc);
                        slat -= 3.0;
                    }
                    slat = maxlat;
                    slon += 3.0;
                }

                Console.WriteLine("n mapcoord = " + mapcoord.Count);

                foreach (coordclass mc in mapcoord)
                {
                    int[,] mainmap = get_9x9map(mc.lat, mc.lon);
                    Console.WriteLine("Map loaded");
                    int mapsize = mainmap.GetLength(0);
                    int fiducialsize = mapsize / 3;
                    int[,] fillmap = new int[mapsize, mapsize];
                    int[,] rivermap = new int[mapsize, mapsize];
                    int nullvalue = -1;
                    for (int x = 0; x < mapsize; x++)
                    {
                        for (int y = 0; y < mapsize; y++)
                        {
                            fillmap[x, y] = nullvalue;
                            rivermap[x, y] = nullvalue;
                        }
                    }

                    Console.WriteLine("put_category_on_map");
                    put_category_on_map(ref fillmap, mc.lat, mc.lon, "streams", 0.4);
                    put_category_on_map(ref rivermap, mc.lat, mc.lon, "streams", 0.4);
                    int nmark = 0;
                    for (int x0 = fiducialsize; x0 < 2 * fiducialsize; x0++)
                        for (int y0 = fiducialsize; y0 < 2 * fiducialsize; y0++)
                        {
                            if (rivermap[x0, y0] > 0)
                                nmark++;
                        }
                    Console.WriteLine("nmark = " + nmark);
                    Console.WriteLine("list_category_in_map");
                    List<int> fiducial_rivers = list_category_in_map(fiducialsize / 3, mc.lat, mc.lon, "streams");
                    Console.WriteLine(fiducial_rivers.Count + " fiducial rivers");
                    List<int> found_rivers = new List<int>();
                    int fillvalue = -2;
                    int deadvalue = -3;
                    int endvalue = -1;
                    int edgevalue = -4;
                    int tolerance = 3; //maximum "uphill" step of river

                    for (int x0 = fiducialsize; x0 < 2 * fiducialsize; x0++)
                        for (int y0 = fiducialsize; y0 < 2 * fiducialsize; y0++)
                        {
                            if (rivermap[x0, y0] > 0)
                            {
                                fillmap[x0, y0] = rivermap[x0, y0];
                                //Console.WriteLine("river at starting point");
                                continue;
                            }
                            if (mainmap[x0, y0] <= 0) //skip ocean
                                continue;
                            if (fillmap[x0, y0] != nullvalue) //done already
                                continue;

                            int x = x0;
                            int y = y0;
                            int maxx = x;
                            int minx = x;
                            int maxy = y;
                            int miny = y;
                            endvalue = -1;

                            while (true) //breaks inside loop //((fillmap[x, y] == nullvalue) && (rivermap[x, y] == nullvalue))
                            {
                                int h = 99999;

                                fillmap[x, y] = fillvalue;
                                int lowx = -1;
                                int lowy = -1;
                                for (int u = -1; u <= 1; u++)
                                    for (int v = -1; v <= 1; v++)
                                    {
                                        if ((u != 0) || (v != 0))
                                            if ((mainmap[x + u, y + v] <= h) && (fillmap[x + u, y + v] != fillvalue))
                                            {
                                                h = mainmap[x + u, y + v];
                                                lowx = x + u;
                                                lowy = y + v;
                                            }
                                    }
                                //Console.WriteLine("x,y,h = " + x + " " + y + " " + h);
                                if ((h > mainmap[x, y] + tolerance)) //reached dead end
                                {
                                    endvalue = deadvalue;
                                    break;
                                }
                                else if (rivermap[lowx, lowy] > 0) //found river
                                {
                                    endvalue = rivermap[lowx, lowy];
                                    break;
                                }
                                else if (mainmap[lowx, lowy] <= 0) // reached ocean
                                {
                                    endvalue = 0;
                                    break;
                                }
                                else if (fillmap[lowx, lowy] != nullvalue) //reached previous filling
                                {
                                    endvalue = fillmap[lowx, lowy];
                                    break;
                                }
                                else if (!in_map(lowx, lowy, mapsize)) //reached edge of map
                                {
                                    endvalue = edgevalue;
                                    break;
                                }
                                else
                                {
                                    x = lowx;
                                    y = lowy;
                                    fillmap[x, y] = fillvalue;
                                    if (x > maxx)
                                        maxx = x;
                                    if (x < minx)
                                        minx = x;
                                    if (y > maxy)
                                        maxy = y;
                                    if (y < miny)
                                        miny = y;

                                }
                            }

                            if (endvalue > 0)
                            {
                                Console.WriteLine("x0,y0 = " + x0 + " " + y0 + " endvalue: " + endvalue);
                                if (!found_rivers.Contains(endvalue))
                                    found_rivers.Add(endvalue);
                            }

                            //Console.ReadLine();

                            for (int xx = minx; xx <= maxx; xx++)
                                for (int yy = miny; yy <= maxy; yy++)
                                {
                                    if (fillmap[xx, yy] == fillvalue)
                                        fillmap[xx, yy] = endvalue;
                                }
                        }
                    Console.WriteLine(fiducial_rivers.Count + " fiducial rivers");

                    Console.WriteLine(found_rivers.Count + " found rivers");

                }
            }
        }

#if (DBGEOFLAG)

        public static void make_rivers() //create rivers-XX file for a country.
        {
            int nriver = 0;

            using (StreamWriter sw = new StreamWriter("rivers-" + makecountry + ".txt"))
            {

                int ngnid = gndict.Count;

                foreach (int gnid in gndict.Keys)
                {
                    if (categorydict[gndict[gnid].featurecode] == "streams")
                    {
                        nriver++;
                        riverclass rc = new riverclass();

                        DbGeography mouth = pointfromlatlong(gndict[gnid].latitude, gndict[gnid].longitude);
                        DbGeometry mmouth = mpointfromlatlong(gndict[gnid].latitude, gndict[gnid].longitude);

                        foreach (string dname in drainageshapedict.Keys)
                        {
                            if (!drainagedict.ContainsKey(dname))
                                continue;

                            if (drainagedict[dname].main_river == gnid)
                            {
                                rc.drainage_name = dname;
                                break;
                            }

                            //foreach (DbGeometry ms in drainageshapedict[dname].mshapes)
                            //{
                            //    if ( ms.Contains(mmouth))
                            //    {
                            //        rc.drainage_name = dname;
                            //    }
                            //}
                            foreach (DbGeography gs in drainageshapedict[dname].shapes)
                            {
                                double? dist = gs.Distance(mouth) / 1000;
                                if ( dist <= 100)
                                     Console.WriteLine(dname + " " + dist);
                                if ( dist <= 0)
                                {
                                    rc.drainage_name = dname;
                                    break;
                                }
                            }
                        }

                        if ( !String.IsNullOrEmpty(rc.drainage_name))
                            if ( drainagedict[rc.drainage_name].main_river != gnid) //main river is not tributary of anything
                            {
                                foreach (int rgnid in rivernamedict.Keys)
                                {
                                    if (rgnid == gnid) //not tributary of itself
                                        continue;

                                    if (rivernamedict[rgnid].drainage_name == rc.drainage_name)
                                    {
                                        string rname = rivernamedict[rgnid].river_name;
                                        if (rivershapedict.ContainsKey(rname))
                                        {
                                            foreach (DbGeography ss in rivershapedict[rname].shapes)
                                            {
                                                if (ss.Distance(mouth) < 2000)
                                                {
                                                    rc.tributary_of = rgnid;
                                                    rivernamedict[rgnid].tributaries.Add(gnid);
                                                }
                                            }

                                        }
                                    }

                                }

                            }


                        sw.Write(gnid.ToString() + tabstring + rc.drainage_name + tabstring + rc.tributary_of.ToString());
                        if (rivernamedict.ContainsKey(rc.tributary_of))
                            sw.Write(tabstring+rivernamedict[rc.tributary_of].river_artname);
                        sw.WriteLine();
                    }

                }

            }
        }

        public static DbGeography polygon_from_coords(List<coordclass> points)
        {
            string s = "POLYGON ((";
            bool first = true;
            foreach (coordclass cc in points)
            {
                if (first)
                {
                    first = false;
                }
                else
                    s += ", ";

                s += cc.lon.ToString(culture_en) + " " + cc.lat.ToString(culture_en);
            }

            s += "))";

            return DbGeography.FromText(s);
        }
#endif


        public static void make_lakes() //create lakes-XX file for a country
        {
            int nlake = 0;
            //int npop = 0;
            //int narea = 0;

            bool resume_lakes = (resume_at > 0);
            Console.WriteLine("resume_lakes = " + resume_lakes);
            resume_at = -1;

#if (DBGEOFLAG)
            //make_glwd_countries();
            read_lakeshapes();

            hbookclass areahist = new hbookclass();
            areahist.SetBins(-1, 1, 20);
#endif
            string filename = "lakes-" + makecountry + ".txt";

            List<int> donelist = new List<int>();
            List<string> donetitlelist = new List<string>();

            if (resume_lakes && File.Exists(filename))
            {
                int nold = 0;
                using (StreamReader sr = new StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        string[] words = line.Split('\t');

                        if (words.Length < 1)
                            continue;

                        int gnid = util.tryconvert(words[0]);
                        if (!gndict.ContainsKey(gnid))
                            continue;
                        donelist.Add(gnid);
                        nold++;
                    }
                    Console.WriteLine("resume_lake; nold = " + nold);

                    //util.mp(311) + " " + countryml[makecountryname]
                    //PageList pl = new PageList(makesite);
                    //pl.FillFromCategory(util.mp(311) + " " + countryml[makecountryname]);
                    //foreach (Page ppp in pl)
                    //    donetitlelist.Add(ppp.title);
                    //foreach (int gnid in gndict.Keys)
                    //{
                    //    if (donetitlelist.Contains(gndict[gnid].articlename))
                    //    {
                    //        donelist.Add(gnid);
                    //        nold++;
                    //    }
                    //}

                }
                Console.WriteLine("resume_lake; nold = " + nold);
                //Console.WriteLine("<cr>");
                //Console.ReadLine();
            }

            List<string> donesquares = new List<string>();
            if (resume_lakes)
            {
                PageList pl = new PageList(makesite);
                pl.FillFromCategory("Mapa KML paghimo ni bot");
                foreach (Page pkr in pl)
                {
                    //  Wikipedia:KML/Kartrutor/AD-sjö-N42E001
                    string mapsquare = pkr.title.Substring(31);
                    Console.WriteLine(mapsquare);
                    if (!donesquares.Contains(mapsquare))
                        donesquares.Add(mapsquare + ".hgt");
                }
            }

            using (StreamWriter sw = new StreamWriter(filename, resume_lakes))
            {

                int ngnid = gndict.Count;

                double minlat = 999;
                double maxlat = -999;
                double minlon = 999;
                double maxlon = -999;

                int nprob = 0;
                int nkml = 0;

                Page pkr = new Page(makesite, util.mp(13) + "Lsjbot/Kartrutor2");
                util.tryload(pkr, 2);
                pkr.text += "\n== Sjöar i " + countryml[makecountryname] + " ==\n";

                foreach (int gnid in gndict.Keys)
                {
                    if (categorydict[gndict[gnid].featurecode] == "lakes")
                    {
                        if (gndict[gnid].latitude < minlat)
                            minlat = gndict[gnid].latitude;
                        else if (gndict[gnid].latitude > maxlat)
                            maxlat = gndict[gnid].latitude;
                        if (gndict[gnid].longitude < minlon)
                            minlon = gndict[gnid].longitude;
                        else if (gndict[gnid].longitude > maxlon)
                            maxlon = gndict[gnid].longitude;
                    }
                }

                int ndone = 0;
                int nround = 0;
                do
                {
                    ndone = 0;
                    ngnid = gndict.Count;
                    nround++;

                    string current_map = "";
                    int[,] donemap = new int[3603, 3603];
                    for (int x = 0; x < 3603; x++)
                        for (int y = 0; y < 3603; y++)
                            donemap[x, y] = -1;

                    string placemarks = "";

                    foreach (int gnid in gndict.Keys)
                    {
                        ngnid--;
                        //if ((ngnid % 100000) == 0)
                        //{
                        //    Console.WriteLine("Garbage collection:");
                        //    GC.Collect();
                        //}

                        //if (resume_at > 0)
                        //    if (resume_at != gnid)
                        //        continue;
                        //    else
                        //    {
                        //        resume_at = -1;
                        //        //Console.WriteLine("<cr>");
                        //        //Console.ReadLine();
                        //    }


                        if (categorydict[gndict[gnid].featurecode] == "lakes")
                        {
                            Console.WriteLine("=====" + makecountry + "==== " + nround + " ==== " + ngnid.ToString() + " remaining. ===========");

                            if (donelist.Contains(gnid))
                                continue;

                            nlake++;

                            double area = -1.0;
                            double lat = gndict[gnid].latitude;
                            double lon = gndict[gnid].longitude;
                            double scale = Math.Cos(lat * 3.1416 / 180);
                            double pixkmx = scale * 40000 / (360 * 1200);
                            double pixkmy = 40000.0 / (360.0 * 1200.0);

                            string mapname = make_hgt_filename(lat, lon); //do only those using the same map; loop over maps
                            Console.WriteLine(mapname);
                            if (donesquares.Contains(mapname))
                            {
                                donelist.Add(gnid);
                                continue;
                            }
                            if (String.IsNullOrEmpty(current_map))
                            {
                                Console.WriteLine("Starting new map");
                                current_map = mapname;
                            }
                            else if (mapname != current_map)
                                continue;

                            ndone++;
                            donelist.Add(gnid);

                            string kmlf = kmlprefix + countryml[makecountryname] + "/" + gndict[gnid].articlename;
                            Page pkml = new Page(makesite, kmlf);
                            util.tryload(pkml, 1);
                            if (pkml.Exists() && !overwrite)
                            {
                                continue;
                            }

                            //=================================================
                            //Console.WriteLine("scale,pixkmx,pixkmy = " + scale.ToString() + "; " + pixkmx.ToString() + "; " + pixkmy.ToString());
                            int[,] mainmap = get_3x3map(lat, lon);

                            int mapsize = mainmap.GetLength(0);

                            byte[,] fillmap = new byte[mapsize, mapsize];

                            for (int x = 0; x < mapsize; x++)
                                for (int y = 0; y < mapsize; y++)
                                    fillmap[x, y] = 1;

                            int x0 = get_x_pixel(lon, lon);
                            int y0 = get_y_pixel(lat, lat);
                            floodfill(ref fillmap, ref mainmap, x0, y0, mainmap[x0, y0], 0, true);

                            if (fillmap[0, 0] == 3) //fill failure
                                continue;

                            int xmax = -1;
                            int ymax = -1;
                            int xmin = 99999;
                            int ymin = 99999;
                            double r2max = -1;
                            int overlaps_with = -1;

                            byte edgevalue = 3;
                            byte fillvalue = 2;


                            int nfill = 0;
                            for (int x = 0; x < mapsize; x++)
                                for (int y = 0; y < mapsize; y++)
                                    if (fillmap[x, y] == fillvalue)
                                    {
                                        nfill++;
                                        if (x > xmax)
                                            xmax = x;
                                        if (y > ymax)
                                            ymax = y;
                                        if (x < xmin)
                                            xmin = x;
                                        if (y < ymin)
                                            ymin = y;
                                        double r2 = scale * scale * (x - x0) * (x - x0) + (y - y0) * (y - y0);
                                        if (r2 > r2max)
                                            r2max = r2;
                                        if (donemap[x, y] > 0)
                                            overlaps_with = donemap[x, y];
                                        else
                                            donemap[x, y] = gnid;
                                    }

                            double kmew = (xmax - xmin + 1) * pixkmx;
                            double kmns = (ymax - ymin + 1) * pixkmy;

                            Console.WriteLine("nfill = " + nfill.ToString());

                            int minpixel = 5;
                            if (nfill < minpixel) //skip lakes with just a few pixels
                                continue;

                            //area per pixel:
                            double km2perpixel = pixkmx * pixkmy;
                            area = nfill * km2perpixel;


                            double rmax = Math.Sqrt(r2max) * pixkmy + 2; //r2max in pixels; rmax in km
                            Console.WriteLine("r2max, rmax = " + r2max.ToString() + "; " + rmax.ToString());

                            List<int> nblist = getneighbors(gnid, rmax);
                            List<int> inlake = new List<int>();
                            List<int> aroundlake = new List<int>();

                            foreach (int nb in nblist) //first find islands and stuff in lake
                            {
                                if ((categorydict[gndict[nb].featurecode] != "islands") && (categorydict[gndict[nb].featurecode] != "seabed"))
                                    continue;
                                int xnb = get_x_pixel(gndict[nb].longitude, lon);
                                if ((xnb < 0) || (xnb >= mapsize))
                                    continue;
                                int ynb = get_y_pixel(gndict[nb].latitude, lat);
                                if ((ynb < 0) || (ynb >= mapsize))
                                    continue;
                                if (fillmap[xnb, ynb] == fillvalue)
                                    inlake.Add(nb);
                                else
                                {
                                    bool atedge = false;
                                    int u = 0;
                                    while ((xnb + u < mapsize) && (fillmap[xnb + u, ynb] != fillvalue))
                                        u++;
                                    if (xnb + u >= mapsize)
                                        atedge = true;
                                    else
                                    {
                                        u = 0;
                                        while ((xnb - u >= 0) && (fillmap[xnb - u, ynb] != fillvalue))
                                            u--;
                                        if (xnb - u < 0)
                                            atedge = true;
                                        else
                                        {
                                            u = 0;
                                            while ((ynb + u < mapsize) && (fillmap[xnb, ynb + u] != fillvalue))
                                                u++;
                                            if (ynb + u >= mapsize)
                                                atedge = true;
                                            else
                                            {
                                                u = 0;
                                                while ((ynb - u >= 0) && (fillmap[xnb, ynb - u] != fillvalue))
                                                    u--;
                                                if (ynb - u < 0)
                                                    atedge = true;
                                            }
                                        }
                                    }
                                    if (!atedge)
                                        inlake.Add(nb);

                                }

                            }

                            int maxdist = 20; //count things up to maxdist pixels away from the lake as "around" the lake
                            if (r2max < 300)   //smaller zone "around" if lake is smaller
                                maxdist = 10;
                            if (r2max < 30)
                                maxdist = 5;

                            for (byte step = edgevalue; step < maxdist + edgevalue; step++) //start at 3, because base level at 2.
                            {
                                for (int x = 1; x < mapsize - 1; x++)
                                    for (int y = 1; y < mapsize - 1; y++)
                                        if (fillmap[x, y] == step - 1)
                                        {
                                            for (int uu = -1; uu <= 1; uu++)
                                                for (int vv = -1; vv <= 1; vv++)
                                                {
                                                    if (fillmap[x + uu, y + vv] < fillvalue)
                                                        fillmap[x + uu, y + vv] = step;
                                                }
                                        }
                            }


                            foreach (int nb in nblist) //now things around the lake
                            {
                                if ((categorydict[gndict[nb].featurecode] == "islands") || (categorydict[gndict[nb].featurecode] == "seabed"))
                                    continue;
                                int xnb = get_x_pixel(gndict[nb].longitude, lon);
                                if ((xnb < 0) || (xnb >= mapsize))
                                    continue;
                                int ynb = get_y_pixel(gndict[nb].latitude, lat);
                                if ((ynb < 0) || (ynb >= mapsize))
                                    continue;
                                if (fillmap[xnb, ynb] > fillvalue)
                                    aroundlake.Add(nb);

                            }

                            //Check so not too many pixels around lake are below lake level. Also find max/min of contour and prepare for KML contour
                            int lower = 0;
                            int higher = 0;
                            //int xmax3 = -9999;
                            //int xmin3 = 9999;
                            //int ymax3 = -9999;
                            //int ymin3 = 9999;
                            int n3 = 0;

                            for (int x = 1; x < mapsize - 1; x++)
                                for (int y = 1; y < mapsize - 1; y++)
                                    if (fillmap[x, y] == edgevalue)
                                    {
                                        n3++;
                                        if (mainmap[x, y] > mainmap[x0, y0])
                                            higher++;
                                        else if (mainmap[x, y] < mainmap[x0, y0])
                                            lower++;
                                    }

                            List<coordclass> kmllist2 = make_kmllist(ref fillmap, edgevalue, fillvalue, x0, y0, gnid);

                            string color = "green";
                            if (overlaps_with > 0)
                                color = "red";
                            else if (higher < 4 * lower)
                                color = "orange";
                            else if (higher < 15 * lower)
                                color = "yellow";
                            else if (area < 0.1 * kmns * kmew)
                                color = "purple";
                            else if (area < 0.25 * kmns * kmew)
                                color = "blue";
                            else if (gndict[gnid].roundminute)
                                color = "turqoise";

                            //if ((color == "green") || (color == "blue") || (color == "purple") || (color == "turqoise"))
                            //{
                            int cgnid = countryid[makecountry];
                            int glwd_found = -1;
                            double glwd_area = 0;

#if (DBGEOFLAG)

                                if ( countrylakedict.ContainsKey(cgnid))
                                {
                                    //DbGeography dlc = DbGeography.FromText("POINT ("+lon.ToString(culture_en)+" "+lat.ToString(culture_en)+")");
                                    DbGeography dlc = polygon_from_coords(kmllist2);
                                    try
                                    {

                                        foreach (int glwd_id in countrylakedict[cgnid])
                                        {
                                            foreach (DbGeography dg in lakeshapedict[glwd_id].shapes)
                                                if (dlc.Intersects(dg))
                                                {
                                                    Console.WriteLine("Match found");
                                                    glwd_found = glwd_id;
                                                }

                                        }
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Exception in Intersects");
                                        break;
                                    }
                                }
                                if (glwd_found > 0)
                                {
                                    glwd_area = (double)lakeshapedict[glwd_found].shapes[0].Area/1000000;
                                    Console.WriteLine("My area = "+area.ToString()+", glwd-area = "+glwd_area.ToString());
                                    double mismatch = (glwd_area - area) / (glwd_area + area);
                                    areahist.Add(mismatch);
                                    if (Math.Abs(mismatch) < 0.2)
                                        color = "purple";
                                    
                                }
                                else
                                {
                                    Console.WriteLine("No match");
                                    if ((color == "green") || (color == "blue") || (color == "purple") || (color == "turqoise"))
                                        color = "black";
                                }
#endif
                            //}
                            //Console.ReadLine();


                            Console.WriteLine("area,kmns,kmew " + area.ToString("N3") + " " + kmew.ToString("N3") + " " + kmns.ToString("N3") + " " + color);
                            //Console.ReadLine();

                            placemarks += kml_placemark(kmllist2, "Edge of the lake " + gndict[gnid].Name_ml, "Lsjbot using altitude data from Viewfinder Panorama", gndict[gnid].articlename, color);

                            if ((color == "green") || (color == "blue") || (color == "purple") || (color == "turqoise") || (color == "black"))
                            {
                                string kmlcategory = util.mp(309) + " " + countryml[makecountryname];

                                string kmlfilename = make_kml_file(kmllist2, "Edge of the lake " + gndict[gnid].Name_ml, "Lsjbot using altitude data from Viewfinder Panorama", gndict[gnid].articlename, kmlcategory);
                                if (makelang != "sv")
                                    kmlfilename = kmlfilename.Replace(kmlprefix, "");
                                nkml++;

                                Page p = new Page(makesite, gndict[gnid].articlename);
                                util.tryload(p, 1);
                                if (p.Exists())
                                {
                                    if (p.text.Contains(util.mp(195)))
                                    {
                                        if (!p.text.Contains("{{KML"))
                                        {
                                            p.text += "\n{{KML|" + util.mp(310) + "=" + kmlfilename + "}}";
                                            util.trysave(p, 2, util.mp(306));
                                        }
                                        else if (p.text.Contains("Wikipedia:KML/" + util.mp(309) + "/"))
                                        {
                                            p.text = p.text.Replace("Wikipedia:KML/" + util.mp(309) + "/", kmlprefix);
                                            util.trysave(p, 2, util.mp(306));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Page p = new Page(makesite, gndict[gnid].articlename);
                                util.tryload(p, 1);
                                if (p.Exists())
                                {
                                    if (p.text.Contains(util.mp(195)))
                                    {
                                        p.AddToCategory(util.mp(311) + " " + countryml[makecountryname]);
                                        util.trysave(p, 2, util.mp(311));
                                        nprob++;
                                    }
                                }
                            }

                            //fillmap.Dispose();

                            //Console.WriteLine("<ret>");
                            //Console.ReadLine();
                            Console.WriteLine(gndict[gnid].Name + "; " + area.ToString() + "; " + kmew.ToString() + "; " + kmns.ToString() + "; " + inlake.Count.ToString() + "; " + aroundlake.Count.ToString());
                            sw.Write(gnid.ToString() + tabstring + area.ToString() + tabstring + kmew.ToString() + tabstring + kmns.ToString() + tabstring + higher.ToString() + tabstring + lower.ToString() + tabstring + overlaps_with + tabstring + glwd_found.ToString() + tabstring + glwd_area.ToString() + tabstring + "in");
                            foreach (int il in inlake)
                            {
                                sw.Write(tabstring + il.ToString());
                                //Console.WriteLine(gndict[il].Name + " in lake");
                            }
                            sw.Write(tabstring + "around");
                            foreach (int al in aroundlake)
                            {
                                sw.Write(tabstring + al.ToString());
                                //Console.WriteLine(gndict[al].Name + " around lake");
                            }
                            sw.WriteLine();
                            //Console.ReadLine();
                        }
                    }

                    if (!String.IsNullOrEmpty(placemarks))
                    {
                        string pmapname = kmlprefix + "Kartrutor/" + makecountry + "-sjö-" + current_map.Replace(".hgt", "");
                        Page pmap = new Page(makesite, pmapname);
                        if (makelang != "sv")
                            pmapname = pmapname.Replace(kmlprefix, "");
                        pmap.text = kml_header(current_map) + placemarks + kml_end(current_map, "Mapa KML paghimo ni bot");
                        util.trysave(pmap, 2, util.mp(60, null) + " KML");

                        pkr.text += "\n" + current_map.Replace(".hgt", "") + " {{KML|" + util.mp(310) + "=" + pmapname + "}}\n";
                        util.trysave(pkr, 1, util.mp(60, null) + " KML");
                    }
                    else
                    {
                        Console.WriteLine("No placemarks");
                        //Console.ReadLine();
                    }
                }
                while (ndone > 0);

                if (nprob > 0)
                {
                    string problemcat = util.mp(1) + util.mp(311) + " " + countryml[makecountryname];
                    Page ppc = new Page(makesite, problemcat);
                    util.tryload(ppc, 1);
                    if (!ppc.Exists())
                    {
                        ppc.AddToCategory(util.mp(311) + "|" + countryml[makecountryname]);
                        util.trysave(ppc, 2);
                    }
                }

                if (nkml > 0)
                {
                    string kmlcat = util.mp(1) + util.mp(309) + " " + countryml[makecountryname];
                    Page pkcat = new Page(makesite, kmlcat);
                    util.tryload(pkcat, 1);
                    if (!pkcat.Exists())
                    {
                        pkcat.AddToCategory(util.mp(309) + "|" + countryml[makecountryname]);
                        pkcat.AddToCategory(countryml[makecountryname] + "|KML");
                        util.trysave(pkcat, 2);
                    }
                }

            }
#if (DBGEOFLAG)

            areahist.PrintDHist();
#endif
        }

        public static bool good_lake(int gnid)
        {
            //                public double area = 0;
            //public double glwd_area = 0;
            //public double kmew = 0;
            //public double kmns = 0;
            //public int higher = 0; //edge pixels higher than lake surface
            //public int lower = 0; //edge pixels lower than lake surface
            //public int overlaps_with = -1; //if two lakes overlap, gnid of the other one
            //public int glwd_id = -1; //id number in GLWD lakes database, -1 if not found
            //public List<int> inlake = new List<int>(); //list of GeoNames id of entities located in the lake (mainly islands).
            //public List<int> atlake = new List<int>(); //list of GeoNames id of entities located around the lake.

            if (!lakedict.ContainsKey(gnid))
                return false;

            if (lakedict[gnid].overlaps_with > 0)
                return false;

            if (lakedict[gnid].glwd_id > 0)
            {
                double mismatch = (lakedict[gnid].glwd_area - lakedict[gnid].area) / (lakedict[gnid].glwd_area + lakedict[gnid].area);
                if (Math.Abs(mismatch) > 0.33)
                    return false;
            }
            else if (lakedict[gnid].higher + lakedict[gnid].lower > 0)
            {
                if (lakedict[gnid].higher < 10 * lakedict[gnid].lower)
                    return false;
            }

            return true;
        }

#if (DBGEOFLAG)


        public static DbGeography pointfromlatlong(double lat, double lon)
        {
            string s = "POINT ("+lon.ToString(culture_en)+" "+lat.ToString(culture_en)+")";
            return DbGeography.FromText(s);
        }
        public static DbGeometry mpointfromlatlong(double lat, double lon)
        {
            string s = "POINT (" + lon.ToString(culture_en) + " " + lat.ToString(culture_en) + ")";
            return DbGeometry.FromText(s);
        }

#endif
        public static void right_turn(ref int up, ref int vp)
        {
            if (up == 0)
            {
                up = vp;
                vp = 0;
            }
            else //vp = 0
            {
                vp = -up;
                up = 0;
            }
        }

        public static void left_turn(ref int up, ref int vp)
        {
            if (up == 0)
            {
                up = -vp;
                vp = 0;
            }
            else //vp = 0
            {
                vp = up;
                up = 0;
            }
        }

        public static coordclass edgepoint(int x, int y, int x0, int y0, int gnid, int ua, int va)
        {
            coordclass cc = new coordclass();

            int uc = 0;
            int vc = 0;

            if (ua == 0)
            {
                if (va == 1)
                {
                    uc = 1;
                    vc = -1;
                }
                else
                {
                    uc = 0;
                    vc = 0;
                }
            }
            else if (ua == 1)
            {
                uc = 0;
                vc = -1;
            }
            else
            {
                uc = 1;
                vc = 0;
            }



            double one1200 = 1.0 / 1200.0;
            double dlon = (x - x0 + uc) * one1200;
            double dlat = -(y - y0 + vc) * one1200; //reverse sign because higher pixel number is lower latitude
            cc.lat = gndict[gnid].latitude + dlat;
            cc.lon = gndict[gnid].longitude + dlon;

            return cc;
        }

        public static List<coordclass> make_kmllist(ref byte[,] fillmap, int edgevalue, int fillvalue, int x0, int y0, int gnid)
        {
            int xmax3 = -9999;
            int xmin3 = 9999;
            int ymax3 = -9999;
            int ymin3 = 9999;
            int n3 = 0;

            int mapsize = fillmap.GetLength(0);

            for (int x = 1; x < mapsize - 1; x++)
                for (int y = 1; y < mapsize - 1; y++)
                    if (fillmap[x, y] == edgevalue)
                    {
                        n3++;

                        if (x > xmax3)
                            xmax3 = x;
                        if (x < xmin3)
                            xmin3 = x;
                        if (y > ymax3)
                            ymax3 = y;
                        if (y < ymin3)
                            ymin3 = y;
                    }

            int x0kml = -1;
            int y0kml = -1;
            Console.WriteLine("n3 = " + n3);
            Console.WriteLine("xmin3 = " + xmin3);
            Console.WriteLine("xmax3 = " + xmax3);
            Console.WriteLine("ymin3 = " + ymin3);
            Console.WriteLine("ymax3 = " + ymax3);
            byte donevalue = 60;
            List<coordclass> kmllist1 = new List<coordclass>();
            List<coordclass> kmllist2 = new List<coordclass>();
            //double one1200 = 1.0 / 1200.0;
            //double dlon;
            //double dlat;
            bool foundedge = false;

            foundedge = false;
            for (int y = ymin3; y <= ymax3; y++) //find a starting point for kml
                for (int x = xmin3; x <= xmax3; x++) //find a starting point for kml
                {
                    if (fillmap[x, y] == edgevalue)
                    {
                        x0kml = x;
                        y0kml = y;
                        foundedge = true;
                        break;
                    }
                }
            if (!foundedge)
                return kmllist2;

            coordclass ccend = new coordclass();
            int xkml = x0kml;
            int ykml = y0kml;

            int up = -999; //perpendicular to shore
            int vp = -999;
            int ua = -999; //along shore
            int va = -999;

            int nround = 0;
            do
            {
                nround++;
                if (nround % 1000 == 0)
                    Console.WriteLine("nround,x,y = " + nround + " " + xkml + " " + ykml);

                if (up < -1) //first round, get starting direction
                {
                    bool founduv = false;
                    for (int uu = -1; uu <= 1; uu++)
                        for (int vv = -1; vv <= 1; vv++)
                            if (uu * vv == 0)
                                if (fillmap[xkml + uu, ykml + vv] == fillvalue)
                                {
                                    up = uu;
                                    vp = vv;
                                    founduv = true;
                                }
                    if (!founduv) //move to another edge square and try again
                    {
                        int newx = -999;
                        int newy = -999;
                        bool foundnewxy = false;
                        for (int uu = -1; uu <= 1; uu++)
                            for (int vv = -1; vv <= 1; vv++)
                                if (fillmap[xkml + uu, ykml + vv] == edgevalue)
                                {
                                    newx = xkml + uu;
                                    newy = ykml + vv;
                                    foundnewxy = true;
                                }
                        if (!foundnewxy)
                            break;
                        else
                        {
                            xkml = newx;
                            ykml = newy;
                            continue;
                        }
                    }
                    else //normal case, lake square next to edge square in up-vp-direction
                    {
                        //get direction along shore
                        if (vp == 0)
                        {
                            va = up;
                            ua = 0;
                        }
                        else //up == 0
                        {
                            ua = -vp;
                            va = 0;
                        }

                        x0kml = xkml;
                        y0kml = ykml;

                        kmllist1.Add(edgepoint(xkml, ykml, x0, y0, gnid, ua, va));
                        ccend = edgepoint(xkml, ykml, x0, y0, gnid, ua, va); //save copy for ending point
                    }
                }

                //normal iteration now:

                if (fillmap[xkml + up + ua, ykml + vp + va] == edgevalue) //right turn
                {
                    xkml = xkml + up + ua;
                    ykml = ykml + vp + va;

                    right_turn(ref up, ref vp);
                    right_turn(ref ua, ref va);

                    kmllist1.Add(edgepoint(xkml, ykml, x0, y0, gnid, ua, va));
                }
                else if ((fillmap[xkml + ua, ykml + va] == edgevalue) && (fillmap[xkml + up + ua, ykml + vp + va] == fillvalue)) //straight ahead
                {
                    xkml = xkml + ua;
                    ykml = ykml + va;
                }
                else if (fillmap[xkml + ua, ykml + va] == fillvalue) //left turn
                {

                    left_turn(ref up, ref vp);
                    left_turn(ref ua, ref va);

                    kmllist1.Add(edgepoint(xkml, ykml, x0, y0, gnid, ua, va));
                }
                else
                {
                    Console.WriteLine("Something wrong");
                    Console.ReadLine();
                }

            }
            while (!((xkml == x0kml) && (ykml == y0kml)));

            //Add final point equal to first point, in order to get closed loop:
            kmllist1.Add(ccend);


            coordclass ccfirst = new coordclass();
            coordclass ccprev = new coordclass();
            coordclass ccfirstmid = new coordclass();

            bool first = true;
            bool firstmid = true;

            foreach (coordclass cc3 in kmllist1)
            {
                if (first)
                {
                    ccfirst.lat = cc3.lat;
                    ccfirst.lon = cc3.lon;
                    first = false;
                }
                else
                {
                    coordclass ccmid = new coordclass();
                    ccmid.lat = 0.5 * (ccprev.lat + cc3.lat);
                    ccmid.lon = 0.5 * (ccprev.lon + cc3.lon);
                    kmllist2.Add(ccmid);
                    if (firstmid)
                    {
                        ccfirstmid.lat = ccmid.lat;
                        ccfirstmid.lon = ccmid.lon;
                        firstmid = false;
                    }
                }
                ccprev.lat = cc3.lat;
                ccprev.lon = cc3.lon;
            }

            kmllist2.Add(ccfirstmid); //add first point again, to close loop

            return kmllist2;
        }

        public static List<coordclass> make_kmllist2(ref byte[,] fillmap, int edgevalue, int x0, int y0, int gnid) //try to follow middle of edge-pixels
        {
            int xmax3 = -9999;
            int xmin3 = 9999;
            int ymax3 = -9999;
            int ymin3 = 9999;
            int n3 = 0;

            int mapsize = fillmap.GetLength(0);

            for (int x = 1; x < mapsize - 1; x++)
                for (int y = 1; y < mapsize - 1; y++)
                    if (fillmap[x, y] == edgevalue)
                    {
                        n3++;

                        if (x > xmax3)
                            xmax3 = x;
                        if (x < xmin3)
                            xmin3 = x;
                        if (y > ymax3)
                            ymax3 = y;
                        if (y < ymin3)
                            ymin3 = y;
                    }

            int x0kml = -1;
            int y0kml = -1;
            Console.WriteLine("n3 = " + n3);
            Console.WriteLine("xmin3 = " + xmin3);
            Console.WriteLine("xmax3 = " + xmax3);
            Console.WriteLine("ymin3 = " + ymin3);
            Console.WriteLine("ymax3 = " + ymax3);
            byte donevalue = 60;
            List<coordclass> kmllist1 = new List<coordclass>();
            List<coordclass> kmllist2 = new List<coordclass>();
            double one1200 = 1.0 / 1200.0;
            double dlon;
            double dlat;
            bool foundedge = false;

            do
            {
                foundedge = false;
                for (int y = ymin3; y <= ymax3; y++) //find a starting point for kml
                    for (int x = xmin3; x <= xmax3; x++) //find a starting point for kml
                    {
                        if (fillmap[x, y] == edgevalue)
                        {
                            x0kml = x;
                            y0kml = y;
                            foundedge = true;
                            break;
                        }
                    }
                if (!foundedge)
                    break;

                int xkml = x0kml;
                int ykml = y0kml;

                bool found = false;
                bool backtrack = false;

                do
                {
                    found = false;
                    //Console.WriteLine("xkml = " + xkml);
                    //Console.WriteLine("ykml = " + ykml);

                    fillmap[xkml, ykml] = donevalue;
                    dlon = (xkml - x0 + 0.5) * one1200;
                    dlat = -(ykml - y0 + 0.5) * one1200; //reverse sign because higher pixel number is lower latitude
                    coordclass cc = new coordclass();
                    cc.lat = gndict[gnid].latitude + dlat;
                    cc.lon = gndict[gnid].longitude + dlon;
                    kmllist1.Add(cc);
                    int newx = -1;
                    int newy = -1;
                    for (int uu = -1; uu <= 1; uu++)
                        for (int vv = -1; vv <= 1; vv++)
                        {
                            if ((uu * vv == 0) || backtrack)
                            {
                                if (fillmap[xkml + uu, ykml + vv] == edgevalue)
                                {
                                    found = true;
                                    backtrack = false;
                                    newx = xkml + uu;
                                    newy = ykml + vv;
                                }
                            }
                        }
                    if (found)
                    {
                        xkml = newx;
                        ykml = newy;
                    }
                    else
                    {
                        bool foundorigin = false;
                        for (int uu = -1; uu <= 1; uu++)
                            for (int vv = -1; vv <= 1; vv++)
                            {
                                if ((xkml + uu == x0kml) && (ykml + vv == y0kml))
                                    foundorigin = true;
                            }
                        if (foundorigin)
                            break;

                        bool found3 = false;
                        for (int x = xmin3; x <= xmax3; x++) //find a starting point for kml
                            for (int y = ymin3; y <= ymax3; y++) //find a starting point for kml
                            {
                                if (fillmap[x, y] == edgevalue)
                                {
                                    found3 = true;
                                }
                            }

                        if (found3)
                        {
                            fillmap[xkml, ykml]++;
                            for (int uu = -1; uu <= 1; uu++)
                                for (int vv = -1; vv <= 1; vv++)
                                {
                                    if (uu * vv == 0)
                                    {
                                        if (fillmap[xkml + uu, ykml + vv] == donevalue)
                                        {
                                            found = true;
                                            backtrack = true;
                                            newx = xkml + uu;
                                            newy = ykml + vv;
                                        }
                                    }
                                }
                            xkml = newx;
                            ykml = newy;
                        }
                    }
                }
                while (found);
            }
            while (foundedge);

            //Add final point equal to first point, in order to get closed loop:
            dlon = (x0kml - x0 + 0.5) * one1200;
            dlat = -(y0kml - y0 + 0.5) * one1200; //reverse sign because higher pixel number is lower latitude
            coordclass cc2 = new coordclass();
            cc2.lat = gndict[gnid].latitude + dlat;
            cc2.lon = gndict[gnid].longitude + dlon;
            kmllist1.Add(cc2);

            coordclass ccfirst = new coordclass();
            coordclass ccprev = new coordclass();
            coordclass ccfirstmid = new coordclass();

            bool first = true;
            bool firstmid = true;

            foreach (coordclass cc3 in kmllist1)
            {
                if (first)
                {
                    ccfirst.lat = cc3.lat;
                    ccfirst.lon = cc3.lon;
                    first = false;
                }
                else
                {
                    coordclass ccmid = new coordclass();
                    ccmid.lat = 0.5 * (ccprev.lat + cc3.lat);
                    ccmid.lon = 0.5 * (ccprev.lon + cc3.lon);
                    kmllist2.Add(ccmid);
                    if (firstmid)
                    {
                        ccfirstmid.lat = ccmid.lat;
                        ccfirstmid.lon = ccmid.lon;
                        firstmid = false;
                    }
                }
                ccprev.lat = cc3.lat;
                ccprev.lon = cc3.lon;
            }

            kmllist2.Add(ccfirstmid); //add first point again, to close loop

            return kmllist2;
        }

        public static void check_islands() //create islands-XX file for a country
        {
            int nisl = 0;
            //int npop = 0;
            //int narea = 0;


            using (StreamWriter sw = new StreamWriter("islands-" + makecountry + ".txt"))
            {

                int ngnid = gndict.Count;

                foreach (int gnid in gndict.Keys)
                {
                    Console.WriteLine("=====" + makecountry + "======== " + ngnid.ToString() + " remaining. ===========");
                    ngnid--;
                    if ((ngnid % 1000) == 0)
                    {
                        Console.WriteLine("Garbage collection:");
                        GC.Collect();
                    }

                    if ((resume_at > 0) && (resume_at != gnid))
                        continue;
                    else
                        resume_at = -1;


                    if (categorydict[gndict[gnid].featurecode] == "islands")
                    {
                        nisl++;

                        double area = -1.0;
                        double lat = gndict[gnid].latitude;
                        double lon = gndict[gnid].longitude;
                        double scale = Math.Cos(lat * 3.1416 / 180);
                        double pixkmx = scale * 40000 / (360 * 1200);
                        double pixkmy = 40000.0 / (360.0 * 1200.0);

                        Console.WriteLine("scale,pixkmx,pixkmy = " + scale.ToString() + "; " + pixkmx.ToString() + "; " + pixkmy.ToString());
                        int[,] mainmap = get_3x3map(lat, lon);

                        int mapsize = mainmap.GetLength(0);

                        byte[,] fillmap = new byte[mapsize, mapsize];

                        for (int x = 0; x < mapsize; x++)
                            for (int y = 0; y < mapsize; y++)
                                fillmap[x, y] = 1;

                        int x0 = get_x_pixel(lon, lon);
                        int y0 = get_y_pixel(lat, lat);
                        floodfill(ref fillmap, ref mainmap, x0, y0, 0, 0, false);

                        if (fillmap[0, 0] == 3) //fill failure
                            continue;

                        int xmax = -1;
                        int ymax = -1;
                        int xmin = 99999;
                        int ymin = 99999;
                        double r2max = -1;

                        int nfill = 0;
                        for (int x = 0; x < mapsize; x++)
                            for (int y = 0; y < mapsize; y++)
                                if (fillmap[x, y] == 2)
                                {
                                    nfill++;
                                    if (x > xmax)
                                        xmax = x;
                                    if (y > ymax)
                                        ymax = y;
                                    if (x < xmin)
                                        xmin = x;
                                    if (y < ymin)
                                        ymin = y;
                                    double r2 = scale * scale * (x - x0) * (x - x0) + (y - y0) * (y - y0);
                                    if (r2 > r2max)
                                        r2max = r2;
                                }

                        double kmew = (xmax - xmin + 1) * pixkmx;
                        double kmns = (ymax - ymin + 1) * pixkmy;

                        Console.WriteLine("nfill = " + nfill.ToString());

                        if (nfill == 0)
                            continue;

                        double rmax = Math.Sqrt(r2max) * pixkmy; //r2max in pixels; rmax in km
                        Console.WriteLine("r2max, rmax = " + r2max.ToString() + "; " + rmax.ToString());

                        List<int> nblist = getneighbors(gnid, rmax);
                        List<int> onisland = new List<int>();

                        foreach (int nb in nblist)
                        {
                            int xnb = get_x_pixel(gndict[nb].longitude, lon);
                            if ((xnb < 0) || (xnb >= mapsize))
                                continue;
                            int ynb = get_y_pixel(gndict[nb].latitude, lat);
                            if ((ynb < 0) || (ynb >= mapsize))
                                continue;
                            if (fillmap[xnb, ynb] == 2)
                                onisland.Add(nb);
                        }

                        //area per pixel:
                        double km2perpixel = pixkmx * pixkmy;
                        area = nfill * km2perpixel;

                        //fillmap.Dispose();

                        //Console.WriteLine("<ret>");
                        //Console.ReadLine();
                        Console.WriteLine(gndict[gnid].Name + "; " + area.ToString() + "; " + kmew.ToString() + "; " + kmns.ToString() + "; " + onisland.Count.ToString());
                        sw.Write(gnid.ToString() + tabstring + area.ToString() + tabstring + kmew.ToString() + tabstring + kmns.ToString());
                        foreach (int oi in onisland)
                            sw.Write(tabstring + oi.ToString());
                        sw.WriteLine();
                    }
                }

            }


        }



        public static void make_ranges() //create ranges-XX file for a country
        {
            int nrange = 0;
            int nisland = 0;
            //int npop = 0;
            //int narea = 0;


            using (StreamWriter swname = new StreamWriter("rangenames-" + makecountry + ".txt"))
            using (StreamWriter sw = new StreamWriter("ranges-" + makecountry + ".txt"))
            {

                int ngnid = gndict.Count;

                foreach (int gnid in gndict.Keys)
                {
                    if ((gndict[gnid].featurecode == "MTS") || (gndict[gnid].featurecode == "HLLS"))
                    {
                        nrange++;
                    }
                    else if (categorydict[gndict[gnid].featurecode] == "islands")
                    {
                        nisland++;
                    }
                }

                if (nrange == 0)
                    return;


                foreach (int gnid in gndict.Keys)
                {
                    //if (gnid != 2700827)
                    //    continue;

                    Console.WriteLine("=====" + makecountry + "======== " + ngnid.ToString() + " remaining. ===========");
                    ngnid--;
                    if ((ngnid % 1000) == 0)
                    {
                        Console.WriteLine("Garbage collection:");
                        GC.Collect();
                    }

                    if ((resume_at > 0) && (resume_at != gnid))
                        continue;
                    else
                        resume_at = -1;


                    if ((gndict[gnid].featurecode == "MTS") || (gndict[gnid].featurecode == "HLLS"))
                    {
                        //nrange++;

                        //double area = -1.0;
                        double lat = gndict[gnid].latitude;
                        double lon = gndict[gnid].longitude;
                        double scale = Math.Cos(lat * 3.1416 / 180);
                        double pixkmx = scale * 40000 / (360 * 1200);
                        double pixkmy = 40000.0 / (360.0 * 1200.0);

                        //Console.WriteLine("scale,pixkmx,pixkmy = " + scale.ToString() + "; " + pixkmx.ToString() + "; " + pixkmy.ToString());
                        int[,] mainmap = get_3x3map(lat, lon);

                        int mapsize = mainmap.GetLength(0);

                        byte[,] fillmap = new byte[mapsize, mapsize];

                        for (int x = 0; x < mapsize; x++)
                            for (int y = 0; y < mapsize; y++)
                                fillmap[x, y] = 1;

                        int x0 = get_x_pixel(lon, lon);
                        int y0 = get_y_pixel(lat, lat);

                        long hsum = 0;
                        int nh = 0;
                        for (int x = 0; x < mapsize; x++)
                        {
                            for (int y = 0; y < mapsize; y++)
                            {
                                hsum += mainmap[x, y];
                                nh++;
                            }
                        }

                        double kmew = -1;
                        double kmns = -1;
                        double maxlength = -1;
                        List<int> inrange = new List<int>();
                        string rangedir = "....";
                        double angle = 999.9;
                        int hmax = -1;
                        double hlat = 999;
                        double hlon = 999;

                        int sealevel = 0;
                        int h0 = mainmap[x0, y0];
                        if (h0 <= 0) //range below sea level
                            continue;

                        long haverage = hsum / nh;
                        double levelfraction = -0.3; //suitable for mainland ranges
                        if (nrange == 1) //countries with single range; don't count the whole country
                            levelfraction = 0.2;

                        if (haverage < 10) //likely lots of ocean around; start higher
                            levelfraction = 0.2;

                        double levelstep = 0.1;

                        do
                        {
                            do
                            {
                                if (h0 > haverage)
                                {
                                    sealevel = (int)(levelfraction * h0 + (1 - levelfraction) * haverage);
                                }
                                else
                                {
                                    sealevel = Convert.ToInt32(h0 * levelfraction);
                                }
                                if (sealevel < 0)
                                    levelfraction += levelstep;
                            }
                            while (sealevel < 0);

                            Console.WriteLine("Base altitude = " + sealevel.ToString());

                            floodfill(ref fillmap, ref mainmap, x0, y0, sealevel, 0, false);

                            if (fillmap[0, 0] == 3) //fill failure
                            {
                                Console.WriteLine("Fill failure");
                                levelfraction += levelstep;
                                for (int x = 0; x < mapsize; x++)
                                    for (int y = 0; y < mapsize; y++)
                                        fillmap[x, y] = 1;
                                continue;
                            }

                            int xmax = -1;
                            int ymax = -1;
                            int xmin = 99999;
                            int ymin = 99999;
                            int xr2max = -1;
                            int yr2max = -1;
                            double r2max = -1;
                            double l2max = -1;
                            maxlength = -1;

                            int nfill = 0;
                            for (int x = 0; x < mapsize; x++)
                                for (int y = 0; y < mapsize; y++)
                                    if (fillmap[x, y] == 2)
                                    {
                                        nfill++;
                                        if (x > xmax)
                                            xmax = x;
                                        if (y > ymax)
                                            ymax = y;
                                        if (x < xmin)
                                            xmin = x;
                                        if (y < ymin)
                                            ymin = y;
                                        double r2 = scale * scale * (x - x0) * (x - x0) + (y - y0) * (y - y0);
                                        if (r2 > r2max)
                                        {
                                            r2max = r2;
                                            xr2max = x;
                                            yr2max = y;
                                        }
                                    }

                            kmew = (xmax - xmin + 1) * pixkmx;
                            kmns = (ymax - ymin + 1) * pixkmy;

                            int xfar = 0;
                            int yfar = 0;
                            for (int x = 0; x < mapsize; x++)
                                for (int y = 0; y < mapsize; y++)
                                    if (fillmap[x, y] == 2)
                                    {
                                        double r2 = scale * scale * (x - xr2max) * (x - xr2max) + (y - yr2max) * (y - yr2max);
                                        if (r2 > l2max)
                                        {
                                            l2max = r2;
                                            xfar = x;
                                            yfar = y;
                                        }
                                    }

                            if (l2max > 0)
                            {
                                maxlength = (Math.Sqrt(l2max) + 1) * pixkmy;
                                double roundarea = maxlength * maxlength * Math.PI / 4; //area of circle with diameter maxlength
                                double realarea = nfill * pixkmx * pixkmy; //actual area of range
                                double fillfraction = realarea / roundarea; //smaller fillfraction = elongate shape
                                //double one1200 = 1.0 / 1200.0;
                                double dx = (xfar - xr2max) * scale;
                                double dy = -(yfar - yr2max); //reverse sign because higher pixel number is lower latitude

                                angle = Math.Atan2(dy, dx);
                                //if (fillfraction < 0.5)
                                //{
                                //    if (Math.Abs(dx) > 2 * Math.Abs(dy))
                                //        rangedir = "EW..";
                                //    else if (Math.Abs(dy) > 2 * Math.Abs(dx))
                                //        rangedir = "NS..";
                                //    else if (dx * dy > 0)
                                //        rangedir = "SWNE";
                                //    else
                                //        rangedir = "SENW";
                                //}

                            }

                            Console.WriteLine("Maxlength = " + maxlength.ToString());


                            Console.WriteLine("nfill = " + nfill.ToString());

                            if (nfill == 0)
                                continue;

                            double rmax = Math.Sqrt(r2max) * pixkmy; //r2max in pixels; rmax in km
                            Console.WriteLine("r2max, rmax = " + r2max.ToString() + "; " + rmax.ToString());

                            List<int> nblist = getneighbors(gnid, rmax);
                            inrange.Clear();

                            bool badrange = false;
                            foreach (int nb in nblist)
                            {
                                if ((gndict[nb].featurecode == "MTS") || (gndict[nb].featurecode == "HLLS"))
                                {
                                    badrange = true;
                                    Console.WriteLine("Range in range");
                                    break;
                                }
                                if (!is_height(gndict[nb].featurecode))
                                    continue;
                                int xnb = get_x_pixel(gndict[nb].longitude, lon);
                                if ((xnb < 0) || (xnb >= mapsize))
                                    continue;
                                int ynb = get_y_pixel(gndict[nb].latitude, lat);
                                if ((ynb < 0) || (ynb >= mapsize))
                                    continue;
                                if (fillmap[xnb, ynb] == 2)
                                    inrange.Add(nb);
                            }

                            if (badrange)
                            {
                                levelfraction += levelstep;
                                for (int x = 0; x < mapsize; x++)
                                    for (int y = 0; y < mapsize; y++)
                                        fillmap[x, y] = 1;

                                continue;
                            }

                            hmax = 0;
                            int xhmax = 0;
                            int yhmax = 0;
                            for (int x = 0; x < mapsize; x++)
                                for (int y = 0; y < mapsize; y++)
                                {
                                    if (fillmap[x, y] == 2)
                                        if (mainmap[x, y] > hmax)
                                        {
                                            hmax = mainmap[x, y];
                                            xhmax = x;
                                            yhmax = y;
                                        }
                                }

                            int hnbmax = 0;
                            int nbmax = -1;
                            foreach (int nb in inrange)
                            {
                                if (!is_height(gndict[nb].featurecode))
                                    continue;
                                if (gndict[nb].elevation > hnbmax)
                                {
                                    hnbmax = gndict[nb].elevation;
                                    nbmax = nb;
                                }
                                int xnb = get_x_pixel(gndict[nb].longitude, lon);
                                if (xnb == xhmax)
                                {
                                    int ynb = get_y_pixel(gndict[nb].latitude, lat);
                                    if (ynb == yhmax)
                                        hmax = -nb; //negative to distinguish from heights

                                }
                            }

                            if (hnbmax >= 0.9 * hmax)
                                hmax = -nbmax;

                            if (hmax > 0)
                            {
                                double one1200 = 1.0 / 1200.0; //degrees per pixel
                                double dlon = (xhmax - x0) * one1200;
                                double dlat = -(yhmax - y0) * one1200; //reverse sign because higher pixel number is lower latitude
                                hlat = lat + dlat;
                                hlon = lon + dlon;
                            }
                            else if (gndict.ContainsKey(-hmax))
                            {
                                hlat = gndict[-hmax].latitude;
                                hlon = gndict[-hmax].longitude;
                            }

                            break;
                        }
                        while (sealevel < h0);
                        //area per pixel:
                        //double km2perpixel = pixkmx * pixkmy;
                        //area = nfill * km2perpixel;

                        //fillmap.Dispose();

                        //Console.WriteLine("<ret>");
                        //Console.ReadLine();

                        if (sealevel < h0)
                        {

                            Console.WriteLine(gndict[gnid].Name + "; " + maxlength.ToString() + "; " + kmew.ToString() + "; " + kmns.ToString() + "; " + inrange.Count.ToString());
                            if (inrange.Count > 1)
                            {
                                sw.Write(gnid.ToString() + tabstring + maxlength.ToString() + tabstring + kmew.ToString() + tabstring + kmns.ToString() + tabstring + angle.ToString() + tabstring + hmax.ToString() + tabstring + hlat.ToString() + tabstring + hlon.ToString());
                                foreach (int oi in inrange)
                                    sw.Write(tabstring + oi.ToString());
                                sw.WriteLine();
                                swname.Write("* [[" + gndict[gnid].Name_ml + "]]: " + maxlength.ToString("N1") + " km lång. Riktning: " + rangedir + " Berg: ");
                                foreach (int oi in inrange)
                                    swname.Write(", [[" + gndict[oi].Name_ml + "]]");
                                swname.WriteLine();
                            }
                        }
                        //if (gnid == 2700827)
                        //    Console.ReadLine();
                    }
                }

            }


        }

        public static int rdf_getentity(string wordpar, string prefix)
        {
            string word = wordpar.Replace("<http://www.wikidata.org/entity/", "");
            word = word.Replace(prefix, "").Replace("c", "");
            return util.tryconvert(word);

        }

        public static string get_in_quotes(string wordpar)
        {
            int i1 = wordpar.IndexOf('"');
            if ((i1 < 0) || (i1 + 1 >= wordpar.Length))
                return "";

            int i2 = wordpar.IndexOf('"', i1 + 1);
            if (i2 < i1 + 2)
                return "";

            return wordpar.Substring(i1 + 1, i2 - i1 - 1);
        }

        public static rdfclass rdf_parse(string line)
        {
            rdfclass rc = new rdfclass();

            string[] words = line.Split('>');

            if (words.Length < 3) //not a triplet
                return rc;

            int o1 = rdf_getentity(words[0], "Q");
            if (o1 < 0) //triplet doesn't start with object id.
            {
                rc.objstring = words[0].Replace("<http://www.wikidata.org/entity/", "");
                //Console.WriteLine("objstring = " + rc.objstring);
            }


            rc.obj = o1;
            int prop = rdf_getentity(words[1], "P");
            if (prop > 0)
                rc.prop = prop;
            else
            {
                if (words[1].Contains("rdf-schema#"))
                {
                    if (words[1].Contains("subClassOf"))
                        rc.prop = 279;
                    else
                        Console.WriteLine(words[1]);
                }
                else if (words[1].Contains("ontology#"))
                {
                    if (words[1].Contains("latitude"))
                        rc.prop = 6250001;
                    else if (words[1].Contains("longitude"))
                        rc.prop = 6250002;
                }
            }

            //<http://www.wikidata.org/entity/Q7743> 
            //<http://www.w3.org/2000/01/rdf-schema#subClassOf> 
            //<http://www.w3.org/2002/07/owl#Class> .


            int o2 = rdf_getentity(words[2], "Q");
            if (o2 > 0)
                rc.objlink = o2;
            else
            {
                rc.value = get_in_quotes(words[2]);
                if (String.IsNullOrEmpty(rc.value))
                {
                    if (words[2].Contains("wikidata.org"))
                        rc.value = words[2].Replace("<http://www.wikidata.org/entity/", "").Trim();
                }
            }

            return rc;

        }

        public static bool search_rdf_tree(int target, int wdid, int depth)
        {
            int maxdepth = 10;
            //Console.WriteLine("search_rdf_tree " + target + " " + wdid);
            if (wdid == target)
            {
                Console.WriteLine("search_rdf_tree FOUND " + target.ToString() + ", " + wdid.ToString() + ", " + depth.ToString());
                return true;
            }
            if (depth > maxdepth)
                return false;
            if (!wdtree.ContainsKey(wdid))
                return false;
            foreach (int upl in wdtree[wdid].uplinks)
                if (search_rdf_tree(target, upl, depth + 1))
                    return true;
            return false;
        }

        public static void read_rdf_tree()
        {
            Console.WriteLine("read_rdf_tree");
            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "wikidata-taxonomy.nt"))
            {
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    rdfclass rc = rdf_parse(line);
                    if (rc.obj < 0)
                        continue;
                    if (rc.prop != 279)
                        continue;
                    if (rc.objlink < 0)
                        continue;

                    if (!wdtree.ContainsKey(rc.obj))
                    {
                        wdtreeclass wtc = new wdtreeclass();
                        wdtree.Add(rc.obj, wtc);
                    }
                    //Console.WriteLine("Added " + rc.obj.ToString());
                    wdtree[rc.obj].uplinks.Add(rc.objlink);
                }
            }

            List<int> dummy = new List<int>();

            foreach (int wdid in wdtree.Keys)
                dummy.Add(wdid);
            foreach (int wdid in dummy)
            {
                foreach (int uplink in wdtree[wdid].uplinks)
                    if (wdtree.ContainsKey(uplink))
                        wdtree[uplink].downlinks.Add(wdid);
            }

            //using (StreamWriter sw = new StreamWriter("wdtree.txt"))
            //{
            //    foreach (int wdid in wdtree.Keys)
            //    {
            //        sw.WriteLine(wdid.ToString());
            //        sw.Write("up");
            //        foreach (int uplink in wdtree[wdid].uplinks)
            //            sw.Write(tabstring + uplink.ToString());
            //        sw.WriteLine();
            //        sw.Write("down");
            //        foreach (int downlink in wdtree[wdid].downlinks)
            //            sw.Write(tabstring + downlink.ToString());
            //        sw.WriteLine();
            //    }
            //}
            Console.WriteLine("read_rdf_tree done");

        }

        public static void test_article_coord()
        {
            while (true)
            {
                Console.Write("Page: ");
                string title = Console.ReadLine();
                Page oldpage = new Page(makesite, title);
                util.tryload(oldpage, 1);
                if (oldpage.Exists())
                {
                    double[] latlong = util.get_article_coord(oldpage);
                    Console.WriteLine(latlong[0].ToString() + "|" + latlong[1].ToString());
                }
            }
        }

        public static void country_center_map()
        {
            Page forkpage = new Page(makesite, "Användare:Lsjbot/Landcentrum");

            int ncountry = 0;
            foreach (int cgnid in countrydict.Keys)
            {
                geonameclass.read_geonames(countrydict[cgnid].iso,makesite);
                string cs = countrydict[cgnid].Name;
                List<coordclass> centers = new List<coordclass>();

                coordclass cc = new coordclass();
                cc.lat = gndict[cgnid].latitude;
                cc.lon = gndict[cgnid].longitude;
                centers.Add(cc);

                double latsum = 0;
                double lonsum = 0;
                int nsum = 0;
                double latppl = 0;
                double lonppl = 0;
                int nppl = 0;
                double latmax = -999;
                double latmin = 999;
                double lonmax = -999;
                double lonmin = 999;

                foreach (int gnid in gndict.Keys)
                {
                    latsum += gndict[gnid].latitude;
                    lonsum += gndict[gnid].longitude;
                    nsum++;
                    if (gndict[gnid].latitude > latmax)
                        latmax = gndict[gnid].latitude;
                    if (gndict[gnid].latitude < latmin)
                        latmin = gndict[gnid].latitude;
                    if (gndict[gnid].longitude > lonmax)
                        lonmax = gndict[gnid].longitude;
                    if (gndict[gnid].longitude < lonmin)
                        lonmin = gndict[gnid].longitude;

                    if (gndict[gnid].featureclass == 'P')
                    {
                        latppl += gndict[gnid].latitude;
                        lonppl += gndict[gnid].longitude;
                        nppl++;
                    }
                }

                coordclass csum = new coordclass();
                coordclass cppl = new coordclass();
                csum.lat = latsum / nsum;
                csum.lon = lonsum / nsum;
                cppl.lat = latppl / nppl;
                cppl.lon = lonppl / nppl;
                centers.Add(csum);
                centers.Add(cppl);

                coordclass cmid = new coordclass();
                cmid.lat = 0.5 * (latmax + latmin);
                cmid.lon = 0.5 * (lonmin + lonmax);
                centers.Add(cmid);

                if (locatordict.ContainsKey(cs) && !makeworldmaponly)
                {
                    int mapsize = 300;

                    string caption = countrydict[cgnid].Name_ml;
                    if (makelang == "sv")
                    {
                        string ifcollapsed = "";//" mw-collapsed";
                        string collapseintro = "{| class=\"mw-collapsible" + ifcollapsed + "\" data-expandtext=\"Visa karta\" data-collapsetext=\"Dölj karta\" style=\"float:right; clear:right;\"\n|-\n!\n|-\n|\n";
                        forkpage.text += collapseintro;
                    }
                    forkpage.text += util.mp(72) + "+|" + locatordict[cs].locatorname + "\n |caption = " + caption + "\n  |float = right\n  |width=" + mapsize.ToString() + "\n  | places =";
                    int inum = 0;
                    foreach (coordclass ccc in centers)
                    {
                        inum++;
                        forkpage.text += util.mp(72) + "~|" + locatordict[cs].locatorname + "| label = " + inum.ToString() + "| mark =Blue_pog.svg|position=right|background=white|lat=" + ccc.lat.ToString(culture_en) + "|long=" + ccc.lon.ToString(culture_en) + "}}\n";
                    }
                    forkpage.text += "}}\n";
                    if (makelang == "sv")
                        forkpage.text += "|}\n"; //collapse-end
                }

                gndict.Clear();
                ghostdict.Clear();
                ncountry++;
                if (ncountry % 10 == 0)
                    util.trysave(forkpage, 1, "Bot creation of country center map");
                //break;
            }

            util.trysave(forkpage, 2, "Bot creation of country center map");
        }

        public static void fill_catwd()
        {
            //populated places
            //public static Dictionary<string, string> catwdclass = new Dictionary<string, string>(); //from category to appropriate wd top class
            //public static Dictionary<string, List<string>> catwdinstance = new Dictionary<string, List<string>>(); //from category to list of appropriate wd instance_of

            catwdclass.Add("populated places", 486972); //"human settlement"
            catwdclass.Add("subdivision1", 56061);//administrative territorial entity
            catwdclass.Add("subdivision2", 1048835);//political territorial entity
            catwdclass.Add("subdivision3", 15916867);//administrative territorial entity of a single country
            catwdclass.Add("lakes", 23397); //lake
            catwdclass.Add("canals", 355304); //watercourse
            catwdclass.Add("streams", 355304); //watercourse
            catwdclass.Add("bays", 15324); //body of water
            catwdclass.Add("wetlands", 170321); //wetland
            catwdclass.Add("waterfalls", 34038); //waterfall
            catwdclass.Add("ice", 23392); //ice
            catwdclass.Add("default", 618123); //geographical object
            catwdclass.Add("landforms", 271669); //landform
            catwdclass.Add("plains", 160091); //plain
            catwdclass.Add("straits", 37901); //strait
            catwdclass.Add("military", 18691599); //military facility
            catwdclass.Add("coasts", 19817101); //coastal landform
            catwdclass.Add("aviation", 62447); //aerodrome
            catwdclass.Add("constructions", 811430); //construction
            catwdclass.Add("caves", 35509); //cave
            catwdclass.Add("islands", 23442); //island
            catwdclass.Add("mountains1", 8502); //mountains
            catwdclass.Add("mountains2", 1437459); //mountain system
            catwdclass.Add("hills", 8502); //mountains
            catwdclass.Add("volcanoes", 8502); //mountains
            catwdclass.Add("peninsulas", 271669); //landform
            catwdclass.Add("valleys", 271669); //landform
            catwdclass.Add("deserts", 271669); //landform
            catwdclass.Add("forests", 4421); //forest

        }

        public static void list_nativenames()
        {
            List<string> nativename_countries = new List<string>();
            //countries with special iw treatment
            nativename_countries.Add("EE");
            nativename_countries.Add("LT");
            nativename_countries.Add("LV");


            using (StreamWriter sw = new StreamWriter("nativenames-" + util.getdatestring() + ".txt"))
            {
                foreach (string nc in nativename_countries)
                {
                    int icountry = countryid[nc];
                    string nwiki = countrydict[icountry].nativewiki;
                    Console.WriteLine(nc + " " + nwiki);
                    int nnames = 0;

                    Dictionary<int, int> wddict = read_wd_dict(nc);

                    foreach (int gnid in wddict.Keys)
                    {
                        int wdid = wddict[gnid];
                        string artname = "";
                        XmlDocument cx = wdtreeclass.get_wd_xml(wdid);
                        if (cx != null)
                        {
                            Dictionary<string, string> rd = wdtreeclass.get_wd_sitelinks(cx);
                            foreach (string wiki in rd.Keys)
                            {
                                string ssw = wiki.Replace("wiki", "");
                                if (ssw == nwiki)
                                {
                                    artname = util.remove_disambig(rd[wiki]);
                                }
                                else if (ssw == makelang)
                                {
                                    artname = "";
                                    break;
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(artname))
                        {
                            sw.WriteLine(gnid.ToString() + tabstring + artname);
                            Console.WriteLine(gnid.ToString() + ", " + artname);
                            nnames++;
                        }

                    }
                    Console.WriteLine("nnames = " + nnames.ToString());
                }
            }

        }
        public static void list_feature()
        {
            using (StreamWriter sw = new StreamWriter("list-" + createfeature + "-" + makecountry + util.getdatestring() + ".txt"))
            {
                foreach (int gnid in gndict.Keys)
                {
                    if (gndict[gnid].featurecode == createfeature)
                    {
                        string country = "";
                        if (countrydict.ContainsKey(gndict[gnid].adm[0]))
                            country = countrydict[gndict[gnid].adm[0]].iso;
                        sw.WriteLine(gnid.ToString() + tabstring + country + tabstring + gndict[gnid].articlename.Replace("*", ""));
                        Console.WriteLine(gnid.ToString() + tabstring + country + tabstring + gndict[gnid].articlename);
                    }
                }
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }


        public static void list_missing_adm1()
        {
            using (StreamWriter sw = new StreamWriter("missing-adm1-" + makecountry + util.getdatestring() + ".txt"))
            {
                foreach (int gnid in gndict.Keys)
                {
                    if (gndict[gnid].featurecode == "ADM1")
                    {
                        if (!gndict[gnid].articlename.Contains("*"))
                        {
                            string country = "";
                            if (gndict.ContainsKey(gndict[gnid].adm[0]))
                                country = gndict[gndict[gnid].adm[0]].Name_ml;
                            sw.WriteLine(gnid.ToString() + tabstring + country + tabstring + gndict[gnid].Name_ml);
                            Console.WriteLine(gnid.ToString() + tabstring + gndict[gnid].Name_ml);
                        }
                    }
                }
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        public static void fix_ppla()
        {
            using (StreamWriter sw = new StreamWriter("fixppla.out"))
            {

                foreach (int icountry in countrydict.Keys)
                {
                    makecountry = countrydict[icountry].iso;
                    string countryname = countrydict[icountry].Name;
                    Console.WriteLine("=== " + countryname + " ===");
                    string countrynameml = countryname;
                    if (countryml.ContainsKey(countryname))
                        countrynameml = countryml[countryname];
                    string[] p167 = new string[] { countrynameml };
                    string admlink = util.mp(167, p167);

                    PageList pl = new PageList(makesite);
                    pl.FillFromLinksToPage(util.initialcap(admlink));
                    int npl = pl.Count();
                    Console.WriteLine(npl + " pages");
                    foreach (Page p in pl)
                    {
                        util.tryload(p, 1);
                        Console.WriteLine(npl + " remaining");
                        npl--;
                        if (p.Exists())
                        {
                            if (!p.text.Contains("<!--P.PPL"))
                                continue;
                            if (!p.text.Contains("geobox"))
                                continue;
                            List<string> pcc = p.GetTemplateParameter("geobox", "country");
                            string pcountry = "";
                            if (pcc.Count == 0)
                                continue;
                            pcountry = pcc.First();

                            if (!pcountry.Contains(countrynameml))
                            {
                                Console.WriteLine("Bad country " + pcountry + ": " + countrynameml);
                                sw.WriteLine("Bad country\t" + pcountry + "\t" + countrynameml);
                                break;
                            }
                        }
                    }
                    Console.WriteLine(countryname + " is OK.");
                    sw.WriteLine(countryname + " is OK.");
                }
            }
        }


        public static void set_folders()
        {
            Console.WriteLine(Environment.MachineName);
            if (Environment.MachineName == "HP2011")
            {
                geonameclass.geonamesfolder = @"C:\dotnwb3\Geonames\";
                extractdir = @"O:\dotnwb3\extract\";
            }
            else if (Environment.MachineName == "KOMPLETT2015")
            {
                geonameclass.geonamesfolder = @"D:\dotnwb3\Geonames\";
                extractdir = @"D:\dotnwb3\extract\";
            }
            else if (Environment.MachineName == "DESKTOP-JOB29A9")
            {
                geonameclass.geonamesfolder = @"I:\dotnwb3\Geonames\";
                extractdir = @"I:\dotnwb3\extract\";
            }
            else
            {
                geonameclass.geonamesfolder = @"C:\dotnwb3\Geonames\";
                extractdir = @"C:\dotnwb3\extract\";
            }
            Console.WriteLine(geonameclass.geonamesfolder);
            Console.WriteLine(extractdir);

        }

        public static void fix_mapedges()
        {
            string countryname = makecountryname;
            string countrynameml = countryname;
            if (countryml.ContainsKey(countryname))
                countrynameml = countryml[countryname];

            //PageList pl = new PageList(makesite);
            //string catname = util.mp(295,new string[] { countrynameml });
            //Console.WriteLine(catname);
            //pl.FillAllFromCategory(catname);
            string right = "|position=right|";
            string left = "|position=left|";

            int ngnid = gndict.Count;

            foreach (int gnid in gndict.Keys)
            {
                if (!artnamedict.ContainsKey(gnid))
                    continue;
                Console.WriteLine("=====" + makecountry + "======== " + ngnid.ToString() + " remaining. ===========");
                ngnid--;
                if ((ngnid % 1000) == 0)
                {
                    Console.WriteLine("Garbage collection:");
                    GC.Collect();
                }

                if ((resume_at > 0) && (resume_at != gnid))
                    continue;
                else
                    resume_at = -1;


                double lat = gndict[gnid].latitude;
                double lon = gndict[gnid].longitude;

                if (makecountry == "US")
                {
                    if (lon > 0)
                        continue;
                }

                if (locatordict[countryname].near_eastern_edge(lat, lon, gndict[gnid].Name_ml))
                {
                    Page p = new Page(makesite, artnamedict[gnid]);
                    if (util.tryload(p, 2))
                    {
                        if (p.Exists())
                        {
                            if (p.IsRedirect())
                            {
                                Page p2 = new Page(p.RedirectsTo());
                                util.tryload(p2, 2);
                                if (!p2.Exists())
                                    continue;
                                if (p2.text.Contains(right))
                                {
                                    p2.text = p2.text.Replace(right, left);
                                    util.trysave(p2, 3);
                                }

                            }
                            else
                            {
                                if (p.text.Contains(right))
                                {
                                    p.text = p.text.Replace(right, left);
                                    util.trysave(p, 3);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Skipping " + artnamedict[gnid]);
                }
            }
        }

        public static void parse_args(string[] args)
        {
            foreach (string s in args)
            {
                string[] words = s.Split(':');
                if (words.Length >= 2)
                {
                    switch (words[0])
                    {
                        case "makelang":
                            makelang = words[1];
                            break;
                        case "makewiki":
                            makewiki = words[1];
                            break;
                        case "makecountry":
                            makecountry = words[1];
                            break;
                        case "resume_at":
                            resume_at = util.tryconvert(words[1]);
                            break;
                        case "resume_at_fork":
                            resume_at_fork = words[1].Replace('_', ' ');
                            break;
                        case "password":
                            password = words[1];
                            break;

                        case "createclass":
                            createclass = words[1][0];
                            break;
                        case "createexceptclass":
                            createexceptclass = words[1][0];
                            break;
                        case "createfeature":
                            createfeature = words[1];
                            break;
                        case "createexceptfeature":
                            createexceptfeature = words[1];
                            break;
                        case "createexceptcategory":
                            createexceptcategory = words[1];
                            break;
                        case "createcategory":
                            createcategory = words[1];
                            break;
                        case "createunit":
                            createunit = util.tryconvert(words[1]);
                            break;
                        case "createexceptunit":
                            createexceptunit = util.tryconvert(words[1]);
                            break;

                        case "makearticles": makearticles = (words[1].ToUpper() == "T"); break;
                        case "makespecificarticles": makespecificarticles = (words[1].ToUpper() == "T"); break;
                        case "remakearticleset": remakearticleset = (words[1].ToUpper() == "T"); break;
                        case "altnamesonly": altnamesonly = (words[1].ToUpper() == "T"); break;
                        case "makefork": makefork = (words[1].ToUpper() == "T"); break;
                        case "checkdoubles": checkdoubles = (words[1].ToUpper() == "T"); break;
                        case "checkwikidata": checkwikidata = (words[1].ToUpper() == "T"); break;
                        case "makeislands": makeislands = (words[1].ToUpper() == "T"); break;
                        case "makelakes": makelakes = (words[1].ToUpper() == "T"); break;
                        case "makerivers": makerivers = (words[1].ToUpper() == "T"); break;
                        case "makeranges": makeranges = (words[1].ToUpper() == "T"); break;
                        case "verifygeonames": verifygeonames = (words[1].ToUpper() == "T"); break;
                        case "verifywikidata": verifywikidata = (words[1].ToUpper() == "T"); break;
                        case "verifyislands": verifyislands = (words[1].ToUpper() == "T"); break;
                        case "verifylakes": verifylakes = (words[1].ToUpper() == "T"); break;
                        case "makealtitude": makealtitude = (words[1].ToUpper() == "T"); break;
                        case "maketranslit": maketranslit = (words[1].ToUpper() == "T"); break;
                        case "makeworldmaponly": makeworldmaponly = (words[1].ToUpper() == "T"); break;
                        case "statisticsonly": statisticsonly = (words[1].ToUpper() == "T"); break;
                        case "savefeaturelink": savefeaturelink = (words[1].ToUpper() == "T"); break;
                        case "savewikilinks": savewikilinks = (words[1].ToUpper() == "T"); break;
                        case "saveadmlinks": saveadmlinks = (words[1].ToUpper() == "T"); break;
                        case "manualcheck": manualcheck = (words[1].ToUpper() == "T"); break;
                        case "listnative": listnative = (words[1].ToUpper() == "T"); break;
                        case "forkduplicates": forkduplicates = (words[1].ToUpper() == "T"); break;
                        case "fixsizecats": fixsizecats = (words[1].ToUpper() == "T"); break;
                        case "testnasa": testnasa = (words[1].ToUpper() == "T"); break;
                        case "retrofitnasa": retrofitnasa = (words[1].ToUpper() == "T"); break;
                        case "checkminutes": checkminutes = (words[1].ToUpper() == "T"); break;
                        case "countrycenters": countrycenters = (words[1].ToUpper() == "T"); break;
                        case "prefergeonamespop": prefergeonamespop = (words[1].ToUpper() == "T"); break;
                        case "makedoubles": makedoubles = (words[1].ToUpper() == "T"); break;
                        case "overwrite": overwrite = (words[1].ToUpper() == "T"); break;
                        case "reallymake": reallymake = (words[1].ToUpper() == "T"); break;
                        case "pauseaftersave": pauseaftersave = (words[1].ToUpper() == "T"); break;
                        case "makehtml": makehtml = (words[1].ToUpper() == "T"); break;
                        case "fixppla": fixppla = (words[1].ToUpper() == "T"); break;
                        case "fixmapedges": fixmapedges = (words[1].ToUpper() == "T"); break;
                        default:
                            Console.WriteLine("Unknown argument " + s);
                            Console.WriteLine("<cr>");
                            Console.ReadLine();
                            break;

                    }
                }
                else
                {
                    switch (s)
                    {
                        case "makearticles": makearticles = true; break;
                        case "makespecificarticles": makespecificarticles = true; break;
                        case "remakearticleset": remakearticleset = true; break;
                        case "altnamesonly": altnamesonly = true; break;
                        case "makefork": makefork = true; break;
                        case "checkdoubles": checkdoubles = true; break;
                        case "checkwikidata": checkwikidata = true; break;
                        case "makeislands": makeislands = true; break;
                        case "makelakes": makelakes = true; break;
                        case "makerivers": makerivers = true; break;
                        case "makeranges": makeranges = true; break;
                        case "verifygeonames": verifygeonames = true; break;
                        case "verifywikidata": verifywikidata = true; break;
                        case "verifyislands": verifyislands = true; break;
                        case "verifylakes": verifylakes = true; break;
                        case "makealtitude": makealtitude = true; break;
                        case "maketranslit": maketranslit = true; break;
                        case "makeworldmaponly": makeworldmaponly = true; break;
                        case "statisticsonly": statisticsonly = true; break;
                        case "savefeaturelink": savefeaturelink = true; break;
                        case "savewikilinks": savewikilinks = true; break;
                        case "saveadmlinks": saveadmlinks = true; break;
                        case "manualcheck": manualcheck = true; break;
                        case "listnative": listnative = true; break;
                        case "listfeature": listfeature = true; break;
                        case "forkduplicates": forkduplicates = true; break;
                        case "fixsizecats": fixsizecats = true; break;
                        case "testnasa": testnasa = true; break;
                        case "retrofitnasa": retrofitnasa = true; break;
                        case "checkminutes": checkminutes = true; break;
                        case "countrycenters": countrycenters = true; break;
                        case "prefergeonamespop": prefergeonamespop = true; break;
                        case "makedoubles": makedoubles = true; break;
                        case "overwrite": overwrite = true; break;
                        case "reallymake": reallymake = true; break;
                        case "pauseaftersave": pauseaftersave = true; break;
                        case "makehtml": makehtml = true; break;
                        case "fixppla": fixppla = true; break;
                        case "fixmapedges": fixmapedges = true; break;
                        default:
                            Console.WriteLine("Unknown argument " + s);
                            Console.WriteLine("<cr>");
                            Console.ReadLine();
                            break;

                    }

                }
            }
        }

        public static void read_locatorlist()
        {
            int n = 0;


            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "locatorlist.txt"))
            {
                int makelangcol = -1;
                int altcol = -1;
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

                    if (words[0] == "en") //headline
                    {
                        for (int i = 1; i < words.Length; i++)
                        {
                            if (words[i] == makewiki)
                                makelangcol = i;
                            if (words[i] == makewiki + "-alt")
                                altcol = i;
                        }
                        Console.WriteLine("makelangcol = " + makelangcol.ToString());
                        Console.WriteLine("altcol = " + altcol.ToString());
                        continue;
                    }
                    //Console.WriteLine("wl = " + words.Length.ToString());

                    locatorclass lc = new locatorclass();
                    lc.locatorname = words[makelangcol];
                    if ((words.Length > altcol) && (!String.IsNullOrEmpty(words[altcol])))
                        lc.altlocator = words[altcol];

                    if ((words.Length > makelangcol) && (!String.IsNullOrEmpty(words[makelangcol])))
                        locatordict.Add(words[0], lc);

                    n++;
                    if ((n % 10) == 0)
                    {
                        Console.WriteLine("n (locatorlist)   = " + n.ToString());

                    }

                }

                Console.WriteLine("n    (locatorlist)= " + n.ToString());

            }

        }



        public static void make_redirect(string frompage, string topage)
        {
            make_redirect(frompage, topage, "", -1);
        }

        public static void make_redirect(string frompage, string topage, string cat, int ilang)
        {
            if (String.IsNullOrEmpty(frompage))
                return;
            if (String.IsNullOrEmpty(topage))
                return;

            Page pred = new Page(makesite, frompage);
            if (util.tryload(pred, 1))
            {
                if (!pred.Exists())
                {
                    pred.text = "#" + util.mp(2) + " [[" + topage + "]]\n";

                    if (makelang == "sv")
                    {
                        if (langdict.ContainsKey(ilang))
                            pred.text += "{{Omdirigering på annat språk|" + langdict[ilang].iso3 + "}}\n";
                        if (!util.is_latin(util.remove_disambig(pred.title)))
                        {
                            string alph_sv = util.get_alphabet_sv(util.get_alphabet(util.remove_disambig(pred.title)));
                            Console.WriteLine(alph_sv);
                            if (!alph_sv.Contains("okänd"))
                                pred.text += "{{Sidnamn annan skrift|" + alph_sv + "}}\n";
                            else
                            {
                                Console.WriteLine(pred.title);
                                Console.WriteLine(util.remove_disambig(pred.title));
                                Console.WriteLine(alph_sv);
                                Console.ReadLine();
                            }

                        }

                    }
                    if (!String.IsNullOrEmpty(cat))
                        pred.AddToCategory(cat);
                    util.trysave(pred, 2, util.mp(60) + " " + countryml[makecountryname] + " " + util.mp(2).ToLower());
                }

            }

        }

        public static void make_redirect_override(Page pred, string topage, string cat, int ilang)
        {

            pred.text = "#" + util.mp(2) + " [[" + topage + "]]\n";

            if (Form1.makelang == "sv")
            {
                if (langdict.ContainsKey(ilang))
                    pred.text += "{{Omdirigering på annat språk|" + langdict[ilang].iso3 + "}}\n";
                if (!util.is_latin(util.remove_disambig(pred.title)))
                {
                    string alph_sv = util.get_alphabet_sv(util.get_alphabet(util.remove_disambig(pred.title)));
                    if (!alph_sv.Contains("okänd"))
                        pred.text += "{{Sidnamn annan skrift|" + alph_sv + "}}\n";
                    else
                    {
                        Console.WriteLine(pred.title);
                        Console.WriteLine(util.remove_disambig(pred.title));
                        Console.WriteLine(alph_sv);
                        Console.ReadLine();
                    }
                }

            }
            if (!String.IsNullOrEmpty(cat))
                pred.AddToCategory(cat);

            util.trysave(pred, 2, util.mp(303) + " " + countryml[makecountryname] + " " + util.mp(2).ToLower());

        }

        public static void romanian_redirect(string topage)
        {
            string frompage = topage;
            Dictionary<char, char> romchars = new Dictionary<char, char>();
            romchars.Add('ş', 'ș');
            romchars.Add('ţ', 'ț');

            foreach (char c in romchars.Keys)
            {
                frompage = frompage.Replace(c, romchars[c]);
            }

            if (frompage != topage)
                make_redirect(frompage, topage, "", -1);

            frompage = topage;
            foreach (char c in romchars.Keys)
            {
                frompage = frompage.Replace(romchars[c], c);
            }

            if (frompage != topage)
                make_redirect(frompage, topage, "", -1);

        }



        public Form1()
        {
            InitializeComponent();
            GeneralSetup();
        }

        public void GeneralSetup()
        {
            set_folders();
            if (makelang == "sv")
            {
                culture = CultureInfo.CreateSpecificCulture("sv-SE");
                nfi = culture.NumberFormat;
                nfi.NumberGroupSeparator = "&nbsp;";
                //nfi_space = culture.NumberFormat.Copy();
                nfi_space.NumberGroupSeparator = " ";
                locatoringeobox = true;  //only works in Swedish!
            }
            else
            {
                culture = CultureInfo.CreateSpecificCulture("en-US");
                nfi = culture.NumberFormat;
                nfi_space = culture.NumberFormat;
                locatoringeobox = false;  //only works in Swedish!
            }
            nfi_en.NumberGroupSeparator = "";

            //==============================
            // Read country-independent stuff:
            //==============================

            wdtreeclass.fill_propdict();
            fill_catwd();
            cyrillic.fill_cyrillic();
            fill_donecountries();
            read_languageiso();
            read_featurecodes();
            admclass.read_adm1();
            admclass.read_adm2();
            countryclass.read_country_info();
#if (DBGEOFLAG)

            read_rivers();
#endif
            if (saveadmlinks)
                admclass.read_adm(makesite);
            read_locatorlist();

            if (countrycenters)
            {
                country_center_map();
                Console.ReadLine();
            }

            fill_ocean_shapes();
            if (makearticles || testnasa || retrofitnasa)
                read_nasa();

            //fix_positionmaps();
            //get_lang_iw("ceb");
            //get_country_iw("sv");

            read_categories();
            read_catstat();

            if (fixppla)
            {
                fix_ppla();
                return;
            }


        }

        private void runparbutton_Click(object sender, EventArgs e)
        {
            string[] args = TBargs.Text.Split();

            foreach (string arg in args)
                Console.WriteLine(arg);

            parse_args(args);

            DateTime starttime = DateTime.Now;

            if (String.IsNullOrEmpty(password))
            {
                Console.Write("Password: ");
                password = Console.ReadLine();
            }


            //convert_shapelist("ne_10m_admin_0_countries");
            //convert_shapelist("glwd_1");
            //convert_shapelist("glwd_2");
            //convert_shapelist("GRDC_687_rivers");
            //foreach (shapeclass sc in read_shapelist("GRDC_687_rivers.shp.txt"))
            //    Console.WriteLine(sc.metadict["river_lake"]);


            //read_drainage_artname_file("artname-ceb.txt", "drainage_names3.txt");

            //DbGeography line = DbGeography.FromText("LINESTRING (0 0, 1 1,1 2)");
            //DbGeography point1 = DbGeography.FromText("POINT (0 0)");
            //DbGeography point2 = DbGeography.FromText("POINT (0.5 0.5)");
            //DbGeography point3 = DbGeography.FromText("POINT (-1 0)");

            //Console.WriteLine("point1 distance = " + line.Distance(point1));
            //Console.WriteLine("point2 distance = " + line.Distance(point2));
            //Console.WriteLine("point3 distance = " + line.Distance(point3));

            //read_drainagedict("drainage_names3.txt");
            //using (StreamWriter sw = new StreamWriter(geonameclass.geonamesfolder + "river_names-"+util.getdatestring()+".txt"))
            //{
            //    foreach (shapeclass sc in read_shapelist(geonameclass.geonamesfolder + "GRDC_687_rivers.shp.txt"))
            //    {
            //        if (sc.metadict.ContainsKey("river_lake") && sc.metadict.ContainsKey("drainage"))
            //        {
            //            double rl = 0;
            //            foreach (DbGeography ss in sc.shapes)
            //            {
            //                if (ss.Length != null)
            //                    rl += (double) ss.Length;
            //            }
            //            string drain = sc.metadict["drainage"];
            //            sw.Write(sc.metadict["river_lake"] + "\t" + drain + "\t" + sc.shapes[0].StartPoint.ToString() + "\t" + sc.shapes[sc.shapes.Count-1].EndPoint.ToString() + "\t" + rl);
            //            if ( drainagedict.ContainsKey(drain))
            //            {
            //                sw.Write("\t"+drainagedict[drain].main_river_artname + "\t" + drainagedict[drain].main_river);
            //            }
            //            sw.WriteLine();
            //            //sc.metadict.Add("Centroid latitude", clat.ToString());
            //            //sc.metadict.Add("Centroid longitude", clon.ToString());

            //        }
            //    }
            //}
            //Console.WriteLine("Done");
            //Console.ReadLine();

            makesite = new Site("https://" + makewiki + ".wikipedia.org", botname, password);
            //wdsite = new Site("http://wikidata.org", botname, password);
            if (makearticles)
            {
                ensite = new Site("https://en.wikipedia.org", botname, password);
                cmsite = new Site("https://commons.wikimedia.org", botname, password);
            }

            //Wikidata login:

            wdtreeclass.get_webpage("https://www.wikidata.org/w/api.php?action=login&lgname=" + botname + "&lgpassword=" + password);

            makesite.defaultEditComment = util.mp(60);
            makesite.minorEditByDefault = false;



            if (makearticles || makefork)
                pausetime = 5;
            else
                pausetime = 7;

            stats.SetMilestone(10000, makesite);

            if (makedoubles)
            {
                doubleprefix = util.mp(13) + botname;
                if (makelang == "sv")
                    doubleprefix += "/Dubletter/";
                else
                    doubleprefix += "/Duplicates/";
            }

            string[] makecountries = makecountry.Split(',');
            //string[] makecountrynames = makecountryname.Split(',');
            //string[] makecountrywikis = makecountrywiki.Split(',');

            int mclength = makecountries.Length;

            if (makecountry == "")
                mclength = 1;



            GeneralSetup();

            //==============================
            // Loop over countries:
            //==============================

            for (int icountry = 0; icountry < mclength; icountry++)
            {
                if (makecountry != "")
                {
                    makecountry = makecountries[icountry];
                    makecountryname = countrydict[countryid[makecountry]].Name;//makecountrynames[icountry];
                    //makecountrywiki = makecountrywikis[icountry];
                    anomalyheadline = false;
                    conflictheadline = false;
                }

                //==============================
                // Read country-dependent stuff:
                //==============================

                admclass.read_adm(makesite);
                tzclass.read_timezone();

                if (!makefork && !checkdoubles && !forkduplicates)
                    geonameclass.read_geonames(makecountry,makesite);

                //if (makearticles && wdthread & !makespecificarticles && !remakearticleset) //Set off wdid thread
                //{
                //    Console.WriteLine("Thread starting branch");
                //    ThreadStart ts_wdid = new ThreadStart(fill_wdid_buffer);
                //    Thread wdid_thread = new Thread(ts_wdid);
                //    threadstop = false; 
                //    resume_at_wdid = resume_at;
                //    wdid_thread.Start();
                //    threadrunning = true;
                //    Console.WriteLine("After thread start, back in main thread");
                //    //Console.ReadLine();
                //}

                else if (checkdoubles)
                {
                    //countries with special treatment in getadmlabel
                    geonameclass.read_geonames("MY",makesite);
                    geonameclass.read_geonames("GB",makesite);
                    geonameclass.read_geonames("RU",makesite);

                    read_existing_coord();
                    if (makelang == "sv")
                        admclass.read_existing_adm1(makesite);
                }

                if (makearticles || checkwikidata || makeislands || makelakes || makerivers || makeranges || retrofitnasa || listfeature || fixmapedges)
                {
                    if (firstround)
                        read_geoboxes();
                    fill_kids_features();
                    //if (!makeislands && !makelakes && !makeranges)
                    //{
                    read_artname();

                    if (makearticles && wdthread & !makespecificarticles && !remakearticleset) //Set off wdid thread
                    {
                        Console.WriteLine("Thread starting branch");
                        ThreadStart ts_wdid = new ThreadStart(fill_wdid_buffer);
                        Thread wdid_thread = new Thread(ts_wdid);
                        threadstop = false;
                        resume_at_wdid = resume_at;
                        wdid_thread.Start();
                        threadrunning = true;
                        Console.WriteLine("After thread start, back in main thread");
                        //Console.ReadLine();
                    }

                    geonameclass.read_altnames();
                    fix_names();
                    //}

                    if (manualcheck)
                        list_missing_adm1();
                }



                //==============================
                // Do stuff:
                //==============================


                if (makearticles)
                {
                    if (makespecificarticles)
                        make_specific_articles();
                    else if (remakearticleset)
                        remake_article_set();
                    else
                        make_articles();
                }

                if (altnamesonly)
                {
                    geonameclass.read_altnames();
                    read_artname();
                    geonameclass.add_nameforks();
                    list_nameforks();
                }

                if (maketranslit)
                {
                    geonameclass.read_altnames();
                    make_translit();
                }

                if (checkdoubles)
                {
                    geonameclass.read_altnames();
                    read_artname();
                    check_doubles();
                }

                if (checkwikidata) //identify wd links from geonames
                    check_wikidata();

                if (verifywikidata) //doublecheck geonames links in wikidata
                    wdtreeclass.verify_wd();

                if (makeislands)
                    check_islands();

                if (makelakes)
                    make_lakes();

#if (DBGEOFLAG)

                if (makerivers)
                    make_rivers();
#endif
                if (makeranges)
                    make_ranges();

                if (verifygeonames)
                    verify_geonames();

                if (makealtitude)
                    make_altitude_files();

                if (makefork)
                {
                    //read_altnames();
                    read_artname();
                    makeforkpages();
                }

                if (forkduplicates)
                    find_duplicate_forks();

                if (listnative)
                    list_nativenames();

                if (listfeature)
                    list_feature();

                if (fixsizecats)
                    fix_sizecats2();

                if (testnasa)
                    test_nasa();

                if (retrofitnasa)
                    retrofit_nasa();

                if (fixmapedges)
                    fix_mapedges();

                //if ( makeworldmaponly )
                //    makeworldmap();

                if (statisticsonly)
                {
                    fchist.PrintSHist();
                    Console.WriteLine("=================================Print bad");
                    fcbad.PrintSHist();
                    //Console.WriteLine("=================================Print large");
                    //fchist.PrintLarge(1000);

                    evarhist.SetBins(0.0, 500000.0, 10);
                    slope1hist.SetBins(0.0, 30.0, 30);
                    slope5hist.SetBins(0.0, 30.0, 30);
                    slope5hist.SetBins(0.0, 50.0, 50);
                    slopermshist.SetBins(0.0, 50.0, 50);
                    elevdiffhist.SetBins(-500.0, 500.0, 100);
                    foreach (int gnid in gndict.Keys)
                    {
                        string tt = get_terrain_type(gnid, 10);
                        string ttext = terrain_text(tt, gnid);
                        Console.WriteLine(ttext);
                        terraintexthist.Add(ttext.Replace(gndict[gnid].Name_ml, "XXX"));
                    }

                    Console.WriteLine("gndict: " + gndict.Count.ToString());
                    evarhist.PrintDHist();
                    Console.WriteLine("Slope1:");
                    slope1hist.PrintDHist();
                    Console.WriteLine("Slope5:");
                    slope5hist.PrintDHist();
                    Console.WriteLine("Slope/RMS:");
                    slopermshist.PrintDHist();
                    ndirhist.PrintIHist();
                    nsameterrhist.PrintIHist();
                    terrainhist.PrintSHist();
                    terraintexthist.PrintSHist();


                    //fclasshist.PrintSHist();
                    //fcathist.PrintSHist();

                    //elevdiffhist.PrintDHist();
                    //foreach (int gnid in gndict.Keys)
                    //{
                    //    get_overrep(gnid,10.0);
                    //}
                    //foverrephist.PrintSHist();
                }

                //stop wd thread, if running
                if (threadrunning)
                {
                    threadstop = true;  //signal thread to stop
                    while (threadrunning) //wait until thread actually stops
                    { }
                }
                //Clear stuff for next round:
                firstround = false;
                gndict.Clear();
                ghostdict.Clear();
                wdid_buffer.Clear();
            }

            if (resume_at > 0)
                Console.WriteLine("Never reached resume_at");

            DateTime endtime = DateTime.Now;

            Console.WriteLine("starttime = " + starttime.ToString());
            Console.WriteLine("endtime = " + endtime.ToString());


        }
        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }



        private void Quitbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Barangaybutton_Click(object sender, EventArgs e)
        {
            if (bgyclass.bgydict.Count > 0)
                return;
            Dictionary<string, int> namewords = new Dictionary<string, int>();
            Dictionary<string, int> cleannamewords = new Dictionary<string, int>();
            bgyclass.read_bgyfile();

            var phdict = geonameclass.get_gndict("PH", makesite);
            var esdict = geonameclass.get_gndict("ES", makesite);
            var ardict = geonameclass.get_gndict("AR", makesite);
            var gbdict = geonameclass.get_gndict("GB", makesite);

            bgyclass.MatchGeoNames(phdict);

            int nfail = 0;
            foreach (bgyclass bc in bgyclass.bgydict.Values)
            {
                if (bc.intlevel == 2)
                {
                    if (bc.latlong(phdict) == null)
                    {
                        nfail++;
                        memo(bc.name  + ", " + bgyclass.bgydict[bc.parent[1]].name + ", " + bgyclass.bgydict[bc.parent[0]].name);
                    }

                }
            }
            memo("nfail(latlong) = " + nfail);

            var esnames = (from c in esdict.Values select c.asciiname.ToLower()).ToList();
            foreach (geonameclass gc in ardict.Values)
            {
                if (!esnames.Contains(gc.asciiname.ToLower()))
                    esnames.Add(gc.asciiname.ToLower());
            }
            esnames.Remove("luzon");
            esnames.Remove("central");
            esnames.Remove("canal");
            var ennames = (from c in gbdict.Values select c.asciiname.ToLower()).ToList();
            var tlwords = util.read_textfile(@"G:\data\Ortnamn\tagalog-wordlist.txt",true);
            var eswords = util.read_textfile_accentspecial(@"G:\data\Ortnamn\spanish-wordlist.txt", true);
            var cebwords = bgyclass.cleanwolff(util.read_textfile(@"G:\data\Ortnamn\cebwolff-words.txt", true));
            var enwords = util.read_textfile(@"G:\data\Ortnamn\english-wordlist.txt", true);
            Dictionary<string, List<string>> dictdict = new Dictionary<string, List<string>>();
            dictdict.Add("en", enwords);
            dictdict.Add("es", eswords);
            dictdict.Add("tl", tlwords);
            dictdict.Add("ceb", cebwords);


            Dictionary<string, transitionmatrixclass> tmdict = new Dictionary<string, transitionmatrixclass>();
            memo("Filling tmes");
            transitionmatrixclass tmes = new transitionmatrixclass("Spanish");
            tmes.Add(esnames);
            memo("Normalizing tmes");
            tmes.Normalize();
            tmdict.Add("es", tmes);

            memo("Filling tmtl");
            transitionmatrixclass tmtl = new transitionmatrixclass("Tagalog");
            tmtl.Add(tlwords);
            memo("Normalizing tmtl");
            tmtl.Normalize();
            tmdict.Add("tl", tmtl);

            memo("Filling tmen");
            transitionmatrixclass tmen = new transitionmatrixclass("English");
            tmen.Add(enwords);
            memo("Normalizing tmen");
            tmen.Normalize();
            tmdict.Add("en", tmen);

            memo("Filling tmceb");
            transitionmatrixclass tmceb = new transitionmatrixclass("Cebuano");
            tmceb.Add(cebwords);
            memo("Normalizing tmceb");
            tmceb.Normalize();
            tmdict.Add("ceb", tmceb);

            var gnadm = from c in phdict where c.Value.featureclass == 'A' select c;

            Console.WriteLine("gnadm = " + gnadm.Count());

            var q1 = from c in gnadm where c.Value.featurecode == "ADM1" select c;

            int nroman = 0;
            List<string> romanlist = new List<string>();
            //List<string> numberlist = bgyclass.fill_numberlist();// new List<string>();

            bgyclass.fill_lists();
            int nnumber = 0;
            int nspain = 0;
            int nspain2 = 0;
            hbookclass spain2hist = new hbookclass("Förled med spanskt efterled");
            int nlists = 0;
            int n = 0;
            hbookclass ledhist = new hbookclass("Antal namnled");
            hbookclass ldiffspanish = new hbookclass("L(tl)-L(es) spanska namn");
            ldiffspanish.SetBins(-20, 20, 20);
            hbookclass ldiffclean = new hbookclass("L(tl)-L(es) rensade namn");
            ldiffclean.SetBins(-20, 20, 20);
            Dictionary<string, Dictionary<string, hbookclass>> ldifftest = new Dictionary<string, Dictionary<string, hbookclass>>();
            hbookclass sainthist = new hbookclass("Saint names");
            hbookclass personhist = new hbookclass("Names of persons");
            hbookclass spanishhist = new hbookclass("Spanish placenames");
            hbookclass langhist = new hbookclass("Languages");
            hbookclass unclearhist = new hbookclass("Names with unclear language");
            hbookclass cleanunclearhist = new hbookclass("Clean names with unclear language");
            hbookclass catcounthist = new hbookclass("# categories for each name");
            hbookclass cathist = new hbookclass("Categories");
            Dictionary<string, hbookclass> langhistdict= new Dictionary<string, hbookclass>();
            Dictionary<string, hbookclass> regionhist = new Dictionary<string, hbookclass>();
            //Dictionary<string, hbookclass> truehistdict = new Dictionary<string, hbookclass>();
            //Dictionary<string, hbookclass> diffhistdict = new Dictionary<string, hbookclass>();
            List<string> uniqueclean = new List<string>();

            //memo("tESTING LDIFF");
            //List<string> dummy = (dictdict.Keys).ToList();
            //dummy.Add("fil");
            //foreach (string ll in dummy)
            //{
            //    truehistdict.Add(ll, new hbookclass(ll + " true reconstruction vs diff"));
            //    diffhistdict.Add(ll, new hbookclass(ll + " all reconstruction vs diff"));
            //    truehistdict[ll].SetBins(0, 10, 10);
            //    diffhistdict[ll].SetBins(0, 10, 10);
            //}
            //foreach (string ll in dictdict.Keys)
            //{
            //    ldifftest.Add(ll, new Dictionary<string, hbookclass>());
            //    foreach (string ll2 in tmdict.Keys)
            //    {
            //        ldifftest[ll].Add(ll2, new hbookclass("ML(" + ll2 + ") for true " + ll));
            //        ldifftest[ll][ll2].SetBins(-40, 0, 10);
            //    }

            //    foreach (string w in dictdict[ll])
            //    {
            //        double dbest = -999;
            //        double diff = -999;
            //        string lbest = "";
            //        Dictionary<string, double> tmd = new Dictionary<string, double>();
            //        foreach (string ll2 in tmdict.Keys)
            //        {
            //            double tm;
            //            if (ll2 == "ceb")
            //                tm = tmdict[ll2].Evaluate2(w.Replace('e', 'i').Replace('o', 'u'));
            //            else
            //                tm = tmdict[ll2].Evaluate2(w);
            //            ldifftest[ll][ll2].Add(tm);
            //            tmd.Add(ll2, tm);
            //            if (tm > dbest)
            //            {
            //                diff = tm - dbest;
            //                dbest = tm;
            //                lbest = ll2;
            //            }
            //            else if (dbest-tm < diff)
            //            {
            //                diff = dbest - tm;
            //            }
            //        }
            //        diffhistdict[lbest].Add(diff);
            //        if (lbest == ll)
            //            truehistdict[lbest].Add(diff);


            //        double tmfil = Math.Max(tmd["ceb"], tmd["tl"]);
            //        tmd.Remove("ceb");
            //        tmd.Remove("tl");
            //        tmd.Add("fil", tmfil);
            //        if (lbest == "ceb" || lbest == "tl")
            //        {
            //            lbest = "fil";
            //            diff = tmd["fil"] - Math.Max(tmd["en"], tmd["es"]);
            //            diffhistdict[lbest].Add(diff);
            //            if (ll == "ceb" || ll == "tl")
            //                truehistdict[lbest].Add(diff);
            //        }

            //    }
            //    foreach (string ll2 in tmdict.Keys)
            //    {
            //        memo(ldifftest[ll][ll2].GetDHist());
            //    }
            //}
            //foreach (string ll in truehistdict.Keys)
            //{
            //    memo(truehistdict[ll].GetDHist());
            //    memo(diffhistdict[ll].GetDHist());
            //}

            //return;

            foreach (bgyclass bc in bgyclass.bgydict.Values)
            {
                if (bc.intlevel != 3)
                    continue;
                n++;
                if (n % 100 == 0)
                {
                    memo("N=" + n + " " + bc.name+"\t"+tmes.Evaluate(bc.name).ToString("N1") + "\t"+tmtl.Evaluate(bc.name).ToString("N1") + "\t"+tmen.Evaluate(bc.name).ToString("N1"));
                }
                string[] ww = bc.name.Trim().Split();
                ledhist.Add(ww.Length);
                if (ww.Length > 5)
                    memo(bc.name);
                foreach (string w in ww)
                {
                    if (String.IsNullOrEmpty(w))
                        continue;
                    if (!namewords.ContainsKey(w))
                        namewords.Add(w, 1);
                    else
                        namewords[w]++;

                    if (bgyclass.numberlist.Contains(w))
                        nnumber++;
                    //if (w.ToUpper() == w && util.is_roman(w))
                    //{
                    //    nroman++;
                    //    if (!romanlist.Contains(w))
                    //        romanlist.Add(w);
                    //}
                    //if (util.onlydigits(w))
                    //{
                    //    nnumber++;
                    //    if (!numberlist.Contains(w))
                    //        numberlist.Add(w);
                    //}
                    if (bgyclass.checklists(w))
                        nlists++;
                    if (bgyclass.dirlang.ContainsKey(w))
                        bc.dirlanguage.Add(bgyclass.dirlang[w]);
                }
                bool spanish = false;
                bool english = false;
                if (bgyclass.is_saint(bc.name))
                {
                    sainthist.Add(bc.name);
                    bc.category.Add(bgyclass.saintstring);
                    spanish = true;
                }
                if (bgyclass.is_person(bc.name))
                {
                    personhist.Add(bc.name);
                    bc.category.Add(bgyclass.personstring);
                }
                if (esnames.Contains(bc.name))
                {
                    spanish = true;
                    nspain++;
                    bc.category.Add(bgyclass.spainstring);
                }
                if (ennames.Contains(bc.name))
                {
                    english = true;
                    bc.category.Add(bgyclass.englandstring);
                }
                else if (ww.Length > 1)
                {
                    string led2 = bgyclass.remove_dirnumberpob(bc.name);//bc.name.Replace(ww[0], "").Trim();
                    if (esnames.Contains(led2))
                    {
                        spanish = true;
                        nspain2++;
                        spain2hist.Add(ww[0]);
                        bc.category.Add(bgyclass.spainstring);
                    }
                    else if (ennames.Contains(led2))
                    {
                        english = true;
                    }
                }
                if (spanish)
                {
                    double ldiff = tmtl.Evaluate(bc.name) - tmes.Evaluate(bc.name);
                    ldiffspanish.Add(ldiff);
                    spanishhist.Add(bc.name);
                    bc.language = "es";
                }
                else if (english)
                {
                    bc.language = "en";
                }
                else
                {
                    string clean = bgyclass.cleanname(bc.name);
                    if (!string.IsNullOrEmpty(clean))
                    {
                        foreach (string w in clean.Split())
                        {

                            if (!cleannamewords.ContainsKey(w))
                                cleannamewords.Add(w, 1);
                            else
                                cleannamewords[w]++;
                        }

                        if (!uniqueclean.Contains(clean))
                        {
                            bgyclass.fillaffix(clean);
                            uniqueclean.Add(clean);
                        }

                        if (esnames.Contains(clean))
                        {
                            bc.language = "es";
                            bc.category.Add(bgyclass.spainstring);
                        }
                        else if (eswords.Contains(clean))
                        {
                            bc.language = "es";
                            bc.category.Add(bgyclass.eswordstring);
                        }
                        else if (tlwords.Contains(clean))
                        {
                            bc.language = "tl";
                            bc.category.Add(bgyclass.tlwordstring);
                        }
                        else if (bgyclass.wolfftest(clean, cebwords))
                        {
                            bc.language = "ceb";
                            bc.category.Add(bgyclass.cebwordstring);
                        }
                        else if (ennames.Contains(clean))
                        {
                            bc.language = "en";
                            bc.category.Add(bgyclass.englandstring);
                        }
                        else if (enwords.Contains(clean))
                        {
                            bc.language = "en";
                            bc.category.Add(bgyclass.enwordstring);
                        }
                        else
                        {
                            bc.language = util.findlanguage(clean, tmdict);
                            //double tlprob = tmtl.Evaluate(clean);
                            //double esprob = tmes.Evaluate(clean);
                            //double enprob = tmen.Evaluate(clean);

                            //double mindiff = 2;
                            //if ((tlprob > esprob + mindiff) && (tlprob > enprob + mindiff))
                            //    bc.language = "tl";
                            //else if ((esprob > tlprob + mindiff) && (esprob > enprob + mindiff))
                            //    bc.language = "es";
                            //else if ((enprob > esprob + mindiff) && (enprob > tlprob + mindiff))
                            //    bc.language = "en";
                            //else
                            //    cleanunclearhist.Add(clean);
                        }
                    }
                    else if (bc.language == "?")
                    {
                        bc.language = "none";
                    }
                }
                langhist.Add(bc.language);
                if (bc.language == "?")
                    unclearhist.Add(bc.name);
                if (!langhistdict.ContainsKey(bc.language))
                    langhistdict.Add(bc.language, new hbookclass(bc.language + " place names"));
                langhistdict[bc.language].Add(bc.name);

                if (!regionhist.ContainsKey(bc.language))
                {
                    regionhist.Add(bc.language, new hbookclass("Region for " + bc.language));
                }
                regionhist[bc.language].Add(bc.getregion());
                catcounthist.Add(bc.category.Count);
                foreach (string cat in bc.category)
                    cathist.Add(cat);
            }

            var q = (from c in namewords select c).OrderByDescending(c => c.Value);
            int nw = 0;
            foreach (var cq in q)
            {
                memo(cq.Key + "\t" + cq.Value);
                nw++;
                if (nw > 500)
                    break;
            }
            var qc = (from c in cleannamewords select c).OrderByDescending(c => c.Value);
            int nwc = 0;
            foreach (var cq in qc)
            {
                memo(cq.Key + "\t" + cq.Value);
                nwc++;
                if (nwc > 100)
                    break;
            }
            memo("nroman = " + nroman);
            memo("nnumber = " + nnumber);
            memo("nlists = " + nlists);
            memo("nspain = " + nspain);
            memo("nspain2 = " + nspain2);

            memo(ledhist.GetIHist());
            memo(spain2hist.GetSHist());

            memo("\nDirections:");
            foreach (string s in bgyclass.dirlist.Keys)
                memo(s+"\t"+bgyclass.dirlist[s]);
            memo("\nHeroes:");
            foreach (string s in bgyclass.herolist.Keys)
                memo(s + "\t" + bgyclass.herolist[s]);
            memo("\nSaint prefixes");
            foreach (string s in bgyclass.saintlist.Keys)
                memo(s + "\t" + bgyclass.saintlist[s]);
            memo("\nTitle prefixes");
            foreach (string s in bgyclass.titlelist.Keys)
                memo(s + "\t" + bgyclass.titlelist[s]);
            memo("\nAdministrative prefixes");
            foreach (string s in bgyclass.poblist.Keys)
                memo(s + "\t" + bgyclass.poblist[s]);

            for (int i=2;i<bgyclass.maxaffix;i++)
            {
                memo("=== affixes of length " + i);
                foreach (string s in bgyclass.topaffix(i,50))
                    memo("'"+s);
            }
            memo("Uniqueclean: " + uniqueclean.Count);

            memo(ldiffspanish.GetDHist());
            memo(ldiffclean.GetDHist());

            memo(spanishhist.GetSHist());
            memo(sainthist.GetSHist());
            memo(personhist.GetSHist());

            //foreach (string s in bgyclass.dirlist.Keys)
            //    memo(s);
            //foreach (string s in bgyclass.poblist.Keys)
            //    memo(s);

            memo(cleanunclearhist.GetSHist(100));
            memo(langhist.GetSHist());

            foreach (string ll in langhistdict.Keys)
                memo(langhistdict[ll].GetSHist(100));

            foreach (string ll in regionhist.Keys)
                memo(regionhist[ll].GetSHist());
            memo(catcounthist.GetIHist());
            memo(cathist.GetSHist());
        }

        private void clipboardbutton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox1.Text);
        }
    }
}
