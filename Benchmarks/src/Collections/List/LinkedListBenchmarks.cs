using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.List;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class LinkedListBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	public static readonly LinkedList<int> Data = new();

	static LinkedListBenchmarks() {
		foreach (int value in CollectionsHelpers.RandomValues) {
			Data.AddLast(value);
		}
	}

	[Benchmark("ListGet", "Tests getting values sequentially from a LinkedList (Note this uses ElementAt)")]
	public static int LinkedListGet() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data.ElementAt(CollectionsHelpers.SequentialIndices[j]);
			}
		}

		return sum;
	}

	[Benchmark("ListGet", "Tests getting values randomly from a LinkedList (Note this uses ElementAt)")]
	public static int LinkedListGetRandom() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += Data.ElementAt(CollectionsHelpers.RandomIndices[j]);
			}
		}

		return sum;
	}

	[Benchmark("ListInsertion", "Tests appending a element to a LinkedList")]
	public static int LinkedListInsertionLast() {
		int result = 0;
		LinkedList<int> target = new();

		for (int i = 0; i < LoopIterations; i++) {
			for (int index = 0; index < Data.Count; index++) {
				target.AddLast(index);
			}

			result += target.Count;
			target.Clear();
		}

		return result;
	}

	[Benchmark("ListInsertion", "Tests inserting an element into a LinkedList at the front")]
	public static int LinkedListInsertionFirst() {
		int result = 0;
		LinkedList<int> target = new();

		for (int i = 0; i < LoopIterations; i++) {
			for (int index = 0; index < Data.Count; index++) {
				target.AddFirst(index);
			}

			result += target.Count;
			target.Clear();
		}

		return result;
	}

	[Benchmark("ListRemoval", "Tests removal from a LinkedList")]
	public static int LinkedListRemoval() {
		int result = 0;
		LinkedList<int> target = new();

		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				target.AddLast(j);
			}

			for (int index = (Data.Count - 1) / 2; index >= 0; index--) {
				target.Remove(target.ElementAt(index));
			}

			result += target.Count;
		}
		
		return result;
	}

	[Benchmark("ListCreation", "Tests allocation and initialization of a LinkedList")]
	public static int LinkedListCreation() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			LinkedList<int> linkedList = new LinkedList<int>();
			for (int index = 0; index < linkedList.Count; index++) {
				linkedList.AddLast(index * 2);
			}

			result += linkedList.Count;
		}
		
		return result;
	}

	[Benchmark("ListCopy", "Tests copying an LinkedList using a loop")]
	public static int LinkedListCopyManual() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			LinkedList<int> target = new();
			for (int j = 0; j < Data.Count; j++) {
				target.AddLast(j);
			}

			result += target.Count;
		}


		return result;
	}
}