using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    public class rangeclass //data for each MTS/HLLS
    {
        public double length = 0;
        public string orientation = "....";
        public double angle = 0; //polar angle of long axis (radians). 0 or pi = EW, pi/2 or 3pi/2 = NS etc.
        public double kmew = 0;
        public double kmns = 0;
        public int maxheight = 0; //highest point; gnid of peak if negative, height if positive
        public double hlat = 999; //latitude/longitude of highest point
        public double hlon = 999;
        public List<int> inrange = new List<int>(); //list of GeoNames id of mountains in the range.
    }

}
