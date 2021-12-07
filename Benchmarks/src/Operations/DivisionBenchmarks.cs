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
			res = a / (b + i);
		}

		return res;
	}


	[Benchmark("Division", "Tests simple division where the parts are marked as constant")]
	public static int Const() {
		const int a = 10;
		const int b = 2;
		int res = 0;
		for (int i = 0; i < LoopIterations; i++) {
			res = a / (b + i);
		}

		return res;
	}


	[Benchmark("Division", "Tests division using compound assignment")]
	public static int CompAssign() {
		int a = 10;
		int res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res /= (a + i);
		}

		return res;
	}

	[Benchmark("Division", "Tests division without compound assignment")]
	public static int Assign() {
		int a = 10;
		int res = 1;
		for (int i = 0; i < LoopIterations; i++) {
			res = res / (a + i);
		}

		return res;
	}
}