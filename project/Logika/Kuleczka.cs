using Dane;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logika
    //refaactor kuleczka - wtorek start 
{
    public class Kuleczka : InterfejsKuleczka, IEquatable<Kuleczka>
    {

        private readonly object pozycjaLock = new();
        private readonly object szybkoscLock = new();       //albo predkosc lock

        public int Srednica { get; init; }

        public Vector2 Szybkosc 
        {
            get
            {
                lock (szybkoscLock) 
                {
                    return _szybkosc;
                }
            }
            set
            {
                lock (szybkoscLock)
                {
                    _szybkosc = value;
                }
            }
        }

        public Vector2 Pozycja
        {
            get
            {
                lock (pozycjaLock)
                {
                    return _pozycja;
                }
            }
            private set
            {
                lock (pozycjaLock)
                {
                    if(_pozycja == value)
                    {
                        return;
                    }
                    _pozycja = value;
                    SledzKulki(this);
                }
            }
        }

        private readonly ISet<IObserver<InterfejsKuleczka>> _observers;
        private readonly Plansza _plansza;
        private readonly InterfejsDaneKulka _daneKulka;

        private IDisposable? _disposable;
        private IDisposable? _unsubscriber;

        private Vector2 _szybkosc;
        private Vector2 _pozycja;


        public Kuleczka(int srednica, Vector2 szybkosc, Vector2 pozycja, Plansza plansza, InterfejsDaneKulka? daneKulka = default)
        {
            Srednica = srednica;
            Szybkosc = szybkosc;
            Pozycja = pozycja;

            _plansza = plansza;
            _daneKulka = daneKulka ?? new DaneKulka(Srednica, Pozycja.X, Pozycja.Y, Szybkosc.X, Szybkosc.Y);

            _observers = new HashSet<IObserver<InterfejsKuleczka>>();
            _disposable = ThreadManager.Add<float>(Poruszanie);
            Follow(_daneKulka);
        }

        public void Follow(IObservable<InterfejsDaneKulka> provider)   
        {
            _unsubscriber = provider.Subscribe(this);
        }


        //stop na rfactor poruszania
        // REFACTOR PORUSZANIA + follow powyżej;
        public void Poruszanie(float delta) 
        {
            if (Szybkosc.CzyZero())    
            {
                return;
            }

            float sila = (delta * 0.01f).Clamp(0f, 1f);

            //Pozycja += Szybkosc;// Jestem predkosciom ~ Wojtas 7.4.2023

            var move = Szybkosc * sila;
            var (pozX, pozY) = Pozycja;
            var (nSzybkoscX, nSzybkoscY) = Szybkosc;

            var (GranicaXx, GranicaXy) = _plansza.GranicaX; 

            
            if (!pozX.IsBetween(GranicaXx , GranicaXy , (Srednica/2) ))
            {
                if(pozX <= GranicaXx + (Srednica / 2))
                {
                    nSzybkoscX = MathF.Abs(nSzybkoscX);
                }
                else
                {
                    nSzybkoscX = -MathF.Abs(nSzybkoscX);

                }
            }

            var (GranicaYx, GranicaYy) = _plansza.GranicaY;

            if (!pozX.IsBetween(GranicaYx, GranicaYy, (Srednica / 2)))
            {
                if (pozX <= GranicaYx + (Srednica / 2))
                {
                    nSzybkoscY = MathF.Abs(nSzybkoscY);
                }
                else
                {
                    nSzybkoscY = -MathF.Abs(nSzybkoscY);

                }
            }

            _daneKulka?.SetPredkosc(nSzybkoscX, nSzybkoscY);    //ustawiamy prendkosc wrrrr

            _daneKulka?.Przesuwanie(move.X, move.Y); // tu jeszcze ew. zerknąć
        }


        public bool CzyWZasiegu(InterfejsKuleczka kulka)
        {
            int minDystans = (this.Srednica/2) + (kulka.Srednica /2) ;    //przypominam ze srednica przez 2 to promień, chyba nie wiem nie umimem matematyki


            float minDystans2 = minDystans * minDystans;
            float aktualnyDystans2 = Vector2.Dystans2(this.Pozycja, kulka.Pozycja);

            return minDystans2 >= aktualnyDystans2; //taki if bo zwrca bool, taki smaczek dla koneserów
        }
        //wymagane do interfejsu equatable
        public bool Equals(Kuleczka? other)
        {
            return other is not null
                && Srednica == other.Srednica
                && Pozycja == other.Pozycja
                && Szybkosc == other.Szybkosc;
        }
        //inaczej visual drze ryja na mnie :<
        public void OnCompleted()
        {
            _unsubscriber?.Dispose();
        }
        public void OnError(Exception error) => throw error;

        public void OnNext(InterfejsDaneKulka kulkaD)
        {
            Pozycja = new Vector2(kulkaD.posX, kulkaD.posY);
            Szybkosc = new Vector2(kulkaD.speedX, kulkaD.speedY);
        }

        public IDisposable Subscribe(IObserver<InterfejsKuleczka> observer) 
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        public void SledzKulki(InterfejsKuleczka kulka)
        {
            if (_observers is null)
            {
                return;
            }
            foreach(var observer in _observers)
            {
                observer.OnNext(kulka);
            }
        }

        private class Unsubscriber : IDisposable
        {
            private readonly ISet<IObserver<InterfejsKuleczka>> _observers;
            private readonly IObserver<InterfejsKuleczka> _observer;

            public Unsubscriber(ISet<IObserver<InterfejsKuleczka>> observers, IObserver<InterfejsKuleczka> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                _observers.Remove(_observer);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _disposable?.Dispose(); 
        }

        public override bool Equals(object? obj)
        {
            return obj is Kuleczka kulka
                && EqualsK(kulka);
        }

        public bool EqualsK(Kuleczka? kuleczka)
        {
            return kuleczka is not null
                && Srednica == kuleczka.Srednica
                && Pozycja == kuleczka.Pozycja
                && Szybkosc == kuleczka.Szybkosc;
        }

        //generated
        public override int GetHashCode()
        {
            return HashCode.Combine(Srednica, Pozycja, Szybkosc);
        }
        //generated, but modified
        public override string? ToString()
        {
            return $"Kuleczka Sr={Srednica}, Pos=[{Pozycja.X:n1}, {Pozycja.Y:n1}], S=[{Szybkosc.X:n1}, {Szybkosc.Y:n1}]";
        }
    }

}
