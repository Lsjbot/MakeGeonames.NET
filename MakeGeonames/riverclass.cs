using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    public class riverclass //data for each river/lake
    {
        public double length = -1;
        public int tributary_of = -1; //if river joins another, gnid of the other
        public List<int> tributaries = new List<int>(); //list of rivers joining this one
        public string drainage_name = ""; //name of drainage basin, index to drainageshapedict
        public string river_name = ""; //name of river, index to rivershapedict (blank if no rivershape)
        public string river_artname = ""; //article name of river/lake
        public bool is_lake = false; //true if "river" is really a lake
        public bool is_drainage = false; //true if main river in drainage area

#if (DBGEOFLAG)

        public static string reorient_polygon(string text)
        {
            string s = text.Replace("POLYGON ((","").Replace("))","");

            string rs = "";

            bool first = true;
            foreach (string cs in s.Split(','))
            {
                if (first)
                {
                    first = false;
                }
                else
                    rs = ", " + rs;

                rs = cs + rs;
            }

            rs = "POLYGON ((" + rs + "))";
            //Console.WriteLine("Original  : " + text);
            //Console.WriteLine("Reoriented: " + rs);
            return rs;
        }

        public static string close_polygon(string text)
        {
            string rs = text.Replace("POLYGON ((", "").Replace("))", "");

            

            bool first = true;
            string firstpoint = "";
            string lastpoint = "";
            foreach (string cs in rs.Trim().Split(','))
            {
                if (first)
                {
                    first = false;
                    firstpoint = cs.Trim();
                }
                lastpoint = cs.Trim();
            }
            if (lastpoint != firstpoint) //Add first point at the end
            {
                Console.WriteLine("Closing polygon");
                rs += ", " + firstpoint;
            }

            rs = "POLYGON ((" + rs + "))";
            if (text != rs)
            {
                Console.WriteLine("Original  : " + text);
                Console.WriteLine("Closed: " + rs);
                Console.ReadLine();
            }
            return rs;
        }

        public static bool clockwise(string text, double cx, double cy)
        {
            //bool cw = true;

            string s = text.Replace("POLYGON ((", "").Replace("))", "");

            double anglesum = 0;

            bool first = true;
            double oldx = 0;
            double oldy = 0;
            double oldangle = 0;
            foreach (string cs in s.Split(','))
            {
                //Console.WriteLine(cs);
                double x = util.tryconvertdouble(cs.Trim().Split(' ')[0]);
                double y = util.tryconvertdouble(cs.Trim().Split(' ')[1]);
                //Console.WriteLine(x + " " + y);
                double angle = Math.Atan2(y - cy, x - cx);
                //Console.WriteLine("angle = " + angle);
                if (first)
                {
                    first = false;
                }
                else
                {
                    double anglechange = angle - oldangle;
                    if (anglechange > Math.PI)
                        anglechange = anglechange - 2 * Math.PI;
                    else if (anglechange < -Math.PI)
                        anglechange = anglechange + 2 * Math.PI;
                    //Console.WriteLine("anglechange = " + anglechange);

                    anglesum += anglechange;
                }
                oldx = x;
                oldy = y;
                oldangle = angle;
            }

            return (anglesum < 0);
        }

        public static DbGeography tryfromtext(string text)
        {
             

            try
            {
                DbGeography dg = DbGeography.FromText(text);
                return dg;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                //Console.WriteLine(e.InnerException.Message);
                try
                {
                    DbGeography dg = DbGeography.FromText(reorient_polygon(text));
                    return dg;
                }
                catch (Exception e2)
                {
                    Console.WriteLine(e2.Message);
                    Console.WriteLine(e2.InnerException.Message);
                    return null;
                }
            }

            //return null;
        }

        public static string strip_polygon(string text,bool naked)
        {
            if ( naked ) //keep no parentheses
                return text.Replace("POLYGON ((", "").Replace("))", "");
            else //keep single parentheses
                return text.Replace("POLYGON ((", "(").Replace("))", ")");
        }

        public static void convert_shapelist(string filename)
        {
            List<shapeclass> shapelist = read_shapelist(filename + ".shp.txt");

            write_shapelist(filename + ".multipoly.txt", shapelist);

        }

        public static void read_lakeshapes()
        {
            lakeshapedict.Clear();
            countrylakedict.Clear();

            Console.WriteLine("read_lakeshapes: glwd_1.multipoly.txt");
            foreach (shapeclass sc in read_shapelist(geonameclass.geonamesfolder+"glwd_1.multipoly.txt"))
            {
                int glwd_id = -1;
                if (sc.metadict.ContainsKey("glwd_id"))
                {
                    glwd_id = util.tryconvert(sc.metadict["glwd_id"]);
                    lakeshapedict.Add(glwd_id, sc);
                }
            }
            Console.WriteLine("read_lakeshapes: glwd_2.multipoly.txt");
            foreach (shapeclass sc in read_shapelist(geonameclass.geonamesfolder+"glwd_2.multipoly.txt"))
            {
                int glwd_id = -1;
                if (sc.metadict.ContainsKey("glwd_id"))
                {
                    glwd_id = util.tryconvert(sc.metadict["glwd_id"]);
                    lakeshapedict.Add(glwd_id, sc);
                }
            }

            Console.WriteLine("read_lakeshapes: glwd-countries.txt");

            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + "glwd-countries.txt"))
            {
                while (!sr.EndOfStream)
                {

                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    if (words.Length < 2)
                        continue;
                    if ( !countryid.ContainsKey(words[0]))
                        continue;
                    int gnid = countryid[words[0]];

                    if (!countrylakedict.ContainsKey(gnid))
                    {
                        List<int> ll = new List<int>();
                        countrylakedict.Add(gnid, ll);
                        for (int i = 1; i < words.Length; i++)
                        {
                            int glwd_id = util.tryconvert(words[i]);
                            if ((glwd_id > 0) && (lakeshapedict.ContainsKey(glwd_id)))
                                countrylakedict[gnid].Add(glwd_id);
                        }
                    }

                }
            }
        }

        public static void read_rivernamefile(string filename)
        {
            //            public class riverclass //data for each river/lake
            //{
            //    public double length = -1;
            //    public int tributary_of = -1; //if river joins another, gnid of the other
            //    public List<int> tributaries = new List<int>(); //list of rivers joining this one
            //    public string drainage_name = ""; //name of drainage basin, index to drainageshapedict
            //    public string river_name = ""; //name of river, index to rivershapedict (blank if no rivershape)
            //    public string river_artname = ""; //article name of river/lake
            //    public bool is_lake = false; //true if "river" is really a lake
            //    public bool is_drainage = false; //true if main river in drainage area
            //}

            Console.WriteLine("rivernamefile " + filename);
            int nriver = 0;
            using (StreamReader sr = new StreamReader(geonameclass.geonamesfolder + filename))
            {
                while (!sr.EndOfStream)
                {

                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    if (words.Length < 2)
                        continue;
                    riverclass rc = new riverclass();
                    rc.river_name = words[0];
                    rc.drainage_name = words[1];
                    int gnid = util.tryconvert(words[7]);
                    if (gnid <= 0)
                        continue;
                    if (words[6] == words[7])
                    {
                        rc.is_drainage = true;
                        rc.river_artname = words[5];
                    }
                    else
                    {
                        rc.is_drainage = false;
                        rc.river_artname = words[8];
                    }
                    rc.is_lake = (words[0].Contains("Lake") || rc.river_artname.Contains("Lake") || rc.river_artname.Contains("lanaw"));

                    if (!rivernamedict.ContainsKey(gnid))
                    {
                        rivernamedict.Add(gnid, rc);
                        nriver++;
                    }
                    else
                    {
                        Console.WriteLine("Duplicate gnid for " + rc.river_name);
                        Console.ReadLine();
                    }
                }
            }
            Console.WriteLine("rivernamefile " + nriver + " rivers");

        }

        public static void read_river_file(string wdcountry)
        {
            //            public class riverclass //data for each river/lake
            //{
            //    public double length = -1;
            //    public int tributary_of = -1; //if river joins another, gnid of the other
            //    public List<int> tributaries = new List<int>(); //list of rivers joining this one
            //    public string drainage_name = ""; //name of drainage basin, index to drainageshapedict
            //    public string river_name = ""; //name of river, index to rivershapedict (blank if no rivershape)
            //    public string river_artname = ""; //article name of river/lake
            //    public bool is_lake = false; //true if "river" is really a lake
            //    public bool is_drainage = false; //true if main river in drainage area
            //}
            string filename = geonameclass.geonamesfolder + @"rivers\rivers-" + wdcountry + ".txt";
            riverdict.Clear();
            if (!File.Exists(filename))
            {
                Console.WriteLine(filename + " not found.");
                return;
            }

            try
            {
                int nrivers = 0;
                using (StreamReader sr = new StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        string[] words = line.Split('\t');
                        nrivers++;
                        int gnid = util.tryconvert(words[0]);
                        if (!gndict.ContainsKey(gnid))
                            continue;
                        riverclass rc = new riverclass();
                        rc.drainage_name = words[1];
                        rc.tributary_of = util.tryconvert(words[2]);
                        riverdict.Add(gnid, rc);
                    }
                }

                Console.WriteLine("# rivers = " + nrivers.ToString());

            }
            catch (IOException e)
            {
                string message = e.Message;
                Console.Error.WriteLine(message);
            }
        }

        public static void read_rivers()
        {
            read_rivershapes();
            read_drainagedict("drainage_names3.txt");
            read_rivernamefile("river_names.txt");
            
        }

        public static void read_rivershapes()
        {
            rivershapedict.Clear();
            drainageshapedict.Clear();

            Console.WriteLine("read_rivershapes: GRDC_687_rivers.shp.txt");
            foreach (shapeclass sc in read_shapelist(geonameclass.geonamesfolder + "GRDC_687_rivers.shp.txt"))
            {
                string rivername = "";
                if (sc.metadict.ContainsKey("river_lake"))
                {
                    rivername = sc.metadict["river_lake"];
                    if (!rivershapedict.ContainsKey(rivername))
                        rivershapedict.Add(rivername, sc);
                    else
                        Console.WriteLine("Duplicate river " + rivername);
                }
            }
            Console.WriteLine("read_rivershapes: GRDC_405_basins_from_mouth.multipoly.txt");
            foreach (shapeclass sc in read_shapelist(geonameclass.geonamesfolder + "GRDC_405_basins_from_mouth.multipoly.txt"))
            {
                string rivername = "";
                if (sc.metadict.ContainsKey("drainage"))
                {
                    rivername = sc.metadict["drainage"];
                    if (!drainageshapedict.ContainsKey(rivername))
                        drainageshapedict.Add(rivername, sc);
                    else
                        Console.WriteLine("Duplicate drainage " + rivername);
                    
                }
            }
        }


        public static void make_glwd_countries()
        {

            foreach (shapeclass sc in read_shapelist(geonameclass.geonamesfolder+"glwd_1.multipoly.txt"))
            {
                int glwd_id = -1;
                if (sc.metadict.ContainsKey("glwd_id"))
                {
                    glwd_id = util.tryconvert(sc.metadict["glwd_id"]);
                    lakeshapedict.Add(glwd_id, sc);

                    List<int> countries = new List<int>();
                    foreach (DbGeography dlake in sc.shapes)
                    {
                        foreach (int gnid in countrydict.Keys)
                        {
                            bool found = false;
                            if (countrydict[gnid].shape != null)
                            {
                                foreach (DbGeography dcountry in countrydict[gnid].shape.shapes)
                                {
                                    if (dcountry.Intersects(dlake))
                                    {
                                        found = true;
                                        DbGeography dis = dcountry.Intersection(dlake);
                                        Console.WriteLine("Intersection : " + dis.Area + ", " + dlake.Area);
                                    }
                                    else
                                    {
                                        Console.Write("No intersection : ");
                                        DbGeography dis = dcountry.Intersection(dlake);
                                        if (dis == null)
                                            Console.WriteLine("dis = null");
                                        else if (dis.IsEmpty)
                                            Console.WriteLine("dis.IsEmpty");
                                        else
                                            Console.WriteLine("dis.Area = "+dis.Area);
                                    }
                                    Console.ReadLine();
                                }
                            }
                            if (found)
                                countries.Add(gnid);
                        }
                    }

                    Console.Write("Countries matching: ");
                    foreach (int gnid in countries)
                    {
                        Console.Write(countrydict[gnid].Name);
                        if ( !countrylakedict.ContainsKey(gnid))
                        {
                            List<int> ll = new List<int>();
                            countrylakedict.Add(gnid,ll);
                        }
                        countrylakedict[gnid].Add(glwd_id);
                    }
                    Console.WriteLine();
                    Console.Write("Countries in lake file: ");
                    if (sc.metadict.ContainsKey("country"))
                        Console.Write(sc.metadict["country"]);
                    if (sc.metadict.ContainsKey("sec_cntry"))
                        Console.Write(sc.metadict["sec_cntry"]);
                    Console.WriteLine();
                }
            }

            foreach (shapeclass sc in read_shapelist(geonameclass.geonamesfolder+"glwd_2.multipoly.txt"))
            {
                int glwd_id = -1;
                if (sc.metadict.ContainsKey("glwd_id"))
                {
                    glwd_id = util.tryconvert(sc.metadict["glwd_id"]);
                    lakeshapedict.Add(glwd_id, sc);

                    List<int> countries = new List<int>();
                    foreach (DbGeography dlake in sc.shapes)
                    {
                        foreach (int gnid in countrydict.Keys)
                        {
                            bool found = false;
                            if (countrydict[gnid].shape != null)
                            {
                                foreach (DbGeography dcountry in countrydict[gnid].shape.shapes)
                                {
                                    if (dcountry.Intersects(dlake))
                                    {
                                        found = true;
                                    }
                                }
                            }
                            if (found)
                                countries.Add(gnid);
                        }
                    }

                    Console.Write("Countries matching: ");
                    foreach (int gnid in countries)
                    {
                        Console.Write(countrydict[gnid].Name);
                        if (!countrylakedict.ContainsKey(gnid))
                        {
                            List<int> ll = new List<int>();
                            countrylakedict.Add(gnid, ll);
                        }
                        countrylakedict[gnid].Add(glwd_id);
                    }
                    Console.WriteLine();
                    Console.Write("Countries in lake file: ");
                    if (sc.metadict.ContainsKey("country"))
                        Console.Write(sc.metadict["country"]);
                    if (sc.metadict.ContainsKey("sec_cntry"))
                        Console.Write(sc.metadict["sec_cntry"]);
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Writing glwd-countries.txt...");
            using (StreamWriter sw = new StreamWriter("glwd-countries.txt"))
            {
                foreach (int gnid in countrylakedict.Keys)
                {
                    sw.Write(countrydict[gnid].iso);
                    foreach (int glwd_id in countrylakedict[gnid])
                        sw.Write("\t"+glwd_id);
                    sw.WriteLine();
                }
            }
            Console.WriteLine("End of lakeshapes");
            Console.ReadLine();
        }

        public static List<shapeclass> read_shapelist(string filename)
        {
            int n = 0;

            //Testing:

            //DbGeography dg1 = DbGeography.FromText("POINT (30 10)");
            //dg1 = DbGeography.FromText("POLYGON ((30 10, 40 40, 20 40, 10 20, 30 10))");
            //Console.WriteLine("Test 1 ok");
            //dg1 = DbGeography.FromText("POLYGON ((30 10, 40.5 40.5, 20 40, 10 20, 30 10))");
            //Console.WriteLine("Test 2 ok");
            //dg1 = DbGeography.FromText("POLYGON ((-2 0,-1 -1,0 0,1 -1,2 0,0 2,-2 0))");
            //Console.WriteLine("Test 3 ok");
            //dg1 = DbGeography.FromText("POLYGON ((-2 0,-1.5 -1.5,0 0,1 -1,2 0,0 2,-2 0))");
            //Console.WriteLine("Test 3 ok again");
            //dg1 = tryfromtext("POLYGON ((-30 10,40.5 40.5,20 -40, 10 20,-30 10))");
            //Console.WriteLine("Test 4 ok");
            //dg1 = tryfromtext("POLYGON ((-69.9969376289999 12.577582098, -69.936390754 12.531724351, -69.924672004 12.519232489, -69.9157608709999 12.4970156920001, -69.8801977199999 12.453558661, -69.8768204419999 12.4273949240001, -69.888091601 12.417669989, -69.9088028639999 12.4177920590001, -69.9305313789999 12.4259707700001, -69.9451391269999 12.4403750670001, -69.924672004 12.4403750670001, -69.924672004 12.4472110050001, -69.9585668609999 12.4632022160001, -70.027658658 12.5229352890001, -70.0480850899999 12.5311546900001, -70.0580948559999 12.5371768250001, -70.0624080069999 12.54682038, -70.060373502 12.5569522160001, -70.0510961579999 12.5740420590001, -70.0487361319999 12.5837263040001, -70.052642382 12.600002346, -70.0596410799999 12.614243882, -70.0611059239999 12.6253929710001, -70.0487361319999 12.6321475280001, -70.0071508449999 12.5855166690001, -69.9969376289999 12.577582098))");
            //Console.WriteLine("Test 5 ok");

            //string s = "POLYGON ((10 10,10 20,20 20,20 10,10 10))";
            //Console.WriteLine("Clockwise = "+clockwise(s,15,15));
            //Console.WriteLine("Clockwise(reoriented) = " + clockwise(reorient_polygon(s), 15, 15));
            //Console.ReadLine();

            //        public class shapeclass
            //{
            //    public Dictionary<string, string> metadict = new Dictionary<string, string>();
            //    public List<DbGeography> shapes = new List<DbGeography>();
            //}

            List<shapeclass> shapelist = new List<shapeclass>();

            bool mpfile = filename.Contains("multipoly") || filename.Contains("rivers");
            int ngood = 0;
            int nfail = 0;
            int nccwsgood = 0;
            int nccwsfail = 9999;
            int ncwsgood = 0;
            int ncwsfail = 9999;
            int mpgood = 0;
            int mpfail = 9999;


            using (StreamReader sr = new StreamReader(filename))
            {
                while (!sr.EndOfStream)
                {
                    shapeclass sc = new shapeclass();
                    double areasum = 0.0;
                    double xwsum = 0.0;
                    double ywsum = 0.0;

                    Dictionary<string, DbGeometry> clockwiselist = new Dictionary<string, DbGeometry>();
                    Dictionary<string, DbGeometry> counterclockwiselist = new Dictionary<string, DbGeometry>();
                    Dictionary<string, List<string>> multipoly = new Dictionary<string, List<string>>();
                    string largest = "";
                    double largestarea = -1;

                    while (true)
                    {
                        String line = sr.ReadLine();
                        //Console.WriteLine(line);
                        if (line[0] == '#')
                        {
                            //shapelist.Add(sc);
                            //Console.WriteLine("break");
                            break;
                        }

                        //if (n > 250)
                        //    Console.WriteLine(line);


                        if (line.IndexOf("POLYGON") == 0)
                        {
                            //Console.WriteLine("POLYGON");

                            if (mpfile)
                                sc.shapes.Add(DbGeography.FromText(line));
                            else
                            {
                                DbGeometry dm = DbGeometry.FromText(line);
                                DbGeometry dmc = dm.Centroid;

                                //bool cw1 = clockwise(line, (double)dmc.XCoordinate, (double)dmc.YCoordinate);
                                //if ( !filename.Contains("glwd"))
                                line = reorient_polygon(line);
                                line = close_polygon(line);
                                bool cw2 = clockwise(line, (double)dmc.XCoordinate, (double)dmc.YCoordinate);
                                if (cw2)
                                    clockwiselist.Add(line, dm);
                                else
                                    counterclockwiselist.Add(line, dm);
                                //Console.WriteLine("Clockwise              = " + cw1);
                                //Console.WriteLine("Clockwise (reoriented) = " + cw2);
                                //if (cw1 == cw2)
                                //{
                                //    Console.WriteLine(line);
                                //    Console.ReadLine();
                                //}

                                DbGeography dg = tryfromtext(line);
                                if (dg == null)
                                {
                                    Console.WriteLine("null dg");
                                    break;
                                }
                                double area = -1;
                                
                                if (dg.Area != null)
                                    area = (double)dg.Area;
                                if (cw2)
                                    area = -area;

                                if (area > largestarea)
                                {
                                    largestarea = area;
                                    largest = line;
                                }
                                //Console.WriteLine("Area = " + area/1000000);
                                areasum += area;
                                xwsum += area * (double)dmc.XCoordinate;
                                ywsum += area * (double)dmc.YCoordinate;

                                //sc.shapes.Add(dg);
                            }
                        }
                        else if (line.IndexOf("POINT") == 0)
                        {
                            sc.shapes.Add(DbGeography.FromText(line));
                            sc.mshapes.Add(DbGeometry.FromText(line));
                        }
                        else if (line.IndexOf("MULTIPOLYGON") == 0)
                        {
                            sc.shapes.Add(DbGeography.FromText(line));
                            sc.mshapes.Add(DbGeometry.FromText(line));
                        }
                        else if (line.IndexOf("POLYLINE") == 0)
                        {
                            sc.shapes.Add(DbGeography.FromText(line.Replace("POLYLINE ((","LINESTRING (").Replace("))",")")));
                            sc.mshapes.Add(DbGeometry.FromText(line.Replace("POLYLINE ((", "LINESTRING (").Replace("))", ")")));
                        }
                        else
                        {
                            string[] words = line.Split('\t');
                            if (words.Count() > 1)
                            {
                                if ( !sc.metadict.ContainsKey(words[0]))
                                    sc.metadict.Add(words[0], words[1]);
                                else
                                    Console.WriteLine(line);
                            }
                        }
                    }

                    if (!mpfile)
                    {
                        foreach (string cws in clockwiselist.Keys)
                        {
                            //bool found = false;
                            foreach (string ccws in counterclockwiselist.Keys)
                            {
                                if (clockwiselist[cws].Within(counterclockwiselist[ccws]))
                                {
                                    //found = true;
                                    if (!multipoly.ContainsKey(ccws))
                                    {
                                        List<string> ls = new List<string>();
                                        multipoly.Add(ccws, ls);
                                    }
                                    multipoly[ccws].Add(cws);
                                    break;
                                }
                            }
                        }

                        Console.WriteLine("ccws: " + counterclockwiselist.Count);
                        Console.WriteLine("cws:  " + clockwiselist.Count);
                        Console.WriteLine("multipoly: " + multipoly.Count);
                        //if ( clockwiselist.Count > 0 )
                        //Console.ReadLine();

                        string ms = "";
                        int np = 0;
                        if (counterclockwiselist.Count == 1)
                        {
                            ms = "POLYGON ()";
                        }
                        else if (counterclockwiselist.Count > 1)
                        {
                            ms = "MULTIPOLYGON (())";
                        }
                        else
                            continue;
                        
                        int mpmax = 0;
                        foreach (string ccws in counterclockwiselist.Keys)
                        {
                            string cstrip = strip_polygon(ccws, false); ;
                            if (multipoly.ContainsKey(ccws))
                            {
                                
                                foreach (string cws in multipoly[ccws])
                                {
                                    cstrip += ", " + strip_polygon(cws, false);
                                }
                                if (multipoly[ccws].Count > mpmax)
                                    mpmax = multipoly[ccws].Count;
                            }

                            if (np == 0)
                                ms = ms.Replace("()", "(" + cstrip + ")#"); //# marking where to insert next
                            else
                            {
                                ms = ms.Replace("#)", ", (" + cstrip + ")#)");
                            }
                            np++;
                        }

                        //Console.WriteLine(ms);

                        ms = ms.Replace("#", "");

                        DbGeography dmp = tryfromtext(ms);
                        if (dmp == null)
                        {
                            Console.WriteLine(ms);
                            Console.WriteLine(ms.Substring(0, 50));
                            Console.WriteLine("dmp=null");
                            if (sc.metadict.ContainsKey("lake_name"))
                                Console.WriteLine(sc.metadict["lake_name"]);
                            if (sc.metadict.ContainsKey("name_long"))
                                Console.WriteLine(sc.metadict["name_long"]);

                            //Console.ReadLine();

                            Console.WriteLine("try with just largest piece:");
                            dmp = tryfromtext(largest); //try with just largest piece
                            Console.WriteLine("Largest area = " + largestarea/1000000);
                            Console.WriteLine("Areasum = " + areasum / 1000000);
                            //Console.ReadLine();

                            if (dmp == null)
                            {
                                nfail++;
                                if (nccwsfail > counterclockwiselist.Count)
                                    nccwsfail = counterclockwiselist.Count;
                                if (ncwsfail > clockwiselist.Count)
                                    ncwsfail = clockwiselist.Count;
                                if (mpfail > mpmax)
                                    mpfail = mpmax;
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine(ms.Substring(0, 30));
                            if (mpgood < mpmax)
                                mpgood = mpmax;

                        }


                        sc.shapes.Add(dmp);
                        sc.mshapes.Add(DbGeometry.FromText(ms));


                        double clat = ywsum / areasum;
                        double clon = xwsum / areasum;
                        sc.metadict.Add("Centroid latitude", clat.ToString());
                        sc.metadict.Add("Centroid longitude", clon.ToString());
                        sc.metadict.Add("Areasum", areasum.ToString());
                    }

                    if (sc.metadict.ContainsKey("lake_name"))
                        Console.WriteLine(sc.metadict["lake_name"]);
                    if (sc.metadict.ContainsKey("drainage"))
                        Console.WriteLine(sc.metadict["drainage"]);
                    if (sc.metadict.ContainsKey("river_lake"))
                        Console.WriteLine(sc.metadict["river_lake"]);
                    if (sc.metadict.ContainsKey("name_long"))
                        Console.WriteLine(sc.metadict["name_long"]);
                    shapelist.Add(sc);
                    ngood++;
                    if (nccwsgood < counterclockwiselist.Count)
                        nccwsgood = counterclockwiselist.Count;
                    if (ncwsgood < clockwiselist.Count)
                        ncwsgood = clockwiselist.Count;
                    if (mpgood < multipoly.Count)
                        mpgood = multipoly.Count;

                    //Console.ReadLine();

                    n++;
                }

            }

            Console.WriteLine("Shapes done, n = "+n);
            Console.WriteLine("Shapes good, ngood = " + ngood);
            Console.WriteLine("Shapes bad, nfail = " + nfail);
            Console.WriteLine("nccwsmax good, ngood = " + nccwsgood);
            Console.WriteLine("nccwsmin bad, nfail = " + nccwsfail);
            Console.WriteLine("ncwsmax good, ngood = " + ncwsgood);
            Console.WriteLine("ncwsmin bad, nfail = " + ncwsfail);
            Console.WriteLine("mpmaxmin good, nfail = " + mpgood);
            Console.WriteLine("mpmaxmin bad, nfail = " + mpfail);
            //Console.ReadLine();
            return shapelist;
        }

        public static void write_shapelist(string filename, List<shapeclass> shapelist)
        {
            //        public class shapeclass
            //{
            //    public Dictionary<string, string> metadict = new Dictionary<string, string>();
            //    public List<DbGeography> shapes = new List<DbGeography>();
            //}

            using (StreamWriter sw = new StreamWriter(geonameclass.geonamesfolder + filename))
            {
                foreach (shapeclass sc in shapelist)
                {
                    foreach (string s in sc.metadict.Keys)
                        sw.WriteLine(s + "\t" + sc.metadict[s]);
                    foreach (DbGeography dg in sc.shapes)
                    {
                        sw.WriteLine(dg.AsText());
                    }
                    sw.WriteLine("#");
                }
            }

        }



#endif


    }

}
