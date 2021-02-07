using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EU4_Province_Generator
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string percorso;
        public MainWindow()
        {
            InitializeComponent();
        }

        //Definizioni delle province.
        private struct ProvinceDefinition
        {
            public int provNumber;
            public int red;
            public int green;
            public int blue;
            public string desc1;
            public string desc2;

            //Costruttori.
            public ProvinceDefinition(int n, int r, int g, int b, string d1, string d2)
            {
                this.provNumber = n;
                this.red = r;
                this.green = g;
                this.blue = b;
                this.desc1 = d1;
                this.desc2 = d2;
            }
        }
        private static bool message;
        private ProvinceDefinition DefinisciProvincia()
        {
            int n = int.Parse(TxtProvNum.Text);
            int r = int.Parse(TxtRedDef.Text);
            int g = int.Parse(TxtGreenDef.Text);
            int b = int.Parse(TxtBlueDef.Text);
            string d1 = TxtDef1.Text.Trim(' ');
            string d2 = TxtDef2.Text.Trim(' ');
            ProvinceDefinition provincia = new ProvinceDefinition(n, r, g, b, d1, d2);
            return provincia;
        }

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            TxtProvNum.Background = Brushes.White;
            TxtBlueDef.Background = Brushes.White;
            TxtGreenDef.Background = Brushes.White;
            TxtRedDef.Background = Brushes.White;
            BtnAdd.IsEnabled = true;
            //Controllo presenza doppie.
            ProvinceDefinition provincia = DefinisciProvincia();
            StreamReader leggi = new StreamReader(TxtDefPath.Text, Encoding.Default);
            int z = 0;
            while (!leggi.EndOfStream)
            {
                bool k = true;
                string[] splitted = leggi.ReadLine().Split(';');
                if (z != 0)
                {
                    if (splitted.Length == 1)
                    {
                        {
                            MessageBox.Show($"Missing line - {z} - ", "Missing line!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                    if (splitted.Length == 4)
                    {
                        if (message)
                        {
                            MessageBox.Show($"The program found some unexpected or incomplete values at line - {z} -, the colors of these lines will be ignored.", "Incomplete values.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        if (int.Parse(splitted[0]) == provincia.provNumber)
                        {
                            TxtProvNum.Background = Brushes.LightYellow;
                            BdRGB.Background = Brushes.LightCoral;
                            TxtErrorLine.Text = z.ToString();
                            BtnAdd.IsEnabled = false;
                            k = false;
                        }
                    }
                    else if ((splitted.Length == 5) || (splitted.Length >= 6))
                    {
                        if ((int.Parse(splitted[0]) == provincia.provNumber) || ((int.Parse(splitted[1]) == provincia.red) && (int.Parse(splitted[2]) == provincia.green) && (int.Parse(splitted[3]) == provincia.blue)))
                        {
                            if (int.Parse(splitted[0]) == provincia.provNumber)
                            {
                                TxtProvNum.Background = Brushes.LightYellow;
                            }
                            else
                            {
                                TxtBlueDef.Background = Brushes.LightYellow;
                                TxtGreenDef.Background = Brushes.LightYellow;
                                TxtRedDef.Background = Brushes.LightYellow;
                            }
                            BdRGB.Background = Brushes.LightCoral;
                            TxtErrorLine.Text = z.ToString();
                            BtnAdd.IsEnabled = false;
                            k = false;
                        }
                        else
                        {
                            BdRGB.Background = Brushes.LightGreen;
                        }
                    }
                }
                if (!k)
                {
                    leggi.Close();
                    break;
                }
                z++;
            }
            leggi.Close();
        }

        //Scelta del percorso delle definizioni delle province.
        private void BtnPercorsoDef_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Province definition file|definition.csv",
                DefaultExt = ".csv"
            };
            if ((bool)dialog.ShowDialog())
            {
                LstProv.Items.Clear();
                TxtDefPath.Text = dialog.FileName;
                percorso = TxtDefPath.Text;
                StreamReader leggi = null;
                try
                {
                    leggi = new StreamReader(TxtDefPath.Text, Encoding.Default);
                    int z = 0;
                    while (!leggi.EndOfStream)
                    {
                        z++;
                        LstProv.Items.Add("Line: " + z + " - ".PadRight(20) + leggi.ReadLine().Replace(";", " | "));
                    }
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("The file has been moved or doesn't exhist anymore.", "NullReferenceException", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                finally
                {
                    leggi.Close();
                }
            }
        }

        private void TxtProv_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnAdd.IsEnabled = false;
            TxtErrorLine.Text = string.Empty;
            bool ok1 = int.TryParse(TxtRedDef.Text, out int n1);
            if ((n1 < 0) || (n1 > 255))
            {
                TxtRedDef.Text = "255";
                n1 = 255;
            }
            bool ok2 = int.TryParse(TxtGreenDef.Text, out int n2);
            if ((n2 < 0) || (n2 > 255))
            {
                TxtGreenDef.Text = "255";
                n2 = 255;
            }
            bool ok3 = int.TryParse(TxtBlueDef.Text, out int n3);
            if ((n3 < 0) || (n3 > 255))
            {
                TxtBlueDef.Text = "255";
                n3 = 255;
            }
            bool ok4 = int.TryParse(TxtProvNum.Text, out int n4);
            bool ok5 = !string.IsNullOrWhiteSpace(TxtDefPath.Text);
            if (ok1 && ok2 && ok3)
            {
                RtgColors.Fill = new SolidColorBrush(Color.FromRgb((byte)n1, (byte)n2, (byte)n3));
            }
            if (ok1 && ok2 && ok3 && ok4 && ok5)
            {
                BtnCheck.IsEnabled = true;
            }
            else
            {
                BtnCheck.IsEnabled = false;
            }
        }

        private void ChkLines_Unchecked(object sender, RoutedEventArgs e)
        {
            message = false;
        }

        private void ChkLines_Checked(object sender, RoutedEventArgs e)
        {
            message = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            message = false;
            TxtDef1.Text = "x";
            TxtDef2.Text = "x";
        }

        private void BtnRandom_Click(object sender, RoutedEventArgs e)
        {
            BdRGB.Background = Brushes.AliceBlue;
            Random numCas = new Random();
            TxtBlueDef.Text = numCas.Next(0, 256).ToString();
            TxtGreenDef.Text = numCas.Next(0, 256).ToString();
            TxtRedDef.Text = numCas.Next(0, 256).ToString();
            TxtProvNum.Background = Brushes.White;
            TxtBlueDef.Background = Brushes.White;
            TxtGreenDef.Background = Brushes.White;
            TxtRedDef.Background = Brushes.White;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter scrivazza = new StreamWriter(percorso, true, Encoding.Default);
            scrivazza.Write($"\n{TxtProvNum.Text};{TxtRedDef.Text};{TxtGreenDef.Text};{TxtBlueDef.Text};{TxtDef1.Text};{TxtDef2.Text}");
            scrivazza.Close();
            BtnAdd.IsEnabled = false;
            StreamReader leggi = new StreamReader(TxtDefPath.Text, Encoding.Default);
            int z = 0;
            LstProv.Items.Clear();
            while (!leggi.EndOfStream)
            {
                LstProv.Items.Add("Line: " + z + " - ".PadRight(20) + leggi.ReadLine().Replace(";", " | "));
                z++;
            }
            leggi.Close();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
