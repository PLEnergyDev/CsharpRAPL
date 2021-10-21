using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Accord.Statistics;
using CsharpRAPL.Analysis;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.CommandLine;
using ScottPlot;
using ScottPlot.Drawing;
using DataSet = CsharpRAPL.Analysis.DataSet;

namespace CsharpRAPL.Plotting;

public static class BenchmarkPlot {
	
	public static void PlotResults(BenchmarkResultType resultType,
		params IBenchmark[] dataSets) {
		PlotResults(resultType,
			dataSets.Select(benchmark => new DataSet(benchmark.Name, benchmark.GetResults())).ToArray());
	}

	//TODO: Note that this expect the path is the root of the groups e.g.
	// Data/ would be a root that contained Data/Loops and Data/Control
	public static void PlotResultsGroupsFromFolder(BenchmarkResultType resultType, string path) {
		var groups = new Dictionary<string, List<DataSet>>();

		foreach (string file in Directory.EnumerateFiles(path, "*.csv", SearchOption.AllDirectories)) {
			string group = Path.GetRelativePath(Directory.GetCurrentDirectory(),file).Split(Path.DirectorySeparatorChar)[1];
			if (!groups.ContainsKey(group)) {
				groups.Add(group, new List<DataSet>());
			}

			groups[group].Add(new DataSet(file));
		}

		foreach (KeyValuePair<string, List<DataSet>> keyValuePair in groups) {
			PlotResults(resultType, keyValuePair.Value.ToArray());
		}
	}

	public static void PlotAllResultsGroupsFromFolder(string path) {
		PlotResultsGroupsFromFolder(BenchmarkResultType.ElapsedTime, path);
		PlotResultsGroupsFromFolder(BenchmarkResultType.PackagePower, path);
		PlotResultsGroupsFromFolder(BenchmarkResultType.DramPower, path);
		PlotResultsGroupsFromFolder(BenchmarkResultType.Temperature, path);
	}

	public static void PlotResults(BenchmarkResultType resultType, params DataSet[] dataSets) {
		if (dataSets.All(set => set.Data.Count == 0)) {
			throw new NotSupportedException("Plotting without data is not supported.");
		}

		var plt = new Plot(600, 450);

		string[] names = dataSets.Select(set => set.Name).ToArray();

		var hatchIndex = 3;
		foreach ((int index, DataSet dataSet) in dataSets.WithIndex()) {
			double[] plotData = GetPlotData(dataSet, resultType);
			double min = plotData.Min();
			double max = plotData.Max();

			BoxPlot bar = plt.PlotBoxPlot(index, plotData, min, max,
				$"{dataSet.Name}\nMax: {max:F4} Min: {min:F4}\nLowerQ: {plotData.LowerQuartile():F4} UpperQ: {plotData.UpperQuartile():F4}");

			if (hatchIndex > 9) {
				hatchIndex = 0;
			}

			bar.HatchStyle = (HatchStyle)hatchIndex;
			bar.HatchColor = Color.Gray;
			hatchIndex++;
		}

		//plt.Legend(location: legendLocation.upperRight);
		plt.XTicks(Enumerable.Range(0, dataSets.Length).Select(i1 => (double)i1).ToArray(), names);
		plt.XLabel("Benchmark");
		plt.YLabel(GetYLabel(resultType));
		plt.Title($"{resultType}");

		DateTime dateTime = DateTime.Now;
		var time = $"{dateTime.ToString("s").Replace(":", "-")}-{dateTime.Millisecond}";
		Directory.CreateDirectory($"{CsharpRAPLCLI.Options.PlotOutputPath}/{resultType}");
		plt.SaveFig($"{CsharpRAPLCLI.Options.PlotOutputPath}/{resultType}/{time}.png");
	}

	private static double[] GetPlotData(DataSet dataSet, BenchmarkResultType resultType) {
		List<double> data = resultType switch {
			BenchmarkResultType.ElapsedTime => dataSet.Data.Where(result => result.ElapsedTime > double.Epsilon)
				.Select(result => result.ElapsedTime).ToList(),
			BenchmarkResultType.PackagePower => dataSet.Data.Where(result => result.PackagePower > double.Epsilon)
				.Select(result => result.PackagePower).ToList(),
			BenchmarkResultType.DramPower => dataSet.Data.Where(result => result.DramPower > double.Epsilon)
				.Select(result => result.DramPower).ToList(),
			BenchmarkResultType.Temperature => dataSet.Data.Where(result => result.Temperature > double.Epsilon)
				.Select(result => result.Temperature / 1000).ToList(),
			_ => throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null)
		};

		return data.ToArray();
	}

	private static string GetYLabel(BenchmarkResultType resultType) {
		string yLabel = resultType switch {
			BenchmarkResultType.ElapsedTime => "Elapsed Time (ms)",
			BenchmarkResultType.PackagePower => "Package Power (µJ)",
			BenchmarkResultType.DramPower => "Dram Power (µJ)",
			BenchmarkResultType.Temperature => "Temperature (C°)",
			_ => throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null)
		};

		return yLabel;
	}

	public static void PlotAllResults(params IBenchmark[] dataSet) {
		PlotResults(BenchmarkResultType.ElapsedTime, dataSet);
		PlotResults(BenchmarkResultType.PackagePower, dataSet);
		PlotResults(BenchmarkResultType.DramPower, dataSet);
		PlotResults(BenchmarkResultType.Temperature, dataSet);
	}

	public static void PlotAllResults(params DataSet[] dataSet) {
		PlotResults(BenchmarkResultType.ElapsedTime, dataSet);
		PlotResults(BenchmarkResultType.PackagePower, dataSet);
		PlotResults(BenchmarkResultType.DramPower, dataSet);
		PlotResults(BenchmarkResultType.Temperature, dataSet);
	}
}