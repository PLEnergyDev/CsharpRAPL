using System;

namespace CsharpRAPL.Benchmarking {
	[AttributeUsage(AttributeTargets.Method)]
	public class BenchmarkAttribute : Attribute {
		public string? Group { get; }

		public string Description { get; }
		
		public int Order { get; }

		public BenchmarkAttribute(string? group, string description, int order = 0) {
			Group = group;
			Description = description;
			Order = order;
		}
	}
}