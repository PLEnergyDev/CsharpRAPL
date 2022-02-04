using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Loops;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class LoopsBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;

	[Benchmark("Loops", "Tests a do-while loop")]
	public static ulong DoWhile() {
		ulong count = 0;
		ulong i = 0;

		do {
			count += 1 + i;
			i++;
		} while (i < LoopIterations);

		return count;
	}

	[Benchmark("Loops", "Tests a while loop")]
	public static ulong While() {
		ulong count = 0;
		ulong i = 0;

		while (i < LoopIterations) {
			count += 1 + i;
			i++;
		}

		return count;
	}

	[Benchmark("Loops", "Tests a for loop")]
	public static ulong For() {
		ulong count = 0;

		for (ulong i = 0; i < LoopIterations; i++) {
			count += 1 + i;
		}


		return count;
	}

	[Benchmark("Loops", "Tests a foreach loop")]
	public static int ForEach() {
		int count = 0;
		var iter = (int)LoopIterations;

		foreach (int i in Enumerable.Range(0, iter)) {
			count += 1 + i;
		}

		return count;
	}

	[Benchmark("Loops", "Tests a for loop with a list that is first built")]
	public static ulong ForCompatibleWithForeach() {
		List<ulong> initialList = new();
		for (ulong i = 0; i < LoopIterations; i++) {
			initialList.Add(i);
		}

		ulong res = 0;
		for (int i = 0; i < initialList.Count; i++) {
			res += initialList[i];
		}

		return res;
	}

	[Benchmark("Loops", "Tests a foreach loop with a list that is first built")]
	public static ulong ForEachWithArray() {
		List<ulong> initialList = new();
		for (ulong i = 0; i < LoopIterations; i++) {
			initialList.Add(i);
		}

		ulong res = 0;
		foreach (ulong i in initialList) {
			res += i;
		}

		return res;
	}

	[Benchmark("Loops", "Tests a goto based loop, in style of do-while loop")]
	public static ulong GotoDoWhile() {
		ulong count = 0;
		ulong i = 0;


		Iterate:
		count += 1 + i;
		i++;
		if (i < LoopIterations) {
			goto Iterate;
		}

		return count;
	}

	[Benchmark("Loops", "Tests a goto based loop in style of while loop")]
	public static ulong GotoWhile() {
		ulong count = 0;
		ulong i = 0;


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
	public static ulong Recursive() {
		ulong count = 0;

		return RecursiveHelper(count);
	}

	private static ulong RecursiveHelper(ulong count) {
		if (count == 174600) {
			return count;
		}

		return RecursiveHelper(count + 1);
	}
}