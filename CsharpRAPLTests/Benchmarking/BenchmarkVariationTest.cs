using CsharpRAPL.Benchmarking.Attributes;

namespace CsharpRAPL.Tests.Benchmarking;

public class BenchmarkVariationTest {
	public static ulong Iterations;
	public static ulong LoopIterations;
	[Variations(10, 15)] public int TestProp { get; set; }
	[Variations(21, 12)] public int TestProp2 { get; set; }
	[Variations(101, 69)] public int TestField;
	[Variations(54, 43)] public int TestField2;

	[Benchmark("Test", "Tests Variations", skip: true)]
	public void TestBenchmark() {
		var res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			res += TestProp + TestProp2 + TestField + TestField2;
		}
	}
}