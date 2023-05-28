using Logika.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Model.API
{
    public interface InterfejsKuleczkaModel : IObserver<InterfejsKuleczkaLogika>, INotifyPropertyChanged
    {
        public int Srednica { get; }
        public float SzybkoscX { get; }
        public float SzybkoscY { get; }
        public float PozycjaX{ get; }
        public float PozycjaY { get; }
    }
}
