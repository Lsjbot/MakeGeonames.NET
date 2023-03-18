using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    public class lakeclass //data for each lake
    {
        public double area = 0;
        public double glwd_area = 0;
        public double kmew = 0;
        public double kmns = 0;
        public int higher = 0; //edge pixels higher than lake surface
        public int lower = 0; //edge pixels lower than lake surface
        public int overlaps_with = -1; //if two lakes overlap, gnid of the other one
        public int glwd_id = -1; //id number in GLWD lakes database, -1 if not found
        public List<int> inlake = new List<int>(); //list of GeoNames id of entities located in the lake (mainly islands).
        public List<int> atlake = new List<int>(); //list of GeoNames id of entities located around the lake.
    }

}
