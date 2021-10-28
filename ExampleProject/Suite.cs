using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsharpRAPL.Analysis;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.CommandLine;
using CsvHelper;
using CsvHelper.Configuration;

CsharpRAPLCLI.SetAnalysis(analysis => {
	Console.WriteLine($"Min:\n{analysis.GetMin()}");
	Console.WriteLine($"Max:\n{analysis.GetMax()}");
	Console.WriteLine($"Average:\n{analysis.GetAverage()}");

	//Plots the two things in the analysis.
	analysis.PlotAnalysis();
});

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
	using var writer = new StreamWriter($"pvalues/{group}/{time}");
	using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" });
	csv.WriteRecords(result);
}


CsharpRAPLCLI.StartAnalysis(suite.GetBenchmarksByGroup());
