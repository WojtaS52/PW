using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dane
{
    public class ZmianaKulka : EventArgs
    {
        public InterfejsDaneKulka Kulka { get; set; }

        public ZmianaKulka(InterfejsDaneKulka kulka)
        {
            this.Kulka = kulka;
        }
    }
}
