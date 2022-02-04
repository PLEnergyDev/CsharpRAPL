using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Invocation;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ReflectionBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


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

	private static readonly Func<InvocationHelper, ulong> ReflectionDelegateInt =
		(Func<InvocationHelper, ulong>)Delegate.CreateDelegate(typeof(Func<InvocationHelper, ulong>),
			MethodReflectionDelegate);

	private static readonly Func<ulong> StaticReflectionDelegateInt =
		(Func<ulong>)Delegate.CreateDelegate(typeof(Func<ulong>), MethodReflectionDelegateStatic);

	[Benchmark("InvocationReflection", "Tests invocation using a reflection on an instance method")]
	public static ulong Reflection() {
		ulong result = 0;

		for (ulong i  = 0; i < LoopIterations; i++) {
			result += (ulong)MethodReflection.Invoke(InstanceObject, Array.Empty<object>())! + i;
		}

		return result;
	}

	[Benchmark("InvocationReflection", "Tests invocation using a reflection on a static method")]
	public static ulong ReflectionStatic() {
		ulong result = 0;

		for (ulong i  = 0; i < LoopIterations; i++) {
			result += (ulong)StaticMethodReflection.Invoke(null, Array.Empty<object>())! + i;
		}

		return result;
	}

	[Benchmark("InvocationReflection", "Tests invocation using a reflection on an instance method")]
	public static ulong ReflectionFlags() {
		ulong result = 0;

		for (ulong i  = 0; i < LoopIterations; i++) {
			result += (ulong)MethodReflectionFlags.Invoke(InstanceObject, Array.Empty<object>())! + i;
		}

		return result;
	}

	[Benchmark("InvocationReflection", "Tests invocation using a reflection on a static method")]
	public static ulong ReflectionStaticFlags() {
		ulong result = 0;

		for (ulong i  = 0; i < LoopIterations; i++) {
			result += (ulong)StaticMethodReflectionFlags.Invoke(null, Array.Empty<object>())! + i;
		}

		return result;
	}

	[Benchmark("InvocationReflection", "Tests invocation using a reflection on an instance method using delegate")]
	public static ulong ReflectionDelegate() {
		ulong result = 0;

		for (ulong i  = 0; i < LoopIterations; i++) {
			result += ReflectionDelegateInt(InstanceObject) + i;
		}

		return result;
	}

	[Benchmark("InvocationReflection", "Tests invocation using a reflection on a static method using delegate")]
	public static ulong ReflectionDelegateStatic() {
		ulong result = 0;

		for (ulong i  = 0; i < LoopIterations; i++) {
			result += StaticReflectionDelegateInt() + i;
		}

		return result;
	}
}