using System;

namespace CsharpRAPL.Benchmarking.Attributes.Parameters;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class BenchmarkLoopiterationsAttribute : BenchParameterAttribute {
	public BenchmarkLoopiterationsAttribute() : base("LoopIterations") {
	}
}
