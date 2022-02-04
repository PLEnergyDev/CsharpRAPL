using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.Table;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ImmutableSortedDictionaryBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	public static readonly ImmutableSortedDictionary<int, int> Data;

	static ImmutableSortedDictionaryBenchmarks() {
		Data = ImmutableSortedDictionary.CreateRange(
			CollectionsHelpers.RandomValues.Select((value, index) => new KeyValuePair<int, int>(index, value)));
	}

	[Benchmark("TableInsertion", "Tests insertion into a ImmutableSortedDictionary")]
	public static int ImmutableSortedDictionaryInsertion() {
		ImmutableSortedDictionary<int, int> temp = ImmutableSortedDictionary.Create<int, int>();
		for (ulong i  = 0; i < LoopIterations; i++) {
			temp = ImmutableSortedDictionary.Create<int, int>();
			for (int j = 0; j < 1000; j++) {
				temp = temp.Add(CollectionsHelpers.SequentialIndices[j], CollectionsHelpers.RandomValues[j]);
			}
		}

		return temp.Count;
	}

	[Benchmark("TableGet", "Tests getting values sequentially from a ImmutableSortedDictionary")]
	public static int ImmutableSortedDictionaryGet() {
		int sum = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableGet", "Tests getting values randomly from a ImmutableSortedDictionary")]
	public static int ImmutableSortedDictionaryGetRandom() {
		int sum = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableRemoval", "Tests removal from a ImmutableSortedDictionary")]
	public static int ImmutableSortedDictionaryRemoval() {
		ImmutableSortedDictionary<int, int> temp = ImmutableSortedDictionary.Create<int, int>();
		for (ulong i  = 0; i < LoopIterations; i++) {
			for (int j = 0; j < 1000; j++) {
				temp = temp.Add(CollectionsHelpers.SequentialIndices[j], CollectionsHelpers.RandomValues[j]);
			}

			for (int k = 0; k < 1000; k++) {
				temp = temp.Remove(CollectionsHelpers.SequentialIndices[k]);
			}
		}

		return temp.Count;
	}

	[Benchmark("TableCreation", "Tests allocation and initialization of an ImmutableSortedDictionary")]
	public static int ImmutableSortedDictionaryCreation() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			ImmutableSortedDictionary<int, int>
				immutableSortedDictionary = ImmutableSortedDictionary.Create<int, int>();
			for (int index = 0; index < Data.Count; index++) {
				immutableSortedDictionary = immutableSortedDictionary.Add(index, index * 2);
			}

			result += immutableSortedDictionary.Count;
		}

		return result;
	}

	[Benchmark("TableCopy", "Tests copying an ImmutableSortedDictionary using a foreach loop looping through k/v pairs")]
	public static int ImmutableSortedDictionaryCopyManualForeach() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			ImmutableSortedDictionary<int, int>
				target = ImmutableSortedDictionary.Create<int, int>();
			foreach (KeyValuePair<int, int> pair in Data) {
				target = target.Add(pair.Key, pair.Value);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("TableCopy", "Tests copying an ImmutableSortedDictionary using a foreach loop and looping through keys")]
	public static int ImmutableSortedDictionaryCopyManualForeachIndex() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			ImmutableSortedDictionary<int, int>
				target = ImmutableSortedDictionary.Create<int, int>();
			foreach (int key in Data.Keys) {
				target = target.Add(key, Data[key]);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("TableCopy", "Tests copying an ImmutableSortedDictionary using a foreach loop using deconstruction")]
	public static int ImmutableSortedDictionaryCopyManualForeachDeconstruct() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			ImmutableSortedDictionary<int, int>
				target = ImmutableSortedDictionary.Create<int, int>();
			foreach ((int key, int value) in Data) {
				target = target.Add(key, value);
			}

			result += target.Count;
		}


		return result;
	}
}