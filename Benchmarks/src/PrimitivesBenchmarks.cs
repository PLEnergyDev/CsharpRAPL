using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PrimitivesBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	[Benchmark("PrimitiveInteger", "Tests operation on primitive int")]
	public static int Int() {
		int primitive = 0;
		for (int i = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 10;
			primitive /= 2;
			primitive--;
			primitive = 0;
		}

		return primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive uint")]
	public static int Uint() {
		uint primitive = 0;
		for (int i = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 10;
			primitive /= 2;
			primitive--;
			primitive = 0;
		}

		return (int)primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive nint")]
	public static int Nint() {
		nint primitive = 0;
		for (int i = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 10;
			primitive /= 2;
			primitive--;
			primitive = 0;
		}

		return (int)primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive nuint")]
	public static int Nuint() {
		nuint primitive = 0;
		for (int i = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 10;
			primitive /= 2;
			primitive--;
			primitive = 0;
		}

		return (int)primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive long")]
	public static int Long() {
		long primitive = 0;
		for (int i = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 10;
			primitive /= 2;
			primitive--;
			primitive = 0;
		}

		return (int)primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive ulong")]
	public static int Ulong() {
		ulong primitive = 0;
		for (int i = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 10;
			primitive /= 2;
			primitive--;
			primitive = 0;
		}

		return (int)primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive short")]
	public static int Short() {
		short primitive = 0;
		for (int i = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 10;
			primitive /= 2;
			primitive--;
			primitive = 0;
		}

		return primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive ushort")]
	public static int Ushort() {
		ushort primitive = 0;
		for (int i = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 10;
			primitive /= 2;
			primitive--;
			primitive = 0;
		}

		return primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive byte")]
	public static int Byte() {
		byte primitive = 0;
		for (int i = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 10;
			primitive /= 2;
			primitive--;
			primitive = 0;
		}

		return primitive;
	}

	[Benchmark("PrimitiveInteger", "Tests operation on primitive sbyte")]
	public static int Sbyte() {
		sbyte primitive = 0;
		for (int i = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 10;
			primitive /= 2;
			primitive--;
			primitive = 0;
		}

		return primitive;
	}

	[Benchmark("PrimitiveDecimal", "Tests operation on primitive float")]
	public static int Float() {
		float primitive = 0;
		for (int i = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 10;
			primitive /= 2;
			primitive--;
			primitive = 0;
		}

		return (int)primitive;
	}

	[Benchmark("PrimitiveDecimal", "Tests operation on primitive double")]
	public static int Double() {
		double primitive = 0;
		for (int i = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 10;
			primitive /= 2;
			primitive--;
			primitive = 0;
		}

		return (int)primitive;
	}

	[Benchmark("PrimitiveDecimal", "Tests operation on primitive decimal")]
	public static int Decimal() {
		decimal primitive = 0;
		for (int i = 0; i < LoopIterations; i++) {
			primitive++;
			primitive *= 10;
			primitive /= 2;
			primitive--;
			primitive = 0;
		}

		return (int)primitive;
	}

	[Benchmark("PrimitiveBool", "Tests setting bool values")]
	public static bool Bool() {
		bool myBool = false;
		for (int i = 0; i < LoopIterations; i++) {
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
		for (int i = 0; i < LoopIterations; i++) {
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