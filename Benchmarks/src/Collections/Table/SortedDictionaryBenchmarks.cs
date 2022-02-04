using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.Table;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SortedDictionaryBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	public static readonly SortedDictionary<int, int> Data = new();

	static SortedDictionaryBenchmarks() {
		foreach ((int index, int value) in CollectionsHelpers.RandomValues.WithIndex()) {
			Data.Add(index, value);
		}
	}

	[Benchmark("TableInsertion", "Tests insertion into a SortedDictionary")]
	public static int SortedDictionaryInsertion() {
		SortedDictionary<int, int> temp = new SortedDictionary<int, int>();
		for (ulong i  = 0; i < LoopIterations; i++) {
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
		for (ulong i  = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableGet", "Tests getting values randomly from a SortedDictionary")]
	public static int SortedDictionaryGetRandom() {
		int sum = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableRemoval", "Tests removal from a SortedDictionary")]
	public static int SortedDictionaryRemoval() {
		SortedDictionary<int, int> temp = new SortedDictionary<int, int>();
		for (ulong i  = 0; i < LoopIterations; i++) {
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
		for (ulong i  = 0; i < LoopIterations; i++) {
			SortedDictionary<int, int> sortedDictionary = new SortedDictionary<int, int>();
			for (int index = 0; index < Data.Count; index++) {
				sortedDictionary.Add(index, index * 2);
			}

			result += sortedDictionary.Count;
		}

		return result;
	}

	[Benchmark("TableCopy", "Tests copying a SortedDictionary using a foreach loop looping through k/v pairs")]
	public static int SortedDictionaryCopyManualForeach() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			SortedDictionary<int, int> target = new SortedDictionary<int, int>();

			foreach (KeyValuePair<int, int> pair in Data) {
				target.Add(pair.Key, pair.Value);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("TableCopy", "Tests copying a SortedDictionary using a foreach loop and looping through keys")]
	public static int SortedDictionaryCopyManualForeachIndex() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			SortedDictionary<int, int> target = new SortedDictionary<int, int>();

			foreach (int key in Data.Keys) {
				target.Add(key, Data[key]);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("TableCopy", "Tests copying a SortedDictionary using a foreach loop using deconstruction")]
	public static int SortedDictionaryCopyManualForeachDeconstruct() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			SortedDictionary<int, int> target = new SortedDictionary<int, int>();

			foreach ((int key, int value) in Data) {
				target.Add(key, value);
			}

			result += target.Count;
		}


		return result;
	}
}