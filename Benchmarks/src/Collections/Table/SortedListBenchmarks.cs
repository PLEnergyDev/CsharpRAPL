using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.Table;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SortedListBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	public static readonly SortedList<int, int> Data = new(1000);

	static SortedListBenchmarks() {
		foreach ((int index, int value) in CollectionsHelpers.RandomValues.WithIndex()) {
			Data.Add(index, value);
		}
	}


	[Benchmark("TableInsertion", "Tests insertion into a SortedList")]
	public static int SortedListInsertion() {
		SortedList<int, int> temp = new SortedList<int, int>();
		for (int i = 0; i < LoopIterations; i++) {
			temp = new SortedList<int, int>();
			for (int j = 0; j < 1000; j++) {
				temp.Add(CollectionsHelpers.SequentialIndices[j], CollectionsHelpers.RandomValues[j]);
			}
		}

		return temp.Count;
	}

	[Benchmark("TableGet", "Tests getting values sequentially from a SortedList")]
	public static int SortedListGet() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableGet", "Tests getting values randomly from a SortedList")]
	public static int SortedListGetRandom() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableRemoval", "Tests removal from a SortedList")]
	public static int SortedListRemoval() {
		SortedList<int, int> temp = new SortedList<int, int>();
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < 1000; j++) {
				temp.Add(CollectionsHelpers.SequentialIndices[j], CollectionsHelpers.RandomValues[j]);
			}

			for (int k = 0; k < 1000; k++) {
				temp.Remove(CollectionsHelpers.SequentialIndices[k]);
			}
		}

		return temp.Count;
	}

	[Benchmark("TableCreation", "Tests allocation and initialization of an SortedList")]
	public static int SortedListCreation() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			SortedList<int, int> sortedList = new SortedList<int, int>();
			for (int index = 0; index < Data.Count; index++) {
				sortedList.Add(index, index * 2);
			}

			result += sortedList.Count;
		}

		return result;
	}

	[Benchmark("TableCopy", "Tests copying an SortedList using a loop")]
	public static int SortedListCopyManual() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			SortedList<int, int> target = new SortedList<int, int>();

			for (int j = 0; j < Data.Count; j++) {
				target.Add(j, Data[j]);
			}

			result += target.Count;
		}


		return result;
	}
}