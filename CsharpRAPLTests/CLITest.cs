using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsharpRAPL.CommandLine;
using CsharpRAPL.Data;
using CsvHelper;
using CsvHelper.Configuration;
using NUnit.Framework;

namespace CsharpRAPL.Tests;

public class CLITest {
	private readonly List<BenchmarkResult> _addSet = new() {
		new BenchmarkResult {
			ElapsedTime = 0.05249999999999977, PackageEnergy = 29236, DramEnergy = 672, Temperature = 43000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.05039999999999978, PackageEnergy = 30823, DramEnergy = 672, Temperature = 43000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.044800000000000395, PackageEnergy = 31494, DramEnergy = 1098, Temperature = 42000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.04449999999999932, PackageEnergy = 39368, DramEnergy = 977, Temperature = 42000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.04420000000000002, PackageEnergy = 39917, DramEnergy = 976, Temperature = 43000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.04460000000000086, PackageEnergy = 33508, DramEnergy = 733, Temperature = 43000,
			BenchmarkReturnValue = "10"
		}
	};

	private readonly List<BenchmarkResult> _subtractSet = new() {
		new BenchmarkResult {
			ElapsedTime = 0.05210000000000026, PackageEnergy = 26122, DramEnergy = 611, Temperature = 44000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.05370000000000008, PackageEnergy = 24720, DramEnergy = 610, Temperature = 44000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.054999999999999716, PackageEnergy = 30884, DramEnergy = 977, Temperature = 44000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.05240000000000222, PackageEnergy = 27709, DramEnergy = 671, Temperature = 44000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.05190000000000339, PackageEnergy = 25696, DramEnergy = 611, Temperature = 44000,
			BenchmarkReturnValue = "10"
		},
		new BenchmarkResult {
			ElapsedTime = 0.050200000000000244, PackageEnergy = 24536, DramEnergy = 611, Temperature = 44000,
			BenchmarkReturnValue = "10"
		}
	};

	private static readonly string[] HelpText = {
		"-g, --SkipPlotGroups         If plotting each benchmark group should be skipped.",
		"-i, --Iterations             (Default: -1) Sets the target iterations. (Disables Dynamic Iteration Calculation)",
		"-l, --LoopIterations         (Default: -1) Sets the target loop iterations. (Disables Dynamic Loop Iteration Scaling)",
		"-r, --RemoveOldResults       If set removes all files from the output folder and the plot folder.",
		"-o, --OutputPath             (Default: results/) Set the output path for results.",
		"-p, --PlotOutputPath         (Default: _plots/) Sets the output path for plots.",
		"--OnlyPlot                   Plots the results in the output path.",
		"--OnlyAnalysis               Analysis the results in the output path.",
		"-a, --BenchmarksToAnalyse    The names of the benchmarks to analyse.",
		"--Verbose                    Enables debug information.",
		"-z, --ZipResults             Zips the CSV results and plots into a single zip file.",
		"--help                       Display this help screen.",
		"--version                    Display version information."
	};

	[OneTimeSetUp]
	public void Setup() {
		Directory.CreateDirectory("TestData");
		Directory.CreateDirectory("TestData/Test");
		using (var addWriter = new StreamWriter("TestData/AddSet.csv")) {
			using var addCsv = new CsvWriter(addWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
				{ Delimiter = ";" });

			addCsv.WriteRecords(_addSet);
		}

		using (var subtractWriter = new StreamWriter("TestData/SubtractSet.csv")) {
			using var subtractCsv = new CsvWriter(subtractWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
				{ Delimiter = ";" });
			subtractCsv.WriteRecords(_subtractSet);
		}

		using (var subtractWriter = new StreamWriter("TestData/Test/SubtractSet2.csv")) {
			using var subtractCsv = new CsvWriter(subtractWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
				{ Delimiter = ";" });
			subtractCsv.WriteRecords(_subtractSet);
		}
	}

	[OneTimeTearDown]
	public void OneTimeTearDown() {
		Directory.Delete("TestData", true);
	}


