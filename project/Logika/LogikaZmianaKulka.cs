using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Logika
{
    public class LogikaZmianaKulka : EventArgs
    {

        public InterfejsKuleczka Kulka {get; init; }

        public LogikaZmianaKulka(InterfejsKuleczka kulka)
        {
            Kulka = kulka;
        }
    }
}
