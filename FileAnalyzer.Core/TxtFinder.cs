using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace FileAnalyzer.Core
{
    public class TxtFinder : IFinder
    {
        public IConnectableObservable<IOccurance> Find(Expression expression, FileMetadata file) =>
            new TxtFinderObservable(expression, file);
    }

    public class TxtFinderObservable : IConnectableObservable<IOccurance>
    {
        private Subject<IOccurance> _subject = new Subject<IOccurance>();
        private FileMetadata _file;
        private Expression _expression;

        public TxtFinderObservable(Expression expression, FileMetadata file)
        {
            _file = file;
            _expression = expression;
        }

        public IDisposable Connect()
        {
            Task.Run(() =>
            {
                foreach(var word in _expression.Words)
                {
                    string text = File.ReadAllText(_file.FullPath);
                    var index = text.IndexOf(word, StringComparison.OrdinalIgnoreCase);

                    while (index != -1)
                    {
                        _subject.OnNext(new Occurance(_file, index.ToString(), word));
                        index = text.IndexOf(word, index + 1, StringComparison.OrdinalIgnoreCase);
                    }
                }
            });
            return Disposable.Empty;
        }

        public IDisposable Subscribe(IObserver<IOccurance> observer)
        {
            return _subject.Subscribe(observer);
        }
    }

    internal class Occurance : IOccurance
    {
        public Occurance(FileMetadata file, string pointer, string word)
        {
            File = file;
            Pointer = pointer;
            Word = word;
        }

        public FileMetadata File { get; }
        public string Pointer { get; }
        public string Word { get; }
    }
}
