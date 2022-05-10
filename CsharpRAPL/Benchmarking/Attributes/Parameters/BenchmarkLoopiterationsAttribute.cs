using System;

namespace CsharpRAPL.Benchmarking.Attributes;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class BenchmarkLoopiterationsAttribute : BenchParameterAttribute {
	public BenchmarkLoopiterationsAttribute() : base("LoopIterations") {
	}
}
