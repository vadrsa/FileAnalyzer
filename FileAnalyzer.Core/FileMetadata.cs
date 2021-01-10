namespace FileAnalyzer.Core
{
	public struct FileMetadata
	{

		public string Id { get; set; }
		public string FullPath { get; set; }
		public long FileSizeInBytes { get; set; }
	}
}
