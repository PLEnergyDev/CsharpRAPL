using System;
using System.Collections.Generic;
using CommandLine;

namespace CsharpRAPL.CommandLine;

// ReSharper disable once ClassNeverInstantiated.Global
public class Options {
	[Option('g', nameof(SkipPlotGroups), Required = false,
		HelpText = "If plotting each benchmark group should be skipped.")]
	public bool SkipPlotGroups { get; set; }

	[Option('l', nameof(LoopIterations), Required = false, HelpText = "Sets the target loop iterations.")]
	public int LoopIterations { get; set; }

	[Option('i', nameof(Iterations), Required = false, HelpText = "Sets the target iterations.")]
	public int Iterations { get; set; }

	[Option('r', nameof(RemoveOldResults), Required = false,
		HelpText = "If set removes all files from the output folder and the plot folder.")]
	public bool RemoveOldResults { get; set; }

	[Option('o', nameof(OutputPath), Required = false, HelpText = "Set the output path for results.",
		Default = "results/")]
	public string OutputPath { get; set; } = "results/";

	[Option('p', nameof(PlotOutputPath), Required = false, HelpText = "Sets the output path for plots.",
		Default = "_plots/")]
	public string PlotOutputPath { get; set; } = "_plots/";

	[Option(nameof(OnlyPlot), Required = false, HelpText = "Plots the results in the output path.")]
	public bool OnlyPlot { get; set; }

	[Option(nameof(OnlyAnalysis), Required = false, HelpText = "Analysis the results in the output path.")]
	public bool OnlyAnalysis { get; set; }

	[Option('a', nameof(BenchmarksToAnalyse), HelpText = "The names of the benchmarks to analyse.")]
	public IEnumerable<string> BenchmarksToAnalyse { get; set; } = Array.Empty<string>();

	public bool UseIterationCalculation => Iterations == 0;

//TODO:
	public bool UseLoopIterationLimit => LoopIterations == 0;

	public bool ShouldExit;
}