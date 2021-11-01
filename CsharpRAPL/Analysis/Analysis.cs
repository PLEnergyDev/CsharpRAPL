using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Statistics.Testing;
using CsharpRAPL.Benchmarking;
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
		if (!firstBenchmark.HasRun || !secondBenchmark.HasRun) {
			throw new NotSupportedException(
				"It's not supported to analyse results before the benchmarks have run. Use Analysis class instead if you have csv files.");
		}

		_firstDataset = new DataSet(firstBenchmark.Name, firstBenchmark.GetResults());
		_secondDataset = new DataSet(secondBenchmark.Name, secondBenchmark.GetResults());
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
		PValueData firstDataSet = new(_firstDataset.Name,
			_firstDataset.Data.Select(data => data.ElapsedTime).ToArray(),
			_firstDataset.Data.Select(data => data.PackagePower).ToArray(),
			_firstDataset.Data.Select(data => data.DramPower).ToArray());
		PValueData secondDataSet = new(_secondDataset.Name,
			_secondDataset.Data.Select(data => data.ElapsedTime).ToArray(),
			_secondDataset.Data.Select(data => data.PackagePower).ToArray(),
			_secondDataset.Data.Select(data => data.DramPower).ToArray());

		// Test if first is significantly different from the second
		var timeTTest = new TwoSampleTTest(firstDataSet.Times, secondDataSet.Times);
		var pkgTTest = new TwoSampleTTest(firstDataSet.Package, secondDataSet.Package);
		var dramTTest = new TwoSampleTTest(firstDataSet.Dram, secondDataSet.Dram);

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
		if (!second.isValid) {
			return second;
		}

		return (true, "");
	}

	public (bool isValid, string message) EnsureResultsMutually() {
		List<string> first = _firstDataset.Data.Select(result => result.Result).Distinct().ToList();
		List<string> second = _secondDataset.Data.Select(result => result.Result).Distinct().ToList();

		(bool isValid, string message) firstEnsure = _firstDataset.EnsureResults();
		if (!firstEnsure.isValid) {
			return firstEnsure;
		}

		(bool isValid, string message) secondEnsure = _secondDataset.EnsureResults();
		if (!secondEnsure.isValid) {
			return secondEnsure;
		}

		if (first.Count != second.Count) {
			return (false,
				$"The two data sets have an unequal number of results. {_firstDataset.Name}: [{string.Join(", ", first)}], {_secondDataset.Name}: [{string.Join(", ", second)}]");
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
		PlotAnalysis(BenchmarkResultType.PackagePower);
		PlotAnalysis(BenchmarkResultType.DramPower);
		PlotAnalysis(BenchmarkResultType.Temperature);
	}

	public void PlotAnalysis(BenchmarkResultType resultType) {
		BenchmarkPlot.PlotResults(resultType, new[] { _firstDataset, _secondDataset }, new PlotOptions {
			Name = $"{_firstDataset.Name}-{_secondDataset.Name}"
		});
	}

	public static Dictionary<string, double> CalculatePValueForGroup(List<IBenchmark> dataSets) {
		var groupToPValue = new Dictionary<string, double>();
		for (int i = 0; i < dataSets.Count; i++) {
			for (int j = i + 1; j < dataSets.Count; j++) {
				var analysis = new Analysis(dataSets[i], dataSets[j]);
				foreach ((string message, double value) in analysis.CalculatePValue()) {
					groupToPValue.Add(message, value);
				}
			}
		}

		return groupToPValue;
	}
}