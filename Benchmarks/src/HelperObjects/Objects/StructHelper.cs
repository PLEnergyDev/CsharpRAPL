namespace Benchmarks.HelperObjects.Objects;

public struct StructHelper {
	public static int StaticField = 4;
	public int Field = 4;

	public int Calculate() {
		Field++;
		return Field + 2;
	}

	public static int CalculateStatic() {
		StaticField++;
		return StaticField + 2;
	}
}