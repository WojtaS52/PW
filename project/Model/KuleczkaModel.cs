using Logika.API;
using Model.API;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{

    public class KuleczkaModel : InterfejsKuleczkaModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public int Srednica => _kulka.Srednica;
        public float PozycjaX => _kulka.Pozycja.X - (Srednica / 2);
        public float PozycjaY => _kulka.Pozycja.Y - (Srednica / 2);
        public float SzybkoscX => _kulka.Szybkosc.X;       //I am speeeeeeed
        public float SzybkoscY => _kulka.Szybkosc.Y;        //Jak zygzak mqqueen tutaj leceeee

        private readonly InterfejsKuleczkaLogika _kulka;

        private IDisposable? _unsubscriber;// added to Follow


        public KuleczkaModel(InterfejsKuleczkaLogika kulka)
        {
            _kulka = kulka;
            Follow(_kulka);
        }

        public void Follow(IObservable<InterfejsKuleczkaLogika> provider)
        {
            _unsubscriber = provider.Subscribe(this);
        }
        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnCompleted()
        {
            _unsubscriber?.Dispose();
        }

        public void OnNext(InterfejsKuleczkaLogika kulka)
        {
            OnPropertyChanged(nameof(PozycjaX));
            OnPropertyChanged(nameof(PozycjaY));
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}