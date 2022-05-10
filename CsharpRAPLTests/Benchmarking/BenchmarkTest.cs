using System;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Variation;
using NUnit.Framework;

namespace CsharpRAPL.Tests.Benchmarking;

public class BenchmarkTest {
	Benchmark<T> CreateBenchmark<T>(string name, ulong iterations, Func<T> benchmark, Type? benchmarkLifecycleClass = null, bool silenceBenchmarkOutput = true,
		string? group = null, int order = 0, int plotOrder = 0) {

		BenchmarkInfo bi = new BenchmarkInfo() {
			Name = name,
			Group = group,
			Iterations = iterations,
			Order = order,
			//Parameters = new VariationInstance(),
			PlotOrder = plotOrder
		};
		return new Benchmark<T>(new NopBenchmarkLifecycle(bi, benchmark.Method), silenceBenchmarkOutput);
	}
		

	[Test]
	public void BenchmarkTest01() {
		var benchmark = CreateBenchmark("DummyBenchmark", 10, BenchmarkSuitTest.DummyBenchmark, typeof(NopBenchmarkLifecycle)); 

		Assert.Null(benchmark.BenchmarkInfo.Group);
		Assert.AreEqual(10, benchmark.BenchmarkInfo.Iterations);
		Assert.False(benchmark.BenchmarkInfo.HasRun);
		Assert.AreEqual("DummyBenchmark", benchmark.BenchmarkInfo.Name);
		Assert.AreEqual(0, benchmark.BenchmarkInfo.Order);
	}

	[Test]
	public void BenchmarkTest02() {
		var benchmark = CreateBenchmark("DummyBenchmark2", 12, BenchmarkSuitTest.DummyBenchmark,null, @group: "Test");

		Assert.NotNull(benchmark.BenchmarkInfo.Group);
		Assert.AreEqual(12, benchmark.BenchmarkInfo.Iterations);
		Assert.False(benchmark.BenchmarkInfo.HasRun);
		Assert.AreEqual("DummyBenchmark2", benchmark.BenchmarkInfo.Name);
		Assert.AreEqual("Test", benchmark.BenchmarkInfo.Group);
		Assert.AreEqual(0, benchmark.BenchmarkInfo.Order);
	}

	[Test]
	public void BenchmarkTest03() {
		var benchmark = CreateBenchmark("DummyBenchmark2", 12, BenchmarkSuitTest.DummyBenchmark,null, order: 21);

		Assert.Null(benchmark.BenchmarkInfo.Group);
		Assert.AreEqual(12, benchmark.BenchmarkInfo.Iterations);
		Assert.False(benchmark.BenchmarkInfo.HasRun);
		Assert.AreEqual("DummyBenchmark2", benchmark.BenchmarkInfo.Name);
		Assert.AreEqual(21, benchmark.BenchmarkInfo.Order);
	}
}