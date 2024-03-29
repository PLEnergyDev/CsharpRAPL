﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;
using CsharpRAPL.Benchmarking.Lifecycles;
using CsharpRAPL.CommandLine;
using NUnit.Framework;

namespace CsharpRAPL.Tests.Benchmarking;

public static class BSExt {

	public static void RegisterBenchmark<T>(this BenchmarkSuite bs, string name, string? group, Func<T> benchmark, Type benchmarkLifecycleClass, int order = 0, int plotOrder = 0) {
		if (benchmark.Method.IsAnonymous()) {
			throw new NotSupportedException("Adding benchmarks through anonymous methods is not supported");
		}
		var attr = new BenchmarkAttribute(group, name, benchmarkLifecycleClass, order, false, name, plotOrder);
		bs.RegisterBenchmark(benchmark.Method, attr);
		//if (!_registeredBenchmarkClasses.Contains(benchmark.Method.DeclaringType!)) {
		//	RegisterBenchmarkClass(benchmark.Method.DeclaringType!);
		//}
		//IBenchmarkLifecycle bs;
		//Benchmarks.Add(new Benchmark<T>(name, Iterations, benchmark, benchmarkLifecycleClass, true, group, order, plotOrder));
	}

	public static void RegisterBenchmark<T>(this BenchmarkSuite bs, string? group, Func<T> benchmark, Type benchmarkLifecycleClass, int order = 0) {
		bs.RegisterBenchmark(benchmark.Method.Name, group, benchmark, benchmarkLifecycleClass, order);
	}
	public static void RegisterBenchmark<T>(this BenchmarkSuite bs, Func<T> benchmark, Type benchmarkLifecycleClass, int order = 0) {
		bs.RegisterBenchmark(null, benchmark, benchmarkLifecycleClass, order);
	}
}

public class BenchmarkSuitTest {
	public static ulong Iterations;
	public static ulong LoopIterations;


	public static Type DummyPrerun => typeof(NopBenchmarkLifecycle);
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
		benchmarkSuit.RegisterBenchmark(DummyBenchmark, DummyPrerun);

		Assert.AreEqual(1, benchmarkSuit.GetBenchmarks().Count);
		Assert.AreEqual(10, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Iterations);
		Assert.False(benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.HasRun);
		Assert.AreEqual("DummyBenchmark", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Name);
		Assert.AreEqual(0, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Order);
	}

	[Test]
	public void TestRegisterBenchmark02() {
		var benchmarkSuit = new BenchmarkSuite();


		benchmarkSuit.RegisterBenchmark("TestGroup", DummyBenchmark, DummyPrerun);


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

		benchmarkSuit.RegisterBenchmark("TestGroup", DummyBenchmark, DummyPrerun, 42);

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
			Assert.Throws<NotSupportedException>(() => benchmarkSuit.RegisterBenchmark(() => "Test", DummyPrerun));
		Assert.AreEqual(exception?.Message, "Adding benchmarks through anonymous methods is not supported");
	}

	[Test]
	public void TestRegisterBenchmark05() {
		var benchmarkSuit = new BenchmarkSuite();
		var exception =
			Assert.Throws<NotSupportedException>(() => benchmarkSuit.RegisterBenchmark(() => { return "Test"; }, DummyPrerun));
		Assert.AreEqual(exception?.Message, "Adding benchmarks through anonymous methods is not supported");
	}

	[Test]
	public void TestRegisterBenchmark06() {
		var benchmarkSuit = new BenchmarkSuite();

		benchmarkSuit.RegisterBenchmark("TestBenchmark", "TestGroup", DummyBenchmark, DummyPrerun, 42);

		Assert.AreEqual("TestGroup", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Group);
		Assert.AreEqual(1, benchmarkSuit.GetBenchmarks().Count);
		Assert.AreEqual(50, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Iterations);
		Assert.False(benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.HasRun);
		Assert.AreEqual("TestBenchmark", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Name);
		Assert.AreEqual(42, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Order);
	}

	//[Test]
	//public void TestRegisterBenchmarkVariation01() {
	//	var benchmarkSuit = new BenchmarkSuite();

	//	benchmarkSuit.RegisterBenchmarkVariation("TestGroup", DummyBenchmark, DummyPrerun, new VariationInstance() { Values = { } });

	//	Assert.AreEqual("TestGroup", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Group);
	//	Assert.AreEqual(1, benchmarkSuit.GetBenchmarks().Count);
	//	Assert.AreEqual(50, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Iterations);
	//	Assert.False(benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.HasRun);
	//	Assert.AreEqual("DummyBenchmark", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Name);
	//	Assert.AreEqual(0, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Order);
	//}

	//[Test]
	//public void TestRegisterBenchmarkVariation02() {
	//	var benchmarkSuit = new BenchmarkSuite();

	//	benchmarkSuit.RegisterBenchmarkVariation("TestBenchmark", "TestGroup", DummyBenchmark, null, new VariationInstance(),
	//		42);

	//	Assert.AreEqual("TestGroup", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Group);
	//	Assert.AreEqual(1, benchmarkSuit.GetBenchmarks().Count);
	//	Assert.AreEqual(50, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Iterations);
	//	Assert.False(benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.HasRun);
	//	Assert.AreEqual("TestBenchmark", benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Name);
	//	Assert.AreEqual(42, benchmarkSuit.GetBenchmarks()[0].BenchmarkInfo.Order);
	//}

	//[Test]
	//public void TestRegisterBenchmarkVariation03() {
	//	var benchmarkSuit = new BenchmarkSuite();
	//	var exception =
	//		Assert.Throws<NotSupportedException>(() =>
	//			benchmarkSuit.RegisterBenchmarkVariation("TestGroup", () => { return "Test"; }, DummyPrerun,
	//				new VariationInstance()));
	//	Assert.AreEqual(exception?.Message, "Adding benchmarks through anonymous methods is not supported");
	//}


	[Test]
	public void TestGetBenchmarksByGroup() {
		var benchmarkSuit = new BenchmarkSuite();

		benchmarkSuit.RegisterBenchmark("TestGroup", DummyBenchmark, DummyPrerun, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup", DummyBenchmark, DummyPrerun, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup2", DummyBenchmark, DummyPrerun, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup2", DummyBenchmark, DummyPrerun, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup2", DummyBenchmark, DummyPrerun, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup3", DummyBenchmark, DummyPrerun, 42);
		benchmarkSuit.RegisterBenchmark("TestGroup", DummyBenchmark, DummyPrerun, 42);

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

		benchmarkSuit.RegisterBenchmark("1", "TestGroup", DummyBenchmark, DummyPrerun, 42);
		benchmarkSuit.RegisterBenchmark("2", "TestGroup", DummyBenchmark, DummyPrerun, 42);
		benchmarkSuit.RegisterBenchmark("3", "TestGroup2", DummyBenchmark, DummyPrerun, 42);
		benchmarkSuit.RegisterBenchmark("4", "TestGroup2", DummyBenchmark, DummyPrerun, 42);
		benchmarkSuit.RegisterBenchmark("5", "TestGroup2", DummyBenchmark, DummyPrerun, 42);
		benchmarkSuit.RegisterBenchmark("6", "TestGroup3", DummyBenchmark, DummyPrerun, 42);
		benchmarkSuit.RegisterBenchmark("7", "TestGroup", DummyBenchmark, DummyPrerun, 42);
		
		
		var exception = Assert.Throws<NotSupportedException>(() => benchmarkSuit.PlotGroups());
		Assert.AreEqual(exception?.Message, "Plotting without data is not supported.");
		
	}
}