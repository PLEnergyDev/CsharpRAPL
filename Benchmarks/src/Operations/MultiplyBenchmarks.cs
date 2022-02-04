using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Operations;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class MultiplyBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	[Benchmark("Multiplication", "Tests simple multiplication")]
	public static ulong Multiply() {
		ulong a = 5;
		ulong res = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			res = a * i;
		}

		return res;
	}

	[Benchmark("Multiplication", "Tests simple multiplication where the parts are marked as constant")]
	public static ulong Const() {
		const ulong a = 5;
		ulong res = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			res = a * i;
		}

		return res;
	}

	[Benchmark("Multiplication", "Tests multiplication using compound assignment")]
	public static ulong MultiplyAssign() {
		ulong a = 5;
		ulong res = 1;
		for (ulong i  = 0; i < LoopIterations; i++) {
			res *= (a + i);
		}

		return res;
	}

	[Benchmark("Multiplication", "Tests multiplication without compound assignment")]
	public static ulong Assign() {
		ulong a = 5;
		ulong res = 1;
		for (ulong i  = 0; i < LoopIterations; i++) {
			res = res * (a + i);
		}

		return res;
	}
}