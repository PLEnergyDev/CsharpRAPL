using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Invocation;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class LocalFunctionBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	private static readonly InvocationHelper InstanceObject = new();

	[Benchmark("InvocationLocalFunction", "Tests invocation using an local function")]
	public static int LocalFunction() {
		int Calc() {
			InvocationHelper.StaticField++;
			return InvocationHelper.StaticField + 2;
		}

		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += Calc() + i;
		}

		return result;
	}

	[Benchmark("InvocationLocalFunction", "Tests invocation using a static local function")]
	public static int LocalStaticFunction() {
		static int Calc() {
			InvocationHelper.StaticField++;
			return InvocationHelper.StaticField + 2;
		}

		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += Calc() + i;
		}

		return result;
	}

	[Benchmark("InvocationLocalFunction", "Tests invocation using an local function")]
	public static int LocalFunctionInvocation() {
		int Calc() {
			return InstanceObject.Calculate();
		}

		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += Calc() + i;
		}

		return result;
	}

	[Benchmark("InvocationLocalFunction", "Tests invocation using a static local function")]
	public static int LocalStaticFunctionInvocation() {
		static int Calc() {
			return InvocationHelper.CalculateStatic();
		}

		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += Calc() + i;
		}

		return result;
	}
}