using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CsharpRAPL.Benchmarking;
using ExampleProject.HelperObjects;

namespace ExampleProject.Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public unsafe class InvocationBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	private static readonly MethodInfo StaticMethodReflection;
	private static readonly MethodInfo MethodReflection;

	private static readonly MethodInfo StaticMethodReflectionFlags;
	private static readonly MethodInfo MethodReflectionFlags;

	private static readonly InvocationHelper InstanceObject;
	private static readonly Func<int> Func;

	private static readonly Func<int> StaticFunc;

	//private static readonly Action Action;
	//private static readonly Action StaticAction;
	private static readonly delegate*<int> FunctionPointer;

	private static readonly MethodInfo MethodReflectionFlagsDelegate =
		typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.Calculate),
			BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod);
	
	private static readonly MethodInfo MethodReflectionFlagsDelegateStatic =
		typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.CalculateStatic),
			BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod);

	private static readonly Func<InvocationHelper, int> ReflectionDelegate =
		(Func<InvocationHelper, int>)Delegate.CreateDelegate(typeof(Func<InvocationHelper, int>),
			MethodReflectionFlagsDelegate);

	private static readonly Func<int> StaticReflectionDelegate =
		(Func<int>)Delegate.CreateDelegate(typeof(Func<int>), MethodReflectionFlagsDelegateStatic);

	static InvocationBenchmarks() {
		MethodReflection = typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.Calculate));
		StaticMethodReflection = typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.CalculateStatic));

		MethodReflectionFlags = typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.Calculate),
			BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod);
		StaticMethodReflectionFlags = typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.CalculateStatic),
			BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod);
		InstanceObject = new InvocationHelper();
		Func = InstanceObject.Calculate;
		StaticFunc = InvocationHelper.CalculateStatic;
		//Action = () => InstanceObject.Calculate();
		//StaticAction = () => StaticMethod.Calculate();
		FunctionPointer = &InvocationHelper.CalculateStatic;
	}


	[Benchmark("Invocation", "Tests invocation using an instance")]
	public static int InstanceInvocation() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += InstanceObject.Calculate();
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a static method")]
	public static int StaticInvocation() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += InvocationHelper.CalculateStatic();
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a func")]
	public static int FuncInvocation() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += Func();
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a static func")]
	public static int StaticFuncInvocation() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += StaticFunc();
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using an local function")]
	public static int LocalFunctionInvocation() {
		int Action() => InstanceObject.Calculate();
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += Action();
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a static local function")]
	public static int StaticLocalFunctionInvocation() {
		static int Action() => InvocationHelper.CalculateStatic();
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += Action();
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a reflection on an instance method")]
	public static int ReflectionInvocation() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += (int)MethodReflection.Invoke(InstanceObject, Array.Empty<object>())!;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a reflection on a static method")]
	public static int StaticReflectionInvocation() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += (int)StaticMethodReflection.Invoke(null, Array.Empty<object>())!;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a reflection on an instance method")]
	public static int ReflectionInvocationFlags() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += (int)MethodReflectionFlags.Invoke(InstanceObject, Array.Empty<object>())!;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a reflection on a static method")]
	public static int StaticReflectionInvocationFlags() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += (int)StaticMethodReflectionFlags.Invoke(null, Array.Empty<object>())!;
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a reflection on an instance method")]
	public static int ReflectionInvocationFlagsDelegate() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += ReflectionDelegate(InstanceObject);
		}

		return result;
	}

	[Benchmark("Invocation", "Tests invocation using a reflection on a static method")]
	public static int StaticReflectionInvocationFlagsDelegate() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += StaticReflectionDelegate();
		}

		return result;
	}


	[Benchmark("Invocation", "Tests invocation using a function pointer")]
	public static int FunctionPointerInvocation() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += FunctionPointer();
		}

		return result;
	}

	//TODO: Figure out how to do this
	public static int LambdaInvocation() {
		return 1;
	}

	//TODO: This has no return type how do we compared?
	public static int ActionInvocation() {
		return 1;
	}

	//TODO: This has no return type how do we compared?
	public static int StaticActionInvocation() {
		return 1;
	}
}