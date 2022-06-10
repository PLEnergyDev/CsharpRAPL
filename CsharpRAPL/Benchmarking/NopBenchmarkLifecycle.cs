using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CsharpRAPL.Benchmarking.Attributes;

namespace CsharpRAPL.Benchmarking;

public static class IBenchmarkLifecycleExt {
	public static object[] GetParameters(this IBenchmarkLifecycle lf) {

		ParameterInfo[] vs = lf.BenchmarkedMethod.GetParameters();
		var paramvalues = new object[vs.Length];
		foreach (var v in vs) {
			paramvalues[v.Position] = v.GetCustomAttribute<BenchParameterAttribute>()?.BenchmarkParameterSource switch {
				"LoopIterations" => lf.BenchmarkInfo.LoopIterations,
				"Iterations" => lf.BenchmarkInfo.Iterations,
				null => throw new NotSupportedException($"Unmarked parameter: [{v.Name}] position:[{v.Position}] of method: [{lf.BenchmarkedMethod.Name}] -- mark with {nameof(BenchParameterAttribute)}"),
				string parameterName => throw new InvalidOperationException($"Unknown parameter: [{parameterName}] position:[{v.Position}] of method: [{lf.BenchmarkedMethod.Name}]")
			};
		}
		return paramvalues;
	}
}


public class NopBenchmarkLifecycle : IBenchmarkLifecycle<IBenchmark> {
	public NopBenchmarkLifecycle(BenchmarkInfo bm, MethodInfo benchmarkedMethod) {
		BenchmarkedMethod = benchmarkedMethod;
		BenchmarkInfo = bm;
	}
	public MethodInfo BenchmarkedMethod { get; }

	public BenchmarkInfo BenchmarkInfo { get; }
	public IBenchmark Initialize(IBenchmark benchmark) => benchmark;
	public IBenchmark PostRun(IBenchmark oldstate) => oldstate;
	public IBenchmark PreRun(IBenchmark oldstate) => oldstate;

	public object Run(IBenchmark state) {

		
		object? instance = BenchmarkedMethod.IsStatic ? null : Activator.CreateInstance(BenchmarkedMethod.DeclaringType!);
		var parameters = this.GetParameters();
		BenchmarkedMethod.Invoke(instance, parameters);
		return state;
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
