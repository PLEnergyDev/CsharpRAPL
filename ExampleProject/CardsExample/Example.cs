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

	foreach (int unused in Enumerable.Range(0, loopIterations)) {
		count = count + 1;
	}

	return count;
}, Console.WriteLine);

suite.AddBenchmark("Add", iterations, () => {
	var a = 10;
	var b = 2;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res = a + b;
	}

	return res;
}, Console.WriteLine);
suite.AddBenchmark("Minus", iterations, () => {
	var a = 10;
	var b = 2;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res = a - b;
	}

	return res;
}, Console.WriteLine);
suite.AddBenchmark("Divide", iterations, () => {
	var a = 10;
	var b = 2;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res = a / b;
	}

	return res;
}, Console.WriteLine);
suite.AddBenchmark("Modulo", iterations, () => {
	var a = 10;
	var b = 2;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res = a % b;
	}

	return res;
}, Console.WriteLine);

suite.AddBenchmark("AddAssign", iterations, () => {
	var a = 10;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res = res + a;
	}

	return res;
}, Console.WriteLine);
suite.AddBenchmark("MinusAssign", iterations, () => {
	var a = 10;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res = res - a;
	}

	return res;
}, Console.WriteLine);
suite.AddBenchmark("DivideAssign", iterations, () => {
	var a = 10;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res = res / a;
	}

	return res;
}, Console.WriteLine);
suite.AddBenchmark("ModuloAssign", iterations, () => {
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

suite.AddBenchmark("If", iterations, () => {
	var a = 10;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		if (a == i) {
			res += 1;
		}
	}

	return res;
}, Console.WriteLine);

suite.AddBenchmark("IfElse", iterations, () => {
	var a = 10;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		if (a == i) {
			res += 1;
		}
		else {
			res += 2;
		}
	}

	return res;
}, Console.WriteLine);

suite.AddBenchmark("IfElseIf", iterations, () => {
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		if (i == 0) {
			res += 1;
		}
		else if (i == 1) {
			res += 2;
		}
	}

	return res;
}, Console.WriteLine);

suite.AddBenchmark("Switch", iterations, () => {
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		switch (i) {
			case 0:
				res += 1;
				break;
			case 1:
				res += 2;
				break;
		}
	}

	return res;
}, Console.WriteLine);

suite.AddBenchmark("CompOp", iterations, () => {
	var a = 10;
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res += a == i ? 1 : 2;
	}

	return res;
}, Console.WriteLine);


suite.AddBenchmark("Increment", iterations, () => {
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res++;
	}

	return res;
}, Console.WriteLine);

suite.AddBenchmark("Decrement", iterations, () => {
	var res = 0;
	for (var i = 0; i < loopIterations; i++) {
		res--;
	}

	return res;
}, Console.WriteLine);

suite.RunAll();