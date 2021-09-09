using System;
using System.Collections.Generic;
using System.Linq;

namespace CsharpRAPL {
	public class BenchmarkSuite {
		private readonly List<Benchmark> _benchmarks = new();

		public void AddBenchmark(string benchmarkName, int iterations, Func<int> benchmark,
			Action<int> benchmarkOutput) {
			if (_benchmarks.Any(bench => bench.Name == benchmarkName))
				throw new Exception($"Trying to add a benchmark with the same name twice. Name is: {benchmarkName}");
			_benchmarks.Add(new Benchmark(benchmarkName, iterations, benchmark, benchmarkOutput));
		}

		public void RunAll() {
			foreach (Benchmark bench in _benchmarks) {
				bench.Run();
			}
		}
	}
}