using System;
using System.Collections.Generic;
using CsharpRAPL.Benchmarking;
using NUnit.Framework;

namespace CsharpRAPL.Tests.Benchmarking;

public class BenchmarkSuitTest {
	public static int DummyBenchmark() {
		return 1;
	}

	[Test]
	public void TestRegisterBenchmark01() {
		var benchmarkSuit = new BenchmarkSuite();
		benchmarkSuit.RegisterBenchmark(10, DummyBenchmark);

		Assert.AreEqual(1, benchmarkSuit.GetBenchmarks().Count);
		Assert.AreEqual(10, benchmarkSuit.GetBenchmarks()[0].Iterations);
		Assert.False(benchmarkSuit.GetBenchmarks()[0].HasRun);
		Assert.AreEqual("DummyBenchmark", benchmarkSuit.GetBenchmarks()[0].Name);
		Assert.AreEqual(0, benchmarkSuit.GetBenchmarks()[0].Order);
	}

	[Test]
	public void TestRegisterBenchmark02() {
		var benchmarkSuit = new BenchmarkSuite();


		benchmarkSuit.RegisterBenchmark("TestGroup", 13, DummyBenchmark);


		Assert.AreEqual("TestGroup", benchmarkSuit.GetBenchmarks()[0].Group);
		Assert.AreEqual(1, benchmarkSuit.GetBenchmarks().Count);
		Assert.AreEqual(13, benchmarkSuit.GetBenchmarks()[0].Iterations);
		Assert.False(benchmarkSuit.GetBenchmarks()[0].HasRun);
		Assert.AreEqual("DummyBenchmark", benchmarkSuit.GetBenchmarks()[0].Name);
		Assert.AreEqual(0, benchmarkSuit.GetBenchmarks()[0].Order);
	}


	[Test]
	public void TestRegisterBenchmark03() {
		var benchmarkSuit = new BenchmarkSuite();


		benchmarkSuit.RegisterBenchmark("TestGroup", 12, DummyBenchmark, 42);


		Assert.AreEqual("TestGroup", benchmarkSuit.GetBenchmarks()[0].Group);
		Assert.AreEqual(1, benchmarkSuit.GetBenchmarks().Count);
		Assert.AreEqual(12, benchmarkSuit.GetBenchmarks()[0].Iterations);
		Assert.False(benchmarkSuit.GetBenchmarks()[0].HasRun);
		Assert.AreEqual("DummyBenchmark", benchmarkSuit.GetBenchmarks()[0].Name);
		Assert.AreEqual(42, benchmarkSuit.GetBenchmarks()[0].Order);
	}

	[Test]
	public void TestRegisterBenchmark04() {
		var benchmarkSuit = new BenchmarkSuite();
		var exception =
			Assert.Throws<NotSupportedException>(() => benchmarkSuit.RegisterBenchmark(10, () => "Test"));
		Assert.AreEqual(exception.Message, "Adding benchmarks through anonymous methods is not supported");
	}

	[Test]
	public void TestRegisterBenchmark05() {
		var benchmarkSuit = new BenchmarkSuite();
		var exception =
			Assert.Throws<NotSupportedException>(() => benchmarkSuit.RegisterBenchmark(10, () => { return "Test"; }));
		Assert.AreEqual(exception.Message, "Adding benchmarks through anonymous methods is not supported");
	}

	[Test]
	public void TestGetBenchmarksByGroup() {
		var benchmarkSuit = new BenchmarkSuite();


		benchmarkSuit.RegisterBenchmark("TestGroup", 12, DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup", 12, DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup2", 12, DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup2", 12, DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup2", 12, DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup3", 12, DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup", 12, DummyBenchmark, 42);

		Dictionary<string, List<IBenchmark>> groups = benchmarkSuit.GetBenchmarksByGroup();

		Assert.AreEqual(3, groups.Keys.Count);
		Assert.AreEqual(3, groups["TestGroup"].Count);
		Assert.AreEqual(3, groups["TestGroup2"].Count);
		Assert.AreEqual(1, groups["TestGroup3"].Count);
	}

	[Test]
	public void TestGetBenchmarksByGroup02() {
		var benchmarkSuit = new BenchmarkSuite();
		Dictionary<string, List<IBenchmark>> groups = benchmarkSuit.GetBenchmarksByGroup();
		Assert.IsEmpty(groups);
	}
}