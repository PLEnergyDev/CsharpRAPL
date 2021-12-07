using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class FieldMutabilityBenchmarks {
	public static int Iterations;
	public static int LoopIterations;
	public static readonly FieldMutabilityHelper FieldMutabilityHelper = new();


	[Benchmark("Field Mutability", "Tests getting a value from a field.")]
	public static int FieldGet() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += FieldMutabilityHelper.Field + 1 + i;
		}

		return result;
	}

	[Benchmark("Field Mutability", "Tests getting a value from a const field.")]
	public static int ConstFieldGet() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += FieldMutabilityHelper.ConstField + 1 + i;
		}

		return result;
	}

	[Benchmark("Field Mutability", "Tests getting a value from a readonly field.")]
	public static int ReadonlyFieldGet() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += FieldMutabilityHelper.ReadonlyField + 1 + i;
		}

		return result;
	}

	[Benchmark("Field Mutability", "Tests getting a value from a property.")]
	public static int PropertyGet() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += FieldMutabilityHelper.Property + 1 + i;
		}

		return result;
	}

	[Benchmark("Field Mutability", "Tests getting a value from a init property.")]
	public static int InitPropertyGet() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += FieldMutabilityHelper.InitProperty + 1 + i;
		}

		return result;
	}

	[Benchmark("Field Mutability", "Tests getting a value from a get only property.")]
	public static int GetOnlyPropertyGet() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += FieldMutabilityHelper.GetProperty + 1 + i;
		}

		return result;
	}

	[Benchmark("Field Mutability", "Tests getting a value from a computed property.")]
	public static int ComputePropertyGet() {
		int result = 0;

		for (int i = 0; i < LoopIterations; i++) {
			result += FieldMutabilityHelper.ComputedProperty + 1 + i;
		}

		return result;
	}
}