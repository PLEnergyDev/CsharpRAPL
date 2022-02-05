using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks.Invocation;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class LocalFunctionBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	private static readonly InvocationHelper InstanceObject = new();

	[Benchmark("InvocationLocalFunction", "Tests invocation using an local function")]
	public static ulong LocalFunction() {
		ulong Calc() {
			InvocationHelper.StaticField++;
			return InvocationHelper.StaticField + 2;
		}

		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += Calc() + i;
		}

		return result;
	}

	[Benchmark("InvocationLocalFunction", "Tests invocation using a static local function")]
	public static ulong LocalStaticFunction() {
		static ulong Calc() {
			InvocationHelper.StaticField++;
			return InvocationHelper.StaticField + 2;
		}

		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += Calc() + i;
		}

		return result;
	}

	[Benchmark("InvocationLocalFunction", "Tests invocation using an local function")]
	public static ulong LocalFunctionInvocation() {
		ulong Calc() {
			return InstanceObject.Calculate();
		}

		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += Calc() + i;
		}

		return result;
	}

	[Benchmark("InvocationLocalFunction", "Tests invocation using a static local function")]
	public static ulong LocalStaticFunctionInvocation() {
		static ulong Calc() {
			return InvocationHelper.CalculateStatic();
		}

		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += Calc() + i;
		}

		return result;
	}
}