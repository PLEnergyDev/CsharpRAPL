namespace Benchmarks.HelperObjects;

public class NonGeneric {
	public int IntValue;
	public object Value;

	public NonGeneric(object value) {
		if (value is int intValue) {
			IntValue = intValue;
		}

		Value = value;
	}
}