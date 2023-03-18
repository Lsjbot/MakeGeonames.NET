using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    public class altnameclass //class for alternate names from GeoNames
    {
        public int altid = 0;
        public string altname = "";
        public int ilang = -1;
        public string wikilink = "";
        public bool official = false;
        public bool shortform = false;
        public bool colloquial = false;
        public bool historic = false;
    }
}
