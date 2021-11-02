using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using CsharpRAPL.CommandLine;
using CsharpRAPL.Plotting;

namespace CsharpRAPL.Benchmarking;

public class BenchmarkSuite {
	private List<IBenchmark> Benchmarks { get; } = new();

	public void RegisterBenchmark<T>(string? group, int iterations, Func<T> benchmark, int order = 0) {
		string benchmarkName = benchmark.Method.Name;

		if (benchmark.Method.IsAnonymous()) {
			throw new NotSupportedException("Adding benchmarks through anonymous methods is not supported");
		}
		
		Benchmarks.Add(new Benchmark<T>(benchmarkName, iterations, benchmark, false, group, order));
	}

	public void RegisterBenchmark<T>(int iterations, Func<T> benchmark) {
		RegisterBenchmark(null, iterations, benchmark);
	}

	public void RunAll() {
		List<IBenchmark> benchmarks = Benchmarks.OrderBy(pair => pair.Order).ToList();

		if (Environment.OSVersion.Platform != PlatformID.Unix) {
			throw new NotSupportedException("Running the benchmarks is only supported on Unix.");
		}

		Warmup();

		var timer = new Stopwatch();
		foreach ((int index, IBenchmark bench) in benchmarks.WithIndex()) {
			Console.WriteLine($"Starting {bench.Name} which is the {index + 1} out of {benchmarks.Count} tests");
			timer.Start();
			bench.Run();
			timer.Stop();
			Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
			Console.WriteLine(
				$"\rFinished {bench.Name} in {timer.ElapsedMilliseconds}ms with {bench.GetResults().Count} iterations\n");
			timer.Reset();
		}

		PlotGroups();

		if (CsharpRAPLCLI.Options.ZipResults) {
			ZipResults();
		}
	}

	private static void Warmup() {
		int warmup = 0;
		Console.WriteLine("Warmup commencing");
		for (int i = 0; i < 10; i++) {
			while (warmup < int.MaxValue) {
				warmup++;
				if (warmup % 1000000 != 0) {
					continue;
				}

				int percentage = (int)((double)warmup / int.MaxValue * 10.0 + 10.0 * i);
				Console.SetCursorPosition(0, Console.CursorTop);
				Console.Write($"{percentage}%");
				Console.Out.Flush();
			}

			warmup = 0;
		}

		Console.Write("\n");
	}

	private static void ZipResults() {
		using var zipToOpen = new FileStream("results.zip", FileMode.Create);
		using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
		foreach (string file in Helpers.GetAllCSVFilesFromOutputPath()) {
			archive.CreateEntryFromFile(file, file);
		}

		foreach (string file in Helpers.GetAllPlotFiles()) {
			archive.CreateEntryFromFile(file, file);
		}
	}

	public IReadOnlyList<IBenchmark> GetBenchmarks() {
		return Benchmarks;
	}

	public Dictionary<string, List<IBenchmark>> GetBenchmarksByGroup() {
		Dictionary<string, List<IBenchmark>> groups = new();
		foreach (IBenchmark benchmark in Benchmarks.Where(benchmark => benchmark.Group != null)) {
			Debug.Assert(benchmark.Group != null, "benchmark.Group != null");
			if (!groups.ContainsKey(benchmark.Group)) {
				groups.Add(benchmark.Group, new List<IBenchmark>());
			}

			groups[benchmark.Group].Add(benchmark);
		}

		return groups;
	}

	public void PlotGroups() {
		foreach ((string? group, List<IBenchmark>? benchmarks) in GetBenchmarksByGroup()) {
			BenchmarkPlot.PlotAllResults(benchmarks.ToArray(), new PlotOptions { Name = group });
		}
	}
}