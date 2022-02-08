using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

namespace CsharpRAPL.Data;

public record BenchmarkResult {
	/// <summary>
	/// Time Elapsed in milliseconds
	/// </summary>
	[Index(0), Name("Elapsed Time (ms)"), JsonPropertyName("Elapsed Time (ms)")]
	public double ElapsedTime { get; init; }

	/// <summary>
	/// Package energy used in µJ
	/// </summary>
	[Index(1), Name("Package Energy (µJ)"), JsonPropertyName("Package Energy (micro J)")]
	public double PackageEnergy { get; init; }

	/// <summary>
	/// DRam energy used in µJ
	/// </summary>
	[Index(2), Name("DRAM Energy (µJ)"), JsonPropertyName("DRAM Energy (micro J)")]
	public double DRAMEnergy { get; init; }

	/// <summary>
	/// Temperature in Celsius (C°)
	/// </summary>
	[Index(3), Name("Temperature (C°)"), JsonPropertyName("Temperature (C degrees)")]
	public double Temperature { get; init; }

	/// <summary>
	/// Return value of the benchmark.
	/// </summary>
	[Index(4)]
	public string BenchmarkReturnValue { get; init; } = string.Empty;
}