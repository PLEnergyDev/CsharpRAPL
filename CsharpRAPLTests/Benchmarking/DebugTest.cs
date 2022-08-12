using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;
using CsharpRAPL.Benchmarking.Attributes.Parameters;
using CsharpRAPL.CommandLine;
using NUnit.Framework;

namespace CsharpRAPL.Tests.Benchmarking;

public class DebugTest {
	[Benchmark("grp", "desc")]
	public int TestFoo([BenchmarkLoopiterations] ulong liter) {
		return 10;
	}
	[Test]
	public void Foo() {
		BenchmarkCollector collector = new BenchmarkCollector();
		CsharpRAPLCLI.Options.OnlyTime = true;
		var bms = collector.GetBenchmarks();
		var bm = bms[0];
		bm.Run();
	}
}
