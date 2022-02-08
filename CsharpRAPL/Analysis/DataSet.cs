using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Data;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsharpRAPL.Analysis;

public class DataSet {
	public string Name { get; }
	public List<BenchmarkResult> Data { get; }

	public DataSet(string name, List<BenchmarkResult> data) {
		Name = name;
		Data = data;
	}

	public DataSet(string pathToDataSet) {
		DataSet temp = ReadData(pathToDataSet);
		Name = temp.Name;
		Data = temp.Data;
	}

	public DataSet(IBenchmark benchmark) {
		Name = benchmark.BenchmarkInfo.Name;
		Data = benchmark.GetResults();
	}

	private static DataSet ReadData(string path) {
		using var reader = new StreamReader(path);
		using var csv = new CsvReader(reader,
			new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" });

		return new DataSet(Path.GetFileNameWithoutExtension(path).Split('-')[0],
			csv.GetRecords<BenchmarkResult>().ToList());
	}

	public (bool isValid, string message) EnsureResults() {
		List<string> first = Data.Select(result => result.BenchmarkReturnValue).Distinct().ToList();

		return first.Count switch {
			0 => (false, $"{Name} has no results"),
			> 1 => (false,
				$"Not all results in {Name} was equal. Namely: {first[0]}, {first[1]}{(first.Count > 2 ? " and more." : ".")}"),
			_ => (true, "")
		};
	}

	public BenchmarkResult GetMin() {
		return new BenchmarkResult {
			Temperature = Data.Min(result => result.Temperature),
			DRAMEnergy = Data.Min(result => result.DRAMEnergy),
			ElapsedTime = Data.Min(result => result.ElapsedTime),
			PackageEnergy = Data.Min(result => result.PackageEnergy)
		};
	}

	public BenchmarkResult GetMinBy(BenchmarkResultType resultType) {
		return resultType switch {
			BenchmarkResultType.ElapsedTime => Data.MinBy(result => result.ElapsedTime),
			BenchmarkResultType.PackageEnergy => Data.MinBy(result => result.PackageEnergy),
			BenchmarkResultType.DRAMEnergy => Data.MinBy(result => result.DRAMEnergy),
			BenchmarkResultType.Temperature => Data.MinBy(result => result.Temperature),
			_ => throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null)
		} ?? throw new InvalidOperationException("The data set had no elements.");
	}

	public BenchmarkResult GetMax() {
		return new BenchmarkResult {
			Temperature = Data.Max(result => result.Temperature),
			DRAMEnergy = Data.Max(result => result.DRAMEnergy),
			ElapsedTime = Data.Max(result => result.ElapsedTime),
			PackageEnergy = Data.Max(result => result.PackageEnergy)
		};
	}

	public BenchmarkResult GetMaxBy(BenchmarkResultType resultType) {
		return resultType switch {
			BenchmarkResultType.ElapsedTime => Data.MaxBy(result => result.ElapsedTime),
			BenchmarkResultType.PackageEnergy => Data.MaxBy(result => result.PackageEnergy),
			BenchmarkResultType.DRAMEnergy => Data.MaxBy(result => result.DRAMEnergy),
			BenchmarkResultType.Temperature => Data.MaxBy(result => result.Temperature),
			_ => throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null)
		} ?? throw new InvalidOperationException("The data set had no elements.");
	}

	public BenchmarkResult GetAverage() {
		return new BenchmarkResult {
			Temperature = Data.Average(result => result.Temperature),
			DRAMEnergy = Data.Average(result => result.DRAMEnergy),
			ElapsedTime = Data.Average(result => result.ElapsedTime),
			PackageEnergy = Data.Average(result => result.PackageEnergy)
		};
	}
}