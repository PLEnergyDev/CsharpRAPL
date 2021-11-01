using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace ExampleProject.Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class OperationsBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	[Benchmark("Addition", "Tests simple addition")]
	public static int Add() {
		int a = 10;
		int b = 2;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a + b;
		}

		return res;
	}

	[Benchmark("Addition", "Tests simple addition where the parts are marked as constant")]
	public static int AddConst() {
		const int a = 10;
		const int b = 2;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a + b;
		}

		return res;
	}

	[Benchmark("Addition", "Tests addition using compound assignment")]
	public static int AddComp() {
		int a = 10;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res += a;
		}

		return res;
	}

	[Benchmark("Addition", "Tests addition without compound assignment")]
	public static int AddAssign() {
		int a = 10;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = res + a;
		}

		return res;
	}


	[Benchmark("Subtraction", "Tests simple subtraction")]
	public static int Minus() {
		int a = 10;
		int b = 2;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a - b;
		}

		return res;
	}


	[Benchmark("Subtraction", "Tests simple subtraction where the parts are marked as constant")]
	public static int MinusConst() {
		const int a = 10;
		const int b = 2;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a - b;
		}

		return res;
	}

	[Benchmark("Subtraction", "Tests subtraction using compound assignment")]
	public static int MinusComp() {
		int a = 10;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res -= a;
		}

		return res;
	}


	[Benchmark("Subtraction", "Tests subtraction without compound assignment")]
	public static int MinusAssign() {
		int a = 10;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = res - a;
		}

		return res;
	}

	[Benchmark("Multiplication", "Tests simple multiplication")]
	public static int Multiply() {
		int a = 5;
		int b = 1;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a * b;
		}

		return res;
	}

	[Benchmark("Multiplication", "Tests simple multiplication where the parts are marked as constant")]
	public static int MultiplyConst() {
		const int a = 5;
		const int b = 1;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a * b;
		}

		return res;
	}

	[Benchmark("Multiplication", "Tests multiplication using compound assignment")]
	public static int MultiplyComp() {
		int a = 5;
		int res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res *= a;
		}

		return res;
	}

	[Benchmark("Multiplication", "Tests multiplication without compound assignment")]
	public static int MultiplyAssign() {
		int a = 5;
		int res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res = res * a;
		}

		return res;
	}


	[Benchmark("Division", "Tests simple division")]
	public static int Divide() {
		int a = 10;
		int b = 2;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a / b;
		}

		return res;
	}


	[Benchmark("Division", "Tests simple division where the parts are marked as constant")]
	public static int DivideConst() {
		const int a = 10;
		const int b = 2;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a / b;
		}

		return res;
	}


	[Benchmark("Division", "Tests division using compound assignment")]
	public static int DivideComp() {
		int a = 10;
		int res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res /= a;
		}

		return res;
	}

	[Benchmark("Division", "Tests division without compound assignment")]
	public static int DivideAssign() {
		int a = 10;
		int res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res = res / a;
		}

		return res;
	}
	
	[Benchmark("Division", "Tests Simple Division with forced double")]
	public static double DivideForcedDouble() {
		double a = 10;
		double b = 3;
		double res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a / b;
		}

		return res;
	}
	
	[Benchmark("Division", "Tests Simple Division with consts and forced double")]
	public static double DivideForcedDoubleConst() {
		const double a = 10;
		const double b = 3;
		double res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a / b;
		}

		return res;
	}
	
	[Benchmark("Division", "Tests division using compound assignment and with forced double")]
	public static double DivideForcedDoubleComp() {
		double a = 10;
		double res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res /= a;
		}

		return res;
	}

	[Benchmark("Division", "Tests division without compound assignment and with forced double")]
	public static double DivideForcedDoubleAssign() {
		double a = 10;
		double res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res = res / a;
		}

		return res;
	}
	
	[Benchmark("Division", "Tests Simple Division with forced double and non-constant denominator")]
	public static double DivideForcedDoubleNonConstDenom() {
		double a = 10;
		double res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a / (i+1);
		}

		return res;
	}
	
	[Benchmark("Division", "Tests Simple Division with consts and forced double and non-constant denominator")]
	public static double DivideForcedDoubleConstNonConstDenom() {
		const double a = 10;
		double res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a / (i+1);
		}

		return res;
	}
	
	[Benchmark("Division", "Tests division using compound assignment and with forced double and non-constant denominator")]
	public static double DivideForcedDoubleCompNonConstDenom() {
		double res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res /= (i+1);
		}

		return res;
	}

	[Benchmark("Division", "Tests division without compound assignment and with forced double and non-constant denominator")]
	public static double DivideForcedDoubleAssignNonConstDenom() {
		double res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res = res / (i+1);
		}

		return res;
	}

	[Benchmark("Modulo", "Tests simple modulo")]
	public static int Modulo() {
		int a = 10;
		int b = 2;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a % b;
		}

		return res;
	}


	[Benchmark("Modulo", "Tests simple modulo where the parts are marked as constant")]
	public static int ModuloConst() {
		const int a = 10;
		const int b = 2;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a % b;
		}

		return res;
	}

	[Benchmark("Modulo", "Tests modulo using compound assignment")]
	public static int ModuloComp() {
		int a = 10;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res %= a;
		}

		return res;
	}

	[Benchmark("Modulo", "Tests modulo without compound assignment")]
	public static int ModuloAssign() {
		int a = 10;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = res % a;
		}

		return res;
	}

	[Benchmark("Operations", "Tests post increment using ++")]
	public static int PostIncrement() {
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res++;
		}

		return res;
	}

	[Benchmark("Operations", "Tests post decrement using --")]
	public static int PostDecrement() {
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res--;
		}

		return res;
	}

	[Benchmark("Operations", "Tests pre increment using ++")]
	public static int PreIncrement() {
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			++res;
		}

		return res;
	}

	[Benchmark("Operations", "Tests pre decrement using --")]
	public static int PreDecrement() {
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			--res;
		}

		return res;
	}
}