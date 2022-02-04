using CsvHelper.Configuration.Attributes;

namespace CsharpRAPL.Data;

public record BenchmarkResult {
	/// <summary>
	/// Time Elapsed in milliseconds
	/// </summary>
	[Index(0), Name("Elapsed Time (ms)")]
	public double ElapsedTime { get; init; }

	/// <summary>
	/// Package energy used in µJ
	/// </summary>
	[Index(1), Name("Package Energy (µJ)")]
	public double PackageEnergy { get; init; }

	/// <summary>
	/// DRam energy used in µJ
	/// </summary>
	[Index(2), Name("DRAM Energy (µJ)")]
	public double DRAMEnergy { get; init; }

	/// <summary>
	/// Temperature in Celsius (C°)
	/// </summary>
	[Index(3),Name("Temperature (C°)")]
	public double Temperature { get; init; }

	/// <summary>
	/// Return value of the benchmark.
	/// </summary>
	[Index(4)]
	public string BenchmarkReturnValue { get; init; } = string.Empty;

	/// <summary>
	/// The amount of loop iterations used 
	/// </summary>
	[Index(5)]
	public ulong LoopIterations { get; init; }
}