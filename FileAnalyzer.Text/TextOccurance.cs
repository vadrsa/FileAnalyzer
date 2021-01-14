using FileAnalyzer.Core;

namespace FileAnalyzer.Text
{
    public class TextOccurance : IOccurance
    {
        public TextOccurance(IFile file, string pointer, string word)
        {
            File = file;
            Pointer = pointer;
            Word = word;
        }

        public IFile File { get; }
        public string Pointer { get; }
        public string Word { get; }

    }
}
