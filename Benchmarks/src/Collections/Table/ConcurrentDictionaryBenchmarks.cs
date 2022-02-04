using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.Table;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ConcurrentDictionaryBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	public static readonly ConcurrentDictionary<int, int> Data = new();

	static ConcurrentDictionaryBenchmarks() {
		foreach ((int index, int value) in CollectionsHelpers.RandomValues.WithIndex()) {
			Data.TryAdd(index, value);
		}
	}

	[Benchmark("TableInsertion", "Tests insertion into a ConcurrentDictionary")]
	public static int ConcurrentDictionaryInsertion() {
		ConcurrentDictionary<int, int> temp = new ConcurrentDictionary<int, int>();
		for (ulong i  = 0; i < LoopIterations; i++) {
			temp = new ConcurrentDictionary<int, int>();
			for (int j = 0; j < 1000; j++) {
				temp.TryAdd(CollectionsHelpers.SequentialIndices[j], CollectionsHelpers.RandomValues[j]);
			}
		}

		return temp.Count;
	}

	[Benchmark("TableGet", "Tests getting values sequentially from a ConcurrentDictionary")]
	public static int ConcurrentDictionaryGet() {
		int sum = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableGet", "Tests getting values randomly from a ConcurrentDictionary")]
	public static int ConcurrentDictionaryGetRandom() {
		int sum = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableRemoval", "Tests removal from a ConcurrentDictionary")]
	public static int ConcurrentDictionaryRemoval() {
		ConcurrentDictionary<int, int> temp = new ConcurrentDictionary<int, int>();
		for (ulong i  = 0; i < LoopIterations; i++) {
			for (int j = 0; j < 1000; j++) {
				temp.TryAdd(CollectionsHelpers.SequentialIndices[j], CollectionsHelpers.RandomValues[j]);
			}

			for (int k = 0; k < 1000; k++) {
				temp.TryRemove(CollectionsHelpers.SequentialIndices[k], out _);
			}
		}

		return temp.Count;
	}

	[Benchmark("TableCreation", "Tests allocation and initialization of a ConcurrentDictionary")]
	public static int ConcurrentDictionaryCreation() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			ConcurrentDictionary<int, int> concurrentDictionary = new ConcurrentDictionary<int, int>();
			for (int index = 0; index < Data.Count; index++) {
				concurrentDictionary.TryAdd(index, index * 2);
			}

			result += concurrentDictionary.Count;
		}

		return result;
	}

	[Benchmark("TableCopy", "Tests copying an ConcurrentDictionary using a foreach loop looping through k/v pairs")]
	public static int ConcurrentDictionaryCopyManualForeach() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			ConcurrentDictionary<int, int> target = new ConcurrentDictionary<int, int>();

			foreach (KeyValuePair<int, int> pair in Data) {
				target.TryAdd(pair.Key, pair.Value);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("TableCopy", "Tests copying an ConcurrentDictionary using a foreach loop and looping through keys")]
	public static int ConcurrentDictionaryCopyManualForeachIndex() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			ConcurrentDictionary<int, int> target = new ConcurrentDictionary<int, int>();

			foreach (int key in Data.Keys) {
				target.TryAdd(key, Data[key]);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("TableCopy", "Tests copying an ConcurrentDictionary using a foreach loop using deconstruction")]
	public static int ConcurrentDictionaryCopyManualForeachDeconstruct() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			ConcurrentDictionary<int, int> target = new ConcurrentDictionary<int, int>();

			foreach ((int key, int value) in Data) {
				target.TryAdd(key, value);
			}

			result += target.Count;
		}


		return result;
	}
}