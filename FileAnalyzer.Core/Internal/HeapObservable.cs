using FileAnalyzer.DataStructures;
using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace FileAnalyzer.Core.Internal
{
	internal class HeapObservable<T> : IConnectableObservable<T>, IDisposable
	{
		private readonly Subject<T> _subject;
		private readonly ConcurrentBinaryMinHeap<T> _heap;
		private CancellationTokenSource _cancellationToken;
		private bool _completed;

		public HeapObservable(ConcurrentBinaryMinHeap<T> heap)
		{
			_subject = new Subject<T>();
			_heap = heap;
			_cancellationToken = new CancellationTokenSource();
		}

		public IDisposable Connect()
		{
			Task.Run(() => {
				while (true)
				{
					while (!_heap.IsEmpty)
					{
						if (_cancellationToken.IsCancellationRequested)
						{
							break;
						}

						_subject.OnNext(_heap.Pop().Value);
					}

					if (_cancellationToken.IsCancellationRequested || _completed)
					{
						_subject.OnCompleted();

						break;
					}
				}
			}).ConfigureAwait(false);

			return Disposable.Create(() => {
				if (_cancellationToken != null && !_cancellationToken.IsCancellationRequested) {
					_cancellationToken.Cancel();
				}
			});
		}

		public void Complete()
		{
			_completed = true;
		}

		public IDisposable Subscribe(IObserver<T> observer) =>
			_subject.Subscribe(observer);

		public void Dispose()
		{
			_cancellationToken.Cancel();
			_subject?.Dispose();
		}
	}
}
