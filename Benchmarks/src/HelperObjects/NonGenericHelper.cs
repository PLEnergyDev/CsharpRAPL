namespace Benchmarks.HelperObjects;

public class NonGeneric {
	public ulong ULongValue;
	public object Value;

	public NonGeneric(object value) {
		if (value is ulong ulongValue) {
			ULongValue = ulongValue;
		}

		Value = value;
	}
}