using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace CsharpRAPL.Tests.Benchmarking;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class DummyBenchmarks {
	public static int Iterations;
	public static int TestField1;
	public int TestField2;
#pragma warning disable CS0169
	//Used via reflection
	private static int _testField3;
#pragma warning restore CS0169
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

	[Benchmark("Operations", "Tests pre increment using ++", 10)]
	public static int PreIncrement() {
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			++res;
		}

		return res;
	}

	[Benchmark("Operations", "Tests pre decrement using --", skip: true)]
	public static int PreDecrement() {
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			--res;
		}

		return res;
	}

	private static int PrivateTest() {
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			--res;
		}

		return res;
	}

	public void VoidTest() {
		PrivateTest();
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			--res;
		}
	}
}