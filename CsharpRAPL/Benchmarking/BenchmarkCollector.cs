using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CsharpRAPL.Benchmarking; 

public class BenchmarkCollector : BenchmarkSuite {
	public int Iterations { get; }
	public int LoopIterations { get; }

	/// <summary>
	/// A map of a return type and a variation of the benchmark method using the return type as the generic argument
	/// </summary>
	private readonly Dictionary<Type, AddBenchmarkVariation> _registeredAddBenchmarkVariations = new();

	private readonly List<Type> _registeredBenchmarkClasses = new();

	public BenchmarkCollector(int iterations, int loopIterations, bool onlyCalling = true) {
		Iterations = iterations;
		LoopIterations = loopIterations;
		if (onlyCalling) {
			CollectBenchmarks(Assembly.GetCallingAssembly());
		}
		else {
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				CollectBenchmarks(assembly);
			}
		}
	}

	private void CollectBenchmarks(Assembly assembly) {
		foreach (MethodInfo benchmark in assembly.GetTypes().SelectMany(type => type.GetMethods())) {
			//Try to get the benchmark attribute
			var benchmarkAttribute = benchmark.GetCustomAttribute<BenchmarkAttribute>();
			if (benchmarkAttribute == null) {
				continue;
			}

			if (benchmarkAttribute.Skip) {
				continue;
			}

			CheckMethodValidity(benchmark);

			Type? benchmarkClass = _registeredBenchmarkClasses.FirstOrDefault(type => type == benchmark.DeclaringType);
			if (benchmarkClass == null) {
				RegisterBenchmarkClass(benchmark.DeclaringType!);
			}

			if (!_registeredAddBenchmarkVariations.ContainsKey(benchmark.ReturnType)) {
				//If we haven't registered this return type yet, register it.
				RegisterAddBenchmarkVariation(benchmark);
			}


			(MethodInfo genericAddBenchmark, Type funcType) =
				_registeredAddBenchmarkVariations[benchmark.ReturnType];

			//Then add the benchmark using the correct generic add benchmark method.
			genericAddBenchmark.Invoke(this, new object[] {
				benchmarkAttribute.Group!, Iterations,
				benchmark.CreateDelegate(funcType),
				benchmarkAttribute.Order
			});
		}
	}

	private void RegisterBenchmarkClass(Type benchmarkClass) {
		TrySetField(benchmarkClass, "Iterations", Iterations);
		TrySetField(benchmarkClass, "LoopIterations", LoopIterations);
		_registeredBenchmarkClasses.Add(benchmarkClass);
	}

	private static void TrySetField(Type benchmarkClass, string name, int value) {
		FieldInfo? fieldInfo = benchmarkClass.GetFields().FirstOrDefault(info => info.Name == name);
		if (fieldInfo == null) {
			return;
		}

		if (!fieldInfo.IsPublic) {
			throw new NotSupportedException($"Your {name} field must be public.");
		}

		if (!fieldInfo.IsStatic) {
			throw new NotSupportedException($"Your {name} field must be static.");
		}

		benchmarkClass.GetField(name, BindingFlags.Public | BindingFlags.Static)?.SetValue(null, value);
	}

	/// <summary>
	/// Checks if the method is public and does not have void as the return type.
	/// </summary>
	/// <param name="benchmark"></param>
	/// <exception cref="NotSupportedException">Throws NotSupportedException if the method isn't public or if the method returns void</exception>
	private static void CheckMethodValidity(MethodInfo benchmark) {
		if (!benchmark.IsPublic) {
			throw new NotSupportedException(
				"The benchmark attribute is only supported and supposed to be used on public methods.");
		}

		if (benchmark.ReturnType == typeof(void)) {
			throw new NotSupportedException(
				"The benchmark attribute is only supported and supposed to be used on methods with a non void return type.");
		}
	}

	private void RegisterAddBenchmarkVariation(MethodInfo benchmark) {
		//Create a generic type of func using the methods return type
		Type funcType = typeof(Func<>).MakeGenericType(benchmark.ReturnType);

		//Create some flags we use for binding see: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.bindingflags
		const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod;

		//Get the correct version of add benchmark method
		MethodInfo addBenchmarkMethod = GetType()
			.GetMethods(flags)
			.First(info => info.Name == "AddBenchmark" && info.GetParameters().Length == 4);

		//Make a generic using the benchmark return type
		MethodInfo genericAddBenchmark = addBenchmarkMethod.MakeGenericMethod(benchmark.ReturnType);

		//Add it to our registry
		_registeredAddBenchmarkVariations.Add(benchmark.ReturnType,
			new AddBenchmarkVariation(genericAddBenchmark, funcType));
	}
}

/// <summary>
/// A container class for a variation of the AddBenchmark method
/// </summary>
/// <param name="GenericAddBenchmark">The generic AddBenchmark method <see cref="BenchmarkSuite.AddBenchmark{T}(string?,int,System.Func{T},int)"/></param>
/// <param name="FuncType">The Func Type using the return type as generic argument</param>
internal sealed record AddBenchmarkVariation(MethodInfo GenericAddBenchmark, Type FuncType);