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
}