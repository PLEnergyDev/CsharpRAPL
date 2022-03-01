using System;

namespace CsharpRAPL.Benchmarking.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class BenchmarkAttribute : Attribute {
	public string Name { get; }
	
	public string? Group { get; }

	public string Description { get; }

	public int Order { get; }

	public bool Skip { get; }

	public BenchmarkAttribute(string? group, string description, int order = 0, bool skip = false, string name = "") {
		Group = group;
		Description = description;
		Order = order;
		Skip = skip;
		Name = name;
	}
}