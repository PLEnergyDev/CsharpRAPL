using CsharpRAPL.Benchmarking;
using NUnit.Framework;

namespace CsharpRAPL.Tests.Benchmarking;

public class BenchmarkTest {
	[Test]
	public void BenchmarkTest01() {
		var benchmark = new Benchmark<int>("DummyBenchmark", 10, BenchmarkSuitTest.DummyBenchmark);

		Assert.Null(benchmark.BenchmarkInfo.Group);
		Assert.AreEqual(10, benchmark.BenchmarkInfo.Iterations);
		Assert.False(benchmark.BenchmarkInfo.HasRun);
		Assert.AreEqual("DummyBenchmark", benchmark.BenchmarkInfo.Name);
		Assert.AreEqual(0, benchmark.BenchmarkInfo.Order);
	}

	[Test]
	public void BenchmarkTest02() {
		var benchmark = new Benchmark<int>("DummyBenchmark2", 12, BenchmarkSuitTest.DummyBenchmark, @group: "Test");

		Assert.NotNull(benchmark.BenchmarkInfo.Group);
		Assert.AreEqual(12, benchmark.BenchmarkInfo.Iterations);
		Assert.False(benchmark.BenchmarkInfo.HasRun);
		Assert.AreEqual("DummyBenchmark2", benchmark.BenchmarkInfo.Name);
		Assert.AreEqual("Test", benchmark.BenchmarkInfo.Group);
		Assert.AreEqual(0, benchmark.BenchmarkInfo.Order);
	}

	[Test]
	public void BenchmarkTest03() {
		var benchmark = new Benchmark<int>("DummyBenchmark2", 12, BenchmarkSuitTest.DummyBenchmark, order: 21);

		Assert.Null(benchmark.BenchmarkInfo.Group);
		Assert.AreEqual(12, benchmark.BenchmarkInfo.Iterations);
		Assert.False(benchmark.BenchmarkInfo.HasRun);
		Assert.AreEqual("DummyBenchmark2", benchmark.BenchmarkInfo.Name);
		Assert.AreEqual(21, benchmark.BenchmarkInfo.Order);
	}
}