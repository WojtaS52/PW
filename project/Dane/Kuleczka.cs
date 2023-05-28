using Dane.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dane
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
                    if (_pozycja == value)
                    {
                        return;
                    }
                    _pozycja = value;
                    SledzKulki(this);
                }
            }
        }

        private readonly ISet<IObserver<InterfejsKuleczka>> _observers;

        private IDisposable? _disposer;

        private Vector2 _szybkosc;
        private Vector2 _pozycja;


        public Kuleczka(int srednica, Vector2 szybkosc, Vector2 pozycja)
        {
            Srednica = srednica;
            Szybkosc = szybkosc;
            Pozycja = pozycja;

            _observers = new HashSet<IObserver<InterfejsKuleczka>>();
            _disposer = ThreadManager.Add<float>(Poruszanie);
        }
        //zrobilismy tzw unfollow followa

        //stop na rfactor poruszania
        // REFACTOR PORUSZANIA + follow powyżej;
        public void Poruszanie(float delta)
        {
            if (Szybkosc.CzyZero())
            {
                return;
            }

            // tu jeszcze ew. zerknąć

            float sila = delta.Clamp(0f, 100f) * 0.01f;

            //Pozycja += Szybkosc;// Jestem predkosciom ~ Wojtas 7.4.2023
            Pozycja += Szybkosc * sila;
        }


        // tu dobrze (chyba)
        public bool CzyWZasiegu(InterfejsKuleczka kulka)
        {
            int minDystans = this.Srednica / 2 + kulka.Srednica / 2;    //przypominam ze srednica przez 2 to promień, chyba nie wiem nie umimem matematyki

            float minDystans2 = minDystans * minDystans;
            float aktualnyDystans2 = Vector2.Dystans2(this.Pozycja, kulka.Pozycja);

            return minDystans2 >= aktualnyDystans2; //taki if bo zwrca bool, taki smaczek dla koneserów
        }

        //inaczej visual drze ryja na mnie :<

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
            foreach (var observer in _observers)
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

        public override bool Equals(object? obj)
        {
            return obj is Kuleczka kulka
                && Equals(kulka);
        }

        //wymagane do interfejsu equatable
        public bool Equals(Kuleczka? kuleczka)
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _disposer?.Dispose();
        }
    }
}
