﻿using Dane.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dane
{
    internal static class Kolizje
    {
        private static readonly int threads = Environment.ProcessorCount;
        private static readonly HashSet<(InterfejsKuleczka, InterfejsKuleczka)> kolizjeKule = new(threads);
        private static readonly List<(InterfejsKuleczka, Vector2, CollisionAxis)> kolizjePlansza = new(threads);

        public static List<(InterfejsKuleczka, Vector2, CollisionAxis)> GetKolizjePlansza(IList<InterfejsKuleczka> kulki, Plansza plansza)
        {
            kolizjePlansza.Clear();

            var (granicaXx, granicaXy) = plansza.GranicaX;
            var (granicaYx, granicaYy) = plansza.GranicaY;


            foreach ( var kulka in kulki)
            {
                var (pozX, pozY) = kulka.Pozycja;
                int promien = kulka.Srednica / 2;

                if(!pozX.IsBetween(granicaXx,granicaXy,promien))
                {
                    kolizjePlansza.Add((kulka, plansza.GranicaX, CollisionAxis.X));
                }
                if (!pozY.IsBetween(granicaYx, granicaYy, promien))
                {
                    kolizjePlansza.Add((kulka, plansza.GranicaY, CollisionAxis.Y));
                }

            }
            return kolizjePlansza;
        }

        public static HashSet<(InterfejsKuleczka, InterfejsKuleczka)> GetKolizjeKule(IList<InterfejsKuleczka> kulki)
        {
            kolizjeKule.Clear();

            foreach (var kulka1 in kulki)
            {
                foreach (var kulka2 in kulki)
                {
                    if (kulka1 == kulka2)
                    {
                        continue;
                    }
                    if (kulka1.CzyWZasiegu(kulka2))
                    {
                        kolizjeKule.Add((kulka1, kulka2));
                    }
                }
            }
            return kolizjeKule;
        }

        public static Vector2 ObliczSpeed(InterfejsKuleczka kulka, Vector2 granica, CollisionAxis collisionAxis) {

            Vector2 pozycja = kulka.Pozycja;
            Vector2 szybkosc = kulka.Szybkosc;
            int promien = kulka.Srednica/2;

            var (newPredkoscX, newPredkoscY) = szybkosc;

            switch (collisionAxis)
            {
                case CollisionAxis.X:
                    if(pozycja.X <= granica.X + promien)
                    {
                        // generated by tab.... 
                        newPredkoscX = MathF.Abs(newPredkoscX);
                    }
                    else
                    {
                        newPredkoscX = -MathF.Abs(newPredkoscX);
                    }
                    break;
                case CollisionAxis.Y:
                    if (pozycja.Y <= granica.X + promien)
                    {
                        // generated by tab.... 
                        newPredkoscY = MathF.Abs(newPredkoscY);
                    }
                    else
                    {
                        newPredkoscY = -MathF.Abs(newPredkoscY);
                    }
                    break;
                default:
                    throw new ArgumentException("Punkt kolizji nie został rozpoaznany poprawnie !!!!1!!", nameof(collisionAxis));
            }
            return new Vector2(newPredkoscX, newPredkoscY);
        }

        public static (Vector2 szybkosc1, Vector2 szybkosc2, bool zmianaSzybkosci) ObliczSzybkosc(InterfejsKuleczka kulka1, InterfejsKuleczka kulka2)
        {
            float promien1 = kulka1.Srednica / 2;
            float promien2 = kulka2.Srednica / 2;
            float dystans = Vector2.Dystans(kulka1.Pozycja, kulka2.Pozycja);//ok

            Vector2 normal = new((kulka2.Pozycja.X - kulka1.Pozycja.X) / dystans, (kulka2.Pozycja.Y - kulka1.Pozycja.Y) / dystans);
            Vector2 tg = new(-normal.Y, normal.X);

            Vector2 kulka1szybkosc = kulka1.Szybkosc;
            Vector2 kulka2szybkosc = kulka2.Szybkosc;


            if(Vector2.Skalar(kulka1.Szybkosc,normal)<0f)
            {
                return (kulka1szybkosc, kulka2szybkosc, false);//ok
            }

            float kulka1Waga = promien1 * promien1;
            float kulka2Waga = promien2 * promien2;


            float dpTg1 = kulka1szybkosc.X * tg.X + kulka1szybkosc.Y * tg.Y;

            float dpTg2 = kulka2szybkosc.X * tg.X + kulka2szybkosc.Y * tg.Y;

            float dpNormal1 = kulka1szybkosc.X * normal.X + kulka1szybkosc.Y * normal.Y;

            float dpNormal2 = kulka2szybkosc.X * normal.X + kulka2szybkosc.Y * normal.Y;

            float moment1 = (dpNormal1 * (kulka1Waga - kulka2Waga) + 2.0f * kulka2Waga * dpNormal2) / (kulka1Waga + kulka2Waga);

            float moment2 = (dpNormal2 * (kulka2Waga - kulka1Waga) + 2.0f * kulka1Waga * dpNormal1) / (kulka2Waga + kulka1Waga);

            Vector2 nowaPredkosc1 = new(tg.X * dpTg1 + normal.X * moment1, tg.Y * dpTg1 + normal.Y * moment1);

            Vector2 nowaPredkosc2 = new(tg.X * dpTg2 + normal.X * moment2, tg.Y * dpTg2 + normal.Y * moment2);

            return (nowaPredkosc1, nowaPredkosc2, true);
        }
    }

    internal enum CollisionAxis
    {
        X,
        Y
    }
}

// best regards for AfterTable,
//Wojtas

