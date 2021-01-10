using FileAnalyzer.Core;
using FileAnalyzer.DataStructures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileAnalyzer.ConsoleApp
{

	class Program
	{

		static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureHostConfiguration(configHost =>
				{
					configHost.SetBasePath(Directory.GetCurrentDirectory());
					configHost.AddJsonFile("hostsettings.json", optional: true);
					configHost.AddCommandLine(args);
				})
				.ConfigureServices((hostContext, services) =>
				{
					ConfigureServices(services, hostContext.Configuration);
                    services.AddHostedService<FileAnalyzerHosting>();
                    //services.AddHostedService<BugTests>();
				});

		private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
		}




		static async Task UnloadFiles()
		{
			//while (true)
			//{
			//	while (!minHeap.IsEmpty)
			//	{
			//		var file = minHeap.Pop();
			//		Interlocked.Increment(ref fileCount);
			//		Console.WriteLine($"Found file at {file.Value.FullPath} with size {ConvertBytesToMegabytes((long)file.Priority)}Mb");
			//	}

			//	await Task.Delay(200);
			//}
		}
	}
}
