using System;
using System.Linq;
using CsharpRAPL;


int iterations = args.Length > 0 ? int.Parse(args[0]) : 1;
int loopIterations = args.Length > 1 ? int.Parse(args[1]) : 100_000_000;


var suite = new BenchmarkSuite();

suite.AddBenchmark("WhileLoop", iterations, () => {
	var count = 0;

	int i = 0;
	while (i < loopIterations) {
		i = i + 1;
		count = count + 1;
	}

	return count;
}, Console.WriteLine);

suite.AddBenchmark("WhileLoop", iterations, () => {
	var count = 0;

	int i = 0;
	while (i < loopIterations) {
		i = i + 1;
		count = count + 1;
	}

	return count;
}, Console.WriteLine);

suite.AddBenchmark("ForLoop", iterations, () => {
	var count = 0;

	for (var i = 0; i < loopIterations; i++) {
		count = count + 1;
	}


	return count;
}, Console.WriteLine);

suite.AddBenchmark("ForEachLoop", iterations, () => {
	var count = 0;

	foreach (int item in Enumerable.Range(0, loopIterations)) {
		count = count + 1;
	}

	return count;
}, Console.WriteLine);


suite.RunAll();