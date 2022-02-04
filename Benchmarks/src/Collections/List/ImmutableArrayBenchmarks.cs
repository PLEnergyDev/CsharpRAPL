using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.List;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ImmutableArrayBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	public static readonly ImmutableArray<int> Data;

	static ImmutableArrayBenchmarks() {
		Data = ImmutableArray.Create(CollectionsHelpers.RandomValues);
	}

	[Benchmark("ListGet", "Tests getting values sequentially from a ImmutableArray")]
	public static int ImmutableArrayGet() {
		int sum = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Length; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("ListGet", "Tests getting values randomly from a ImmutableArray")]
	public static int ImmutableArrayGetRandom() {
		int sum = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Length; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("ListInsertion", "Tests insertion into a ImmutableArray")]
	public static int ImmutableArrayInsertion() {
		int result = 0;
		ImmutableArray<int> target = ImmutableArray<int>.Empty;

		for (ulong i = 0; i < LoopIterations; i++) {
			for (int index = 0; index < Data.Length; index++) {
				target = target.Add(index);
			}

			result += target.Length;
			target = target.Clear();
		}

		return result;
	}

	[Benchmark("ListRemoval", "Tests removal from a ImmutableArray")]
	public static int ImmutableArrayRemoval() {
		int result = 0;
		ImmutableArray<int> target = ImmutableArray<int>.Empty;

		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Length; j++) {
				target = target.Add(j);
			}

			for (int index = (Data.Length - 1) / 2; index >= 0; index--) {
				target = target.RemoveAt(index);
			}

			result += target.Length;
		}

		return result;
	}

	[Benchmark("ListCreation", "Tests allocation and initialization of an ImmutableArray")]
	public static int ImmutableArrayCreation() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ImmutableArray<int> array = ImmutableArray<int>.Empty;
			for (int index = 0; index < Data.Length; index++) {
				array = array.Add(index * 2);
			}

			result += array.Length;
		}


		return result;
	}


	[Benchmark("ListCopy", "Tests copying an ImmutableArray using a foreach loop")]
	public static int ImmutableArrayCopyManualForeach() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ImmutableArray<int> target = ImmutableArray<int>.Empty;
			foreach (int element in Data) {
				target = target.Add(element);
			}

			result += target.Length;
		}


		return result;
	}

	[Benchmark("ListCopy", "Tests copying an ImmutableArray using a for loop")]
	public static int ImmutableArrayCopyManualFor() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ImmutableArray<int> target = ImmutableArray<int>.Empty;
			for (int index = 0; index < Data.Length; index++) {
				target = target.Add(Data[index]);
			}

			result += target.Length;
		}


		return result;
	}
}