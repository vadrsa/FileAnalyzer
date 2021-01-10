using Microsoft.Extensions.Hosting;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace FileAnalyzer.ConsoleApp
{
    public class Observable1 : IConnectableObservable<object>
    {
        private Subject<object> _subject = new Subject<object>();


        public IDisposable Connect()
        {
            Task.Run(() =>
            {
                _subject.OnNext("test1");
                _subject.OnNext("test2");
            });
            return Disposable.Empty;
        }

        public IDisposable Subscribe(IObserver<object> observer)
        {
            return _subject.Subscribe(observer);
        }
    }
    public class Observable2 : IObservable<object>
    {
        private Subject<object> _subject = new Subject<object>();

        public Observable2(IObservable<object> input)
        {
            input.Subscribe(OnInput);
        }

        private void OnInput(object obj)
        {
            var obs = new Observable1();
            obs.Subscribe(_subject);
            obs.Connect();
        }

        public IDisposable Subscribe(IObserver<object> observer)
        {
            return _subject.Subscribe(observer);
        }
    }

    public class BugTests : IHostedService
	{
		public async Task StartAsync(CancellationToken cancellationToken)
		{
            var inputPipe1 = new Subject<object>();
            var obs1 = new Observable2(inputPipe1);

            var inputPipe2 = new Subject<object>();
            var obs2 = new Observable2(inputPipe2);

            var merged = Observable.Merge(obs1, obs2);

            merged.Subscribe(o => Console.WriteLine($"OnNext: {o}"));

            await Task.Run(() => {
                inputPipe1.OnNext(1);
                inputPipe1.OnNext(1);
                inputPipe2.OnNext(1);
                inputPipe2.OnNext(1);
                inputPipe2.OnNext(1);
            });
            

        }

		public async Task StopAsync(CancellationToken cancellationToken)
		{
		}
	}
}
