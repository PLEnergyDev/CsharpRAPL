using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Operations;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class AdditionBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	[Benchmark("Addition", "Tests simple addition")]
	public static int Add() {
		int a = 10;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a + i;
		}

		return res;
	}

	[Benchmark("Addition", "Tests simple addition where the parts are marked as constant")]
	public static int Const() {
		const int a = 10;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a + i;
		}

		return res;
	}

	[Benchmark("Addition", "Tests addition using compound assignment")]
	public static long AddAssign() {
		int a = 1;
		int res = 9;
		for (int i = 0; i < LoopIterations; i++) {
			res += a + i;
		}

		return res;
	}

	[Benchmark("Addition", "Tests addition without compound assignment")]
	public static int Assign() {
		int a = 1;
		int res = 9;
		for (int i = 0; i < LoopIterations; i++) {
			res = res + a + i;
		}

		return res;
	}
}