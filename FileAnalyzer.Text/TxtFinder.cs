using FileAnalyzer.Text;
using System.Reactive.Subjects;

namespace FileAnalyzer.Core
{
    public class TxtFinder : IFinder
    {
        public IConnectableObservable<IOccurance> Find(Expression expression, IFile file) =>
            new TxtFinderObservable(expression, file);
    }
}
