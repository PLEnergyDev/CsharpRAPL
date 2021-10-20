using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;
using ExampleProject.HelperObjects;

namespace ExampleProject.Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class VariablesBenchmark {
	public static int Iterations;
	public static int LoopIterations;

	[Benchmark("Variables", "Tests local variables")]
	public static int LocalVariable() {
		int localA = 0, localB = 1;
		for (var i = 0; i < LoopIterations; i++) {
			localA += localB;
		}

		return localA + localB;
	}

	[Benchmark("Variables", "Tests static variables")]
	public static int StaticVariable() {
		for (var i = 0; i < LoopIterations; i++) {
			VariableObject.StaticA += VariableObject.StaticB;
		}

		return VariableObject.StaticA + VariableObject.StaticB;
	}

	[Benchmark("Variables", "Tests instance variables")]
	public static int InstanceVariable() {
		var obj = new VariableObject();

		for (var i = 0; i < LoopIterations; i++) {
			obj.LocalA += obj.LocalB;
		}

		return obj.LocalA + obj.LocalB;
	}
}