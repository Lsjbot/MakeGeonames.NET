using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    public class islandclass //data for each island
    {
        public double area = 0;
        public double kmew = 0;
        public double kmns = 0;
        public List<int> onisland = new List<int>(); //list of GeoNames id of entities located on the island.
    }

}
