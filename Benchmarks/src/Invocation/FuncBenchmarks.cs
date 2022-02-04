using System;
using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Invocation;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class FuncBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	private static readonly InvocationHelper InstanceObject = new();
	private static readonly Func<ulong> FuncLong = InstanceObject.Calculate;
	private static readonly Func<ulong> StaticFuncInt = InvocationHelper.CalculateStatic;

	[Benchmark("InvocationFunc", "Tests invocation using a func")]
	public static ulong Func() {
		ulong result = 0;

		for (ulong i  = 0; i < LoopIterations; i++) {
			result += FuncLong() + i;
		}

		return result;
	}

	[Benchmark("InvocationFunc", "Tests invocation using a static func")]
	public static ulong FuncStatic() {
		ulong result = 0;

		for (ulong i  = 0; i < LoopIterations; i++) {
			result += StaticFuncInt() + i;
		}

		return result;
	}
}