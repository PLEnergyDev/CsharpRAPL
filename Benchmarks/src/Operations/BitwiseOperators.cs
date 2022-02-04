using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Operations;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class BitwiseOperators {
	public static ulong Iterations;
	public static ulong LoopIterations;


	//<< >> & | <<= >>= |= &= ^ ^= ~


	[Benchmark("BitwiseOperators", "Tests bit shift left using result = result >> 10 + 1 + i")]
	public static int BitShiftLeft() {
		int result = 10;
		int iter = (int)LoopIterations;
		for (int i = 0; i < iter; i++) {
			result = result >> 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests bit shift left compound using result >>= 10 + 1 + i")]
	public static int BitShiftLeftCompound() {
		int result = 10;
		int iter = (int)LoopIterations;
		for (int i = 0; i < iter; i++) {
			result >>= 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests bit shift right using result = result << 10 + 1 + i")]
	public static int BitShiftRight() {
		int result = 10;
		int iter = (int)LoopIterations;
		for (int i = 0; i < iter; i++) {
			result = result << 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests bit shift right compound using result <<= 10 + 1 + i")]
	public static int BitShiftRightCompound() {
		int result = 10;
		int iter = (int)LoopIterations;
		for (int i = 0; i < iter; i++) {
			result <<= 10 + 1 + i;
		}

		return result;
	}


	[Benchmark("BitwiseOperators", "Tests logical AND using result = result & 10 + 1 + i")]
	public static ulong LogicalAND() {
		ulong result = 10;
		for (ulong i = 0; i < LoopIterations; i++) {
			result = result & 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests logical AND compound using result &= 10 + 1 + i")]
	public static ulong LogicalANDCompound() {
		ulong result = 10;
		for (ulong i = 0; i < LoopIterations; i++) {
			result &= 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests logical OR using result = result | 10 + 1 + i")]
	public static ulong LogicalOR() {
		ulong result = 10;
		for (ulong i = 0; i < LoopIterations; i++) {
			result = result | 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests logical OR compound using result |= 10 + 1 + i")]
	public static ulong LogicalORCompound() {
		ulong result = 10;
		for (ulong i = 0; i < LoopIterations; i++) {
			result |= 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests logical XOR using result = result ^ 10 + 1 + i")]
	public static ulong LogicalXOR() {
		ulong result = 10;
		for (ulong i = 0; i < LoopIterations; i++) {
			result = result ^ 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests logical XOR compound using result ^= 10 + 1 + i")]
	public static ulong LogicalXORCompound() {
		ulong result = 10;
		for (ulong i = 0; i < LoopIterations; i++) {
			result ^= 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests bitwise complement using result = ~result + 1 + i")]
	public static ulong BitwiseComplement() {
		ulong result = 10;
		for (ulong i = 0; i < LoopIterations; i++) {
			result = ~result + 1 + i;
		}

		return result;
	}
}