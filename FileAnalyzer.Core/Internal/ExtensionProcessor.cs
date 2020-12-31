using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace FileAnalyzer.Core.Internal
{
	public class ProcessorFunction : IObserver<FileMetadata>, IFunction<IConnectableObservable<IOccurance>>
	{
		private readonly IFinder _finder;

		public ProcessorFunction(IFinder finder, IObservable<FileMetadata> observable)
		{
			_finder = finder;
			observable.Subscribe(this);
		}

		public IConnectableObservable<IOccurance> Execute()
		{
		}

		public void OnCompleted()
		{
		}

		public void OnError(Exception error)
		{
		}

		public void OnNext(FileMetadata file)
		{

		}
	}
}
