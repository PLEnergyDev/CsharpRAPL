using System.Collections.Generic;
using CsharpRAPL.Benchmarking.Variation;
using CsharpRAPL.Data;

namespace CsharpRAPL.Benchmarking;

public interface IBenchmark {
	public ulong Iterations { get; }
	public string Name { get; }
	public string? Group { get; }
	public int Order { get; }
	public bool HasRun { get; }
	public double ElapsedTime { get; }
	public VariationInstance Parameters { get; }
	public void Run();
	public List<BenchmarkResult> GetResults(bool ignoreFirst = true);
}