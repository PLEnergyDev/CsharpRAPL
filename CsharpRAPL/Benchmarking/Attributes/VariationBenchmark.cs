using System;

namespace CsharpRAPL.Benchmarking.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class VariationBenchmark : BenchmarkAttribute {
	public VariationBenchmark(string? group, string description, int order = 0, bool skip = false) :
		base(group, description, order, skip) { }
}