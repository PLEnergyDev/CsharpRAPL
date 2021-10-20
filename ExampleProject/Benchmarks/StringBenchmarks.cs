using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using CsharpRAPL.Benchmarking;

namespace ExampleProject.Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class StringBenchmarks {
	public static int Iterations;
	public static int LoopIterations;


	[Benchmark("StringConcat", "Tests operation on simple string")]
	public static string StringPlusSign() {
		string str = "";
		string hello = "hello";
		string world = "world";
		for (var i = 0; i < LoopIterations; i++) {
			str += hello;
			str += world;
			if (i % 100 == 0) str = "";
		}

		return str;
	}

	[Benchmark("StringConcat", "Tests operation on stringbuilder")]
	public static string StringBuilderConcat() {
		StringBuilder sb = new StringBuilder();
		string hello = "hello";
		string world = "world";
		for (var i = 0; i < LoopIterations; i++) {
			sb.Append(hello);
			sb.Append(world);
			if (i % 100 == 0) sb.Clear();
		}

		return sb.ToString();
	}

	[Benchmark("StringConcat", "Tests string.Format")]
	public static string StringFormat() {
		string str = "";
		string hello = "hello";
		string world = "world";
		for (var i = 0; i < LoopIterations; i++) {
			str = string.Format("{0}{1}", hello, world);
		}

		return str;
	}

	[Benchmark("StringConcat", "Tests string interpolation")]
	public static string StringInterpolation() {
		string str = "";
		string hello = "hello";
		string world = "world";
		for (var i = 0; i < LoopIterations; i++) {
			str = $"{hello}{world}";
		}

		return str;
	}
}