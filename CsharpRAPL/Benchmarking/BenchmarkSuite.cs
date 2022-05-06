using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using CsharpRAPL.Benchmarking.Variation;
using CsharpRAPL.CommandLine;
using CsharpRAPL.Plotting;

namespace CsharpRAPL.Benchmarking;

public class BenchmarkSuite {
	public ulong Iterations { get; }
	public ulong LoopIterations { get; }
	public PlotOptions PlotOptions { get; set; } = new();
	private List<IBenchmark> Benchmarks { get; } = new();

	private readonly HashSet<Type> _registeredBenchmarkClasses = new();

	private const string LoopIterationsName = nameof(LoopIterations);
	private const string IterationsName = nameof(Iterations);

	public BenchmarkSuite() : this(CsharpRAPLCLI.Options.Iterations, CsharpRAPLCLI.Options.LoopIterations) { }

	public BenchmarkSuite(ulong iterations, ulong loopIterations) {
		Iterations = iterations;
		LoopIterations = loopIterations;
	}

	public void RegisterBenchmark<T>(Func<T> benchmark, Type benchmarkLifecycleClass, int order = 0) {
		RegisterBenchmark(null, benchmark, benchmarkLifecycleClass, order);
	}

	public void RegisterBenchmark<T>(string? group, Func<T> benchmark, Type benchmarkLifecycleClass, int order = 0) {
		RegisterBenchmark(benchmark.Method.Name, group, benchmark, benchmarkLifecycleClass, order);
	}

	public void RegisterBenchmark<T>(string name, string? group, Func<T> benchmark, Type benchmarkLifecycleClass, int order = 0, int plotOrder = 0) {
		if (benchmark.Method.IsAnonymous()) {
			throw new NotSupportedException("Adding benchmarks through anonymous methods is not supported");
		}

		if (!_registeredBenchmarkClasses.Contains(benchmark.Method.DeclaringType!)) {
			RegisterBenchmarkClass(benchmark.Method.DeclaringType!);
		}

		Benchmarks.Add(new Benchmark<T>(name, Iterations, benchmark, benchmarkLifecycleClass, true, group, order, plotOrder));
	}

	public void RegisterBenchmarkVariation<T>(string name, string? group, Func<T> benchmark, Type benchmarkLifecycleClass,
		VariationInstance parameters, int order = 0, int plotOrder = 0, string namePostfix = "") {
		if (benchmark.Method.IsAnonymous()) {
			throw new NotSupportedException("Adding benchmarks through anonymous methods is not supported");
		}

		if (!_registeredBenchmarkClasses.Contains(benchmark.Method.DeclaringType!)) {
			RegisterBenchmarkClass(benchmark.Method.DeclaringType!);
		}

		var bench = new Benchmark<T>($"{name}{namePostfix}", Iterations, benchmark, benchmarkLifecycleClass,true, group, order, plotOrder)
			{ BenchmarkInfo = { Parameters = parameters } };
		Benchmarks.Add(bench);
	}

	public void RegisterBenchmarkVariation<T>(string? group, Func<T> benchmark, Type benchmarkLifecycleClass, VariationInstance parameters,
		int order = 0, int plotOrder = 0, string namePostfix = "") {
		RegisterBenchmarkVariation(benchmark.Method.Name, group, benchmark, benchmarkLifecycleClass, parameters, order, plotOrder, namePostfix);
	}

	private void RegisterBenchmarkClass(Type benchmarkClass) {
		SetField(benchmarkClass, IterationsName, Iterations);
		SetField(benchmarkClass, LoopIterationsName, LoopIterations);
		_registeredBenchmarkClasses.Add(benchmarkClass);
	}

	private static void CheckFieldValidity(Type benchmarkClass, string name) {
		FieldInfo? fieldInfo = benchmarkClass.GetFields()
			.FirstOrDefault(info => info.Name == name);
		if (fieldInfo == null) {
			throw new NotSupportedException(
				$"Your class '{benchmarkClass.Name}' doesn't have the field '{name}' and it is required.");
		}

		if (!fieldInfo.IsStatic) {
			throw new NotSupportedException($"Your '{name}' field must be static.");
		}

		if (fieldInfo.FieldType != typeof(ulong) && fieldInfo.FieldType != typeof(uint)) {
			throw new NotSupportedException(
				$"Your field '{fieldInfo.Name}' must have the type 'ulong' or 'uint' for the benchmark '{benchmarkClass.Name}'.");
		}
	}

	protected static void SetField(Type benchmarkClass, string name, ulong value) {
		CheckFieldValidity(benchmarkClass, name);
		benchmarkClass.GetField(name, BindingFlags.Public | BindingFlags.Static)?.SetValue(null, value);
	}

	public static void SetField(Type benchmarkClass, string name, uint value) {
		CheckFieldValidity(benchmarkClass, name);
		benchmarkClass.GetField(name, BindingFlags.Public | BindingFlags.Static)?.SetValue(null, value);
	}

	public void RunAll(bool warmup = true) {
		if (Environment.OSVersion.Platform != PlatformID.Unix && !CsharpRAPLCLI.Options.OnlyTime) {
			throw new NotSupportedException("Running the benchmarks is only supported on Unix.");
		}

		List<IBenchmark> benchmarks = Benchmarks.OrderBy(benchmark => benchmark.BenchmarkInfo.Order).ToList();
		if (benchmarks.Count == 0) {
			Console.WriteLine("There are no benchmarks to run.");
			return;
		}

		if (warmup) {
			Warmup();
		}

		foreach ((int index, IBenchmark bench) in benchmarks.WithIndex()) {
			Console.WriteLine(
				$"Starting {bench.BenchmarkInfo.Name} which is the {index + 1} benchmark out of {benchmarks.Count} benchmarks");
			bench.Run();
			Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
			Console.WriteLine(
				$"\rFinished {bench.BenchmarkInfo.Name} in {bench.BenchmarkInfo.ElapsedTime:F3}s with {bench.GetResults().Count} iterations\n");
		}

		if (CsharpRAPLCLI.Options.PlotResults) {
			PlotGroups();
		}

		Analysis.Analysis.CheckExecutionTime();

		if (CsharpRAPLCLI.Options.ZipResults) {
			ZipResults();
		}
	}

	private static void Warmup() {
		var warmup = 0;
		Console.WriteLine("Warmup commencing");
		for (var i = 0; i < 10; i++) {
			while (warmup < int.MaxValue) {
				warmup++;
				if (warmup % 1000000 != 0) {
					continue;
				}

				var percentage = (int)((double)warmup / int.MaxValue * 10.0 + 10.0 * i);
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
		foreach (string file in CsharpRAPLCLI.Options.Json
			? Helpers.GetAllJsonFilesFromOutputPath()
			: Helpers.GetAllCSVFilesFromOutputPath()) {
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
		foreach (IBenchmark benchmark in Benchmarks.Where(benchmark => benchmark.BenchmarkInfo.Group != null)) {
			Debug.Assert(benchmark.BenchmarkInfo.Group != null, "benchmark.Group != null");
			if (!groups.ContainsKey(benchmark.BenchmarkInfo.Group)) {
				groups.Add(benchmark.BenchmarkInfo.Group, new List<IBenchmark>());
			}

			groups[benchmark.BenchmarkInfo.Group].Add(benchmark);
		}

		return groups;
	}

	public void PlotGroups() {
		foreach ((string? group, List<IBenchmark>? benchmarks) in GetBenchmarksByGroup()) {
			PlotOptions options = PlotOptions;
			options.Name = group;
			BenchmarkPlot.PlotAllResults(benchmarks.ToArray(), options);
		}
	}
}