using FileAnalyzer.Core;
using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace FileAnalyzer.Text
{
    public class TxtFinderObservable : IConnectableObservable<IOccurance>
    {
        private Subject<IOccurance> _subject = new Subject<IOccurance>();
        private IFile _file;
        private Expression _expression;

        public TxtFinderObservable(Expression expression, IFile file)
        {
            _file = file;
            _expression = expression;
        }

        public IDisposable Connect()
        {
            Task.Run(() =>
            {
                foreach (var word in _expression.Words)
                {
                    string text = null;

                    using(var stream = _file.OpenRead())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            text = reader.ReadToEnd();
                        }
                    }

                    var index = text.IndexOf(word, StringComparison.OrdinalIgnoreCase);

                    while (index != -1)
                    {
                        _subject.OnNext(new TextOccurance(_file, index.ToString(), word));
                        index = text.IndexOf(word, index + 1, StringComparison.OrdinalIgnoreCase);
                    }
                }
            });
            return Disposable.Empty;
        }

        public IDisposable Subscribe(IObserver<IOccurance> observer) => _subject.Subscribe(observer);
    }

}
