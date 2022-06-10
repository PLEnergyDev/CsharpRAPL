using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CsharpRAPL.Benchmarking.Attributes;
using CsharpRAPL.Benchmarking.Variation;
using CsharpRAPL.CommandLine;

namespace CsharpRAPL.Benchmarking;
public class BenchmarkCollector : BenchmarkSuite {
	/// <summary>
	/// A map of a return type and a variation of the benchmark method using the return type as the generic argument
	/// </summary>
	//private readonly Dictionary<Type, RegisterBenchmarkVariation> _registeredBenchmarkVariations = new();

	//Create some flags we use for binding see: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.bindingflags
	private const BindingFlags RegisterBenchmarkFlags =
		BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod;

	//Get the correct version of add benchmark method
	//private static readonly MethodInfo RegisterBenchmarkGenericMethod = typeof(BenchmarkSuite)
	//	.GetMethods(RegisterBenchmarkFlags)
	//	.First(info => info.Name == nameof(RegisterBenchmark) && info.GetParameters().Length == 6);

	/// <summary>
	/// BenchmarkCollector tries to collect all benchmarks in the current assembly or referenced assemblies by use of the <see cref="BenchmarkAttribute"/>.
	/// </summary>
	/// <param name="onlyCallingAssembly">If we should only load the current assembly. default: false</param>
	/// <param name="forceLoadAllAssemblies">Forces to load all assemblies in the program folder. default: false</param>
	public BenchmarkCollector(bool onlyCallingAssembly = false, bool forceLoadAllAssemblies = false) : this(
		CsharpRAPLCLI.Options.Iterations,
		CsharpRAPLCLI.Options.LoopIterations, onlyCallingAssembly, forceLoadAllAssemblies) { }

	/// <summary>
	/// BenchmarkCollector tries to collect all benchmarks in the current assembly or referenced assemblies by use of the <see cref="BenchmarkAttribute"/>.
	/// </summary>
	/// <param name="iterations">The amount iterations to run ignored if <see cref="Options.UseIterationCalculation"/></param>
	/// <param name="loopIterations">The amount loop iterations to run ignored if <see cref="Options.UseLoopIterationScaling"/></param>
	/// <param name="onlyCallingAssembly">If we should only load the current assembly. default: false</param>
	/// <param name="forceLoadAllAssemblies">Forces to load all assemblies in the program folder. default: false</param>
	/// <exception cref="InvalidOperationException">Thrown if we can't get the calling assembly</exception>
	public BenchmarkCollector(ulong iterations, ulong loopIterations, bool onlyCallingAssembly = false,
		bool forceLoadAllAssemblies = false) :
		base(iterations, loopIterations) {
		if (onlyCallingAssembly) {
			CollectBenchmarks(Assembly.GetCallingAssembly() ?? throw new InvalidOperationException());
			return;
		}

		if (forceLoadAllAssemblies) {
			List<Assembly> loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
			IEnumerable<string> loadedPaths = loadedAssemblies.Select(a => a.Location);

			string[] referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
			List<string> toLoad = referencedPaths
				.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

			toLoad.ForEach(path =>
				loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));
		}

		foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
			CollectBenchmarks(assembly);
		}
	}
	private void CollectBenchmarks(Assembly assembly) {
		IEnumerable<MethodInfo> methods = assembly.GetTypes().SelectMany(type => type
			.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
			.Where(info => info.GetCustomAttribute<BenchmarkAttribute>() != null));

		foreach (MethodInfo benchmarkMethod in methods) {
			var benchmarkAttribute = benchmarkMethod.GetCustomAttribute<BenchmarkAttribute>()!;
			if (benchmarkAttribute.Skip ||
				benchmarkMethod.DeclaringType!.GetCustomAttribute<SkipBenchmarksAttribute>() != null) {
				continue;
			}
			RegisterBenchmark(benchmarkMethod, benchmarkAttribute);
		}
	}

}

/// <summary>
/// A container class for a variation of the AddBenchmark method
/// </summary>
/// <param name="GenericAddBenchmark">The generic AddBenchmark method <see cref="BenchmarkSuite.RegisterBenchmark{T}(string?,System.Func{T},int)"/></param>
/// <param name="FuncType">The Func Type using the return type as generic argument</param>
internal sealed record RegisterBenchmarkVariation(MethodInfo GenericAddBenchmark, Type FuncType);