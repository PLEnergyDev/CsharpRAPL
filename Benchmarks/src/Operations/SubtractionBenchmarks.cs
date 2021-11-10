using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Operations; 
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SubtractionBenchmarks {
	public static int Iterations;
	public static int LoopIterations;
	[Benchmark("Subtraction", "Tests simple subtraction")]
	public static int Subtraction() {
		int a = 10;
		int b = 2;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a - b;
		}

		return res;
	}


	[Benchmark("Subtraction", "Tests simple subtraction where the parts are marked as constant")]
	public static int Const() {
		const int a = 10;
		const int b = 2;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a - b;
		}

		return res;
	}

	[Benchmark("Subtraction", "Tests subtraction using compound assignment")]
	public static int SubtractionAssign() {
		int a = 10;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res -= a;
		}

		return res;
	}


	[Benchmark("Subtraction", "Tests subtraction without compound assignment")]
	public static int Assign() {
		int a = 10;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = res - a;
		}

		return res;
	}
}