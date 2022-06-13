using System;
using System.Reflection;
using CsharpRAPL.Benchmarking;
using NUnit.Framework;

namespace CsharpRAPL.Tests.Benchmarking;

public class BenchmarkCollectorTest {
	[Test, Order(0)]
	public void BenchmarkCollectorTest01() {
		var collector = new BenchmarkCollector(1000L, 1001L);

		Assert.AreEqual(11, collector.GetBenchmarks().Count);
		Assert.AreEqual(1000, DummyBenchmarks.Iterations);
		Assert.AreEqual(1001, DummyBenchmarks.LoopIterations);
	}

	[Test, Order(1)]
	public void BenchmarkCollectorTest02() {
		var collector = new BenchmarkCollector(999, 1003, true);

		Assert.AreEqual(11, collector.GetBenchmarks().Count);
		Assert.AreEqual(999, DummyBenchmarks.Iterations);
		Assert.AreEqual(1003, DummyBenchmarks.LoopIterations);
	}

	//[Test]
	//public void CheckMethodValidityTest01() {
	//	MethodInfo? methodInfo = typeof(DummyBenchmarks).GetMethod("PreDecrement");
	//	Assert.DoesNotThrow(() => BenchmarkCollector.CheckMethodValidity(methodInfo!));
	//}

	//[Test]
	//public void CheckMethodValidityTest02() {
	//	MethodInfo? methodInfo =
	//		typeof(DummyBenchmarks).GetMethod("PrivateTest", BindingFlags.NonPublic | BindingFlags.Static);
	//	Assert.DoesNotThrow(() => BenchmarkCollector.CheckMethodValidity(methodInfo!));
	//}

	//[Test]
	//public void CheckMethodValidityTest03() {
	//	var methodInfo =
	//		typeof(DummyBenchmarks).GetMethod("VoidTest");
	//	var exception = Assert.Throws<NotSupportedException>(() => BenchmarkCollector.CheckMethodValidity(methodInfo!));
	//	Assert.AreEqual(
	//		"The benchmark 'VoidTest' is returning void which isn't supported.",
	//		exception?.Message);
	//}

	//[Test]
	//public void CheckMethodValidityTest04() {
	//	var methodInfo = typeof(DummyBenchmarks).GetMethod(nameof(DummyBenchmarks.ParamTest));
	//	var exception = Assert.Throws<NotSupportedException>(() => BenchmarkCollector.CheckMethodValidity(methodInfo!));
	//	Assert.AreEqual("The Benchmark 'ParamTest' has parameters which isn't supported.", exception?.Message);
	//}


}