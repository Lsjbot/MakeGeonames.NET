using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DotNetWikiBot;
using System.Threading;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Net;
using System.Xml;
using System.Drawing.Imaging;

namespace MakeGeonames
{
    class util
    {
        public static Dictionary<int, string> phrases = new Dictionary<int, string>();
        public static Dictionary<string, string> alphabet_sv = new Dictionary<string, string>();//from English name to Swedish name of alphabets
        public static List<string> coordparams = new List<string>(); //possible template parameters for latitude/longitude

        public static int LevenshteinDistance(string src, string dest)
        {
            //From http://www.codeproject.com/Articles/36869/Fuzzy-Search
            //License CPOL (http://www.codeproject.com/info/cpol10.aspx)

            int[,] d = new int[src.Length + 1, dest.Length + 1];
            int i, j, cost;
            char[] str1 = src.ToCharArray();
            char[] str2 = dest.ToCharArray();

            for (i = 0; i <= str1.Length; i++)
            {
                d[i, 0] = i;
            }
            for (j = 0; j <= str2.Length; j++)
            {
                d[0, j] = j;
            }
            for (i = 1; i <= str1.Length; i++)
            {
                for (j = 1; j <= str2.Length; j++)
                {

                    if (str1[i - 1] == str2[j - 1])
                        cost = 0;
                    else
                        cost = 1;

                    d[i, j] =
                        Math.Min(
                            d[i - 1, j] + 1,              // Deletion
                            Math.Min(
                                d[i, j - 1] + 1,          // Insertion
                                d[i - 1, j - 1] + cost)); // Substitution

                    if ((i > 1) && (j > 1) && (str1[i - 1] ==
                        str2[j - 2]) && (str1[i - 2] == str2[j - 1]))
                    {
                        d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + cost);
                    }
                }
            }

            return d[str1.Length, str2.Length];
        }

        public static string fixcase(string ss)
        {
            string s = String.Copy(ss);
            for (int i = 1; i < s.Length; i++)
            {
                if ((s[i - 1] != ' ') && (s[i - 1] != '.'))
                {
                    s = s.Remove(i, 1).Insert(i, Char.ToLower(s[i]).ToString());
                }
            }
            return s;
        }

        public static string findlanguage(string w, Dictionary<string,transitionmatrixclass> tmdict)
        {
            string lang = "?";
            Dictionary<string, double> diffmin = new Dictionary<string, double>()
            {{ "en",5},{"es",5},{"tl",2},{"ceb",4},{"fil",3} };

            double dbest = -999;
            double diff = -999;
            string lbest = "";
            string l2best = "";
            Dictionary<string, double> tmd = new Dictionary<string, double>();
            foreach (string ll2 in tmdict.Keys)
            {
                double tm;
                if (ll2 == "ceb")
                    tm = tmdict[ll2].Evaluate2(w.Replace('e', 'i').Replace('o', 'u').Replace('c','k'));
                if (ll2 == "tl")
                    tm = tmdict[ll2].Evaluate2(w.Replace('c', 'k'));
                else
                    tm = tmdict[ll2].Evaluate2(w);
                tmd.Add(ll2, tm);
                if (tm > dbest)
                {
                    diff = tm - dbest;
                    dbest = tm;
                    l2best = lbest;
                    lbest = ll2;

                }
                else if (dbest - tm < diff)
                {
                    diff = dbest - tm;
                    l2best = ll2;
                }
            }

            if (diffmin.ContainsKey(lbest) && diff > diffmin[lbest])
                lang = lbest;
            else if (lbest == "ceb" || lbest == "tl")
            {
                double tmfil = Math.Max(tmd["ceb"], tmd["tl"]);
                lbest = "fil";
                diff = tmfil - Math.Max(tmd["en"], tmd["es"]);
                if (diffmin.ContainsKey(lbest) && diff > diffmin[lbest])
                    lang = lbest;
            }

            return lang;
        }

        //public static char '\t' = '\t';

