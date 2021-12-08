using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.Table;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class DictionaryBenchmarks {
	public static int Iterations;
	public static int LoopIterations;
	public static readonly Dictionary<int, int> Data = new(1000);

	static DictionaryBenchmarks() {
		foreach ((int index, int value) in CollectionsHelpers.RandomValues.WithIndex()) {
			Data.Add(index, value);
		}
	}


	[Benchmark("TableInsertion", "Tests insertion into a Dictionary")]
	public static int DictionaryInsertion() {
		Dictionary<int, int> temp = new Dictionary<int, int>();
		for (int i = 0; i < LoopIterations; i++) {
			temp = new Dictionary<int, int>();
			for (int j = 0; j < 1000; j++) {
				temp.Add(CollectionsHelpers.SequentialIndices[j], CollectionsHelpers.RandomValues[j]);
			}
		}

		return temp.Count;
	}

	[Benchmark("TableGet", "Tests getting values sequentially from a Dictionary")]
	public static int DictionaryGet() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableGet", "Tests getting values randomly from a Dictionary")]
	public static int DictionaryGetRandom() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableRemoval", "Tests removal from a Dictionary")]
	public static int DictionaryRemoval() {
		Dictionary<int, int> temp = new Dictionary<int, int>();
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

	[Benchmark("TableCreation", "Tests allocation and initialization of a Dictionary")]
	public static int DictionaryCreation() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int index = 0; index < Data.Count; index++) {
				dictionary.Add(index, index * 2);
			}

			result += dictionary.Count;
		}

		return result;
	}

	[Benchmark("TableCopy", "Tests copying an Dictionary using a loop")]
	public static int DictionaryCopyManual() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			Dictionary<int, int> target = new Dictionary<int, int>();

			for (int j = 0; j < Data.Count; j++) {
				target.Add(j, Data[j]);
			}

			result += target.Count;
		}


		return result;
	}
}