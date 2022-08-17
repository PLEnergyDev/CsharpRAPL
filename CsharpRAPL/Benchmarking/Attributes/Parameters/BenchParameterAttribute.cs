using System;

namespace CsharpRAPL.Benchmarking.Attributes.Parameters;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class BenchParameterAttribute : Attribute{
	public BenchParameterAttribute(string benchmarkParameterSource) {
		BenchmarkParameterSource = benchmarkParameterSource;
	}
	public string BenchmarkParameterSource { get; set; }
}
