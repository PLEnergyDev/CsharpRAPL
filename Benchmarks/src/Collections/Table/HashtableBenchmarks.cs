using System.Collections;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Collections.Table;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class HashtableBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	public static readonly Hashtable Data = new(1000);

	static HashtableBenchmarks() {
		foreach ((int index, int value) in CollectionsHelpers.RandomValues.WithIndex()) {
			Data.Add(index, value);
		}
	}


	[Benchmark("TableInsertion", "Tests insertion into a Hashtable")]
	public static int HashtableInsertion() {
		Hashtable temp = new Hashtable();
		for (int i = 0; i < LoopIterations; i++) {
			temp = new Hashtable();
			for (int j = 0; j < 1000; j++) {
				temp.Add(CollectionsHelpers.SequentialIndices[j], CollectionsHelpers.RandomValues[j]);
			}
		}

		return temp.Count;
	}

	[Benchmark("TableGet", "Tests getting values sequentially from a Hashtable")]
	public static int HashtableGet() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += (int)Data[CollectionsHelpers.SequentialIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableGet", "Tests getting values randomly from a Hashtable")]
	public static int HashtableGetRandom() {
		int sum = 0;
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < Data.Count; j++) {
				sum += (int)Data[CollectionsHelpers.RandomIndices[j]];
			}
		}

		return sum;
	}

	[Benchmark("TableRemoval", "Tests removal from a Hashtable")]
	public static int HashtableRemoval() {
		Hashtable temp = new Hashtable();
		for (int i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < 1000; j++) {
				temp.Add(CollectionsHelpers.SequentialIndices[j], CollectionsHelpers.RandomValues[j]);
			}

			for (int k = 0; k < 1000; k++) {
				temp.Remove(CollectionsHelpers.SequentialIndices[k]);
			}
		}

		return temp.Count;
	}

	[Benchmark("TableCreation", "Tests allocation and initialization of a Hashtable")]
	public static int HashtableCreation() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			Hashtable hashtable = new Hashtable();
			for (int index = 0; index < Data.Count; index++) {
				hashtable.Add(index, index * 2);
			}

			result += hashtable.Count;
		}

		return result;
	}

	[Benchmark("TableCopy", "Tests copying an Hashtable using a loop")]
	public static int HashtableCopyManual() {
		int result = 0;
		for (int i = 0; i < LoopIterations; i++) {
			Hashtable target = new Hashtable();

			for (int j = 0; j < Data.Count; j++) {
				target.Add(j, Data[j]);
			}

			result += target.Count;
		}


		return result;
	}
}