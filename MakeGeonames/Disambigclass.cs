using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    public class Disambigclass //class for disambiguation in article names
    {
        public bool existsalready = false;
        public bool country = false;
        public bool adm1 = false;
        public bool adm2 = false;
        public bool latlong = false;
        public bool fcode = false;
        public forkclass fork = new forkclass();
    }

}
