using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CsharpRAPL;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks.Collections.Table;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ReadOnlyDictionaryBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	public static readonly ReadOnlyDictionary<int, int> Data;

	static ReadOnlyDictionaryBenchmarks() {
		Data = new ReadOnlyDictionary<int, int>(CollectionsHelpers.RandomValues.WithIndex()
			.ToDictionary(tuple => tuple.index, tuple => tuple.value));
	}

	[Benchmark("TableGet", "Tests getting values sequentially from a ReadOnlyDictionary")]
	public static int ReadOnlyDictionaryGet() {
		int sum = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableGet", "Tests getting values randomly from a ReadOnlyDictionary")]
	public static int ReadOnlyDictionaryGetRandom() {
		int sum = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}
}