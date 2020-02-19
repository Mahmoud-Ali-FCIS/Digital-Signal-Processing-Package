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

namespace Signal
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<double> signal1 = new List<double>();
            List<double> signal2 = new List<double>();
            List<Complex> signal1_complex = new List<Complex>();
            List<Complex> signal2_complex = new List<Complex>();
            data data_object = new data();
            string s1 = textBox1.Text;
            string s2 = textBox2.Text;
            signal1 = data_object.read_data_file(s1);
            signal2 = data_object.read_data_file(s2);

            string s1_CX = textBox1.Text;
            string s2_CX = textBox2.Text;
            //signal1_complex = data_object.read_data_file_Complex(s1_CX);
            //signal2_complex = data_object.read_data_file_Complex(s2_CX);

            ///////////////////////////////////////////
            #region convelution_Directe " true "
            if (checkBox10.Checked == true)
            {
               List<double> direct_convelution_out = convelution_Directe(signal1, signal2);
            }
            #endregion

            #region convelution_fast " false " 
            if (checkBox9.Checked == true)
            {
                List<double> convelution_fast_out = convelution_fast(signal1_complex, signal2_complex);
            }
            #endregion
            ////////////////////////////////////////////

            #region direct correlation true
            if (checkBox4.Checked == true) // direct correlation
            {
                if(checkBox2.Checked == true) // cross
                {
                    List<double> sl = new List<double>();
                    sl.AddRange(signal1);
                    List<double> sl2 = new List<double>();
                    sl2.AddRange(signal2);

                   List<double> direct_correlation = correlation_Directe(signal1, signal2);
                   double norm = Normalization(sl, sl2, direct_correlation.Count);
                   for (int i = 0; i < direct_correlation.Count; i++)
                   {
                       direct_correlation[i] = direct_correlation[i] / norm;
                   }
                }
                if (checkBox1.Checked == true) // auto
                {
                    List<double> sl = new List<double>();
                    sl.AddRange(signal1);
                    List<double> sl2 = new List<double>();
                    sl2.AddRange(signal1);

                    List<double> direct_correlation1 = correlation_Directe_auto(signal1, signal1);
                    double norm = Normalization(sl, sl2, direct_correlation1.Count);
                    for (int i = 0; i < direct_correlation1.Count; i++ )
                    {
                        direct_correlation1[i] = direct_correlation1[i] / norm;
                    }
                }
            }

            #endregion

            #region fast correlation
            if (checkBox3.Checked == true)  // fast correlation " auto "t" _____ cross "f" "
            {
                List<Complex> sl = new List<Complex>();
                sl.AddRange(signal1_complex);
                List<Complex> sl2 = new List<Complex>();
                sl2.AddRange(signal2_complex);
                List<double> list_correlation_fast = correlation_fast(signal1_complex, signal2_complex);
                double norm = Normalization_Complex(sl, sl2, list_correlation_fast.Count);
                for (int i = 0; i < list_correlation_fast.Count; i++)
                {
                    list_correlation_fast[i] = list_correlation_fast[i] / norm;
                }
            }
            #endregion
   
        }

        public double Normalization_Complex(List<Complex> input_list1, List<Complex> input_list2 , int x)
        {
            double norm_value = 0.0;
            double norm_value2 = 0.0;
            double res = 0.0;
            for (int i = 0; i < input_list1.Count; i++)
            {
                norm_value += input_list1[i].Real * input_list1[i].Real;
            }
            for (int i = 0; i < input_list2.Count; i++)
            {
                norm_value2 += input_list2[i].Real * input_list2[i].Real;
            }
            res = Math.Pow(norm_value * norm_value2, 0.5) / x;
            return res;
        }

        public double Normalization(List<double> input_list1, List<double> input_list2 , int x)
        {
            double norm_value = 0.0;
            double norm_value2 = 0.0;
            double res = 0.0;
            for (int i = 0; i < input_list1.Count; i++)
            {
                norm_value += input_list1[i] * input_list1[i];
            }
            for (int i = 0; i < input_list2.Count; i++)
            {
                norm_value2 += input_list2[i] * input_list2[i];
            }
            res = Math.Pow(norm_value * norm_value2, 0.5) / x;
            return res;
        }
        
        public List<double> convelution_fast(List<Complex> input_list1, List<Complex> input_list2)
        {

            List<Complex> result_ff = new List<Complex>();
            List<Complex> result = new List<Complex>();
            List<double> result1 = new List<double>();
            int num_point = input_list1.Count;


            List<double> l1 = new List<double>();
            List<double> l2 = new List<double>();

            num_point = (input_list1.Count + input_list2.Count) - 1;
  

            int c = input_list1.Count;
            int c2 = input_list2.Count;
            for (int i = 0; i < input_list1.Count; i++)
            {
                l1.Add(input_list1[i].Real);
            }
            for (int i = 0; i < input_list2.Count; i++)
            {
                l2.Add(input_list2[i].Real);
            }

            for (int i = c; i < num_point; i++)
            {
                l1.Add(0.0);
            }

            for (int i = c2; i < num_point; i++)
            {        
                l2.Add(0.0);
            }

         
            List<Tuple<double, double>> l1fft = Fast_forurier_series(l1, l1.Count);
            List<Tuple<double, double>> l2fft = Fast_forurier_series(l2, l1.Count);

            input_list1.Clear();
            input_list2.Clear();

            for (int i = 0; i < l1fft.Count; i++)
            {
                input_list1.Add(new Complex(l1fft[i].Item1, l1fft[i].Item2));
            }
            for (int i = 0; i < l2fft.Count; i++)
            {
                input_list2.Add(new Complex(l2fft[i].Item1, l2fft[i].Item2));
            }
            

      
            for (int i = 0; i < num_point; i++)
            {
                result.Add(input_list1[i] * input_list2[i]);
            }

            result_ff = IFFT(result, result.Count);
            for (int i = 0; i < result_ff.Count; i++)
            {
                result1.Add(result_ff[i].Real / num_point);
            }
            return result1;
        
        
        }

        public List<double> convelution_Directe(List<double> l1 , List<double> l2)
        {
            List<double> result = new List<double>();
            for (int n = 0; n < l1.Count + l2.Count ; n++ )
            {
                double sum = sumation(n, l1, l2);
                result.Add(sum);
            }
            return result;
        }

        public double sumation(int n, List<double> l1, List<double> l2)
        {
               double sum = 0.0;
                for (int k = 0; k < l2.Count + l1.Count; k++)
                {
                    int h = n - k;
                    if (k < l1.Count)
                    {
                        if (h >= 0  && h < l2.Count )
                        {
                            sum += l1[k] * l2[h];
                        }
                        else
                        {
                           continue;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

            return sum;
        }

        //
        public List<double> correlation_Directe(List<double> l1, List<double> l2)
        {
            
            List<double> result = new List<double>();
            List<double> new_l2 = new List<double>();
            int num_point = l2.Count;
            new_l2.AddRange(l2);

            if(l1.Count != l2.Count)
            {
             num_point = (l1.Count + l2.Count)-1;
            int c = l1.Count;
            int c2 = l2.Count;
            for (int i = c; i < num_point ; i++)
            {
                l1.Add(0.0);
            }
            for (int i = c2; i < num_point; i++)
            {
                new_l2.Add(0.0);
            }

            }

            List<List<double>> shifted_list = new List<List<double>>();
            if (checkBox5.Checked == true) // Nonperiodic_correlation
            {
                shifted_list = shifted_Nonperiodic_correlation(num_point, new_l2);
            }

            if (checkBox6.Checked == true) //periodic_correlation
            {
                shifted_list = shifted_periodic_correlation(num_point, new_l2); 
            }

            double sum = 0.0;
            for (int n = 0; n < num_point ; n++ )
            {
                sum = 0.0;
                sum = sum_correlation(num_point, l1, shifted_list[n]);
                sum = sum / num_point;
                result.Add(sum);
            }
            return result;
        }

        public List<double> correlation_Directe_auto(List<double> l1, List<double> l2)
        {
            List<double> result = new List<double>();
            List<double> new_l2 = new List<double>();
            new_l2.AddRange(l2);
        
            List<List<double>> shifted_list = new List<List<double>>();
            if (checkBox5.Checked == true) // Nonperiodic_correlation
            {
                shifted_list = shifted_Nonperiodic_correlation_auto(l1.Count, new_l2);
            }

            if (checkBox6.Checked == true) //periodic_correlation
            {
                shifted_list = shifted_periodic_correlation_auto(l1.Count, new_l2);
            }

            double sum = 0.0;
            for (int n = 0; n < l1.Count; n++)
            {
                sum = 0.0;
                sum = sum_correlation(l1.Count, l1, shifted_list[n]);
                sum = sum / l1.Count;
                result.Add(sum);
            }
            return result;
        }

        public List<List<double>> shifted_periodic_correlation( int n ,List<double> l)
        {
             List<List<double>> res = new List<List<double>>();
             List<double> shifted = new List<double>();
             List<double> l11 = new List<double>();
             l11.AddRange(l);
             List<double> remove  = new List<double>();
             for( int i = 0 ; i < n ; i++)
             {
                 shifted = new List<double>();
                 double r = l11.First();
                 remove.Add(r);
                 l11.Remove(r);
                 shifted.AddRange(l11);
                 l11 = new List<double>();
                 l11.AddRange(shifted);
                 for (int j = 0; j < remove.Count; j++ )
                 {
                     shifted.Add(remove[j]);
                 }
                 res.Add(shifted);                
             }

             res.Insert(0,shifted);   

            return res;
        }

        public List<List<double>> shifted_periodic_correlation_auto(int n, List<double> l)
        {
            List<List<double>> res = new List<List<double>>();
            List<double> shifted = new List<double>();
            List<double> l11 = new List<double>();
            l11.AddRange(l);

            List<double> remove = new List<double>();
            for (int i = 0; i < n; i++)
            {
                shifted = new List<double>();
                double r = l11.First();
                remove.Add(r);
                l11.Remove(r);
                shifted.AddRange(l11);
                l11 = new List<double>();
                l11.AddRange(shifted);
                for (int j = 0; j < remove.Count; j++)
                {
                    shifted.Add(remove[j]);
                }
                res.Add(shifted);
            }

            res.Insert(0, shifted);

            return res;
        }

        public List<List<double>> shifted_Nonperiodic_correlation(int n, List<double> l)
        {
            List<List<double>> res = new List<List<double>>();
            List<double> shifted = new List<double>();
            List<double> l11 = new List<double>();
            l11.AddRange(l);

            List<double> remove = new List<double>();
            for (int i = 0; i < n; i++)
            {
                shifted = new List<double>();
                double r = l11.First();
                remove.Add(r);
                l11.Remove(r);
                shifted.AddRange(l11);
                l11 = new List<double>();
                l11.AddRange(shifted);
                for (int j = 0; j < remove.Count; j++)
                {
                    shifted.Add(0.0);
                }
                res.Add(shifted);
            }

            res.Insert(0, l);

            return res;
        }

        public List<List<double>> shifted_Nonperiodic_correlation_auto(int n, List<double> l)
        {
            List<List<double>> res = new List<List<double>>();
            List<double> shifted = new List<double>();
            List<double> l11 = new List<double>();
            l11.AddRange(l);
            List<double> remove = new List<double>();
            for (int i = 0; i < n; i++)
            {
                shifted = new List<double>();
                double r = l11.First();
                remove.Add(r);
                l11.Remove(r);
                shifted.AddRange(l11);
                l11 = new List<double>();
                l11.AddRange(shifted);
                for (int j = 0; j < remove.Count; j++)
                {
                    shifted.Add(0.0);
                }
                res.Add(shifted);
            }

            res.Insert(0, l);

            return res;
        }

        public double sum_correlation(int n ,List<double> l1 , List<double>l2)
        {
            double sum =0.0;

            for (int i = 0; i < n; i++ )
            {
                sum += l1[i] * l2[i];

            }
        
            return sum;
        }
    
        public List<Complex> IFFT(List<Complex> input_list, int Num_Samples)
        {

            int N = Num_Samples;
            if (Num_Samples == 2)
            {
                List<Complex> result_list = new List<Complex>();
                result_list.Add(input_list[0] + input_list[1]);
                result_list.Add(input_list[0] - input_list[1]);
                return result_list;
            }
            else
            {
                List<Complex> even_list = new List<Complex>();
                List<Complex> odd_list = new List<Complex>();
                List<Complex> out_list = new List<Complex>();
                Complex[] arr = new Complex[Num_Samples];

                for (int i = 0; i < input_list.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        even_list.Add(input_list[i]);
                    }
                    else
                    {
                        odd_list.Add(input_list[i]);
                    }
                }

                List<Complex> FFT1 = new List<Complex>();
                List<Complex> FFT2 = new List<Complex>();

                FFT1 = IFFT(even_list, Num_Samples / 2);
                FFT2 = IFFT(odd_list, Num_Samples / 2);

                for (int k = 0; k < Num_Samples / 2; k++)
                {
                    Complex W = new Complex(Math.Cos((-2 * Math.PI * k) / Num_Samples), -1*Math.Sin((-2 * Math.PI * k) / Num_Samples));
                    arr[k] = (FFT1[k] + W * FFT2[k]);
                    arr[k + Num_Samples / 2] = (FFT1[k] - W * FFT2[k]);
                }

                out_list = arr.ToList();
                return out_list;
            }
        }

        public List<Complex> signal_X_signalconv_Auto(List<Complex> input_list)
        {
            List<Complex> result = new List<Complex>();
            List<double> real= new List<double>();
            List<Complex> imag = new List<Complex>();
            List<Complex> list_conv = new List<Complex>();
            for(int i =0 ;i< input_list.Count; i++)
            {
                imag.Add(new Complex(input_list[i].Real, input_list[i].Imaginary * -1));
                result.Add(imag[i] * input_list[i]);     
            }

            return result;
        }

        public List<Complex> signal_X_signalconv_Cross(List<Complex> input_list1, List<Complex> input_list2)
        {
            List<Complex> result = new List<Complex>();
            List<double> real= new List<double>();
            List<Complex> imag = new List<Complex>();
            List<Complex> list_conv = new List<Complex>();
            for(int i =0 ;i< input_list1.Count; i++)
            {
                imag.Add(new Complex(input_list1[i].Real ,input_list1[i].Imaginary * -1));
                result.Add(imag[i]*input_list2[i]);     
            }
            return result;
        }

        public List<double> correlation_fast(List<Complex> input_list1, List<Complex> input_list2)
        {
            List<Complex> result1 = new List<Complex>();
            List<double> result = new List<double>();
            List<Complex> list_signal_X_signalconv = new List<Complex>();
            int org_size = 0;
            if(checkBox1.Checked == true) // auto
            {
                list_signal_X_signalconv = signal_X_signalconv_Auto(input_list1);
            }
            if (checkBox2.Checked == true) // cross
            {
                List<double> l1 = new List<double>();
                List<double> l2 = new List<double>();
                for (int i = 0; i < input_list1.Count; i++)
                {
                   l1.Add(input_list1[i].Real);
                }
                for (int i = 0; i < input_list2.Count; i++)
                {
                    l2.Add( input_list2[i].Real);
                }
                int num_point = input_list1.Count;
                org_size = num_point;
                if (l1.Count != l2.Count)
                {
                    num_point = (input_list1.Count + input_list2.Count) - 1;
                    org_size = num_point;
                    int value = (int)Math.Pow(2, Math.Round(Math.Log(num_point, 2)));
                    if (value < num_point)
                    {
                        value = (int)Math.Pow(2, Math.Round(Math.Log(num_point, 2)) + 1);
                    }
                    num_point = value;

                    int c = input_list1.Count;
                    int c2 = input_list2.Count;
                    for (int i = c; i < num_point; i++)
                    {
                        l1.Add(0.0);
                    }
                    for (int i = c2; i < num_point; i++)
                    {
                        l2.Add(0.0);
                    }

                }
                List<Tuple<double, double>> l1fft = Fast_forurier_series(l1, l1.Count);
                List<Tuple<double, double>> l2fft = Fast_forurier_series(l2, l1.Count);

                input_list1.Clear();
                input_list2.Clear();

                for (int i = 0; i < l1fft.Count; i++)
                {
                    input_list1.Add(new Complex(l1fft[i].Item1, l1fft[i].Item2));
                }
                for (int i = 0; i < l2fft.Count; i++)
                {
                    input_list2.Add(new Complex(l2fft[i].Item1, l2fft[i].Item2));
                }


                list_signal_X_signalconv = signal_X_signalconv_Cross(input_list1 , input_list2);
            }
            result1 = IFFT(list_signal_X_signalconv, list_signal_X_signalconv.Count);
            for (int i = 0; i < list_signal_X_signalconv.Count; i++)
            {
              result1[i] =  result1[i] / list_signal_X_signalconv.Count;
            }

            if (checkBox2.Checked == true) // cross
            {
                for (int i = 0; i < list_signal_X_signalconv.Count; i++)
                {
                    result.Add(result1[i].Real / org_size);
                }
            }
            else
            {
                for (int i = 0; i < list_signal_X_signalconv.Count; i++)
                {
                    result.Add(result1[i].Real);
                }
            }
            return result;
        }


        public List<Complex> FFT(List<Complex> input_list, int Num_Samples)
        {

            int N = Num_Samples;

            if (Num_Samples == 2)
            {
                List<Complex> result_list = new List<Complex>();
                result_list.Add(input_list[0] + input_list[1]);
                result_list.Add(input_list[0] - input_list[1]);
                return result_list;
            }
            else
            {
                List<Complex> even_list = new List<Complex>();
                List<Complex> odd_list = new List<Complex>();
                List<Complex> out_list = new List<Complex>();
                Complex[] arr = new Complex[Num_Samples];

                for (int i = 0; i < input_list.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        even_list.Add(input_list[i]);
                    }
                    else
                    {
                        odd_list.Add(input_list[i]);
                    }
                }

                List<Complex> FFT1 = new List<Complex>();
                List<Complex> FFT2 = new List<Complex>();


                FFT1 = FFT(even_list, Num_Samples / 2);
                FFT2 = FFT(odd_list, Num_Samples / 2);
                Tuple<Complex, Complex> L;
                Complex W;
                for (int k = 0; k < Num_Samples / 2; k++)
                {
                     W = new Complex(Math.Cos((2 * Math.PI * k) / Num_Samples), Math.Sin((2 * Math.PI * k) / Num_Samples));
                    arr[k] = (FFT1[k] + W * FFT2[k]);
                    arr[k + Num_Samples / 2] = (FFT1[k] - W * FFT2[k]);
                }


                out_list = arr.ToList();



                return out_list;
            }
        }

        public List<Tuple<double, double>> Fast_forurier_series(List<double> l, int Number)
        {

            if (Number == 2)
            {
                Tuple<double, double>[] out_list = new Tuple<double, double>[Number];
                Tuple<double, double> t1 = new Tuple<double, double>((l[0] + l[1]), 0.0);
                Tuple<double, double> t2 = new Tuple<double, double>((l[0] - l[1]), 0.0);

                out_list[0] = t1;
                out_list[1] = t2;
                return out_list.ToList();
            }

            Tuple<double, double>[] out_list_1 = new Tuple<double, double>[Number];
            List<double> odd_list = new List<double>();
            List<double> even_list = new List<double>();

            for (int i = 0; i < l.Count; i++)
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

            int new_N = Number / 2;
            FFt_odd = Fast_forurier_series(odd_list, new_N);
            FFt_even = Fast_forurier_series(even_list, new_N);

            Tuple<double, double> w;
            double real = 0.0, imagin = 0.0;
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

            Tuple<double, double> t = new Tuple<double, double>(real, imagin);
            return t;
        }
        public Tuple<double, double> Butterfly_down(Tuple<double, double> a, Tuple<double, double> b, Tuple<double, double> w)
        {
            double real = a.Item1 - (b.Item1 * w.Item1) - (b.Item2 * w.Item2); // -  -
            double imagin = a.Item2 + (b.Item1 * w.Item2) - (b.Item2 * w.Item1); // +  -

            Tuple<double, double> t = new Tuple<double, double>(real, imagin);
            return t;
        }

        public Tuple<double, double> calculate_W(int k, int count)
        {
            double real = Math.Cos(2 * k * Math.PI / count);
            double imagin = Math.Sin(2 * k * Math.PI / count);

            Tuple<double, double> t = new Tuple<double, double>(real, imagin);
            return t;

        }

    }
}
