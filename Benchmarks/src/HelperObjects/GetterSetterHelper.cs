namespace Benchmarks.HelperObjects;

public class GetterSetterHelper {
	public ulong Property { get; set; }

	public ulong PropertyWithBackingField {
		get { return _backingField; }
		set { _backingField = value; }
	}

	public ulong Field;

	private ulong _backingField;

	public ulong GetValue() {
		return _backingField;
	}

	public void SetValue(ulong value) {
		_backingField = value;
	}
}