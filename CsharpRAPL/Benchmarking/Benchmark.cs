using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Accord.Statistics;
using Accord.Statistics.Distributions.Univariate;
using CsharpRAPL.Analysis;
using CsharpRAPL.Benchmarking.Lifecycles;
using CsharpRAPL.Benchmarking.Serialization;
using CsharpRAPL.Benchmarking.Variation;
using CsharpRAPL.CommandLine;
using CsharpRAPL.Data;
using CsharpRAPL.Measuring;

namespace CsharpRAPL.Benchmarking;

public interface IBenchmarkState {
	public ulong LoopIterations { get; set; }
	public ulong Iterations { get; set; }
}

public class Benchmark<T> : IBenchmark {
	public IBenchmarkLifecycle BenchmarkLifecycle { get; init; }

	public BenchmarkInfo BenchmarkInfo { get; }
	private IMeasureApi MeasureApiApi { get; set; }
	public IResultsSerializer ResultsSerializer { get; }
	public bool ResetBenchmark { get; set; }

	//TODO: Check if using a interface here hurts measurements
	IMeasureApi IBenchmark.MeasureApiApi {
		get => MeasureApiApi;
		set => MeasureApiApi = value;
	}

	private const int MaxExecutionTime = 2700; //In seconds
	private const int TargetLoopIterationTime = 250; //In milliseconds

	// Prints everything to a null stream similar to /dev/null
	private readonly TextWriter _benchmarkOutputStream = new StreamWriter(Stream.Null);
	private readonly TextWriter _stdout;
	//private readonly Func<T> _benchmark;

	private string? _normalizedReturnValue;
	//private readonly FieldInfo _loopIterationsFieldInfo;

	//public Benchmark(BenchmarkInfo bi, IBenchmarkLifecycle)

	public Benchmark(IBenchmarkLifecycle blc, bool silenceBenchmarkOutput = true) {
		MeasureApiApi = null!;
		if (CsharpRAPLCLI.Options.Json) {
			ResultsSerializer = new JsonResultSerializer();
		}
		else {
			ResultsSerializer = new CSVResultSerializer();
		}

		BenchmarkLifecycle = blc;
		BenchmarkInfo = blc.BenchmarkInfo;
		_stdout = Console.Out;
		if (!silenceBenchmarkOutput) {
			_benchmarkOutputStream = _stdout;
		}
	}


	private void Start() {
		MeasureApiApi.Start();
	}

	private void End(object benchmarkOutput) {
		MeasureApiApi.End();

		//Result only valid if all results are valid
		//Only then is the result added and duration is incremented
		if (!MeasureApiApi.IsValid()) {
			return;
		}

		ulong loopIterations = GetLoopIterations();

		BenchmarkResult result = MeasureApiApi.GetResults(loopIterations) with {
			BenchmarkReturnValue = benchmarkOutput?.ToString() ?? string.Empty
		};

		BenchmarkResult normalizedResult = MeasureApiApi.GetNormalizedResults(loopIterations) with {
			BenchmarkReturnValue = _normalizedReturnValue ?? string.Empty
		};
		BenchmarkInfo.RawResults.Add(result);
		BenchmarkInfo.NormalizedResults.Add(normalizedResult);
		BenchmarkInfo.ElapsedTime += BenchmarkInfo.RawResults[^1].ElapsedTime / 1_000;
	}

	private void Setup() {
		//Sets console to write to null
		Console.SetOut(_benchmarkOutputStream);
		MeasureApiApi = CsharpRAPLCLI.Options.OnlyTime ? new TimerOnly() : new RAPL();

		if (MeasureApiApi is null) {
			throw new RAPLNotInitializedException();
		}

		BenchmarkInfo.ElapsedTime = 0;
		BenchmarkInfo.RawResults.Clear();
		BenchmarkInfo.NormalizedResults.Clear();

		if (CsharpRAPLCLI.Options.UseIterationCalculation) {
			BenchmarkInfo.Iterations = IterationCalculationAll();
		}

		//ulong oldLoopIter = BenchmarkInfo.LoopIterations;
		// Get normalized return value
		//SetLoopIterations(10);

		
		///TODO: Figure out... is this warmup? -- comment until figured out
/*****
 * SetLoopIterations(10); //TODO: Macrobenchmarks??	
		_normalizedReturnValue = _benchmark()?.ToString() ?? string.Empty;
		SetLoopIterations(oldLoopIter);
*******/
	}

