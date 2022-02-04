using System.Diagnostics.CodeAnalysis;
using System.Text;
using CsharpRAPL.Benchmarking;

namespace Benchmarks;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class StringBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;



	[Benchmark("StringConcat", "Tests operation using + on strings")]
	public static string PlusSign() {
		string str = "";
		string iStr = "I ";
		string am = "am ";
		string a = "a ";
		string stringStr = "string ";
		string with = "with ";
		string integer = "integer ";
		ulong myInt = 42;
		for (ulong i  = 0; i < LoopIterations; i++) {
			str = iStr + am + a + stringStr + with + integer + myInt;
		}

		return str;
	}


	[Benchmark("StringConcat", "Tests operation using += strings")]
	public static string PlusCompSign() {
		string str = "";
		string iStr = "I ";
		string am = "am ";
		string a = "a ";
		string stringStr = "string ";
		string with = "with ";
		string integer = "integer ";
		ulong myInt = 42;
		for (ulong i  = 0; i < LoopIterations; i++) {
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

	[Benchmark("StringConcat", "Tests operation on a single line of compound using + strings")]
	public static string PlusCompSignSingle() {
		string str = "";
		string iStr = "I ";
		string am = "am ";
		string a = "a ";
		string stringStr = "string ";
		string with = "with ";
		string integer = "integer ";
		ulong myInt = 42;
		for (ulong i  = 0; i < LoopIterations; i++) {
			str = "";
			str += iStr + am + a + stringStr + with + integer + myInt;
		}

		return str;
	}

	[Benchmark("StringConcat", "Tests operation on string builder")]
	public static string StringBuilder() {
		StringBuilder sb = new StringBuilder();
		string str = "";
		string iStr = "I ";
		string am = "am ";
		string a = "a ";
		string stringStr = "string ";
		string with = "with ";
		string integer = "integer ";
		ulong myInt = 42;
		for (ulong i  = 0; i < LoopIterations; i++) {
			str = "";
			sb.Clear();
			sb.Append(iStr);
			sb.Append(am);
			sb.Append(a);
			sb.Append(stringStr);
			sb.Append(with);
			sb.Append(integer);
			sb.Append(myInt);
			str = sb.ToString();
		}

		return str;
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
		ulong myInt = 42;
		for (ulong i  = 0; i < LoopIterations; i++) {
			str = "";
			str = string.Format("{0}{1}{2}{3}{4}{5}{6}", iStr, am, a, stringStr, with, integer, myInt);
		}

		return str;
	}

	[Benchmark("StringConcat", "Tests string.Concat")]
	public static string Concat() {
		string str = "";
		string iStr = "I ";
		string am = "am ";
		string a = "a ";
		string stringStr = "string ";
		string with = "with ";
		string integer = "integer ";
		ulong myInt = 42;
		for (ulong i  = 0; i < LoopIterations; i++) {
			str = "";
			str = string.Concat(iStr, am, a, stringStr, with, integer, myInt);
		}

		return str;
	}

	[Benchmark("StringConcat", "Tests string.Join")]
	public static string Join() {
		string str = "";
		string iStr = "I ";
		string am = "am ";
		string a = "a ";
		string stringStr = "string ";
		string with = "with ";
		string integer = "integer ";
		ulong myInt = 42;
		for (ulong i  = 0; i < LoopIterations; i++) {
			str = "";
			str = string.Join("", iStr, am, a, stringStr, with, integer, myInt);
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
		ulong myInt = 42;
		for (ulong i  = 0; i < LoopIterations; i++) {
			str = "";
			str = $"{iStr}{am}{a}{stringStr}{with}{integer}{myInt}";
		}

		return str;
	}

	[Benchmark("StringConcat", "Tests constant string interpolation")]
	public static string InterpolationConst() {
		string str = "";
		const string iStr = "I ";
		const string am = "am ";
		const string a = "a ";
		const string stringStr = "string ";
		const string with = "with ";
		const string integer = "integer ";
		const ulong myInt = 42;
		for (ulong i  = 0; i < LoopIterations; i++) {
			str = "";
			str = $"{iStr}{am}{a}{stringStr}{with}{integer}{myInt}";
		}

		return str;
	}
}