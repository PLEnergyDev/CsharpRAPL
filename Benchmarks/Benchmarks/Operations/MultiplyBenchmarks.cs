using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace ExampleProject.Benchmarks.Operations;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class MultiplyBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	[Benchmark("Multiplication", "Tests simple multiplication")]
	public static int Multiply() {
		int a = 5;
		int b = 1;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a * b;
		}

		return res;
	}

	[Benchmark("Multiplication", "Tests simple multiplication where the parts are marked as constant")]
	public static int Const() {
		const int a = 5;
		const int b = 1;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a * b;
		}

		return res;
	}

	[Benchmark("Multiplication", "Tests multiplication using compound assignment")]
	public static int MultiplyAssign() {
		int a = 5;
		int res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res *= a;
		}

		return res;
	}

	[Benchmark("Multiplication", "Tests multiplication without compound assignment")]
	public static int Assign() {
		int a = 5;
		int res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res = res * a;
		}

		return res;
	}
}