using Dane;
using Dane.API;
using Logika.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logika
{
    internal class KuleczkaLogika : InterfejsKuleczkaLogika
    {
        private readonly InterfejsKuleczka _kulka;
        public int Srednica => _kulka.Srednica;
        public Vector2 Pozycja=> new(_kulka.Pozycja.X, _kulka.Pozycja.Y);
        public Vector2 Szybkosc => new(_kulka.Szybkosc.X,_kulka.Szybkosc.Y);

        private readonly ISet<IObserver<InterfejsKuleczkaLogika>> _observers;
        private IDisposable? _unsubscriber;

        public KuleczkaLogika(InterfejsKuleczka kulka)
        {
            _observers = new HashSet<IObserver<InterfejsKuleczkaLogika>>();

            _kulka = kulka;
            Follow(_kulka);
        }

        public void Follow(IObservable<InterfejsKuleczka> provider)
        {
            _unsubscriber = provider.Subscribe(this);
        }

        public void OnError(Exception err)
        {
            throw err;
        }

        public void OnCompleted()
        {
            _unsubscriber?.Dispose();
        }

        public void OnNext(InterfejsKuleczka kulka)
        {
            OnPropertyChanged(nameof(Pozycja));
            SledzKulki(this);
            
        }

        public IDisposable Subscribe(IObserver<InterfejsKuleczkaLogika> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        public void SledzKulki(InterfejsKuleczkaLogika kulka)
        {
            if (_observers is null) return;
            foreach (var observer in _observers)
            {
                observer.OnNext(kulka);
            }
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

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
