using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logika
{
    public static class Kolizje
    {
        public static ISet<(InterfejsKuleczka, InterfejsKuleczka)> Get(IList<InterfejsKuleczka> kulki)
        {
            var kolizje = new HashSet<(InterfejsKuleczka, InterfejsKuleczka)>(kulki.Count);

            foreach (var kulka1 in kulki)
            {
                foreach (var kulka2 in kulki)
                {
                    if(kulka1 == kulka2)
                    {
                        continue;
                    }
                    if (kulka1.CzyWZasiegu(kulka2)){
                        kolizje.Add((kulka1,kulka2));
                    }
                }
            }
            return kolizje;
        }

        public static (Vector2 szybkosc1, Vector2 szybkosc2) ObliczSzybkosc(InterfejsKuleczka kulka1, InterfejsKuleczka kulka2)
        {
            float dystans = Vector2.Dystans(kulka1.Pozycja, kulka2.Pozycja);

            Vector2 normal = new((kulka2.Pozycja.X - kulka1.Pozycja.X) / dystans, (kulka2.Pozycja.Y - kulka1.Pozycja.Y) / dystans);
            Vector2 tg = new(-normal.Y, normal.X);

            if(Vector2.Skalar(kulka2.Szybkosc,normal)<0f)
            {
                return (kulka1.Szybkosc,kulka2.Szybkosc);
            }

            float dpTg1 = kulka1.Szybkosc.X * tg.X + kulka2.Szybkosc.Y * tg.Y;

            float dpTg2 = kulka2.Szybkosc.X * tg.X + kulka2.Szybkosc.Y * tg.Y;

            float dpNormal1 = kulka1.Szybkosc.X * normal.X + kulka2.Szybkosc.Y * normal.Y;

            float dpNormal2 = kulka2.Szybkosc.X * normal.X + kulka2.Szybkosc.Y * normal.Y;

            float moment1 = (dpNormal1 * ((kulka1.Srednica/2) - (kulka2.Srednica/2)) + 2.0f * (kulka2.Srednica/2) * dpNormal2) / ((kulka1.Srednica/2) + (kulka2.Srednica /2));

            float moment2 = (dpNormal2 * ((kulka2.Srednica/2) - (kulka1.Srednica/2)) + 2.0f * (kulka1.Srednica/2) * dpNormal1) / ((kulka1.Srednica/2) + (kulka2.Srednica/2));

            Vector2 nowaPredkosc1 = new(tg.X * dpTg1 + normal.X * moment1, tg.Y * dpTg1 + normal.Y * moment1);

            Vector2 nowaPredkosc2 = new(tg.X * dpTg2 + normal.X * moment2, tg.Y * dpTg2 + normal.Y * moment2);

            return (nowaPredkosc1, nowaPredkosc2);
        }
    }
}

// best regards for AfterTable,
//Wojtas