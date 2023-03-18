using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    public class drainageclass //data for each drainage
    {
        public double area = 0; //area of watershed
        public double kmew = 0;
        public double kmns = 0;
        public double length = 0;
        public List<int> inarea = new List<int>(); //list of GeoNames id of entities located in the catchment area of the river.
        public string drainage_name = ""; //name of drainage basin, index to drainageshapedict
        public int main_river = -1; //gnid of main river; index to riverdict
        public string main_river_artname = ""; //article name of main river
    }
}
