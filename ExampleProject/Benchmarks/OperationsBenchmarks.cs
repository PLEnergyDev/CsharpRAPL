using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace ExampleProject.Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class OperationsBenchmark {
	public static int Iterations;
	public static int LoopIterations;

	[Benchmark("Addition", "Tests simple addition")]
	public static int Add() {
		var a = 10;
		var b = 2;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res = a + b;
		}

		return res;
	}

	[Benchmark("Addition", "Tests simple addition where the parts are marked as constant")]
	public static int AddConst() {
		const int a = 10;
		const int b = 2;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res = a + b;
		}

		return res;
	}

	[Benchmark("Addition", "Tests addition using compound assignment")]
	public static int AddComp() {
		var a = 10;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res += a;
		}

		return res;
	}

	[Benchmark("Addition", "Tests addition without compound assignment")]
	public static int AddAssign() {
		var a = 10;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res = res + a;
		}

		return res;
	}


	[Benchmark("Subtraction", "Tests simple subtraction")]
	public static int Minus() {
		var a = 10;
		var b = 2;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res = a - b;
		}

		return res;
	}


	[Benchmark("Subtraction", "Tests simple subtraction where the parts are marked as constant")]
	public static int MinusConst() {
		const int a = 10;
		const int b = 2;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res = a - b;
		}

		return res;
	}

	[Benchmark("Subtraction", "Tests subtraction using compound assignment")]
	public static int MinusComp() {
		var a = 10;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res -= a;
		}

		return res;
	}


	[Benchmark("Subtraction", "Tests subtraction without compound assignment")]
	public static int MinusAssign() {
		var a = 10;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res = res - a;
		}

		return res;
	}

	[Benchmark("Multiplication", "Tests simple multiplication")]
	public static int Multiply() {
		var a = 5;
		var b = 1;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res = a * b;
		}

		return res;
	}

	[Benchmark("Multiplication", "Tests simple multiplication where the parts are marked as constant")]
	public static int MultiplyConst() {
		const int a = 5;
		const int b = 1;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res = a * b;
		}

		return res;
	}

	[Benchmark("Multiplication", "Tests multiplication using compound assignment")]
	public static int MultiplyComp() {
		var a = 5;
		var res = 1;
		for (var i = 0; i < LoopIterations; i++) {
			res *= a;
		}

		return res;
	}

	[Benchmark("Multiplication", "Tests multiplication without compound assignment")]
	public static int MultiplyAssign() {
		var a = 5;
		var res = 1;
		for (var i = 0; i < LoopIterations; i++) {
			res = res * a;
		}

		return res;
	}


	[Benchmark("Division", "Tests simple division")]
	public static int Divide() {
		var a = 10;
		var b = 2;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res = a / b;
		}

		return res;
	}


	[Benchmark("Division", "Tests simple division where the parts are marked as constant")]
	public static int DivideConst() {
		const int a = 10;
		const int b = 2;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res = a / b;
		}

		return res;
	}


	[Benchmark("Division", "Tests division using compound assignment")]
	public static int DivideComp() {
		var a = 10;
		var res = 1;
		for (var i = 0; i < LoopIterations; i++) {
			res /= a;
		}

		return res;
	}

	[Benchmark("Division", "Tests division without compound assignment")]
	public static int DivideAssign() {
		var a = 10;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res = res / a;
		}

		return res;
	}

	[Benchmark("Modulo", "Tests simple modulo")]
	public static int Modulo() {
		var a = 10;
		var b = 2;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res = a % b;
		}

		return res;
	}


	[Benchmark("Modulo", "Tests simple modulo where the parts are marked as constant")]
	public static int ModuloConst() {
		const int a = 10;
		const int b = 2;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res = a % b;
		}

		return res;
	}

	[Benchmark("Modulo", "Tests modulo using compound assignment")]
	public static int ModuloComp() {
		var a = 10;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res %= a;
		}

		return res;
	}

	[Benchmark("Modulo", "Tests modulo without compound assignment")]
	public static int ModuloAssign() {
		var a = 10;
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res = res % a;
		}

		return res;
	}

	[Benchmark("Operations", "Tests post increment using ++")]
	public static int PostIncrement() {
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res++;
		}

		return res;
	}

	[Benchmark("Operations", "Tests post decrement using --")]
	public static int PostDecrement() {
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			res--;
		}

		return res;
	}

	[Benchmark("Operations", "Tests pre increment using ++")]
	public static int PreIncrement() {
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			++res;
		}

		return res;
	}

	[Benchmark("Operations", "Tests pre decrement using --")]
	public static int PreDecrement() {
		var res = 0;
		for (var i = 0; i < LoopIterations; i++) {
			--res;
		}

		return res;
	}
}