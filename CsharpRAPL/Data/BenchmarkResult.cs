using CsvHelper.Configuration.Attributes;

namespace CsharpRAPL.Data;

public record BenchmarkResult {
	/// <summary>
	/// Time Elapsed in milliseconds
	/// </summary>
	[Index(0)]
	public double ElapsedTime { get; init; }

	/// <summary>
	/// Package energy used in µJ
	/// </summary>
	[Index(1)]
	public double PackageEnergy { get; init; }

	/// <summary>
	/// DRam energy used in µJ
	/// </summary>
	[Index(2)]
	public double DramEnergy { get; init; }

	/// <summary>
	/// Temperature in Celsius (C°)
	/// </summary>
	[Index(3)]
	public double Temperature { get; init; }

	/// <summary>
	/// Return value of the benchmark.
	/// </summary>
	[Index(4)]
	public string BenchmarkReturnValue { get; init; } = string.Empty;
}