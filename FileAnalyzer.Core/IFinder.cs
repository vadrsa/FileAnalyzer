using System.IO;
using System.Reactive.Subjects;

namespace FileAnalyzer.Core
{
	public interface IFinder
	{
		IConnectableObservable<IOccurance> Find(string expression, FileMetadata file, FileStream stream);
	}
}
