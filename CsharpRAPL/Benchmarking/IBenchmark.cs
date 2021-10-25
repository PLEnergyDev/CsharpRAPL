using System.Collections.Generic;
using CsharpRAPL.Data;

namespace CsharpRAPL.Benchmarking;

public interface IBenchmark {
	public int Iterations { get; }
	public string Name { get; }
	public string? Group { get; }
	public int Order { get; }
	public bool HasRun { get; }
	public void Run();
	public List<BenchmarkResult> GetResults(bool ignoreFirst = true);
}