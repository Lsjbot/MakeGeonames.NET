using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    public class wdminiclass //minimal wikidata entry needed for verifying Geonames-links
    {
        public int gnid = 0;
        public double latitude = 9999.9;
        public double longitude = 9999.9;
        public List<int> instance_of = new List<int>();
        public double dist = 9999.9;
        public bool okdist = false;
        public bool okclass = false;
        public bool goodmatch = false;
    }
}
