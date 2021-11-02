using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using CsharpRAPL.Analysis;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.CommandLine;
using CsvHelper;
using CsvHelper.Configuration;

CsharpRAPLCLI.SetAnalysisCallback(_ => { });

Options options = CsharpRAPLCLI.Parse(args);

if (options.ShouldExit) {
	return;
}

var suite = new BenchmarkCollector(options.Iterations != -1 ? options.Iterations : options.DefaultIterations,
	options.LoopIterations != -1 ? options.LoopIterations : options.DefaultLoopIterations);

suite.RunAll();

foreach ((string group, List<IBenchmark> benchmarks) in suite.GetBenchmarksByGroup()) {
	Dictionary<string, double> result = Analysis.CalculatePValueForGroup(benchmarks);
	DateTime dateTime = DateTime.Now;
	var time = $"{dateTime.ToString("s").Replace(":", "-")}-{dateTime.Millisecond}";
	Directory.CreateDirectory(Path.Join(options.OutputPath, $"_pvalues/{group}/"));
	using var writer = new StreamWriter(Path.Join(options.OutputPath, $"_pvalues/{group}/{time}.csv"));
	using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" });
	csv.WriteRecords(result);
}


using var zipToOpen = new FileStream("results.zip", FileMode.Open);
using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
foreach (string file in Directory.EnumerateFiles(Path.Join(options.OutputPath, "_pvalues/"), "*.csv",
	SearchOption.AllDirectories)) {
	archive.CreateEntryFromFile(file, file);
}
archive.Dispose();
zipToOpen.Dispose();

CsharpRAPLCLI.StartAnalysis(suite.GetBenchmarksByGroup());