	[Test]
	[Order(0)]
	public void TestParse01() {
		string[] args = { "--help" };
		using var sw = new StringWriter();
		Console.SetOut(sw);

		CsharpRAPLCLI.Parse(args, 1000);

		string result = sw.ToString().Trim();
		//We skip the three first lines since they contain information about the test runner and not the test results
		string[] split = result.Split("\n").Skip(3).Select(s => s.Trim()).Where(s => s != string.Empty).ToArray();

		Assert.AreEqual(HelpText, split);


		var standardOutput = new StreamWriter(Console.OpenStandardOutput());
		standardOutput.AutoFlush = true;
		Console.SetOut(standardOutput);
	}

	[Test]
	[Order(1)]
	public void TestParse02() {
		string[] args = { "--Help" };
		using var sw = new StringWriter();
		Console.SetOut(sw);

		CsharpRAPLCLI.Parse(args, 1000);

		string result = sw.ToString().Trim();
		//We skip the three first lines since they contain information about the test runner and not the test results
		string[] split = result.Split("\n").Skip(3).Select(s => s.Trim()).Where(s => s != string.Empty).ToArray();

		Assert.AreEqual(HelpText, split);

		var standardOutput = new StreamWriter(Console.OpenStandardOutput());
		standardOutput.AutoFlush = true;
		Console.SetOut(standardOutput);
	}


	[Test]
	public void TestParse03() {
		string[] args = { "-l" };
		var exception = Assert.Throws<NotSupportedException>(() => CsharpRAPLCLI.Parse(args, 1000));
		Assert.NotNull(exception);
		Assert.AreEqual("Option 'l, LoopIterations' has no value.", exception?.Message);
	}

	[Test]
	public void TestParse04() {
		string[] args = { "-l", " 1000" };
		Options options = CsharpRAPLCLI.Parse(args, 1000);

		Assert.False(options.UseLoopIterationScaling);
		Assert.AreEqual(1000, options.LoopIterations);
	}

	[Test]
	public void TestParse05() {
		string[] args = { "--LoopIterations=1000" };
		Options options = CsharpRAPLCLI.Parse(args, 1000);

		Assert.False(options.UseLoopIterationScaling);
		Assert.AreEqual(1000, options.LoopIterations);
	}

	[Test]
	public void TestParse06() {
		string[] args = { "-i", " 1000" };
		Options options = CsharpRAPLCLI.Parse(args, 1000);

		Assert.False(options.UseIterationCalculation);
		Assert.AreEqual(1000, options.Iterations);
	}

	[Test]
	public void TestParse07() {
		string[] args = { "--Iterations=1000" };
		Options options = CsharpRAPLCLI.Parse(args, 1000);

		Assert.False(options.UseIterationCalculation);
		Assert.AreEqual(1000, options.Iterations);
	}

	[Test]
	public void TestParse08() {
		string[] args = { "-o", "Data" };
		Options options = CsharpRAPLCLI.Parse(args, 1000);

		Assert.AreEqual("Data/", options.OutputPath);
		Assert.AreEqual("_plots/", options.PlotOutputPath);
	}

	[Test]
	public void TestParse09() {
		string[] args = { "-p", "Plots" };
		Options options = CsharpRAPLCLI.Parse(args, 1000);

		Assert.AreEqual("results/", options.OutputPath);
		Assert.AreEqual("Plots/", options.PlotOutputPath);
	}

	[Test]
	public void TestParse10() {
		string[] args = { "-o", "Data", "-p", "Plots" };
		Options options = CsharpRAPLCLI.Parse(args, 1000);

		Assert.AreEqual("Data/", options.OutputPath);
		Assert.AreEqual("Plots/", options.PlotOutputPath);
	}

	[Test]
	public void TestParse11() {
		string[] args = { "-o", "Data\\Test", "-p", "Plots\\Test" };
		Options options = CsharpRAPLCLI.Parse(args, 1000);

		Assert.AreEqual("Data/Test/", options.OutputPath);
		Assert.AreEqual("Plots/Test/", options.PlotOutputPath);
	}

	[Test]
	public void TestParse12() {
		Options options = CsharpRAPLCLI.Parse(Array.Empty<string>(), 1000);
		Assert.False(options.SkipPlotGroups);
	}

	[Test]
	public void TestParse13() {
		string[] args = { "-g" };
		Options options = CsharpRAPLCLI.Parse(args, 1000);
		Assert.True(options.SkipPlotGroups);
	}

