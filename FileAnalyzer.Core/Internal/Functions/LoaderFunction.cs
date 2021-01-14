using FileAnalyzer.DataStructures;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace FileAnalyzer.Core.Internal
{

    internal class LoaderFunction : AbstractFunction<string, IConnectableObservable<IFile>>, IDisposable
	{
		private ConcurrentBinaryMinHeap<IFile> _minHeap;
		private HeapObservable<IFile> _observable;
		private CancellationTokenSource _cancellationTokenSource;
        private readonly IFileSystem _fileSystem;

        public LoaderFunction()
		{
			_minHeap = new ConcurrentBinaryMinHeap<IFile>();
			_observable = _minHeap.ToHeapObservable();
			_cancellationTokenSource = new CancellationTokenSource();
			_fileSystem = new FileSystem();
		}

		public override IConnectableObservable<IFile> Execute(string path)
		{
			_fileSystem.Directory.SetCurrentDirectory(path);
			Task.Run(() => LoadFiles(_fileSystem.DirectoryInfo.FromDirectoryName(path), _cancellationTokenSource.Token));
			return _observable;
		}

		private async Task LoadFiles(IDirectoryInfo dir, CancellationToken cancellation = default)
		{
			try
			{
				LoadRecursive(dir, cancellation);
			}
			catch (Exception) { }
			_observable.Complete();
		}

		private void LoadRecursive(IDirectoryInfo dir, CancellationToken cancellation = default)
		{
			try
			{
				LoadDirectoryFiles(dir.EnumerateFiles(), cancellation);

				Parallel.ForEach(dir.EnumerateDirectories(), (subDir, state, i) =>
				{
					LoadRecursive(subDir, cancellation);
				});
			}
			catch (UnauthorizedAccessException) { }
		}

		private void LoadDirectoryFiles(IEnumerable<IFileInfo> files, CancellationToken cancellation = default)
		{
			try
			{
				Parallel.ForEach(files, (f, state, j) =>
				{
					cancellation.ThrowIfCancellationRequested();
					var file = new File(f);
					_minHeap.Push(new PriorityValuePair<IFile>(file.FileSizeInBytes, file));
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
