using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.Table;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ImmutableSortedDictionaryBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	public static readonly ImmutableSortedDictionary<int, int> Data;

	static ImmutableSortedDictionaryBenchmarks() {
		Data = ImmutableSortedDictionary.CreateRange(
			CollectionsHelpers.RandomValues.Select((value, index) => new KeyValuePair<int, int>(index, value)));
	}

	[Benchmark("TableInsertion", "Tests insertion into a ImmutableSortedDictionary")]
	public static int ImmutableSortedDictionaryInsertion() {
		ImmutableSortedDictionary<int, int> temp = ImmutableSortedDictionary.Create<int, int>();
		for (int i = 0; i < LoopIterations; i++) {
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
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableGet", "Tests getting values randomly from a ImmutableSortedDictionary")]
	public static int ImmutableSortedDictionaryGetRandom() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableRemoval", "Tests removal from a ImmutableSortedDictionary")]
	public static int ImmutableSortedDictionaryRemoval() {
		ImmutableSortedDictionary<int, int> temp = ImmutableSortedDictionary.Create<int, int>();
		for (int i = 0; i < LoopIterations; i++) {
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
		for (int i = 0; i < LoopIterations; i++) {
			ImmutableSortedDictionary<int, int>
				immutableSortedDictionary = ImmutableSortedDictionary.Create<int, int>();
			for (int index = 0; index < immutableSortedDictionary.Count; index++) {
				immutableSortedDictionary = immutableSortedDictionary.Add(index, index * 2);
			}

			result += immutableSortedDictionary.Count;
		}

		return result;
	}

	[Benchmark("TableCopy", "Tests copying an ImmutableSortedDictionary using a loop")]
	public static int ImmutableSortedDictionaryCopyManual() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			ImmutableSortedDictionary<int, int> target = ImmutableSortedDictionary.Create<int, int>();

			for (int j = 0; j < Data.Count; j++) {
				target = target.Add(j, Data[j]);
			}

			result += target.Count;
		}


		return result;
	}
}