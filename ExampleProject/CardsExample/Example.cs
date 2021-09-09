using System;
using System.Linq;
using CsharpRAPL;


int iterations = args.Length > 0 ? int.Parse(args[0]) : 1;
int loopIterations = args.Length > 1 ? int.Parse(args[1]) : 100_000_000;


var suite = new BenchmarkSuite();

suite.AddBenchmark("WhileLoop", iterations, () => {
	var count = 0;

	var i = 0;
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

suite.AddBenchmark("Add", iterations, () => {
	var a = 10;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res = res + a;
	}

	return res;
}, Console.WriteLine);
suite.AddBenchmark("Minus", iterations, () => {
	var a = 10;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res = res - a;
	}

	return res;
}, Console.WriteLine);
suite.AddBenchmark("Divide", iterations, () => {
	var a = 10;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res = res / a;
	}

	return res;
}, Console.WriteLine);
suite.AddBenchmark("Modulo", iterations, () => {
	var a = 10;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res = res % a;
	}

	return res;
}, Console.WriteLine);

suite.AddBenchmark("AddComp", iterations, () => {
	var a = 10;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res += a;
	}

	return res;
}, Console.WriteLine);
suite.AddBenchmark("MinusComp", iterations, () => {
	var a = 10;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res -= a;
	}

	return res;
}, Console.WriteLine);
suite.AddBenchmark("DivideComp", iterations, () => {
	var a = 10;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res /= a;
	}

	return res;
}, Console.WriteLine);
suite.AddBenchmark("ModuloComp", iterations, () => {
	var a = 10;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res %= a;
	}

	return res;
}, Console.WriteLine);

suite.RunAll();