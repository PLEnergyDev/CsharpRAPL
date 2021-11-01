using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsharpRAPL.Data;
using CsvHelper;
using CsvHelper.Configuration;
using NUnit.Framework;

namespace CsharpRAPL.Tests; 

public class AnalysisTest {
	private readonly List<BenchmarkResult> _addSet = new() {
		new BenchmarkResult {
			ElapsedTime = 0.05249999999999977, PackagePower = 29236, DramPower = 672, Temperature = 43000,
			Result = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.05039999999999978, PackagePower = 30823, DramPower = 672, Temperature = 43000,
			Result = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.044800000000000395, PackagePower = 31494, DramPower = 1098, Temperature = 42000,
			Result = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.04449999999999932, PackagePower = 39368, DramPower = 977, Temperature = 42000,
			Result = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.04420000000000002, PackagePower = 39917, DramPower = 976, Temperature = 43000,
			Result = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.04460000000000086, PackagePower = 33508, DramPower = 733, Temperature = 43000,
			Result = "10"
		}
	};

	private readonly List<BenchmarkResult> _subtractSet = new() {
		new BenchmarkResult {
			ElapsedTime = 0.05210000000000026, PackagePower = 26122, DramPower = 611, Temperature = 44000,
			Result = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.05370000000000008, PackagePower = 24720, DramPower = 610, Temperature = 44000,
			Result = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.054999999999999716, PackagePower = 30884, DramPower = 977, Temperature = 44000,
			Result = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.05240000000000222, PackagePower = 27709, DramPower = 671, Temperature = 44000,
			Result = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.05190000000000339, PackagePower = 25696, DramPower = 611, Temperature = 44000,
			Result = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.050200000000000244, PackagePower = 24536, DramPower = 611, Temperature = 44000,
			Result = "10"
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

		Assert.AreEqual(672.0d, firstData.DramPower, double.Epsilon);
		Assert.AreEqual(610.0d, secondData.DramPower, double.Epsilon);

		Assert.AreEqual(29236.0d, firstData.PackagePower, double.Epsilon);
		Assert.AreEqual(24536.0d, secondData.PackagePower, double.Epsilon);

		Assert.AreEqual(0.044200000000000017d, firstData.ElapsedTime, double.Epsilon);
		Assert.AreEqual(0.050200000000000244d, secondData.ElapsedTime, double.Epsilon);

		Assert.AreEqual(42000.0d, firstData.Temperature, double.Epsilon);
		Assert.AreEqual(44000.0d, secondData.Temperature, double.Epsilon);
	}

	[Test]
	public void TestMax01() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		((string firstName, BenchmarkResult firstData), (string secondName, BenchmarkResult secondData)) =
			analysis.GetMax();
		Assert.AreEqual("AddSet", firstName);
		Assert.AreEqual("SubtractSet", secondName);

		Assert.AreEqual(1098.0d, firstData.DramPower, double.Epsilon);
		Assert.AreEqual(977.0d, secondData.DramPower, double.Epsilon);

		Assert.AreEqual(39917.0d, firstData.PackagePower, double.Epsilon);
		Assert.AreEqual(30884.0d, secondData.PackagePower, double.Epsilon);

		Assert.AreEqual(0.052499999999999769d, firstData.ElapsedTime, double.Epsilon);
		Assert.AreEqual(0.054999999999999716d, secondData.ElapsedTime, double.Epsilon);

		Assert.AreEqual(43000.0d, firstData.Temperature, double.Epsilon);
		Assert.AreEqual(44000.0d, secondData.Temperature, double.Epsilon);
	}

	[Test]
	public void TestAverage01() {
		Analysis.Analysis analysis = new("AddSet.csv", "SubtractSet.csv");
		((string firstName, BenchmarkResult firstData), (string secondName, BenchmarkResult secondData)) =
			analysis.GetAverage();
		Assert.AreEqual("AddSet", firstName);
		Assert.AreEqual("SubtractSet", secondName);

		Assert.AreEqual(854.66666666666663d, firstData.DramPower, double.Epsilon);
		Assert.AreEqual(681.83333333333337d, secondData.DramPower, double.Epsilon);

		Assert.AreEqual(34057.666666666664d, firstData.PackagePower, double.Epsilon);
		Assert.AreEqual(26611.166666666668d, secondData.PackagePower, double.Epsilon);

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
				ElapsedTime = 0.05210000000000026, PackagePower = 26122, DramPower = 611, Temperature = 44000,
				Result = "10"
			},
			new BenchmarkResult {
				ElapsedTime = 0.05370000000000008, PackagePower = 24720, DramPower = 610, Temperature = 44000,
				Result = "10"
			},
			new BenchmarkResult {
				ElapsedTime = 0.054999999999999716, PackagePower = 30884, DramPower = 977, Temperature = 44000,
				Result = "10"
			},
			new BenchmarkResult {
				ElapsedTime = 0.05240000000000222, PackagePower = 27709, DramPower = 671, Temperature = 44000,
				Result = "10"
			},
			new BenchmarkResult {
				ElapsedTime = 0.05190000000000339, PackagePower = 25696, DramPower = 611, Temperature = 44000,
				Result = "10"
			},
			new BenchmarkResult {
				ElapsedTime = 0.050200000000000244, PackagePower = 24536, DramPower = 611, Temperature = 44000,
				Result = "11"
			}
		};
		Analysis.Analysis analysis = new("BadSet", badSet, "AddSet", _addSet);

		(bool isValid, string message) = analysis.EnsureResults();

		Assert.False(isValid);
		Assert.AreEqual("Not all results in BadSet was equal. Namely: 10, 11.", message);
	}
}