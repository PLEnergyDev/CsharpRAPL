using System;

namespace CsharpRAPL.Benchmarking.Attributes;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class BencharkIterationsAttribute : BenchParameterAttribute {
	public BencharkIterationsAttribute() : base("Iterations") {
	}
}
