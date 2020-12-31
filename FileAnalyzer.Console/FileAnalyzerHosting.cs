using FileAnalyzer.Core;
using FileAnalyzer.Core.Internal;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileAnalyzer.ConsoleApp
{
	public class FileAnalyzerHosting : IHostedService
	{
		private IReadOnlyDictionary<string, IFinder> _finders;
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			Console.WriteLine("Enter root directory path");
			var path = Console.ReadLine();
			var loader = new LoaderFunction();
			var classification = new ClassificationFunction(new Dictionary<string, IFinder>());

			var function = loader.Pipe(classification).Select((KeyValuePair<string, IObservable<FileMetadata>> item) => new ProcessorFunction(_finders[item.Key], item.Value), (results) => Observable.Merge(results));

			function("D:/FileAnalyzerTest");
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
		}
	}
}
