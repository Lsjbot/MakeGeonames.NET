using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    public class transliterationclass
    {
        private string defaultlang = "ru";
        private string badreturn = "?";
        private string contextdependent = "*";
        private List<char> vowels = new List<char>();
        public List<char> badlist = new List<char>();

        private Dictionary<string, Dictionary<char, string>> tldict = new Dictionary<string, Dictionary<char, string>>();

        public void Addchar(char fromchar, string tochars)
        {
            Addchar(fromchar, tochars, defaultlang, false);
        }

        public void Addchar(char fromchar, string tochars, bool isvowel)
        {
            Addchar(fromchar, tochars, defaultlang, isvowel);
        }

        public void Addchar(char fromchar, string tochars, string lang)
        {
            Addchar(fromchar, tochars, lang, false);
        }

        public void Addchar(char fromchar, string tochars, string lang, bool isvowel)
        {
            if (!tldict.ContainsKey(lang))
            {
                Dictionary<char, string> csdict = new Dictionary<char, string>();
                tldict.Add(lang, csdict);
            }
            if (!tldict[lang].ContainsKey(fromchar))
            {
                tldict[lang].Add(fromchar, tochars);
                if (isvowel)
                    if (!vowels.Contains(fromchar))
                        vowels.Add(fromchar);
            }
        }

        private string Transchar(char fromchar, char contextbefore, char contextafter, string langparam)
        {
            if (Convert.ToInt32(fromchar) <= 0x0041) //punctuation etc.
                return fromchar.ToString();

            string lang = langparam;
            if (!tldict.ContainsKey(lang)) //russian default language 
                lang = defaultlang;
            if (!tldict[lang].ContainsKey(fromchar))
                lang = defaultlang;
            if (!tldict[lang].ContainsKey(fromchar))
            {
                if (util.is_latin(fromchar.ToString()))
                    return fromchar.ToString();
                if (!badlist.Contains(fromchar))
                    badlist.Add(fromchar);
                return badreturn;
            }

            if (tldict[lang][fromchar] != contextdependent)
                return tldict[lang][fromchar];
            else //context-dependent:
            {
                List<char> tszlist = new List<char> { 'С', 'с', 'Т', 'т', 'З', 'з' };
                List<char> jlist = new List<char> { 'Ж', 'ж', 'Љ', 'љ', 'Ш', 'ш', 'Щ', 'щ', 'Й', 'й', 'Ч', 'ч' };

                if (fromchar == 'Е')
                {
                    if ((contextbefore == '#') || (vowels.Contains(contextbefore)))
                        return "Je";
                    else
                        return "E";
                }
                else if (fromchar == 'е')
                {
                    if ((contextbefore == '#') || (vowels.Contains(contextbefore)))
                        return "je";
                    else
                        return "e";
                }
                else if (fromchar == 'Ё')
                {
                    if (tszlist.Contains(contextbefore))
                        return "Io";
                    else if (jlist.Contains(contextbefore))
                        return "O";
                    else
                        return "Jo";
                }
                else if ((fromchar == 'ë') || (fromchar == 'ё'))
                {
                    if (tszlist.Contains(contextbefore))
                        return "io";
                    else if (jlist.Contains(contextbefore))
                        return "o";
                    else
                        return "jo";
                }
                else if (fromchar == 'Ю')
                {
                    if (tszlist.Contains(contextbefore))
                        return "Iu";
                    else if (jlist.Contains(contextbefore))
                        return "U";
                    else
                        return "Ju";
                }
                else if (fromchar == 'ю')
                {
                    if (tszlist.Contains(contextbefore))
                        return "iu";
                    else if (jlist.Contains(contextbefore))
                        return "u";
                    else
                        return "ju";
                }
                else if (fromchar == 'Я')
                {
                    if (tszlist.Contains(contextbefore))
                        return "Ia";
                    else if (jlist.Contains(contextbefore))
                        return "A";
                    else
                        return "Ja";
                }
                else if (fromchar == 'я')
                {
                    if (tszlist.Contains(contextbefore))
                        return "ia";
                    else if (jlist.Contains(contextbefore))
                        return "a";
                    else
                        return "ja";
                }
                else
                {
                    if (!badlist.Contains(fromchar))
                        badlist.Add(fromchar);

                    return badreturn;
                }

            }
        }

        public string Transliterate(string name, string lang)
        {
            char[] letters = name.ToCharArray();
            string result = "";
            for (int ic = 0; ic < letters.Length; ic++)
            {
                char contextbefore = '#';
                if (ic > 0)
                    contextbefore = letters[ic - 1];
                char contextafter = '#';
                if (ic < letters.Length - 1)
                    contextafter = letters[ic + 1];
                result += Transchar(letters[ic], contextbefore, contextafter, lang);
            }

            return result;
        }

        public void fill_cyrillic()
        {
            //Swedish transliteration!
            this.Addchar('А', "A", true);
            this.Addchar('а', "a", true);
            this.Addchar('Б', "B");
            this.Addchar('б', "b");
            this.Addchar('В', "V");
            this.Addchar('в', "v");
            this.Addchar('Г', "H", "uk");
            this.Addchar('г', "h", "uk");
            this.Addchar('Г', "H", "be");
            this.Addchar('г', "h", "be");
            this.Addchar('Г', "G");
            this.Addchar('г', "g");
            this.Addchar('Ѓ', "Ǵ");
            this.Addchar('ѓ', "ǵ");
            this.Addchar('Ґ', "G");
            this.Addchar('ґ', "g");
            this.Addchar('Д', "D");
            this.Addchar('д', "d");
            this.Addchar('Ђ', "D");
            this.Addchar('ђ', "đ");
            this.Addchar('Ђ', "Dj", "sr");
            this.Addchar('ђ', "dj", "sr");
            this.Addchar('Е', "*", true);
            this.Addchar('е', "*", true);
            this.Addchar('Е', "E", "uk", true);
            this.Addchar('е', "e", "uk", true);
            this.Addchar('Е', "E", "bg", true);
            this.Addchar('е', "e", "bg", true);
            this.Addchar('Ё', "*", true);
            this.Addchar('ë', "*", true);
            this.Addchar('ё', "*", true);
            this.Addchar('Є', "Je", true);//є
            this.Addchar('є', "je", true);
            this.Addchar('Ж', "Zj");
            this.Addchar('ж', "zj");
            this.Addchar('Ж', "Ž", "sr");
            this.Addchar('ж', "Ž".ToLower(), "sr");
            this.Addchar('Ж', "Ž", "mk");
            this.Addchar('ж', "Ž".ToLower(), "mk");
            this.Addchar('З', "Z");
            this.Addchar('з', "z");
            this.Addchar('Ѕ', "Dz", "mk");
            this.Addchar('ѕ', "dz", "mk");
            this.Addchar('И', "Y", "uk", true);
            this.Addchar('и', "y", "uk", true);
            this.Addchar('И', "Y", "be", true);
            this.Addchar('и', "y", "be", true);
            this.Addchar('И', "I", true);
            this.Addchar('и', "i", true);
            this.Addchar('Й', "J");
            this.Addchar('й', "j");
            this.Addchar('І', "I", true);
            this.Addchar('і', "і", true);
            this.Addchar('Ї', "Ji", true);
            this.Addchar('ї', "ji", true);
            this.Addchar('J', "J");
            this.Addchar('j', "j");
            this.Addchar('К', "K");//seemingly identical
            this.Addchar('K', "K");//but different unicodes
            this.Addchar('к', "k");
            this.Addchar('Ќ', "Ḱ");
            this.Addchar('ќ', "ḱ");
            this.Addchar('Л', "L");
            this.Addchar('л', "l");
            this.Addchar('Љ', "Lj");
            this.Addchar('љ', "lj");
            this.Addchar('М', "M");
            this.Addchar('м', "m");
            this.Addchar('Н', "N");
            this.Addchar('н', "n");
            this.Addchar('Њ', "Nj");
            this.Addchar('њ', "nj");
            this.Addchar('О', "O", true);
            this.Addchar('о', "o", true);
            this.Addchar('o', "o", true);
            this.Addchar('П', "P");
            this.Addchar('п', "p");
            this.Addchar('Р', "R");
            this.Addchar('р', "r");
            this.Addchar('С', "S");//seemingly identical
            this.Addchar('C', "S");//but different unicodes
            this.Addchar('с', "s");
            this.Addchar('Т', "T");
            this.Addchar('т', "t");
            this.Addchar('Ћ', "Ć");
            this.Addchar('ћ', "ć");
            this.Addchar('У', "U", true);
            this.Addchar('у', "u", true);
            this.Addchar('Ў', "Ŭ", true);
            this.Addchar('ў', "ŭ", true);
            this.Addchar('Ф', "F");
            this.Addchar('ф', "f");
            this.Addchar('Х', "H", "sr");
            this.Addchar('х', "h", "sr");
            this.Addchar('Х', "H", "mk");
            this.Addchar('х', "h", "mk");
            this.Addchar('Х', "Ch");
            this.Addchar('х', "ch");
            this.Addchar('Ц', "Ts");
            this.Addchar('ц', "ts");
            this.Addchar('Ц', "C", "sr");
            this.Addchar('ц', "c", "sr");
            this.Addchar('Ц', "C", "mk");
            this.Addchar('ц', "c", "mk");
            this.Addchar('Ч', "Tj");
            this.Addchar('ч', "tj");
            this.Addchar('Ч', "Č", "sr");
            this.Addchar('ч', "Č".ToLower(), "sr");
            this.Addchar('Ч', "Č", "mk");
            this.Addchar('ч', "Č".ToLower(), "mk");
            this.Addchar('Џ', "Dž");
            this.Addchar('џ', "dž");
            this.Addchar('Ш', "Sj");
            this.Addchar('ш', "sj");
            this.Addchar('Ш', "Š", "sr");
            this.Addchar('ш', "Š".ToLower(), "sr");
            this.Addchar('Щ', "Sjt", "bg");
            this.Addchar('щ', "sjt", "bg");
            this.Addchar('Щ', "Sjtj");
            this.Addchar('щ', "sjtj");
            this.Addchar('Ъ', "", true);
            this.Addchar('ъ', "", true);
            this.Addchar('Ъ', "", true);
            this.Addchar('ъ', "", true);
            this.Addchar('Ы', "Y", true);
            this.Addchar('ы', "y", true);
            this.Addchar('Ь', "", true);
            this.Addchar('ь', "", true);
            this.Addchar('Ѣ', "", true);
            this.Addchar('ѣ', "", true);
            this.Addchar('Э', "E", true);
            this.Addchar('э', "e", true);
            this.Addchar('Ю', "*", true);
            this.Addchar('ю', "*", true);
            this.Addchar('Я', "*", true);
            this.Addchar('я', "*", true);
            this.Addchar('Ө', "Ḟ");
            this.Addchar('ө', "ḟ");
            this.Addchar('Ѵ', "Ẏ");
            this.Addchar('ѵ', "ẏ");
            this.Addchar('Ѫ', "A", true);
            this.Addchar('ѫ', "a", true);//“”
            this.Addchar('“', "“");
            this.Addchar('”', "”");
            this.Addchar('«', "«");
            this.Addchar('»', "»");
            this.Addchar('’', "’");
            this.Addchar('„', "„");
            this.Addchar('´', "");//
            this.Addchar('Ғ', "Gh", "kk");
            this.Addchar('ғ', "gh");
            this.Addchar('Ə', "Ä", "kk", true);
            this.Addchar('ə', "ä", "kk", true);
            this.Addchar('İ', "I", "kk", true);
            this.Addchar('і', "i", "kk", true);
            this.Addchar('Қ', "Q", "kk");
            this.Addchar('қ', "q", "kk");
            this.Addchar('қ', "q");
            this.Addchar('Ң', "Ng", "kk");
            this.Addchar('ң', "ng", "kk");
            this.Addchar('Ө', "Ö", "kk", true);
            this.Addchar('ө', "ö", "kk", true);
            this.Addchar('Ү', "Ü", "kk", true);
            this.Addchar('ү', "ü", "kk", true);
            this.Addchar('Ұ', "U", "kk", true);
            this.Addchar('ұ', "u", "kk", true);
            this.Addchar('Һ', "H", "kk");
            this.Addchar('һ', "h", "kk");
            this.Addchar('Ң', "Ng", "ky");
            this.Addchar('ң', "ng", "ky");
            this.Addchar('Ө', "Ö", "ky", true);
            this.Addchar('ө', "ö", "ky", true);
            this.Addchar('Ү', "Ü", "ky", true);
            this.Addchar('ү', "ü", "ky", true);
            this.Addchar('γ', "ü", true);
            this.Addchar('Ғ', "Gh", "tg");
            this.Addchar('ғ', "gh", "tg");
            this.Addchar('Ӣ', "Y", "tg", true);
            this.Addchar('ӣ', "y", "tg", true);
            this.Addchar('Қ', "Q", "tg");
            this.Addchar('қ', "q", "tg");
            this.Addchar('Ӯ', "Ö", "tg", true);
            this.Addchar('ӯ', "ö", "tg", true);
            this.Addchar('Ҳ', "H", "tg");
            this.Addchar('ҳ', "h", "tg");
            this.Addchar('Ҷ', "Dzj", "tg");
            this.Addchar('ҷ', "dzj", "tg");
            this.Addchar('ж', "j", "mn");
            this.Addchar('Ж', "J", "mn");
            this.Addchar('З', "Dz", "mn");
            this.Addchar('з', "dz", "mn");
            this.Addchar('Ы', "Ij", "mn");
            this.Addchar('ы', "ij", "mn");
            this.Addchar('Ө', "Ö", "mn", true);
            this.Addchar('ө', "ö", "mn", true);
            this.Addchar('Ү', "Ü", "mn", true);
            this.Addchar('ү', "ü", "mn", true);//
            this.Addchar('ј', "j");
            this.Addchar('Ј', "J");
            this.Addchar('ј', "j");
            this.Addchar('ј', "j");
            this.Addchar('ј', "j");

        }

        public static void read_translit(string tlcountry)
        {

            string filename = geonameclass.geonamesfolder + @"translit\translit-" + tlcountry + ".txt";
            int n = 0;
            if (File.Exists(filename))
            {
                Console.WriteLine("read_translit " + filename);
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

                        if (words.Length < 3)
                            continue;

                        altnameclass an = new altnameclass();

                        an.altid = -1;

                        int gnid = util.tryconvert(words[0]);

                        if (!Form1.checkdoubles && !Form1.gndict.ContainsKey(gnid))
                            continue;

                        an.altname = words[2];

                        an.ilang = Form1.langtoint["sv"];

                        if (Form1.makelang == "sv")
                        {
                            if (Form1.gndict.ContainsKey(gnid))
                            {
                                if (Form1.gndict[gnid].Name_ml == Form1.gndict[gnid].Name)
                                    Form1.gndict[gnid].Name_ml = an.altname;
                            }

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
            }
            else
                Console.WriteLine("File not found! " + filename);
            Console.WriteLine("Names found in translit: " + n.ToString());
        }


    }

}
