using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace FileAnalyzer.Core.Internal
{
	internal class ClassificationFunction : AbstractFunction<IConnectableObservable<FileMetadata>, (IReadOnlyDictionary<string, IObservable<FileMetadata>> Classified, Func<IDisposable> ConnectFunc)>
	{
		public ClassificationFunction(IReadOnlyDictionary<string, IFinder> finders)
		{
			Finders = finders;
		}

		public IReadOnlyDictionary<string, IFinder> Finders { get; }

		public override (IReadOnlyDictionary<string, IObservable<FileMetadata>> Classified, Func<IDisposable> ConnectFunc) Execute(IConnectableObservable<FileMetadata> observable) =>
			(Finders.Keys.ToDictionary(x => x, e => observable.Where(f => Path.GetExtension(f.FullPath).Equals(e, StringComparison.OrdinalIgnoreCase))), observable.Connect);
	}
}
