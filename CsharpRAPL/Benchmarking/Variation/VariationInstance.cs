using System.Collections.Generic;

namespace CsharpRAPL.Benchmarking.Variation;

public class VariationInstance {
	public readonly List<MemberInfo> Values = new();

	public class MemberInfo {
		public readonly string Name;
		public readonly object Value;
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