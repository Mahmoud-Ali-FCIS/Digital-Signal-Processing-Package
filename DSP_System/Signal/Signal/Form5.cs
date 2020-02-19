using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ZedGraph;

namespace Signal
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }      
        private void button1_Click(object sender, EventArgs e)
        {
           List<string> lines1 = new List<string>();
           List<double> lines_1  = new List<double>();

            using (OpenFileDialog open1 = new OpenFileDialog()
            {
                Filter = "text documents |*.txt",
                ValidateNames = true,
                Multiselect = false
            })
            {
                if (open1.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader s = new StreamReader(open1.FileName))
                    {
                        string line;
                        while ((line = s.ReadLine()) != null)
                        {
                            lines1.Add(line);
                        }

                    }
                }
            }

            for (int i = 0; i < lines1.Count; i++)
            {
                lines_1.Add(double.Parse(lines1[i]));
            }
        }
        List<double> l1 = new List<double>();
        int flag = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            data data_object = new data();
            if(flag == 0)
            {
            string s = textBox2.Text;
            l1 = data_object.read_data_file(s);
            flag = 1;
            }
            List<double> l2 = new List<double>();

            if (checkBox1.Checked == true)
            {
                zedGraphControl1.GraphPane.CurveList.Clear();
                zedGraphControl1.GraphPane.GraphObjList.Clear();
                string s2 = textBox4.Text;
                l2 = data_object.read_data_file(s2);
                List<double> mul_signal = new List<double>();
                mul_signal = multi_signal(l1,l2 );
                l1 = mul_signal;
                CreateGraph2(zedGraphControl1, 0, mul_signal);

            }

            if (checkBox2.Checked == true) // const 
            {
                zedGraphControl1.GraphPane.CurveList.Clear();
                zedGraphControl1.GraphPane.GraphObjList.Clear();
                List<double> mul_signal = new List<double>();
                double constant = double.Parse(textBox1.Text);
                mul_signal = const_signal(l1, constant);
                l1 = mul_signal;
                CreateGraph2(zedGraphControl1, 0, mul_signal);

            }

            if(checkBox3.Checked == true) // foold
            {
                zedGraphControl1.GraphPane.CurveList.Clear();
                zedGraphControl1.GraphPane.GraphObjList.Clear();
                CreateGraph_flood(zedGraphControl1,0, l1);
                for (int i = 0; i < l1.Count; i++)
                {
                   // l1[i] = l1[i] * -1;
                    l1[0] = l1[0] * -1; /////////////////////////////////// check **************************
                }
            }

            if(checkBox4.Checked == true) //shift
            {
                zedGraphControl1.GraphPane.CurveList.Clear();
                zedGraphControl1.GraphPane.GraphObjList.Clear();
                int shifted_value = int.Parse(textBox3.Text);
                shifted_value = shifted_value * -1;
                CreateGraph2(zedGraphControl1, shifted_value, l1);

            }

            if (checkBox5.Checked == true) // shift_foold
            {
                zedGraphControl1.GraphPane.CurveList.Clear();
                zedGraphControl1.GraphPane.GraphObjList.Clear();
                int shifted_value = int.Parse(textBox3.Text);
                CreateGraph_flood(zedGraphControl1, shifted_value, l1);
            
            }

        }
        public List<double> multi_signal(List<double> y1 , List<double> y2)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < y1.Count; i++)
            {
                result.Add(y1[i] * y2[i]);

            }
            return result;
        
        }
        public List<double> const_signal(List<double> y1 , double x)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < y1.Count; i++)
            {
                result.Add(y1[i] * x);

            }
            return result;
        
        
        }
        private void CreateGraph2(ZedGraphControl zgc , int x , List<double> l)
        {
            // get a reference to the GraphPane
            GraphPane myPane2 = new GraphPane();
            myPane2 = zgc.GraphPane;
            myPane2.Title.Text = "My Test Graph\n(For CodeProject Sample)";
            myPane2.XAxis.Title.Text = "My X Axis";
            myPane2.YAxis.Title.Text = "My Y Axis";
            PointPairList list1_merg = new PointPairList();
         
            for (int i = 0; i < l.Count; i++)
            {
                list1_merg.Add(i + x, l[i]);
            }
            LineItem myCurve = myPane2.AddCurve("Porsche", list1_merg, Color.Red, SymbolType.Diamond);
            myCurve.Line.IsVisible = false;
            zgc.AxisChange();
        }
        private void CreateGraph_flood(ZedGraphControl zgc, int x, List<double> l)
        {
            // get a reference to the GraphPane
            GraphPane myPane2 = new GraphPane();
             myPane2=zgc.GraphPane;
            myPane2.Title.Text = "My Test Graph\n(For CodeProject Sample)";
            myPane2.XAxis.Title.Text = "My X Axis";
            myPane2.YAxis.Title.Text = "My Y Axis";
            PointPairList list1_merg = new PointPairList();

            for (int i = 0; i < l.Count; i++)
            {
                list1_merg.Add((i * -1) + x , l[i]);
            }
            LineItem myCurve = myPane2.AddCurve("Porsche", list1_merg, Color.Red, SymbolType.Diamond);
            myCurve.Line.IsVisible = false;
            zgc.AxisChange();
        }

        }
    }

