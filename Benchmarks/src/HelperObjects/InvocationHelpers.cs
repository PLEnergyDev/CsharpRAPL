namespace Benchmarks.HelperObjects;

public class InvocationHelper {
	public static int StaticField = 4;
	private int Field = 4;
	
	public int Calculate() {
		Field++;
		return Field + 2;
	}
	
	public int CalculateUsingStaticField() {
		StaticField++;
		return StaticField + 2;
	}
	
	public static int CalculateStatic() {
		StaticField++;
		return StaticField + 2;
	}
}