namespace Benchmarks.HelperObjects;

public class VariableObject {
	public static int StaticPropertyA { get; set; }
	public static int StaticPropertyB { get; set; } = 1;
	public int InstancePropertyA { get; set; }
	public int InstancePropertyB { get; set; } = 1;

	public static int StaticVariableA;
	public static int StaticVariableB = 1;
	public int InstanceVariableA;
	public int InstanceVariableB = 1;

	public int ParameterValue;
	public static int StaticParameterValue;

	public int TestParameter(int value, int loopIterations) {
		for (int i = 0; i < loopIterations; i++) {
			value += i + ParameterValue;
		}

		return value;
	}

	public static int StaticTestParameter(int value, int loopIterations) {
		for (int i = 0; i < loopIterations; i++) {
			value += i + StaticParameterValue;
		}

		return value;
	}
}