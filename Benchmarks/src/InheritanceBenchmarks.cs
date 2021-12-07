using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects.Inheritance;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class InheritanceBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	[Benchmark("Inheritance", "Tests getting and updating a value with no inheritance")]
	public static int NoInheritance() {
		int result = 0;
		ClassHelper helper = new ClassHelper();
		for (int i = 0; i < LoopIterations; i++) {
			result += helper.UpdateAndGetValue();
		}

		return result;
	}

	[Benchmark("Inheritance", "Tests getting and updating a value with inheritance")]
	public static int Inheritance() {
		int result = 0;
		ClassHelper helper = new InheritanceHelper();
		for (int i = 0; i < LoopIterations; i++) {
			result += helper.UpdateAndGetValue();
		}

		return result;
	}

	[Benchmark("Inheritance", "Tests getting and updating a value using an polymorphism with an abstract class")]
	public static int Abstract() {
		int result = 0;
		ADoable helper = new AbstractHelper();
		for (int i = 0; i < LoopIterations; i++) {
			result += helper.UpdateAndGetValue();
		}

		return result;
	}

	[Benchmark("Inheritance", "Tests getting and updating a value using an polymorphism with a interface")]
	public static int Interface() {
		int result = 0;
		IDoable helper = new InterfaceHelper();
		for (int i = 0; i < LoopIterations; i++) {
			result += helper.UpdateAndGetValue();
		}

		return result;
	}

	[Benchmark("Inheritance", "Tests getting and updating a value using a virtual method")]
	public static int InheritanceVirtual() {
		int result = 0;
		VirtualHelper helper = new VirtualHelper();
		for (int i = 0; i < LoopIterations; i++) {
			result += helper.UpdateAndGetValue();
		}

		return result;
	}

	[Benchmark("Inheritance", "Tests getting and updating a value using a overriden virtual method")]
	public static int InheritanceVirtualOverride() {
		int result = 0;
		VirtualHelper helper = new OverrideHelper();
		for (int i = 0; i < LoopIterations; i++) {
			result += helper.UpdateAndGetValue();
		}

		return result;
	}
}