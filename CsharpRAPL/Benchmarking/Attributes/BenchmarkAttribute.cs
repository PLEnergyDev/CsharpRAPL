﻿using System;
using CsharpRAPL.Benchmarking.Lifecycles;

namespace CsharpRAPL.Benchmarking.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class BenchmarkAttribute : Attribute {
	public string Name { get; }

	public string? Group { get; }

	public string Description { get; }
	public int Order { get; }

	public bool Skip { get; }

	public int PlotOrder { get; }
	public ulong LoopIterations { get; }

	public Type BenchmarkLifecycleClass { get; }
	public BenchmarkAttribute(string? group, string description, Type? benchmarkLifecycleClass=null, int order = 0, bool skip = false,
		string name = "", int plotOrder = 0, ulong loopIterations=0) { ///TODO: Tempoary loopIterations, #5,3ba15eb
				BenchmarkLifecycleClass = benchmarkLifecycleClass??typeof(NopBenchmarkLifecycle);
		Group = group;
		Description = description;
		Order = order;
		PlotOrder = plotOrder;
		Skip = skip;
		Name = name;
		LoopIterations = loopIterations;
	}
}