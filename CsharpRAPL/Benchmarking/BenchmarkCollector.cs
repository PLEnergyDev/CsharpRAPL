using System;
using System.Collections.Generic;
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

	public BenchmarkCollector(bool onlyCallingAssembly = false) : this(CsharpRAPLCLI.Options.Iterations,
		CsharpRAPLCLI.Options.LoopIterations, onlyCallingAssembly) { }

	public BenchmarkCollector(ulong iterations, ulong loopIterations, bool onlyCallingAssembly = false) :
		base(iterations, loopIterations) {
		if (onlyCallingAssembly) {
			CollectBenchmarks(Assembly.GetCallingAssembly() ?? throw new InvalidOperationException());
		}
		else {
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				CollectBenchmarks(assembly);
			}
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

			//SetField(benchmarkMethod.DeclaringType!, nameof(LoopIterations), LoopIterations);
			//SetField(benchmarkMethod.DeclaringType!, nameof(Iterations), Iterations);

			//CheckMethodValidity(benchmarkMethod);
			RegisterBenchmark(benchmarkMethod, benchmarkAttribute);



			////Func<object> f = benchmarkMethod.Invoke(benchmarkMethod.IsStatic?null: Activator.CreateInstance(benchmarkMethod.DeclaringType), )


			//if (!_registeredBenchmarkVariations.ContainsKey(benchmarkMethod.ReturnType)) {
			//	//If we haven't registered this return type yet, register it.
			//	RegisterAddBenchmarkVariation(benchmarkMethod);
			//}

			//(MethodInfo genericRegisterBenchmark, Type funcType) =
			//	_registeredBenchmarkVariations[benchmarkMethod.ReturnType];


			//if (benchmarkMethod.GetCustomAttribute<VariationBenchmark>() != null && !benchmarkMethod.IsStatic) {
			//	VariationGenerator.CreateVariations(this, benchmarkAttribute, benchmarkMethod);
			//}

			////If the benchmark method is static, we don't need an instance to call the method.
			////So if it is we use Activator to create a new instance which we use for calling the method.
			//Delegate benchmarkDelegate = benchmarkMethod.IsStatic
			//	? benchmarkMethod.CreateDelegate(funcType)
			//	: benchmarkMethod.CreateDelegate(funcType, Activator.CreateInstance(benchmarkMethod.DeclaringType!));



			////Then add the benchmark using the correct generic add benchmark method.
			//genericRegisterBenchmark.Invoke(this, new object[] {
			//	benchmarkAttribute.Name == "" ? benchmarkMethod.Name : benchmarkAttribute.Name,
			//	benchmarkAttribute.Group!,
			//	benchmarkDelegate,
			//	benchmarkAttribute.BenchmarkLifecycleClass,
			//	//PreBenchLookup.TryGetValue(benchmarkMethod.Name, out var preb)?preb.CreateDelegate<Action>():()=>{},
			//	benchmarkAttribute.Order,
			//	benchmarkAttribute.PlotOrder
			//});
		}
	}



	/// <summary>
	/// Checks if the method has none or are single parameter and does not have void as the return type.
	/// </summary>
	/// <param name="benchmark"></param>
	/// <exception cref="NotSupportedException">Throws NotSupportedException if the method has parameters or if the method returns void</exception>
	public static void CheckMethodValidity(MethodInfo benchmark) {
		if (benchmark.ReturnType == typeof(void)) {
			throw new NotSupportedException(
				$"The benchmark '{benchmark.Name}' is returning void which isn't supported.");
		}

		if (benchmark.GetParameters().Length >1) {
			throw new NotSupportedException($"The Benchmark '{benchmark.Name}' has parameters which isn't supported.");
		}
	}

	//private void RegisterAddBenchmarkVariation(MethodInfo benchmark) {
	//	//Create a generic type of func using the methods return type
	//	Type funcType = typeof(Func<>).MakeGenericType(benchmark.ReturnType);

	//	//Make a generic using the benchmark return type
	//	MethodInfo genericAddBenchmark = RegisterBenchmarkGenericMethod.MakeGenericMethod(benchmark.ReturnType);

	//	//Add it to our registry
	//	_registeredBenchmarkVariations.Add(benchmark.ReturnType,
	//		new RegisterBenchmarkVariation(genericAddBenchmark, funcType));
	//}
}

/// <summary>
/// A container class for a variation of the AddBenchmark method
/// </summary>
/// <param name="GenericAddBenchmark">The generic AddBenchmark method <see cref="BenchmarkSuite.RegisterBenchmark{T}(string?,System.Func{T},int)"/></param>
/// <param name="FuncType">The Func Type using the return type as generic argument</param>
internal sealed record RegisterBenchmarkVariation(MethodInfo GenericAddBenchmark, Type FuncType);