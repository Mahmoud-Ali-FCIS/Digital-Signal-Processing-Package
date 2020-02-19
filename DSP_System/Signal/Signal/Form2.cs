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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        List<double> y_axis_1_signal = new List<double>();
        List<double> y_axis_2_signal_1 = new List<double>();
        List<double> y_axis_2_signal_2 = new List<double>();

        private void button1_Click(object sender, EventArgs e)
        {
            data data_object = new data();
            #region 1_signal
            if (checkBox1.Checked == true)
            {
              string s = textBox1.Text;

              y_axis_1_signal = data_object.read_data_file(s);
            }

            #endregion

            #region 2_signal
            if (checkBox2.Checked == true)
            {
                string s = textBox1.Text;
                string s2 = textBox2.Text;
                y_axis_2_signal_1 = data_object.read_data_file(s);
                y_axis_2_signal_2 = data_object.read_data_file(s2);

            }
            #endregion

        }

        private void CreateGraph(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;

            // Set the Titles
            myPane.Title.Text = "My Test Graph\n(For CodeProject Sample)";
            myPane.XAxis.Title.Text = "My X Axis";
            myPane.YAxis.Title.Text = "My Y Axis";

            // Make up some data arrays based on the Sine function
           // double x, y1, y2;
            PointPairList list1_1 = new PointPairList();
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();

            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            if(checkBox1.Checked == true)
            {
                for (int i = 0; i < y_axis_1_signal.Count; i++)
                {
                    list1_1.Add(i, y_axis_1_signal[i]);
                }
            LineItem myCurve = myPane.AddCurve("Porsche",list1_1, Color.Red, SymbolType.Diamond);
            myCurve.Line.IsVisible = false;
            }
            // Generate a blue curve with circle
            // symbols, and "Piper" in the legend
            if (checkBox2.Checked == true)
            {
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
            }
            // Tell ZedGraph to refigure the
            // axes since the data have changed
            zgc.AxisChange();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateGraph(zedGraphControl1);
        }



    }
}
