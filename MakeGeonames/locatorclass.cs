using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetWikiBot;

namespace MakeGeonames
{
    public class locatorclass //for locator maps
    {
        public string locatorname = ""; //country name in locator template on target wiki
        public string locatorimage = "";//map image name
        public string altlocator = "";
        public double latmin = -999;
        public double latmax = -999;
        public double lonmin = -999;
        public double lonmax = -999;
        public bool loaded = false;
        public string get_locator(double lat, double lon)
        {
            if (!loaded)
            {
                string templatename = util.mp(63) + util.mp(72).Replace("{{", "") + " " + locatorname;
                Console.WriteLine(templatename);
                //string imagename = "";
                Page ltp = new Page(Form1.makesite, templatename);
                util.tryload(ltp, 2);
                if (ltp.Exists())
                {
                    locatorimage = Form1.get_pictureparam(ltp);
                    latmax = Form1.get_edgeparam(ltp, "top");
                    latmin = Form1.get_edgeparam(ltp, "bottom");
                    lonmax = Form1.get_edgeparam(ltp, "right");
                    lonmin = Form1.get_edgeparam(ltp, "left");
                    Console.WriteLine("3 lonmin, lonmax = " + lonmin + ", " + lonmax);
                }
                loaded = true;
            }

            if (String.IsNullOrEmpty(altlocator))
            {
                if (String.IsNullOrEmpty(locatorname))
                {
                    if (Form1.makelang == "sv")
                        return "Världen";
                    else
                        return "World";
                }
                else
                    return locatorname;
            }

            if (latmin < -99) //failed to get edges, probably complicated coordinates
                return locatorname;

            if (lat < latmin)
                return altlocator;
            if (lat > latmax)
                return altlocator;
            if (lon < lonmin)
                return altlocator;
            if (lon > lonmax)
                return altlocator;
            return locatorname;
        }

        public bool near_eastern_edge(double lat, double lon, string name) //determine if lat/long near estern edge of map
        {
            if (!loaded)
            {
                string templatename = util.mp(63) + util.mp(72).Replace("{{", "") + " " + locatorname;
                Console.WriteLine(templatename);
                //string imagename = "";
                Page ltp = new Page(Form1.makesite, templatename);
                util.tryload(ltp, 2);
                if (ltp.Exists())
                {
                    locatorimage = Form1.get_pictureparam(ltp);
                    latmax = Form1.get_edgeparam(ltp, "top");
                    latmin = Form1.get_edgeparam(ltp, "bottom");
                    lonmax = Form1.get_edgeparam(ltp, "right");
                    lonmin = Form1.get_edgeparam(ltp, "left");
                    Console.WriteLine("2 lonmin, lonmax = " + lonmin + ", " + lonmax);
                }
                loaded = true;
            }

            Console.WriteLine("1 lonmin, lonmax = " + lonmin + ", " + lonmax);
            if (latmin < -99) //failed to get edges, probably complicated coordinates
                return false;

            double margin = 0.8;
            if (!String.IsNullOrEmpty(name))
            { //rough estimate of name size on map
                string[] ns = name.Split();
                int lmax = (from c in ns select c.Length).Max();
                margin = 0.96 - 0.023 * lmax;
            }
            if ((lon - lonmin) / (lonmax - lonmin) > margin)
                return true;
            else
                return false;

        }

    }

}
