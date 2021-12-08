using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.Table;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SortedDictionaryBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	public static readonly SortedDictionary<int, int> Data = new();

	static SortedDictionaryBenchmarks() {
		foreach ((int index, int value) in CollectionsHelpers.RandomValues.WithIndex()) {
			Data.Add(index, value);
		}
	}

	[Benchmark("TableInsertion", "Tests insertion into a SortedDictionary")]
	public static int SortedDictionaryInsertion() {
		SortedDictionary<int, int> temp = new SortedDictionary<int, int>();
		for (int i = 0; i < LoopIterations; i++) {
			temp = new SortedDictionary<int, int>();
			for (int j = 0; j < 1000; j++) {
				temp.Add(CollectionsHelpers.SequentialIndices[j], CollectionsHelpers.RandomValues[j]);
			}
		}

		return temp.Count;
	}

	[Benchmark("TableGet", "Tests getting values sequentially from a SortedDictionary")]
	public static int SortedDictionaryGet() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableGet", "Tests getting values randomly from a SortedDictionary")]
	public static int SortedDictionaryGetRandom() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableRemoval", "Tests removal from a SortedDictionary")]
	public static int SortedDictionaryRemoval() {
		SortedDictionary<int, int> temp = new SortedDictionary<int, int>();
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

	[Benchmark("TableCreation", "Tests allocation and initialization of an SortedDictionary")]
	public static int SortedDictionaryCreation() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			SortedDictionary<int, int> sortedDictionary = new SortedDictionary<int, int>();
			for (int index = 0; index < Data.Count; index++) {
				sortedDictionary.Add(index, index * 2);
			}

			result += sortedDictionary.Count;
		}

		return result;
	}

	[Benchmark("TableCopy", "Tests copying an SortedDictionary using a loop")]
	public static int SortedDictionaryCopyManual() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			SortedDictionary<int, int> target = new SortedDictionary<int, int>();

			for (int j = 0; j < Data.Count; j++) {
				target.Add(j, Data[j]);
			}

			result += target.Count;
		}


		return result;
	}
}