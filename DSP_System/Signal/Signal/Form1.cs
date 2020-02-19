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
using System.Numerics;

namespace Signal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Form2 f2 = new Form2();
            f2.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form f3 = new Form3();
            f3.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4_task3 f4 = new Form4_task3();
            f4.Show();

        }

        private void button4_Click(object sender, EventArgs e)
        {

            Form_FFT f5 = new Form_FFT();
            f5.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Form5 obj = new Form5();
            obj.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.Show();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            FilterForm f = new FilterForm();
            f.Show();
        }
    }
}
