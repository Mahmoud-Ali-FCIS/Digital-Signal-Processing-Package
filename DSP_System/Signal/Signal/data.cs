using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace Signal
{
    class data
    {

        public List<double> read_data_file(string s)
        {
            List<double> y_list = new List<double>();
            FileStream file = new FileStream(s,FileMode.Open);
            StreamReader sr = new StreamReader(file);
            while(sr.Peek() > -1)
            {
                y_list.Add(double.Parse(sr.ReadLine()));
            }
            sr.Close();
            return y_list;
        }

        public double[] ambitude;
        public double[] phase;
        public void read_data_file_pA(string s)
        {
            int count = 0;

            string[] Read_File = System.IO.File.ReadAllLines(s);
            ambitude = new double[Read_File.Length];
            phase = new double[Read_File.Length];
            foreach (string str_line in Read_File)
            {
                string[] store = Read_File[count].Split(',');
                ambitude[count] = double.Parse(store[0]);
                phase[count] = double.Parse(store[1]);
                count++;
            }


        }

        public List<Complex> read_data_file_Complex(string s)
        {
            int count = 0;
              double[] real;
              double[] image;
            string[] Read_File = System.IO.File.ReadAllLines(s);
            real = new double[Read_File.Length];
            image = new double[Read_File.Length];
            foreach (string str_line in Read_File)
            {
                string[] store = Read_File[count].Split(',');
                real[count] = double.Parse(store[0]);
                image[count] = double.Parse(store[1]);
                count++;
            }

            List<Complex> c = new List<Complex>();
            for (int i = 0; i < Read_File.Length; i++ )
            {
                c.Add(new Complex(real[i], image[i]));
            }
            return c;

        }

        public   List<double> real = new List<double>();
        public  List<double> image = new List<double>();

        public void caculate_rael_image(double[] ambi , double[] phase)
        {
            double real_val = 0.0;
            double image_val= 0.0;

            for (int i = 0; i < ambi.Length; i++ )
            {
                real_val = ambi[i] * Math.Cos(phase[i]);
                image_val = ambi[i] * Math.Sin(phase[i]);
                real.Add(real_val);
                image.Add(image_val);
            }
         
        
        }

        public Tuple<double ,double ,double> Get_min_Max_X_resolution(List<double> l1 , int level)
        {
            double min_x = double.MaxValue;
            double max_x = double.MinValue;
            double resolution = 0.0;
            for (int i = 0; i < l1.Count; i++ )
            {
                if(min_x > l1[i])
                {
                    min_x = l1[i];
                }
                if(max_x < l1[i])
                {
                    max_x = l1[i];
                }

            }
            resolution = (max_x - min_x) / level ;

            Tuple<double, double, double> t1 = new Tuple<double, double, double>(min_x, max_x, resolution);
            return t1;
        }

        public Tuple<List<double>,List<double> ,List<double>> creat_ranges_midPoint(double min_x, double resolution, int level)
        {
            double x = min_x;
            double y = min_x + resolution;
            double mid_point = 0.0;
            List<double> x_list = new List<double>();
            List<double> y_list = new List<double>();
            List<double> mid_point_list = new List<double>();
        
            for (int i = 0; i < level; i++ )
            {
                x_list.Add(x);
                y_list.Add(y);
                x = y ;
                y = x + resolution;
            }

            for (int i = 0; i < level; i++)
            {
                mid_point = (x_list[i] + y_list[i]) / 2;
                mid_point_list.Add(mid_point);
            }
            Tuple<List<double>, List<double>, List<double>> t1 = new Tuple<List<double>, List<double>, List<double>>(x_list, y_list, mid_point_list);
            return t1;

        }

        public void write_data_inFile(string s,List<double> l)
        {

            System.IO.StreamWriter file = new System.IO.StreamWriter(s);
            for (int i = 0; i < l.Count; i++ )
            {
                file.WriteLine(l[i]);
            }

            file.Close();
        }

        public void write_data_inFile_2(string s, List<double> l, List<double> l2)
        {

            System.IO.StreamWriter file = new System.IO.StreamWriter(s);
            for (int i = 0; i < l.Count; i++)
            {
                string str = l[i].ToString() + " , " + l2[i].ToString();
                file.WriteLine(str);
            }

            file.Close();
        }

        public void write_data_inFile_3(string s, List<Tuple<double,double>> l)
        {

            System.IO.StreamWriter file = new System.IO.StreamWriter(s);
            for (int i = 0; i < l.Count; i++)
            {
                string str = l[i].Item1.ToString() + " , " + l[i].Item2.ToString();
                file.WriteLine(str);
            }

            file.Close();
        }

    }
}
