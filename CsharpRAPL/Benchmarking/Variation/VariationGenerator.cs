using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CsharpRAPL.Benchmarking.Attributes;

namespace CsharpRAPL.Benchmarking.Variation;
//TODO: Add so that groups separate variations
public static class VariationGenerator {
	private const BindingFlags RegisterBenchmarkFlags =
		BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod;

	private static readonly MethodInfo RegisterBenchmarkVariationGenericMethod = typeof(BenchmarkSuite)
		.GetMethods(RegisterBenchmarkFlags)
		.First(info => info.Name == nameof(RegisterBenchmarkVariation) && info.GetParameters().Length == 7);

	private static List<VariationParameter> GetFields(Type declaringType) {
		List<FieldInfo> fieldVariations = declaringType
			.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance)
			.Where(info => info.GetCustomAttribute<VariationsAttribute>() != null ||
			               info.GetCustomAttribute<TypeVariationsAttribute>() != null).ToList();
		List<PropertyInfo> propertyVariations = declaringType
			.GetProperties(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance)
			.Where(info => info.GetCustomAttribute<VariationsAttribute>() != null ||
			               info.GetCustomAttribute<TypeVariationsAttribute>() != null).ToList();

		if (declaringType.IsAbstract || declaringType.IsInterface) {
			throw new NotSupportedException(
				$"Having Variations in interfaces, static or abstract classes isn't supported as in {declaringType.Name}.");
		}

		CheckFieldVariationValidity(fieldVariations, propertyVariations);

		var input = new List<VariationParameter>();
		foreach (FieldInfo field in fieldVariations) {
			if (field.GetCustomAttribute<VariationsAttribute>() != null) {
				List<object> values = field.GetCustomAttribute<VariationsAttribute>()!.Values.ToList();
				input.Add(new VariationParameter(field.Name, values, true));
			}

			if (field.GetCustomAttribute<TypeVariationsAttribute>() != null) {
				var attribute = field.GetCustomAttribute<TypeVariationsAttribute>();
				List<object> values = attribute!.Types.Select(o => Activator.CreateInstance(o)).ToList();
				input.Add(new VariationParameter(field.Name, values, true));
			}
		}

		foreach (PropertyInfo property in propertyVariations) {
			if (property.GetCustomAttribute<VariationsAttribute>() != null) {
				List<object> values = property.GetCustomAttribute<VariationsAttribute>()!.Values.ToList();
				input.Add(new VariationParameter(property.Name, values, false));
			}

			if (property.GetCustomAttribute<TypeVariationsAttribute>() != null) {
				var attribute = property.GetCustomAttribute<TypeVariationsAttribute>();
				List<object> values = attribute!.Types.Select(o => Activator.CreateInstance(o)).ToList();

				input.Add(new VariationParameter(property.Name, values, false));
			}
		}

		return input;
	}

	////TODO: Maybe put variations in to a table to reuse so we don't have to create instances for all benchmarks
	//public static void CreateVariations(BenchmarkCollector benchmarkCollector, BenchmarkAttribute benchmarkAttribute,
	//	MethodInfo benchmarkMethod) {
	//	Type funcType = typeof(Func<>).MakeGenericType(benchmarkMethod.ReturnType);
	//	//Make a generic using the benchmark return type
	//	MethodInfo genericAddBenchmark =
	//		RegisterBenchmarkVariationGenericMethod.MakeGenericMethod(benchmarkMethod.ReturnType);

	//	Type declaringType = benchmarkMethod.DeclaringType!;

	//	var result = new List<VariationInstance>();
	//	GeneratePermutations(GetFields(declaringType), result);

	//	foreach ((int index, VariationInstance instance) in result.WithIndex()) {
	//		object inst = Activator.CreateInstance(declaringType)!;
	//		foreach ((string name, object value, bool isField) in instance.Values) {
	//			SetValue(inst, name, value, isField);
	//		}


			
	//		Delegate benchmark = benchmarkMethod.CreateDelegate(funcType, inst);
	//		genericAddBenchmark.Invoke(benchmarkCollector, new object[] {
	//			benchmarkAttribute.Name == "" ? benchmark.Method.Name : benchmarkAttribute.Name,
	//			benchmarkAttribute.Group!,
	//			benchmark,
	//			instance,
	//			benchmarkAttribute.Order,
	//			benchmarkAttribute.PlotOrder,
	//			$"Variation-{index + 1}"
	//		});
	//	}
	//}

	private static void SetValue(object instance, string name, object value, bool isField) {
		if (isField) {
			FieldInfo? field = instance.GetType().GetField(name);

			if (field == null) {
				throw new MissingMemberException(
					$"Somehow the field '{name}' has disappeared from your benchmark '{instance.GetType()}'");
			}

			field.SetValue(instance, value);
		}
		else {
			PropertyInfo? property = instance.GetType().GetProperty(name);
			if (property == null) {
				throw new MissingMemberException(
					$"Somehow the property '{name}' has disappeared from your benchmark '{instance.GetType()}'");
			}

			property.SetValue(instance, value);
		}
	}

	private static void CheckFieldVariationValidity(IEnumerable<FieldInfo> fieldVariations,
		IEnumerable<PropertyInfo> propertyVariations) {
		List<FieldInfo> staticFields = fieldVariations.Where(info => info.IsStatic).ToList();
		if (staticFields.Count != 0) {
			Type declaringType = staticFields[0].DeclaringType ?? throw new InvalidOperationException();
			throw new NotSupportedException(
				$"Static fields isn't supported for variations the field(s) are: '{string.Join(",", staticFields.Select(info => info.Name))}" +
				$"' in '{declaringType.Name}'");
		}

		List<PropertyInfo> staticProperties =
			propertyVariations.Where(info => info.GetAccessors(true)[0].IsStatic).ToList();
		if (staticProperties.Count != 0) {
			Type declaringType = staticProperties[0].DeclaringType ?? throw new InvalidOperationException();
			throw new NotSupportedException(
				$"Static properties isn't supported for variations the property(ies) are: '{string.Join(",", staticProperties.Select(info => info.Name))}" +
				$"' in '{declaringType.Name}'");
		}
	}

	//private static void GeneratePermutations(IReadOnlyList<VariationParameter> input,
	//	ICollection<VariationInstance> result,
	//	int depth = 0, List<VariationInstance.MemberInfo>? current = null) {
	//	if (input.Count == 0) {
	//		return;
	//	}

	//	current ??= new List<VariationInstance.MemberInfo>();

	//	if (depth == input.Count) {
	//		var inst = new VariationInstance();
	//		foreach ((string name, object value, bool isField) in current) {
	//			inst.Values.Add(new VariationInstance.MemberInfo(name, value, isField));
	//		}

	//		result.Add(inst);
	//		return;
	//	}

	//	for (var i = 0; i < input[depth].Values.Count; i++) {
	//		var cur = new List<VariationInstance.MemberInfo>
	//			{ new(input[depth].Name, input[depth].Values[i], input[depth].IsField) };
	//		cur.AddRange(current);
	//		GeneratePermutations(input, result, depth + 1, cur);
	//	}
	//}
}