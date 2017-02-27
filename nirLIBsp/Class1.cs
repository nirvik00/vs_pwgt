using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Threading;
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Geometry;


namespace nirLIBsp
{
    public class nir_DB_classes
    {
        private const string SERVER = "127.0.0.1";
        private const string DATABASE = "test";
        private const string UID = "root";
        private const string PASSWORD = "Root";
        private static MySqlConnection dbConn;

        public static string insert_string_DB(string inp, string s0, string s1, string s2, string s3, string s4, string s5, string s6, string s7, string s8, string s9, string s10, string s11, string s12)
        {
            if (String.IsNullOrEmpty(inp) && String.IsNullOrEmpty(s0) && String.IsNullOrEmpty(s1) && String.IsNullOrEmpty(s2) && String.IsNullOrEmpty(s3) && String.IsNullOrEmpty(s4) && String.IsNullOrEmpty(s5) && String.IsNullOrEmpty(s6) && String.IsNullOrEmpty(s7) && String.IsNullOrEmpty(s8) && String.IsNullOrEmpty(s9) && String.IsNullOrEmpty(s10) && String.IsNullOrEmpty(s11) && String.IsNullOrEmpty(s12))
            {
                return "none";
            }
            else
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = SERVER;
                builder.UserID = UID;
                builder.Password = PASSWORD;
                builder.Database = DATABASE;
                string connString = builder.ToString();
                builder = null;
                dbConn = new MySqlConnection(connString);
                string query = string.Format("Insert into geometry (polylines, design_seed, site_inset, cell_dim, min_notch_distance, site_coverage, design_seed_randomize_department, design_seed_randomize_adjacency_department, design_seed_program, minimum_allowed_dimension, external_weight_view, travel_distance_weight, circulation_width, program_name) Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')", inp, s0, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12);
                MySqlCommand cmd = new MySqlCommand(query, dbConn);
                dbConn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Close();
                dbConn.Close();
                return s12;
            }
        }
        public static string convertChar(string s)
        {
            string t = s.Replace("|", "=");
            string v = t.Replace(" ", "");
            return v;
        }
        public static List<List<string>> retrieve_string_DB(int key)
        {
            List<List<string>> result_string = new List<List<string>>();
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = SERVER;
            builder.UserID = UID;
            builder.Password = PASSWORD;
            builder.Database = DATABASE;
            string connString = builder.ToString();
            builder = null;
            dbConn = new MySqlConnection(connString);
            string query = string.Format("select * from geometry where sl_no='{0}' ", key);
            MySqlCommand cmd = new MySqlCommand(query, dbConn);
            dbConn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                String str = reader["polylines"].ToString();
                string[] polylineArr = str.Split('&');
                foreach (string polyline in polylineArr)
                {
                    if (!String.IsNullOrEmpty(polyline))
                    {
                        string[] coordinateArr = polyline.Split(';');
                        List<string> coorArr = new List<string>();
                        foreach (string coordinates in coordinateArr)
                        {
                            if (!String.IsNullOrEmpty(coordinates))
                            {
                                coorArr.Add(coordinates);
                            }
                        }
                        result_string.Add(coorArr);
                    }
                }
            }
            reader.Close();
            dbConn.Close();
            return result_string;
        }
        public static int waitAndGenerate(int i, int t, string s)
        {
            var sw = Stopwatch.StartNew();
            Thread.Sleep(t * 1000);
            return i++;
        }
        public static void startConsloeProcess()
        {
            Process.Start("C:/Users/Nirvik Saha/Documents/GEORGIA TECH FILES/Perkins_will/space_planning_new/nirLIBsp/nirLIBsp/bin/Debug/timerApp.exe");
        }
        public static int returnRandomWebInt(int i, int e, string s)
        {
            if (i < e)
            {
                Random r = new Random();
                int lx = r.Next(i, e);
                return lx;

            }
            else if (i == e)
            {
                return i;
            }
            else
            {
                return 1;
            }

        }
        public static double returnRandomWebInt01(int i, string s)
        {
            Random r = new Random();
            double lx = r.NextDouble() * i;
            return lx;
        }
        public static String returnRandomWebSite(string w1, string w2, string w3, string w4, string w5)
        {
            Random r = new Random();
            int i = r.Next(0, 1000);
            if (i < 200)
            {
                return w1;
            }
            else if (i > 200 && i < 400)
            {
                return w2;
            }
            else if (i > 400 && i < 600)
            {
                return w3;
            }
            else if (i > 600 && i < 800)
            {
                return w4;
            }
            else
            {
                return w5;
            }
        }
        public static bool nir_PtInPoly(Polygon poly, Point pt)
        {
            bool t = poly.ContainmentTest(pt);
            return t;
        }

        public static List<Point> nir_PolyPts(Polygon poly)
        {
            List<Point> poly_pt_list = new List<Point>();
            var pts = poly.Points;
            foreach (Point pt in pts)
            {
                poly_pt_list.Add(pt);
            }
            return poly_pt_list;
        }
        //return double distance between points
        public static double nir_Points_distance(Point p0, Point p1)
        {
            double d = 0;
            double x_p0 = p0.X;
            double y_p0 = p0.Y;
            double z_p0 = p0.Z;
            double x_p1 = p1.X;
            double y_p1 = p1.Y;
            double z_p1 = p1.Z;
            d = Math.Sqrt(((x_p0 - x_p1) * (x_p0 - x_p1)) + ((y_p0 - y_p1) * (y_p0 - y_p1)) + ((z_p0 - z_p1) * (z_p0 - z_p1)));
            return d;
        }
        //return all grid points inside a poly
        public static List<Point> nir_poly_grid(Polygon poly, Point min_pt, Point max_pt, int cell_length, int cell_width)
        {
            List<Point> gridPtList = new List<Point>();
            for (int i = (int)min_pt.X; i < (int)max_pt.X; i += cell_length)
            {
                for (int j = (int)min_pt.Y; j < (int)max_pt.Y; j += cell_width)
                {
                    double x = i;
                    double y = j;
                    double z = 0;
                    Point p = Point.ByCoordinates(x, y, z);
                    bool t = poly.ContainmentTest(p);
                    if (t == true)
                    {
                        gridPtList.Add(p);
                    }
                }
            }
            return gridPtList;
        }


    }
}