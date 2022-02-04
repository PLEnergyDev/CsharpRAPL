using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class VariablesBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	[Benchmark("Variables", "Tests local variables")]
	public static ulong LocalVariable() {
		ulong localA = 0, localB = 1;
		for (ulong i = 0; i < LoopIterations; i++) {
			localA += localB + i;
		}

		return localA;
	}

	[Benchmark("Variables", "Tests static property")]
	public static ulong StaticProperty() {
		for (ulong i = 0; i < LoopIterations; i++) {
			VariableObject.StaticPropertyA += VariableObject.StaticPropertyB + i;
		}

		return VariableObject.StaticPropertyA;
	}

	[Benchmark("Variables", "Tests instance property")]
	public static ulong InstanceProperty() {
		VariableObject obj = new VariableObject();

		for (ulong i = 0; i < LoopIterations; i++) {
			obj.InstancePropertyA += obj.InstancePropertyB + i;
		}

		return obj.InstancePropertyA;
	}

	[Benchmark("Variables", "Tests static variables")]
	public static ulong StaticVariable() {
		for (ulong i = 0; i < LoopIterations; i++) {
			VariableObject.StaticVariableA += VariableObject.StaticVariableB + i;
		}

		return VariableObject.StaticVariableA;
	}

	[Benchmark("Variables", "Tests instance variables")]
	public static ulong InstanceVariable() {
		VariableObject obj = new VariableObject();

		for (ulong i = 0; i < LoopIterations; i++) {
			obj.InstanceVariableA += obj.InstanceVariableB + i;
		}

		return obj.InstanceVariableA;
	}


	[Benchmark("Variables", "Tests using a parameter as variable")]
	public static ulong Parameter() {
		VariableObject obj = new VariableObject();
		return obj.TestParameter(0, LoopIterations);
	}

	[Benchmark("Variables", "Tests using a parameter as variable in a static method")]
	public static ulong ParameterStatic() {
		VariableObject obj = new VariableObject();
		return VariableObject.StaticTestParameter(0, LoopIterations);
	}
}