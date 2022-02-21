namespace Benchmarks.HelperObjects.Objects;

public struct StructHelper {
	public static ulong StaticField = 4;
	public ulong Field = 4;
	public StructHelper() { }

	public ulong Calculate() {
		Field++;
		return Field + 2;
	}

	public static ulong CalculateStatic() {
		StaticField++;
		return StaticField + 2;
	}
}