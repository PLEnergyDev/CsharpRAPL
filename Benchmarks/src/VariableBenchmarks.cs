using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class VariablesBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	[Benchmark("Variables", "Tests local variables")]
	public static int LocalVariable() {
		int localA = 0, localB = 1;
		for (int i = 0; i < LoopIterations; i++) {
			localA += localB;
		}

		return localA;
	}

	[Benchmark("Variables", "Tests static property")]
	public static int StaticProperty() {
		for (int i = 0; i < LoopIterations; i++) {
			VariableObject.StaticPropertyA += VariableObject.StaticPropertyB;
		}

		return VariableObject.StaticPropertyA;
	}

	[Benchmark("Variables", "Tests instance property")]
	public static int InstanceProperty() {
		VariableObject obj = new VariableObject();

		for (int i = 0; i < LoopIterations; i++) {
			obj.InstancePropertyA += obj.InstancePropertyB;
		}

		return obj.InstancePropertyA;
	}
	
	[Benchmark("Variables", "Tests static variables")]
	public static int StaticVariable() {
		for (int i = 0; i < LoopIterations; i++) {
			VariableObject.StaticVariableA += VariableObject.StaticVariableB;
		}

		return VariableObject.StaticVariableA;
	}

	[Benchmark("Variables", "Tests instance variables")]
	public static int InstanceVariable() {
		VariableObject obj = new VariableObject();

		for (int i = 0; i < LoopIterations; i++) {
			obj.InstanceVariableA += obj.InstanceVariableB;
		}

		return obj.InstanceVariableA;
	}
}