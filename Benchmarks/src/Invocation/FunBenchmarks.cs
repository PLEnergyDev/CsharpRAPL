using System;
using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Invocation;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class FunBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	private static readonly InvocationHelper InstanceObject = new();
	private static readonly Func<int> FuncInt = InstanceObject.Calculate;
	private static readonly Func<int> StaticFuncInt = InvocationHelper.CalculateStatic;

	[Benchmark("InvocationFunc", "Tests invocation using a func")]
	public static int Func() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += FuncInt() + i;
		}

		return result;
	}

	[Benchmark("InvocationFunc", "Tests invocation using a static func")]
	public static int FuncStatic() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += StaticFuncInt() + i;
		}

		return result;
	}
}