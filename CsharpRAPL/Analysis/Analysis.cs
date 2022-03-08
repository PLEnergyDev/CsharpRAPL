using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Statistics.Testing;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.CommandLine;
using CsharpRAPL.Data;
using CsharpRAPL.Plotting;

namespace CsharpRAPL.Analysis;

public class Analysis {
	private readonly DataSet _firstDataset;
	private readonly DataSet _secondDataset;

	public Analysis(string pathToFirstData, string pathToSecondData) {
		_firstDataset = new DataSet(pathToFirstData);
		_secondDataset = new DataSet(pathToSecondData);
	}

	public Analysis(IBenchmark firstBenchmark, IBenchmark secondBenchmark) {
		if (!firstBenchmark.BenchmarkInfo.HasRun || !secondBenchmark.BenchmarkInfo.HasRun) {
			throw new NotSupportedException(
				"It's not supported to analyse results before the benchmarks have run. Use Analysis class instead if you have csv files.");
		}

		_firstDataset = new DataSet(firstBenchmark.BenchmarkInfo.Name, firstBenchmark.GetResults());
		_secondDataset = new DataSet(secondBenchmark.BenchmarkInfo.Name, secondBenchmark.GetResults());
	}

	public Analysis(string firstBenchmarkName, List<BenchmarkResult> firstBenchmarkResults,
		string secondBenchmarkName, List<BenchmarkResult> secondBenchmarkResults) {
		_firstDataset = new DataSet(firstBenchmarkName, firstBenchmarkResults);
		_secondDataset = new DataSet(secondBenchmarkName, secondBenchmarkResults);
	}

	public Analysis(DataSet firstDataset, DataSet secondDataset) {
		_firstDataset = firstDataset;
		_secondDataset = secondDataset;
	}

	public ((string Name, BenchmarkResult Data) FirstDataSet, (string Name, BenchmarkResult Data) SecondDataSet)
		GetAverage() {
		return ((_firstDataset.Name, _firstDataset.GetAverage()),
			(_secondDataset.Name, _secondDataset.GetAverage()));
	}

	public ((string Name, BenchmarkResult Data) FirstDataSet, (string Name, BenchmarkResult Data) SecondDataSet)
		GetMax() {
		return ((_firstDataset.Name, _firstDataset.GetMax()), (_secondDataset.Name, _secondDataset.GetMax()));
	}

	public ((string Name, BenchmarkResult Data) FirstDataSet, (string Name, BenchmarkResult Data) SecondDataSet)
		GetMin() {
		return ((_firstDataset.Name, _firstDataset.GetMin()), (_secondDataset.Name, _secondDataset.GetMin()));
	}

	public ((string Name, BenchmarkResult Data) FirstDataSet, (string Name, BenchmarkResult Data) SecondDataSet)
		GetMaxBy(BenchmarkResultType resultType) {
		return ((_firstDataset.Name, _firstDataset.GetMaxBy(resultType)),
			(_secondDataset.Name, _secondDataset.GetMaxBy(resultType)));
	}

	public ((string Name, BenchmarkResult Data) FirstDataSet, (string Name, BenchmarkResult Data) SecondDataSet)
		GetMinBy(BenchmarkResultType resultType) {
		return ((_firstDataset.Name, _firstDataset.GetMinBy(resultType)),
			(_secondDataset.Name, _secondDataset.GetMinBy(resultType)));
	}

	public List<(string Message, double Value)> CalculatePValue() {
		// Set up the datasets that we want to compare
		PValueData firstDataSet = new(_firstDataset);
		PValueData secondDataSet = new(_secondDataset);

		// Test if first is significantly different from the second
		var timeTTest = new TwoSampleTTest(firstDataSet.TimesValues, secondDataSet.TimesValues);
		if (CsharpRAPLCLI.Options.OnlyTime) {
			return new List<(string Message, double Value)> {
				($"{firstDataSet.Name} significantly different from {secondDataSet.Name} - Time", timeTTest.PValue)
			};
		}

		var pkgTTest = new TwoSampleTTest(firstDataSet.PackageValues, secondDataSet.PackageValues);
		var dramTTest = new TwoSampleTTest(firstDataSet.DRAMValues, secondDataSet.DRAMValues);

		// Save the P-values
		return new List<(string Message, double Value)> {
			($"{firstDataSet.Name} significantly different from {secondDataSet.Name} - Time", timeTTest.PValue),
			($"{firstDataSet.Name} significantly different from {secondDataSet.Name} - Package", pkgTTest.PValue),
			($"{firstDataSet.Name} significantly different from {secondDataSet.Name} - DRAM", dramTTest.PValue)
		};
	}

