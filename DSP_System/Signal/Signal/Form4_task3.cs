using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using ZedGraph;
using System.Diagnostics;

namespace Signal
{
    public partial class Form4_task3 : Form
    {
        public Form4_task3()
        {
            InitializeComponent();
        }

        List<double> real = new List<double>();
        List<double> imagin = new List<double>();

        public void DFT(List<double> l)
        {
            
            for (int k = 0; k < l.Count; k++ )
            {
               double real_value =0.0;
               double imagin_value = 0.0;
                for (int n = 0; n < l.Count; n++ )
                {

                    real_value += l[n] * Math.Cos(2 * k * n *Math.PI / l.Count);
                    imagin_value += -1 * l[n] * Math.Sin(2 * k * n * Math.PI / l.Count);
                 
                }

                real.Add(real_value);
                imagin.Add(imagin_value);
            }
        
        }

        List<double> result = new List<double>();

        public void inverse_DFT(List<double> real, List<double> image)
        {
           
       
            for (int n = 0; n < image.Count; n++)
            {
                double real_value = 0.0;
                for (int k = 0; k< image.Count; k++)
                {
                    real_value += real[k] * Math.Cos(2 * k * n * Math.PI / image.Count);
                    real_value += -1 * image[k] * Math.Sin(2 * k * n * Math.PI / image.Count);

                 
                }

                result.Add(Math.Round( real_value / image.Count));
            }

        } 

        public List<double> Get_Ampitude(List<double> r , List<double> a)
        {
            List<double> lA = new List<double>();
            double Am =0.0;
            for (int i = 0; i < r.Count; i++ )
            {
                Am = Math.Sqrt((r[i] * r[i]) + (a[i] * a[i]));
                lA.Add(Am);
            }

            return lA;
        }

        public List<double> Get_Phase(List<double> r, List<double> a)
        {
            List<double> L_phase = new List<double>();
            double ph = 0.0;

            for (int i = 0; i < r.Count; i++ )
            {
                ph = Math.Atan2(a[i] , r[i]);
                L_phase.Add(ph);
            }
            return L_phase;
        
        }

        public List<double> Get_X_axis(double fs , List<double> l_input)
        {
            List<double> l_x = new List<double>();
            double range = 0.0;
            double x = 0.0;
            range = (2 * Math.PI) / (l_input.Count * (1 / fs));

            for (int i = 0; i < l_input.Count; i++ )
            {
                x = i * range;
                l_x.Add(x);
            }
               
            return l_x;
        } 

        private void CreateGraph2(ZedGraphControl zgc , List<double> l_y , List<double> l_X)
        {
            // get a reference to the GraphPane
            GraphPane myPane2 = zgc.GraphPane;
            myPane2.Title.Text = "My Test Graph\n(For CodeProject Sample)";
            myPane2.XAxis.Title.Text = "My X Axis";
            myPane2.YAxis.Title.Text = "My Y Axis";
            PointPairList list1_merg = new PointPairList();

            for (int i = 0; i < l_y.Count; i++)
            {
                list1_merg.Add(l_X[i], l_y[i]);
            }
            LineItem myCurve = myPane2.AddCurve("Porsche", list1_merg, Color.Red, SymbolType.Diamond);
            myCurve.Line.IsVisible = false;
            zgc.AxisChange();
        }

        List<double> phase = new List<double>();
        List<double> ampitide = new List<double>();
        List<double> X_axis = new List<double>();
        List<double> signal = new List<double>();

        private void button2_Click(object sender, EventArgs e)
        {
            CreateGraph2(zedGraphControl2, phase, X_axis);
            CreateGraph2(zedGraphControl1, ampitide, X_axis);
        }

        private void button1_Click(object sender, EventArgs e)
        {        
            data data_object = new data();
            string s = textBox1.Text;
            double freq = double.Parse(textBox2.Text);
            signal = data_object.read_data_file(s);
            Stopwatch time = Stopwatch.StartNew();
            DFT(signal);
            time.Stop();
            MessageBox.Show(time.ElapsedMilliseconds.ToString());
            ampitide = Get_Ampitude(real, imagin);
            phase = Get_Phase(real, imagin);
            X_axis = Get_X_axis(freq , signal);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string s = textBox3.Text;
            data d = new data();
            d.write_data_inFile_2(s, ampitide , phase);

        }

        private void button4_Click(object sender, EventArgs e)
        {

            data d = new data();
            string s = textBox4.Text;
            d.read_data_file_pA(s);
            d.caculate_rael_image(d.ambitude, d.phase);
            inverse_DFT(d.real, d.image);
              string s1 = textBox5.Text;
              d.write_data_inFile(s1, result);

        }



    }
}
