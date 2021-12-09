using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.Set;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ImmutableHashSetBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	public static readonly ImmutableHashSet<int> Data;


	static ImmutableHashSetBenchmarks() {
		Data = ImmutableHashSet.CreateRange(CollectionsHelpers.SequentialIndices);
	}

	[Benchmark("SetCreation", "Tests allocation and initialization of an ImmutableHashSet")]
	public static int ImmutableHashSetCreation() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			ImmutableHashSet<int> hashSet = ImmutableHashSet<int>.Empty;
			for (int index = 0; index < Data.Count; index++) {
				hashSet = hashSet.Add(index * 2);
			}

			result += hashSet.Count;
		}


		return result;
	}

	[Benchmark("SetGet", "Tests getting values from a ImmutableHashSet")]
	public static int ImmutableHashSetGet() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				Data.TryGetValue(j, out int value);
				sum += value;
			}
		}

		return sum;
	}

	[Benchmark("SetInsertion", "Tests insertion into a ImmutableHashSet")]
	public static int ImmutableHashSetInsertion() {
		ImmutableHashSet<int> temp = ImmutableHashSet<int>.Empty;
		for (int i = 0; i < LoopIterations; i++) {
			temp = ImmutableHashSet<int>.Empty;
			for (int j = 0; j < 1000; j++) {
				temp = temp.Add(j);
			}
		}

		return temp.Count;
	}

	[Benchmark("SetRemoval", "Tests removal from a ImmutableHashSet")]
	public static int ImmutableHashSetRemoval() {
		ImmutableHashSet<int> temp = ImmutableHashSet<int>.Empty;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < 1000; j++) {
				temp = temp.Add(j);
			}

			for (int k = 0; k < 1000; k++) {
				temp = temp.Remove(k);
			}
		}

		return temp.Count;
	}

	[Benchmark("SetCopy", "Tests copying an HashSet using a loop")]
	public static int ImmutableHashSetCopyManual() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			ImmutableHashSet<int> target = ImmutableHashSet<int>.Empty;
			for (int j = Data.Count - 1; j >= 0; j--) {
				target = target.Add(j);
			}

			result += target.Count;
		}


		return result;
	}
}