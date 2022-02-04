namespace Benchmarks.HelperObjects;

public class VariableObject {
	public static ulong StaticPropertyA { get; set; }
	public static ulong StaticPropertyB { get; set; } = 1;
	public ulong InstancePropertyA { get; set; }
	public ulong InstancePropertyB { get; set; } = 1;

	public static ulong StaticVariableA;
	public static ulong StaticVariableB = 1;
	public ulong InstanceVariableA;
	public ulong InstanceVariableB = 1;

	public ulong ParameterValue;
	public static ulong StaticParameterValue;

	public ulong TestParameter(ulong value, ulong loopIterations) {
		for (ulong i = 0; i < loopIterations; i++) {
			value += i + ParameterValue;
		}

		return value;
	}

	public static ulong StaticTestParameter(ulong value, ulong loopIterations) {
		for (ulong i = 0; i < loopIterations; i++) {
			value += i + StaticParameterValue;
		}

		return value;
	}
}