        public static void read_phrases()
        {
            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "phraselist.txt"))
            {

                String headline = "";
                headline = sr.ReadLine();

                int icol = 0;
                string[] langs = headline.Split('\t');
                for (icol = 0; icol < langs.Length; icol++)
                {
                    if (langs[icol] == Form1.makelang)
                    {
                        break;
                    }
                }

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    //Console.WriteLine(line);

                    string[] words = line.Split('\t');
                    if (words.Length < icol + 1)
                        continue;
                    //for (int jj = 1; jj < words.Length; jj++)
                    //{
                    //    words[jj] = words[jj].Trim();
                    //}
                    int ip = util.tryconvert(words[0]);
                    phrases.Add(ip, words[icol]);
                }
            }

        }

        public static string mp(int np)
        {
            return mp(np, null);
        }

        public static string mp(int np, string[] param)
        {
            if (phrases.Count == 0)
                read_phrases();

            int ip = 0;
            string sret = phrases[np];
            if (param != null)
                foreach (string s in param)
                {
                    ip++;
                    sret = sret.Replace("#" + ip.ToString(), s);
                }

            return sret;
        }

        public static string ReplaceOne(string textparam, string oldt, string newt, int position) //Replace ONE occurrence of oldt in textparam, the first one after position
        {
            string text = textparam;
            int oldpos = text.IndexOf(oldt, position);
            if (oldpos < 0)
                return text;
            text = text.Remove(oldpos, oldt.Length);
            text = text.Insert(oldpos, newt);

            return text;
        }

        public static List<int> IndexOfAll(string text, string find) //Return a list with ALL occurrences of find in text
        {
            int start = 0;
            int pos = 0;
            List<int> rl = new List<int>();
            do
            {
                pos = text.IndexOf(find, start);
                if (pos >= 0)
                {
                    start = pos + find.Length;
                    rl.Add(pos);
                }
            }
            while (pos >= 0);

            return rl;
        }

        public static string initialcap(string orig)
        {
            if (String.IsNullOrEmpty(orig))
                return "";

            int initialpos = 0;
            if (orig.IndexOf('|') > 0)
                initialpos = orig.IndexOf('|') + 1;
            else if (orig.IndexOf("[[") >= 0)
                initialpos = orig.IndexOf("[[") + 2;
            string s = orig.Substring(initialpos, 1);
            s = s.ToUpper();
            string final = orig;
            final = final.Remove(initialpos, 1).Insert(initialpos, s);
            //s += orig.Remove(0, 1);
            return final;
        }

        public static bool tryload(Page p, int iattempt)
        {
            int itry = 1;

            if (!Form1.reallymake)
            {
                Console.WriteLine("NOT loading " + p.title);
                return true;
            }

            if (String.IsNullOrEmpty(p.title))
                return false;

            while (true)
            {

                try
                {
                    p.Load();
                    return true;
                }
                catch (WebException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("tl we " + message);
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(600000);//milliseconds
                }
                catch (NullReferenceException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("tl ne " + message);
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(6000);//milliseconds
                }
                catch (XmlException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("tl xe " + message);
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(6000);//milliseconds
                }
                catch (IOException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("tl xe " + message);
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(6000);//milliseconds
                }
            }

        }

        public static bool trysave(Page p, int iattempt)
        {
            return trysave(p, iattempt, Form1.makesite.defaultEditComment);
        }

        public static bool trysave(Page p, int iattempt, string editcomment)
        {
            int itry = 1;

            if (!Form1.reallymake)
                return true;

            if (Form1.makehtml)
            {
                //Console.WriteLine(wikitohtml.convert(p.text, makesite));
                string fn = @"html\" + p.title.Replace("/", @"_").Replace(":", "_") + ".html";
                Console.WriteLine(fn);
                //Console.ReadKey();
                using (StreamWriter sw = new StreamWriter(fn))
                {
                    sw.WriteLine(wikitohtml.convert(p.text, Form1.makesite));
                }
                if (Form1.pauseaftersave)
                {
                    Console.WriteLine("<ret>");
                    Console.ReadKey();
                }
                return true;
            }

            while (true)
            {

                try
                {
                    //Bot.editComment = mp(60);
                    p.Save(editcomment, false);
                    Form1.stats.Add(p.title, p.text, (Form1.makearticles || Form1.makefork));
                    DateTime newtime = DateTime.Now;
                    while (newtime < Form1.oldtime)
                    {
                        Thread.Sleep(1000);
                        newtime = DateTime.Now;
                    }
                    Form1.oldtime = newtime.AddSeconds(Form1.pausetime);

                    if (Form1.pauseaftersave)
                    {
                        Console.WriteLine("<ret>");
                        Console.ReadKey();
                    }
                    return true;
                }
                catch (WebException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("ts we " + message);
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(600000);//milliseconds
                }
                catch (WikiBotException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("ts wbe " + message);
                    if (message.Contains("Bad title"))
                        return false;
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(600000);//milliseconds
                }
                catch (IOException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("ts ioe " + message);
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(600000);//milliseconds
                }
            }

        }

        public static int tryconvert(string word)
        {
            int i = -1;

            if (word.Length == 0)
                return i;

            if (!onlydigits(word))
                return i;

            try
            {
                i = Convert.ToInt32(word);
            }
            catch (OverflowException)
            {
                Console.WriteLine("i Outside the range of the Int32 type: " + word);
            }
            catch (FormatException)
            {
                //if ( !String.IsNullOrEmpty(word))
                //    Console.WriteLine("i Not in a recognizable format: " + word);
            }

            return i;

        }

        public static long tryconvertlong(string word)
        {
            long i = -1;

            if (word.Length == 0)
                return i;

            try
            {
                i = Convert.ToInt64(word);
            }
            catch (OverflowException)
            {
                Console.WriteLine("i Outside the range of the Int64 type: " + word);
            }
            catch (FormatException)
            {
                //Console.WriteLine("i Not in a recognizable long format: " + word);
            }

            return i;

        }

        public static double tryconvertdouble(string word)
        {
            double i = -1;

            if (word.Length == 0)
                return i;

            try
            {
                i = Convert.ToDouble(word.Replace(".", ","));
            }
            catch (OverflowException)
            {
                Console.WriteLine("i Outside the range of the Double type: " + word);
            }
            catch (FormatException)
            {
                try
                {
                    i = Convert.ToDouble(word);
                }
                catch (FormatException)
                {
                    //Console.WriteLine("i Not in a recognizable double format: " + word.Replace(".", ","));
                }
                //Console.WriteLine("i Not in a recognizable double format: " + word);
            }

            return i;

        }

        static Dictionary<char, int> romanNumbersDictionary = new Dictionary<char, int>() {
    {
        'I',
        1
    }, {
        'V',
        5
    }, {
        'X',
        10
    }, {
        'L',
        50
    }, {
        'C',
        100
    }, {
        'D',
        500
    }, {
        'M',
        1000
    }
};

        public static int RomanToInt(string s)
        {
            int sum = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char currentRomanChar = s[i];
                romanNumbersDictionary.TryGetValue(currentRomanChar, out int num);
                if (i + 1 < s.Length && romanNumbersDictionary[s[i + 1]] > romanNumbersDictionary[currentRomanChar])
                {
                    sum -= num;
                }
                else
                {
                    sum += num;
                }
            }
            return sum;
        }
        public static bool is_roman(string name) //check for roman numeral
        {
            if (String.IsNullOrEmpty(name))
                return false;
            for (int i = 0; i < name.Length; i++)
            {
                char currentRomanChar = name.ToUpper()[i];
                if (!romanNumbersDictionary.ContainsKey(currentRomanChar))
                    return false;
            }
            return true;
        }

        public static bool onlydigits(string s)
        {
            string rex = @"\D";
            foreach (Match m in Regex.Matches(s, rex))
                return false;
            return true;
        }

        public static bool is_latin(string name)
        {
            return (get_alphabet(name) == "latin");
        }

        public static string get_alphabet(string name)
        {
            char[] letters = remove_disambig(name).ToCharArray();
            int n = 0;
            int sum = 0;
            //int nlatin = 0;
            Dictionary<string, int> alphdir = new Dictionary<string, int>();
            foreach (char c in letters)
            {
                int uc = Convert.ToInt32(c);
                sum += uc;
                string alphabet = "none";
                if (uc <= 0x0040) alphabet = "none";
                //else if ((uc >= 0x0030) && (uc <= 0x0039)) alphabet = "number";
                //else if ((uc >= 0x0020) && (uc <= 0x0040)) alphabet = "punctuation";
                else if ((uc >= 0x0041) && (uc <= 0x007F)) alphabet = "latin";
                else if ((uc >= 0x00A0) && (uc <= 0x00FF)) alphabet = "latin";
                else if ((uc >= 0x0100) && (uc <= 0x017F)) alphabet = "latin";
                else if ((uc >= 0x0180) && (uc <= 0x024F)) alphabet = "latin";
                else if ((uc >= 0x0250) && (uc <= 0x02AF)) alphabet = "phonetic";
                else if ((uc >= 0x02B0) && (uc <= 0x02FF)) alphabet = "spacing modifier letters";
                else if ((uc >= 0x0300) && (uc <= 0x036F)) alphabet = "combining diacritical marks";
                else if ((uc >= 0x0370) && (uc <= 0x03FF)) alphabet = "greek and coptic";
                else if ((uc >= 0x0400) && (uc <= 0x04FF)) alphabet = "cyrillic";
                else if ((uc >= 0x0500) && (uc <= 0x052F)) alphabet = "cyrillic";
                else if ((uc >= 0x0530) && (uc <= 0x058F)) alphabet = "armenian";
                else if ((uc >= 0x0590) && (uc <= 0x05FF)) alphabet = "hebrew";
                else if ((uc >= 0x0600) && (uc <= 0x06FF)) alphabet = "arabic";
                else if ((uc >= 0x0700) && (uc <= 0x074F)) alphabet = "syriac";
                else if ((uc >= 0x0780) && (uc <= 0x07BF)) alphabet = "thaana";
                else if ((uc >= 0x0900) && (uc <= 0x097F)) alphabet = "devanagari";
                else if ((uc >= 0x0980) && (uc <= 0x09FF)) alphabet = "bengali";
                else if ((uc >= 0x0A00) && (uc <= 0x0A7F)) alphabet = "gurmukhi";
                else if ((uc >= 0x0A80) && (uc <= 0x0AFF)) alphabet = "gujarati";
                else if ((uc >= 0x0B00) && (uc <= 0x0B7F)) alphabet = "oriya";
                else if ((uc >= 0x0B80) && (uc <= 0x0BFF)) alphabet = "tamil";
                else if ((uc >= 0x0C00) && (uc <= 0x0C7F)) alphabet = "telugu";
                else if ((uc >= 0x0C80) && (uc <= 0x0CFF)) alphabet = "kannada";
                else if ((uc >= 0x0D00) && (uc <= 0x0D7F)) alphabet = "malayalam";
                else if ((uc >= 0x0D80) && (uc <= 0x0DFF)) alphabet = "sinhala";
                else if ((uc >= 0x0E00) && (uc <= 0x0E7F)) alphabet = "thai";
                else if ((uc >= 0x0E80) && (uc <= 0x0EFF)) alphabet = "lao";
                else if ((uc >= 0x0F00) && (uc <= 0x0FFF)) alphabet = "tibetan";
                else if ((uc >= 0x1000) && (uc <= 0x109F)) alphabet = "myanmar";
                else if ((uc >= 0x10A0) && (uc <= 0x10FF)) alphabet = "georgian";
                else if ((uc >= 0x1100) && (uc <= 0x11FF)) alphabet = "korean";
                else if ((uc >= 0x1200) && (uc <= 0x137F)) alphabet = "ethiopic";
                else if ((uc >= 0x13A0) && (uc <= 0x13FF)) alphabet = "cherokee";
                else if ((uc >= 0x1400) && (uc <= 0x167F)) alphabet = "unified canadian aboriginal syllabics";
                else if ((uc >= 0x1680) && (uc <= 0x169F)) alphabet = "ogham";
                else if ((uc >= 0x16A0) && (uc <= 0x16FF)) alphabet = "runic";
                else if ((uc >= 0x1700) && (uc <= 0x171F)) alphabet = "tagalog";
                else if ((uc >= 0x1720) && (uc <= 0x173F)) alphabet = "hanunoo";
                else if ((uc >= 0x1740) && (uc <= 0x175F)) alphabet = "buhid";
                else if ((uc >= 0x1760) && (uc <= 0x177F)) alphabet = "tagbanwa";
                else if ((uc >= 0x1780) && (uc <= 0x17FF)) alphabet = "khmer";
                else if ((uc >= 0x1800) && (uc <= 0x18AF)) alphabet = "mongolian";
                else if ((uc >= 0x1900) && (uc <= 0x194F)) alphabet = "limbu";
                else if ((uc >= 0x1950) && (uc <= 0x197F)) alphabet = "tai le";
                else if ((uc >= 0x19E0) && (uc <= 0x19FF)) alphabet = "khmer";
                else if ((uc >= 0x1D00) && (uc <= 0x1D7F)) alphabet = "phonetic";
                else if ((uc >= 0x1E00) && (uc <= 0x1EFF)) alphabet = "latin";
                else if ((uc >= 0x1F00) && (uc <= 0x1FFF)) alphabet = "greek and coptic";
                else if ((uc >= 0x2000) && (uc <= 0x206F)) alphabet = "none";
                else if ((uc >= 0x2070) && (uc <= 0x209F)) alphabet = "none";
                else if ((uc >= 0x20A0) && (uc <= 0x20CF)) alphabet = "none";
                else if ((uc >= 0x20D0) && (uc <= 0x20FF)) alphabet = "combining diacritical marks for symbols";
                else if ((uc >= 0x2100) && (uc <= 0x214F)) alphabet = "letterlike symbols";
                else if ((uc >= 0x2150) && (uc <= 0x218F)) alphabet = "none";
                else if ((uc >= 0x2190) && (uc <= 0x21FF)) alphabet = "none";
                else if ((uc >= 0x2200) && (uc <= 0x22FF)) alphabet = "none";
                else if ((uc >= 0x2300) && (uc <= 0x23FF)) alphabet = "none";
                else if ((uc >= 0x2400) && (uc <= 0x243F)) alphabet = "none";
                else if ((uc >= 0x2440) && (uc <= 0x245F)) alphabet = "optical character recognition";
                else if ((uc >= 0x2460) && (uc <= 0x24FF)) alphabet = "enclosed alphanumerics";
                else if ((uc >= 0x2500) && (uc <= 0x257F)) alphabet = "none";
                else if ((uc >= 0x2580) && (uc <= 0x259F)) alphabet = "none";
                else if ((uc >= 0x25A0) && (uc <= 0x25FF)) alphabet = "none";
                else if ((uc >= 0x2600) && (uc <= 0x26FF)) alphabet = "none";
                else if ((uc >= 0x2700) && (uc <= 0x27BF)) alphabet = "none";
                else if ((uc >= 0x27C0) && (uc <= 0x27EF)) alphabet = "none";
                else if ((uc >= 0x27F0) && (uc <= 0x27FF)) alphabet = "none";
                else if ((uc >= 0x2800) && (uc <= 0x28FF)) alphabet = "braille";
                else if ((uc >= 0x2900) && (uc <= 0x297F)) alphabet = "none";
                else if ((uc >= 0x2980) && (uc <= 0x29FF)) alphabet = "none";
                else if ((uc >= 0x2A00) && (uc <= 0x2AFF)) alphabet = "none";
                else if ((uc >= 0x2B00) && (uc <= 0x2BFF)) alphabet = "none";
                else if ((uc >= 0x2E80) && (uc <= 0x2EFF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x2F00) && (uc <= 0x2FDF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x2FF0) && (uc <= 0x2FFF)) alphabet = "none";
                else if ((uc >= 0x3000) && (uc <= 0x303F)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3040) && (uc <= 0x309F)) alphabet = "chinese/japanese";
                else if ((uc >= 0x30A0) && (uc <= 0x30FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3100) && (uc <= 0x312F)) alphabet = "bopomofo";
                else if ((uc >= 0x3130) && (uc <= 0x318F)) alphabet = "korean";
                else if ((uc >= 0x3190) && (uc <= 0x319F)) alphabet = "chinese/japanese";
                else if ((uc >= 0x31A0) && (uc <= 0x31BF)) alphabet = "bopomofo";
                else if ((uc >= 0x31F0) && (uc <= 0x31FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3200) && (uc <= 0x32FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3300) && (uc <= 0x33FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3400) && (uc <= 0x4DBF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x4DC0) && (uc <= 0x4DFF)) alphabet = "none";
                else if ((uc >= 0x4E00) && (uc <= 0x9FFF)) alphabet = "chinese/japanese";
                else if ((uc >= 0xA000) && (uc <= 0xA48F)) alphabet = "chinese/japanese";
                else if ((uc >= 0xA490) && (uc <= 0xA4CF)) alphabet = "chinese/japanese";
                else if ((uc >= 0xAC00) && (uc <= 0xD7AF)) alphabet = "korean";
                else if ((uc >= 0xD800) && (uc <= 0xDB7F)) alphabet = "high surrogates";
                else if ((uc >= 0xDB80) && (uc <= 0xDBFF)) alphabet = "high private use surrogates";
                else if ((uc >= 0xDC00) && (uc <= 0xDFFF)) alphabet = "low surrogates";
                else if ((uc >= 0xE000) && (uc <= 0xF8FF)) alphabet = "private use area";
                else if ((uc >= 0xF900) && (uc <= 0xFAFF)) alphabet = "chinese/japanese";
                else if ((uc >= 0xFB00) && (uc <= 0xFB4F)) alphabet = "alphabetic presentation forms";
                else if ((uc >= 0xFB50) && (uc <= 0xFDFF)) alphabet = "arabic";
                else if ((uc >= 0xFE00) && (uc <= 0xFE0F)) alphabet = "variation selectors";
                else if ((uc >= 0xFE20) && (uc <= 0xFE2F)) alphabet = "combining half marks";
                else if ((uc >= 0xFE30) && (uc <= 0xFE4F)) alphabet = "chinese/japanese";
                else if ((uc >= 0xFE50) && (uc <= 0xFE6F)) alphabet = "small form variants";
                else if ((uc >= 0xFE70) && (uc <= 0xFEFF)) alphabet = "arabic";
                else if ((uc >= 0xFF00) && (uc <= 0xFFEF)) alphabet = "halfwidth and fullwidth forms";
                else if ((uc >= 0xFFF0) && (uc <= 0xFFFF)) alphabet = "specials";
                else if ((uc >= 0x10000) && (uc <= 0x1007F)) alphabet = "linear b";
                else if ((uc >= 0x10080) && (uc <= 0x100FF)) alphabet = "linear b";
                else if ((uc >= 0x10100) && (uc <= 0x1013F)) alphabet = "aegean numbers";
                else if ((uc >= 0x10300) && (uc <= 0x1032F)) alphabet = "old italic";
                else if ((uc >= 0x10330) && (uc <= 0x1034F)) alphabet = "gothic";
                else if ((uc >= 0x10380) && (uc <= 0x1039F)) alphabet = "ugaritic";
                else if ((uc >= 0x10400) && (uc <= 0x1044F)) alphabet = "deseret";
                else if ((uc >= 0x10450) && (uc <= 0x1047F)) alphabet = "shavian";
                else if ((uc >= 0x10480) && (uc <= 0x104AF)) alphabet = "osmanya";
                else if ((uc >= 0x10800) && (uc <= 0x1083F)) alphabet = "cypriot syllabary";
                else if ((uc >= 0x1D000) && (uc <= 0x1D0FF)) alphabet = "byzantine musical symbols";
                else if ((uc >= 0x1D100) && (uc <= 0x1D1FF)) alphabet = "musical symbols";
                else if ((uc >= 0x1D300) && (uc <= 0x1D35F)) alphabet = "tai xuan jing symbols";
                else if ((uc >= 0x1D400) && (uc <= 0x1D7FF)) alphabet = "none";
                else if ((uc >= 0x20000) && (uc <= 0x2A6DF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x2F800) && (uc <= 0x2FA1F)) alphabet = "chinese/japanese";
                else if ((uc >= 0xE0000) && (uc <= 0xE007F)) alphabet = "none";

                bool ucprint = false;
                if (alphabet != "none")
                {
                    n++;
                    if (!alphdir.ContainsKey(alphabet))
                        alphdir.Add(alphabet, 0);
                    alphdir[alphabet]++;
                }
                else if (uc != 0x0020)
                {
                    //Console.Write("c=" + c.ToString() + ", uc=0x" + uc.ToString("x5") + "|");
                    //ucprint = true;
                }
                if (ucprint)
                    Console.WriteLine();
            }

            int nmax = 0;
            string alphmax = "none";
            foreach (string alph in alphdir.Keys)
            {
                //Console.WriteLine("ga:" + alph + " " + alphdir[alph].ToString());
                if (alphdir[alph] > nmax)
                {
                    nmax = alphdir[alph];
                    alphmax = alph;
                }
            }

            if (letters.Length > 2 * n) //mostly non-alphabetic
                return "none";
            else if (nmax > n / 2) //mostly same alphabet
                return alphmax;
            else
                return "mixed"; //mixed alphabets
        }

        public static string get_alphabet_sv(string alph_en)
        {
            Console.WriteLine("gas:" + alph_en);
            if (alphabet_sv.Count == 0)
            {
                alphabet_sv.Add("bopomofo", "zhuyin");
                alphabet_sv.Add("halfwidth and fullwidth forms", "");
                alphabet_sv.Add("syriac", "syriska alfabetet");
                alphabet_sv.Add("thaana", "tāna");
                alphabet_sv.Add("lao", "laotisk skrift");
                alphabet_sv.Add("khmer", "khmerisk skrift");
                alphabet_sv.Add("gurmukhi", "gurmukhi");
                alphabet_sv.Add("myanmar", "burmesisk skrift");
                alphabet_sv.Add("tibetan", "tibetansk skrift");
                alphabet_sv.Add("sinhala", "singalesisk skrift");
                alphabet_sv.Add("ethiopic", "etiopisk skrift");
                alphabet_sv.Add("oriya", "oriya-skrift");
                alphabet_sv.Add("kannada", "kannada");
                alphabet_sv.Add("malayalam", "malayalam");
                alphabet_sv.Add("telugu", "teluguskrift");
                alphabet_sv.Add("tamil", "tamilska alfabetet");
                alphabet_sv.Add("gujarati", "gujarati");
                alphabet_sv.Add("bengali", "bengalisk skrift");
                alphabet_sv.Add("armenian", "armeniska alfabetet");
                alphabet_sv.Add("georgian", "georgiska alfabetet");
                alphabet_sv.Add("devanagari", "devanāgarī");
                alphabet_sv.Add("korean", "hangul");
                alphabet_sv.Add("hebrew", "hebreiska alfabetet");
                alphabet_sv.Add("greek and coptic", "grekiska alfabetet");
                alphabet_sv.Add("chinese/japanese", "kinesiska tecken");
                alphabet_sv.Add("thai", "thailändska alfabetet");
                alphabet_sv.Add("cyrillic", "kyrilliska alfabetet");
                alphabet_sv.Add("arabic", "arabiska alfabetet");
                alphabet_sv.Add("latin", "latinska alfabetet");
            }

            if (alphabet_sv.ContainsKey(alph_en))
                return alphabet_sv[alph_en];
            else
                return "okänd skrift";
        }

        public static string getdatestring()
        {
            DateTime thismonth = DateTime.Now;
            string monthstring = thismonth.Month.ToString();
            while (monthstring.Length < 2)
                monthstring = "0" + monthstring;
            string daystring = thismonth.Day.ToString();
            while (daystring.Length < 2)
                daystring = "0" + daystring;
            return thismonth.Year.ToString() + monthstring + daystring;
        }

        public static List<string> read_textfile(string fn)
        {
            List<string> ls = new List<string>();
            using (StreamReader sr = new StreamReader(fn))
            {
                while (!sr.EndOfStream)
                {
                    ls.Add(sr.ReadLine());
                }
            }
            return ls;
        }

        public static List<string> read_textfile(string fn,bool loweronly)
        {
            List<string> ls = new List<string>();
            using (StreamReader sr = new StreamReader(fn))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line == line.ToLower() ||!loweronly)
                        ls.Add(line);
                }
            }
            return ls;
        }

        public static List<string> read_textfile_accentspecial(string fn, bool loweronly)
        {
            List<string> ls = new List<string>();
            using (StreamReader sr = new StreamReader(fn))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line == line.ToLower() || !loweronly)
                    {
                        ls.Add(line);
                        if (line != remove_accents(line))
                            ls.Add(remove_accents(line));
                    }
                }
            }
            return ls;
        }

        public static string remove_accents(string txt)
        {
            //https://gist.github.com/bjornsallarp/3304522
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt); //Tailspin uses Cyrillic (ISO-8859-5); others use Hebraw (ISO-8859-8)
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static double coordlat(string coordstring)
        {
            //{{Coord|42|33|18|N|1|31|59|E|region:AD_type:city|display=title,inline}}

            string[] cs = coordstring.Split('|');

            if (cs.Length <= 2)
                return 9999.9;
            else
            {
                int ins = -1;
                int iew = -1;
                int iregion = -1;
                for (int i = 1; i < cs.Length; i++)
                {
                    if ((cs[i].ToUpper() == "N") || (cs[i].ToUpper() == "S"))
                        ins = i;
                    if ((cs[i].ToUpper() == "E") || (cs[i].ToUpper() == "W"))
                        iew = i;
                    if (cs[i].ToLower().Contains("region"))
                        iregion = i;
                }
                if (ins < 0)
                    return util.tryconvertdouble(cs[1]);
                else
                {
                    double lat = 0.0;
                    double scale = 1.0;
                    for (int i = 1; i < ins; i++)
                    {
                        double lx = util.tryconvertdouble(cs[i]);
                        if (lx < 90.0)
                            lat += lx / scale;
                        scale *= 60.0;
                    }
                    if (cs[ins].ToUpper() == "S")
                        lat = -lat;
                    return lat;
                }
            }
            //else if (cs.Length < 9)
            //{
            //    return util.tryconvertdouble(cs[1]);
            //}
            //else
            //{
            //    double lat = util.tryconvertdouble(cs[1]) + util.tryconvertdouble(cs[2]) / 60 + util.tryconvertdouble(cs[3]) / 3600;
            //    if (cs[4].ToUpper() == "S")
            //        lat = -lat;
            //    return lat;
            //}
        }

        public static double coordlong(string coordstring)
        {
            //{{Coord|42|33|18|N|1|31|59|E|region:AD_type:city|display=title,inline}}

            string[] cs = coordstring.Split('|');
            if (cs.Length <= 2)
                return 9999.9;
            else
            {
                int ins = -1;
                int iew = -1;
                int iregion = -1;
                for (int i = 1; i < cs.Length; i++)
                {
                    if ((cs[i].ToUpper() == "N") || (cs[i].ToUpper() == "S"))
                        ins = i;
                    if ((cs[i].ToUpper() == "E") || (cs[i].ToUpper() == "W"))
                        iew = i;
                    if (cs[i].ToLower().Contains("region"))
                        iregion = i;
                }
                if (iew < 0)
                    return util.tryconvertdouble(cs[2]);
                else
                {
                    double lon = 0.0;
                    double scale = 1.0;
                    for (int i = ins + 1; i < iew; i++)
                    {
                        double lx = util.tryconvertdouble(cs[i]);
                        if (lx < 180.0)
                            lon += lx / scale;
                        scale *= 60.0;
                    }
                    if (cs[iew].ToUpper() == "W")
                        lon = -lon;
                    return lon;
                }
            }
            //else
            //{
            //    double lon = util.tryconvertdouble(cs[5]) + util.tryconvertdouble(cs[6]) / 60 + util.tryconvertdouble(cs[7]) / 3600;
            //    if (cs[8].ToUpper() == "W")
            //        lon = -lon;
            //    return lon;
            //}
        }


        public static string removearticle(string s)
        {
            string rs = s;
            if (Form1.makelang == "sv")
            {
                if (s.IndexOf("en ") == 0)
                    rs = s.Remove(0, 3);
                else if (s.IndexOf("ett ") == 0)
                    rs = s.Remove(0, 4);
            }
            else if (Form1.makelang == "no")
            {
                if (s.IndexOf("en ") == 0)
                    rs = s.Remove(0, 3);
                else if (s.IndexOf("et ") == 0)
                    rs = s.Remove(0, 4);
            }
            else if (Form1.makelang == "ceb")
            {
                if (s.IndexOf("ang ") == 0)
                    rs = s.Remove(0, 4);

            }
            return rs;
        }

        public static string getwikilink(string s)
        {
            int i1 = s.IndexOf("[[");
            int i2 = s.IndexOf("]]");
            if (i1 < 0)
                return "";
            if (i2 < i1 + 2)
                return "";

            return s.Substring(i1 + 2, i2 - i1 - 2);

        }

        public static string comment(string incomment)
        {
            return "<!--" + incomment + "-->";
        }

        public static List<string> getcomments(string text)
        {
            List<string> rl = new List<string>();
            Match m;
            Regex HeaderPattern = new Regex("<!--(.+?)-->");

            try
            {
                m = HeaderPattern.Match(text);
                while (m.Success)
                {
                    //Console.WriteLine("Found comment " + m.Groups[1] + " at " + m.Groups[1].Index);
                    rl.Add(m.Groups[1].Value);
                    m = m.NextMatch();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rl;

        }

        public static string get_fcode(string text)
        {
            List<string> comments = getcomments(text);
            foreach (string c in comments)
            {
                string[] c2 = c.Split('.');
                if (c2.Length == 2)
                {
                    if (c2[0].Length == 1)
                        if (c2[1].ToUpper() == c2[1])
                            return c2[1];
                }
            }

            return "";
        }

        public static string make_coord_template(string countrycode, string fcode, double lat, double lon, string artname)
        {
            string rs = "{{Coord|";
            rs += lat.ToString(Form1.culture_en) + "|";
            rs += lon.ToString(Form1.culture_en) + "|";
            //rs += "display=inline|";

            string typecode = "landmark";
            //Console.WriteLine("fcode = " + fcode);
            string cat = "geography";
            if (Form1.categorydict.ContainsKey(fcode))
                cat = Form1.categorydict[fcode];

            if (fcode == "ADM1")
                typecode = "adm1st";
            else if (fcode == "ADM2")
                typecode = "adm2nd";
            else if (fcode.Contains("ADM"))
                typecode = "adm3rd";
            else
            {
                switch (cat)
                {
                    case "populated places":
                        typecode = "city";
                        break;
                    case "areas":
                    case "plains":
                    case "deserts":
                        typecode = "adm1st";
                        break;
                    case "navigation":
                    case "wetlands":
                    case "seabed":
                    case "lakes":
                    case "coasts":
                    case "straits":
                    case "bays":
                        typecode = "waterbody";
                        break;
                    case "mountains":
                    case "hills":
                    case "volcanoes":
                    case "rock formations":
                    case "valleys":
                        typecode = "mountain";
                        break;
                    case "islands":
                    case "peninsulas":
                        typecode = "isle";
                        break;
                    case "forests":
                        typecode = "forest";
                        break;
                    case "streams":
                        typecode = "river";
                        break;
                    case "ice":
                        typecode = "glacier";
                        break;
                    default:
                        typecode = "landmark";
                        break;
                }

            }


            rs += "region:" + countrycode + "_type:" + typecode;
            rs += "|name=" + artname + "}}";
            return rs;
        }

        public static bool is_fork(Page p)
        {
            if (!p.Exists())
                return false;

            if (Form1.makelang == "ceb")
            {
                if (p.text.ToLower().Contains("{{giklaro"))
                    return true;
            }
            else if (Form1.makelang == "war")
            {
                if (p.text.ToLower().Contains("{{pansayod"))
                    return true;
            }
            else if (Form1.makelang == "sv")
            {
                if (Form1.forktemplates.Count == 0)
                {
                    PageList pl = new PageList(Form1.makesite);
                    pl.FillFromCategory("Förgreningsmallar");
                    foreach (Page pp in pl)
                        Form1.forktemplates.Add(pp.title.Replace("Mall:", "").ToLower());
                }
                foreach (string ft in Form1.forktemplates)
                    if (p.text.ToLower().Contains("{{" + ft))
                    {
                        Console.WriteLine("is_fork: ft = |" + ft + "|");
                        return true;
                    }
            }

            return false;
        }


        public static string remove_disambig(string title)
        {
            string tit = title;
            if (tit.IndexOf("(") > 0)
                tit = tit.Remove(tit.IndexOf("(")).Trim();
            else if (tit.IndexOf(",") > 0)
                tit = tit.Remove(tit.IndexOf(",")).Trim();
            //if (tit != title)
            //    Console.WriteLine(title + " |" + tit + "|");
            return tit;
        }

        public static bool is_disambig(string title)
        {
            return (title != remove_disambig(title));
        }

        public static double get_distance_latlong(double fromlat, double fromlong, double tolat, double tolong) //returns distance in km
        {
            double kmdeg = 40000 / 360; //km per degree at equator
            double scale = Math.Cos(fromlat * 3.1416 / 180); //latitude-dependent longitude scale
            double dlat = (tolat - fromlat) * kmdeg;
            double dlong = (tolong - fromlong) * kmdeg * scale;

            double dist = Math.Sqrt(dlat * dlat + dlong * dlong);

            if (dist > 1000.0) //use great circle distance (Haversine formula)
            {
                double f1 = fromlat * Math.PI / 180.0; //convert to radians
                double f2 = tolat * Math.PI / 180.0;
                double l1 = fromlong * Math.PI / 180.0;
                double l2 = tolong * Math.PI / 180.0;
                double r = 6371.0; //Earth radius

                double underroot = Math.Pow(Math.Sin((f2 - f1) / 2), 2) + Math.Cos(f1) * Math.Cos(f2) * Math.Pow(Math.Sin((l2 - l1) / 2), 2);
                double root = Math.Sqrt(underroot);
                if (root > 1)
                    root = 1;
                dist = 2 * r * Math.Asin(root);

            }

            return dist;

        }


        public static double[] get_article_coord(Page p)
        {
            double lat = 9999.9;
            double lon = 9999.9;
            double[] latlong = { lat, lon };
            int ncoord = 0;

            if (coordparams.Count == 0)
            {
                coordparams.Add("Coord");
                coordparams.Add("coord");
                coordparams.Add("lat_d");
                coordparams.Add("lat_g");
                coordparams.Add("latitude");
                coordparams.Add("latitud");
                coordparams.Add("nordliggrad");
                coordparams.Add("sydliggrad");
                coordparams.Add("breddgrad");
            }


            Dictionary<string, int> geotempdict = new Dictionary<string, int>();

            //string template = mp(63);
            foreach (string tt in p.GetTemplates(true, true))
            {
                if (tt.Length < 5)
                    continue;
                string cleantt = tt.Replace("\n", "").Trim().Substring(0, 5).ToLower();
                Console.WriteLine("cleantt = |" + cleantt + "|");
                //if (true)//(geolist.Contains(template + cleantt))
                //{
                //geotemplatefound = true;
                //Console.WriteLine("Possible double");

                if (!geotempdict.ContainsKey(cleantt))
                    geotempdict.Add(cleantt, 1);
                else
                    geotempdict[cleantt]++;
                bool foundwithparams = false;
                //foreach (string ttt in p.GetTemplates(true, true))
                //    if (ttt.IndexOf(tt) == 0)
                //{
                foundwithparams = true;
                //Console.WriteLine("foundwithparams");
                if (cleantt == "coord")
                {
                    Console.WriteLine("found {{coord}}");
                    string coordstring = tt;
                    if (coordstring.Length > 10)
                    {
                        double newlat = coordlat(coordstring);
                        double newlon = coordlong(coordstring);
                        if (newlat + newlon < 720.0)
                        {
                            if (ncoord == 0)
                            {
                                lat = newlat;
                                lon = newlon;
                            }
                            else if ((Math.Abs(newlat - lat) + Math.Abs(newlon - lon) > 0.01)) //two different coordinates in article; skip!
                            {
                                lat = 9999;
                                lon = 9999;
                                ncoord = 9999;
                                break;
                            }
                            else
                            {
                                lat = newlat;
                                lon = newlon;
                            }
                        }
                        if (lat + lon < 720.0)
                            ncoord++;
                        if (ncoord > 3)
                            break;
                    }

                }
                else
                {
                    Dictionary<string, string> pdict = Page.ParseTemplate(tt);
                    foreach (string cp in coordparams)
                    {
                        Console.WriteLine("cp = " + cp);
                        double oldlat = lat;
                        double oldlon = lon;
                        if (pdict.ContainsKey(cp))
                        {
                            //coordfound = true;
                            Console.WriteLine("found coordparams");
                            switch (cp)
                            {
                                case "latitude":
                                case "latitud":
                                    lat = util.tryconvertdouble(pdict[cp]);
                                    if (pdict.ContainsKey("longitude"))
                                        lon = util.tryconvertdouble(pdict["longitude"]);
                                    else if (pdict.ContainsKey("longitud"))
                                        lon = util.tryconvertdouble(pdict["longitud"]);
                                    else
                                        Console.WriteLine("latitude but no longitude");
                                    break;
                                case "nordliggrad":
                                case "sydliggrad":
                                    lat = util.tryconvertdouble(pdict[cp]);
                                    if (pdict.ContainsKey("östliggrad"))
                                        lon = util.tryconvertdouble(pdict["östliggrad"]);
                                    else if (pdict.ContainsKey("västliggrad"))
                                        lon = util.tryconvertdouble(pdict["västliggrad"]);
                                    else
                                        Console.WriteLine("latitude but no longitude");
                                    break;
                                case "breddgrad":
                                    lat = util.tryconvertdouble(pdict[cp]);
                                    if (pdict.ContainsKey("längdgrad"))
                                        lon = util.tryconvertdouble(pdict["längdgrad"]);
                                    else
                                        Console.WriteLine("latitude but no longitude");
                                    break;
                                case "lat_d":
                                case "latd":
                                case "lat_g":
                                    double llat = 0.0;
                                    llat = util.tryconvertdouble(pdict[cp]);
                                    if (llat > 0)
                                    {
                                        lat = llat;
                                        if (pdict.ContainsKey("long_d"))
                                            lon = util.tryconvertdouble(pdict["long_d"]);
                                        else if (pdict.ContainsKey("longd"))
                                            lon = util.tryconvertdouble(pdict["longd"]);
                                        else if (pdict.ContainsKey("long_g"))
                                            lon = util.tryconvertdouble(pdict["long_g"]);
                                        if (pdict.ContainsKey("lat_m"))
                                            lat += util.tryconvertdouble(pdict["lat_m"]) / 60;
                                        if (pdict.ContainsKey("long_m"))
                                            lon += util.tryconvertdouble(pdict["long_m"]) / 60;
                                        if (pdict.ContainsKey("lat_s"))
                                            lat += util.tryconvertdouble(pdict["lat_s"]) / 3600;
                                        if (pdict.ContainsKey("long_s"))
                                            lon += util.tryconvertdouble(pdict["long_s"]) / 3600;
                                        if (pdict.ContainsKey("lat_NS"))
                                        {
                                            if (pdict["lat_NS"].ToUpper() == "S")
                                                lat = -lat;
                                        }
                                        if (pdict.ContainsKey("long_EW"))
                                        {
                                            if (pdict["long_EW"].ToUpper() == "W")
                                                lon = -lon;
                                        }
                                    }
                                    break;
                                case "Coord":
                                case "coord": //{{Coord|42|33|18|N|1|31|59|E|region:AD_type:city|display=title,inline}}
                                    string coordstring = pdict[cp];
                                    if (coordstring.Length > 10)
                                    {
                                        lat = coordlat(coordstring);
                                        lon = coordlong(coordstring);
                                    }
                                    break;
                                default:
                                    Console.WriteLine("coord-default:" + tt);
                                    break;


                            }
                            if (lat + lon < 720.0)
                            {
                                if ((Math.Abs(oldlat - lat) + Math.Abs(oldlon - lon) > 0.01)) //two different coordinates in article; skip!
                                {
                                    lat = 9999;
                                    lon = 9999;
                                    ncoord = 9999;
                                    break;
                                }
                            }
                            else
                            {
                                lat = oldlat;
                                lon = oldlon;
                            }

                            if (lat + lon < 720.0)
                                ncoord++;
                            if (ncoord > 3)
                                break;



                        }
                    }
                }
                //}
                if (!foundwithparams)
                    Console.WriteLine("Params not found");
                Console.WriteLine("lat = " + lat.ToString());
                Console.WriteLine("lon = " + lon.ToString());
                //}
            }

            if (ncoord > 4) //several coordinate sets, probably a list or something; return failure
                return latlong;

            latlong[0] = lat;
            latlong[1] = lon;
            return latlong;
        }

        //public static double[] get_article_coord_old(Page p)
        //{
        //    double lat = 9999.9;
        //    double lon = 9999.9;
        //    double[] latlong = { lat, lon };

        //    if (coordparams.Count == 0)
        //    {
        //        coordparams.Add("coord");
        //        coordparams.Add("lat_d");
        //        coordparams.Add("lat_g");
        //        coordparams.Add("latitude");
        //        coordparams.Add("latitud");
        //    }


        //    Dictionary<string, int> geotempdict = new Dictionary<string, int>();

        //    string template = mp(63);
        //    foreach (string tt in p.GetTemplates(false, true))
        //    {
        //        string cleantt = initialcap(tt.Replace("\n", "").Trim());
        //        Console.WriteLine("tt = |" + cleantt + "|");
        //        if (true)//(geolist.Contains(template + cleantt))
        //        {
        //            //geotemplatefound = true;
        //            Console.WriteLine("Possible double");

        //            if (!geotempdict.ContainsKey(cleantt))
        //                geotempdict.Add(cleantt, 1);
        //            else
        //                geotempdict[cleantt]++;
        //            bool foundwithparams = false;
        //            foreach (string ttt in p.GetTemplates(true, true))
        //                if (ttt.IndexOf(tt) == 0)
        //                {
        //                    foundwithparams = true;
        //                    Console.WriteLine("foundwithparams");
        //                    if (cleantt == "Coord")
        //                    {
        //                        Console.WriteLine("found {{coord}}");
        //                        string coordstring = ttt;
        //                        if (coordstring.Length > 10)
        //                        {
        //                            lat = coordlat(coordstring);
        //                            lon = coordlong(coordstring);
        //                        }

        //                    }
        //                    else
        //                    {
        //                        Dictionary<string, string> pdict = makesite.ParseTemplate(ttt);
        //                        foreach (string cp in coordparams)
        //                        {
        //                            Console.WriteLine("cp = " + cp);
        //                            if (pdict.ContainsKey(cp))
        //                            {
        //                                //coordfound = true;
        //                                Console.WriteLine("found coordparams");
        //                                switch (cp)
        //                                {
        //                                    case "latitude":
        //                                    case "latitud":
        //                                        lat = util.tryconvertdouble(pdict[cp]);
        //                                        if (pdict.ContainsKey("longitude"))
        //                                            lon = util.tryconvertdouble(pdict["longitude"]);
        //                                        else if (pdict.ContainsKey("longitud"))
        //                                            lon = util.tryconvertdouble(pdict["longitud"]);
        //                                        else
        //                                            Console.WriteLine("latitude but no longitude");
        //                                        break;
        //                                    case "lat_d":
        //                                    case "latd":
        //                                    case "lat_g":
        //                                        double llat = 0.0;
        //                                        llat = util.tryconvertdouble(pdict[cp]);
        //                                        if (llat > 0)
        //                                        {
        //                                            lat = llat;
        //                                            if (pdict.ContainsKey("long_d"))
        //                                                lon = util.tryconvertdouble(pdict["long_d"]);
        //                                            else if (pdict.ContainsKey("longd"))
        //                                                lon = util.tryconvertdouble(pdict["longd"]);
        //                                            else if (pdict.ContainsKey("long_g"))
        //                                                lon = util.tryconvertdouble(pdict["long_g"]);
        //                                            if (pdict.ContainsKey("lat_m"))
        //                                                lat += util.tryconvertdouble(pdict["lat_m"]) / 60;
        //                                            if (pdict.ContainsKey("long_m"))
        //                                                lon += util.tryconvertdouble(pdict["long_m"]) / 60;
        //                                            if (pdict.ContainsKey("lat_s"))
        //                                                lat += util.tryconvertdouble(pdict["lat_s"]) / 3600;
        //                                            if (pdict.ContainsKey("long_s"))
        //                                                lon += util.tryconvertdouble(pdict["long_s"]) / 3600;
        //                                            if (pdict.ContainsKey("lat_NS"))
        //                                            {
        //                                                if (pdict["lat_NS"].ToUpper() == "S")
        //                                                    lat = -lat;
        //                                            }
        //                                            if (pdict.ContainsKey("long_EW"))
        //                                            {
        //                                                if (pdict["long_EW"].ToUpper() == "W")
        //                                                    lon = -lon;
        //                                            }
        //                                        }
        //                                        break;
        //                                    case "coord": //{{Coord|42|33|18|N|1|31|59|E|region:AD_type:city|display=title,inline}}
        //                                        string coordstring = pdict["coord"];
        //                                        if (coordstring.Length > 10)
        //                                        {
        //                                            lat = coordlat(coordstring);
        //                                            lon = coordlong(coordstring);
        //                                        }
        //                                        break;
        //                                    default:
        //                                        Console.WriteLine("coord-default:" + ttt);
        //                                        break;


        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            if (!foundwithparams)
        //                Console.WriteLine("Params not found");
        //            Console.WriteLine("lat = " + lat.ToString());
        //            Console.WriteLine("lon = " + lon.ToString());
        //        }
        //    }
        //    latlong[0] = lat;
        //    latlong[1] = lon;
        //    return latlong;
        //}

        public static int get_direction_latlong(double fromlat, double fromlong, double tolat, double tolong)
        {
            double kmdeg = 40000.0 / 360.0; //km per degree at equator
            double scale = Math.Cos(fromlat * 3.1416 / 180); //latitude-dependent longitude scale
            double dlat = (tolat - fromlat) * kmdeg;

            double dlong = (tolong - fromlong) * kmdeg * scale;

            if (Math.Abs(tolong - fromlong) > 180.0)
            {
                if (tolong > fromlong)
                    dlong = (tolong - (fromlong + 360)) * kmdeg * scale;
                else
                    dlong = ((tolong + 360) - fromlong) * kmdeg * scale;
            }

            //Console.WriteLine("dlat,dlong = " + dlat.ToString() + " " + dlong.ToString());
            if (dlat * dlat > 4.0 * dlong * dlong)
            {
                if (dlat > 0) // north
                    return 1;
                else           //south
                    return 2;
            }
            else if (dlong * dlong > 4.0 * dlat * dlat)
            {
                if (dlong > 0) // east
                    return 4;
                else            //west
                    return 3;
            }
            else if (dlong > 0)
            {
                if (dlat > 0) //northeast
                    return 5;
                else           //southeast
                    return 6;
            }
            else
            {
                if (dlat > 0) //northwest
                    return 7;
                else           //southwest
                    return 8;
            }

            //      1
            //   7    5
            //  3      4
            //   8    6
            //      2
        }

        public static int get_pix_direction(int x, int y, int x0, int y0, double scale)
        {
            double dlat = y0 - y; //reverse sign; +y=south!
            double dlong = (x - x0) * scale;

            //Console.WriteLine("dlat,dlong = " + dlat.ToString() + " " + dlong.ToString());
            if (dlat * dlat > 4.0 * dlong * dlong)
            {
                if (dlat > 0) // north
                    return 1;
                else           //south
                    return 2;
            }
            else if (dlong * dlong > 4.0 * dlat * dlat)
            {
                if (dlong > 0) // east
                    return 4;
                else            //west
                    return 3;
            }
            else if (dlong > 0)
            {
                if (dlat > 0) //northeast
                    return 5;
                else           //southeast
                    return 6;
            }
            else
            {
                if (dlat > 0) //northwest
                    return 7;
                else           //southwest
                    return 8;
            }

            //      1
            //   7    5
            //  3      4
            //   8    6
            //      2

        }

        public static int[] getdircoord(int dir) //translate from direction codes of get_pix_direction() into +/-1 x +/-1 y
        {
            int[] rc = new int[2] { 0, 0 };
            switch (dir)
            {
                case 1:
                    rc[1] = 1;
                    break;
                case 2:
                    rc[1] = -1;
                    break;
                case 3:
                    rc[0] = -1;
                    break;
                case 4:
                    rc[0] = 1;
                    break;
                case 5:
                    rc[0] = 1;
                    rc[1] = 1;
                    break;
                case 6:
                    rc[0] = 1;
                    rc[1] = -1;
                    break;
                case 7:
                    rc[0] = -1;
                    rc[1] = 1;
                    break;
                case 8:
                    rc[0] = -1;
                    rc[1] = -1;
                    break;
            }
            return rc;
        }

        public static string fnum(double x)
        {
            string rt = "{{formatnum:";
            if (x < 1.0)
                rt += x.ToString("F2", Form1.culture_en);
            else if (x < 30.0)
                rt += x.ToString("F1", Form1.culture_en);
            else
                rt += x.ToString("F0", Form1.culture_en);
            rt += "}}";
            return rt;
        }

        public static string fnum(long i)
        {
            return "{{formatnum:" + i.ToString() + "}}";
        }

        public static string fnum(int i)
        {
            return "{{formatnum:" + i.ToString() + "}}";
        }

        public static void make_mapimage(int[,] sourcemap, string mapfile)
        {
            int mapsize = sourcemap.GetLength(0);
            Bitmap map = new Bitmap(mapsize, mapsize);

            for (int x = 0; x < mapsize; x++)
                for (int y = 0; y < mapsize; y++)
                {
                    Color col = Color.White;

                    if (sourcemap[x, y] <= 0)
                        col = Color.Blue;
                    else if (sourcemap[x, y] > 10000)
                        col = Color.Red;
                    else
                        col = Color.Green;
                    map.SetPixel(x, y, col);
                }

            map.Save(mapfile, ImageFormat.Jpeg);
            map.Dispose();
        }

        public static void makeworldmap()
        {
            Bitmap map = new Bitmap(3600, 1800);

            int n = 0;

            string filename = geonameclass.geonamesfolder;

            filename += "allCountries.txt";

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
                    double lat = util.tryconvertdouble(words[4]);
                    double lon = util.tryconvertdouble(words[5]);

                    double scale = 0.5 + 0.5 * Math.Cos(lat * 3.1416 / 180); //latitude-dependent longitude scale

                    int x = Convert.ToInt32((lon * scale + 180) * 10);
                    int y = 1800 - Convert.ToInt32((lat + 90) * 10);

                    if ((x >= 0) && (x < 3600) && (y >= 0) && (y < 1800))
                    {

                        Color oldcol = map.GetPixel(x, y);
                        Color newcol = Color.White;
                        if (oldcol.GetBrightness() < 1.0)
                        {
                            int nargb = oldcol.ToArgb() + 0x00030303;
                            newcol = Color.FromArgb(nargb);
                        }
                        map.SetPixel(x, y, newcol);
                    }
                    else
                        Console.WriteLine("lat,lon,x,y = " + lat.ToString() + " " + lon.ToString() + " " + x.ToString() + " " + y.ToString());
                    n++;
                    if ((n % 10000) == 0)
                    {
                        Console.WriteLine("n (world map)   = " + n.ToString());
                        //if (n >= 500000)
                        //    break;

                    }
                }
            }


            map.Save(geonameclass.geonamesfolder + "worldmap.jpg", ImageFormat.Jpeg);
            map.Dispose();
        }

        public static string cleanup_text(string text)
        {
            Dictionary<string, string> cleanstring = new Dictionary<string, string>();
            cleanstring.Add(".  ", ". ");
            cleanstring.Add(",  ", ", ");
            cleanstring.Add("\n ", "\n");
            cleanstring.Add("\n\n\n", "\n\n");

            string rs = text;
            foreach (string cs in cleanstring.Keys)
            {
                while (rs.Contains(cs))
                    rs = rs.Replace(cs, cleanstring[cs]);
            }
            return rs;

        }



    }
}