	//IBenchmarkState LegacyState { get; set; } 
	class BenchmarkStateLegacyImpl : IBenchmarkState {
		public ulong LoopIterations { get; set; }
		public ulong Iterations { get; set; }
	}
	//Performs benchmarking
	//Writes progress to stdout if there is more than one iteration
	public void Run() {
		Console.WriteLine($"BenchmarkLifecycle: {BenchmarkLifecycle?.GetType().FullName}") ;
		Setup();
		Print(Console.WriteLine,"Initializing benchmark");
		object state = BenchmarkLifecycle.Initialize(this);
		Print(Console.WriteLine,"Warmup");
		for(ulong i=0;i<BenchmarkInfo.Iterations;i++) {
			state = BenchmarkLifecycle.WarmupIteration(state);
		}

		for (ulong i = 0; i <= BenchmarkInfo.Iterations; i++) {
			PrintExecutionHeader(i);
			
			if (CsharpRAPLCLI.Options.TryTurnOffGC) {
				GC.Collect();
				GC.TryStartNoGCRegion(CsharpRAPLCLI.Options.GCMemory, false);
			}

			MemoryApi? memoryMeasurement = null;

			if (CsharpRAPLCLI.Options.CollectMemoryInformation) {
				memoryMeasurement = new MemoryApi();
				memoryMeasurement.Start();
			}

			//Actually performing benchmark and resulting IO
			state = BenchmarkLifecycle.PreRun(state);
			Start();
			//T benchmarkOutput = _benchmark();
			state = BenchmarkLifecycle.Run(state);
			End(state);
			if (i == BenchmarkInfo.Iterations) {
				state = BenchmarkLifecycle.End(state);
			}
			//TODO: handle when IPC error
			state = BenchmarkLifecycle.PostRun(state);

			if (CsharpRAPLCLI.Options.TryTurnOffGC) {
				try {
					GC.EndNoGCRegion();
				}
				catch (InvalidOperationException e) {
					Print(Console.WriteLine, "");
					Print(Console.WriteLine, e.Message);
					Print(Console.WriteLine,
						"Unless this happens at a lot of iterations, unlikely to mean more than an outlier");
				}
			}

			if (CsharpRAPLCLI.Options.CollectMemoryInformation) {
				MemoryMeasurement measure = memoryMeasurement!.End();
				BenchmarkInfo.RawResults[^1].MemoryMeasurement = measure;
				BenchmarkInfo.NormalizedResults[^1].MemoryMeasurement = measure;
			}

			if (CsharpRAPLCLI.Options.UseLoopIterationScaling &&
				BenchmarkInfo.RawResults[^1].ElapsedTime < TargetLoopIterationTime) {
				state = BenchmarkLifecycle.AdjustLoopIterations(state);
			}
			if (ResetBenchmark) {
				i = 0;
				BenchmarkInfo.RawResults.Clear();
				BenchmarkInfo.NormalizedResults.Clear();
				ResetBenchmark = false;
				//TODO: re-initialize IPC
				state = BenchmarkLifecycle.Initialize(this);
			}

			if (BenchmarkInfo.ElapsedTime < MaxExecutionTime) {
				if (CsharpRAPLCLI.Options.UseIterationCalculation) {
					BenchmarkInfo.Iterations = IterationCalculationAll();
				}

				continue;
			}

			Print(Console.WriteLine,
				BenchmarkInfo.HasRun
					? $"\rEnding for {BenchmarkInfo.Name} benchmark due to repeated failure"
					: $"\rEnding for {BenchmarkInfo.Name} benchmark due to time constraints");

			break;
		}
		if(CsharpRAPLCLI.Options.Verbose) {
			Print(Console.WriteLine, $"\n LoopIterations: {BenchmarkInfo.LoopIterations}");
		}

		BenchmarkInfo.HasRun = true;
		/*** Redundant 
		BenchmarkInfo.LoopIterations = GetLoopIterations();*/
		ResultsSerializer.SerializeResults(this);
		//SetLoopIterations(CsharpRAPLCLI.Options.LoopIterations);


		//Resets console output
		Console.SetOut(_stdout);
	}

