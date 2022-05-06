using System.Collections.Generic;
using CsharpRAPL.Data;
using CsharpRAPL.Measuring;

namespace CsharpRAPL.Benchmarking;

public interface IBenchmark {
	public BenchmarkInfo BenchmarkInfo { get; }
	public IMeasureApi MeasureApiApi { get; protected set; }
	public IResultsSerializer ResultsSerializer { get; }
	public void PreRun();
	public void Run();
	public List<BenchmarkResult> GetResults(bool ignoreFirst = true);
}