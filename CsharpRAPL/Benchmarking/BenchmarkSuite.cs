using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using CsharpRAPL.Benchmarking.Attributes;
using CsharpRAPL.Benchmarking.Variation;
using CsharpRAPL.CommandLine;
using CsharpRAPL.Plotting;

namespace CsharpRAPL.Benchmarking;

public class BenchmarkSuite {
	public ulong Iterations { get; }
	public ulong LoopIterations { get; }
	public PlotOptions PlotOptions { get; set; } = new();
	protected List<IBenchmark> Benchmarks { get; } = new();

	private readonly HashSet<Type> _registeredBenchmarkClasses = new();

	private const string LoopIterationsName = nameof(LoopIterations);
	private const string IterationsName = nameof(Iterations);

	public BenchmarkSuite() : this(CsharpRAPLCLI.Options.Iterations, CsharpRAPLCLI.Options.LoopIterations) { }

	public BenchmarkSuite(ulong iterations, ulong loopIterations) {
		Iterations = iterations;
		LoopIterations = loopIterations;
	}
	public void RegisterBenchmark(MethodInfo benchmarkMethod, BenchmarkAttribute benchmarkAttribute) {

		var bi = new BenchmarkInfo {
			Iterations = Iterations,
			LoopIterations = LoopIterations,
			Name = benchmarkAttribute.Name == "" ? benchmarkMethod.Name : benchmarkAttribute.Name,
			Group = benchmarkAttribute.Group!,
			Order = benchmarkAttribute.Order,
			PlotOrder = benchmarkAttribute.PlotOrder,
			//benchmarkAttribute.BenchmarkLifecycleClass,
			//PreBenchLookup.TryGetValue(benchmarkMethod.Name, out var preb)?preb.CreateDelegate<Action>():()=>{},
			//benchmarkAttribute.Order,
			//benchmarkAttribute.PlotOrder
		};


		var ctors = benchmarkAttribute.BenchmarkLifecycleClass.GetConstructors()
			.Select(
				v => new { 
					types = v.GetParameters()
					.Select(v => v.ParameterType)
					.ToArray(), ctor = v })
			.Where(v=>v.types.Count()<=2)
			.OrderByDescending(v=>v.types.Count())
			.ToList();
		IBenchmarkLifecycle? lifecycle = null;
		List<object> args = new List<object>();
		foreach(var v in ctors) {
			foreach (var t in v.types) {
				if (t.IsAssignableFrom(bi.GetType())) {
					args.Add(bi);
				}
				else if (t.IsAssignableFrom(benchmarkMethod.GetType())) {
					args.Add(benchmarkMethod);
				}
				else {
					goto continueOuter;
				}
			}
			lifecycle = (IBenchmarkLifecycle)v.ctor.Invoke(args.ToArray());
				//(IBenchmarkLifecycle)Activator.CreateInstance(benchmarkAttribute.BenchmarkLifecycleClass, args.ToArray());
		continueOuter:
			args.Clear();
			continue;
		}
		Benchmarks.Add(new Benchmark<object>(lifecycle));
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

		//if (warmup) {
		//	Warmup();
		//}

		foreach ((int index, IBenchmark bench) in benchmarks.OrderBy(v=>v.BenchmarkInfo.Order).WithIndex()) {
			//object o = bench.Initialize();

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