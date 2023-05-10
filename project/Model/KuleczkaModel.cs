using Logika;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{

    public class KuleczkaModel : InterfejsKuleczkaModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly InterfejsKuleczka _kulka;

        public KuleczkaModel(InterfejsKuleczka kulka)
        {
            _kulka = kulka;
            Follow(_kulka);
        }
        
        public int Srednica => _kulka.Srednica;
        public Vector2 Pozycja => ObliczaniePrzesunieciaPozycji(_kulka.Pozycja);
        public Vector2 Szybkosc => _kulka.Szybkosc;

        private Vector2 ObliczaniePrzesunieciaPozycji(Vector2 pozycja)
        {
            return new Vector2(pozycja.X - (Srednica/2), pozycja.Y - (Srednica / 2));
        }

        public void Follow(IObservable<InterfejsKuleczka> provider)
        {
            _unsubscriber = provider.Subscribe(this);
        }
        private IDisposable? _unsubscriber;     // added to Follow
        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnCompleted()
        {
            _unsubscriber?.Dispose();
        }

        public void OnNext(InterfejsKuleczka kulka)
        {
            OnPropertyChanged(nameof(Pozycja));
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}