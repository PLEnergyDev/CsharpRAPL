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
		Name = benchmark.Name;
		Data = benchmark.GetResults();
	}

	private static DataSet ReadData(string path) {
		using var reader = new StreamReader(path);
		using var csv = new CsvReader(reader,
			new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" });

		return new DataSet(Path.GetFileNameWithoutExtension(path).Split('-')[0], csv.GetRecords<BenchmarkResult>().ToList());
	}

	public (bool isValid, string message) EnsureResults() {
		List<string> first = Data.Select(result => result.Result).Distinct().ToList();

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
			DramPower = Data.Min(result => result.DramPower),
			ElapsedTime = Data.Min(result => result.ElapsedTime),
			PackagePower = Data.Min(result => result.PackagePower)
		};
	}

	public BenchmarkResult GetMax() {
		return new BenchmarkResult {
			Temperature = Data.Max(result => result.Temperature),
			DramPower = Data.Max(result => result.DramPower),
			ElapsedTime = Data.Max(result => result.ElapsedTime),
			PackagePower = Data.Max(result => result.PackagePower)
		};
	}

	public BenchmarkResult GetAverage() {
		return new BenchmarkResult {
			Temperature = Data.Average(result => result.Temperature),
			DramPower = Data.Average(result => result.DramPower),
			ElapsedTime = Data.Average(result => result.ElapsedTime),
			PackagePower = Data.Average(result => result.PackagePower)
		};
	}
}