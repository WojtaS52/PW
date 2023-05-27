using Logika;
using Logika.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal class ApiModel : ModelAbstractApi
    {
        private readonly LogikaAbstractApi _logika;
        private readonly ISet<IObserver<InterfejsKuleczkaModel>> _observers;
        private readonly IDictionary<InterfejsKuleczka, InterfejsKuleczkaModel> _kulkaDoModelu;

        private IDisposable? _unsubscriber;

        public ApiModel(LogikaAbstractApi? logika = default)
        {
            _logika = logika ?? LogikaAbstractApi.StworzLogikaApi();
            _observers = new HashSet<IObserver<InterfejsKuleczkaModel>>();
            _kulkaDoModelu = new Dictionary<InterfejsKuleczka, InterfejsKuleczkaModel>(); 
        }

        public override void Start(int liczbaKulek)
        {
            Follow(_logika);
            _logika.StworzKuleczki(liczbaKulek);
        }

        public override void Stop()
        {
            _logika.Dispose();
        }

        public void Follow(IObservable<InterfejsKuleczka> provider)
        {
            _unsubscriber = provider.Subscribe(this);
        }

        public override void OnCompleted()
        {
            _unsubscriber?.Dispose();
            KoniecTransmisji();
        }

        public override void OnNext(InterfejsKuleczka kulka)
        {
            _kulkaDoModelu.TryGetValue(kulka, out var kuleczkaModel);
            if(kuleczkaModel == null)
            {
                kuleczkaModel = new KuleczkaModel(kulka);
                _kulkaDoModelu.Add(kulka, kuleczkaModel);
            }
            SledzKule(kuleczkaModel);
        }

        public override IDisposable Subscribe(IObserver<InterfejsKuleczkaModel> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        private void SledzKule(InterfejsKuleczkaModel kulka)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(kulka);
            }
        }

        private void KoniecTransmisji()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
            _observers.Clear();
        }

        private class Unsubscriber : IDisposable
        {
            private readonly ISet<IObserver<InterfejsKuleczkaModel>> _observers;
            private readonly IObserver<InterfejsKuleczkaModel> _observer;

            public Unsubscriber(ISet<IObserver<InterfejsKuleczkaModel>> observers, IObserver<InterfejsKuleczkaModel> observer)
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
