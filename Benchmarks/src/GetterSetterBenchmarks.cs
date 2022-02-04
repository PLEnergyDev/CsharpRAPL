using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class GetterSetterBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	[Benchmark("GetterSetter", "Tests incrementing a property")]
	public static ulong Property() {
		GetterSetterHelper helper = new GetterSetterHelper();
		ulong result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			helper.Property++;
		}

		return result;
	}

	[Benchmark("GetterSetterGet", "Tests getting a property")]
	public static ulong PropertyGet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		ulong result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			result += helper.Property + 2;
		}

		return result;
	}

	[Benchmark("GetterSetterSet", "Tests setting a property")]
	public static ulong PropertySet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		ulong result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			helper.Property = i + 2;
		}

		return result;
	}

	[Benchmark("GetterSetter", "Tests incrementing a property with a backing field")]
	public static ulong PropertyWithBackingField() {
		GetterSetterHelper helper = new GetterSetterHelper();
		ulong result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			helper.PropertyWithBackingField++;
		}

		return result;
	}

	[Benchmark("GetterSetterGet", "Tests getting a property with a backing field")]
	public static ulong PropertyWithBackingFieldGet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		ulong result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			result += helper.PropertyWithBackingField + 2;
		}

		return result;
	}

	[Benchmark("GetterSetterSet", "Tests setting a property with a backing field")]
	public static ulong PropertyWithBackingFieldSet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		ulong result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			helper.PropertyWithBackingField = i + 2;
		}

		return result;
	}

	[Benchmark("GetterSetter", "Tests incrementing value using both getter and setter methods")]
	public static ulong GetterSetter() {
		GetterSetterHelper helper = new GetterSetterHelper();
		ulong result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			helper.SetValue(helper.GetValue() + 1);
		}

		return result;
	}

	[Benchmark("GetterSetterGet", "Tests getting a using a getter method")]
	public static ulong GetterSetterGet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		ulong result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			result += helper.GetValue() + 2;
		}

		return result;
	}

	[Benchmark("GetterSetterSet", "Tests setting a value using a setter method")]
	public static ulong GetterSetterSet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		ulong result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			helper.SetValue(i + 2);
		}

		return result;
	}

	[Benchmark("GetterSetter", "Tests incrementing a field")]
	public static ulong Field() {
		GetterSetterHelper helper = new GetterSetterHelper();
		ulong result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			helper.Field++;
		}

		return result;
	}

	[Benchmark("GetterSetterGet", "Tests getting a field")]
	public static ulong FieldGet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		ulong result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			result += helper.Field + 2;
		}

		return result;
	}

	[Benchmark("GetterSetterSet", "Tests setting a field")]
	public static ulong FieldSet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		ulong result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			helper.Field = i + 2;
		}

		return result;
	}
}