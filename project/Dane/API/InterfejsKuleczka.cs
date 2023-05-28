using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dane.API
{
    public interface InterfejsKuleczka : IObservable<InterfejsKuleczka>, IDisposable
    {
        int Srednica { get;}
        Vector2 Szybkosc { get; set; }
        Vector2 Pozycja { get; }

        void Poruszanie(float s);
        bool CzyWZasiegu(InterfejsKuleczka kulka);  
    }
}
