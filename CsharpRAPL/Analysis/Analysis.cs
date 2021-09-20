using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Accord.Statistics.Testing;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Data;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsharpRAPL.Analysis {
	public class Analysis {
		private readonly (string Name, List<BenchmarkResult> Data) _firstDataset;
		private readonly (string Name, List<BenchmarkResult> Data) _secondDataset;

		public Analysis(string pathToFirstData, string pathToSecondData) {
			_firstDataset = (Path.GetFileNameWithoutExtension(pathToFirstData), ReadData(pathToFirstData));
			_secondDataset = (Path.GetFileNameWithoutExtension(pathToSecondData), ReadData(pathToSecondData));
		}

		public Analysis(Benchmark firstBenchmark, Benchmark secondBenchmark) {
			_firstDataset = (firstBenchmark.Name, firstBenchmark.GetResults());
			_secondDataset = (secondBenchmark.Name, secondBenchmark.GetResults());
		}

		public Analysis(string firstBenchmarkName, List<BenchmarkResult> firstBenchmarkResults,
			string secondBenchmarkName, List<BenchmarkResult> secondBenchmarkResults) {
			_firstDataset = (firstBenchmarkName, firstBenchmarkResults);
			_secondDataset = (secondBenchmarkName, secondBenchmarkResults);
		}

		private static List<BenchmarkResult> ReadData(string path) {
			using var reader = new StreamReader(path);
			using var csv = new CsvReader(reader,
				new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" });

			return csv.GetRecords<BenchmarkResult>().ToList();
		}

		public ((string Name, BenchmarkResult First), (string Name, BenchmarkResult Second)) GetAverage() {
			var first = new BenchmarkResult {
				Temperature = _firstDataset.Data.Average(result => result.Temperature),
				DramPower = _firstDataset.Data.Average(result => result.DramPower),
				ElapsedTime = _firstDataset.Data.Average(result => result.ElapsedTime),
				PackagePower = _firstDataset.Data.Average(result => result.PackagePower),
			};

			var second = new BenchmarkResult {
				Temperature = _secondDataset.Data.Average(result => result.Temperature),
				DramPower = _secondDataset.Data.Average(result => result.DramPower),
				ElapsedTime = _secondDataset.Data.Average(result => result.ElapsedTime),
				PackagePower = _secondDataset.Data.Average(result => result.PackagePower),
			};


			return ((_firstDataset.Name, first), (_secondDataset.Name, second));
		}

		public ((string Name, BenchmarkResult First), (string Name, BenchmarkResult Second)) GetMax() {
			var first = new BenchmarkResult {
				Temperature = _firstDataset.Data.Max(result => result.Temperature),
				DramPower = _firstDataset.Data.Max(result => result.DramPower),
				ElapsedTime = _firstDataset.Data.Max(result => result.ElapsedTime),
				PackagePower = _firstDataset.Data.Max(result => result.PackagePower),
			};

			var second = new BenchmarkResult {
				Temperature = _secondDataset.Data.Max(result => result.Temperature),
				DramPower = _secondDataset.Data.Max(result => result.DramPower),
				ElapsedTime = _secondDataset.Data.Max(result => result.ElapsedTime),
				PackagePower = _secondDataset.Data.Max(result => result.PackagePower),
			};


			return ((_firstDataset.Name, first), (_secondDataset.Name, second));
		}

		public ((string Name, BenchmarkResult First), (string Name, BenchmarkResult Second)) GetMin() {
			var first = new BenchmarkResult {
				Temperature = _firstDataset.Data.Min(result => result.Temperature),
				DramPower = _firstDataset.Data.Min(result => result.DramPower),
				ElapsedTime = _firstDataset.Data.Min(result => result.ElapsedTime),
				PackagePower = _firstDataset.Data.Min(result => result.PackagePower),
			};

			var second = new BenchmarkResult {
				Temperature = _secondDataset.Data.Min(result => result.Temperature),
				DramPower = _secondDataset.Data.Min(result => result.DramPower),
				ElapsedTime = _secondDataset.Data.Min(result => result.ElapsedTime),
				PackagePower = _secondDataset.Data.Min(result => result.PackagePower),
			};


			return ((_firstDataset.Name, first), (_secondDataset.Name, second));
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
	}
}