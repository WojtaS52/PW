using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;
using Dane.API;
using Dane.Logi;
using Logika.API;
using Dane;


namespace Logika
{
    internal class LogikaApi : LogikaAbstractApi
    {
        private readonly IList<InterfejsKuleczka> _kulki;
        private readonly ISet<IObserver<InterfejsKuleczkaLogika>> _observers;
        private readonly DaneAbstractApi _dane;
        private readonly Plansza _plansza;
        private readonly Random _random = new();
        private readonly InterfejsLogger _logger;
        public LogikaApi(DaneAbstractApi? dane = default,InterfejsLogger? logger = default)
        {
            _dane = dane ?? DaneAbstractApi.StworzDaneApi();
            _observers = new HashSet<IObserver<InterfejsKuleczkaLogika>>();
            _logger = logger ?? new Log();
            _plansza = new Plansza( _dane.SzerokoscPlanszy, _dane.WysokoscPlanszy);
            _kulki = new List<InterfejsKuleczka>();
        }

        public override IEnumerable<InterfejsKuleczka> StworzKuleczki(int count)
        {
           for(var i = 0; i < count; i++)
            {
                int srednica = GetRandomSrednica();
                Vector2 poz = GetRandomPos(srednica);
                Vector2 szybkosc = GetRandomSzybkosc();
                
                var nowaKulka = new Kuleczka(srednica, szybkosc, poz);

                _kulki.Add(nowaKulka);
                SledzKule(new KuleczkaLogika(nowaKulka));
            }
            ThreadManager.SetValidator(ObslugaKolizji);
            ThreadManager.Start();

            return _kulki;
        }
        
        private void ObslugaKolizji()
        {
            foreach(var (kulka1, kulka2) in Kolizje.GetKolizjeKule(_kulki))
            {
                (kulka1.Szybkosc, kulka2.Szybkosc, var zmianaSzybkosci) = Kolizje.ObliczSzybkosc(kulka1, kulka2);
                if (zmianaSzybkosci)
                {
                   _logger.LogInfo($"Wykryto kolizjke kulek !!! 1# {kulka1}; 2# {kulka2}");
                }
                
            }

            foreach(var (kulka, granica, collisionAxis) in Kolizje.GetKolizjePlansza(_kulki, _plansza))
            {
                kulka.Szybkosc = Kolizje.ObliczSpeed(kulka, granica, collisionAxis);
                _logger.LogInfo($"Wykryto kolizje kulki z granicą naszej planszy!!! A o to cwaniara co chciala uciec: {kulka}");
            }
        }
        
        private Vector2 GetRandomPos(int srednica)
        {
            int promien = (srednica / 2);
            int x = _random.Next(promien, _plansza.Szerokosc - promien);
            int y = _random.Next(promien, _plansza.Wysokosc - promien);
            return new Vector2(x, y);
        }

        private Vector2 GetRandomSzybkosc()
        {
            double x = (_random.NextDouble() * 2.0 - 1.0) * _dane.predkosc;
            double y = (_random.NextDouble() * 2.0 - 1.0) * _dane.predkosc;
            return new Vector2((float)x, (float)y);
        }

        private int GetRandomSrednica()
        {
            return _random.Next(_dane.minSrednicaKuli, _dane.maxSrednicaKuli + 1);
        }
        //clasiccly generated...
        public override IDisposable Subscribe(IObserver<InterfejsKuleczkaLogika> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        private void SledzKule(InterfejsKuleczkaLogika kulka)
        {
            foreach(var observer in _observers)
            {
                observer.OnNext(kulka);
            }
        }

        private void KoniecTransmisji()
        {
            foreach(var observer in _observers)
            {
                observer.OnCompleted();
            }
            _observers.Clear();
        }

      

        private class Unsubscriber : IDisposable
        {
            private readonly ISet<IObserver<InterfejsKuleczkaLogika>> _observers;
            private readonly IObserver<InterfejsKuleczkaLogika> _observer;

            public Unsubscriber(ISet<IObserver<InterfejsKuleczkaLogika>> observers, IObserver<InterfejsKuleczkaLogika> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                _observers.Remove(_observer);
            }
        }

        public override void Dispose()
        {
            KoniecTransmisji();
            ThreadManager.Stop();

            Trace.WriteLine($"Srednia delta = {ThreadManager.AverageDeltaTime}");
            _logger.LogInfo($"Srednia delta = {ThreadManager.AverageDeltaTime}");
            Trace.WriteLine($"Srednie Fps'y = {ThreadManager.AverageFps}");
            _logger.LogInfo($"Srednie Fps'y = {ThreadManager.AverageFps}");
            Trace.WriteLine($"Klatki = {ThreadManager.FrameCount}");
            _logger.LogInfo($"Klatki = {ThreadManager.FrameCount}");

            foreach (var kulka in _kulki)
            {
                kulka.Dispose();
            }
            _kulki.Clear();
            _logger.Dispose();
        }
    }
}