	public (bool isValid, string message) EnsureResults() {
		(bool isValid, string message) first = _firstDataset.EnsureResults();
		if (!first.isValid) {
			return first;
		}

		(bool isValid, string message) second = _secondDataset.EnsureResults();
		return !second.isValid ? second : (true, "");
	}

	public (bool isValid, string message) EnsureResultsMutually() {
		List<string> first = _firstDataset.Data.Select(result => result.BenchmarkReturnValue).Distinct().ToList();
		List<string> second = _secondDataset.Data.Select(result => result.BenchmarkReturnValue).Distinct().ToList();

		(bool isValid, string message) firstEnsure = _firstDataset.EnsureResults();
		if (!firstEnsure.isValid) {
			return firstEnsure;
		}

		(bool isValid, string message) secondEnsure = _secondDataset.EnsureResults();
		if (!secondEnsure.isValid) {
			return secondEnsure;
		}

		for (var i = 0; i < first.Count; i++) {
			if (first[i] != second[i]) {
				return (false, $"The to datasets differ in {first[i]} and {second[i]}");
			}
		}

		return (true, "");
	}

	public void PlotAnalysis() {
		PlotAnalysis(BenchmarkResultType.ElapsedTime);
		PlotAnalysis(BenchmarkResultType.PackageEnergy);
		PlotAnalysis(BenchmarkResultType.DRAMEnergy);
		PlotAnalysis(BenchmarkResultType.Temperature);
	}

	public void PlotAnalysis(BenchmarkResultType resultType) {
		BenchmarkPlot.PlotResults(resultType, new[] { _firstDataset, _secondDataset }, new PlotOptions {
			Name = $"{_firstDataset.Name}-{_secondDataset.Name}"
		});
	}


	public static Dictionary<string, double> CalculatePValueForGroup(List<IBenchmark> dataSets) {
		var groupToPValue = new Dictionary<string, double>();
		for (var i = 0; i < dataSets.Count; i++) {
			for (int j = i + 1; j < dataSets.Count; j++) {
				var analysis = new Analysis(dataSets[i], dataSets[j]);
				foreach ((string message, double value) in analysis.CalculatePValue()) {
					groupToPValue.Add(message, value);
				}
			}
		}

		return groupToPValue;
	}

	public static Dictionary<string, double> CalculatePValueForGroup(List<DataSet> dataSets) {
		var groupToPValue = new Dictionary<string, double>();
		for (var i = 0; i < dataSets.Count; i++) {
			for (int j = i + 1; j < dataSets.Count; j++) {
				var analysis = new Analysis(dataSets[i], dataSets[j]);
				foreach ((string message, double value) in analysis.CalculatePValue()) {
					groupToPValue.Add(message, value);
				}
			}
		}

		return groupToPValue;
	}

	public static Dictionary<string, double> CalculatePValueForOutput() {
		var result = new List<DataSet>();

		result.AddRange(CsharpRAPLCLI.Options.Json
			? Helpers.GetAllJsonFilesFromOutputPath().Select(data => new DataSet(data))
			: Helpers.GetAllCSVFilesFromOutputPath().Select(data => new DataSet(data)));

		return CalculatePValueForGroup(result);
	}

	public static void CheckExecutionTime(DataSet dataSet) {
		(string name, List<double> minTimeElapsed) = (dataSet.Name,
			dataSet.Data.Select(set => set.ElapsedTime).Where(tuple => tuple < 0.3).ToList());

		if (minTimeElapsed.Count == 0) {
			Console.WriteLine("No results were below 0.3 seconds");
		}
		else {
			Console.WriteLine(
				$"{name} was found to be below 0.3 seconds ({minTimeElapsed.Min()} seconds) so we might need to check if this gets compiled away");
		}
	}

	public static void CheckExecutionTime() {
		if (CsharpRAPLCLI.Options.Json) {
			foreach (string pathToDataSet in Helpers.GetAllJsonFilesFromOutputPath()) {
				CheckExecutionTime(new DataSet(pathToDataSet));
			}
		}
		else {
			foreach (string pathToDataSet in Helpers.GetAllCSVFilesFromOutputPath()) {
				CheckExecutionTime(new DataSet(pathToDataSet));
			}
		}
	}
}