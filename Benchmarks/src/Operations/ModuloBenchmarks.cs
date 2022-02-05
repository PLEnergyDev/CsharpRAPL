using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks.Operations;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ModuloBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	[Benchmark("Modulo", "Tests simple modulo")]
	public static ulong Modulo() {
		ulong a = 10;
		ulong b = 2;
		ulong res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			res = a % (b + i);
		}

		return res;
	}


	[Benchmark("Modulo", "Tests simple modulo where the parts are marked as constant")]
	public static ulong Const() {
		const ulong a = 10;
		const ulong b = 2;
		ulong res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			res = a % (b + i);
		}

		return res;
	}

	[Benchmark("Modulo", "Tests modulo using compound assignment")]
	public static ulong ModuloAssign() {
		ulong a = 10;
		ulong res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			res %= (a + i);
		}

		return res;
	}

	[Benchmark("Modulo", "Tests modulo without compound assignment")]
	public static ulong Assign() {
		ulong a = 10;
		ulong res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			res = res % (a + i);
		}

		return res;
	}
}