using System;

namespace CsharpRAPL.Benchmarking.Attributes.Parameters;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class BenchmarkIterationsAttribute : BenchParameterAttribute {
	public BenchmarkIterationsAttribute() : base("Iterations") {
	}
}
