namespace Benchmarks.HelperObjects;

public class FieldMutabilityHelper {
	public int Field = 10;
	public const int ConstField = 10;
	public readonly int ReadonlyField = 10;

	public int Property { get; set; } = 10;
	public int InitProperty { get; init; } = 10;
	public int GetProperty { get; } = 10;
	public int ComputedProperty => 10;
}