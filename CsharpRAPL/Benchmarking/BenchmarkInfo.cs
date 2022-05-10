using System.Collections.Generic;
using CsharpRAPL.Benchmarking.Variation;
using CsharpRAPL.Data;

namespace CsharpRAPL.Benchmarking;

public class BenchmarkInfo {
	public string Name { get; init; } = "";
	public string? Group { get; init; }
	public int Order { get; init; }
	public int PlotOrder { get; set; }
	public bool HasRun { get; set; }
	public double ElapsedTime { get; set; }
	public ulong Iterations { get; set; }
	public ulong LoopIterations { get; set; }
	//public VariationInstance Parameters { get; set; } = new();
	public List<BenchmarkResult> RawResults { get; init; } = new();
	public List<BenchmarkResult> NormalizedResults { get; init; } = new();
}