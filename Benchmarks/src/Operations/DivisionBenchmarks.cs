﻿using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Operations;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class DivisionBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

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
	public static int Const() {
		const int a = 10;
		const int b = 2;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a / b;
		}

		return res;
	}


	[Benchmark("Division", "Tests division using compound assignment")]
	public static int CompAssign() {
		int a = 10;
		int res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res /= a;
		}

		return res;
	}

	[Benchmark("Division", "Tests division without compound assignment")]
	public static int Assign() {
		int a = 10;
		int res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res = res / a;
		}

		return res;
	}

	[Benchmark("Division", "Tests Simple Division with forced double")]
	public static double ForcedDouble() {
		double a = 10;
		double b = 3;
		double res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a / b;
		}

		return res;
	}

	[Benchmark("Division", "Tests Simple Division with consts and forced double")]
	public static double ForcedDoubleConst() {
		const double a = 10;
		const double b = 3;
		double res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a / b;
		}

		return res;
	}

	[Benchmark("Division", "Tests division using compound assignment and with forced double")]
	public static double ForcedDoubleComp() {
		double a = 10;
		double res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res /= a;
		}

		return res;
	}

	[Benchmark("Division", "Tests division without compound assignment and with forced double ForcedDoubleAssign")]
	public static double FDA() {
		double a = 10;
		double res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res = res / a;
		}

		return res;
	}

	[Benchmark("Division",
		"Tests Simple Division with forced double and non-constant denominator ForcedDoubleNonConstDenom")]
	public static double FDNCD() {
		double a = 10;
		double res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a / (i + 1);
		}

		return res;
	}

	[Benchmark("Division",
		"Tests Simple Division with consts and forced double and non-constant denominator ForcedDoubleConstNonConstDenom")]
	public static double FDCNCD() {
		const double a = 10;
		double res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a / (i + 1);
		}

		return res;
	}

	[Benchmark("Division",
		"Tests division using compound assignment and with forced double and non-constant denominator ForcedDoubleCompNonConstDenom")]
	public static double FDCmpNCD() {
		double res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res /= (i + 1);
		}

		return res;
	}

	[Benchmark("Division",
		"Tests division without compound assignment and with forced double and non-constant denominator ForcedDoubleAssignNonConstDenom")]
	public static double FDANCD() {
		double res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res = res / (i + 1);
		}

		return res;
	}
}