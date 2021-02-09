using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU4_Province_Generator
{
    //Definizione della provincia.
    public class Provincia : IEquatable<Provincia>
    {
        //Attributi di istanza.
        public string ProvNumber { get; private set; }
        public string red;
        public string green;
        public string blue;
        public string desc1;
        public string desc2;

        //Per l'uguaglianza di proprietà
        public bool Equals(Provincia p)
        {
            return this.ProvNumber.Equals(p.ProvNumber);
        }
        public static bool operator ==(Provincia p1, Provincia p2)
        {
            return p1.ProvNumber == p2.ProvNumber;
        }
        public static bool operator ==(string p1, Provincia p2)
        {
            return p1 == p2.ProvNumber;
        }
        public static bool operator !=(string p1, Provincia p2)
        {
            return p1 != p2.ProvNumber;
        }
        public static bool operator !=(Provincia p1, Provincia p2)
        {
            return p1.ProvNumber != p2.ProvNumber;
        }

        //Costruttori.
        public Provincia(string n, string r, string g, string b, string d1, string d2)
        {
            this.ProvNumber = n;
            this.red = r;
            this.green = g;
            this.blue = b;
            this.desc1 = d1;
            this.desc2 = d2;
        }

        //Per lo split da classe.
        public Provincia(string[] array)
        {
            if (array.Length > 3)
            {
                this.ProvNumber = array[0];
                this.red = array[1];
                this.blue = array[2];
                this.green = array[3];
                this.desc1 = "x";
                this.desc2 = "x";
                if (array.Length == 5)
                {
                    this.desc1 = array[4];
                }
                if (array.Length == 6)
                {
                    this.desc1 = array[4];
                    this.desc2 = array[5];
                }
            }
        }


        //Metodi ToString per la scrittura su file.
        public string ToString(bool comma)
        {
            return comma ? $"{ProvNumber};{red};{green};{blue};{desc1};{desc2}": $"{ProvNumber}|{red}|{green}|{blue}|{desc1}|{desc2}";
        }

        public override string ToString()
        {
            return $"{ProvNumber};{red};{green};{blue};{desc1};{desc2}";
        }
    }
}
