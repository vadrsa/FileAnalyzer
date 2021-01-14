using System.IO;

namespace FileAnalyzer.Core
{
	public interface IFile
	{
		string Id { get; }
		string FullPath { get; }
		long FileSizeInBytes { get; }
		Stream OpenRead();
	}
}
