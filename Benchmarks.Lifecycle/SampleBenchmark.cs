﻿using CsharpRAPL.Benchmarking.Attributes;
using CsharpRAPL.Benchmarking.Attributes.Parameters;

namespace Benchmarks.Lifecycle {
	public class VariablesBenchmarks {
		[Benchmark("Variables", "Tests local variables")]
		public static ulong LocalVariable([BenchmarkLoopiterations] ulong loopIterations) {
			ulong localA = 0, localB = 1;
			for (ulong i = 0; i < loopIterations; i++) {
				localA += localB + i;
			}
			return localA;
		}
	}
}