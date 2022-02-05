using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Accord.Statistics;
using Accord.Statistics.Distributions.Univariate;
using CsharpRAPL.Analysis;
using CsharpRAPL.CommandLine;
using CsharpRAPL.Data;
using CsharpRAPL.Measuring;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsharpRAPL.Benchmarking;

public class Benchmark<T> : IBenchmark {
	public ulong Iterations { get; private set; }
	public string Name { get; }
	public string? Group { get; }
	public int Order { get; }
	public bool HasRun { get; private set; }
	public double ElapsedTime { get; private set; }

	private const int MaxExecutionTime = 2700; //In seconds
	private const int TargetLoopIterationTime = 250; //In milliseconds

	// Prints everything to a null stream similar to /dev/null
	private readonly TextWriter _benchmarkOutputStream = new StreamWriter(Stream.Null);
	private readonly TextWriter _stdout;
	private readonly Func<T> _benchmark;
	private readonly List<BenchmarkResult> _rawResults = new();
	private readonly List<BenchmarkResult> _normalizedResults = new();
	private readonly FieldInfo _loopIterationsFieldInfo;

	//TODO: Check if using a interface here hurts measurements
	private IMeasureApi _measureApiApi;
	private string? _normalizedReturnValue;


	public Benchmark(string name, ulong iterations, Func<T> benchmark, bool silenceBenchmarkOutput = true,
		string? group = null, int order = 0) {
		Name = name;
		Group = group;
		Iterations = iterations;
		Order = order;
		_measureApiApi = null!;

		_benchmark = benchmark;
		_stdout = Console.Out;

		Debug.Assert(_benchmark.Method.DeclaringType != null, "_benchmark.Method.DeclaringType != null");
		_loopIterationsFieldInfo =
			_benchmark.Method.DeclaringType.GetField("LoopIterations", BindingFlags.Public | BindingFlags.Static) ??
			throw new InvalidOperationException(
				$"Your class '{_benchmark.Method.DeclaringType.Name}' must have the field '{name}'.");

		if (!silenceBenchmarkOutput) {
			_benchmarkOutputStream = _stdout;
		}
	}

	private void Start() {
		_measureApiApi.Start();
	}

	private void End(T benchmarkOutput) {
		_measureApiApi.End();

		//Result only valid if all results are valid
		//Only then is the result added and duration is incremented
		if (!_measureApiApi.IsValid()) {
			return;
		}

		ulong loopIterations = GetLoopIterations();

		BenchmarkResult result = _measureApiApi.GetResults(loopIterations) with {
			BenchmarkReturnValue = benchmarkOutput?.ToString() ?? string.Empty
		};

		BenchmarkResult normalizedResult = _measureApiApi.GetNormalizedResults(loopIterations) with {
			BenchmarkReturnValue = _normalizedReturnValue ?? string.Empty
		};
		_rawResults.Add(result);
		_normalizedResults.Add(normalizedResult);
		ElapsedTime += _rawResults[^1].ElapsedTime / 1_000;
	}

	//Performs benchmarking
	//Writes progress to stdout if there is more than one iteration
	public void Run() {
		//Sets console to write to null
		Console.SetOut(_benchmarkOutputStream);
		_measureApiApi = CsharpRAPLCLI.Options.OnlyTime ? new TimerOnly() : new RAPL();

		if (_measureApiApi is null) {
			throw new RAPLNotInitializedException();
		}

		ElapsedTime = 0;
		_rawResults.Clear();
		_normalizedResults.Clear();

		if (CsharpRAPLCLI.Options.UseIterationCalculation) {
			Iterations = IterationCalculationAll();
		}

		// Get normalized return value
		SetLoopIterations(10);
		_normalizedReturnValue = _benchmark()?.ToString() ?? string.Empty;

		for (ulong i = 0; i <= Iterations; i++) {
			if (Iterations != 1) {
				if (CsharpRAPLCLI.Options.Verbose) {
					Print(Console.Write,
						$"\r{i} of {Iterations} for {Name}. LoopIterations: {GetLoopIterations()}. " +
						$"Time target: {TargetLoopIterationTime}. UseLoopScaling: {CsharpRAPLCLI.Options.UseLoopIterationScaling} UseIterationCalculation: {CsharpRAPLCLI.Options.UseIterationCalculation}" +
						$" Last took: {(_rawResults.Count > 0 ? _rawResults[^1].ElapsedTime : 0.00):##,###}ms");
				}
				else {
					Print(Console.Write, $"\r{i} of {Iterations} for {Name}");
				}
			}


			//Actually performing benchmark and resulting IO
			Start();
			T benchmarkOutput = _benchmark();
			End(benchmarkOutput);


			if (CsharpRAPLCLI.Options.UseLoopIterationScaling &&
			    _rawResults[^1].ElapsedTime < TargetLoopIterationTime) {
				ulong currentValue = GetLoopIterations();

				switch (currentValue) {
					case ulong.MaxValue:
						break;
					case >= ulong.MaxValue / 2:
						SetLoopIterations(ulong.MaxValue);
						_rawResults.Clear();
						_normalizedResults.Clear();
						i = 0;
						continue;
					default:
						SetLoopIterations(currentValue + currentValue);
						_rawResults.Clear();
						_normalizedResults.Clear();
						i = 0;
						continue;
				}
			}

			if (ElapsedTime < MaxExecutionTime) {
				if (CsharpRAPLCLI.Options.UseIterationCalculation) {
					Iterations = IterationCalculationAll();
				}

				continue;
			}

			Print(Console.WriteLine, $"\rEnding for {Name} benchmark due to time constraints");
			break;
		}

		SaveResults();
		HasRun = true;
		SetLoopIterations(CsharpRAPLCLI.Options.LoopIterations);


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

	/// <summary>
	/// Saves Normalized Results to a file.
	/// </summary>
	private void SaveResults() {
		DateTime dateTime = DateTime.Now;
		string time = $"{dateTime.ToString("s").Replace(":", "-")}-{dateTime.Millisecond}";
		string outputPath = Group != null
			? $"{CsharpRAPLCLI.Options.OutputPath}/{Group}/{Name}"
			: $"{CsharpRAPLCLI.Options.OutputPath}/{Name}";

		Directory.CreateDirectory(outputPath);
		using var writer = new StreamWriter($"{outputPath}/{Name}-{time}.csv");
		using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
			{ Delimiter = ";" });
		csv.WriteRecords(GetResults());
	}

