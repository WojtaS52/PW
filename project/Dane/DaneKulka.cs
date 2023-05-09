using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dane
{
    public class DaneKulka : InterfejsDaneKulka
    {
        public int srednica { get; init; }

        public float posX { get; private set; }

        public float posY { get; private set; }

        public float speedX { get; private set; }

        public float speedY { get; private set; }

        private ISet<IObserver<InterfejsDaneKulka>> _observers; //czesciowo wygenerowane - wg visuala to sie tak robi

        public DaneKulka(int srednica, float posX, float posY, float speedX, float speedY)
        {
            this.srednica = srednica;
            this.posX = posX;
            this.posY = posY;
            this.speedX = speedX;
            this.speedY = speedY;

            _observers = new HashSet<IObserver<InterfejsDaneKulka>>();
        }

        public async Task Przesuwanie(float przesuniecieX, float przesuniecieY)
        {
            posX += przesuniecieX;
            posY += przesuniecieY;
            SledzKulki(this);

            await Zapis();
        }
        private async Task Zapis()
        {
            await Task.Delay(1); //czas w milisekundach
        }

        public async Task SetPredkosc(float predkoscX, float predkoscY)
        {
            speedX = predkoscX;
            speedY = predkoscY;
            SledzKulki(this);

            await Zapis();
        }

        public IDisposable Subscribe(IObserver<InterfejsDaneKulka> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        private void SledzKulki(InterfejsDaneKulka kuleczka)
        {   
           foreach (var observer in _observers)
            {
                observer.OnNext(kuleczka);  //provides the observer with new data - karzesz obserwatorowi sledzic kulke, obserwuje np. kiedy jest zmiana jej polozenia
            }
        }

        private class Unsubscriber : IDisposable
        {
            private readonly ISet<IObserver<InterfejsDaneKulka>> _observers;
            private readonly IObserver<InterfejsDaneKulka> _observer;
            public Unsubscriber(ISet<IObserver<InterfejsDaneKulka>> observers, IObserver<InterfejsDaneKulka> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                _observers.Remove(_observer);
            }
        }
    }
}
