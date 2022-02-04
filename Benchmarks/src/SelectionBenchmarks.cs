using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SelectionBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	[Benchmark("SelectionIf", "Tests if statement")]
	public static ulong If() {
		ulong halfLoopIteration = LoopIterations / 2;
		ulong count = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			if (i < halfLoopIteration) {
				count++;
				continue;
			}

			if (i == halfLoopIteration) {
				count += 10;
				continue;
			}
			
			count--;
			continue;
		}

		return count;
	}

	[Benchmark("SelectionSwitch", "Tests if statement compared to switch")]
	public static ulong IfComparableWithSwitch() {
		ulong count = 1;
		for (ulong i  = 0; i < LoopIterations; i++) {
			if (count == 1) {
				count = 2;
				continue;
			}

			if (count == 2) {
				count = 3;
				continue;
			}

			if (count == 3) {
				count = 4;
				continue;
			}

			if (count == 4) {
				count = 5;
				continue;
			}

			if (count == 5) {
				count = 6;
				continue;
			}

			if (count == 6) {
				count = 7;
				continue;
			}

			if (count == 7) {
				count = 8;
				continue;
			}

			if (count == 8) {
				count = 9;
				continue;
			}

			if (count == 9) {
				count = 10;
				continue;
			}

			count = 1;
		}

		return count;
	}

	[Benchmark("SelectionIf", "Tests if and else statement")]
	public static ulong IfElse() {
		ulong count = 0;
		ulong halfLoopIteration = LoopIterations / 2;
		for (ulong i  = 0; i < LoopIterations; i++) {
			if (i < halfLoopIteration) {
				count++;
				continue;
			}

			if (i == halfLoopIteration) {
				count += 10;
				continue;
			}
			else {
				count--;
				continue;
			}
		}

		return count;
	}

	[Benchmark("SelectionIf", "Tests if and if else statement")]
	public static ulong IfElseIf() //TODO IL code is equivalent with IfElse
	{
		ulong count = 0;
		ulong halfLoopIteration = LoopIterations / 2;
		for (ulong i  = 0; i < LoopIterations; i++) {
			if (i < halfLoopIteration) {
				count++;
				continue;
			}
			else if (i == halfLoopIteration) {
				count += 10;
				continue;
			}
			else {
				count--;
				continue;
			}
		}

		return count;
	}

	[Benchmark("SelectionSwitch", "Tests switch statement")]
	public static ulong Switch() {
		ulong count = 1;
		for (ulong i  = 0; i < LoopIterations; i++) {
			switch (count) {
				case 1:
					count = 2;
					break;
				case 2:
					count = 3;
					break;
				case 3:
					count = 4;
					break;
				case 4:
					count = 5;
					break;
				case 5:
					count = 6;
					break;
				case 6:
					count = 7;
					break;
				case 7:
					count = 8;
					break;
				case 8:
					count = 9;
					break;
				case 9:
					count = 10;
					break;
				default:
					count = 1;
					break;
			}
		}

		return count;
	}

	[Benchmark("SelectionConst", "Switch comparable with If with const number")]
	public static ulong SwitchConstNumber() {
		ulong count = 1;
		const ulong halfLoopIteration = 25000;
		for (ulong i  = 0; i < LoopIterations; i++) {
			switch (i) {
				case < halfLoopIteration:
					count++;
					break;
				case halfLoopIteration:
					count--;
					break;
				case > halfLoopIteration:
					count++;
					break;
			}
		}

		return count;
	}

	[Benchmark("SelectionConst", "Tests if statement with constant number")]
	public static ulong IfConstNumber() {
		const ulong halfLoopIteration = 25000;
		ulong count = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			if (i < halfLoopIteration) {
				count++;
				continue;
			}

			if (i == halfLoopIteration) {
				count += 10;
				continue;
			}

			if (i > halfLoopIteration) {
				count--;
				continue;
			}

			count++; //Will never get here
		}

		return count;
	}

	[Benchmark("SelectionConditional", "Tests if else comparable with conditional operator")]
	public static ulong IfElseComparableWithConditional() {
		ulong count = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			if (i <= LoopIterations) {
				count = 1;
			}
			else {
				count = 2;
			}
		}

		return count;
	}


	[Benchmark("SelectionConditional", "Tests if conditional operator")]
	public static ulong ConditionalOperator() {
		ulong count = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			count = i <= LoopIterations ? 1ul : 2ul;
		}

		return count;
	}
}