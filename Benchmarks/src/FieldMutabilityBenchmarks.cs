using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class FieldMutabilityBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;

	public static readonly FieldMutabilityHelper FieldMutabilityHelper = new();


	[Benchmark("Field Mutability", "Tests getting a value from a field.")]
	public static ulong FieldGet() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += FieldMutabilityHelper.Field + 1 + i;
		}

		return result;
	}

	[Benchmark("Field Mutability", "Tests getting a value from a const field.")]
	public static ulong ConstFieldGet() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += FieldMutabilityHelper.ConstField + 1 + i;
		}

		return result;
	}

	[Benchmark("Field Mutability", "Tests getting a value from a readonly field.")]
	public static ulong ReadonlyFieldGet() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += FieldMutabilityHelper.ReadonlyField + 1 + i;
		}

		return result;
	}

	[Benchmark("Field Mutability", "Tests getting a value from a property.")]
	public static ulong PropertyGet() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += FieldMutabilityHelper.Property + 1 + i;
		}

		return result;
	}

	[Benchmark("Field Mutability", "Tests getting a value from a init property.")]
	public static ulong InitPropertyGet() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += FieldMutabilityHelper.InitProperty + 1 + i;
		}

		return result;
	}

	[Benchmark("Field Mutability", "Tests getting a value from a get only property.")]
	public static ulong GetOnlyPropertyGet() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += FieldMutabilityHelper.GetProperty + 1 + i;
		}

		return result;
	}

	[Benchmark("Field Mutability", "Tests getting a value from a computed property.")]
	public static ulong ComputePropertyGet() {
		ulong result = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			result += FieldMutabilityHelper.ComputedProperty + 1 + i;
		}

		return result;
	}
}