	private void PrintExecutionHeader(ulong i) {
		if (BenchmarkInfo.Iterations != 1) {
			if (CsharpRAPLCLI.Options.Verbose) {
				Print(Console.Write,
					$"\r{i} of {BenchmarkInfo.Iterations} for {BenchmarkInfo.Name}. LoopIterations: {GetLoopIterations()}. " +
					$"Time target: {TargetLoopIterationTime}. UseLoopScaling: {CsharpRAPLCLI.Options.UseLoopIterationScaling} UseIterationCalculation: {CsharpRAPLCLI.Options.UseIterationCalculation}" +
					$" Last took: {(BenchmarkInfo.RawResults.Count > 0 ? BenchmarkInfo.RawResults[^1].ElapsedTime : 0.00):##,###}ms");
			}
			else {
				Print(Console.Write, $"\r{i} of {BenchmarkInfo.Iterations} for {BenchmarkInfo.Name}");
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns>Returns true if we should reset the loop iterations</returns>
	private bool ScaleLoopIterations() {
		ulong currentValue = GetLoopIterations();

		switch (currentValue) {
			case ulong.MaxValue:
				return false;
			case >= ulong.MaxValue / 2:
				SetLoopIterations(ulong.MaxValue);
				BenchmarkInfo.RawResults.Clear();
				BenchmarkInfo.NormalizedResults.Clear();
				return true;
			default:
				SetLoopIterations(currentValue + currentValue);
				BenchmarkInfo.RawResults.Clear();
				BenchmarkInfo.NormalizedResults.Clear();
				return true;
		}
	}


	/// Used to print to standard out -- Everything printed outside this method will not be shown
	private void Print(Action<string> printAction, string value = "") {
		Console.SetOut(_stdout);
		printAction(value);
		Console.Out.Flush();
		Console.SetOut(_benchmarkOutputStream);
	}

	//TODO: Figure out if we want to still ignore first
	public List<BenchmarkResult> GetResults(bool ignoreFirst = true) {
		return new List<BenchmarkResult>(BenchmarkInfo.NormalizedResults.Skip(ignoreFirst ? 1 : 0));
	}
	

	//TODO: Figure out if we want to still ignore first
	public List<BenchmarkResult> GetRawResults(bool ignoreFirst = true) {
		return new List<BenchmarkResult>(BenchmarkInfo.RawResults.Skip(ignoreFirst ? 1 : 0));
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
		if (BenchmarkInfo.RawResults.Count < 10) {
			return 10;
		}
		
		// The alpha value is 1 - the confidence we want (Also known as p-value)
		double alpha = 1.0 - confidence;

		// Calculate the mean and standard deviation of the current sample
		// We start by adding all the values to a double-array as this is used to calculate mean and standard
		// deviation
		var values = new double[BenchmarkInfo.RawResults.Count];
		for (var i = 0; i < BenchmarkInfo.RawResults.Count; i++) {
			values[i] = resultType switch {
				BenchmarkResultType.Temperature => BenchmarkInfo.RawResults[i].Temperature,
				BenchmarkResultType.DRAMEnergy => BenchmarkInfo.RawResults[i].DRAMEnergy,
				BenchmarkResultType.ElapsedTime => BenchmarkInfo.RawResults[i].ElapsedTime,
				BenchmarkResultType.PackageEnergy => BenchmarkInfo.RawResults[i].PackageEnergy,
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
		return BenchmarkInfo.LoopIterations;
	}

	private void SetLoopIterations(ulong value) {
		BenchmarkInfo.LoopIterations = value;
	}
}