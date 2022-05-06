namespace CsharpRAPL.Benchmarking;

public class NopBenchmarkLifecycle : IBenchmarkLifecycle<IBenchmark> {
	public NopBenchmarkLifecycle(IBenchmark bm) {
		Benchmark = bm;
	}
	public IBenchmark Benchmark { get; }
	public IBenchmark Initialize(IBenchmark benchmark) => benchmark;
	public IBenchmark PostRun(IBenchmark oldstate) => oldstate;
	public IBenchmark PreRun(IBenchmark oldstate) => oldstate;
	public IBenchmark WarmupIteration(IBenchmark oldstate) => oldstate;
}
