using System.Collections.Generic;

namespace CsharpRAPL.Benchmarking.Variation;

public class VariationInstance {
	public List<MemberInfo> Values { get; } = new();

	public class MemberInfo {
		public string Name { get; }
		public object Value { get; }
		public readonly bool IsField;

		public MemberInfo(string name, object value, bool isField) {
			Name = name;
			Value = value;
			IsField = isField;
		}

		public override string ToString() {
			return $"{nameof(Name)}: {Name}, {nameof(Value)}: {Value}";
		}

		public void Deconstruct(out string name, out object value, out bool isField) {
			name = Name;
			value = Value;
			isField = IsField;
		}
	}
}