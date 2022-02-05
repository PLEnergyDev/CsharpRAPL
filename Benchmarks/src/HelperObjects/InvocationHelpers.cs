namespace Benchmarks.HelperObjects;

public class InvocationHelper {
	public static ulong StaticField = 4;
	private ulong Field = 4;

	public ulong Calculate() {
		Field++;
		return Field + 2;
	}

	public ulong CalculateUsingStaticField() {
		StaticField++;
		return StaticField + 2;
	}

	public static ulong CalculateStatic() {
		StaticField++;
		return StaticField + 2;
	}
}