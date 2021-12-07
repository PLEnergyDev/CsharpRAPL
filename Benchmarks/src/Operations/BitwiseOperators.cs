using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Operations;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class BitwiseOperators {
	public static int Iterations;
	public static int LoopIterations;

	//<< >> & | <<= >>= |= &= ^ ^= ~


	[Benchmark("BitwiseOperators", "Tests bit shift left using result = result >> 10 + 1 + i")]
	public static int BitShiftLeft() {
		int result = 10;
		for (int i = 0; i < LoopIterations; i++) {
			result = result >> 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests bit shift left compound using result >>= 10 + 1 + i")]
	public static int BitShiftLeftCompound() {
		int result = 10;
		for (int i = 0; i < LoopIterations; i++) {
			result >>= 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests bit shift right using result = result << 10 + 1 + i")]
	public static int BitShiftRight() {
		int result = 10;
		for (int i = 0; i < LoopIterations; i++) {
			result = result << 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests bit shift right compound using result <<= 10 + 1 + i")]
	public static int BitShiftRightCompound() {
		int result = 10;
		for (int i = 0; i < LoopIterations; i++) {
			result <<= 10 + 1 + i;
		}

		return result;
	}


	[Benchmark("BitwiseOperators", "Tests logical AND using result = result & 10 + 1 + i")]
	public static int LogicalAND() {
		int result = 10;
		for (int i = 0; i < LoopIterations; i++) {
			result = result & 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests logical AND compound using result &= 10 + 1 + i")]
	public static int LogicalANDCompound() {
		int result = 10;
		for (int i = 0; i < LoopIterations; i++) {
			result &= 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests logical OR using result = result | 10 + 1 + i")]
	public static int LogicalOR() {
		int result = 10;
		for (int i = 0; i < LoopIterations; i++) {
			result = result | 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests logical OR compound using result |= 10 + 1 + i")]
	public static int LogicalORCompound() {
		int result = 10;
		for (int i = 0; i < LoopIterations; i++) {
			result |= 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests logical XOR using result = result ^ 10 + 1 + i")]
	public static int LogicalXOR() {
		int result = 10;
		for (int i = 0; i < LoopIterations; i++) {
			result = result ^ 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests logical XOR compound using result ^= 10 + 1 + i")]
	public static int LogicalXORCompound() {
		int result = 10;
		for (int i = 0; i < LoopIterations; i++) {
			result ^= 10 + 1 + i;
		}

		return result;
	}

	[Benchmark("BitwiseOperators", "Tests bitwise complement using result = ~result + 1 + i")]
	public static int BitwiseComplement() {
		int result = 10;
		for (int i = 0; i < LoopIterations; i++) {
			result = ~result + 1 + i;
		}

		return result;
	}
}