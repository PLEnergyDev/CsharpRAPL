using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PrimitivesBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	[Benchmark("PrimitiveInteger", "Tests operation on primitive int")]
	public static int Int() {
		int primitive = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 3;
			primitive /= 2;
			primitive--;
			primitive %= 20;
		}

		return primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive uint")]
	public static uint Uint() {
		uint primitive = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 3;
			primitive /= 2;
			primitive--;
			primitive %= 20;
		}

		return primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive nint")]
	public static nint Nint() {
		nint primitive = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 3;
			primitive /= 2;
			primitive--;
			primitive %= 20;
		}

		return primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive nuint")]
	public static nuint Nuint() {
		nuint primitive = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 3;
			primitive /= 2;
			primitive--;
			primitive %= 20;
		}

		return primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive long")]
	public static long Long() {
		long primitive = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 3;
			primitive /= 2;
			primitive--;
			primitive %= 20;
		}

		return primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive ulong")]
	public static ulong Ulong() {
		ulong primitive = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 3;
			primitive /= 2;
			primitive--;
			primitive %= 20;
		}

		return primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive short")]
	public static short Short() {
		short primitive = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 3;
			primitive /= 2;
			primitive--;
			primitive %= 20;
		}

		return primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive ushort")]
	public static ushort Ushort() {
		ushort primitive = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 3;
			primitive /= 2;
			primitive--;
			primitive %= 20;
		}

		return primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive byte")]
	public static byte Byte() {
		byte primitive = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 3;
			primitive /= 2;
			primitive--;
			primitive %= 20;
		}

		return primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive sbyte")]
	public static sbyte Sbyte() {
		sbyte primitive = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 3;
			primitive /= 2;
			primitive--;
			primitive %= 20;
		}

		return primitive;
	}

	[Benchmark("PrimitiveDecimal", "Tests operation on primitive float")]
	public static float Float() {
		float primitive = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 3;
			primitive /= 2;
			primitive--;
			primitive %= 20;
		}

		return primitive;
	}

	[Benchmark("PrimitiveDecimal", "Tests operation on primitive double")]
	public static double Double() {
		double primitive = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 3;
			primitive /= 2;
			primitive--;
			primitive %= 20;
		}

		return primitive;
	}

	[Benchmark("PrimitiveDecimal", "Tests operation on primitive decimal")]
	public static decimal Decimal() {
		decimal primitive = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 3;
			primitive /= 2;
			primitive--;
			primitive %= 20;
		}

		return primitive;
	}

	[Benchmark("PrimitiveBool", "Tests setting bool values")]
	public static bool Bool() {
		bool myBool = false;
		for (ulong i  = 0; i < LoopIterations; i++) {
			if (myBool) {
				myBool = false;
			}
			else {
				myBool = true;
			}
		}

		return myBool;
	}

	[Benchmark("PrimitiveBool", "Tests using byte as boolean values")]
	public static byte ByteAsBool() {
		byte myBool = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			if (myBool != 0) {
				myBool = 0;
			}
			else {
				myBool = 1;
			}
		}

		return myBool;
	}
}