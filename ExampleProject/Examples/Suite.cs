using System;
using System.Collections.Generic;
using CsharpRAPL;
using ExampleProject.Examples;

Benchmarks.Iterations = args.Length > 0 ? int.Parse(args[0]) : 1;
Benchmarks.LoopIterations = args.Length > 1 ? int.Parse(args[1]) : 100_000_000;

var suite = new BenchmarkSuite();

suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.WhileLoop);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.ForLoop);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.ForEachLoop);


suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.Add);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.AddAssign);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.AddComp);

suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.Minus);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.MinusAssign);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.MinusComp);

suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.Divide);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.DivideAssign);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.DivideComp);

suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.Modulo);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.ModuloAssign);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.ModuloComp);


suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.If);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.IfElse);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.IfElseIf);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.Switch);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.CompOp);

suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.Increment);
suite.AddBenchmark(Benchmarks.Iterations, Benchmarks.Decrement);

suite.RunAll();

// EXAMPLE USAGE OF ANALYSIS
// Save the p-values to a dictionary
Dictionary<string, double> pValues = suite.AnalyseResults("ForLoop", "WhileLoop");


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