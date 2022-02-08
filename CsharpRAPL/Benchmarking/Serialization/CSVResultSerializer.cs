using System;
using System.Globalization;
using System.IO;
using CsharpRAPL.CommandLine;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsharpRAPL.Benchmarking.Serialization;

public class CSVResultSerializer : IResultsSerializer {
	public void SerializeResults(IBenchmark benchmark) {
		DateTime dateTime = DateTime.Now;
		string time = $"{dateTime.ToString("s").Replace(":", "-")}-{dateTime.Millisecond}";
		string outputPath = benchmark.BenchmarkInfo.Group != null
			? $"{CsharpRAPLCLI.Options.OutputPath}/{benchmark.BenchmarkInfo.Group}/{benchmark.BenchmarkInfo.Name}"
			: $"{CsharpRAPLCLI.Options.OutputPath}/{benchmark.BenchmarkInfo.Name}";

		Directory.CreateDirectory(outputPath);
		using var writer = new StreamWriter($"{outputPath}/{benchmark.BenchmarkInfo.Name}-{time}.csv");
		using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
			{ Delimiter = ";" });
		csv.WriteRecords(benchmark.GetResults());
	}
}