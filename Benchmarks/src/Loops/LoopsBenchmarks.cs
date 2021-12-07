using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Loops;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class LoopsBenchmarks {
	public static int Iterations;
	public static int LoopIterations;


	[Benchmark("Loops", "Tests a do-while loop")]
	public static int DoWhile() {
		int count = 0;
		int i = 0;

		do {
			count += 1 + i;
			i++;
		} while (i < LoopIterations);

		return count;
	}

	[Benchmark("Loops", "Tests a while loop")]
	public static int While() {
		int count = 0;
		int i = 0;

		while (i < LoopIterations) {
			count += 1 + i;
			i++;
		}

		return count;
	}

	[Benchmark("Loops", "Tests a for loop")]
	public static int For() {
		int count = 0;

		for (int i = 0; i < LoopIterations; i++) {
			count += 1 + i;
		}


		return count;
	}

	[Benchmark("Loops", "Tests a foreach loop")]
	public static int ForEach() {
		int count = 0;

		foreach (int i in Enumerable.Range(0, LoopIterations)) {
			count += 1 + i;
		}

		return count;
	}

	[Benchmark("Loops", "Tests a for loop with a list that is first built")]
	public static int ForCompatibleWithForeach() {
		List<int> initialList = new();
		for (int i = 0; i < LoopIterations; i++) {
			initialList.Add(i);
		}

		int res = 0;
		for (int i = 0; i < initialList.Count; i++) {
			res += initialList[i];
		}

		return res;
	}

	[Benchmark("Loops", "Tests a foreach loop with a list that is first built")]
	public static int ForEachWithArray() {
		List<int> initialList = new();
		for (int i = 0; i < LoopIterations; i++) {
			initialList.Add(i);
		}

		int res = 0;
		foreach (int i in initialList) {
			res += i;
		}

		return res;
	}

	[Benchmark("Loops", "Tests a goto based loop, in style of do-while loop")]
	public static int GotoDoWhile() {
		int count = 0;
		int i = 0;


		Iterate:
		count += 1 + i;
		i++;
		if (i < LoopIterations) {
			goto Iterate;
		}

		return count;
	}

	[Benchmark("Loops", "Tests a goto based loop in style of while loop")]
	public static int GotoWhile() {
		int count = 0;
		int i = 0;


		Iterate:
		if (i < LoopIterations) {
			count += 1 + i;
			i++;
			goto Iterate;
		}

		return count;
	}

	//Todo: Unusable if depth cannot go deeper than 174601
	//Todo: Fix amount of loop iterations. If becomes larger the stack overflows
	[Benchmark("Loops", "Tests a recursive loop", skip: true)]
	public static int Recursive() {
		int count = 0;

		return RecursiveHelper(count);
	}

	private static int RecursiveHelper(int count) {
		if (count == 174600) {
			return count;
		}

		return RecursiveHelper(count + 1);
	}
}