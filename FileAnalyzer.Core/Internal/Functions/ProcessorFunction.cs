using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace FileAnalyzer.Core.Internal
{
	internal class ProcessorFunction : AbstractFunction<IObservable<IOccurance>>, IObserver<IFile>
	{
		private readonly IEnumerable<IFinder> _finders;
        private readonly Expression _expression;
        private readonly Subject<IOccurance> _subject;

		public ProcessorFunction(Expression expression, IEnumerable<IFinder> finders, IObservable<IFile> observable)
		{
			_expression = expression;
			_subject = new Subject<IOccurance>();
			_finders = finders;
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

		public void OnNext(IFile file)
		{
			var observables = _finders.Select(f => f.Find(_expression, file).AutoConnect());
			Observable.Merge(observables).Subscribe(_subject);
		}
	}
}
