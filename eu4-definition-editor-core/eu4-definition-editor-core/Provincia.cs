using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eu4_definition_editor_core
{
    //Definizione della provincia.
    public class Provincia : IEquatable<Provincia>
    {
        //Attributi di istanza.
        public int ProvNumber { get; private set; }
        public int Red { get; private set; }
        public int Green { get; private set; }
        public int Blue { get; private set; }
        public string Desc1 { get; private set; }
        public string Desc2 { get; private set; }

        //Per l'uguaglianza di proprietà
        public bool Equals(Provincia p)
        {
            return this.ProvNumber.Equals(p.ProvNumber);
        }
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(Provincia p1, Provincia p2)
        {
            return p1.ProvNumber == p2.ProvNumber;
        }
        public static bool operator ==(int p1, Provincia p2)
        {
            return p1 == p2.ProvNumber;
        }
        public static bool operator !=(int p1, Provincia p2)
        {
            return p1 != p2.ProvNumber;
        }
        public static bool operator !=(Provincia p1, Provincia p2)
        {
            return p1.ProvNumber != p2.ProvNumber;
        }

        //Costruttori.
        public Provincia(int n, int r, int g, int b, string d1, string d2)
        {
            this.ProvNumber = n;
            this.Red = r;
            this.Green = g;
            this.Blue = b;
            this.Desc1 = d1;
            this.Desc2 = d2;
        }

        //Per lo split da classe.
        public Provincia(string[] array)
        {
            if (array.Length > 3)
            {
                this.ProvNumber = int.Parse(array[0]);
                this.Red = int.Parse(array[1]);
                this.Green = int.Parse(array[2]);
                this.Blue = int.Parse(array[3]);
                this.Desc1 = "x";
                this.Desc2 = "x";
                if (array.Length == 5)
                {
                    this.Desc1 = array[4];
                }
                if (array.Length == 6)
                {
                    this.Desc1 = array[4];
                    this.Desc2 = array[5];
                }
            }
        }


        //Metodi ToString per la scrittura su file.
        public string ToString(bool comma)
        {
            return comma ? $"{ProvNumber} ; {Red} ; {Green} ; {Blue} ; {Desc1} ; {Desc2}": $"{ProvNumber} | {Red} | {Green} | {Blue} | {Desc1} | {Desc2}";
        }

        public override string ToString()
        {
            return $"{ProvNumber};{Red};{Green};{Blue};{Desc1};{Desc2}";
        }
    }
}
