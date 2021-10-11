using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CsharpRAPL.Benchmarking;

namespace ExampleProject {
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	public static class Benchmarks {
		public static int Iterations;
		public static int LoopIterations;


		[Benchmark("Loops", "Tests a while loop")]
		public static int WhileLoop() {
			var count = 0;

			var i = 0;
			while (i < LoopIterations) {
				i = i + 1;
				count = count + 1;
			}

			return count;
		}

		[Benchmark("Loops", "Tests a for loop")]
		public static int ForLoop() {
			var count = 0;

			for (var i = 0; i < LoopIterations; i++) {
				count = count + 1;
			}


			return count;
		}

		[Benchmark("Loops", "Tests a foreach loop")]
		public static int ForEachLoop() {
			var count = 0;

			foreach (int unused in Enumerable.Range(0, LoopIterations)) {
				count = count + 1;
			}

			return count;
		}

		[Benchmark("Operations", "Tests simple addition")]
		public static int Add() {
			var a = 10;
			var b = 2;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = a + b;
			}

			return res;
		}

		[Benchmark("Operations", "Tests simple subtraction")]
		public static int Minus() {
			var a = 10;
			var b = 2;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = a - b;
			}

			return res;
		}

		[Benchmark("Operations", "Tests simple division")]
		public static int Divide() {
			var a = 10;
			var b = 2;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = a / b;
			}

			return res;
		}

		[Benchmark("Operations", "Tests simple modulo")]
		public static int Modulo() {
			var a = 10;
			var b = 2;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = a % b;
			}

			return res;
		}

		[Benchmark("Operations", "Tests simple addition where the parts are marked as constant")]
		public static int AddConst() {
			const int a = 10;
			const int b = 2;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = a + b;
			}

			return res;
		}

		[Benchmark("Operations", "Tests simple subtraction where the parts are marked as constant")]
		public static int MinusConst() {
			const int a = 10;
			const int b = 2;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = a - b;
			}

			return res;
		}

		[Benchmark("Operations", "Tests simple division where the parts are marked as constant")]
		public static int DivideConst() {
			const int a = 10;
			const int b = 2;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = a / b;
			}

			return res;
		}

		[Benchmark("Operations", "Tests simple modulo where the parts are marked as constant")]
		public static int ModuloConst() {
			const int a = 10;
			const int b = 2;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = a % b;
			}

			return res;
		}

		[Benchmark("Operations", "Tests addition without compound assignment")]
		public static int AddAssign() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = res + a;
			}

			return res;
		}

		[Benchmark("Operations", "Tests subtraction without compound assignment")]
		public static int MinusAssign() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = res - a;
			}

			return res;
		}

		[Benchmark("Operations", "Tests division without compound assignment")]
		public static int DivideAssign() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = res / a;
			}

			return res;
		}

		[Benchmark("Operations", "Tests modulo without compound assignment")]
		public static int ModuloAssign() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = res % a;
			}

			return res;
		}

		[Benchmark("Operations", "Tests addition using compound assignment")]
		public static int AddComp() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res += a;
			}

			return res;
		}

		[Benchmark("Operations", "Tests subtraction using compound assignment")]
		public static int MinusComp() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res -= a;
			}

			return res;
		}

		[Benchmark("Operations", "Tests division using compound assignment")]
		public static int DivideComp() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res /= a;
			}

			return res;
		}

		[Benchmark("Operations", "Tests modulo using compound assignment")]
		public static int ModuloComp() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res %= a;
			}

			return res;
		}

		[Benchmark("Control", "Tests if statement")]
		public static int If() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				if (a == i) {
					res += 1;
				}
			}

			return res;
		}

		[Benchmark("Control", "Tests if and else statement")]
		public static int IfElse() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				if (a == i) {
					res += 1;
				}
				else {
					res += 2;
				}
			}

			return res;
		}

		[Benchmark("Control", "Tests if and if else statement")]
		public static int IfElseIf() {
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				if (i == 0) {
					res += 1;
				}
				else if (i == 1) {
					res += 2;
				}
			}

			return res;
		}

		[Benchmark("Control", "Tests switch statement")]
		public static int Switch() {
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
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
		}

		[Benchmark("Control", "Tests if conditional operator")]
		public static int ConditionalOperator() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res += a == i ? 1 : 2;
			}

			return res;
		}

		[Benchmark("Operation", "Tests post increment using ++")]
		public static int PostIncrement() {
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res++;
			}

			return res;
		}

		[Benchmark("Operation", "Tests post decrement using --")]
		public static int PostDecrement() {
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res--;
			}

			return res;
		}

		[Benchmark("Operation", "Tests pre increment using ++")]
		public static int PreIncrement() {
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				++res;
			}

			return res;
		}

		[Benchmark("Operation", "Tests pre decrement using --")]
		public static int PreDecrement() {
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				--res;
			}

			return res;
		}
	}
}