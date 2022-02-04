using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Operations;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class AdditionBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;

	[Benchmark("Addition", "Tests simple addition")]
	public static ulong Add() {
		ulong a = 10;
		ulong res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			res = a + i;
		}

		return res;
	}

	[Benchmark("Addition", "Tests simple addition where the parts are marked as constant")]
	public static ulong Const() {
		const ulong a = 10;
		ulong res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			res = a + i;
		}

		return res;
	}

	[Benchmark("Addition", "Tests addition using compound assignment")]
	public static ulong AddAssign() {
		ulong a = 1;
		ulong res = 9;
		for (ulong i = 0; i < LoopIterations; i++) {
			res += a + i;
		}

		return res;
	}

	[Benchmark("Addition", "Tests addition without compound assignment")]
	public static ulong Assign() {
		ulong a = 1;
		ulong res = 9;
		for (ulong i = 0; i < LoopIterations; i++) {
			res = res + a + i;
		}

		return res;
	}
}