using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dane
{
    public interface InterfejsDaneKulka : IObservable<InterfejsDaneKulka>
    {
        int Srednica { get; init; }

        float PosX { get; }
        float PosY { get; }

        float SpeedX { get; }//zamiast predkosc speed zeby nam sie nie pomylilo
        float SpeedY { get; }

        Task SetPredkosc(float predkoscX, float predkoscY); // "Jestem predkoscia PART2" ~ Maj 2023, WŚ MR
        Task Przesuwanie(float przesuniecieX, float przesuniecieY);
    }
}
