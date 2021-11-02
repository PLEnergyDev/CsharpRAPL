using CsharpRAPL.Benchmarking;
using NUnit.Framework;

namespace CsharpRAPL.Tests.Benchmarking;

public class BenchmarkTest {
	[Test]
	public void BenchmarkTest01() {
		var benchmark = new Benchmark<int>("DummyBenchmark", 10, BenchmarkSuitTest.DummyBenchmark);

		Assert.Null(benchmark.Group);
		Assert.AreEqual(10, benchmark.Iterations);
		Assert.False(benchmark.HasRun);
		Assert.AreEqual("DummyBenchmark", benchmark.Name);
		Assert.AreEqual(0, benchmark.Order);
	}

	[Test]
	public void BenchmarkTest02() {
		var benchmark = new Benchmark<int>("DummyBenchmark2", 12, BenchmarkSuitTest.DummyBenchmark, @group: "Test");

		Assert.NotNull(benchmark.Group);
		Assert.AreEqual(12, benchmark.Iterations);
		Assert.False(benchmark.HasRun);
		Assert.AreEqual("DummyBenchmark2", benchmark.Name);
		Assert.AreEqual("Test", benchmark.Group);
		Assert.AreEqual(0, benchmark.Order);
	}

	[Test]
	public void BenchmarkTest03() {
		var benchmark = new Benchmark<int>("DummyBenchmark2", 12, BenchmarkSuitTest.DummyBenchmark, order: 21);

		Assert.Null(benchmark.Group);
		Assert.AreEqual(12, benchmark.Iterations);
		Assert.False(benchmark.HasRun);
		Assert.AreEqual("DummyBenchmark2", benchmark.Name);
		Assert.AreEqual(21, benchmark.Order);
	}
}