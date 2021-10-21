using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Accord.Statistics;
using Accord.Statistics.Distributions.Univariate;
using CsharpRAPL.Analysis;
using CsharpRAPL.CommandLine;
using CsharpRAPL.Data;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsharpRAPL.Benchmarking {
	public class Benchmark<T> : IBenchmark {
		public int Iterations { get; private set; }
		public string Name { get; }
		public string? Group { get; }
		public int Order { get; }
		public bool HasRun { get; private set; }

		private const int MaxExecutionTime = 2700; //In seconds

		// Prints everything to a null stream similar to /dev/null
		private readonly TextWriter _benchmarkOutputStream = new StreamWriter(Stream.Null);
		private readonly TextWriter _stdout;
		private readonly string _outputFilePath;
		private readonly Func<T> _benchmark;
		private readonly List<BenchmarkResult> _resultBuffer = new();

		private RAPL? _rapl;
		private double _elapsedTime;

		public Benchmark(string name, int iterations, Func<T> benchmark, bool silenceBenchmarkOutput = true,
			string? group = null, int order = 0) {
			Name = name;
			Group = group;
			Iterations = iterations;
			Order = order;

			_benchmark = benchmark;
			_stdout = Console.Out;

			if (!silenceBenchmarkOutput)
				_benchmarkOutputStream = _stdout;

			DateTime dateTime = DateTime.Now;
			var time = $"{dateTime.ToString("s").Replace(":", "-")}-{dateTime.Millisecond}";
			string outputPath = Group != null
				? $"{CsharpRAPLCLI.Options.OutputPath}/{group}/{name}"
				: $"{CsharpRAPLCLI.Options.OutputPath}/{name}";

			Directory.CreateDirectory(outputPath);
			_outputFilePath = $"{outputPath}/{name}-{time}.csv";
		}

		private void Start() {
			if (_rapl is null)
				throw new NotSupportedException("Rapl has not been initialized");

			_rapl.Start();
		}

		private void End(T benchmarkOutput) {
			if (_rapl is null)
				throw new NotSupportedException("Rapl has not been initialized");

			_rapl.End();

			//Result only valid if all results are valid
			//Only then is the result added and duration is incremented
			if (!_rapl.IsValid()) return;
			BenchmarkResult result = _rapl.GetResults() with { Result = benchmarkOutput?.ToString() ?? string.Empty };
			_resultBuffer.Add(result);
			_elapsedTime += _resultBuffer[^1].ElapsedTime / 1_000;
		}

		//Performs benchmarking
		//Writes progress to stdout if there is more than one iteration
		public void Run() {
			//Sets console to write to null
			Console.SetOut(_benchmarkOutputStream);

			_rapl = new RAPL();

			_elapsedTime = 0;
			_resultBuffer.Clear();
			if (CsharpRAPLCLI.Options.UseIterationCalculation)
				Iterations = IterationCalculation();
			for (var i = 0; i <= Iterations; i++) {
				if (Iterations != 1)
					Print(Console.Write, $"\r{i} of {Iterations} for {Name}");

				//Actually performing benchmark and resulting IO
				Start();
				T benchmarkOutput = _benchmark();
				End(benchmarkOutput);
				
				if (CsharpRAPLCLI.Options.UseIterationCalculation)
					Iterations = IterationCalculation();
				
				if (!(_elapsedTime >= MaxExecutionTime)) continue;

				Print(Console.WriteLine, $"\rEnding for {Name} benchmark due to time constraints");
				break;
			}

			SaveResults();
			HasRun = true;

			//Resets console output
			Console.SetOut(_stdout);
		}


		/// Used to print to standard out -- Everything printed outside this method will not be shown
		private void Print(Action<string> printAction, string value = "") {
			Console.SetOut(_stdout);
			printAction(value);
			Console.Out.Flush();
			Console.SetOut(_benchmarkOutputStream);
		}

		//Saves result to temporary file
		//This is overwritten each time SaveResults is run
		private void SaveResults() {
			using var writer = new StreamWriter(_outputFilePath);
			using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
				{ Delimiter = ";" });
			csv.WriteRecords(GetResults());
		}

		public List<BenchmarkResult> GetResults(bool ignoreFirst = true) {
			return ignoreFirst
				? new List<BenchmarkResult>(_resultBuffer.Skip(1))
				: new List<BenchmarkResult>(_resultBuffer);
		}

		/// <summary>
		/// Uses the formula described in the report in formula 4.2 to calculate sample size
		/// </summary>
		/// <param name="confidence">The confidence (from 0 to 1) that we want</param>
		/// <param name="resultType">The result type we should look for</param>
		/// <returns>The amount of samples needed for the given confidence</returns>
		private int IterationCalculation(double confidence = 0.95,
			BenchmarkResultType resultType = BenchmarkResultType.PackagePower) {
			// If we have less than 2 results, we return 2 so we can get a sample to calculate from
			if (_resultBuffer.Count < 2) {
				return 2;
			}

			// The alpha value is 1 - the confidence we want (Also known as p-value)
			double alpha = 1 - confidence;

			// Calculate the mean and standard deviation of the current sample
			// We start by adding all the values to a double-array as this is used to calculate mean and standard
			// deviation
			double[] values = new double[_resultBuffer.Count];
			for (int i = 0; i < _resultBuffer.Count; i++) {
				values[i] = resultType switch {
					BenchmarkResultType.Temperature => _resultBuffer[i].Temperature,
					BenchmarkResultType.DramPower => _resultBuffer[i].DramPower,
					BenchmarkResultType.ElapsedTime => _resultBuffer[i].ElapsedTime,
					BenchmarkResultType.PackagePower => _resultBuffer[i].PackagePower,
					_ => throw new ArgumentOutOfRangeException(nameof(resultType),
						$"The benchmark result, {resultType}, has not been implemented in IterationCalculation")
				};
			}

			// Calculate the mean
			double mean = values.Average();

			// Calculate the standard deviation
			double stdDeviation = values.StandardDeviation(mean);

			// Create a normal distribution to calculate the z-value
			NormalDistribution nd = new(mean, stdDeviation);

			// Calculate the sample size needed for the confidence
			// 0.005 is the relative margin of error we want (0.5%)
			// We want the ZScore at the alpha/2 number, so we get the range at that point, take the highest and 
			// calculate from there
			return (int)Math.Ceiling(Math.Pow((nd.ZScore(nd.GetRange(alpha / 2).Max) * stdDeviation) / (0.005 * mean),
				2));
		}
	}
}