using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Invocation;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ReflectionBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	private static readonly InvocationHelper InstanceObject = new();

	private static readonly MethodInfo MethodReflection =
		typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.Calculate));

	private static readonly MethodInfo StaticMethodReflection =
		typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.CalculateStatic));

	private static readonly MethodInfo MethodReflectionFlags =
		typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.Calculate),
			BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod);

	private static readonly MethodInfo StaticMethodReflectionFlags =
		typeof(InvocationHelper).GetMethod(nameof(InvocationHelper.CalculateStatic),
			BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod);


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

	[Benchmark("InvocationReflection", "Tests invocation using a reflection on an instance method")]
	public static int Reflection() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += (int)MethodReflection.Invoke(InstanceObject, Array.Empty<object>())! + i;
		}

		return result;
	}

	[Benchmark("InvocationReflection", "Tests invocation using a reflection on a static method")]
	public static int ReflectionStatic() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += (int)StaticMethodReflection.Invoke(null, Array.Empty<object>())! + i;
		}

		return result;
	}

	[Benchmark("InvocationReflection", "Tests invocation using a reflection on an instance method")]
	public static int ReflectionFlags() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += (int)MethodReflectionFlags.Invoke(InstanceObject, Array.Empty<object>())! + i;
		}

		return result;
	}

	[Benchmark("InvocationReflection", "Tests invocation using a reflection on a static method")]
	public static int ReflectionStaticFlags() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += (int)StaticMethodReflectionFlags.Invoke(null, Array.Empty<object>())! + i;
		}

		return result;
	}

	[Benchmark("InvocationReflection", "Tests invocation using a reflection on an instance method using delegate")]
	public static int ReflectionDelegate() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += ReflectionDelegateInt(InstanceObject) + i;
		}

		return result;
	}

	[Benchmark("InvocationReflection", "Tests invocation using a reflection on a static method using delegate")]
	public static int ReflectionDelegateStatic() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += StaticReflectionDelegateInt() + i;
		}

		return result;
	}
}