using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.Set;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class HashSetBenchmarks {
	public static int Iterations;
	public static int LoopIterations;
	public static readonly HashSet<int> Data = new(1000);


	static HashSetBenchmarks() {
		foreach (int value in CollectionsHelpers.SequentialIndices) {
			Data.Add(value);
		}
	}

	[Benchmark("SetCreation", "Tests allocation and initialization of a HashSet")]
	public static int HashSetCreation() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			HashSet<int> array = new HashSet<int>();
			for (int index = 0; index < Data.Count; index++) {
				array.Add(index * 2);
			}

			result += array.Count;
		}

		return result;
	}

	[Benchmark("SetGet", "Tests getting values from a HashSet")]
	public static int HashSetGet() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				Data.TryGetValue(j, out int value);
				sum += value;
			}
		}

		return sum;
	}

	[Benchmark("SetInsertion", "Tests insertion into a HashSet")]
	public static int HashSetInsertion() {
		HashSet<int> temp = new();
		for (int i = 0; i < LoopIterations; i++) {
			temp = new HashSet<int>();
			for (int j = 0; j < 1000; j++) {
				temp.Add(j);
			}
		}

		return temp.Count;
	}

	[Benchmark("SetRemoval", "Tests removal from a HashSet")]
	public static int HashSetRemoval() {
		HashSet<int> temp = new();
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

	[Benchmark("SetCopy", "Tests copying a HashSet using a foreach loop")]
	public static int HashSetCopyManualForeach() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			HashSet<int> target = new HashSet<int>();
			foreach (int element in Data) {
				target.Add(element);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("SetCopy", "Tests copying a HashSet using a for loop")]
	public static int HashSetCopyManualFor() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			HashSet<int> target = new HashSet<int>();
			for (int j = 0; j < Data.Count; j++) {
				Data.TryGetValue(j, out int value);
				target.Add(value);
			}

			result += target.Count;
		}


		return result;
	}
}