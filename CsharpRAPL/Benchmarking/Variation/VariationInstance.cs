using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CsharpRAPL.Benchmarking.Variation;

public record VariationInstance {
	public List<MemberInfo> Values { get; } = new();

	public record MemberInfo {
		public string Name { get; }
		public object Value { get; }
		public readonly bool IsField;

		public MemberInfo(string name, object value, bool isField) {
			Name = name;
			Value = value;
			IsField = isField;
		}
		
		public void Deconstruct(out string name, out object value, out bool isField) {
			name = Name;
			value = Value;
			isField = IsField;
		}
	}
}