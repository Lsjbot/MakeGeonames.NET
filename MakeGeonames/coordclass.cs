using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    public class coordclass
    {
        public double lat = 9999;
        public double lon = 9999;

        public static coordclass gnidcoord(int gnid)
        {
            coordclass cc = new coordclass();

            if (!Form1.gndict.ContainsKey(gnid))
                return cc;

            cc.lat = Form1.gndict[gnid].latitude;
            cc.lon = Form1.gndict[gnid].longitude;
            return cc;
        }

        public static coordclass countrylatlong(int gnid)
        {
            coordclass cc = gnidcoord(gnid);

#if (DBGEOFLAG)

            if (countrydict.ContainsKey(gnid))
            {
                if (countrydict[gnid].shape != null)
                {
                    if (countrydict[gnid].shape.metadict.ContainsKey("Centroid latitude"))
                        cc.lat = util.tryconvertdouble(countrydict[gnid].shape.metadict["Centroid latitude"]);
                    if (countrydict[gnid].shape.metadict.ContainsKey("Centroid longitude"))
                        cc.lon = util.tryconvertdouble(countrydict[gnid].shape.metadict["Centroid longitude"]);
                }
            }
#endif
            return cc;
        }

        public static int getcountrypart(int gnid)
        {
            double gnidlat = Form1.gndict[gnid].latitude;
            double gnidlong = Form1.gndict[gnid].longitude;
            coordclass cc = countrylatlong(Form1.gndict[gnid].adm[0]);
            double countrylat = cc.lat;
            double countrylong = cc.lon;
            double area = Form1.countrydict[Form1.gndict[gnid].adm[0]].area;
            double kmdeg = 40000 / 360; //km per degree at equator
            double scale = Math.Cos(0.5 * (countrylat + gnidlat) * 3.1416 / 180); //latitude-dependent longitude scale
            double dlat = (gnidlat - countrylat) * kmdeg;
            double dlong = (gnidlong - countrylong) * kmdeg * scale;

            if (countrylat < -80) //Antarctica
            {
                if (gnidlat < -86) //central part
                    return 82;

                if ((gnidlat > -61) && ((gnidlong > -47) && (gnidlong < -44))) //South Orkney Islands
                    return 84;

                if (gnidlong > -45) //East Antarctica
                    return 86;
                else                 //West Antarctica
                {
                    if ((gnidlong > -64) && (gnidlong < -52))
                    {
                        if (((gnidlong < -60) && (gnidlat > -64)) || ((gnidlong >= -60) && (gnidlat > -62.8))) //South Shetland Islands
                            return 83;
                        else
                            return 85;
                    }
                    else
                        return 85;
                }
            }

            if ((dlat * dlat + dlong * dlong) < (area / 9)) //central part
                return 82;

            if (dlat * dlat > 4 * dlong * dlong)
            {
                if (dlat > 0) // northern part
                    return 83;
                else           //southern part
                    return 84;
            }
            else if (dlong * dlong > 4 * dlat * dlat)
            {
                if (dlong > 0) // eastern part
                    return 86;
                else            //western part
                    return 85;
            }
            else if (dlong > 0)
            {
                if (dlat > 0) //northeastern
                    return 87;
                else           //southeastern
                    return 88;
            }
            else
            {
                if (dlat > 0) //northwestern
                    return 89;
                else           //southwestern
                    return 90;
            }


        }


    }
}
