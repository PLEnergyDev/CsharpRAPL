using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.Set;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SortedSetBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	public static readonly SortedSet<int> Data = new();


	static SortedSetBenchmarks() {
		foreach (int value in CollectionsHelpers.SequentialIndices) {
			Data.Add(value);
		}
	}

	[Benchmark("SetCreation", "Tests allocation and initialization of a SortedSet")]
	public static int SortedSetCreation() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			SortedSet<int> hashSet = new SortedSet<int>();
			for (int index = 0; index < Data.Count; index++) {
				hashSet.Add(index * 2);
			}

			result += hashSet.Count;
		}
		
		return result;
	}

	[Benchmark("SetGet", "Tests getting values from a SortedSet")]
	public static int SortedSetGet() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				Data.TryGetValue(i, out int value);
				sum += value;
			}
		}

		return sum;
	}

	[Benchmark("SetInsertion", "Tests insertion into a SortedSet")]
	public static int SortedSetInsertion() {
		SortedSet<int> temp = new SortedSet<int>();
		for (int i = 0; i < LoopIterations; i++) {
			temp = new SortedSet<int>();
			for (int j = 0; j < 1000; j++) {
				temp.Add(j);
			}
		}

		return temp.Count;
	}

	[Benchmark("SetRemoval", "Tests removal from a SortedSet")]
	public static int SortedSetRemoval() {
		SortedSet<int> temp = new SortedSet<int>();
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < 1000; j++) {
				temp.Add(j);
			}

			for (int k = 0; k < 1000; k++) {
				temp.Remove(k);
			}
		}

		return temp.Count;
	}

	[Benchmark("SetCopy", "Tests copying an HashSet using a loop")]
	public static int SortedHashSetCopyManual() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			SortedSet<int> target = new SortedSet<int>();
			for (int j = Data.Count - 1; j >= 0; j--) {
				target.Add(j);
			}

			result += target.Count;
		}


		return result;
	}
}