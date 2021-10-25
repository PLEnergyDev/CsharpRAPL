using CsvHelper.Configuration.Attributes;

namespace CsharpRAPL.Data;

public record BenchmarkResult {
	[Index(0)] public double ElapsedTime { get; init; }
	[Index(1)] public double PackagePower { get; init; }
	[Index(2)] public double DramPower { get; init; }
	[Index(3)] public double Temperature { get; init; }
	[Index(4)] public string Result { get; init; } = string.Empty;
}