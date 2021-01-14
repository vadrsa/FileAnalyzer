using FileAnalyzer.Core.Internal;
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace FileAnalyzer.Core
{
    public class Analyzer
    {
        private readonly IReadOnlyDictionary<string, IEnumerable<IFinder>> _finders;

        public Analyzer(IReadOnlyDictionary<string, IEnumerable<IFinder>> finders)
        {
			_finders = finders;

		}

        public IConnectableObservable<IOccurance> Find(string path, Expression expression)
        {
			var loader = new LoaderFunction();
			var classification = new ClassificationFunction(_finders);
			var function = loader
					.Pipe(classification)
					.Branch(
						x => x.Classified,
						x => x.Select(
								(KeyValuePair<string, IObservable<IFile>> item) =>
									new ProcessorFunction(
										expression,
										_finders[item.Key], item.Value),
								(results) => Observable.Merge(results, Scheduler.CurrentThread)
							),
						x => x.ConnectFunc,
						x => x,
						(res1, res2) => (IConnectableObservable<IOccurance>)new ConnectableObservable<IOccurance>(res1, res2)
					);

			return function(path);
		}
    }
}
