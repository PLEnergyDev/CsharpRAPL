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

	public Dictionary<string, double> CalculatePValue() {
		PValueData firstDataSet = new(_firstDataset.Name,
			_firstDataset.Data.Select(data => data.ElapsedTime).ToArray(),
			_firstDataset.Data.Select(data => data.PackagePower).ToArray(),
			_firstDataset.Data.Select(data => data.DramPower).ToArray());
		PValueData secondDataSet = new(_secondDataset.Name,
			_secondDataset.Data.Select(data => data.ElapsedTime).ToArray(),
			_secondDataset.Data.Select(data => data.PackagePower).ToArray(),
			_secondDataset.Data.Select(data => data.DramPower).ToArray());

		// Test if first is significantly lower than second
		var timeFirstTTest = new TTest(firstDataSet.Times, secondDataSet.TimeMean,
			OneSampleHypothesis.ValueIsSmallerThanHypothesis);
		var pkgFirstTTest = new TTest(firstDataSet.Package, secondDataSet.PackageMean,
			OneSampleHypothesis.ValueIsSmallerThanHypothesis);
		var dramFirstTTest = new TTest(firstDataSet.Dram, secondDataSet.DramMean,
			OneSampleHypothesis.ValueIsSmallerThanHypothesis);

		// Test if second is significantly lower than first
		var timeSecondTTest = new TTest(secondDataSet.Times, firstDataSet.TimeMean,
			OneSampleHypothesis.ValueIsSmallerThanHypothesis);
		var pkgSecondTTest = new TTest(secondDataSet.Package, firstDataSet.PackageMean,
			OneSampleHypothesis.ValueIsSmallerThanHypothesis);
		var dramSecondTTest = new TTest(secondDataSet.Dram, firstDataSet.DramMean,
			OneSampleHypothesis.ValueIsSmallerThanHypothesis);

		var results = new Dictionary<string, double> {
			{ $"{firstDataSet.Name} lower than {secondDataSet.Name} Time", timeFirstTTest.PValue },
			{ $"{secondDataSet.Name} lower than {firstDataSet.Name} Time", timeSecondTTest.PValue },
			{ $"{firstDataSet.Name} lower than {secondDataSet.Name} Package", pkgFirstTTest.PValue },
			{ $"{secondDataSet.Name} lower than {firstDataSet.Name} Package", pkgSecondTTest.PValue },
			{ $"{firstDataSet.Name} lower than {secondDataSet.Name} Dram", dramFirstTTest.PValue },
			{ $"{secondDataSet.Name} lower than {firstDataSet.Name} Dram", dramSecondTTest.PValue }
		};

		return results;
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
		BenchmarkPlot.PlotResults(resultType, $"{_firstDataset.Name}-{_secondDataset.Name}",
			_firstDataset, _secondDataset);
	}
}