using Dane.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logika.API
{
    public abstract class LogikaAbstractApi : IObservable<InterfejsKuleczkaLogika>, IDisposable
    {
        public static LogikaAbstractApi StworzLogikaApi(DaneAbstractApi? dane = default)
        {
            return new LogikaApi(dane ?? DaneAbstractApi.StworzDaneApi());
        }
        //wygenerowane przez visuala
        public abstract IDisposable Subscribe(IObserver<InterfejsKuleczkaLogika> observer);

        public abstract void Dispose();

        public abstract IEnumerable<InterfejsKuleczka> StworzKuleczki(int count);
    }
}
