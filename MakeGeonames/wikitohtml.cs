using System;
using System.Collections.Generic;
using System.Linq;
using DotNetWikiBot;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace MakeGeonames
{
    class wikitohtml
    {
        static char[] trimchars = { ' ', '\n' };
        public static string convert(string text, Site site)
        {
            string html = "";
            //https://sv.wikipedia.org/w/api.php?action=parse&text=xxxx%20[[prov|test]]%20{{robotskapad}}&contentmodel=wikitext 

            string url = "action=parse&text=" + WebUtility.UrlEncode(text) + "&contentmodel=wikitext&format=json&prop=text|links|images";
            //string tmpStr = site.PostDataAndGetResultHTM(site.site+"/w/api.php", url);
            try
            {
                //string tmpStr = site.PostDataAndGetResultHTM(site.site + "/w/api.php", url);
                string tmpStr = site.PostDataAndGetResult(site.address + "/w/api.php", url);
                //Console.WriteLine(tmpStr);
                JObject wd = JObject.Parse(tmpStr);
                html = wd["parse"]["text"]["*"].ToString();
                html = html.Trim('"').Replace("\\\"","\"").Replace("//upload.","https://upload.").Replace("//commons.", "https://commons.").Replace("&lt;","<").Replace("&gt;", ">").Replace("â†‘", "↑");

            }
            catch (WebException e)
            {
                string message = e.Message;
                Console.Error.WriteLine(message);
            }


            return html;
        }

        static string removecomment(string s)
        {
            //Regex HeaderPattern = new Regex("<!--(.+?)-->");
            return Regex.Replace(s, "<!--(.+?)-->", "");
        }

        public static string make_html_box(Page dummy, Site makesite)
        {
            string s = "<dl><aside id='facts'>\n";

            Dictionary<string, string> par = Page.ParseTemplate(dummy.text);

            foreach (string pname in par.Keys)
            {
                if (pname == "0")
                    continue;
                Console.WriteLine(pname + "|" + par[pname] + "|");
                if (!String.IsNullOrEmpty(removecomment(par[pname]).Trim(trimchars)))
                {
                    s += "<dt>" + pname + "</dt>\n";
                    if ( pname == "image")
                    {
                        //<a href="/wiki/Payl:Flag_of_Andorra.svg" class="image" title="Flag of Andorra">
                        string imagelink = "//commons.wikimedia.org/wiki/File:" + par[pname];
                        s += "<dd>\n <a href=\""+imagelink+"\" class=\"image\"><img alt=\"Image\" src=\"" + imagelink + "\" width=\"100\" height=\"100\"/></a> \n</dd>\n";
                    }
                    else
                        s += "<dd>\n " + par[pname] + "\n</dd>\n";
                }

                //<img alt="Robot icon.svg" src="https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Robot_icon.svg/40px-Robot_icon.svg.png" width="40" height="40" srcset="https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Robot_icon.svg/60px-Robot_icon.svg.png 1.5x, https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Robot_icon.svg/80px-Robot_icon.svg.png 2x" data-file-width="48" data-file-height="48" />
            }

            s += "</aside>\n</dl>";

            return s;
        }


    }
}
