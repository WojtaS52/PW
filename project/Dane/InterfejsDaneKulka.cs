using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dane
{
    public interface InterfejsDaneKulka : IObservable<InterfejsDaneKulka>
    {
        int srednica { get; init; }

        float posX { get; }
        float posY { get; }

        float speedX { get; }//zamiast predkosc speed zeby nam sie nie pomylilo
        float speedY { get; }

        Task SetPredkosc(float predkoscX, float predkoscY); // "Jestem predkoscia PART2" ~ Maj 2023, WŚ MR
        Task Przesuwanie(float przesuniecieX, float przesuniecieY);
    }
}
