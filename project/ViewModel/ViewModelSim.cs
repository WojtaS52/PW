using Model;
using Model.API;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace ViewModel
{
    public class ViewModelSim : ViewModelBase, IObserver<InterfejsKuleczkaModel>
    {
        
        private readonly InterfaceValidator<int> validator;//ok
        private ModelAbstractApi? _model;//ok
        private IDisposable? unsubscriber;

        private int liczbaKulek = 5;
        private bool flag = false;

        public int LiczbaKulek
        {
            get => liczbaKulek;
            set => SetField(ref liczbaKulek, value, validator, 1);
        }
        public bool getSetFlag
        {
            get => flag;
            private set => SetField(ref flag, value);
        }
        public ObservableCollection<InterfejsKuleczkaModel> Kulki { get; } = new();
        public ICommand SimStartCommand { get; init; }
        public ICommand SimStopCommand { get; init; }

        public ViewModelSim(InterfaceValidator<int>? validatorKulek = default)
            : base()
        {
            validator = validatorKulek ?? new ValidatorKulek();

            SimStartCommand = new SimStartCommand(this);
            SimStopCommand = new SimStopCommand(this);
        }

        public void SimStart()
        {
            _model = ModelAbstractApi.StworzModelApi();
            getSetFlag = true;
            Follow(_model);
            _model.Start(liczbaKulek);
        }

        public void SimStop()
        {
            getSetFlag = false;
            Kulki.Clear();
            _model?.Dispose();
        }
        //wymagane przez visual
        public void OnCompleted()
        {
            unsubscriber?.Dispose();
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(InterfejsKuleczkaModel kulka)
        {
            Kulki.Add(kulka);
        }

        public void Follow(IObservable<InterfejsKuleczkaModel> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }
    }
}