using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Statistics.Testing;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Data;

namespace CsharpRAPL.Analysis {
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

		public Dictionary<string, double> CalculatePValue() {
			double[] timesOne = _firstDataset.Data.Select(data => data.ElapsedTime).ToArray();
			double[] timesTwo = _secondDataset.Data.Select(data => data.ElapsedTime).ToArray();

			double[] packageOne = _firstDataset.Data.Select(data => data.PackagePower).ToArray();
			double[] packageTwo = _secondDataset.Data.Select(data => data.PackagePower).ToArray();

			double[] dramOne = _firstDataset.Data.Select(data => data.DramPower).ToArray();
			double[] dramTwo = _secondDataset.Data.Select(data => data.DramPower).ToArray();


			double firstTimeMean = timesOne.Average();
			double firstPkgMean = packageOne.Average();
			double firstDramMean = dramOne.Average();

			double secondTimeMean = timesTwo.Average();
			double secondPkgMean = packageTwo.Average();
			double secondDramMean = dramTwo.Average();

			// Test if first is significantly lower than second
			var timeFirstTTest = new TTest(timesOne, secondTimeMean, OneSampleHypothesis.ValueIsSmallerThanHypothesis);
			var pkgFirstTTest = new TTest(packageOne, secondPkgMean, OneSampleHypothesis.ValueIsSmallerThanHypothesis);
			var dramFirstTTest = new TTest(dramOne, secondDramMean, OneSampleHypothesis.ValueIsSmallerThanHypothesis);

			// Test if second is significantly lower than first
			var timeSecondTTest = new TTest(timesTwo, firstTimeMean, OneSampleHypothesis.ValueIsSmallerThanHypothesis);
			var pkgSecondTTest = new TTest(packageTwo, firstPkgMean, OneSampleHypothesis.ValueIsSmallerThanHypothesis);
			var dramSecondTTest = new TTest(dramTwo, firstDramMean, OneSampleHypothesis.ValueIsSmallerThanHypothesis);

			var results = new Dictionary<string, double> {
				{ $"{_firstDataset.Name} lower than {_secondDataset.Name} Time", timeFirstTTest.PValue },
				{ $"{_secondDataset.Name} lower than {_firstDataset.Name} Time", timeSecondTTest.PValue },
				{ $"{_firstDataset.Name} lower than {_secondDataset.Name} Package", pkgFirstTTest.PValue },
				{ $"{_secondDataset.Name} lower than {_firstDataset.Name} Package", pkgSecondTTest.PValue },
				{ $"{_firstDataset.Name} lower than {_secondDataset.Name} Dram", dramFirstTTest.PValue },
				{ $"{_secondDataset.Name} lower than {_firstDataset.Name} Dram", dramSecondTTest.PValue }
			};

			return results;
		}

		public (bool isValid, string message) EnsureResults() {
			(bool isValid, string message) first = _firstDataset.EnsureResults();
			if (!first.isValid)
				return first;
			(bool isValid, string message) second = _secondDataset.EnsureResults();
			if (!second.isValid)
				return second;

			return (true, "");
		}

		public void PlotAnalysis() {
			PlotAnalysis(BenchmarkResultType.ElapsedTime);
			PlotAnalysis(BenchmarkResultType.PackagePower);
			PlotAnalysis(BenchmarkResultType.DramPower);
			PlotAnalysis(BenchmarkResultType.Temperature);
		}

		public void PlotAnalysis(BenchmarkResultType resultType) {
			BenchmarkPlot.PlotResults(resultType, _firstDataset, _secondDataset);
		}
	}
}