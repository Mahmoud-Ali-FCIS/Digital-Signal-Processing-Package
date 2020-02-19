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
    public partial class Form_FFT : Form
    {
        public Form_FFT()
        {
            InitializeComponent();
        }

      /*  public List<double> Fast_forurier_series(List<double> l , int Number)
        { 
            List<double> out_list = new List<double>();

            if(Number == 2)
            {
                
                out_list.Add(l[0] + l[1]);
                out_list.Add(l[0] - l[1]);
                return out_list;
            }

            List<double> odd_list = new List<double>();
            List<double> even_list = new List<double>();

            for (int i = 0; i < l.Count;i++)
            {
                if (i % 2 == 0)
                {
                    even_list.Add(l[i]);
                }
                else
                { 
                    odd_list.Add(l[i]);
                }

            }

            List<double> FFt1 = new List<double>();
            List<double> FFt2 = new List<double>();

            FFt_odd = Fast_forurier_series(odd_list , Number /2);
            FFt_even = Fast_forurier_series(even_list , Number /2);

            double w;
            Tuple<double, double> result;
            double sum = 0.0;
            double sub = 0.0;
            for (int k = 0; k < Number / 2 - 1; k++)
            {
                
              //  w = 
                //
                result = new Tuple<double, double>(sum,sub);
              //result = Butterfly();
                out_list.Insert(k, result.Item1);
                out_list.Insert((k + Number /2), result.Item2);
            }
            return out_list;
        }*/

        public List<Tuple<double, double>> Fast_forurier_series(List<double> l, int Number)
        {
            
            if(Number == 2)
            {
                Tuple<double, double>[] out_list = new Tuple<double, double>[Number];
                Tuple<double, double> t1 = new Tuple<double, double>((l[0] + l[1]) , 0.0);
                Tuple<double, double> t2 = new Tuple<double, double>((l[0] - l[1]), 0.0);

                out_list[0] = t1;
                out_list[1] = t2;
                return out_list.ToList();
            }

            Tuple<double, double>[] out_list_1 = new Tuple<double, double>[Number];
            List<double> odd_list = new List<double>();
            List<double> even_list = new List<double>();

            for (int i = 0; i < l.Count;i++)
            {
                if (i % 2 == 0)
                {
                    even_list.Add(l[i]);
                }
                else
                { 
                    odd_list.Add(l[i]);
                }

            }

            List<Tuple<double, double>> FFt_odd = new List<Tuple<double, double>>();
            List<Tuple<double, double>> FFt_even = new List<Tuple<double, double>>();

            int new_N = Number /2;
            FFt_odd = Fast_forurier_series(odd_list, new_N);
            FFt_even = Fast_forurier_series(even_list, new_N);

            Tuple<double, double> w ;
            double real = 0.0, imagin =0.0;
            Tuple<double, double> result_sum;
            Tuple<double, double> result_sub;

            for (int k = 0; k <= Number / 2 - 1; k++)
            {

                w = new Tuple<double, double>(real, imagin);
                w = calculate_W(k, Number);
                result_sum = new Tuple<double, double>(real, imagin);
                result_sub = new Tuple<double, double>(real, imagin);

                result_sum = Butterfly_up(FFt_even[k], FFt_odd[k], w);
                result_sub = Butterfly_down(FFt_even[k], FFt_odd[k], w);

                out_list_1[k] = result_sum;
                out_list_1[k + Number / 2] = result_sub;

            }
            return out_list_1.ToList();
        }

        public Tuple<double, double> Butterfly_up(Tuple<double, double> a, Tuple<double, double> b, Tuple<double, double> w)
        {
            double real = a.Item1 + (b.Item1 * w.Item1) + (b.Item2 * w.Item2); // +  +
            double imagin = a.Item2 - (b.Item1 * w.Item2) + (b.Item2 * w.Item1); // -   +

            Tuple<double, double> t = new Tuple<double, double>(real,imagin);
            return t;
        }
        public Tuple<double, double> Butterfly_down(Tuple<double, double> a, Tuple<double, double> b, Tuple<double, double> w)
        {
            double real = a.Item1 - (b.Item1 * w.Item1) - (b.Item2 * w.Item2); // -  -
            double imagin = a.Item2 + (b.Item1 * w.Item2) - (b.Item2 * w.Item1); // +  -

            Tuple<double, double> t = new Tuple<double, double>(real, imagin);
            return t;
        }

        public Tuple<double, double> calculate_W(int k , int count)
        {
            double real = Math.Cos(2 * k * Math.PI / count);
            double imagin = Math.Sin(2 * k * Math.PI / count);

            Tuple<double, double> t = new Tuple<double, double>(real, imagin);
            return t;
        
        }

        List<double> Ampitud = new List<double>();
        List<double> phase = new List<double>();
        List<double> x = new List<double>();

        private void button1_Click(object sender, EventArgs e)
       {
            data d = new data();
            string s = textBox1.Text;
            List<double> signal = new List<double>();
            List<Tuple<double, double>> result = new List<Tuple<double, double>>();
            signal = d.read_data_file(s);
            int n = signal.Count;
            result = Fast_forurier_series(signal, n);
            Ampitud = Get_Ampitude(result);
            phase = Get_Phase(result);
            double freq = double.Parse(textBox2.Text);
            x = Get_X_axis(freq, signal);
            string s1 = textBox3.Text;
            d.write_data_inFile_3(s1, result);

       }

        public List<double> Get_Ampitude(List<Tuple<double, double>>complex)
        {
            List<double> lA = new List<double>();
            double Am = 0.0;
            for (int i = 0; i < complex.Count; i++)
            {
                Am = Math.Sqrt((complex[i].Item1 * complex[i].Item1) + (complex[i].Item2 * complex[i].Item2));
                lA.Add(Am);
            }

            return lA;
        }

        public List<double> Get_Phase(List<Tuple<double, double>> complex)
        {
            List<double> L_phase = new List<double>();
            double ph = 0.0;

            for (int i = 0; i < complex.Count; i++)
            {
                ph = Math.Atan2(complex[i].Item2, complex[i].Item1);
                L_phase.Add(ph);
            }
            return L_phase;

        }

        public List<double> Get_X_axis(double fs, List<double> l_input)
        {
            List<double> l_x = new List<double>();
            double range = 0.0;
            double x = 0.0;
            range = (2 * Math.PI) / (l_input.Count * (1 / fs));

            for (int i = 0; i < l_input.Count; i++)
            {
                x = i * range;
                l_x.Add(x);
            }

            return l_x;
        }

        private void CreateGraph2(ZedGraphControl zgc, List<double> l_y, List<double> l_X)
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

        private void button2_Click(object sender, EventArgs e) // GRAPH
        {
             CreateGraph2(zedGraphControl2 ,phase , x);
             CreateGraph2(zedGraphControl1 , Ampitud , x);
        }



    }
}
