using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects.Inheritance;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class InheritanceBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	[Benchmark("Inheritance", "Tests getting and updating a value with no inheritance")]
	public static ulong NoInheritance() {
		ulong result = 0;
		ClassHelper helper = new ClassHelper();
		for (ulong i = 0; i < LoopIterations; i++) {
			result += helper.UpdateAndGetValue();
		}

		return result;
	}

	[Benchmark("Inheritance", "Tests getting and updating a value with inheritance")]
	public static ulong Inheritance() {
		ulong result = 0;
		ClassHelper helper = new InheritanceHelper();
		for (ulong i = 0; i < LoopIterations; i++) {
			result += helper.UpdateAndGetValue();
		}

		return result;
	}

	[Benchmark("Inheritance", "Tests getting and updating a value using an polymorphism with an abstract class")]
	public static ulong Abstract() {
		ulong result = 0;
		ADoable helper = new AbstractHelper();
		for (ulong i = 0; i < LoopIterations; i++) {
			result += helper.UpdateAndGetValue();
		}

		return result;
	}

	[Benchmark("Inheritance", "Tests getting and updating a value using an polymorphism with a interface")]
	public static ulong Interface() {
		ulong result = 0;
		IDoable helper = new InterfaceHelper();
		for (ulong i = 0; i < LoopIterations; i++) {
			result += helper.UpdateAndGetValue();
		}

		return result;
	}

	[Benchmark("Inheritance", "Tests getting and updating a value using a virtual method")]
	public static ulong InheritanceVirtual() {
		ulong result = 0;
		VirtualHelper helper = new VirtualHelper();
		for (ulong i = 0; i < LoopIterations; i++) {
			result += helper.UpdateAndGetValue();
		}

		return result;
	}

	[Benchmark("Inheritance", "Tests getting and updating a value using a overriden virtual method")]
	public static ulong InheritanceVirtualOverride() {
		ulong result = 0;
		VirtualHelper helper = new OverrideHelper();
		for (ulong i = 0; i < LoopIterations; i++) {
			result += helper.UpdateAndGetValue();
		}

		return result;
	}
}