using System;

namespace CsharpRAPL.Benchmarking {
	[AttributeUsage(AttributeTargets.Method)]
	public class BenchmarkAttribute : Attribute {
		public string? Group { get; }

		public string Description { get; }

		public BenchmarkAttribute(string? group, string description) {
			Group = group;
			Description = description;
		}
	}
}