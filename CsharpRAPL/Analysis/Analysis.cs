using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Accord.Statistics.Testing;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsharpRAPL.Analysis {
	public class Analysis {
		private readonly List<BenchmarkResult> _firstDataset;
		private readonly List<BenchmarkResult> _secondDataset;

		public Analysis(string pathToFirstData, string pathToSecondData) {
			_firstDataset = ReadData(pathToFirstData);
			_secondDataset = ReadData(pathToSecondData);
		}

		public Analysis(Benchmark firstBenchmark, Benchmark secondBenchmark) {
			_firstDataset = firstBenchmark.GetResults();
			_secondDataset = secondBenchmark.GetResults();
		}

		private static List<BenchmarkResult> ReadData(string path) {
			using var reader = new StreamReader(path);
			using var csv = new CsvReader(reader,
				new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" });

			return csv.GetRecords<BenchmarkResult>().ToList();
		}

		public Dictionary<string, double> CalculatePValue() {
			double[] timesOne = _firstDataset.Select(data => data.ElapsedTime).ToArray();
			double[] timesTwo = _secondDataset.Select(data => data.ElapsedTime).ToArray();

			double[] packageOne = _firstDataset.Select(data => data.PackagePower).ToArray();
			double[] packageTwo = _secondDataset.Select(data => data.PackagePower).ToArray();

			double[] dramOne = _firstDataset.Select(data => data.DramPower).ToArray();
			double[] dramTwo = _secondDataset.Select(data => data.DramPower).ToArray();


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
				{ "firstLowerThanSecondTime", timeFirstTTest.PValue },
				{ "secondLowerThanFirstTime", timeSecondTTest.PValue },
				{ "firstLowerThanSecondPkg", pkgFirstTTest.PValue },
				{ "secondLowerThanFirstPkg", pkgSecondTTest.PValue },
				{ "firstLowerThanSecondDram", dramFirstTTest.PValue },
				{ "secondLowerThanFirstDram", dramSecondTTest.PValue }
			};

			return results;
		}
	}
}