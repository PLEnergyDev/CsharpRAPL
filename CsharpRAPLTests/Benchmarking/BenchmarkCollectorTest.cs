using System;
using System.Reflection;
using CsharpRAPL.Benchmarking;
using NUnit.Framework;

namespace CsharpRAPL.Tests.Benchmarking;

public class BenchmarkCollectorTest {
	[Test, Order(0)]
	public void BenchmarkCollectorTest01() {
		var collector = new BenchmarkCollector(1000, 1001);

		Assert.AreEqual(3, collector.GetBenchmarks().Count);
		Assert.AreEqual(1000, DummyBenchmarks.Iterations);
		Assert.AreEqual(1001, DummyBenchmarks.LoopIterations);
	}

	[Test, Order(1)]
	public void BenchmarkCollectorTest02() {
		var collector = new BenchmarkCollector(999, 1003, false);

		Assert.AreEqual(3, collector.GetBenchmarks().Count);
		Assert.AreEqual(999, DummyBenchmarks.Iterations);
		Assert.AreEqual(1003, DummyBenchmarks.LoopIterations);
	}

	[Test]
	public void CheckMethodValidityTest01() {
		MethodInfo? methodInfo = typeof(DummyBenchmarks).GetMethod("PreDecrement");
		Assert.DoesNotThrow(() => BenchmarkCollector.CheckMethodValidity(methodInfo!));
	}

	[Test]
	public void CheckMethodValidityTest02() {
		MethodInfo? methodInfo =
			typeof(DummyBenchmarks).GetMethod("PrivateTest", BindingFlags.NonPublic | BindingFlags.Static);
		var exception = Assert.Throws<NotSupportedException>(() => BenchmarkCollector.CheckMethodValidity(methodInfo!));
		Assert.AreEqual("The benchmark attribute is only supported and supposed to be used on public methods.",
			exception?.Message);
	}

	[Test]
	public void CheckMethodValidityTest03() {
		var methodInfo =
			typeof(DummyBenchmarks).GetMethod("VoidTest");
		var exception = Assert.Throws<NotSupportedException>(() => BenchmarkCollector.CheckMethodValidity(methodInfo!));
		Assert.AreEqual(
			"The benchmark attribute is only supported and supposed to be used on methods with a non void return type.",
			exception?.Message);
	}

	[Test]
	public void TrySetFieldTest01() {
		Assert.AreEqual(0, DummyBenchmarks.TestField1);
		BenchmarkSuite.TrySetField(typeof(DummyBenchmarks), "TestField1", 10);
		Assert.AreEqual(10, DummyBenchmarks.TestField1);
	}

	[Test]
	public void TrySetFieldTest02() {
		var exception = Assert.Throws<NotSupportedException>(() =>
			BenchmarkSuite.TrySetField(typeof(DummyBenchmarks), "TestField2", 10));
		Assert.AreEqual("Your TestField2 field must be static.", exception?.Message);
	}

	[Test]
	public void TrySetFieldTest03() {
		Assert.False(BenchmarkSuite.TrySetField(typeof(DummyBenchmarks), "_testField3", 10));
	}
}