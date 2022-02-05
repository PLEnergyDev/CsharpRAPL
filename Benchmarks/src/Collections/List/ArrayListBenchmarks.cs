using System.Collections;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks.Collections.List;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ArrayListBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	public static readonly ArrayList Data = new(1000);

	static ArrayListBenchmarks() {
		foreach (int value in CollectionsHelpers.RandomValues) {
			Data.Add(value);
		}
	}

	[Benchmark("ListGet", "Tests getting values sequentially from a ArrayList")]
	public static int ArrayListGet() {
		int sum = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += (int)Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("ListGet", "Tests getting values randomly from a ArrayList")]
	public static int ArrayListGetRandom() {
		int sum = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += (int)Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("ListInsertion", "Tests insertion into a ArrayList")]
	public static int ArrayListInsertion() {
		int result = 0;
		ArrayList target = new();

		for (ulong i = 0; i < LoopIterations; i++) {
			for (int index = 0; index < Data.Count; index++) {
				target.Add(index);
			}

			result += target.Count;
			target.Clear();
		}

		return result;
	}

	[Benchmark("ListRemoval", "Tests removal from a ArrayList")]
	public static int ArrayListRemoval() {
		int result = 0;
		ArrayList target = new();

		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				target.Add(j);
			}

			for (int index = (Data.Count - 1) / 2; index >= 0; index--) {
				target.RemoveAt(index);
			}

			result += target.Count;
		}

		return result;
	}

	[Benchmark("ListCreation", "Tests allocation and initialization of an ArrayList")]
	public static int ArrayListCreation() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ArrayList array = new ArrayList();
			for (int index = 0; index < Data.Count; index++) {
				array.Add(index * 2);
			}

			result += array.Count;
		}

		return result;
	}

	[Benchmark("ListCopy", "Tests copying an ArrayList using a foreach loop")]
	public static int ArrayListCopyManualForeach() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ArrayList target = new();
			foreach (int element in Data) {
				target.Add(element);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("ListCopy", "Tests copying an ArrayList using a for loop")]
	public static int ArrayListCopyManualFor() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ArrayList target = new();
			for (int index = 0; index < Data.Count; index++) {
				target.Add(Data[index]);
			}

			result += target.Count;
		}


		return result;
	}

	[Benchmark("ListCopy", "Tests copying an ArrayList using Clone")]
	public static int ArrayListCopyClone() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ArrayList target = new();
			target = (ArrayList)Data.Clone();
			result += target.Count;
		}

		return result;
	}
}