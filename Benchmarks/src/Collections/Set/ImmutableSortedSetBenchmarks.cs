using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks.Collections.Set;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ImmutableSortedSetBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	public static readonly ImmutableSortedSet<int> Data;


	static ImmutableSortedSetBenchmarks() {
		Data = ImmutableSortedSet.CreateRange(CollectionsHelpers.SequentialIndices);
	}

	[Benchmark("SetCreation", "Tests allocation and initialization of an ImmutableSortedSet")]
	public static int ImmutableSortedSetCreation() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ImmutableSortedSet<int> sortedSet = ImmutableSortedSet<int>.Empty;
			for (int index = 0; index < Data.Count; index++) {
				sortedSet = sortedSet.Add(index * 2);
			}

			result += sortedSet.Count;
		}


		return result;
	}

	[Benchmark("SetGet", "Tests getting values from a ImmutableSortedSet")]
	public static int ImmutableSortedSetGet() {
		int sum = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				Data.TryGetValue(j, out int value);
				sum += value;
			}
		}

		return sum;
	}

	[Benchmark("SetInsertion", "Tests insertion into a ImmutableSortedSet")]
	public static int ImmutableSortedSetInsertion() {
		ImmutableSortedSet<int> temp = ImmutableSortedSet<int>.Empty;
		for (ulong i = 0; i < LoopIterations; i++) {
			temp = ImmutableSortedSet<int>.Empty;
			for (int j = 0; j < 1000; j++) {
				temp = temp.Add(j);
			}
		}

		return temp.Count;
	}

	[Benchmark("SetRemoval", "Tests removal from a ImmutableSortedSet")]
	public static int ImmutableSortedSetRemoval() {
		ImmutableSortedSet<int> temp = ImmutableSortedSet<int>.Empty;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < 1000; j++) {
				temp = temp.Add(j);
			}

			for (int k = 0; k < 1000; k++) {
				temp = temp.Remove(k);
			}
		}

		return temp.Count;
	}

	[Benchmark("SetCopy", "Tests copying an ImmutableSortedSet using a foreach loop")]
	public static int ImmutableSortedSetCopyManualForeach() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ImmutableSortedSet<int> target = ImmutableSortedSet<int>.Empty;
			foreach (int element in Data) {
				target = target.Add(element);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("SetCopy", "Tests copying an ImmutableSortedSet using a for loop")]
	public static int ImmutableSortedSetCopyManualFor() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ImmutableSortedSet<int> target = ImmutableSortedSet<int>.Empty;
			for (int j = 0; j < Data.Count; j++) {
				Data.TryGetValue(j, out int value);
				target = target.Add(value);
			}

			result += target.Count;
		}


		return result;
	}
}