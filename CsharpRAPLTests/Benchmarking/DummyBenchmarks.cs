using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace CsharpRAPL.Tests.Benchmarking;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class DummyBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;
	public static ulong TestField1;
	public ulong TestField2;
	public static double TestField3;
	public static uint TestField4;
	public int TestField5;
	public double TestField6;
	public static float TestField7;
#pragma warning disable CS0169
	//Used via reflection
	private static int _testField3;
#pragma warning restore CS0169


	[Benchmark("Operations", "Tests post increment using ++")]
	public static int PostIncrement() {
		int res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			res++;
		}

		return res;
	}

	[Benchmark("Operations", "Tests post decrement using --")]
	public static int PostDecrement() {
		int res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			res--;
		}

		return res;
	}

	[Benchmark("Operations", "Tests pre increment using ++", 10)]
	public static int PreIncrement() {
		int res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			++res;
		}

		return res;
	}

	[Benchmark("Operations", "Tests pre decrement using --", skip: true)]
	public static int PreDecrement() {
		int res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			--res;
		}

		return res;
	}

	private static int PrivateTest() {
		int res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			--res;
		}

		return res;
	}

	public void VoidTest() {
		PrivateTest();
		int res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			--res;
		}
	}
}