using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects.Objects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedVariable")]
#pragma warning disable CS0219
public class ObjectsBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	[Benchmark("ObjectCreation", "Tests creating a class")]
	public static ulong ClassCreate() {
		ulong result = 0;


		for (ulong i = 0; i < LoopIterations; i++) {
			result += new ClassHelper().Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a field on a class")]
	public static ulong ClassField() {
		ulong result = 0;
		ClassHelper classObject = new ClassHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += classObject.Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a static field on a class")]
	public static ulong ClassFieldStatic() {
		ulong result = 0;
		ClassHelper unused = new ClassHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += ClassHelper.StaticField + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a method on a class")]
	public static ulong ClassMethod() {
		ulong result = 0;
		ClassHelper classObject = new ClassHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += classObject.Calculate() + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a static method on a class")]
	public static ulong ClassMethodStatic() {
		ulong result = 0;
		ClassHelper unused = new ClassHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += ClassHelper.CalculateStatic() + i;
		}

		return result;
	}

	[Benchmark("ObjectCreation", "Tests creating a struct")]
	public static ulong StructCreate() {
		ulong result = 0;


		for (ulong i = 0; i < LoopIterations; i++) {
			result += new StructHelper().Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a field on a struct")]
	public static ulong StructField() {
		ulong result = 0;
		StructHelper structObject = new StructHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += structObject.Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a static field on a struct")]
	public static ulong StructFieldStatic() {
		ulong result = 0;
		StructHelper unused = new StructHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += StructHelper.StaticField + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a method on a struct")]
	public static ulong StructMethod() {
		ulong result = 0;
		StructHelper structObject = new StructHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += structObject.Calculate() + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a static method on a struct")]
	public static ulong StructMethodStatic() {
		ulong result = 0;
		StructHelper unused = new StructHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += StructHelper.CalculateStatic() + i;
		}

		return result;
	}

	[Benchmark("ObjectCreation", "Tests creating a record")]
	public static ulong RecordCreate() {
		ulong result = 0;


		for (ulong i = 0; i < LoopIterations; i++) {
			result += new RecordHelper().Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a field on a record")]
	public static ulong RecordField() {
		ulong result = 0;
		RecordHelper recordObject = new RecordHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += recordObject.Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a static field on a record")]
	public static ulong RecordFieldStatic() {
		ulong result = 0;
		RecordHelper unused = new RecordHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += RecordHelper.StaticField + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a method on a record")]
	public static ulong RecordMethod() {
		ulong result = 0;
		RecordHelper recordObject = new RecordHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += recordObject.Calculate() + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a static method on a record struct")]
	public static ulong RecordMethodStatic() {
		ulong result = 0;
		RecordHelper unused = new RecordHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += RecordHelper.CalculateStatic() + i;
		}

		return result;
	}

	[Benchmark("ObjectCreation", "Tests creating a record struct")]
	public static ulong RecordStructCreate() {
		ulong result = 0;


		for (ulong i = 0; i < LoopIterations; i++) {
			result += new RecordStructHelper().Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a field on a record struct")]
	public static ulong RecordStructField() {
		ulong result = 0;
		RecordStructHelper recordObject = new RecordStructHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += recordObject.Field + i;
		}

		return result;
	}

	[Benchmark("ObjectFieldAccess", "Tests accessing a static field on a record struct")]
	public static ulong RecordStructFieldStatic() {
		ulong result = 0;
		RecordStructHelper unused = new RecordStructHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += RecordStructHelper.StaticField + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a method on a record struct")]
	public static ulong RecordStructMethod() {
		ulong result = 0;
		RecordStructHelper recordObject = new RecordStructHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += recordObject.Calculate() + i;
		}

		return result;
	}

	[Benchmark("ObjectInvocation", "Tests invocation of a static method on a record struct")]
	public static ulong RecordStructStaticMethod() {
		ulong result = 0;
		RecordStructHelper unused = new RecordStructHelper();

		for (ulong i = 0; i < LoopIterations; i++) {
			result += RecordStructHelper.CalculateStatic() + i;
		}

		return result;
	}
}
#pragma warning restore CS0219