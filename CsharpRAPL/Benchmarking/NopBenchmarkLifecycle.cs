using System;
using System.Reflection;

namespace CsharpRAPL.Benchmarking;


public class NopBenchmarkLifecycle : IBenchmarkLifecycle<IBenchmark> {
	public NopBenchmarkLifecycle(BenchmarkInfo bm, MethodInfo benchmarkedMethod) {
		BenchmarkedMethod = benchmarkedMethod;

		object? instance = benchmarkedMethod.IsStatic ? null : Activator.CreateInstance(benchmarkedMethod.DeclaringType!);
		Invoker = (benchmarkedMethod.GetParameters().Length == 0) ?
			(state) => benchmarkedMethod.Invoke(null, null)
			: (state) => benchmarkedMethod.Invoke(null, new object[] { state });
	}
	Func<object, object> Invoker { get; }
	public MethodInfo BenchmarkedMethod { get; }

	public BenchmarkInfo BenchmarkInfo { get; }
	public IBenchmark Initialize(IBenchmark benchmark) => benchmark;
	public IBenchmark PostRun(IBenchmark oldstate) => oldstate;
	public IBenchmark PreRun(IBenchmark oldstate) => oldstate;

	public object Run(IBenchmark state) {
		return Invoker(state);
	}

	public IBenchmark WarmupIteration(IBenchmark oldstate) => oldstate;
}
//public class NopBenchmarkLifecycle : IBenchmarkLifecycle<IBenchmark> {
//	public NopBenchmarkLifecycle(IBenchmark bm) {
//		Benchmark = bm;
//	}
//	public IBenchmark Benchmark { get; }
//	public IBenchmark Initialize(IBenchmark benchmark) => benchmark;
//	public IBenchmark PostRun(IBenchmark oldstate) => oldstate;
//	public IBenchmark PreRun(IBenchmark oldstate) => oldstate;

//	public object Run(IBenchmark state) {
//		throw new System.NotImplementedException();
//	}

//	public IBenchmark WarmupIteration(IBenchmark oldstate) => oldstate;
//}
