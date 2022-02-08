using System;
using System.Collections.Generic;
using CsharpRAPL.Benchmarking;
using NUnit.Framework;

namespace CsharpRAPL.Tests.Benchmarking;

public class BenchmarkSuitTest {
	public static ulong Iterations;
	public static ulong LoopIterations;

	public static int DummyBenchmark() {
		return 1;
	}

	[Test]
	public void TestRegisterBenchmark01() {
		var benchmarkSuit = new BenchmarkSuite(10, 29);
		benchmarkSuit.RegisterBenchmark(DummyBenchmark);

		Assert.AreEqual(1, benchmarkSuit.GetBenchmarks().Count);
		Assert.AreEqual(10, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Iterations);
		Assert.False(benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.HasRun);
		Assert.AreEqual("DummyBenchmark", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Name);
		Assert.AreEqual(0, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Order);
	}

	[Test]
	public void TestRegisterBenchmark02() {
		var benchmarkSuit = new BenchmarkSuite();


		benchmarkSuit.RegisterBenchmark("TestGroup", DummyBenchmark);


		Assert.AreEqual("TestGroup", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Group);
		Assert.AreEqual(1, benchmarkSuit.GetBenchmarks().Count);
		Assert.AreEqual(50, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Iterations);
		Assert.False(benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.HasRun);
		Assert.AreEqual("DummyBenchmark", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Name);
		Assert.AreEqual(0, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Order);
	}


	[Test]
	public void TestRegisterBenchmark03() {
		var benchmarkSuit = new BenchmarkSuite();


		benchmarkSuit.RegisterBenchmark("TestGroup", DummyBenchmark, 42);


		Assert.AreEqual("TestGroup", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Group);
		Assert.AreEqual(1, benchmarkSuit.GetBenchmarks().Count);
		Assert.AreEqual(50, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Iterations);
		Assert.False(benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.HasRun);
		Assert.AreEqual("DummyBenchmark", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Name);
		Assert.AreEqual(42, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Order);
	}

	[Test]
	public void TestRegisterBenchmark04() {
		var benchmarkSuit = new BenchmarkSuite();
		var exception =
			Assert.Throws<NotSupportedException>(() => benchmarkSuit.RegisterBenchmark(() => "Test"));
		Assert.AreEqual(exception?.Message, "Adding benchmarks through anonymous methods is not supported");
	}

	[Test]
	public void TestRegisterBenchmark05() {
		var benchmarkSuit = new BenchmarkSuite();
		var exception =
			Assert.Throws<NotSupportedException>(() => benchmarkSuit.RegisterBenchmark(() => { return "Test"; }));
		Assert.AreEqual(exception?.Message, "Adding benchmarks through anonymous methods is not supported");
	}

	[Test]
	public void TestGetBenchmarksByGroup() {
		var benchmarkSuit = new BenchmarkSuite();


		benchmarkSuit.RegisterBenchmark("TestGroup", DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup", DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup2", DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup2", DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup2", DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup3", DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup", DummyBenchmark, 42);

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