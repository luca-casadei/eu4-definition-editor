﻿using Microsoft.Win32;
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

namespace eu4_definition_editor_core
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Attributi di istanza della finestra.
        private List<Provincia> ListaProvince;
        private bool message;
        private static string percorso;
       //Costruttore.
        public MainWindow()
        {
            InitializeComponent();
        }

        //Metodo per la definizione di una provincia da window.
        private Provincia DefinisciProvincia()
        {
            int n = int.Parse(TxtProvNum.Text);
            int r = int.Parse(TxtRedDef.Text);
            int g = int.Parse(TxtGreenDef.Text);
            int b = int.Parse(TxtBlueDef.Text);
            string d1 = TxtDef1.Text.Trim(' ');
            string d2 = TxtDef2.Text.Trim(' ');
            Provincia provincia = new Provincia(n, r, g, b, d1, d2);
            return provincia;
        }

        //Verifica della presenza di province doppie.
        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            TxtProvNum.Background = Brushes.White;
            TxtBlueDef.Background = Brushes.White;
            TxtGreenDef.Background = Brushes.White;
            TxtRedDef.Background = Brushes.White;
            BtnAdd.IsEnabled = true;
            //Controllo presenza doppie.
            Provincia provincia = DefinisciProvincia();
            StreamReader leggi = new StreamReader(TxtDefPath.Text, Encoding.Latin1);
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
                        if (int.Parse(splitted[0]) == provincia)
                        {
                            TxtProvNum.Background = Brushes.LightYellow;
                            BdRGB.Background = Brushes.LightCoral;
                            BtnAdd.IsEnabled = false;
                            k = false;
                        }
                    }
                    else if ((splitted.Length == 5) || (splitted.Length >= 6))
                    {
                        if ((int.Parse(splitted[0]) == provincia) || ((int.Parse(splitted[1]) == provincia.Red) && (int.Parse(splitted[2]) == provincia.Green) && (int.Parse(splitted[3]) == provincia.Blue)))
                        {
                            if (int.Parse(splitted[0]) == provincia)
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
                ListaProvince.Clear();
                TxtDefPath.Text = dialog.FileName;
                percorso = TxtDefPath.Text;
                StreamReader leggi = null;
                try
                {
                    leggi = new StreamReader(TxtDefPath.Text, Encoding.Latin1);
                    while ( leggi != null && !leggi.EndOfStream) 
                    {
                        string[] provincia = leggi.ReadLine().Split(';');
                        if (provincia[0] != string.Empty && int.TryParse(provincia[0], out _))
                        {
                            ListaProvince.Add(new Provincia(provincia));
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("The file has been moved or doesn't exist anymore.", "NullReferenceException", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                finally
                {
                    LstProv.Height = ((StackPanel)LstProv.Parent).ActualHeight - 80;
                    leggi.Close();
                    AggiornaTutto(false);
                }
            }
        }

        //Al cambio dei valori delle caselle per l'aggiunta di una provincia.
        private void TxtProv_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnAdd.IsEnabled = false;
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
                RtgColors.Background = new SolidColorBrush(Color.FromRgb((byte)n1, (byte)n2, (byte)n3));
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

        //Metodi per il messaggio di linee incomplete.
        private void ChkLines_Unchecked(object sender, RoutedEventArgs e)
        {
            message = false;
        }

        private void ChkLines_Checked(object sender, RoutedEventArgs e)
        {
            message = true;
        }

        //Creazione della finestra con tutti gli oggetti.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            message = false;
            ListaProvince = new List<Provincia>();
            TxtDef1.Text = "x";
            TxtDef2.Text = "x";
            LstProv.ItemsSource = ListaProvince;
            LstProv.Items.Refresh();
        }

        //Generazione di un colore random.
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

        //Aggiunta di una provincia al file.
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter scrivazza = new StreamWriter(percorso, true, Encoding.Latin1))
            {
                scrivazza.Write($"\n{TxtProvNum.Text};{TxtRedDef.Text};{TxtGreenDef.Text};{TxtBlueDef.Text};{TxtDef1.Text};{TxtDef2.Text}");
            }
            BtnAdd.IsEnabled = false;
            StreamReader leggi = new StreamReader(TxtDefPath.Text, Encoding.Latin1);
            ListaProvince.Clear();
            while (!leggi.EndOfStream)
            {
                string[] provincia = leggi.ReadLine().Split(';') ;
                if (provincia[0] != string.Empty && int.TryParse(provincia[0],out _))
                {
                    ListaProvince.Add(new Provincia(provincia));
                }
            }
            leggi.Close();
            AggiornaTutto(true);
            TxtDef1.Text = "x";
            TxtDef2.Text = "x";
        }

        //Link alla pagina steam per chiarimenti.
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            ProcessStartInfo ps = new ProcessStartInfo(e.Uri.AbsoluteUri);
            ps.UseShellExecute = true;
            Process.Start(ps);
            e.Handled = true;
        }

        //Metodi che aggiornano sia la lista delle province da codice e da finestra.
        private void AggiornaTutto(bool toWrite)
        {
            if (toWrite)
            {
                Riscrivi();
            }
            AggiornaLista();
        }

        private void Riscrivi()
        {
            using (StreamWriter sc = new StreamWriter(percorso, false, Encoding.Latin1))
            {
                foreach (Provincia p in ListaProvince)
                {
                    sc.WriteLine(p);
                }
            }
         }

        private void AggiornaLista()
        {
            LstProv.Items.Refresh();
        }
    }
}
