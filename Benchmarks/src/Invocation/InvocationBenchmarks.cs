using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks.Invocation;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public static unsafe class InvocationBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	private static readonly InvocationHelper InstanceObject = new();

	private static readonly MethodInfo MethodReflection =
		typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.Calculate));

	private static readonly Func<ulong> FuncInt = InstanceObject.Calculate;
	private static readonly delegate*<ulong> FunctionPointerInt = &InvocationHelper.CalculateStatic;

	private delegate ulong DelegatePrototype();

	private static readonly DelegatePrototype DelegatePrototypeInstance = InstanceObject.Calculate;

	[Benchmark("Invocation", "Tests invocation using an instance")]
	public static ulong Instance() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += InstanceObject.Calculate() + i;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using an instance")]
	public static ulong InstanceStaticField() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += InstanceObject.CalculateUsingStaticField() + i;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a static method")]
	public static ulong Static() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += InvocationHelper.CalculateStatic() + i;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using an local function")]
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

	[Benchmark("Invocation", "Tests invocation using a func")]
	public static ulong Func() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += FuncInt() + i;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a reflection on an instance method")]
	public static ulong Reflection() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += (ulong)MethodReflection.Invoke(InstanceObject, Array.Empty<object>())! + i;
		}

		return result;
	}


	[Benchmark("Invocation", "Tests invocation using a function pointer")]
	public static ulong FunctionPointer() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += FunctionPointerInt() + i;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests delegate invoking an instance method")]
	public static ulong Delegate() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += DelegatePrototypeInstance.Invoke();
		}

		return result;
	}
}