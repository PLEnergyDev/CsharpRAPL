using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace ExampleProject.Benchmarks.Operations;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class AdditionBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	[Benchmark("Addition", "Tests simple addition")]
	public static int Add() {
		int a = 10;
		int b = 2;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a + b;
		}

		return res;
	}

	[Benchmark("Addition", "Tests simple addition where the parts are marked as constant")]
	public static int Const() {
		const int a = 10;
		const int b = 2;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a + b;
		}

		return res;
	}

	[Benchmark("Addition", "Tests addition using compound assignment")]
	public static int AddAssign() {
		int a = 10;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res += a;
		}

		return res;
	}

	[Benchmark("Addition", "Tests addition without compound assignment")]
	public static int Assign() {
		int a = 10;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = res + a;
		}

		return res;
	}
}