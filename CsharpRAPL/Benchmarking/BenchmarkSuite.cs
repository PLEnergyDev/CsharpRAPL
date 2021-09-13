using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CsharpRAPL.Benchmarking {
	public class BenchmarkSuite {
		public bool HasRun { get; private set; }
		private Dictionary<string, Benchmark> Benchmarks { get; } = new();

		public void AddBenchmark(string group, int iterations, Func<int> benchmark) {
			string benchmarkName = benchmark.Method.Name;
			if (Benchmarks.ContainsKey(benchmarkName))
				throw new Exception($"Trying to add a benchmark with the same name twice. Name is: {benchmarkName}");

			Benchmarks.Add(benchmarkName, new Benchmark(benchmarkName, iterations, benchmark, Console.WriteLine,
				@group: group));
		}

		public void AddBenchmark(int iterations, Func<int> benchmark) {
			AddBenchmark(null, iterations, benchmark);
		}

		public void RunAll(int skipAmount = 0) {
			if (Environment.OSVersion.Platform != PlatformID.Unix) {
				throw new NotSupportedException("Running the benchmarks is only supported on Unix.");
			}

			List<Benchmark> benchmarks = Benchmarks.Skip(skipAmount).Select(pair => pair.Value).ToList();

			if (benchmarks.Count != 0)
				HasRun = true;

			var timer = new Stopwatch();
			foreach ((int index, Benchmark bench) in benchmarks.WithIndex()) {
				Console.WriteLine($"Starting {bench.Name} which is the {index + 1} out of {benchmarks.Count} tests");
				timer.Start();
				bench.Run();
				timer.Stop();
				Console.WriteLine($"Finished {bench.Name} which took {timer.ElapsedMilliseconds}ms");
				timer.Reset();
			}
		}

		public Dictionary<string, double> AnalyseResults(string firstBenchmarkName, string secondBenchmarkName) {
			if (!HasRun) {
				throw new NotSupportedException(
					"It's not supported to analyse results before the benchmarks have run. Use Analysis class instead where you can use paths");
			}

			if (!Benchmarks.ContainsKey(firstBenchmarkName)) {
				throw new KeyNotFoundException($"No benchmark with the name {firstBenchmarkName} has been registered.");
			}

			if (!Benchmarks.ContainsKey(secondBenchmarkName)) {
				throw new KeyNotFoundException(
					$"No benchmark with the name {secondBenchmarkName} has been registered.");
			}

			Benchmark firstBenchmark = Benchmarks[firstBenchmarkName];
			Benchmark secondBenchmark = Benchmarks[secondBenchmarkName];
			var analysis = new Analysis.Analysis(firstBenchmark, secondBenchmark);
			return analysis.CalculatePValue();
		}
	}
}