	[Test]
	public void TestParse14() {
		Directory.CreateDirectory("testFolder");
		Directory.CreateDirectory("testPlots");
		string[] args = { "-o", "testFolder", "-p", "testPlots", "-r" };
		CsharpRAPLCLI.Parse(args, 1000);

		Assert.False(Directory.Exists("testFolder"));
		Assert.False(Directory.Exists("testPlots"));
	}

	[Test]
	public void TestParse15() {
		Options options = CsharpRAPLCLI.Parse(Array.Empty<string>(), 1000);

		Assert.False(options.OnlyPlot);
	}

	[Test]
	public void TestParse16() {
		var hasPlotted = false;
		CsharpRAPLCLI.SetPlottingCallback(_ => hasPlotted = true);
		string[] args = { "--OnlyPlot" };
		Options options = CsharpRAPLCLI.Parse(args, 1000);

		Assert.True(options.OnlyPlot);
		Assert.True(hasPlotted);
	}
	
	[Test]
	public void TestSetAnalysis01() {
		string[] args = { "--OnlyAnalysis" };
		var exception = Assert.Throws<NotSupportedException>(() => CsharpRAPLCLI.Parse(args));
		Assert.NotNull(exception);
		Assert.AreEqual("You have to pass at least two benchmarks to analyse.", exception?.Message);
	}

	[Test]
	public void TestSetAnalysis02() {
		string[] args =
			{ "-o", "TestData", "--OnlyAnalysis", "-a", "AddSet.csv", "SubtractSet.csv", "SubtractSet2.csv" };
		var exception = Assert.Throws<NotSupportedException>(() => CsharpRAPLCLI.Parse(args));
		Assert.NotNull(exception);
		Assert.AreEqual("You need to pass an even number of benchmarks to analyse.", exception?.Message);
	}


	[Test]
	public void TestSetAnalysis03() {
		string[] args = { "-o", "TestData", "--OnlyAnalysis", "-a", "AddSet.csv", "SubtractSet.csv" };
		var analysed = false;
		CsharpRAPLCLI.SetAnalysisCallback(_ => analysed = true);
		CsharpRAPLCLI.Parse(args);
		Assert.True(analysed);
	}

	[Test]
	public void TestSetAnalysis04() {
		string[] args = { "-o", "TestData", "--OnlyAnalysis", "-a", "Test/", "SubtractSet.csv" };
		var analysed = false;
		CsharpRAPLCLI.SetAnalysisCallback(_ => analysed = true);
		CsharpRAPLCLI.Parse(args);
		Assert.True(analysed);
	}

	[Test]
	[Order(2)]
	public void TestSetAnalysis05() {
		using var sw = new StringWriter();
		Console.SetOut(sw);
		string[] args = { "-o", "TestData", "--OnlyAnalysis", "-a", "Test/", "SubtractSet.csv" };
		CsharpRAPLCLI.Parse(args);

		string result = sw.ToString().Trim();
		//We skip the three first lines since they contain information about the test runner and not the test results
		string[] split = result.Split("\n").Select(s => s.Trim()).Where(s => s != string.Empty).ToArray();

		var expected = new[] {
			"Results are mutually ensured!",
			"Is the null hypothesis true? I.e. is the opposite of what the key implies true?",
			"SubtractSet2 significantly different from SubtractSet - Time:0.9999999999999998",
			"SubtractSet2 significantly different from SubtractSet - Package:0.9999999999999998",
			"SubtractSet2 significantly different from SubtractSet - DRAM:0.9999999999999998",
			"Is the alternate hypothesis true? I.e. is what the key implies true?",
			"SubtractSet2 significantly different from SubtractSet - Time:2.220446049250313E-16",
			"SubtractSet2 significantly different from SubtractSet - Package:2.220446049250313E-16",
			"SubtractSet2 significantly different from SubtractSet - DRAM:2.220446049250313E-16"
		};
		Assert.AreEqual(expected, split);

		var standardOutput = new StreamWriter(Console.OpenStandardOutput());
		standardOutput.AutoFlush = true;
		Console.SetOut(standardOutput);
	}
}