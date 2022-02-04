using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public static class OperationsBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;



	[Benchmark("Operations", "Tests post increment using ++")]
	public static ulong PostIncrement() {
		ulong res = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			res++;
			res++;
		}

		return res;
	}

	[Benchmark("Operations", "Tests post decrement using --")]
	public static ulong PostDecrement() {
		ulong res = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			res--;
			res--;
		}

		return res;
	}

	[Benchmark("Operations", "Tests pre increment using ++")]
	public static ulong PreIncrement() {
		ulong res = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			++res;
			++res;
		}

		return res;
	}

	[Benchmark("Operations", "Tests pre decrement using --")]
	public static ulong PreDecrement() {
		ulong res = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			--res;
			--res;
		}

		return res;
	}
	
	[Benchmark("Operations", "Tests post increment using ++")]
	public static int PostIncrementInt() {
		int res = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			res++;
			res++;
		}

		return res;
	}

	[Benchmark("Operations", "Tests post decrement using --")]
	public static int PostDecrementInt() {
		int res = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			res--;
			res--;
		}

		return res;
	}

	[Benchmark("Operations", "Tests pre increment using ++")]
	public static int PreIncrementInt() {
		int res = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			++res;
			++res;
		}

		return res;
	}

	[Benchmark("Operations", "Tests pre decrement using --")]
	public static int PreDecrementInt() {
		int res = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			--res;
			--res;
		}

		return res;
	}
}