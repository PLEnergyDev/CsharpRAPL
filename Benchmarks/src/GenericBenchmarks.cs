using System.Diagnostics.CodeAnalysis;
using Benchmarks.HelperObjects;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class GenericBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;

	public static readonly Generic<ulong> GenericIntClass = new(10);
	public static readonly NonGeneric NonGenericIntClass = new(10);

	public static readonly Generic<long> GenericLongClass = new(10);
	public static readonly NonGeneric NonGenericLongClass = new(10L);

	[Benchmark("Generics", "Tests a addition using a generic field (ulong in this case)")]
	public static ulong GenericAddInt() {
		ulong results = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			results += GenericIntClass.Value + 2;
		}

		return results;
	}

	[Benchmark("Generics", "Tests a addition using a object field (Note we have to cast to ulong)")]
	public static ulong NonGenericCastInt() {
		ulong results = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			results += (ulong)NonGenericIntClass.Value + 2;
		}

		return results;
	}

	[Benchmark("Generics", "Tests a addition using a ulong field (Note we have to cast to ulong)")]
	public static ulong NonGenericInt() {
		ulong results = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			results += NonGenericIntClass.ULongValue + 2;
		}

		return results;
	}

	[Benchmark("Generics", "Tests a addition using a generic field (Long in this case)")]
	public static long GenericAddLong() {
		long results = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			results += GenericLongClass.Value + 2;
		}

		return results;
	}

	[Benchmark("Generics", "Tests a addition using a object field (Note we have to cast to long)")]
	public static long NonGenericCastLong() {
		long results = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			results += (long)NonGenericLongClass.Value + 2;
		}

		return results;
	}
}