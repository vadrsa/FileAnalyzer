using System;
using System.Collections.Generic;
using System.Text;

namespace FileAnalyzer.Core
{
	public interface IOccurance
	{
		FileMetadata File { get; }
		string Pointer { get; }
		string Word { get; }
	}
}
