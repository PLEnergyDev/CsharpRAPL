using System;
using System.IO;
using System.Text.Json;
using CsharpRAPL.CommandLine;

namespace CsharpRAPL.Benchmarking.Serialization;

public class JsonResultSerializer : IResultsSerializer {
	public void SerializeResults(IBenchmark benchmark) {
		DateTime dateTime = DateTime.Now;
		string time = $"{dateTime.ToString("s").Replace(":", "-")}-{dateTime.Millisecond}";
		string outputPath = benchmark.BenchmarkInfo.Group != null
			? $"{CsharpRAPLCLI.Options.OutputPath}/{benchmark.BenchmarkInfo.Group}/{benchmark.BenchmarkInfo.Name}"
			: $"{CsharpRAPLCLI.Options.OutputPath}/{benchmark.BenchmarkInfo.Name}";

		Directory.CreateDirectory(outputPath);
		using var writer = new StreamWriter($"{outputPath}/{benchmark.BenchmarkInfo.Name}-{time}.json");
		writer.Write(JsonSerializer.Serialize(benchmark.BenchmarkInfo,
			new JsonSerializerOptions { WriteIndented = true }));
	}
}