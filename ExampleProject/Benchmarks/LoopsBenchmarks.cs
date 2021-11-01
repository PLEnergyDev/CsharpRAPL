using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CsharpRAPL.Benchmarking;

namespace ExampleProject.Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class LoopsBenchmarks {
	public static int Iterations;
	public static int LoopIterations;


	[Benchmark("Loops", "Tests a do-while loop")]
	public static int DoWhileLoop() {
		int count = 0;

		do {
			count += 1;
		} while (count < LoopIterations);

		return count;
	}

	[Benchmark("Loops", "Tests a while loop")]
	public static int WhileLoop() {
		int count = 0;

		while (count < LoopIterations) {
			count += 1;
		}

		return count;
	}

	[Benchmark("Loops", "Tests a for loop")]
	public static int ForLoop() {
		int count = 0;

		for (int i = 0; i < LoopIterations; i++) {
			count += 1;
		}


		return count;
	}

	[Benchmark("Loops", "Tests a foreach loop")]
	public static int ForEachLoop() {
		int count = 0;

		foreach (int unused in Enumerable.Range(0, LoopIterations)) {
			count += 1;
		}

		return count;
	}

	[Benchmark("Loops", "Tests a goto based loop, in style of do-while loop")]
	public static int GotoDoWhileLoop() {
		int count = 0;


		Iterate:
		count += 1;
		if (count < LoopIterations) {
			goto Iterate;
		}

		return count;
	}

	[Benchmark("Loops", "Tests a goto based loop in style of while loop")]
	public static int GotoWhileLoop() {
		int count = 0;


		Iterate:
		if (count < LoopIterations) {
			count += 1;
			goto Iterate;
		}

		return count;
	}

	//TODO unusable if depth cannot go deeper than 174601
	[Benchmark("Loops", "Tests a recursive loop", skip: true)]
	public static int RecursiveLoop() {
		int count = 0;

		return RecursiveHelper(count);
	}

	private static int RecursiveHelper(int count) {
		if (count == 174600) {
			return count; //TODO fix amount of loop iterations. If becomes larger the stack overflows
		}

		return RecursiveHelper(count + 1);
	}

	//TODO make it not cheat
	[Benchmark("Loops", "Tests a LINQ loop", skip: true)]
	public static int LinqLoop() {
		int count = Enumerable.Range(0, LoopIterations).Count();

		return count;
	}
}