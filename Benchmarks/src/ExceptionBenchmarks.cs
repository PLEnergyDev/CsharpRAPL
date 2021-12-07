using System;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ExceptionBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	[Benchmark("Exception", "Tests try-catch no exception thrown")]
	public static int TryCatchNoE() {
		int a = 1;
		int b = 2;
		for (int i = 0; i < LoopIterations; i++) {
			try {
				a *= 2;
				a += 2;
				a %= 20;
				a /= b;
			}
			catch (Exception) {
				a %= 10;
				throw;
			}
		}

		return a;
	}

	[Benchmark("ExceptionFinally", "Tests try-finally no exception thrown")]
	public static int TryFinallyNoE() {
		int a = 1;
		int b = 2;
		for (int i = 0; i < LoopIterations; i++) {
			try {
				a *= 2;
				a += 2;
				a %= 20;
			}
			finally {
				a /= b;
			}
		}

		return a;
	}

	[Benchmark("ExceptionFinally", "Tests try-finally equivalent no exception thrown")]
	public static int TryFinallyEquivNoE() {
		int a = 1;
		int b = 2;
		for (int i = 0; i < LoopIterations; i++) {
			a *= 2;
			a += 2;
			a %= 20;
			a /= b;
		}

		return a;
	}


	[Benchmark("Exception",
		"Tests try-catch exception thrown with catch all statement, throws floor(LoopIterations / 2) exceptions")]
	public static int TryCatchAllE() {
		int a = 1;
		int b = 2;
		for (int i = 0; i < LoopIterations; i++) {
			try {
				a *= 2;
				a += 2;
				a %= 20;
				a /= b;
				b = 0;
			}
			catch (Exception) {
				a %= 10;
				b = 2;
			}
		}

		return a;
	}

	[Benchmark("Exception",
		"Tests try-catch exception thrown with specific catch statement, throws floor(LoopIterations / 2) exceptions")]
	public static int TryCatchSpecificE() {
		int a = 1;
		int b = 2;
		for (int i = 0; i < LoopIterations; i++) {
			try {
				a *= 2;
				a += 2;
				a %= 20;
				a /= b;
				b = 0;
			}
			catch (DivideByZeroException) {
				a %= 10;
				b = 2;
			}
		}

		return a;
	}

	[Benchmark("Exception", "Tests try-catch exception thrown, throws floor(LoopIterations / 2) exceptions")]
	public static int TryCatchEWMessage() {
		int a = 1;
		int b = 2;
		for (int i = 0; i < LoopIterations; i++) {
			try {
				if (b == 0) {
					throw new ArgumentException("I'm an exception message");
				}

				b = 0;
			}
			catch (ArgumentException) {
				b = 2;
			}
		}

		return a;
	}

	[Benchmark("Exception", "Tests try-catch exception thrown, throws floor(LoopIterations / 2) exception")]
	public static int TryCatchEWOMessage() {
		int a = 1;
		int b = 2;
		for (int i = 0; i < LoopIterations; i++) {
			try {
				if (b == 0) {
					throw new ArgumentException();
				}

				b = 0;
			}
			catch (ArgumentException) {
				b = 2;
			}
		}

		return a;
	}

	[Benchmark("Exception", "Tests try-catch 'equivalent' using an if statement to check for exception")]
	public static int TryCatchEWithIf() {
		int a = 1;
		int b = 2;
		for (int i = 0; i < LoopIterations; i++) {
			a *= 2;
			a += 2;
			a %= 20;

			if (b == 0) {
				a %= 10;
				b = 2;
			}
			else {
				a /= b;
				b = 0;
			}
		}

		return a;
	}

	[Benchmark("ExceptionCatchFinally",
		"Tests try-catch-finally 'equivalent' using an if statement to check for exception")]
	public static int TryCatchEFinallyWithIf() {
		int a = 1;
		int b = 2;
		for (int i = 0; i < LoopIterations; i++) {
			a *= 2;
			a += 2;
			a %= 20;

			if (b == 0) {
				a %= 10;
				b = 2;
			}
			else {
				a /= b;
				b = 0;
			}

			a++;
		}

		return a;
	}

	[Benchmark("ExceptionCatchFinally",
		"Tests try-catch-finally exception thrown with catch all statement, throws floor(LoopIterations / 2) exceptions")]
	public static int TryCatchFinallyAllE() {
		int a = 1;
		int b = 2;
		for (int i = 0; i < LoopIterations; i++) {
			try {
				a *= 2;
				a += 2;
				a %= 20;
				a /= b;
				b = 0;
			}
			catch (Exception) {
				a %= 10;
				b = 2;
			}
			finally {
				a++;
			}
		}

		return a;
	}

	[Benchmark("ExceptionCatchFinally", "Tests try-catch-finally no exception thrown")]
	public static int TryCatchFinallyNoE() {
		int a = 1;
		int b = 2;
		for (int i = 0; i < LoopIterations; i++) {
			try {
				a *= 2;
				a += 2;
				a %= 20;
				a /= b;
				b /= 2;
				b++;
			}
			catch (Exception) {
				a %= 10;
				throw;
			}
			finally {
				a++;
			}
		}

		return a;
	}
	
	[Benchmark("ExceptionCatchFinally",
		"Tests try-catch-finally 'equivalent' using an if statement to check for exception")]
	public static int TryCatchNoEFinallyWithIf() {
		int a = 1;
		int b = 2;
		for (int i = 0; i < LoopIterations; i++) {
			a *= 2;
			a += 2;
			a %= 20;

			if (b == 0) {
				a %= 10;
			}
			else {
				a /= b;
				b /= 2;
				b++;
			}

			a++;
		}

		return a;
	}
}