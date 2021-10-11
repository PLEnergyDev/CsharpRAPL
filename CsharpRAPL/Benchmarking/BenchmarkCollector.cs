using System;
using System.Linq;
using System.Reflection;

namespace CsharpRAPL.Benchmarking {
	public class BenchmarkCollector : BenchmarkSuite {
		public int Iterations { get; }

		public BenchmarkCollector(int iterations, bool onlyCalling = true) {
			Iterations = iterations;
			if (onlyCalling)
				CollectBenchmarks(Assembly.GetCallingAssembly());
			else {
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
					CollectBenchmarks(assembly);
				}
			}
		}

		private void CollectBenchmarks(Assembly assembly) {
			foreach (MethodInfo methodInfo in assembly.GetTypes().SelectMany(type => type.GetMethods())) {
				var benchmarkAttribute = methodInfo.GetCustomAttribute<BenchmarkAttribute>();
				if (benchmarkAttribute == null) continue;
				AddBenchmark(benchmarkAttribute.Group, Iterations, methodInfo.CreateDelegate<Func<int>>(),
					benchmarkAttribute.Order);
			}
		}
	}
}