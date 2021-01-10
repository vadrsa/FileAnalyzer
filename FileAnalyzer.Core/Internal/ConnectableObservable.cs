using System;
using System.Reactive.Subjects;

namespace FileAnalyzer.Core.Internal
{
    internal class ConnectableObservable<T> : IConnectableObservable<T>
    {
        private readonly IObservable<T> _observabale;
        private readonly Func<IDisposable> _connector;

        public ConnectableObservable(IObservable<T> observable, Func<IDisposable> connector)
        {
            _observabale = observable;
            _connector = connector;
        }

        public IDisposable Connect() => 
            _connector();

        public IDisposable Subscribe(IObserver<T> observer) => 
            _observabale.Subscribe(observer);
    }
}
