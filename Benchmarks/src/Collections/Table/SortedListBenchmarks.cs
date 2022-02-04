using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.Table;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SortedListBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	public static readonly SortedList<int, int> Data = new(1000);

	static SortedListBenchmarks() {
		foreach ((int index, int value) in CollectionsHelpers.RandomValues.WithIndex()) {
			Data.Add(index, value);
		}
	}


	[Benchmark("TableInsertion", "Tests insertion into a SortedList")]
	public static int SortedListInsertion() {
		SortedList<int, int> temp = new SortedList<int, int>();
		for (ulong i  = 0; i < LoopIterations; i++) {
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
		for (ulong i  = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableGet", "Tests getting values randomly from a SortedList")]
	public static int SortedListGetRandom() {
		int sum = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableRemoval", "Tests removal from a SortedList")]
	public static int SortedListRemoval() {
		SortedList<int, int> temp = new SortedList<int, int>();
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

	[Benchmark("TableCreation", "Tests allocation and initialization of an SortedList")]
	public static int SortedListCreation() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			SortedList<int, int> sortedList = new SortedList<int, int>();
			for (int index = 0; index < Data.Count; index++) {
				sortedList.Add(index, index * 2);
			}

			result += sortedList.Count;
		}

		return result;
	}

	[Benchmark("TableCopy", "Tests copying a SortedList using a foreach loop looping through k/v pairs")]
	public static int SortedListCopyManualForeach() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			SortedList<int, int> target = new SortedList<int, int>();

			foreach (KeyValuePair<int, int> pair in Data) {
				target.Add(pair.Key, pair.Value);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("TableCopy", "Tests copying a SortedList using a foreach loop and looping through keys")]
	public static int SortedListCopyManualForeachIndex() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			SortedList<int, int> target = new SortedList<int, int>();

			foreach (int key in Data.Keys) {
				target.Add(key, Data[key]);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("TableCopy", "Tests copying a SortedList using a foreach loop using deconstruction")]
	public static int SortedListCopyManualForeachDeconstruct() {
		int result = 0;
		for (ulong i  = 0; i < LoopIterations; i++) {
			SortedList<int, int> target = new SortedList<int, int>();

			foreach ((int key, int value) in Data) {
				target.Add(key, value);
			}

			result += target.Count;
		}


		return result;
	}
}