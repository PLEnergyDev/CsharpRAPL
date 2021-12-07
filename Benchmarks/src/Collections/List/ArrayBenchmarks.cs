using System;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.List;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ArrayBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	public static readonly int[] Data = new int[1000];

	static ArrayBenchmarks() {
		for (int index = 0; index < CollectionsHelpers.RandomValues.Length; index++) {
			int value = CollectionsHelpers.RandomValues[index];
			Data[index] = value;
		}
	}

	[Benchmark("ListGet", "Tests getting values sequentially from an Array")]
	public static int ArrayGet() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Length; j++) {
				sum += Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("ListGet", "Tests getting values randomly from an Array")]
	public static int ArrayGetRandom() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Length; j++) {
				sum += Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("ListCreation", "Tests allocation and initialization of a array")]
	public static int ArrayCreation() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			int[] array = new int[1000];
			for (int index = 0; index < array.Length; index++) {
				array[index] = index * 2;
			}

			result += array.Length;
		}

		return result;
	}

	[Benchmark("ListCopy", "Tests copying an array using a loop")]
	public static int ArrayCopyManual() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			int[] target = new int[1000];
			for (int j = 0; j < Data.Length; j++) {
				target[j] = Data[j];
			}

			result += target.Length;
		}


		return result;
	}

	[Benchmark("ListCopy", "Tests copying an array using CopyTo")]
	public static int ArrayCopyTo() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			int[] target = new int[1000];
			Data.CopyTo(target, 0);
			result += target.Length;
		}


		return result;
	}

	[Benchmark("ListCopy", "Tests copying an array using Array.Copy")]
	public static int ArrayCopy() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			int[] target = new int[1000];
			Array.Copy(Data, target, Data.Length);
			result += target.Length;
		}

		return result;
	}

	[Benchmark("ListCopy", "Tests copying an array using Clone")]
	public static int ArrayCopyClone() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			int[] target = new int[1000];
			target = (int[])Data.Clone();
			result += target.Length;
		}

		return result;
	}
}