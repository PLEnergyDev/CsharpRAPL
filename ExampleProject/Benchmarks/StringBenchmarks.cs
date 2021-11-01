using System.Diagnostics.CodeAnalysis;
using System.Text;
using CsharpRAPL.Benchmarking;

namespace ExampleProject.Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class StringBenchmarks {
	public static int Iterations;
	public static int LoopIterations;


	[Benchmark("StringConcat", "Tests operation on simple string")]
	public static string PlusSign() {
		string str = "";
		string iStr = "I ";
		string am = "am ";
		string a = "a ";
		string stringStr = "string ";
		string with = "with ";
		string integer = "integer ";
		int myInt = 42;
		for (int i = 0; i < LoopIterations; i++) {
			str = "";
			str += iStr;
			str += am;
			str += a;
			str += stringStr;
			str += with;
			str += integer;
			str += myInt;
		}

		return str;
	}

	[Benchmark("StringConcat", "Tests operation on stringbuilder")]
	public static string StringBuilder() {
		StringBuilder sb = new StringBuilder();
		string iStr = "I ";
		string am = "am ";
		string a = "a ";
		string stringStr = "string ";
		string with = "with ";
		string integer = "integer ";
		int myInt = 42;
		for (int i = 0; i < LoopIterations; i++) {
			sb.Clear();
			sb.Append(iStr);
			sb.Append(am);
			sb.Append(a);
			sb.Append(stringStr);
			sb.Append(with);
			sb.Append(integer);
			sb.Append(myInt);
		}

		return sb.ToString();
	}

	[Benchmark("StringConcat", "Tests string.Format")]
	public static string Format() {
		string str = "";
		string iStr = "I ";
		string am = "am ";
		string a = "a ";
		string stringStr = "string ";
		string with = "with ";
		string integer = "integer ";
		int myInt = 42;
		for (int i = 0; i < LoopIterations; i++) {
			str = string.Format("{0}{1}{2}{3}{4}{5}{6}", iStr, am, a, stringStr, with, integer, myInt);
		}

		return str;
	}

	[Benchmark("StringConcat", "Tests string interpolation")]
	public static string Interpolation() {
		string str = "";
		string iStr = "I ";
		string am = "am ";
		string a = "a ";
		string stringStr = "string ";
		string with = "with ";
		string integer = "integer ";
		int myInt = 42;
		for (int i = 0; i < LoopIterations; i++) {
			str = $"{iStr}{am}{a}{stringStr}{with}{integer}{myInt}";
		}

		return str;
	}
}