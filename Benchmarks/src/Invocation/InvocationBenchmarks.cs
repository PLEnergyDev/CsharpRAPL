using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Invocation;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public static unsafe class InvocationBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	private static readonly InvocationHelper InstanceObject = new();
	
	private static readonly MethodInfo MethodReflection =
		typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.Calculate));
	private static readonly Func<int> FuncInt = InstanceObject.Calculate;
	private static readonly delegate*<int> FunctionPointerInt = &InvocationHelper.CalculateStatic;
	private delegate int DelegatePrototype();
	private static readonly DelegatePrototype DelegatePrototypeInstance = InstanceObject.Calculate;

	[Benchmark("Invocation", "Tests invocation using an instance")]
	public static int Instance() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += InstanceObject.Calculate() + i;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using an instance")]
	public static int InstanceStaticField() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += InstanceObject.CalculateUsingStaticField() + i;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a static method")]
	public static int Static() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += InvocationHelper.CalculateStatic() + i;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using an local function")]
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

	[Benchmark("Invocation", "Tests invocation using a func")]
	public static int Func() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += FuncInt() + i;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a reflection on an instance method")]
	public static int Reflection() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += (int)MethodReflection.Invoke(InstanceObject, Array.Empty<object>())! + i;
		}

		return result;
	}


	[Benchmark("Invocation", "Tests invocation using a function pointer")]
	public static int FunctionPointer() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += FunctionPointerInt() + i;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests delegate invoking an instance method")]
	public static int Delegate() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += DelegatePrototypeInstance.Invoke();
		}

		return result;
	}
}