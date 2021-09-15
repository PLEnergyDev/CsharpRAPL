using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsharpRAPL.Data;
using CsvHelper;
using CsvHelper.Configuration;
using NUnit.Framework;

namespace CsharpRAPL.Tests {
	public class AnalysisTest {
		private readonly List<BenchmarkResult> _addSet = new() {
			new BenchmarkResult
				{ ElapsedTime = 149.586, PackagePower = 2566705, DramPower = 80688, Temperature = 76.5 },
			new BenchmarkResult { ElapsedTime = 148.821, PackagePower = 2483820, DramPower = 75745, Temperature = 75 },
			new BenchmarkResult
				{ ElapsedTime = 148.009, PackagePower = 2458673, DramPower = 74646, Temperature = 74.5 },
			new BenchmarkResult
				{ ElapsedTime = 147.915, PackagePower = 2459466, DramPower = 74523, Temperature = 74.5 },
			new BenchmarkResult { ElapsedTime = 148.290, PackagePower = 2483575, DramPower = 74463, Temperature = 75 },
			new BenchmarkResult { ElapsedTime = 151.454, PackagePower = 2592645, DramPower = 78369, Temperature = 73.5 }
		};

		private readonly List<BenchmarkResult> _divideSet = new() {
			new BenchmarkResult
				{ ElapsedTime = 293.600, PackagePower = 4949878, DramPower = 147582, Temperature = 74.5 },
			new BenchmarkResult
				{ ElapsedTime = 295.657, PackagePower = 5075792, DramPower = 149658, Temperature = 73.5 },
			new BenchmarkResult
				{ ElapsedTime = 297.470, PackagePower = 5139086, DramPower = 157470, Temperature = 73.5 },
			new BenchmarkResult { ElapsedTime = 293.774, PackagePower = 4986316, DramPower = 148743, Temperature = 74 },
			new BenchmarkResult { ElapsedTime = 294.654, PackagePower = 5026781, DramPower = 149535, Temperature = 75 },
			new BenchmarkResult { ElapsedTime = 293.086, PackagePower = 4952380, DramPower = 147156, Temperature = 75 }
		};

		[OneTimeSetUp]
		public void Setup() {
			using (var addWriter = new StreamWriter("addSet.csv")) {
				using var addCsv = new CsvWriter(addWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
					{ Delimiter = ";" });

				addCsv.WriteRecords(_addSet);
			}

			using (var divideWriter = new StreamWriter("divideSet.csv")) {
				using var divideCsv = new CsvWriter(divideWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
					{ Delimiter = ";" });
				divideCsv.WriteRecords(_divideSet);
			}
		}

		[OneTimeTearDown]
		public void OneTimeTearDown() {
			if (File.Exists("addSet.csv"))
				File.Delete("addSet.csv");

			if (File.Exists("divideSet.csv"))
				File.Delete("divideSet.csv");
		}

		[Test]
		public void TestAnalysisUsingResults01() {
			Analysis.Analysis analysis = new("Add", _addSet, "Divide", _divideSet);

			Dictionary<string, double> pValues = analysis.CalculatePValue();

			Assert.AreEqual(6, pValues.Count);
			Assert.AreEqual(7.2445981000707043E-12, pValues["Add lower than Divide Time"], double.Epsilon);
			Assert.AreEqual(0.9999999999812168, pValues["Divide lower than Add Time"], double.Epsilon);
			Assert.AreEqual(6.7758661046381478E-10, pValues["Add lower than Divide Package"], double.Epsilon);
			Assert.AreEqual(0.99999999750195823, pValues["Divide lower than Add Package"], double.Epsilon);
			Assert.AreEqual(5.5762042132764618E-09, pValues["Add lower than Divide Dram"], double.Epsilon);
			Assert.AreEqual(0.99999996149930748, pValues["Divide lower than Add Dram"], double.Epsilon);
		}

		[Test]
		public void TestAnalysisUsingResults02() {
			Analysis.Analysis analysis = new("Test1", _addSet, "Test2", _divideSet);

			Dictionary<string, double> pValues = analysis.CalculatePValue();

			Assert.AreEqual(6, pValues.Count);
			Assert.AreEqual(7.2445981000707043E-12, pValues["Test1 lower than Test2 Time"], double.Epsilon);
			Assert.AreEqual(0.9999999999812168, pValues["Test2 lower than Test1 Time"], double.Epsilon);
			Assert.AreEqual(6.7758661046381478E-10, pValues["Test1 lower than Test2 Package"], double.Epsilon);
			Assert.AreEqual(0.99999999750195823, pValues["Test2 lower than Test1 Package"], double.Epsilon);
			Assert.AreEqual(5.5762042132764618E-09, pValues["Test1 lower than Test2 Dram"], double.Epsilon);
			Assert.AreEqual(0.99999996149930748, pValues["Test2 lower than Test1 Dram"], double.Epsilon);
		}

		[Test]
		public void TestAnalysisUsingCSV01() {
			Analysis.Analysis analysis = new("addSet.csv", "divideSet.csv");

			Dictionary<string, double> pValues = analysis.CalculatePValue();

			Assert.AreEqual(6, pValues.Count);
			Assert.AreEqual(7.2445981000707043E-12, pValues["addSet lower than divideSet Time"], double.Epsilon);
			Assert.AreEqual(0.9999999999812168, pValues["divideSet lower than addSet Time"], double.Epsilon);
			Assert.AreEqual(6.7758661046381478E-10, pValues["addSet lower than divideSet Package"], double.Epsilon);
			Assert.AreEqual(0.99999999750195823, pValues["divideSet lower than addSet Package"], double.Epsilon);
			Assert.AreEqual(5.5762042132764618E-09, pValues["addSet lower than divideSet Dram"], double.Epsilon);
			Assert.AreEqual(0.99999996149930748, pValues["divideSet lower than addSet Dram"], double.Epsilon);
		}
	}
}