using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using CsharpRAPL.Analysis;
using CsharpRAPL.Data;
using CsvHelper;
using CsvHelper.Configuration;
using NUnit.Framework;

namespace CsharpRAPL.Tests;

public class AnalysisTest {
	private readonly List<BenchmarkResult> _addSet = new() {
		new BenchmarkResult {
			ElapsedTime = 0.05249999999999977, PackageEnergy = 29236, DRAMEnergy = 672, Temperature = 43000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.05039999999999978, PackageEnergy = 30823, DRAMEnergy = 672, Temperature = 43000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.044800000000000395, PackageEnergy = 31494, DRAMEnergy = 1098, Temperature = 42000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.04449999999999932, PackageEnergy = 39368, DRAMEnergy = 977, Temperature = 42000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.04420000000000002, PackageEnergy = 39917, DRAMEnergy = 976, Temperature = 43000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.04460000000000086, PackageEnergy = 33508, DRAMEnergy = 733, Temperature = 43000,
			BenchmarkReturnValue = "10"
		}
	};

	private readonly List<BenchmarkResult> _subtractSet = new() {
		new BenchmarkResult {
			ElapsedTime = 0.05210000000000026, PackageEnergy = 26122, DRAMEnergy = 611, Temperature = 44000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.05370000000000008, PackageEnergy = 24720, DRAMEnergy = 610, Temperature = 44000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.054999999999999716, PackageEnergy = 30884, DRAMEnergy = 977, Temperature = 44000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.05240000000000222, PackageEnergy = 27709, DRAMEnergy = 671, Temperature = 44000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.05190000000000339, PackageEnergy = 25696, DRAMEnergy = 611, Temperature = 44000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.050200000000000244, PackageEnergy = 24536, DRAMEnergy = 611, Temperature = 44000,
			BenchmarkReturnValue = "10"
		}
	};

	[OneTimeSetUp]
	public void Setup() {
		using (var addWriter = new StreamWriter("AddSet.csv")) {
			using var addCsv = new CsvWriter(addWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
				{ Delimiter = ";" });

			addCsv.WriteRecords(_addSet);
		}

		using (var subtractWriter = new StreamWriter("SubtractSet.csv")) {
			using var subtractCsv = new CsvWriter(subtractWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
				{ Delimiter = ";" });
			subtractCsv.WriteRecords(_subtractSet);
		}
	}

	[OneTimeTearDown]
	public void OneTimeTearDown() {
		if (File.Exists("AddSet.csv")) {
			File.Delete("AddSet.csv");
		}

		if (File.Exists("SubtractSet.csv")) {
			File.Delete("SubtractSet.csv");
		}

		if (Directory.Exists("_plots")) {
			Directory.Delete("_plots", true);
		}
	}

	[Test]
	public void TestAnalysisUsingResults01() {
		Analysis.Analysis analysis = new("Add", _addSet, "Subtract", _subtractSet);

		List<(string Message, double Value)> pValues = analysis.CalculatePValue();

		Assert.AreEqual(3, pValues.Count);
		Assert.AreEqual(0.0056837042764399381d, pValues[0].Value, double.Epsilon);
		Assert.AreEqual(0.0052142299229074318d, pValues[1].Value, double.Epsilon);
		Assert.AreEqual(0.10275597927588676d, pValues[2].Value, double.Epsilon);
	}

	[Test]
	public void TestAnalysisUsingResults02() {
		Analysis.Analysis analysis = new("Test1", _addSet, "Test2", _subtractSet);

		List<(string Message, double Value)> pValues = analysis.CalculatePValue();

		Assert.AreEqual(3, pValues.Count);
		Assert.AreEqual(0.0056837042764399381d, pValues[0].Value, double.Epsilon);
		Assert.AreEqual(0.0052142299229074318d, pValues[1].Value, double.Epsilon);
		Assert.AreEqual(0.10275597927588676d, pValues[2].Value, double.Epsilon);
	}

	[Test]
	public void TestAnalysisUsingCSV01() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");

		List<(string Message, double Value)> pValues = analysis.CalculatePValue();

