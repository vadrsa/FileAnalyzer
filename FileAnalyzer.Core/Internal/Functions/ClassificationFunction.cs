using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace FileAnalyzer.Core.Internal
{
	public class ClassificationFunction : IFunction<IConnectableObservable<FileMetadata>, IReadOnlyDictionary<string, IObservable<FileMetadata>>>
	{
		public ClassificationFunction(IReadOnlyDictionary<string, IFinder> finders)
		{
			Finders = finders;
		}

		public IReadOnlyDictionary<string, IFinder> Finders { get; }

		public IReadOnlyDictionary<string, IObservable<FileMetadata>> Execute(IConnectableObservable<FileMetadata> observable) =>
			Finders.Keys.ToDictionary(x => x, e => observable.Where(f => Path.GetExtension(f.FullPath) == e));
	}
}
