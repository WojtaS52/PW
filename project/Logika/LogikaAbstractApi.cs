using Dane.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logika
{
    public abstract class LogikaAbstractApi : IObservable<InterfejsKuleczka>
    {
        public static LogikaAbstractApi StworzLogikaApi(DaneAbstractApi? dane = default)
        {
            return new LogikaApi(dane ?? DaneAbstractApi.StworzDaneApi());
        }
        //wygenerowane przez visuala
        public abstract IDisposable Subscribe(IObserver<InterfejsKuleczka> observer);

        public abstract void Dispose();

        public abstract IEnumerable<InterfejsKuleczka> StworzKuleczki(int count);
    }
}
