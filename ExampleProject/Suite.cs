using System;
using System.Collections.Generic;
using CsharpRAPL.Benchmarking;
using ExampleProject;

Benchmarks.Iterations = args.Length > 0 ? int.Parse(args[0]) : 1;
Benchmarks.LoopIterations = args.Length > 1 ? int.Parse(args[1]) : 100_000_000;

var suite = new BenchmarkCollector(Benchmarks.Iterations);

suite.RunAll();

// EXAMPLE USAGE OF ANALYSIS
var analysis = suite.AnalyseResults("ForLoop", "WhileLoop");
// Save the p-values to a dictionary
Dictionary<string, double> pValues = analysis.CalculatePValue();


// Print the p-values to console
// The lower the p-value the higher the chance for that statement to be correct
// P-value means the chance of the null hypothesis to be true
Console.WriteLine("Is the null hypothesis true? I.e. is the opposite of what the key implies true?");
foreach ((string name, double value) in pValues) {
	Console.WriteLine($"{name}:{value}");
}

Console.WriteLine("Is the alternate hypothesis true? I.e. is what the key implies true?");
foreach ((string name, double value) in pValues) {
	Console.WriteLine($"{name}:{1 - value}");
}


Console.WriteLine("Min:\n" + analysis.GetMin());
Console.WriteLine("Max:\n" + analysis.GetMax());
Console.WriteLine("Average:\n" + analysis.GetAverage());