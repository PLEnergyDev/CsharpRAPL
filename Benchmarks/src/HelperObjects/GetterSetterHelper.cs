namespace Benchmarks.HelperObjects;

public class GetterSetterHelper {
	public int Property { get; set; }

	public int PropertyWithBackingField {
		get { return _backingField; }
		set { _backingField = value; }
	}

	public int Field;

	private int _backingField;

	public int GetValue() {
		return _backingField;
	}

	public void SetValue(int value) {
		_backingField = value;
	}
}