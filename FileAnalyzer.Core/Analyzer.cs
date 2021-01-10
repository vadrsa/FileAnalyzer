using FileAnalyzer.Core.Internal;
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace FileAnalyzer.Core
{
    public class Analyzer
    {
        public IConnectableObservable<IOccurance> Find(string path, Expression expression, IReadOnlyDictionary<string, IFinder> finders)
        {
			var loader = new LoaderFunction();
			var classification = new ClassificationFunction(finders);
			var function = loader
					.Pipe(classification)
					.Branch(
						x => x.Classified,
						x => x.Select(
								(KeyValuePair<string, IObservable<FileMetadata>> item) =>
									new ProcessorFunction(
										expression,
										finders[item.Key], item.Value),
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
