using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Signal
{
    public partial class Form3_2 : Form
    {
        public Form3_2()
        {
            InitializeComponent();
        }


        List<double> y_axis_signal = new List<double>();

        List<double> xq = new List<double>();
        private void button1_Click(object sender, EventArgs e)
        {
                data data_object = new data();
                string s = textBox2.Text;
                int level;
               
            if(checkBox1.Checked == true)
            {
                double bits = double.Parse(textBox3.Text);
                level = int.Parse(Math.Pow(2.0, bits).ToString());

            }
            else
            {
              level = int.Parse(textBox1.Text);
            }

                y_axis_signal = data_object.read_data_file(s);
                double min_x = 0.0;
                double max_x =0.0;
                double resolution = 0.0;
                List<double> x_list = new List<double>();
                List<double> y_list = new List<double>();
                List<double> mid_point_list = new List<double>();

                Tuple<double, double, double> t1= new Tuple<double, double, double>(min_x, max_x, resolution);
                t1 = data_object.Get_min_Max_X_resolution(y_axis_signal,level);
                min_x = t1.Item1;
                max_x = t1.Item2;
                resolution = t1.Item3;

                Tuple<List<double>, List<double>, List<double>> t2 = new Tuple<List<double>, List<double>, List<double>>(x_list, y_list, mid_point_list);
                t2 = data_object.creat_ranges_midPoint(min_x, resolution, level);
                x_list = t2.Item1;
                y_list = t2.Item2;
                mid_point_list = t2.Item3;

                List<int> index = new List<int>();
              

               for (int j = 0; j < y_axis_signal.Count ; j++)
                {
                   for (int l = 0; l < y_list.Count ; l++)
                 {
                   if( (x_list[l] <= y_axis_signal[j]) && (y_axis_signal[j] <= y_list[l]))
                    {
                        index.Add(l);
                        break;
                    }
                 }

               }


               List<string> encode = new List<string>();
               for (int k = 0; k < index.Count; k++)
               {
                   int id = index[k] ;
                   double x = mid_point_list[id];
                   xq.Add(x);
                   string s1 = Convert.ToString(index[k], 2).PadLeft((int)Math.Log(level,2),'0'); ////////////
                   encode.Add(s1);

               }

                dataGridView1.Rows.Add(y_axis_signal.Count);

                for (int i = 0; i < y_axis_signal.Count ; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = i;
                    dataGridView1.Rows[i].Cells[1].Value = y_axis_signal[i];
                    dataGridView1.Rows[i].Cells[2].Value = index[i];
                    dataGridView1.Rows[i].Cells[3].Value = xq[i];
                    dataGridView1.Rows[i].Cells[4].Value = encode[i];
                    dataGridView1.Rows[i].Cells[5].Value = double.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString()) - double.Parse( dataGridView1.Rows[i].Cells[1].Value.ToString()) ;
                   

                }

            double Error = 0.0;
            for(int j = 0 ; j < y_axis_signal.Count ; j++)
            {
               
                Error += double.Parse( dataGridView1.Rows[j].Cells[5].Value.ToString()) * double.Parse(dataGridView1.Rows[j].Cells[5].Value.ToString());
            
            }
 
            double m = (1.0 / y_axis_signal.Count);

            dataGridView1.Rows[y_axis_signal.Count].Cells[5].Value =  m * Error;


        }

        private void CreateGraph(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane2 = zgc.GraphPane;
            myPane2.Title.Text = "My Test Graph\n(For CodeProject Sample)";
            myPane2.XAxis.Title.Text = "My X Axis";
            myPane2.YAxis.Title.Text = "My Y Axis";
            PointPairList list1_merg = new PointPairList();

            for (int i = 0; i < xq.Count; i++)
            {
                list1_merg.Add(i, xq[i]);
            }
            LineItem myCurve = myPane2.AddCurve("Porsche", list1_merg, Color.Orchid, SymbolType.Diamond);
            myCurve.Line.IsVisible = false;
            zgc.AxisChange();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            CreateGraph(zedGraphControl1);
        }




    }
}
