using System;

namespace CsharpRAPL.Benchmarking.Attributes; 

public class IpcBenchmarkAttribute : BenchmarkAttribute{
	
	public string ExePath { get; }

	public IpcBenchmarkAttribute(string? group, string description, string executablePath,
		Type? benchmarkLifecycleClass = null, int order = 0,
		bool skip = false, string name = "", int plotOrder = 0, ulong loopIterations = 0) : base(group, description,
		benchmarkLifecycleClass, order, skip, name, plotOrder, loopIterations) {
		ExePath = executablePath;
	}
}