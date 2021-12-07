using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class GetterSetterBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	[Benchmark("GetterSetter", "Tests incrementing a property")]
	public static int Property() {
		GetterSetterHelper helper = new GetterSetterHelper();
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			helper.Property++;
		}

		return result;
	}

	[Benchmark("GetterSetterGet", "Tests getting a property")]
	public static int PropertyGet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			result += helper.Property + 2;
		}

		return result;
	}

	[Benchmark("GetterSetterSet", "Tests setting a property")]
	public static int PropertySet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			helper.Property = i + 2;
		}

		return result;
	}

	[Benchmark("GetterSetter", "Tests incrementing a property with a backing field")]
	public static int PropertyWithBackingField() {
		GetterSetterHelper helper = new GetterSetterHelper();
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			helper.PropertyWithBackingField++;
		}

		return result;
	}

	[Benchmark("GetterSetterGet", "Tests getting a property with a backing field")]
	public static int PropertyWithBackingFieldGet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			result += helper.PropertyWithBackingField + 2;
		}

		return result;
	}

	[Benchmark("GetterSetterSet", "Tests setting a property with a backing field")]
	public static int PropertyWithBackingFieldSet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			helper.PropertyWithBackingField = i + 2;
		}

		return result;
	}

	[Benchmark("GetterSetter", "Tests incrementing value using both getter and setter methods")]
	public static int GetterSetter() {
		GetterSetterHelper helper = new GetterSetterHelper();
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			helper.SetValue(helper.GetValue() + 1);
		}

		return result;
	}

	[Benchmark("GetterSetterGet", "Tests getting a using a getter method")]
	public static int GetterSetterGet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			result += helper.GetValue() + 2;
		}

		return result;
	}

	[Benchmark("GetterSetterSet", "Tests setting a value using a setter method")]
	public static int GetterSetterSet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			helper.SetValue(i + 2);
		}

		return result;
	}

	[Benchmark("GetterSetter", "Tests incrementing a field")]
	public static int Field() {
		GetterSetterHelper helper = new GetterSetterHelper();
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			helper.Field++;
		}

		return result;
	}

	[Benchmark("GetterSetterGet", "Tests getting a field")]
	public static int FieldGet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			result += helper.Field + 2;
		}

		return result;
	}

	[Benchmark("GetterSetterSet", "Tests setting a field")]
	public static int FieldSet() {
		GetterSetterHelper helper = new GetterSetterHelper();
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			helper.Field = i + 2;
		}

		return result;
	}
}