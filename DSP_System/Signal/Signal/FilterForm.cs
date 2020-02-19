using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Signal
{
    public partial class FilterForm : Form
    {
        public FilterForm()
        {
            InitializeComponent();
        }
        data d = new data();
        Form6 f = new Form6();
      
        List<double> Window_calculation(double N,double attinuation)//should convert to radian
        {
            List<double> Window = new List<double>();
            double n;
            double w;
            n = (N - 1 )/ 2;
            if (attinuation <= 21)
            {
                for (int i = 0; i < n; i++)
                {
                    Window.Add(1);
                }
            }
            else if (attinuation <= 44)
            {
                for (int i = 0; i < n; i++)
                {
                    w = 0.5 + (0.5 * (Math.Cos((2 * Math.PI * i) / N)) );
                    Window.Add(w);
                }
            }
            else if (attinuation <= 53)
            {
                for (int i = 0; i < n; i++)
                {
                    w = 0.54 + (0.46 * (Math.Cos((2 * Math.PI * i) / N))) ;
                    Window.Add(w);
                }
            }
            else if (attinuation <= 74)
            {
                for (int i = 0; i < n; i++)
                {
                    w = 0.42 + (0.5 * (Math.Cos((2 * Math.PI * i) / (N - 1))) ) + (0.08 * Math.Cos((4 * Math.PI * i) / (N - 1)) );
                    Window.Add(w);
                }
            }
            return Window;
            
        }
        double Calculation_N(double Transition_width, double sampling_freq,double attinuation)
        {
            double N=0;
            if (attinuation<= 21)
            { N = ((sampling_freq * 0.9) / Transition_width); }
            else if (attinuation <= 44)
            { N = ((sampling_freq * 3.1) / Transition_width); }
            else if (attinuation <= 53)
            { N = ((sampling_freq * 3.3) / Transition_width); }
            else if (attinuation <= 74)
            { N = ((sampling_freq * 5.5) / Transition_width); }
            //else
            //{ return 0; }
            return N;
        }
        double Calculation_F(double fc,double Transition_width,double sampling_freq)
        {
            double result = 0;
            result = (fc + (Transition_width / 2)) / sampling_freq;
            return result;
        }
        List<double> Calculation_of_h_of_Lowpass(double fc, double Transition_width, double sampling_freq,double N)
        {
            List<double> H = new List<double>();
            double wc = 0;
            double FC=0;
            double res = 0;
            FC=Calculation_F(fc,Transition_width,sampling_freq);
            wc = 2 * Math.PI * FC;
            double n = 0;
            n = (N - 1) / 2;
            for (int i = 0; i < n; i++)
            {
                if (i == 0)
                {
                    res = 2 * FC;
                }
                else
                {
                    res = 2 * FC * ((Math.Sin(i * wc)) / (i * wc));
                   
                }
                H.Add(res);
            }
            return H;
        }
        List<double> Calculation_of_h_of_HighPass(double fc, double Transition_width, double sampling_freq, double N)
        {
            List<double> H = new List<double>();
            double wc = 0;
            double FC = 0;
            double res = 0;
            FC = Calculation_F(fc, Transition_width, sampling_freq);
            wc = 2 * Math.PI * FC;
            double n = 0;
            n = (N - 1) / 2;
            for (int i = 0; i < n; i++)
            {
                if (i == 0)
                {
                    res = 1-(2 * FC);
                }
                else
                {
                    res = -2 * FC * ((Math.Sin(i * wc)) / (i * wc));
                }
                H.Add(res);
            }
            return H;
        }
        List<double> Calculation_of_h_of_Bandpass(double fc1,double fc2, double Transition_width, double sampling_freq, double N)
        {
            List<double> H = new List<double>();
            double w1 = 0;
            double w2 = 0;
            double F1= 0;
            double F2 = 0;
            double res = 0;
            F1 = (fc1 - (Transition_width / 2)) / sampling_freq;
            F2 = Calculation_F(fc2, Transition_width, sampling_freq);

            w1 = 2 * Math.PI * F1;
            w2 = 2 * Math.PI * F2;
            double n = 0;
            n = (N - 1) / 2;
            for (int i = 0; i < n; i++)
            {
                if (i == 0)
                {
                    res = 2 * (F2-F1);
                }
                else
                {
                    res = (2 * F2 * ((Math.Sin(i * w2)) / (i * w2))) - (2 * F1 * ((Math.Sin(i * w1)) / (i * w1)));
                }
                H.Add(res);
            }
            return H;
        }
        List<double> Calculation_of_h_of_BandStop(double fc1, double fc2, double Transition_width, double sampling_freq, double N)
        {
            List<double> H = new List<double>();
            double w1 = 0;
            double w2 = 0;
            double F1 = 0;
            double F2 = 0;
            double res = 0;
            F1 = (fc1 - (Transition_width / 2)) / sampling_freq;
            F2 = Calculation_F(fc2, Transition_width, sampling_freq);

            w1 = 2 * Math.PI * F1;
            w2 = 2 * Math.PI * F2;
            double n = 0;
            n = (N - 1) / 2;
            for (int i = 0; i < n; i++)
            {
                if (i == 0)
                {
                    res = 1-(2 * (F2 - F1));
                }
                else
                {
                    res = (2 * F1 * ((Math.Sin(i * w1)) / (i * w1))) - (2 * F2 * ((Math.Sin(i * w2)) / (i * w2)));
                }
                H.Add(res);
            }
            return H;
        }
        List<double> Calculation_of_Hn(double attinuation,double Transition_width, double sampling_freq,double fc1,double fc2)
        {
            double N = 0;
            List<double> Windows = new List<double>();
            List<double> H = new List<double>();
            N = Calculation_N(Transition_width, sampling_freq, attinuation);
            Windows = Window_calculation(N, attinuation);
            if (checkBox1.Checked == true)
            {
              H=  Calculation_of_h_of_Lowpass(fc1,Transition_width,sampling_freq,N);
            }
            else if (checkBox2.Checked == true)
            {
                H = Calculation_of_h_of_HighPass(fc1, Transition_width, sampling_freq, N);
            }
            else if (checkBox3.Checked == true)
            {
                H = Calculation_of_h_of_Bandpass(fc1, fc2, Transition_width, sampling_freq, N);
            }
            else if (checkBox4.Checked == true)
            {
                H = Calculation_of_h_of_BandStop(fc1, fc2, Transition_width, sampling_freq, N);
            }
            List<double> FinalHn = new List<double>();
            double res = 0;
            for (int i = 0; i < H.Count; i++)
            {
                res = Windows[i] * H[i];
                FinalHn.Add(res);
            }
            return FinalHn;
        }
        List<double> UPSamplingAndDownSampling(List<double>H,List<double> ECG , int NForUp , int NForDown)
        {
            List<double> AddZeros = new List<double>();
            List<double> Result =new List<double>();
            List<double> FinalResult = new List<double>();
            for (int i = 0; i < NForUp - 1;i++ )
            {
                AddZeros.Add(0);
            }
            for (int i = 0; i < ECG.Count; i++)
            {
                Result.Add(ECG[i]);
                Result.AddRange(AddZeros);
            }
            //convolution
           List<double> Convolution_result = f.convelution_Directe(H,Result);
            for (int i = 0; i <Convolution_result.Count; i = i+NForDown)
            {
                FinalResult.Add(Convolution_result[i]);
            }
            return (FinalResult);
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<double> FinalResult = new List<double>();
            List<double> ecg = new List<double>();
            List<double> hN_after_convultion = new List<double>();
            double fc1 = 0;
            double fc2 = 0;
            double transition_width = 0;
            double Attinuation = 0;
            double Sampling = 0;
            fc1 = double.Parse(textBox4.Text);
            fc2 = double.Parse(textBox5.Text);
            transition_width = double.Parse(textBox1.Text);
            Attinuation = double.Parse(textBox2.Text);
            Sampling = double.Parse(textBox3.Text);
            if (textBox5.Text == "")
            { fc2 = 0; }
            FinalResult = Calculation_of_Hn(Attinuation,transition_width,Sampling,fc1,fc2);//store in file
            string coffetionet = "coffetionet.txt";
            d.write_data_inFile(coffetionet,FinalResult);

            string s = textBox8.Text;
            ecg = d.read_data_file(s);

            //hN_after_convultion = f.convelution_Directe(FinalResult, ecg);//sto in file
            string Coeffaftercon = "coffetionet_After_convolution.txt";
            d.write_data_inFile(Coeffaftercon, hN_after_convultion);
             int Nforup;
            int Nfordown;
            Nforup=int.Parse(textBox7.Text);
            Nfordown=int.Parse(textBox6.Text);
            //SAMPLING
            List<double> SamplingResult = UPSamplingAndDownSampling(FinalResult, ecg, Nforup, Nfordown);//store in file
            string SamplingRes = "SamplingResult.txt";
            d.write_data_inFile(SamplingRes, SamplingResult);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
