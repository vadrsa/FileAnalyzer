﻿using FileAnalyzer.Core;
using FileAnalyzer.Core.Internal;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileAnalyzer.ConsoleApp
{
	public class FileAnalyzerHosting : IHostedService
	{
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			Console.WriteLine("Enter root directory path");
			var path = @"C:\Users\davit.asryan\Documents";

			var analyzer = new AnalyzerBuilder()
								.Text(".txt")
								.Text(".cs")
								.Build();

			var obs = analyzer.Find(path, new Expression("testtt David"));

			obs.Subscribe(occ => {
				Console.WriteLine($"Found occurance of word {occ.Word} in {occ.File.FullPath}({occ.File.Id}) at {occ.Pointer}");
			}, (err) => { 
				
			});
			obs.Connect();
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
		}
	}
}
