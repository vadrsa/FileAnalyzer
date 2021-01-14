using System.IO;
using System.Reactive.Subjects;

namespace FileAnalyzer.Core
{
	public interface IFinder
	{
		IConnectableObservable<IOccurance> Find(Expression expression, IFile file);
	}
}
