using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using CsharpRAPL.CommandLine;
using CsharpRAPL.Plotting;

namespace CsharpRAPL.Benchmarking;

public class BenchmarkSuite {
	public int Iterations { get; }
	public int LoopIterations { get; }
	private List<IBenchmark> Benchmarks { get; } = new();

	private readonly HashSet<Type> _registeredBenchmarkClasses = new();

	private const string LoopIterationsName = nameof(LoopIterations);
	private const string IterationsName = nameof(Iterations);

	public BenchmarkSuite() {
		Iterations = CsharpRAPLCLI.Options.DefaultIterations;
		LoopIterations = CsharpRAPLCLI.Options.LoopIterations;
	}

	public BenchmarkSuite(int iterations, int loopIterations) {
		Iterations = iterations;
		LoopIterations = loopIterations;
	}

	public void RegisterBenchmark<T>(Func<T> benchmark, int order = 0) {
		RegisterBenchmark(null, benchmark, order);
	}

	public void RegisterBenchmark<T>(string? group, Func<T> benchmark, int order = 0) {
		if (benchmark.Method.IsAnonymous()) {
			throw new NotSupportedException("Adding benchmarks through anonymous methods is not supported");
		}

		if (!_registeredBenchmarkClasses.Contains(benchmark.Method.DeclaringType!)) {
			RegisterBenchmarkClass(benchmark.Method.DeclaringType!);
		}

		Benchmarks.Add(new Benchmark<T>(benchmark.Method.Name, Iterations, benchmark, false, group, order));
	}

	private void RegisterBenchmarkClass(Type benchmarkClass) {
		TrySetField(benchmarkClass, IterationsName, Iterations);
		TrySetField(benchmarkClass, LoopIterationsName, LoopIterations);
		_registeredBenchmarkClasses.Add(benchmarkClass);
	}

	public static bool TrySetField(Type benchmarkClass, string name, int value) {
		FieldInfo? fieldInfo = benchmarkClass.GetFields()
			.FirstOrDefault(info => info.Name == name);
		if (fieldInfo == null) {
			return false;
		}

		if (!fieldInfo.IsStatic) {
			throw new NotSupportedException($"Your {name} field must be static.");
		}

		benchmarkClass.GetField(name, BindingFlags.Public | BindingFlags.Static)?.SetValue(null, value);
		return true;
	}

	public void RunAll() {
		List<IBenchmark> benchmarks = Benchmarks.OrderBy(benchmark => benchmark.Order).ToList();

		if (Environment.OSVersion.Platform != PlatformID.Unix) {
			throw new NotSupportedException("Running the benchmarks is only supported on Unix.");
		}

		Warmup();
		
		foreach ((int index, IBenchmark bench) in benchmarks.WithIndex()) {
			Console.WriteLine($"Starting {bench.Name} which is the {index + 1} out of {benchmarks.Count} tests");
			bench.Run();
			Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
			Console.WriteLine(
				$"\rFinished {bench.Name} in {bench.ElapsedTime}ms with {bench.GetResults().Count} iterations\n");
		}

		PlotGroups();

		if (CsharpRAPLCLI.Options.ZipResults) {
			ZipResults();
		}
	}

	private static void Warmup() {
		int warmup = 0;
		Console.WriteLine("Warmup commencing");
		for (int i = 0; i < 10; i++) {
			while (warmup < int.MaxValue) {
				warmup++;
				if (warmup % 1000000 != 0) {
					continue;
				}

				int percentage = (int)((double)warmup / int.MaxValue * 10.0 + 10.0 * i);
				Console.SetCursorPosition(0, Console.CursorTop);
				Console.Write($"{percentage}%");
				Console.Out.Flush();
			}

			warmup = 0;
		}

		Console.Write("\n");
	}

	private static void ZipResults() {
		using var zipToOpen = new FileStream("results.zip", FileMode.Create);
		using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
		foreach (string file in Helpers.GetAllCSVFilesFromOutputPath()) {
			archive.CreateEntryFromFile(file, file);
		}

		foreach (string file in Helpers.GetAllPlotFiles()) {
			archive.CreateEntryFromFile(file, file);
		}
	}

	public IReadOnlyList<IBenchmark> GetBenchmarks() {
		return Benchmarks;
	}

	public Dictionary<string, List<IBenchmark>> GetBenchmarksByGroup() {
		Dictionary<string, List<IBenchmark>> groups = new();
		foreach (IBenchmark benchmark in Benchmarks.Where(benchmark => benchmark.Group != null)) {
			Debug.Assert(benchmark.Group != null, "benchmark.Group != null");
			if (!groups.ContainsKey(benchmark.Group)) {
				groups.Add(benchmark.Group, new List<IBenchmark>());
			}

			groups[benchmark.Group].Add(benchmark);
		}

		return groups;
	}

	public void PlotGroups() {
		foreach ((string? group, List<IBenchmark>? benchmarks) in GetBenchmarksByGroup()) {
			BenchmarkPlot.PlotAllResults(benchmarks.ToArray(), new PlotOptions { Name = group });
		}
	}
}