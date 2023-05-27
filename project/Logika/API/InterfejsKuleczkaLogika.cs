using Dane;
using Dane.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Logika.API
{
    public interface InterfejsKuleczkaLogika : IObservable<InterfejsKuleczkaLogika>, IObserver<InterfejsKuleczka>, INotifyPropertyChanged
    {
        public int Srednica { get; }
        public Vector2 Szybkosc {  get; }
        public Vector2 Pozycja { get; }
    }
}
