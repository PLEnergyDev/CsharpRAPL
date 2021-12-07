using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.List;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ListBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	public static readonly List<int> Data = new(1000);

	static ListBenchmarks() {
		foreach (int value in CollectionsHelpers.RandomValues) {
			Data.Add(value);
		}
	}

	[Benchmark("ListGet", "Tests getting values sequentially from a List")]
	public int ListGet() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("ListGet", "Tests getting values randomly from a List")]
	public static int ListGetRandom() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("ListInsertion", "Tests insertion into a List")]
	public static int ListInsertion() {
		int result = 0;
		List<int> target = new List<int>();

		for (int i = 0; i < LoopIterations; i++) {
			for (int index = 0; index < Data.Count; index++) {
				target.Add(index);
			}

			result += target.Count;
			target.Clear();
		}

		return result;
	}

	[Benchmark("ListRemoval", "Tests removal from a List")]
	public static int ListRemoval() {
		int result = 0;
		List<int> target = new List<int>();

		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				target.Add(j);
			}

			for (int index = (Data.Count - 1) / 2; index >= 0; index--) {
				target.RemoveAt(index);
			}

			result += target.Count;
		}

		return result;
	}

	[Benchmark("ListCreation", "Tests allocation and initialization of a List")]
	public static int ListCreation() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			List<int> list = new List<int>();
			for (int index = 0; index < list.Count; index++) {
				list.Add(index * 2);
			}

			result += list.Count;
		}
		
		return result;
	}

	[Benchmark("ListCopy", "Tests copying an List using a loop")]
	public static int ListCopyManual() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			List<int> target = new List<int>();
			for (int j = 0; j < Data.Count; j++) {
				target.Add(j);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("ListCopy", "Tests copying a list using a loop where capacity is allocated from the beginning")]
	public static int ListCopyPreAllocated() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			List<int> target = new List<int>(Data.Count);
			for (int index = 0; index < Data.Count; index++) {
				target.Add(index);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("ListCopy", "Tests copying a list using the list constructor")]
	public static int ListCopyConstructor() {
		List<int> target = new List<int>();
		for (int i = 0; i < LoopIterations; i++) {
			target = new List<int>(Data);
		}

		return target.Count;
	}

	[Benchmark("ListCopy", "Tests copying a list using GetRange")]
	public static int ListCopyGetRange() {
		List<int> target = new List<int>();
		for (int i = 0; i < LoopIterations; i++) {
			target = Data.GetRange(0, Data.Count);
		}

		return target.Count;
	}

	[Benchmark("ListCopy", "Tests copying a list using ToList from Linq")]
	public static int ListLinqClone() {
		List<int> target = new List<int>();
		for (int i = 0; i < LoopIterations; i++) {
			target = Data.ToList();
		}

		return target.Count;
	}
}