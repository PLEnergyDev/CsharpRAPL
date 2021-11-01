using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace ExampleProject.Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class PrimitivesBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	[Benchmark("PrimitiveInteger", "Tests operation on primitive int")]
	public static int PrimitiveInt() {
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
	public static int PrimitiveUint() {
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
	public static int PrimitiveNint() {
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
	public static int PrimitiveNuint() {
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
	public static int PrimitiveLong() {
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
	public static int PrimitiveUlong() {
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
	public static int PrimitiveShort() {
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
	public static int PrimitiveUshort() {
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
	public static int PrimitiveByte() {
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
	public static int PrimitiveSbyte() {
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
	public static int PrimitiveFloat() {
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
	public static int PrimitiveDouble() {
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
	public static int PrimitiveDecimal() {
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