using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Accord.Statistics;
using CsharpRAPL.Analysis;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.CommandLine;
using Humanizer;
using ScottPlot;
using ScottPlot.Drawing;
using DataSet = CsharpRAPL.Analysis.DataSet;

namespace CsharpRAPL.Plotting;

public static class BenchmarkPlot {
	//TODO: Note that this expect the path is the root of the groups e.g.
	// Data/ would be a root that contained Data/Loops and Data/Control
	public static void PlotResultsGroupsFromFolder(BenchmarkResultType resultType, string path,
		PlotOptions plotOptions = default) {
		var groups = new Dictionary<string, List<DataSet>>();

		foreach (string file in Helpers.GetAllCSVFilesFromPath(path)) {
			//TODO: what if there is no group?
			string group = Path.GetRelativePath(Directory.GetCurrentDirectory(), file)
				.Split(Path.DirectorySeparatorChar)[1];
			if (!groups.ContainsKey(group)) {
				groups.Add(group, new List<DataSet>());
			}

			groups[group].Add(new DataSet(file));
		}

		foreach ((string? group, List<DataSet>? dataSets) in groups) {
			plotOptions.Name = group;
			PlotResults(resultType, dataSets.ToArray(), plotOptions);
		}
	}

	public static void PlotAllResultsGroupsFromFolder(string path, PlotOptions plotOptions = default) {
		PlotResultsGroupsFromFolder(BenchmarkResultType.ElapsedTime, path, plotOptions);
		PlotResultsGroupsFromFolder(BenchmarkResultType.PackagePower, path, plotOptions);
		PlotResultsGroupsFromFolder(BenchmarkResultType.DramPower, path, plotOptions);
		PlotResultsGroupsFromFolder(BenchmarkResultType.Temperature, path, plotOptions);
	}

	public static void PlotResults(BenchmarkResultType resultType, IBenchmark[] dataSets,
		PlotOptions plotOptions = default) {
		DataSet[] data = dataSets.Select(benchmark => new DataSet(benchmark.Name, benchmark.GetResults())).ToArray();
		PlotResults(resultType, data, plotOptions);
	}

	public static void PlotResults(BenchmarkResultType resultType, DataSet[] dataSets,
		PlotOptions plotOptions = default) {
		if (!ValidateData(ref dataSets)) {
			return;
		}

		var plt = new Plot(600, 450);

		string[] names = dataSets.Select(set => set.Name).ToArray();

		var hatchIndex = 3;
		foreach ((int index, DataSet dataSet) in dataSets.WithIndex()) {
			double[] plotData = GetPlotData(dataSet, resultType);
			double min = plotData.Min();
			double max = plotData.Max();

			BoxPlot bar = plt.PlotBoxPlot(index, plotData, min, max,
				$"{dataSet.Name}\nMax: {max:F4} Min: {min:F4}\nLowerQ: {plotData.LowerQuartile():F4} UpperQ: {plotData.UpperQuartile():F4}",
				useMinSize: false);

			if (hatchIndex > 9) {
				hatchIndex = 0;
			}

			bar.PlotOptions.HatchStyle = (HatchStyle)hatchIndex;
			bar.PlotOptions.HatchColor = Color.Gray;
			hatchIndex++;
		}

		if (plotOptions.RotateText && names.Max(s => s.Length) > 10 && dataSets.Length > 6) {
			plt.XAxis.TickLabelStyle(rotation: 45);
		}

		plt.XTicks(Enumerable.Range(0, dataSets.Length).Select(i1 => (double)i1).ToArray(), names);
		plt.XLabel("Benchmark");
		plt.YLabel(GetYLabel(resultType));
		plt.Title(string.IsNullOrEmpty(plotOptions.Name)
			? $"{resultType}"
			: $"{plotOptions.Name.Humanize(LetterCasing.Title)}");

		DateTime dateTime = DateTime.Now;
		var time = $"{dateTime.ToString("s").Replace(":", "-")}-{dateTime.Millisecond}";
		Directory.CreateDirectory($"{CsharpRAPLCLI.Options.PlotOutputPath}/{resultType}");
		plt.SaveFig(string.IsNullOrEmpty(plotOptions.Name)
			? $"{CsharpRAPLCLI.Options.PlotOutputPath}/{resultType}/{time}.png"
			: $"{CsharpRAPLCLI.Options.PlotOutputPath}/{resultType}/{plotOptions.Name}-{time}.png");
	}

	private static bool ValidateData(ref DataSet[] dataSets) {
		if (dataSets.Length == 0) {
			throw new NotSupportedException("Plotting without data is not supported.");
		}

		List<DataSet> dataWhereZero = dataSets.Where(set => set.Data.Count == 0).ToList();
		if (dataWhereZero.Count == dataSets.Length) {
			throw new NotSupportedException("Plotting without data is not supported.");
		}

		if (dataWhereZero.Count == 0) {
			return true;
		}

		List<DataSet> newDatasets = dataSets.ToList();
		foreach (DataSet dataSet in dataWhereZero) {
			Console.Error.WriteLine($"Skipping plotting {dataSet.Name} since it contains no data.");
			newDatasets.Remove(dataSet);
		}

		dataSets = newDatasets.ToArray();

		return true;
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
				.Select(result => result.Temperature).ToList(),
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

	public static void PlotAllResults(IBenchmark[] dataSet, PlotOptions plotOptions = default) {
		PlotResults(BenchmarkResultType.ElapsedTime, dataSet, plotOptions);
		PlotResults(BenchmarkResultType.PackagePower, dataSet, plotOptions);
		PlotResults(BenchmarkResultType.DramPower, dataSet, plotOptions);
		PlotResults(BenchmarkResultType.Temperature, dataSet, plotOptions);
	}

	public static void PlotAllResults(DataSet[] dataSet, PlotOptions plotOptions = default) {
		PlotResults(BenchmarkResultType.ElapsedTime, dataSet, plotOptions);
		PlotResults(BenchmarkResultType.PackagePower, dataSet, plotOptions);
		PlotResults(BenchmarkResultType.DramPower, dataSet, plotOptions);
		PlotResults(BenchmarkResultType.Temperature, dataSet, plotOptions);
	}
}