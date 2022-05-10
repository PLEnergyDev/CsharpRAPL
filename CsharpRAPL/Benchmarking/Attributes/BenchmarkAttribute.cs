using System;

namespace CsharpRAPL.Benchmarking.Attributes;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class BenchParameterAttribute : Attribute{
	public BenchParameterAttribute(string benchmarkParameterSource) {
		BenchmarkParameterSource = benchmarkParameterSource;
	}
	public string BenchmarkParameterSource { get; set; }
}

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class BenchmarkLoopiterationsAttribute : BenchParameterAttribute {
	public BenchmarkLoopiterationsAttribute() : base("LoopIterations") {
	}
}
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class BencharkIterationsAttribute : BenchParameterAttribute {
	public BencharkIterationsAttribute() : base("Iterations") {
	}
}



[AttributeUsage(AttributeTargets.Method)]
public class PreBenchmarkAttribute : Attribute {
	public string Target { get; set; }
	public PreBenchmarkAttribute(string target) {
		Target = target;
	}
}

[AttributeUsage(AttributeTargets.Method)]
public class BenchmarkAttribute : Attribute {
	public string Name { get; }

	public string? Group { get; }

	public string Description { get; }
	public int Order { get; }

	public bool Skip { get; }

	public int PlotOrder { get; }

	public Type? BenchmarkLifecycleClass { get; }
	public BenchmarkAttribute(string? group, string description, Type? benchmarkLifecycleClass=null, int order = 0, bool skip = false,
		string name = "", int plotOrder = 0) {
		BenchmarkLifecycleClass = benchmarkLifecycleClass;
		Group = group;
		Description = description;
		Order = order;
		PlotOrder = plotOrder;
		Skip = skip;
		Name = name;
	}
}