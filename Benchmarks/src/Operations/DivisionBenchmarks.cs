using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Operations;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class DivisionBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	[Benchmark("Division", "Tests simple division")]
	public static ulong Divide() {
		ulong a = 10;
		ulong b = 2;
		ulong res = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			res = a / (b + i);
		}

		return res;
	}


	[Benchmark("Division", "Tests simple division where the parts are marked as constant")]
	public static ulong Const() {
		const ulong a = 10;
		const ulong b = 2;
		ulong res = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			res = a / (b + i);
		}

		return res;
	}


	[Benchmark("Division", "Tests division using compound assignment")]
	public static ulong CompAssign() {
		ulong a = 10;
		ulong res = 1;
		for (ulong i  = 0; i < LoopIterations; i++) {
			res /= (a + i);
		}

		return res;
	}

	[Benchmark("Division", "Tests division without compound assignment")]
	public static ulong Assign() {
		ulong a = 10;
		ulong res = 1;
		for (ulong i  = 0; i < LoopIterations; i++) {
			res = res / (a + i);
		}

		return res;
	}
}