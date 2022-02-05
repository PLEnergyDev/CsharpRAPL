using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks.Invocation;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class DelegateBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	private static readonly InvocationHelper InstanceObject = new();

	private delegate ulong DelegatePrototype();

	private static readonly DelegatePrototype DelegatePrototypeInstance = InstanceObject.Calculate;
	private static readonly DelegatePrototype DelegatePrototypeStatic = InvocationHelper.CalculateStatic;

	private static readonly DelegatePrototype DelegatePrototypeLambdaInstance =
		() => InstanceObject.Calculate();

	private static readonly DelegatePrototype DelegatePrototypeLambdaStatic =
		() => InstanceObject.CalculateUsingStaticField();

	[Benchmark("InvocationDelegate", "Tests delegate invoking an instance method")]
	public static ulong Delegate() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += DelegatePrototypeInstance.Invoke();
		}

		return result;
	}

	[Benchmark("InvocationDelegate", "Tests delegate invoking a static method")]
	public static ulong DelegateStatic() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += DelegatePrototypeStatic.Invoke();
		}

		return result;
	}

	[Benchmark("InvocationDelegate", "Tests delegate invoking a instance method using a lambda.")]
	public static ulong DelegateLambda() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += DelegatePrototypeLambdaInstance.Invoke();
		}

		return result;
	}

	[Benchmark("InvocationDelegate", "Tests delegate invoking a static method using a lambda.")]
	public static ulong DelegateLambdaStatic() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += DelegatePrototypeLambdaStatic.Invoke();
		}

		return result;
	}
}