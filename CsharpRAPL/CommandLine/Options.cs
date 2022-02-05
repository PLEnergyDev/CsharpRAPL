using System;
using System.Collections.Generic;
using CommandLine;

namespace CsharpRAPL.CommandLine;

// ReSharper disable once ClassNeverInstantiated.Global
public class Options {
	private const ulong DefaultIterations = 50;
	private const ulong DefaultLoopIterations = 10000000;

	private ulong _iterations;
	private ulong _loopIterations;

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

	[Option('r', nameof(RemoveOldResults), Required = false,
		HelpText = "If set removes all files from the output folder and the plot folder.")]
	public bool RemoveOldResults { get; set; }

	[Option('o', nameof(OutputPath), Required = false, HelpText = "Set the output path for results.",
		Default = "results/")]
	public string OutputPath { get; set; } = "results/";

	[Option('p', nameof(PlotOutputPath), Required = false, HelpText = "Sets the output path for plots.",
		Default = "_plots/")]
	public string PlotOutputPath { get; set; } = "_plots/";


	[Option('a', nameof(BenchmarksToAnalyse), HelpText = "The names of the benchmarks to analyse.")]
	public IEnumerable<string> BenchmarksToAnalyse { get; set; } = Array.Empty<string>();

	[Option('z', nameof(ZipResults), HelpText = "Zips the CSV results and plots into a single zip file.")]
	public bool ZipResults { get; set; }

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