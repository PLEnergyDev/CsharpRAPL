using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks.Lifecycle {
	class SampleBenchmark {
		public class VariablesBenchmarks {
			public static ulong Iterations;
			public static ulong LoopIterations;


			[Benchmark("Variables", "Tests local variables")]
			public static ulong LocalVariable() {
				ulong localA = 0, localB = 1;
				for (ulong i = 0; i < LoopIterations; i++) {
					localA += localB + i;
				}

				return localA;
			}
		}
	}
}