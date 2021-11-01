using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace ExampleProject.Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class SelectionBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	[Benchmark("Selection", "Tests if statement")]
	public static int If() {
		int halfLoopIteration = LoopIterations / 2;
		int count = 0;
		for (int i = 0; i < LoopIterations; i++) {
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

			halfLoopIteration++;
		}

		return count;
	}

	[Benchmark("Selection", "Tests if statement compared to switch")]
	public static int IfComparableWithSwitch() {
		int count = 1;
		for (int i = 0; i < LoopIterations; i++) {
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

	[Benchmark("Selection", "Tests if and else statement")]
	public static int IfElse() {
		int count = 0;
		int halfLoopIteration = LoopIterations / 2;
		for (int i = 0; i < LoopIterations; i++) {
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

	[Benchmark("Selection", "Tests if and if else statement")]
	public static int IfElseIf() //TODO IL code is equivalent with IfElse
	{
		int count = 0;
		int halfLoopIteration = LoopIterations / 2;
		for (int i = 0; i < LoopIterations; i++) {
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

	[Benchmark("Selection", "Tests switch statement")]
	public static int Switch() {
		int count = 1;
		for (int i = 0; i < LoopIterations; i++) {
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

	[Benchmark("Selection", "Tests if else comparable with conditional operator")]
	public static int IfElseComparableWithConditional() {
		int count = 0;
		for (int i = 0; i < LoopIterations; i++) {
			if (i <= LoopIterations) {
				count = 1;
			}
			else {
				count = 2;
			}
		}

		return count;
	}


	[Benchmark("Selection", "Tests if conditional operator")]
	public static int ConditionalOperator() {
		int count = 0;
		for (int i = 0; i < LoopIterations; i++) {
			count = i <= LoopIterations ? 1 : 2;
		}

		return count;
	}
}