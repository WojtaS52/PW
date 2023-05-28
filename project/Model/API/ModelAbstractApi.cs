using Logika;
using Logika.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Model.API
{
    public abstract class ModelAbstractApi : IObserver<InterfejsKuleczkaLogika>, IObservable<InterfejsKuleczkaModel>, IDisposable
    {
        public static ModelAbstractApi StworzModelApi(LogikaAbstractApi? logika = default)
        {
            return new ApiModel(logika ?? LogikaAbstractApi.StworzLogikaApi());
        }

        public abstract void Start(int liczba);

        public abstract IDisposable Subscribe(IObserver<InterfejsKuleczkaModel> observer);

        public abstract void OnCompleted();
        public virtual void OnError(Exception error) => throw error;
        public abstract void OnNext(InterfejsKuleczkaLogika value);

        public abstract void Dispose();

    }
}