	//TODO: Figure out if we want to still ignore first
	public List<BenchmarkResult> GetResults(bool ignoreFirst = true) {
		return ignoreFirst
			? new List<BenchmarkResult>(_normalizedResults.Skip(1))
			: new List<BenchmarkResult>(_normalizedResults);
	}

	//TODO: Figure out if we want to still ignore first
	public List<BenchmarkResult> GetRawResults(bool ignoreFirst = true) {
		return ignoreFirst
			? new List<BenchmarkResult>(_rawResults.Skip(1))
			: new List<BenchmarkResult>(_rawResults);
	}

	/// <summary>
	/// Finds the highest iteration calculation of all three measurement types
	/// Should be used when general results are wanted instead of specifically for DRAM, Package or Elapsed Time
	/// </summary>
	/// <param name="confidence">The confidence (from 0 to 1) that we want</param>
	/// <returns>The amount of samples needed for the given confidence for all measurements</returns>
	private ulong IterationCalculationAll(double confidence = 0.95) {
		ulong timeIteration = IterationCalculation(confidence, BenchmarkResultType.ElapsedTime);
		if (CsharpRAPLCLI.Options.OnlyTime) {
			return timeIteration;
		}

		ulong dramIteration = IterationCalculation(confidence, BenchmarkResultType.DRAMEnergy);
		ulong packageIteration = IterationCalculation(confidence);
		return Math.Max(dramIteration, Math.Max(timeIteration, packageIteration));
	}

	/// <summary>
	/// Uses the formula described in the report in formula 4.2 to calculate sample size
	/// </summary>
	/// <param name="confidence">The confidence (from 0 to 1) that we want</param>
	/// <param name="resultType">The result type we should look for</param>
	/// <returns>The amount of samples needed for the given confidence</returns>
	private ulong IterationCalculation(double confidence = 0.95,
		BenchmarkResultType resultType = BenchmarkResultType.PackageEnergy) {
		// If we have less than 10 results, we return 10 so we can get a sample to calculate from
		if (_rawResults.Count < 10) {
			return 10;
		}

		// The alpha value is 1 - the confidence we want (Also known as p-value)
		double alpha = 1 - confidence;

		// Calculate the mean and standard deviation of the current sample
		// We start by adding all the values to a double-array as this is used to calculate mean and standard
		// deviation
		var values = new double[_rawResults.Count];
		for (var i = 0; i < _rawResults.Count; i++) {
			values[i] = resultType switch {
				BenchmarkResultType.Temperature => _rawResults[i].Temperature,
				BenchmarkResultType.DRAMEnergy => _rawResults[i].DRAMEnergy,
				BenchmarkResultType.ElapsedTime => _rawResults[i].ElapsedTime,
				BenchmarkResultType.PackageEnergy => _rawResults[i].PackageEnergy,
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
		return (ulong)Math.Ceiling(Math.Pow(nd.ZScore(nd.GetRange(alpha / 2).Max) * stdDeviation / (0.005 * mean),
			2));
	}

	private ulong GetLoopIterations() {
		return (ulong)(_loopIterationsFieldInfo.GetValue(null) ??
		               throw new InvalidOperationException(
			               $"Your class '{_benchmark.Method.DeclaringType?.Name}' must have the field 'LoopIterations'."));
	}

	private void SetLoopIterations(ulong value) {
		_loopIterationsFieldInfo.SetValue(null, value);
	}
}