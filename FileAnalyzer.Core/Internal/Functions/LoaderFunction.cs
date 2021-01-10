using FileAnalyzer.Core.Internal;
using FileAnalyzer.DataStructures;
using System;
using System.IO;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace FileAnalyzer.Core.Internal
{

	internal class LoaderFunction : AbstractFunction<string, IConnectableObservable<FileMetadata>>, IDisposable
	{
		private ConcurrentBinaryMinHeap<FileMetadata> _minHeap;
		private HeapObservable<FileMetadata> _observable;
		private CancellationTokenSource _cancellationTokenSource;


		public LoaderFunction()
		{
			_minHeap = new ConcurrentBinaryMinHeap<FileMetadata>();
			_observable = _minHeap.ToHeapObservable();
			_cancellationTokenSource = new CancellationTokenSource();
		}

		public override IConnectableObservable<FileMetadata> Execute(string path)
		{
			Task.Run(() => LoadFiles(new DirectoryInfo(path), _cancellationTokenSource.Token));
			return _observable;
		}

		private async Task LoadFiles(DirectoryInfo root, CancellationToken cancellation = default)
		{
			try
			{
				LoadRecursive(root, cancellation);
			}
			catch (Exception) { }
			_observable.Complete();
		}

		private void LoadRecursive(DirectoryInfo dir, CancellationToken cancellation = default)
		{
			LoadDirectoryFiles(dir, cancellation);

			try
			{
				Parallel.ForEach(dir.GetDirectories(), (subDir, state, i) =>
				{
					LoadRecursive(subDir, cancellation);
				});
			}
			catch (UnauthorizedAccessException) { }
		}

		private void LoadDirectoryFiles(DirectoryInfo dir, CancellationToken cancellation = default)
		{
			try
			{
				Parallel.ForEach(dir.GetFiles(), (file, state, j) =>
				{
					cancellation.ThrowIfCancellationRequested();
					var metadata = new FileMetadata() { Id = Guid.NewGuid().ToString(), FullPath = file.FullName, FileSizeInBytes = file.Length };
					_minHeap.Push(new PriorityValuePair<FileMetadata>(file.Length, metadata));
				});
			}
			catch (OperationCanceledException) { }
			catch (UnauthorizedAccessException) { }
		}

		public void Dispose()
		{
			if (!_cancellationTokenSource.IsCancellationRequested)
			{
				_cancellationTokenSource.Cancel();
			}
		}
	}
}
