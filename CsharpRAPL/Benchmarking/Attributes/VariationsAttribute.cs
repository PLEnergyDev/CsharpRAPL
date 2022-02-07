using System;

namespace CsharpRAPL.Benchmarking.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class VariationsAttribute : Attribute {
	public object[] Values { get; }

	public VariationsAttribute(params object[] values) => Values = values;
}