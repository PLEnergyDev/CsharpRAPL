using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects.Objects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedVariable")]
#pragma warning disable CS0219
public class ObjectsBenchmarks {
	public static int Iterations;
	public static int LoopIterations;


	[Benchmark("ObjectCreation", "Tests creating a class")]
	public static int ClassCreate() {
		int result = 0;


		for (int i = 0; i < LoopIterations; i++) {
			result += new ClassHelper().Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a field on a class")]
	public static int ClassField() {
		int result = 0;
		ClassHelper classObject = new ClassHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += classObject.Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a static field on a class")]
	public static int ClassFieldStatic() {
		int result = 0;
		ClassHelper unused = new ClassHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += ClassHelper.StaticField + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a method on a class")]
	public static int ClassMethod() {
		int result = 0;
		ClassHelper classObject = new ClassHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += classObject.Calculate() + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a static method on a class")]
	public static int ClassMethodStatic() {
		int result = 0;
		ClassHelper unused = new ClassHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += ClassHelper.CalculateStatic() + i;
		}

		return result;
	}

	[Benchmark("ObjectCreation", "Tests creating a struct")]
	public static int StructCreate() {
		int result = 0;


		for (int i = 0; i < LoopIterations; i++) {
			result += new StructHelper().Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a field on a struct")]
	public static int StructField() {
		int result = 0;
		StructHelper structObject = new StructHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += structObject.Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a static field on a struct")]
	public static int StructFieldStatic() {
		int result = 0;
		StructHelper unused = new StructHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += StructHelper.StaticField + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a method on a struct")]
	public static int StructMethod() {
		int result = 0;
		StructHelper structObject = new StructHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += structObject.Calculate() + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a static method on a struct")]
	public static int StructMethodStatic() {
		int result = 0;
		StructHelper unused = new StructHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += StructHelper.CalculateStatic() + i;
		}

		return result;
	}

	[Benchmark("ObjectCreation", "Tests creating a record")]
	public static int RecordCreate() {
		int result = 0;


		for (int i = 0; i < LoopIterations; i++) {
			result += new RecordHelper().Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a field on a record")]
	public static int RecordField() {
		int result = 0;
		RecordHelper recordObject = new RecordHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += recordObject.Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a static field on a record")]
	public static int RecordFieldStatic() {
		int result = 0;
		RecordHelper unused = new RecordHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += RecordHelper.StaticField + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a method on a record")]
	public static int RecordMethod() {
		int result = 0;
		RecordHelper recordObject = new RecordHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += recordObject.Calculate() + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a static method on a record struct")]
	public static int RecordMethodStatic() {
		int result = 0;
		RecordHelper unused = new RecordHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += RecordHelper.CalculateStatic() + i;
		}

		return result;
	}

	[Benchmark("ObjectCreation", "Tests creating a record struct")]
	public static int RecordStructCreate() {
		int result = 0;


		for (int i = 0; i < LoopIterations; i++) {
			result += new RecordStructHelper().Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a field on a record struct")]
	public static int RecordStructField() {
		int result = 0;
		RecordStructHelper recordObject = new RecordStructHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += recordObject.Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a static field on a record struct")]
	public static int RecordStructFieldStatic() {
		int result = 0;
		RecordStructHelper unused = new RecordStructHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += RecordStructHelper.StaticField + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a method on a record struct")]
	public static int RecordStructMethod() {
		int result = 0;
		RecordStructHelper recordObject = new RecordStructHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += recordObject.Calculate() + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a static method on a record struct")]
	public static int RecordStructStaticMethod() {
		int result = 0;
		RecordStructHelper unused = new RecordStructHelper();

		for (int i = 0; i < LoopIterations; i++) {
			result += RecordStructHelper.CalculateStatic() + i;
		}

		return result;
	}
}
#pragma warning restore CS0219