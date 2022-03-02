using System;
using CsharpRAPL.Benchmarking.Attributes;

namespace CsharpRAPL.Tests.Benchmarking;

public class TypeVariationExample {
	public static ulong Iterations;
	public static ulong LoopIterations;

	[TypeVariations(typeof(NotSupportedException), typeof(OutOfMemoryException))]
	public Exception Exception = new Exception();

	[VariationBenchmark("TypeVariations", "Tests Variations")]
	public int TestBenchmark() {
		var res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			res += Exception.GetType() == typeof(NotSupportedException) ? 10 : 1;
		}

		return res;
	}
}