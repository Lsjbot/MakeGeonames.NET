using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    public class forkclass
    {
        public int geonameid = 0;
        public string featurecode = "";
        public string[] admname = new string[3];
        public double latitude = 0.0;
        public double longitude = 0.0;
        public string realname = "*";
        public int wdid = -1;    //wikidata id
        public string iso = "XX"; //country iso code
        public string featurename = "";

    }
}
