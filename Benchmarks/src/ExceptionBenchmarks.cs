using System;
using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ExceptionBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	[Benchmark("Exception", "Tests try-catch no exception thrown")]
	public static ulong TryCatchNoE() {
		ulong a = 1;
		ulong b = 2;
		for (ulong i = 0; i < LoopIterations; i++) {
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
	public static ulong TryFinallyNoE() {
		ulong a = 1;
		ulong b = 2;
		for (ulong i = 0; i < LoopIterations; i++) {
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
	public static ulong TryFinallyEquivNoE() {
		ulong a = 1;
		ulong b = 2;
		for (ulong i = 0; i < LoopIterations; i++) {
			a *= 2;
			a += 2;
			a %= 20;
			a /= b;
		}

		return a;
	}


	[Benchmark("Exception",
		"Tests try-catch exception thrown with catch all statement, throws floor(LoopIterations / 2) exceptions")]
	public static ulong TryCatchAllE() {
		ulong a = 1;
		ulong b = 2;
		for (ulong i = 0; i < LoopIterations; i++) {
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
	public static ulong TryCatchSpecificE() {
		ulong a = 1;
		ulong b = 2;
		for (ulong i = 0; i < LoopIterations; i++) {
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
	public static ulong TryCatchEWMessage() {
		ulong a = 1;
		ulong b = 2;
		for (ulong i = 0; i < LoopIterations; i++) {
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
	public static ulong TryCatchEWOMessage() {
		ulong a = 1;
		ulong b = 2;
		for (ulong i = 0; i < LoopIterations; i++) {
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
	public static ulong TryCatchEWithIf() {
		ulong a = 1;
		ulong b = 2;
		for (ulong i = 0; i < LoopIterations; i++) {
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
	public static ulong TryCatchEFinallyWithIf() {
		ulong a = 1;
		ulong b = 2;
		for (ulong i = 0; i < LoopIterations; i++) {
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
	public static ulong TryCatchFinallyAllE() {
		ulong a = 1;
		ulong b = 2;
		for (ulong i = 0; i < LoopIterations; i++) {
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
	public static ulong TryCatchFinallyNoE() {
		ulong a = 1;
		ulong b = 2;
		for (ulong i = 0; i < LoopIterations; i++) {
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
	public static ulong TryCatchNoEFinallyWithIf() {
		ulong a = 1;
		ulong b = 2;
		for (ulong i = 0; i < LoopIterations; i++) {
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