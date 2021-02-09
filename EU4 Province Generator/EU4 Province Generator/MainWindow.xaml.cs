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

        private bool message;
        private List<Provincia> listaProvince;
        private Provincia DefinisciProvincia()
        {
            string n = TxtProvNum.Text;
            string r = TxtRedDef.Text;
            string g = TxtGreenDef.Text;
            string b = TxtBlueDef.Text;
            string d1 = TxtDef1.Text.Trim(' ');
            string d2 = TxtDef2.Text.Trim(' ');
            Provincia provincia = new Provincia(n, r, g, b, d1, d2);
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
            Provincia provincia = DefinisciProvincia();
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
                        if (splitted[0] == provincia)
                        {
                            TxtProvNum.Background = Brushes.LightYellow;
                            BdRGB.Background = Brushes.LightCoral;
                            TxtErrorLine.Text = (z).ToString();
                            BtnAdd.IsEnabled = false;
                            k = false;
                        }
                    }
                    else if ((splitted.Length == 5) || (splitted.Length >= 6))
                    {
                        if ((splitted[0] == provincia) || ((splitted[1] == provincia.red) && (splitted[2] == provincia.green) && (splitted[3] == provincia.blue)))
                        {
                            if (splitted[0] == provincia)
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
                            TxtErrorLine.Text = (z).ToString();
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
                    listaProvince.Clear();
                    while (!leggi.EndOfStream)
                    {
                        string provincia = leggi.ReadLine();
                        listaProvince.Add(new Provincia(provincia.Split(';')));
                        z++;
                    }
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("The file has been moved or doesn't exist anymore.", "NullReferenceException", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                finally
                {
                    leggi.Close();
                    AggiornaTutto();
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
            listaProvince = new List<Provincia>();
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
            LstProv.Items.Clear();
            while (!leggi.EndOfStream)
            {
                string provincia = leggi.ReadLine();
                listaProvince.Add(new Provincia(provincia.Split(';')));
            }
            leggi.Close();
            AggiornaTutto();
            TxtDef1.Text = "x";
            TxtDef2.Text = "x";
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void AggiornaLista()
        {
            LstProv.Items.Clear();
            for (int i = 0; i < listaProvince.Count; i++)
            {
                LstProv.Items.Add($"Line: {i}".PadRight(20) + listaProvince[i].ToString(false));
            }
        }

        private void Riscrivi()
        {
            StreamWriter sc = new StreamWriter(percorso, false, Encoding.Default);
            foreach (Provincia p in listaProvince)
            {
                sc.WriteLine(p);
            }
            sc.Close();
        }

        private void AggiornaTutto()
        {
            Riscrivi();
            AggiornaLista();
        }

        private void LstProv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Provincia p = listaProvince[LstProv.SelectedIndex];
                if(MessageBox.Show($"This province definition record will be deleted permanently:\nNumber: {p.ProvNumber} - Red: {p.red} - Green: {p.green} - Blue: {p.blue} - Name: {p.desc1} - {p.desc2}", "Delete Definition", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    int index = LstProv.SelectedIndex;
                    listaProvince.RemoveAt(index);
                    AggiornaTutto();
                }
            }
        }
    }
}
