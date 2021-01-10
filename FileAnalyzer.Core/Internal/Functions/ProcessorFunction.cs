using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace FileAnalyzer.Core.Internal
{
	internal class ProcessorFunction : AbstractFunction<IObservable<IOccurance>>, IObserver<FileMetadata>
	{
		private readonly IFinder _finder;
        private readonly Expression _expression;
        private readonly Subject<IOccurance> _subject;

		public ProcessorFunction(Expression expression, IFinder finder, IObservable<FileMetadata> observable)
		{
			_expression = expression;
			_subject = new Subject<IOccurance>();
			_finder = finder;
			observable.Subscribe(this);
		}

		public override IObservable<IOccurance> Execute()
		{
			return _subject;
		}

		public void OnCompleted()
		{
			//_subject.OnCompleted();
		}

		public void OnError(Exception error)
		{
			_subject.OnError(error);
		}

		public void OnNext(FileMetadata file)
		{
			var observable = _finder.Find(_expression, file);
			observable.Subscribe(_subject);
			observable.Connect();
		}
	}
}
