using System;

namespace CsharpRAPL.Benchmarking.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class TypeVariationsAttribute : Attribute {
	public Type[] Types { get; }

	public TypeVariationsAttribute(params Type[] values) {
		Types = values;
	}
}