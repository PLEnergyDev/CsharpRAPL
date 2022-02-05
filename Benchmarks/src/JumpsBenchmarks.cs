using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class JumpsBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	//Return
	//Throw

	[Benchmark("Jump", "Tests breaking out of a loop")]
	public static ulong Break() {
		ulong count = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			count += 1 + i;
			for (;;) {
				if (count % 1 == 0) {
					break;
				}

				count += 1 + i;
			}
		}

		return count;
	}

	[Benchmark("Jump", "Tests continuing in a loop")]
	public static ulong Continue() {
		ulong count = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			count += 1 + i;
			if (count % 1 == 0) {
				continue;
			}

			count += 1 + i;
		}

		return count;
	}

	[Benchmark("Jump", "Tests using goto in a loop")]
	public static ulong Goto() {
		ulong count = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			count += 1 + i;
			if (count % 1 == 0) {
				goto Iterate;
			}

			count += 1 + i;
			Iterate: ;
		}

		return count;
	}

	[Benchmark("ReturnJump", "Tests using return to return")]
	public static ulong Return() {
		ulong result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			result += ReturnHelper() + i;
		}

		return result;
	}

	public static ulong ReturnHelper() {
		return 4;
	}

	[Benchmark("ReturnJump", "Tests using Exception to return")]
	public static ulong Throw() {
		ulong result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			try {
				ThrowHelper();
			}
			catch (JumpHelperException e) {
				result += e.Number + i;
			}
		}

		return result;
	}

	public static ulong ThrowHelper() {
		throw new JumpHelperException();
	}
}