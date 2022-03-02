using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Variation;
using CsharpRAPL.CommandLine;
using NUnit.Framework;

namespace CsharpRAPL.Tests.Benchmarking;

public class BenchmarkSuitTest {
	public static ulong Iterations;
	public static ulong LoopIterations;

	public static int DummyBenchmark() {
		int result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			Thread.Sleep(10);
		}

		return result;
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
	public void TestRegisterBenchmark06() {
		var benchmarkSuit = new BenchmarkSuite();

		benchmarkSuit.RegisterBenchmark("TestBenchmark", "TestGroup", DummyBenchmark, 42);

		Assert.AreEqual("TestGroup", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Group);
		Assert.AreEqual(1, benchmarkSuit.GetBenchmarks().Count);
		Assert.AreEqual(50, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Iterations);
		Assert.False(benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.HasRun);
		Assert.AreEqual("TestBenchmark", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Name);
		Assert.AreEqual(42, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Order);
	}

	[Test]
	public void TestRegisterBenchmarkVariation01() {
		var benchmarkSuit = new BenchmarkSuite();

		benchmarkSuit.RegisterBenchmarkVariation("TestGroup", DummyBenchmark, new VariationInstance() { Values = { } });

		Assert.AreEqual("TestGroup", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Group);
		Assert.AreEqual(1, benchmarkSuit.GetBenchmarks().Count);
		Assert.AreEqual(50, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Iterations);
		Assert.False(benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.HasRun);
		Assert.AreEqual("DummyBenchmark", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Name);
		Assert.AreEqual(0, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Order);
	}

	[Test]
	public void TestRegisterBenchmarkVariation02() {
		var benchmarkSuit = new BenchmarkSuite();

		benchmarkSuit.RegisterBenchmarkVariation("TestBenchmark", "TestGroup", DummyBenchmark, new VariationInstance(),
			42);

		Assert.AreEqual("TestGroup", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Group);
		Assert.AreEqual(1, benchmarkSuit.GetBenchmarks().Count);
		Assert.AreEqual(50, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Iterations);
		Assert.False(benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.HasRun);
		Assert.AreEqual("TestBenchmark", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Name);
		Assert.AreEqual(42, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Order);
	}

	[Test]
	public void TestRegisterBenchmarkVariation03() {
		var benchmarkSuit = new BenchmarkSuite();
		var exception =
			Assert.Throws<NotSupportedException>(() =>
				benchmarkSuit.RegisterBenchmarkVariation("TestGroup", () => { return "Test"; },
					new VariationInstance()));
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

	[Test]
	public void TestRunAll01() {
		CsharpRAPLCLI.Options.OnlyTime = true;
		var benchmarkSuit = new BenchmarkSuite();
		using var sw = new StringWriter();
		Console.SetOut(sw);
		benchmarkSuit.RunAll();

		string result = sw.ToString().Trim();
		Assert.AreEqual("There are no benchmarks to run.", result);


		var standardOutput = new StreamWriter(Console.OpenStandardOutput());
		standardOutput.AutoFlush = true;
		Console.SetOut(standardOutput);
		CsharpRAPLCLI.Options.OnlyTime = false;
	}

	[Test]
	public void TestPlotGroups01() {
		var benchmarkSuit = new BenchmarkSuite();

		benchmarkSuit.RegisterBenchmark("1", "TestGroup", DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("2", "TestGroup", DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("3", "TestGroup2", DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("4", "TestGroup2", DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("5", "TestGroup2", DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("6", "TestGroup3", DummyBenchmark, 42);
		benchmarkSuit.RegisterBenchmark("7", "TestGroup", DummyBenchmark, 42);
		
		
		var exception = Assert.Throws<NotSupportedException>(() => benchmarkSuit.PlotGroups());
		Assert.AreEqual(exception?.Message, "Plotting without data is not supported.");
		
	}
}