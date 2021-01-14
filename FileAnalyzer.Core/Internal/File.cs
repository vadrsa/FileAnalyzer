using System;
using System.IO;
using System.IO.Abstractions;

namespace FileAnalyzer.Core.Internal
{
    public class File : IFile
    {
        private readonly IFileInfo _fileInfo;

        public File(IFileInfo fileInfo)
        {
            _fileInfo = fileInfo;
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; }
        public string FullPath => _fileInfo.FullName;
        public long FileSizeInBytes => _fileInfo.Length;

        public Stream OpenRead() => _fileInfo.OpenRead();
    }
}
