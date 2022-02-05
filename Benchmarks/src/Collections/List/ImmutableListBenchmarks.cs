using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks.Collections.List;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ImmutableListBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	public static readonly ImmutableList<int> Data;

	static ImmutableListBenchmarks() {
		Data = ImmutableList<int>.Empty.AddRange(CollectionsHelpers.RandomValues);
	}

	[Benchmark("ListGet", "Tests getting values sequentially from a ImmutableList")]
	public static int ImmutableListGet() {
		int sum = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("ListGet", "Tests getting values randomly from a ImmutableList")]
	public static int ImmutableListGetRandom() {
		int sum = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}


	[Benchmark("ListInsertion", "Tests insertion into a ImmutableList")]
	public static int ImmutableListInsertion() {
		int result = 0;
		ImmutableList<int> target = ImmutableList<int>.Empty;

		for (ulong i = 0; i < LoopIterations; i++) {
			for (int index = 0; index < Data.Count; index++) {
				target = target.Add(index);
			}

			result += target.Count;
			target = target.Clear();
		}

		return result;
	}

	[Benchmark("ListRemoval", "Tests removal from a ImmutableList")]
	public static int ImmutableListRemoval() {
		int result = 0;
		ImmutableList<int> target = ImmutableList<int>.Empty;

		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				target = target.Add(j);
			}

			for (int index = (Data.Count - 1) / 2; index >= 0; index--) {
				target = target.RemoveAt(index);
			}

			result += target.Count;
		}

		return result;
	}

	[Benchmark("ListCreation", "Tests allocation and initialization of an ImmutableList")]
	public static int ImmutableListCreation() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ImmutableList<int> immutableList = ImmutableList<int>.Empty;
			for (int index = 0; index < Data.Count; index++) {
				immutableList = immutableList.Add(index * 2);
			}

			result += immutableList.Count;
		}


		return result;
	}

	[Benchmark("ListCopy", "Tests copying an ImmutableList using a foreach loop")]
	public static int ImmutableListCopyManualForeach() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ImmutableList<int> target = ImmutableList<int>.Empty;
			foreach (int element in Data) {
				target = target.Add(element);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("ListCopy", "Tests copying an ImmutableList using a for loop")]
	public static int ImmutableListCopyManualFor() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ImmutableList<int> target = ImmutableList<int>.Empty;
			for (int index = 0; index < Data.Count; index++) {
				target = target.Add(Data[index]);
			}

			result += target.Count;
		}


		return result;
	}
}