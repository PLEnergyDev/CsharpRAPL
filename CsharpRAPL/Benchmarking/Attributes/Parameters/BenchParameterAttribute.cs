using System;

namespace CsharpRAPL.Benchmarking.Attributes;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class BenchParameterAttribute : Attribute{
	public BenchParameterAttribute(string benchmarkParameterSource) {
		BenchmarkParameterSource = benchmarkParameterSource;
	}
	public string BenchmarkParameterSource { get; set; }
}
