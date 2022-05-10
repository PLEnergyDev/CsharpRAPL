using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CsharpRAPL.Benchmarking.Attributes;

namespace CsharpRAPL.Benchmarking;


public class NopBenchmarkLifecycle : IBenchmarkLifecycle<IBenchmark> {
	public NopBenchmarkLifecycle(BenchmarkInfo bm, MethodInfo benchmarkedMethod) {
		BenchmarkedMethod = benchmarkedMethod;
		BenchmarkInfo = bm;
	}
	//Func<object, object> Invoker { get; }
	public MethodInfo BenchmarkedMethod { get; }

	public BenchmarkInfo BenchmarkInfo { get; }
	public IBenchmark Initialize(IBenchmark benchmark) => benchmark;
	public IBenchmark PostRun(IBenchmark oldstate) => oldstate;
	public IBenchmark PreRun(IBenchmark oldstate) => oldstate;


	private object[] GetPameters() {

		ParameterInfo[] vs = BenchmarkedMethod.GetParameters();
		var paramvalues = new object[vs.Length];
		foreach (var v in vs) {
			paramvalues[v.Position] = v.GetCustomAttribute<BenchParameterAttribute>()?.BenchmarkParameterSource switch {
				"LoopIterations" => BenchmarkInfo.LoopIterations,
				"Iterations" => BenchmarkInfo.Iterations,
				string parameterName => throw new InvalidOperationException($"Unknown parameter: [{parameterName}] position:[{v.Position}] of method: [{BenchmarkedMethod.Name}]")
			};
		}
		return paramvalues;
	}

	public object Run(IBenchmark state) {

		
		object? instance = BenchmarkedMethod.IsStatic ? null : Activator.CreateInstance(BenchmarkedMethod.DeclaringType!);
		var parameters = GetPameters();
		return BenchmarkedMethod.Invoke(instance, parameters);
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
