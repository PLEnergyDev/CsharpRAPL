using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class GenericBenchmarks {
	public static int Iterations;
	public static int LoopIterations;
	public static readonly Generic<int> GenericIntClass = new(10);
	public static readonly NonGeneric NonGenericIntClass = new(10);

	public static readonly Generic<long> GenericLongClass = new(10);
	public static readonly NonGeneric NonGenericLongClass = new(10L);
	
	[Benchmark("Generics", "Tests a addition using a generic field (Int in this case)")]
	public static int GenericAddInt() {
		int results = 0;
		for (int i = 0; i < LoopIterations; i++) {
			results += GenericIntClass.Value + 2;
		}

		return results;
	}

	[Benchmark("Generics", "Tests a addition using a object field (Note we have to cast to int)")]
	public static int NonGenericCastInt() {
		int results = 0;
		for (int i = 0; i < LoopIterations; i++) {
			results += (int)NonGenericIntClass.Value + 2;
		}

		return results;
	}

	[Benchmark("Generics", "Tests a addition using a int field (Note we have to cast to int)")]
	public static int NonGenericInt() {
		int results = 0;
		for (int i = 0; i < LoopIterations; i++) {
			results += NonGenericIntClass.IntValue + 2;
		}

		return results;
	}

	[Benchmark("Generics", "Tests a addition using a generic field (Long in this case)")]
	public static long GenericAddLong() {
		long results = 0;
		for (int i = 0; i < LoopIterations; i++) {
			results += GenericLongClass.Value + 2;
		}

		return results;
	}

	[Benchmark("Generics", "Tests a addition using a object field (Note we have to cast to long)")]
	public static long NonGenericCastLong() {
		long results = 0;
		for (int i = 0; i < LoopIterations; i++) {
			results += (long)NonGenericLongClass.Value + 2;
		}

		return results;
	}
}