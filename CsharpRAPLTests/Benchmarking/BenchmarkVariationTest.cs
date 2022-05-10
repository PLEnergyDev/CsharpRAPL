using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;
using NUnit.Framework;

namespace CsharpRAPL.Tests.Benchmarking;

public class BenchmarkVariationTest {
	public static ulong Iterations;
	public static ulong LoopIterations;
	[Variations(10, 15)] public int TestProp { get; set; }
	[Variations(101, 69)] public int TestField;
	[VariationBenchmark("Variations", "Tests Variations")]
	public int TestBenchmark() {
		var res = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			res += TestProp + TestField;
		}

		return res;
	}

	[Test]
	public void BenchmarkVariationTest01() {
		BenchmarkCollector collector = new BenchmarkCollector();

		Assert.AreEqual(11, collector.GetBenchmarks().Count);
	}

	//[Test]
	//public void BenchmarkVariationTest02() {
	//	var collector = new BenchmarkCollector();

	//	Assert.AreEqual(5, collector.GetBenchmarksByGroup()["Variations"].Count);

	//	List<VariationInstance.MemberInfo> var0 = collector.GetBenchmarksByGroup()["Variations"][0].BenchmarkInfo
	//		.Parameters.Values;
	//	var var0Expected = new List<VariationInstance.MemberInfo> {
	//		new(nameof(TestProp), 10, false),
	//		new(nameof(TestField), 101, true)
	//	};
	//	Assert.That(var0, Is.EquivalentTo(var0Expected));


	//	List<VariationInstance.MemberInfo> var1 = collector.GetBenchmarksByGroup()["Variations"][1].BenchmarkInfo
	//		.Parameters.Values;
	//	var var1Expected = new List<VariationInstance.MemberInfo> {
	//		new(nameof(TestProp), 15, false),
	//		new(nameof(TestField), 101, true)
	//	};
	//	Assert.That(var1, Is.EquivalentTo(var1Expected));

	//	List<VariationInstance.MemberInfo> var2 = collector.GetBenchmarksByGroup()["Variations"][2].BenchmarkInfo
	//		.Parameters.Values;
	//	var var2Expected = new List<VariationInstance.MemberInfo> {
	//		new(nameof(TestProp), 10, false),
	//		new(nameof(TestField), 69, true)
	//	};
	//	Assert.That(var2, Is.EquivalentTo(var2Expected));

	//	List<VariationInstance.MemberInfo> var3 = collector.GetBenchmarksByGroup()["Variations"][3].BenchmarkInfo
	//		.Parameters.Values;
	//	var var3Expected = new List<VariationInstance.MemberInfo> {
	//		new(nameof(TestProp), 15, false),
	//		new(nameof(TestField), 69, true)
	//	};
	//	Assert.That(var3, Is.EquivalentTo(var3Expected));

	//	List<VariationInstance.MemberInfo> var4 = collector.GetBenchmarksByGroup()["Variations"][4].BenchmarkInfo
	//		.Parameters.Values;
	//	var var4Expected = new List<VariationInstance.MemberInfo>();
	//	Assert.That(var4, Is.EquivalentTo(var4Expected));
	//}

	//[Test]
	//public void BenchmarkVariationTest03() {
	//	var collector = new BenchmarkCollector();
	//	Assert.AreEqual(3, collector.GetBenchmarksByGroup()["TypeVariations"].Count);
	//	List<VariationInstance.MemberInfo> var0 = collector.GetBenchmarksByGroup()["TypeVariations"][0].BenchmarkInfo
	//		.Parameters.Values;
		
	//	List<VariationInstance.MemberInfo> var1 = collector.GetBenchmarksByGroup()["TypeVariations"][1].BenchmarkInfo
	//		.Parameters.Values;
		
	//	List<VariationInstance.MemberInfo> var2 = collector.GetBenchmarksByGroup()["TypeVariations"][2].BenchmarkInfo
	//		.Parameters.Values;
		
		
	//	Assert.AreEqual(typeof(NotSupportedException),var0[0].Value.GetType());
	//	Assert.AreEqual(typeof(OutOfMemoryException),var1[0].Value.GetType());
	//	Assert.IsEmpty(var2);
	//}
}