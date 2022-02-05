using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks.Operations;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SubtractionBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;

	[Benchmark("Subtraction", "Tests simple subtraction")]
	public static ulong Subtraction() {
		ulong a = 10;
		ulong res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			res = a - i - 3;
		}

		return res;
	}


	[Benchmark("Subtraction", "Tests simple subtraction where the parts are marked as constant")]
	public static ulong Const() {
		const ulong a = 10;
		ulong res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			res = a - i - 3;
		}

		return res;
	}

	[Benchmark("Subtraction", "Tests subtraction using compound assignment")]
	public static ulong SubtractionAssign() {
		ulong a = 10;
		ulong res = 10;
		for (ulong i = 0; i < LoopIterations; i++) {
			res -= (a - i - 3);
		}

		return res;
	}


	[Benchmark("Subtraction", "Tests subtraction without compound assignment")]
	public static ulong Assign() {
		ulong a = 10;
		ulong res = 10;
		for (ulong i = 0; i < LoopIterations; i++) {
			res = res - (a - i - 3);
		}

		return res;
	}
}