		Assert.AreEqual(3, pValues.Count);
		Assert.AreEqual(0.0056837042764399381d, pValues[0].Value, double.Epsilon);
		Assert.AreEqual(0.0052142299229074318d, pValues[1].Value, double.Epsilon);
		Assert.AreEqual(0.10275597927588676d, pValues[2].Value, double.Epsilon);
	}

	[Test]
	public void TestMin01() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		((string firstName, BenchmarkResult firstData), (string secondName, BenchmarkResult secondData)) =
			analysis.GetMin();
		Assert.AreEqual("AddSet", firstName);
		Assert.AreEqual("SubtractSet", secondName);

		Assert.AreEqual(672.0d, firstData.DRAMEnergy, double.Epsilon);
		Assert.AreEqual(610.0d, secondData.DRAMEnergy, double.Epsilon);

		Assert.AreEqual(29236.0d, firstData.PackageEnergy, double.Epsilon);
		Assert.AreEqual(24536.0d, secondData.PackageEnergy, double.Epsilon);

		Assert.AreEqual(0.044200000000000017d, firstData.ElapsedTime, double.Epsilon);
		Assert.AreEqual(0.050200000000000244d, secondData.ElapsedTime, double.Epsilon);

		Assert.AreEqual(42000.0d, firstData.Temperature, double.Epsilon);
		Assert.AreEqual(44000.0d, secondData.Temperature, double.Epsilon);
	}

	[Test]
	public void TestMinBy01() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		((string firstName, BenchmarkResult firstData), (string secondName, BenchmarkResult secondData)) =
			analysis.GetMinBy(BenchmarkResultType.ElapsedTime);
		Assert.AreEqual("AddSet", firstName);
		Assert.AreEqual("SubtractSet", secondName);

		Assert.AreEqual(976.0d, firstData.DRAMEnergy, double.Epsilon);
		Assert.AreEqual(611.0d, secondData.DRAMEnergy, double.Epsilon);

		Assert.AreEqual(39917.0d, firstData.PackageEnergy, double.Epsilon);
		Assert.AreEqual(24536.0d, secondData.PackageEnergy, double.Epsilon);

		Assert.AreEqual(0.044200000000000017d, firstData.ElapsedTime, double.Epsilon);
		Assert.AreEqual(0.050200000000000244d, secondData.ElapsedTime, double.Epsilon);

		Assert.AreEqual(43000.0d, firstData.Temperature, double.Epsilon);
		Assert.AreEqual(44000.0d, secondData.Temperature, double.Epsilon);
	}

	[Test]
	public void TestMinBy02() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		((string firstName, BenchmarkResult firstData), (string secondName, BenchmarkResult secondData)) =
			analysis.GetMinBy(BenchmarkResultType.Temperature);
		Assert.AreEqual("AddSet", firstName);
		Assert.AreEqual("SubtractSet", secondName);

		Assert.AreEqual(1098.0d, firstData.DRAMEnergy, double.Epsilon);
		Assert.AreEqual(611.0d, secondData.DRAMEnergy, double.Epsilon);

		Assert.AreEqual(31494.0d, firstData.PackageEnergy, double.Epsilon);
		Assert.AreEqual(26122.0d, secondData.PackageEnergy, double.Epsilon);

		Assert.AreEqual(0.044800000000000395d, firstData.ElapsedTime, double.Epsilon);
		Assert.AreEqual(0.052100000000000257d, secondData.ElapsedTime, double.Epsilon);

		Assert.AreEqual(42000.0d, firstData.Temperature, double.Epsilon);
		Assert.AreEqual(44000.0d, secondData.Temperature, double.Epsilon);
	}

	[Test]
	public void TestMinBy03() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		((string firstName, BenchmarkResult firstData), (string secondName, BenchmarkResult secondData)) =
			analysis.GetMinBy(BenchmarkResultType.PackageEnergy);
		Assert.AreEqual("AddSet", firstName);
		Assert.AreEqual("SubtractSet", secondName);

		Assert.AreEqual(672.0d, firstData.DRAMEnergy, double.Epsilon);
		Assert.AreEqual(611.0d, secondData.DRAMEnergy, double.Epsilon);

		Assert.AreEqual(29236.0d, firstData.PackageEnergy, double.Epsilon);
		Assert.AreEqual(24536.0d, secondData.PackageEnergy, double.Epsilon);

		Assert.AreEqual(0.052499999999999769d, firstData.ElapsedTime, double.Epsilon);
		Assert.AreEqual(0.050200000000000244d, secondData.ElapsedTime, double.Epsilon);

		Assert.AreEqual(43000.0d, firstData.Temperature, double.Epsilon);
		Assert.AreEqual(44000.0d, secondData.Temperature, double.Epsilon);
	}

	[Test]
	public void TestMinBy04() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		((string firstName, BenchmarkResult firstData), (string secondName, BenchmarkResult secondData)) =
			analysis.GetMinBy(BenchmarkResultType.DRAMEnergy);
		Assert.AreEqual("AddSet", firstName);
		Assert.AreEqual("SubtractSet", secondName);

		Assert.AreEqual(672.0d, firstData.DRAMEnergy, double.Epsilon);
		Assert.AreEqual(610.0d, secondData.DRAMEnergy, double.Epsilon);

		Assert.AreEqual(29236.0d, firstData.PackageEnergy, double.Epsilon);
		Assert.AreEqual(24720.0d, secondData.PackageEnergy, double.Epsilon);

		Assert.AreEqual(0.052499999999999769d, firstData.ElapsedTime, double.Epsilon);
		Assert.AreEqual(0.053700000000000081d, secondData.ElapsedTime, double.Epsilon);

		Assert.AreEqual(43000.0d, firstData.Temperature, double.Epsilon);
		Assert.AreEqual(44000.0d, secondData.Temperature, double.Epsilon);
	}

	[Test]
	public void TestMax01() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		((string firstName, BenchmarkResult firstData), (string secondName, BenchmarkResult secondData)) =
			analysis.GetMax();
		Assert.AreEqual("AddSet", firstName);
		Assert.AreEqual("SubtractSet", secondName);

		Assert.AreEqual(1098.0d, firstData.DRAMEnergy, double.Epsilon);
		Assert.AreEqual(977.0d, secondData.DRAMEnergy, double.Epsilon);

		Assert.AreEqual(39917.0d, firstData.PackageEnergy, double.Epsilon);
		Assert.AreEqual(30884.0d, secondData.PackageEnergy, double.Epsilon);

		Assert.AreEqual(0.052499999999999769d, firstData.ElapsedTime, double.Epsilon);
		Assert.AreEqual(0.054999999999999716d, secondData.ElapsedTime, double.Epsilon);

		Assert.AreEqual(43000.0d, firstData.Temperature, double.Epsilon);
		Assert.AreEqual(44000.0d, secondData.Temperature, double.Epsilon);
	}

	[Test]
	public void TestMaxBy01() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		((string firstName, BenchmarkResult firstData), (string secondName, BenchmarkResult secondData)) =
			analysis.GetMaxBy(BenchmarkResultType.ElapsedTime);
		Assert.AreEqual("AddSet", firstName);
		Assert.AreEqual("SubtractSet", secondName);

		Assert.AreEqual(672.0d, firstData.DRAMEnergy, double.Epsilon);
		Assert.AreEqual(977.0d, secondData.DRAMEnergy, double.Epsilon);

		Assert.AreEqual(29236.0d, firstData.PackageEnergy, double.Epsilon);
		Assert.AreEqual(30884.0d, secondData.PackageEnergy, double.Epsilon);

		Assert.AreEqual(0.052499999999999769d, firstData.ElapsedTime, double.Epsilon);
		Assert.AreEqual(0.054999999999999716d, secondData.ElapsedTime, double.Epsilon);

		Assert.AreEqual(43000.0d, firstData.Temperature, double.Epsilon);
		Assert.AreEqual(44000.0d, secondData.Temperature, double.Epsilon);
	}

	[Test]
	public void TestMaxBy02() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		((string firstName, BenchmarkResult firstData), (string secondName, BenchmarkResult secondData)) =
			analysis.GetMaxBy(BenchmarkResultType.Temperature);
		Assert.AreEqual("AddSet", firstName);
		Assert.AreEqual("SubtractSet", secondName);

		Assert.AreEqual(672.0d, firstData.DRAMEnergy, double.Epsilon);
		Assert.AreEqual(611.0d, secondData.DRAMEnergy, double.Epsilon);

		Assert.AreEqual(29236.0d, firstData.PackageEnergy, double.Epsilon);
		Assert.AreEqual(26122.0d, secondData.PackageEnergy, double.Epsilon);

		Assert.AreEqual(0.052499999999999769d, firstData.ElapsedTime, double.Epsilon);
		Assert.AreEqual(0.052100000000000257d, secondData.ElapsedTime, double.Epsilon);

		Assert.AreEqual(43000.0d, firstData.Temperature, double.Epsilon);
		Assert.AreEqual(44000.0d, secondData.Temperature, double.Epsilon);
	}

	[Test]
	public void TestMaxBy03() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		((string firstName, BenchmarkResult firstData), (string secondName, BenchmarkResult secondData)) =
			analysis.GetMaxBy(BenchmarkResultType.PackageEnergy);
		Assert.AreEqual("AddSet", firstName);
		Assert.AreEqual("SubtractSet", secondName);

		Assert.AreEqual(976.0d, firstData.DRAMEnergy, double.Epsilon);
		Assert.AreEqual(977.0d, secondData.DRAMEnergy, double.Epsilon);

		Assert.AreEqual(39917.0d, firstData.PackageEnergy, double.Epsilon);
		Assert.AreEqual(30884.0d, secondData.PackageEnergy, double.Epsilon);

		Assert.AreEqual(0.044200000000000017d, firstData.ElapsedTime, double.Epsilon);
		Assert.AreEqual(0.054999999999999716d, secondData.ElapsedTime, double.Epsilon);

		Assert.AreEqual(43000.0d, firstData.Temperature, double.Epsilon);
		Assert.AreEqual(44000.0d, secondData.Temperature, double.Epsilon);
	}

	[Test]
	public void TestMaxBy04() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		((string firstName, BenchmarkResult firstData), (string secondName, BenchmarkResult secondData)) =
			analysis.GetMaxBy(BenchmarkResultType.DRAMEnergy);
		Assert.AreEqual("AddSet", firstName);
		Assert.AreEqual("SubtractSet", secondName);

		Assert.AreEqual(1098.0d, firstData.DRAMEnergy, double.Epsilon);
		Assert.AreEqual(977.0d, secondData.DRAMEnergy, double.Epsilon);

		Assert.AreEqual(31494.0d, firstData.PackageEnergy, double.Epsilon);
		Assert.AreEqual(30884.0d, secondData.PackageEnergy, double.Epsilon);

		Assert.AreEqual(0.044800000000000395d, firstData.ElapsedTime, double.Epsilon);
		Assert.AreEqual(0.054999999999999716d, secondData.ElapsedTime, double.Epsilon);

		Assert.AreEqual(42000.0d, firstData.Temperature, double.Epsilon);
		Assert.AreEqual(44000.0d, secondData.Temperature, double.Epsilon);
	}

	[Test]
	public void TestAverage01() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		((string firstName, BenchmarkResult firstData), (string secondName, BenchmarkResult secondData)) =
			analysis.GetAverage();
		Assert.AreEqual("AddSet", firstName);
		Assert.AreEqual("SubtractSet", secondName);

		Assert.AreEqual(854.66666666666663d, firstData.DRAMEnergy, double.Epsilon);
		Assert.AreEqual(681.83333333333337d, secondData.DRAMEnergy, double.Epsilon);

		Assert.AreEqual(34057.666666666664d, firstData.PackageEnergy, double.Epsilon);
		Assert.AreEqual(26611.166666666668d, secondData.PackageEnergy, double.Epsilon);

		Assert.AreEqual(0.046833333333333359d, firstData.ElapsedTime, double.Epsilon);
		Assert.AreEqual(0.052550000000000985d, secondData.ElapsedTime, double.Epsilon);

		Assert.AreEqual(42666.666666666664d, firstData.Temperature, double.Epsilon);
		Assert.AreEqual(44000.0d, secondData.Temperature, double.Epsilon);
	}

	[Test]
	public void TestEnsure01() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		(bool isValid, string message) = analysis.EnsureResults();
		Assert.True(isValid);
		Assert.AreEqual("", message);
	}

	[Test]
	public void TestEnsure02() {
		List<BenchmarkResult> badSet = new() {
			new BenchmarkResult {
				ElapsedTime = 0.05210000000000026, PackageEnergy = 26122, DRAMEnergy = 611, Temperature = 44000,
				BenchmarkReturnValue = "10"
			},
			new BenchmarkResult {
				ElapsedTime = 0.05370000000000008, PackageEnergy = 24720, DRAMEnergy = 610, Temperature = 44000,
				BenchmarkReturnValue = "10"
			},
			new BenchmarkResult {
				ElapsedTime = 0.054999999999999716, PackageEnergy = 30884, DRAMEnergy = 977, Temperature = 44000,
				BenchmarkReturnValue = "10"
			},
			new BenchmarkResult {
				ElapsedTime = 0.05240000000000222, PackageEnergy = 27709, DRAMEnergy = 671, Temperature = 44000,
				BenchmarkReturnValue = "10"
			},
			new BenchmarkResult {
				ElapsedTime = 0.05190000000000339, PackageEnergy = 25696, DRAMEnergy = 611, Temperature = 44000,
				BenchmarkReturnValue = "10"
			},
			new BenchmarkResult {
				ElapsedTime = 0.050200000000000244, PackageEnergy = 24536, DRAMEnergy = 611, Temperature = 44000,
				BenchmarkReturnValue = "11"
			}
		};
		Analysis.Analysis analysis = new("BadSet", badSet, "AddSet", _addSet);

		(bool isValid, string message) = analysis.EnsureResults();

		Assert.False(isValid);
		Assert.AreEqual("Not all results in BadSet was equal. Namely: 10, 11.", message);
	}

	[Test]
	public void TestEnsureMutually01() {
		Analysis.Analysis analysis = new("t1", new List<BenchmarkResult>(), "t2", new List<BenchmarkResult>());
		(bool isValid, string message) = analysis.EnsureResultsMutually();
		Assert.False(isValid);
		Assert.AreEqual("t1 has no results", message);
	}

	[Test]
	public void TestEnsureMutually02() {
		Analysis.Analysis analysis = new("t1",
			new List<BenchmarkResult>() { new BenchmarkResult() { ElapsedTime = 0.2d, Temperature = 21 } }, "t2",
			new List<BenchmarkResult>());
		(bool isValid, string message) = analysis.EnsureResultsMutually();
		Assert.False(isValid);
		Assert.AreEqual("t2 has no results", message);
	}

	[Test]
	public void TestEnsureMutually03() {
		Analysis.Analysis analysis = new("t1",
			new List<BenchmarkResult>() { new() { BenchmarkReturnValue = "10" } }, "t2",
			new List<BenchmarkResult>() { new() { BenchmarkReturnValue = "13" } });
		(bool isValid, string message) = analysis.EnsureResultsMutually();
		Assert.False(isValid);
		Assert.AreEqual("The to datasets differ in 10 and 13", message);
	}

	[Test]
	public void TestPlotAnalysis01() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		analysis.PlotAnalysis();
		Assert.AreEqual(1, GetFileCountWildCard("_plots/ElapsedTime", "AddSet-SubtractSet-([\\s\\S]*).png"));
		Assert.AreEqual(1, GetFileCountWildCard("_plots/Temperature", "AddSet-SubtractSet-([\\s\\S]*).png"));
		Assert.AreEqual(1, GetFileCountWildCard("_plots/PackageEnergy", "AddSet-SubtractSet-([\\s\\S]*).png"));
		Assert.AreEqual(1, GetFileCountWildCard("_plots/DRAMEnergy", "AddSet-SubtractSet-([\\s\\S]*).png"));
	}

	[Test]
	public void TestCheckExecutionTime01() {
		using var sw = new StringWriter();
		Console.SetOut(sw);
		Analysis.Analysis.CheckExecutionTime(new DataSet("AddSet.csv"));
		string result = sw.ToString().Trim().Replace("\r","");
		Assert.AreEqual(
			@"AddSet was found to be below 0.3 seconds (0.04420000000000002 seconds) so we might need to check if this gets compiled away",
			result);


		var standardOutput = new StreamWriter(Console.OpenStandardOutput());
		standardOutput.AutoFlush = true;
		Console.SetOut(standardOutput);
	}
	
	[Test]
	public void TestCheckExecutionTime02() {
		using var sw = new StringWriter();
		Console.SetOut(sw);
		Analysis.Analysis.CheckExecutionTime(new DataSet("t1",new List<BenchmarkResult>()));
		string result = sw.ToString().Trim().Replace("\r","");
		Assert.AreEqual("No results were below 0.3 seconds", result);


		var standardOutput = new StreamWriter(Console.OpenStandardOutput());
		standardOutput.AutoFlush = true;
		Console.SetOut(standardOutput);
	}


	private static int GetFileCountWildCard(string path, string pattern) {
		string[] files = Directory.GetFiles(path);
		int res = 0;
		foreach (string file in files) {
			if (Regex.IsMatch(file, pattern)) {
				res++;
			}
		}

		return res;
	}
}