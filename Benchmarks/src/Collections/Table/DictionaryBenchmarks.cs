using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks.Collections.Table;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class DictionaryBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;

	public static readonly Dictionary<int, int> Data = new(1000);

	static DictionaryBenchmarks() {
		foreach ((int index, int value) in CollectionsHelpers.RandomValues.WithIndex()) {
			Data.Add(index, value);
		}
	}


	[Benchmark("TableInsertion", "Tests insertion into a Dictionary")]
	public static int DictionaryInsertion() {
		Dictionary<int, int> temp = new Dictionary<int, int>();
		for (ulong i = 0; i < LoopIterations; i++) {
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
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableGet", "Tests getting values randomly from a Dictionary")]
	public static int DictionaryGetRandom() {
		int sum = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableRemoval", "Tests removal from a Dictionary")]
	public static int DictionaryRemoval() {
		Dictionary<int, int> temp = new Dictionary<int, int>();
		for (ulong i = 0; i < LoopIterations; i++) {
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
		for (ulong i = 0; i < LoopIterations; i++) {
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int index = 0; index < Data.Count; index++) {
				dictionary.Add(index, index * 2);
			}

			result += dictionary.Count;
		}

		return result;
	}

	[Benchmark("TableCopy", "Tests copying a Dictionary using a foreach loop looping through k/v pairs")]
	public static int DictionaryCopyManualForeach() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			Dictionary<int, int> target = new Dictionary<int, int>();

			foreach (KeyValuePair<int, int> pair in Data) {
				target.Add(pair.Key, pair.Value);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("TableCopy", "Tests copying a Dictionary using a foreach loop and looping through keys")]
	public static int DictionaryCopyManualForeachIndex() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			Dictionary<int, int> target = new Dictionary<int, int>();

			foreach (int key in Data.Keys) {
				target.Add(key, Data[key]);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("TableCopy", "Tests copying a Dictionary using a foreach loop using deconstruction")]
	public static int DictionaryCopyManualForeachDeconstruct() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			Dictionary<int, int> target = new Dictionary<int, int>();

			foreach ((int key, int value) in Data) {
				target.Add(key, value);
			}

			result += target.Count;
		}


		return result;
	}
}