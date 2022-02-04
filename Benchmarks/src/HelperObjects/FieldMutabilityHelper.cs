namespace Benchmarks.HelperObjects;

public class FieldMutabilityHelper {
	public ulong Field = 10;
	public const ulong ConstField = 10;
	public readonly ulong ReadonlyField = 10;

	public ulong Property { get; set; } = 10;
	public ulong InitProperty { get; init; } = 10;
	public ulong GetProperty { get; } = 10;
	public ulong ComputedProperty => 10;
}