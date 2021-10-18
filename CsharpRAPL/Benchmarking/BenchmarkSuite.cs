using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CsharpRAPL.Analysis;

namespace CsharpRAPL.Benchmarking {
	public class BenchmarkSuite {
		private Dictionary<string, IBenchmark> Benchmarks { get; } = new();

		public void AddBenchmark<T>(string? group, int iterations, Func<T> benchmark, int order = 0) {
			string benchmarkName = benchmark.Method.Name;
			if (Benchmarks.ContainsKey(benchmarkName))
				throw new Exception($"Trying to add a benchmark with the same name twice. Name is: {benchmarkName}");

			Benchmarks.Add(benchmarkName, new Benchmark<T>(benchmarkName, iterations, benchmark, false, group, order));
		}

		public void AddBenchmark<T>(int iterations, Func<T> benchmark) {
			AddBenchmark(null, iterations, benchmark);
		}

		public void RunAll(int skipAmount = 0) {
			List<IBenchmark> benchmarks = Benchmarks.OrderBy(pair => pair.Value.Order).Skip(skipAmount)
				.Select(pair => pair.Value).ToList();
			if (Environment.OSVersion.Platform != PlatformID.Unix) {
				throw new NotSupportedException("Running the benchmarks is only supported on Unix.");
			}

			var timer = new Stopwatch();
			foreach ((int index, IBenchmark bench) in benchmarks.WithIndex()) {
				Console.WriteLine($"Starting {bench.Name} which is the {index + 1} out of {benchmarks.Count} tests");
				timer.Start();
				bench.Run();
				timer.Stop();
				Console.WriteLine($"\rFinished {bench.Name} which took {timer.ElapsedMilliseconds}ms\n");
				timer.Reset();
			}
		}

		public Analysis.Analysis AnalyseResults(string firstBenchmarkName, string secondBenchmarkName) {
			if (!Benchmarks.ContainsKey(firstBenchmarkName)) {
				throw new KeyNotFoundException($"No benchmark with the name {firstBenchmarkName} has been registered.");
			}

			if (!Benchmarks.ContainsKey(secondBenchmarkName)) {
				throw new KeyNotFoundException(
					$"No benchmark with the name {secondBenchmarkName} has been registered.");
			}

			IBenchmark firstBenchmark = Benchmarks[firstBenchmarkName];
			IBenchmark secondBenchmark = Benchmarks[secondBenchmarkName];
			return new Analysis.Analysis(firstBenchmark, secondBenchmark);
		}

		public IReadOnlyCollection<IBenchmark> GetBenchmarks() {
			return Benchmarks.Values;
		}

		public void PlotGroups() {
			Dictionary<string, List<IBenchmark>> groups = new();
			foreach (IBenchmark benchmark in Benchmarks.Values.Where(benchmark => benchmark.Group != null)) {
				Debug.Assert(benchmark.Group != null, "benchmark.Group != null");
				if (!groups.ContainsKey(benchmark.Group)) {
					groups.Add(benchmark.Group, new List<IBenchmark>());
				}

				groups[benchmark.Group].Add(benchmark);
			}

			foreach (KeyValuePair<string, List<IBenchmark>> pair in groups) {
				BenchmarkPlot.PlotAllResults(pair.Value.ToArray());
			}
		}
	}
}