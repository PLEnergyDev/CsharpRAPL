using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CommandLine;

namespace CsharpRAPL.CommandLine;

// ReSharper disable once ClassNeverInstantiated.Global
public class Options {
	private const ulong DefaultIterations = 50;
	private const ulong DefaultLoopIterations = 1;

	private ulong _iterations;
	private ulong _loopIterations;
	private long _memoryForTurningOffGarbageCollection;

	[Option('g', nameof(SkipPlotGroups), Required = false,
		HelpText = "If plotting each benchmark group should be skipped.")]
	public bool SkipPlotGroups { get; set; }

	[Option('i', nameof(Iterations), Required = false,
		HelpText = "Sets the target iterations. (Disables Dynamic Iteration Calculation)", Default = 0UL)]
	public ulong Iterations {
		get => UseIterationCalculation ? DefaultIterations : _iterations;
		set => _iterations = value;
	}

	[Option('l', nameof(LoopIterations), Required = false,
		HelpText = "Sets the target loop iterations. (Disables Dynamic Loop Iteration Scaling)", Default = 0UL)]
	public ulong LoopIterations {
		get => UseLoopIterationScaling ? DefaultLoopIterations : _loopIterations;
		set => _loopIterations = value;
	}

	[Option('k', nameof(KeepOldResults), Required = false,
		HelpText = "If not set removes all files from the output folder and the plot folder.")]
	public bool KeepOldResults { get; set; } = false;

	[Option('o', nameof(OutputPath), Required = false, HelpText = "Set the output path for results.",
		Default = "results/")]
	public string OutputPath { get; set; } = "results/";


	[Option('p', nameof(PlotResults), Required = false, HelpText = "Should the results be plotted?")]
	public bool PlotResults { get; set; } = false;

	[Option('a', nameof(BenchmarksToAnalyse), HelpText = "The names of the benchmarks to analyse.")]
	public IEnumerable<string> BenchmarksToAnalyse { get; set; } = Array.Empty<string>();

	[Option('z', nameof(ZipResults), HelpText = "Zips the CSV results and plots into a single zip file.")]
	public bool ZipResults { get; set; }

	[Option('j', nameof(Json), HelpText = "Uses json for output instead of CVS, includes more information.")]
	public bool Json { get; set; } = false;

	[Option(nameof(TryTurnOffGC), Required = false, HelpText = "Tries to turn off GC during running of benchmarks.")]
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public bool TryTurnOffGC { get; set; }

	[Option(nameof(GCMemory), Required = false, HelpText = "Sets the amount of memory in bytes allowed to be used when turning off garbage collection.", Default = 250000000)]
	public long GCMemory {
		get => _memoryForTurningOffGarbageCollection;
		set => _memoryForTurningOffGarbageCollection = value;
	}
	
	[Option(nameof(PlotOutputPath), Required = false, HelpText = "Sets the output path for plots.",
		Default = "_plots/")]
	public string PlotOutputPath { get; set; } = "_plots/";

	[Option(nameof(OnlyPlot), Required = false, HelpText = "Plots the results in the output path.")]
	public bool OnlyPlot { get; set; }

	[Option(nameof(OnlyAnalysis), Required = false, HelpText = "Analysis the results in the output path.")]
	public bool OnlyAnalysis { get; set; }

	[Option(nameof(Verbose), HelpText = "Enables debug information.")]
	public bool Verbose { get; set; }

	[Option(nameof(OnlyTime), HelpText = "Only measures time.")]
	public bool OnlyTime { get; set; } = false;

	public bool UseIterationCalculation => _iterations == 0;

	public bool UseLoopIterationScaling => _loopIterations == 0;

	/// <summary>
	/// Determines if the program should exit when encountering an error.
	/// </summary>
	public static bool ShouldExit { get; set; } = true;
}