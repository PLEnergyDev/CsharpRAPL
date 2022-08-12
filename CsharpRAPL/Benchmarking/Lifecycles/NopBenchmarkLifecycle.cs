using System;
using System.Reflection;
using CsharpRAPL.Benchmarking.Attributes;

namespace CsharpRAPL.Benchmarking.Lifecycles;

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
	private readonly FieldInfo _loopIterationsFieldInfo;

	public NopBenchmarkLifecycle(BenchmarkInfo bm, MethodInfo benchmarkedMethod) {
		BenchmarkedMethod = benchmarkedMethod;
		BenchmarkInfo = bm;
		// Fetch loopiterations field in benchmark class
		_loopIterationsFieldInfo =
			BenchmarkedMethod.DeclaringType?.GetField("LoopIterations", BindingFlags.Public | BindingFlags.Static) ??
			throw new InvalidOperationException(
				$"Your class '{BenchmarkedMethod.DeclaringType?.Name}' must have the field 'LoopIterations'.");
	}
	public MethodInfo BenchmarkedMethod { get; }

	public BenchmarkInfo BenchmarkInfo { get; }

	public IBenchmark Initialize(IBenchmark benchmark) {
		SetLoopIterations(BenchmarkInfo.LoopIterations);
		return benchmark;
	}
	public IBenchmark AdjustLoopIterations(IBenchmark oldstate) {
		if (ScaleLoopIterations()) {
			oldstate.ResetBenchmark = true;
		}
		return oldstate;
	}

	public IBenchmark End(IBenchmark oldstate) {
		return oldstate;
	}

	public IBenchmark PostRun(IBenchmark oldstate) => oldstate;
	public IBenchmark PreRun(IBenchmark oldstate) => oldstate;

	public object Run(IBenchmark state) {

		
		object? instance = BenchmarkedMethod.IsStatic ? null : Activator.CreateInstance(BenchmarkedMethod.DeclaringType!);
		var parameters = this.GetParameters();
		BenchmarkedMethod.Invoke(instance, parameters);
		return state;
	}

	public IBenchmark WarmupIteration(IBenchmark oldstate) => oldstate;
	
	private bool ScaleLoopIterations() {
		ulong currentValue = GetLoopIterations();
		

		switch (currentValue) {
			case ulong.MaxValue:
				return false;
			case >= ulong.MaxValue / 2:
				SetLoopIterations(ulong.MaxValue);
				BenchmarkInfo.RawResults.Clear();
				BenchmarkInfo.NormalizedResults.Clear();
				return true;
			default:
				SetLoopIterations(currentValue + currentValue);
				BenchmarkInfo.RawResults.Clear();
				BenchmarkInfo.NormalizedResults.Clear();
				return true;
		}
	}

	private void SetLoopIterations(ulong maxValue) {
		BenchmarkInfo.LoopIterations = maxValue;
		_loopIterationsFieldInfo.SetValue(null, maxValue);
	}

	private ulong GetLoopIterations() {
		return (ulong)(_loopIterationsFieldInfo.GetValue(null) ?? throw new InvalidOperationException("Your class must have the field 'LoopIterations'"));
	}
}
