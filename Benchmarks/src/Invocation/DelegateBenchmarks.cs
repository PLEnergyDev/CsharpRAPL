using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Invocation;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class DelegateBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	private static readonly InvocationHelper InstanceObject = new();

	private delegate int DelegatePrototype();

	private static readonly DelegatePrototype DelegatePrototypeInstance = InstanceObject.Calculate;
	private static readonly DelegatePrototype DelegatePrototypeStatic = InvocationHelper.CalculateStatic;

	private static readonly DelegatePrototype DelegatePrototypeLambdaInstance =
		() => InstanceObject.Calculate();

	private static readonly DelegatePrototype DelegatePrototypeLambdaStatic =
		() => InstanceObject.CalculateUsingStaticField();

	[Benchmark("InvocationDelegate", "Tests delegate invoking an instance method")]
	public static int Delegate() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += DelegatePrototypeInstance.Invoke();
		}

		return result;
	}

	[Benchmark("InvocationDelegate", "Tests delegate invoking a static method")]
	public static int DelegateStatic() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += DelegatePrototypeStatic.Invoke();
		}

		return result;
	}

	[Benchmark("InvocationDelegate", "Tests delegate invoking a instance method using a lambda.")]
	public static int DelegateLambda() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += DelegatePrototypeLambdaInstance.Invoke();
		}

		return result;
	}

	[Benchmark("InvocationDelegate", "Tests delegate invoking a static method using a lambda.")]
	public static int DelegateLambdaStatic() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += DelegatePrototypeLambdaStatic.Invoke();
		}

		return result;
	}
}