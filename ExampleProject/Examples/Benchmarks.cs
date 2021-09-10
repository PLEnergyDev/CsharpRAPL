using System;
using System.Linq;

namespace ExampleProject.Examples {
	public class Benchmarks {
		public static int Iterations;
		public static int LoopIterations;

		public static int WhileLoop() {
			var count = 0;

			var i = 0;
			while (i < LoopIterations) {
				i = i + 1;
				count = count + 1;
			}

			return count;
		}

		public static int ForLoop() {
			var count = 0;

			for (var i = 0; i < LoopIterations; i++) {
				count = count + 1;
			}


			return count;
		}

		public static int ForEachLoop() {
			var count = 0;

			foreach (int unused in Enumerable.Range(0, LoopIterations)) {
				count = count + 1;
			}

			return count;
		}

		public static int Add() {
			var a = 10;
			var b = 2;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = a + b;
			}

			return res;
		}

		public static int Minus() {
			var a = 10;
			var b = 2;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = a - b;
			}

			return res;
		}

		public static int Divide() {
			var a = 10;
			var b = 2;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = a / b;
			}

			return res;
		}

		public static int Modulo() {
			var a = 10;
			var b = 2;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = a % b;
			}

			return res;
		}

		public static int AddAssign() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = res + a;
			}

			return res;
		}

		public static int MinusAssign() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = res - a;
			}

			return res;
		}

		public static int DivideAssign() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = res / a;
			}

			return res;
		}

		public static int ModuloAssign() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res = res % a;
			}

			return res;
		}

		public static int AddComp() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res += a;
			}

			return res;
		}

		public static int MinusComp() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res -= a;
			}

			return res;
		}

		public static int DivideComp() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res /= a;
			}

			return res;
		}

		public static int ModuloComp() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res %= a;
			}

			return res;
		}

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

		public static int CompOp() {
			var a = 10;
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res += a == i ? 1 : 2;
			}

			return res;
		}

		public static int Increment() {
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res++;
			}

			return res;
		}

		public static int Decrement() {
			var res = 0;
			for (var i = 0; i < LoopIterations; i++) {
				res--;
			}

			return res;
		}
	}
}