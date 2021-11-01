using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CsharpRAPL.Benchmarking;
using ExampleProject.HelperObjects;

namespace ExampleProject.Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public static unsafe class InvocationBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	private static readonly MethodInfo StaticMethodReflection;
	private static readonly MethodInfo MethodReflection;

	private static readonly MethodInfo StaticMethodReflectionFlags;
	private static readonly MethodInfo MethodReflectionFlags;

	private static readonly InvocationHelper InstanceObject;
	private static readonly Func<int> FuncInt;

	private static readonly Func<int> StaticFuncInt;

	private static readonly delegate*<int> FunctionPointerInt;

	private static readonly MethodInfo MethodReflectionDelegate =
		typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.Calculate),
			BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod);

	private static readonly MethodInfo MethodReflectionDelegateStatic =
		typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.CalculateStatic),
			BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod);

	private static readonly Func<InvocationHelper, int> ReflectionDelegateInt =
		(Func<InvocationHelper, int>)Delegate.CreateDelegate(typeof(Func<InvocationHelper, int>),
			MethodReflectionDelegate);

	private static readonly Func<int> StaticReflectionDelegateInt =
		(Func<int>)Delegate.CreateDelegate(typeof(Func<int>), MethodReflectionDelegateStatic);

	static InvocationBenchmarks() {
		MethodReflection = typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.Calculate));
		StaticMethodReflection = typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.CalculateStatic));

		MethodReflectionFlags = typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.Calculate),
			BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod);
		StaticMethodReflectionFlags = typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.CalculateStatic),
			BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod);
		InstanceObject = new InvocationHelper();
		FuncInt = InstanceObject.Calculate;
		StaticFuncInt = InvocationHelper.CalculateStatic;
		FunctionPointerInt = &InvocationHelper.CalculateStatic;
	}


	[Benchmark("Invocation", "Tests invocation using an instance")]
	public static int Instance() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += InstanceObject.Calculate();
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a static method")]
	public static int Static() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += InvocationHelper.CalculateStatic();
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a func")]
	public static int Func() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += FuncInt();
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a static func")]
	public static int StaticFunc() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += StaticFuncInt();
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using an local function")]
	public static int LocalFunction() {
		//TODO: Make non const maybe?
		int Calc() => 1 + 1;
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += Calc();
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a static local function")]
	public static int StaticLocalFunction() {
		//TODO: Make non const maybe?
		static int Calc() => 1 + 1;
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += Calc();
		}

		return result;
	}
	
	[Benchmark("Invocation", "Tests invocation using a reflection on an instance method")]
	public static int Reflection() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += (int)MethodReflection.Invoke(InstanceObject, Array.Empty<object>())!;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a reflection on a static method")]
	public static int StaticReflection() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += (int)StaticMethodReflection.Invoke(null, Array.Empty<object>())!;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a reflection on an instance method")]
	public static int ReflectionFlags() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += (int)MethodReflectionFlags.Invoke(InstanceObject, Array.Empty<object>())!;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a reflection on a static method")]
	public static int StaticReflectionFlags() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += (int)StaticMethodReflectionFlags.Invoke(null, Array.Empty<object>())!;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a reflection on an instance method using delegate")]
	public static int ReflectionDelegate() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += ReflectionDelegateInt(InstanceObject);
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a reflection on a static method using delegate")]
	public static int StaticReflectionDelegate() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += StaticReflectionDelegateInt();
		}

		return result;
	}


	[Benchmark("Invocation", "Tests invocation using a function pointer")]
	public static int FunctionPointer() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += FunctionPointerInt();
		}

		return result;
	}

	//TODO: Figure out how to do this
	public static int Lambda() {
		return 1;
	}

	//TODO: This has no return type how do we compared?
	public static int Action() {
		return 1;
	}

	//TODO: This has no return type how do we compared?
	public static int StaticAction() {
		return 1;
	}
}