using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace CsharpRAPL {
	public class BenchmarkSuite {
		private readonly List<Benchmark> _benchmarks = new();

		public void AddBenchmark(int iterations, Func<int> benchmark) {
			string benchmarkName = benchmark.Method.Name;
			if (_benchmarks.Any(bench => bench.Name == benchmarkName))
				throw new Exception($"Trying to add a benchmark with the same name twice. Name is: {benchmarkName}");
			_benchmarks.Add(new Benchmark(benchmarkName, iterations, benchmark, Console.WriteLine));
		}

		public void RunAll(int skipAmount = 0) {
			List<Benchmark> benchmarks = _benchmarks.Skip(skipAmount).ToList();
			foreach ((int index, Benchmark bench) in benchmarks.WithIndex()) {
				Console.WriteLine($"Starting {bench.Name} which is {index} out of {benchmarks.Count - 1} tests");
				bench.Run();
			}
		}
	}
}