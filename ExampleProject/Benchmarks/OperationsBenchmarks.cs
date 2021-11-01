using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace ExampleProject.Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public static class OperationsBenchmarks {
	public static int Iterations;
	public static int LoopIterations;


	[Benchmark("Operations", "Tests post increment using ++")]
	public static int PostIncrement() {
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res++;
		}

		return res;
	}

	[Benchmark("Operations", "Tests post decrement using --")]
	public static int PostDecrement() {
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res--;
		}

		return res;
	}

	[Benchmark("Operations", "Tests pre increment using ++")]
	public static int PreIncrement() {
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			++res;
		}

		return res;
	}

	[Benchmark("Operations", "Tests pre decrement using --")]
	public static int PreDecrement() {
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			--res;
		}

		return res;
	}
}