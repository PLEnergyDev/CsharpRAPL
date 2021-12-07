using System.Diagnostics.CodeAnalysis;
using CsharpRAPL.Benchmarking;

namespace Benchmarks.Operations;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ConditionalOperatorsBenchmarks {
	public static int Iterations;
	public static int LoopIterations;

	//== != > >= < <= && || !

	[Benchmark("ConditionalOperators", "Tests equals using i % 5 == 0")]
	public static bool Equal() {
		bool result = false;
		for (int i = 0; i < LoopIterations; i++) {
			result = i % 5 == 0;
		}

		return result;
	}

	[Benchmark("ConditionalOperators", "Tests not equal using i % 5 != 0")]
	public static bool NotEqual() {
		bool result = false;
		for (int i = 0; i < LoopIterations; i++) {
			result = i % 5 != 0;
		}

		return result;
	}

	[Benchmark("ConditionalOperators", "Tests greater than using i % 5 > 2")]
	public static bool GreaterThan() {
		bool result = false;
		for (int i = 0; i < LoopIterations; i++) {
			result = i % 5 > 2;
		}

		return result;
	}

	[Benchmark("ConditionalOperators", "Tests less than using i % 5 < 2")]
	public static bool LessThan() {
		bool result = false;
		for (int i = 0; i < LoopIterations; i++) {
			result = i % 5 < 2;
		}

		return result;
	}

	[Benchmark("ConditionalOperators", "Tests greater or equal than using i % 5 >= 2")]
	public static bool GreaterOrEqualThan() {
		bool result = false;
		for (int i = 0; i < LoopIterations; i++) {
			result = i % 5 >= 2;
		}

		return result;
	}

	[Benchmark("ConditionalOperators", "Tests less or equal than using i % 5 <= 2")]
	public static bool LessOrEqualThan() {
		bool result = false;
		for (int i = 0; i < LoopIterations; i++) {
			result = i % 5 <= 2;
		}

		return result;
	}

	[Benchmark("ConditionalOperators", "Tests or using i % 5 < 2 || i % 5 < 4")]
	public static bool Or() {
		bool result = false;
		for (int i = 0; i < LoopIterations; i++) {
			result = i % 5 < 2 || i % 5 < 4;
		}

		return result;
	}

	[Benchmark("ConditionalOperators", "Tests or pattern using i % 5 is < 2 or < 4")]
	public static bool OrPattern() {
		bool result = false;
		for (int i = 0; i < LoopIterations; i++) {
			result = i % 5 is < 2 or < 4;
		}

		return result;
	}

	[Benchmark("ConditionalOperators", "Tests and using i % 5 > 2 && i % 5 < 4")]
	public static bool And() {
		bool result = false;
		for (int i = 0; i < LoopIterations; i++) {
			result = i % 5 > 2 && i % 5 < 4;
		}

		return result;
	}

	[Benchmark("ConditionalOperators", "Tests and pattern using i % 5 is > 2 and < 4")]
	public static bool AndPattern() {
		bool result = false;
		for (int i = 0; i < LoopIterations; i++) {
			result = i % 5 is > 2 and < 4;
		}

		return result;
	}

	[Benchmark("ConditionalOperators", "Tests negate than using !(i % 5 == 0)")]
	public static bool Negate() {
		bool result = false;
		for (int i = 0; i < LoopIterations; i++) {
			result = !(i % 5 == 0);
		}

		return result;
	}
}