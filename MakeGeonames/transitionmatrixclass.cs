using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    class transitionmatrixclass
    {
        Dictionary<char, Dictionary<char, double>> m = new Dictionary<char, Dictionary<char, double>>();
        Dictionary<string, Dictionary<char, double>> m2 = new Dictionary<string, Dictionary<char, double>>();
        Dictionary<char, double> msum = new Dictionary<char, double>();

        public static char wordstart = '*';
        public static char wordend = '#';
        public static double zeropenalty = -20;
        public string language;

        public transitionmatrixclass(string lang)
        {
            language = lang;
        }
        public void Add(string w)
        {
            char[] cc = (wordstart+w+wordend).ToCharArray();

            for (int i=0;i<cc.Length-1;i++)
            {
                if (!m.ContainsKey(cc[i]))
                    m.Add(cc[i], new Dictionary<char, double>());
                if (!m[cc[i]].ContainsKey(cc[i + 1]))
                    m[cc[i]].Add(cc[i + 1], 1);
                else
                    m[cc[i]][cc[i + 1]]++;
                if (i < cc.Length-2)
                {
                    string c2 = cc[i].ToString() + cc[i + 1];
                    if (!m2.ContainsKey(c2))
                        m2.Add(c2, new Dictionary<char, double>());
                    if (!m2[c2].ContainsKey(cc[i + 2]))
                        m2[c2].Add(cc[i + 2], 1);
                    else
                        m2[c2][cc[i + 2]]++;
                }
            }
        }

        public void Add(List<string> wordlist)
        {
            foreach (string w in wordlist)
                this.Add(w);
        }

        public void Normalize()
        {
            foreach (char c in m.Keys.ToList())
            {
                double msum = m[c].Values.Sum();
                foreach (char c2 in m[c].Keys.ToList())
                    m[c][c2] = Math.Log(m[c][c2] / msum);
            }
            foreach (string s in m2.Keys.ToList())
            {
                double m2sum = m2[s].Values.Sum();
                foreach (char c2 in m2[s].Keys.ToList())
                    m2[s][c2] = Math.Log(m2[s][c2] / m2sum);

            }
        }

        public double Evaluate(string w)
        {
            double logsum = 0;
            if (w.Contains(' '))
            {
                foreach (string ww in w.Split())
                    logsum += Evaluate(ww);
            }
            else
            {
                char[] cc = (wordstart + w + wordend).ToCharArray();

                for (int i = 0; i < cc.Length - 1; i++)
                {
                    if (!m.ContainsKey(cc[i]))
                        logsum += zeropenalty;
                    else
                    {
                        if (!m[cc[i]].ContainsKey(cc[i + 1]))
                            logsum += zeropenalty;
                        else
                            logsum += m[cc[i]][cc[i + 1]];
                    }
                }
            }
            return logsum;

        }
        public double Evaluate2(string w)
        {
            double logsum = 0;
            if (w.Contains(' '))
            {
                foreach (string ww in w.Split())
                    logsum += Evaluate2(ww);
            }
            else
            {
                char[] cc = (wordstart + w + wordend).ToCharArray();

                for (int i = 0; i < cc.Length - 2; i++)
                {
                    string c2 = cc[i].ToString() + cc[i + 1];
                    if (!m2.ContainsKey(c2))
                        logsum += zeropenalty;
                    else
                    {
                        if (!m2[c2].ContainsKey(cc[i + 2]))
                            logsum += zeropenalty;
                        else
                            logsum += m2[c2][cc[i + 2]];
                    }
                }
            }
            return logsum;

        }
    }
}
