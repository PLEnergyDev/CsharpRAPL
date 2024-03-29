﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks.Collections.Table;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ImmutableDictionaryBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;

	public static readonly ImmutableDictionary<int, int> Data;

	static ImmutableDictionaryBenchmarks() {
		Data = ImmutableDictionary.CreateRange(
			CollectionsHelpers.RandomValues.Select((value, index) => new KeyValuePair<int, int>(index, value)));
	}

	[Benchmark("TableInsertion", "Tests insertion into a ImmutableDictionary")]
	public static int ImmutableDictionaryInsertion() {
		ImmutableDictionary<int, int> temp = ImmutableDictionary.Create<int, int>();
		for (ulong i = 0; i < LoopIterations; i++) {
			temp = ImmutableDictionary.Create<int, int>();
			for (int j = 0; j < 1000; j++) {
				temp = temp.Add(CollectionsHelpers.SequentialIndices[j], CollectionsHelpers.RandomValues[j]);
			}
		}

		return temp.Count;
	}

	[Benchmark("TableGet", "Tests getting values sequentially from a ImmutableDictionary")]
	public static int ImmutableDictionaryGet() {
		int sum = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableGet", "Tests getting values randomly from a ImmutableDictionary")]
	public static int ImmutableDictionaryGetRandom() {
		int sum = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableRemoval", "Tests removal from a ImmutableDictionary")]
	public static int ImmutableDictionaryRemoval() {
		ImmutableDictionary<int, int> temp = ImmutableDictionary.Create<int, int>();
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < 1000; j++) {
				temp = temp.Add(CollectionsHelpers.SequentialIndices[j], CollectionsHelpers.RandomValues[j]);
			}

			for (int k = 0; k < 1000; k++) {
				temp = temp.Remove(CollectionsHelpers.SequentialIndices[k]);
			}
		}

		return temp.Count;
	}

	[Benchmark("TableCreation", "Tests allocation and initialization of an ImmutableDictionary")]
	public static int ImmutableDictionaryCreation() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ImmutableDictionary<int, int> immutableDictionary = ImmutableDictionary.Create<int, int>();
			for (int index = 0; index < Data.Count; index++) {
				immutableDictionary = immutableDictionary.Add(index, index * 2);
			}

			result += immutableDictionary.Count;
		}

		return result;
	}

	[Benchmark("TableCopy", "Tests copying an ImmutableDictionary using a foreach loop looping through k/v pairs")]
	public static int ImmutableDictionaryCopyManualForeach() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ImmutableDictionary<int, int> target = ImmutableDictionary.Create<int, int>();

			foreach (KeyValuePair<int, int> pair in Data) {
				target = target.Add(pair.Key, pair.Value);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("TableCopy", "Tests copying an ImmutableDictionary using a foreach loop and looping through keys")]
	public static int ImmutableDictionaryCopyManualForeachIndex() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ImmutableDictionary<int, int> target = ImmutableDictionary.Create<int, int>();

			foreach (int key in Data.Keys) {
				target = target.Add(key, Data[key]);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("TableCopy", "Tests copying an ImmutableDictionary using a foreach loop using deconstruction")]
	public static int ImmutableDictionaryCopyManualForeachDeconstruct() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ImmutableDictionary<int, int> target = ImmutableDictionary.Create<int, int>();

			foreach ((int key, int value) in Data) {
				target = target.Add(key, value);
			}

			result += target.Count;
		}


		return result;
	}
}