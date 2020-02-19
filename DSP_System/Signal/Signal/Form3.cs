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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
        List<double> y_axis_2_signal_1 = new List<double>();
        List<double> y_axis_2_signal_2 = new List<double>();
        List<double> y_axis_signal_merge = new List<double>();

        private void button1_Click(object sender, EventArgs e)
        {
                data data_object = new data();
                string s = textBox1.Text;
                string s2 = textBox2.Text;
                y_axis_2_signal_1 = data_object.read_data_file(s);
                y_axis_2_signal_2 = data_object.read_data_file(s2);

                for (int i = 0; i < y_axis_2_signal_1.Count;i++ )
                {
                    y_axis_signal_merge.Add(y_axis_2_signal_1[i] + y_axis_2_signal_2[i]);

                }

        }

        private void CreateGraph(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;

            // Set the Titles
            myPane.Title.Text = "My Test Graph\n(For CodeProject Sample)";
            myPane.XAxis.Title.Text = "My X Axis";
            myPane.YAxis.Title.Text = "My Y Axis";

            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
                for (int i = 0; i < y_axis_2_signal_1.Count; i++)
                {

                    list1.Add(i, y_axis_2_signal_1[i]);

                }
                for (int i = 0; i < y_axis_2_signal_2.Count; i++)
                {
                    list2.Add(i, y_axis_2_signal_2[i]);
                }
                LineItem myCurve = myPane.AddCurve("Porsche", list1, Color.DeepPink, SymbolType.Diamond);
                myCurve.Line.IsVisible = false;
                LineItem myCurve2 = myPane.AddCurve("Piper", list2, Color.DarkOliveGreen, SymbolType.Circle);
                myCurve2.Line.IsVisible = false;

               zgc.AxisChange();
        }

        private void CreateGraph2(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane2 = zgc.GraphPane;
            myPane2.Title.Text = "My Test Graph\n(For CodeProject Sample)";
            myPane2.XAxis.Title.Text = "My X Axis";
            myPane2.YAxis.Title.Text = "My Y Axis";
            PointPairList list1_merg = new PointPairList();
 
                for (int i = 0; i < y_axis_signal_merge.Count; i++)
                {
                    list1_merg.Add(i, y_axis_signal_merge[i]);
                }
                LineItem myCurve = myPane2.AddCurve("Porsche", list1_merg, Color.Red, SymbolType.Diamond);
                myCurve.Line.IsVisible = false;
                zgc.AxisChange();
        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateGraph(zedGraphControl1);
            CreateGraph2(zedGraphControl2);
        }

        private void zedGraphControl2_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3_2 f3_2 = new Form3_2();
            f3_2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string s = textBox3.Text;
            data d = new data();
            d.write_data_inFile(s,y_axis_signal_merge);
        }


    }
}
