using CsharpRAPL.Data;

namespace CsharpRAPL.Measuring;

public interface IMeasureApi {
	void Start();
	void End();
	bool IsValid();
	BenchmarkResult GetResults(ulong loopIterations);
	BenchmarkResult GetNormalizedResults(ulong loopIterations, int normalizedIterations = 1000000);
}