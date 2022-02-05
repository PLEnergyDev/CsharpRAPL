using System;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class BoxingBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	[Benchmark("BoxedInteger", "Tests operation on boxed int")]
	public static Int32 BInt() {
		Int32 boxed = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			boxed++;
			boxed *= 3;
			boxed /= 2;
			boxed--;
			boxed %= 20;
		}

		return boxed;
	}

	[Benchmark("BoxedInteger", "Tests operation on boxed uint")]
	public static UInt32 BUint() {
		UInt32 boxed = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			boxed++;
			boxed *= 3;
			boxed /= 2;
			boxed--;
			boxed %= 20;
		}

		return boxed;
	}

	[Benchmark("BoxedInteger", "Tests operation on boxed nint")]
	public static nint BNint() {
		nint primitive = 0;
		object boxed = primitive;
		for (ulong i = 0; i < LoopIterations; i++) {
			boxed = (nint)boxed + 1;
			boxed = (nint)boxed * 3;
			boxed = (nint)boxed / 2;
			boxed = (nint)boxed - 1;
			boxed = (nint)boxed % 20;
		}

		return (nint)boxed;
	}

	[Benchmark("BoxedInteger", "Tests operation on boxed nuint")]
	public static nuint BNuint() {
		nuint primitive = 0;
		object boxed = primitive;
		for (ulong i = 0; i < LoopIterations; i++) {
			boxed = (nuint)boxed + 1;
			boxed = (nuint)boxed * 3;
			boxed = (nuint)boxed / 2;
			boxed = (nuint)boxed - 1;
			boxed = (nuint)boxed % 20;
		}

		return (nuint)boxed;
	}

	[Benchmark("BoxedInteger", "Tests operation on boxed long")]
	public static Int64 BLong() {
		Int64 boxed = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			boxed++;
			boxed *= 3;
			boxed /= 2;
			boxed--;
			boxed %= 20;
		}

		return boxed;
	}

	[Benchmark("BoxedInteger", "Tests operation on boxed ulong")]
	public static UInt64 BUlong() {
		UInt64 boxed = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			boxed++;
			boxed *= 3;
			boxed /= 2;
			boxed--;
			boxed %= 20;
		}

		return boxed;
	}

	[Benchmark("BoxedInteger", "Tests operation on boxed short")]
	public static Int16 BShort() {
		Int16 boxed = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			boxed = (short)(boxed + 1);
			boxed = (short)(boxed * 3);
			boxed = (short)(boxed / 2);
			boxed = (short)(boxed - 1);
			boxed = (short)(boxed % 20);
		}

		return boxed;
	}

	[Benchmark("BoxedInteger", "Tests operation on boxed ushort")]
	public static UInt16 BUshort() {
		UInt16 boxed = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			boxed = (ushort)(boxed + 1);
			boxed = (ushort)(boxed * 3);
			boxed = (ushort)(boxed / 2);
			boxed = (ushort)(boxed - 1);
			boxed = (ushort)(boxed % 20);
		}

		return boxed;
	}

	[Benchmark("BoxedInteger", "Tests operation on boxed byte")]
	public static Byte BByte() {
		Byte boxed = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			boxed = (byte)(boxed + 1);
			boxed = (byte)(boxed * 3);
			boxed = (byte)(boxed / 2);
			boxed = (byte)(boxed - 1);
			boxed = (byte)(boxed % 20);
		}

		return boxed;
	}

	[Benchmark("BoxedInteger", "Tests operation on boxed sbyte")]
	public static SByte BSbyte() {
		SByte boxed = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			boxed = (sbyte)(boxed + 1);
			boxed = (sbyte)(boxed * 3);
			boxed = (sbyte)(boxed / 2);
			boxed = (sbyte)(boxed - 1);
			boxed = (sbyte)(boxed % 20);
		}

		return boxed;
	}

	[Benchmark("BoxedDecimal", "Tests operation on boxed float")]
	public static Single BFloat() {
		Single boxed = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			boxed++;
			boxed *= 3;
			boxed /= 2;
			boxed--;
			boxed %= 20;
		}

		return boxed;
	}

	[Benchmark("BoxedDecimal", "Tests operation on boxed double")]
	public static Double BDouble() {
		Double boxed = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			boxed++;
			boxed *= 3;
			boxed /= 2;
			boxed--;
			boxed %= 20;
		}

		return boxed;
	}

	[Benchmark("BoxedDecimal", "Tests operation on boxed decimal")]
	public static Decimal BDecimal() {
		Decimal boxed = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			boxed++;
			boxed *= 3;
			boxed /= 2;
			boxed--;
			boxed %= 20;
		}

		return boxed;
	}

	[Benchmark("BoxedBool", "Tests setting bool values")]
	public static Boolean BBool() {
		Boolean boxed = false;
		for (ulong i = 0; i < LoopIterations; i++) {
			if (boxed) {
				boxed = false;
			}
			else {
				boxed = true;
			}
		}

		return boxed;
	}

	[Benchmark("BoxedBool", "Tests using byte as boolean values")]
	public static Byte BByteAsBool() {
		Byte boxed = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			if (boxed != 0) {
				boxed = 0;
			}
			else {
				boxed = 1;
			}
		}

		return boxed;
	}
}