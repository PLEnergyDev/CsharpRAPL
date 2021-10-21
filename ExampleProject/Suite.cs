using System;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.CommandLine;

CsharpRAPLCLI.SetAnalysis(analysis => {
	Console.WriteLine($"Min:\n{analysis.GetMin()}");
	Console.WriteLine($"Max:\n{analysis.GetMax()}");
	Console.WriteLine($"Average:\n{analysis.GetAverage()}");

	//Plots the two things in the analysis.
	analysis.PlotAnalysis();
});

Options options = CsharpRAPLCLI.Parse(args);

if (options.ShouldExit) {
	return;
}

var suite = new BenchmarkCollector(options.Iterations, options.LoopIterations);

suite.RunAll();
suite.PlotGroups();