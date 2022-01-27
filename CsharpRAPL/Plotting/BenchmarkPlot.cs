using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using CsharpRAPL.Analysis;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.CommandLine;
using Humanizer;
using ScottPlot;
using ScottPlot.Drawing;
using DataSet = CsharpRAPL.Analysis.DataSet;

namespace CsharpRAPL.Plotting;

public static class BenchmarkPlot {
	/// <summary>
	/// Plots results groups from a folder.
	/// </summary>
	/// <param name="resultType">The result type to plot.</param>
	/// <param name="path">The root path to the groups e.g. Data/ would be a root that contained Data/Loops and Data/Control.</param>
	/// <param name="plotOptions">Plot configuration options.</param>
	public static void PlotResultsGroupsFromFolder(BenchmarkResultType resultType, string path,
		PlotOptions? plotOptions = null) {
		var groups = new Dictionary<string, List<DataSet>>();

		foreach (string file in Helpers.GetAllCSVFilesFromPath(path)) {
			string group = Path.GetRelativePath(Directory.GetCurrentDirectory(), file)
				.Split(Path.DirectorySeparatorChar)[1];
			if (!groups.ContainsKey(group)) {
				groups.Add(group, new List<DataSet>());
			}

			groups[group].Add(new DataSet(file));
		}

		plotOptions ??= new PlotOptions();

		foreach ((string? group, List<DataSet>? dataSets) in groups) {
			var options = new PlotOptions(plotOptions) { Name = group };
			PlotResults(resultType, dataSets.ToArray(), options);
		}
	}

	public static void PlotAllResultsGroupsFromFolder(string path, PlotOptions? plotOptions = null) {
		PlotResultsGroupsFromFolder(BenchmarkResultType.ElapsedTime, path, plotOptions);
		PlotResultsGroupsFromFolder(BenchmarkResultType.PackageEnergy, path, plotOptions);
		PlotResultsGroupsFromFolder(BenchmarkResultType.DRAMEnergy, path, plotOptions);
		PlotResultsGroupsFromFolder(BenchmarkResultType.Temperature, path, plotOptions);
	}

	public static void PlotResults(BenchmarkResultType resultType, IBenchmark[] dataSets,
		PlotOptions? plotOptions = null) {
		DataSet[] data = dataSets.Select(benchmark => new DataSet(benchmark.Name, benchmark.GetResults())).ToArray();
		PlotResults(resultType, data, plotOptions);
	}

	public static void PlotResults(BenchmarkResultType resultType, DataSet[] dataSets,
		PlotOptions? plotOptions = null) {
		if (!ValidateData(ref dataSets)) {
			return;
		}

		plotOptions ??= new PlotOptions();

		var plt = new Plot(plotOptions.Width, plotOptions.Height);

		dataSets = dataSets.OrderBy(set => set.Name).ToArray();

		string[] names = dataSets.Select(set => set.Name.Humanize(LetterCasing.Title)).ToArray();


		var hatchIndex = 3;
		foreach ((int index, DataSet dataSet) in dataSets.WithIndex()) {
			double[] plotData = GetPlotData(dataSet, resultType);
			if (plotData.Length == 0) {
				Console.Error.WriteLine($"No data for {resultType} skipping.");
				continue;
			}

			double min = plotData.Min();
			double max = plotData.Max();

			BoxPlot boxPlot = plt.AddBoxPlot(index, plotData, min, max, plotOptions);

			boxPlot.PlotOptions.LegendLabel =
				$"{dataSet.Name}\nMax: {boxPlot.MaxValue:F4} Min: {boxPlot.MinValue:F4}\nLowerPQ: {boxPlot.LowerPValueQuantile:F4} UpperPQ: {boxPlot.UpperPValueQuantile:F4}\n Average: {boxPlot.Average:F4} Median: {boxPlot.Median:F4}";

			if (hatchIndex > 9) {
				hatchIndex = 0;
			}

			if (boxPlot.PlotOptions.UseColorRange) {
				boxPlot.PlotOptions.FillColor = plt.GetSettings().GetNextColor();
			}

			boxPlot.PlotOptions.HatchStyle = (HatchStyle)hatchIndex;
			boxPlot.PlotOptions.HatchColor = Color.Gray;

			hatchIndex++;
		}

		if (plotOptions.RotateText && names.Max(s => s.Length) > 10 && dataSets.Length > 3) {
			plt.XAxis.TickLabelStyle(rotation: 45);
		}

		plt.XTicks(Enumerable.Range(0, dataSets.Length).Select(i1 => (double)i1).ToArray(), names);
		plt.XLabel("Benchmark");
		plt.YLabel(GetYLabel(resultType));
		plt.YAxis.TickLabelFormat(d => Math.Round(d, 5).ToString("G", CultureInfo.CreateSpecificCulture("da-DK")));
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
			BenchmarkResultType.PackageEnergy => dataSet.Data.Where(result => result.PackageEnergy > double.Epsilon)
				.Select(result => result.PackageEnergy).ToList(),
			BenchmarkResultType.DRAMEnergy => dataSet.Data.Where(result => result.DRAMEnergy > double.Epsilon)
				.Select(result => result.DRAMEnergy).ToList(),
			BenchmarkResultType.Temperature => dataSet.Data.Where(result => result.Temperature > double.Epsilon)
				.Select(result => result.Temperature).ToList(),
			_ => throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null)
		};

		return data.ToArray();
	}

	private static string GetYLabel(BenchmarkResultType resultType) {
		string yLabel = resultType switch {
			BenchmarkResultType.ElapsedTime => "Elapsed Time (ms)",
			BenchmarkResultType.PackageEnergy => "Package Energy (µJ)",
			BenchmarkResultType.DRAMEnergy => "DRAM Energy (µJ)",
			BenchmarkResultType.Temperature => "Temperature (C°)",
			_ => throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null)
		};

		return yLabel;
	}

	public static void PlotAllResults(IBenchmark[] dataSet, PlotOptions? plotOptions = null) {
		PlotResults(BenchmarkResultType.ElapsedTime, dataSet, plotOptions);
		PlotResults(BenchmarkResultType.PackageEnergy, dataSet, plotOptions);
		PlotResults(BenchmarkResultType.DRAMEnergy, dataSet, plotOptions);
		PlotResults(BenchmarkResultType.Temperature, dataSet, plotOptions);
	}

	public static void PlotAllResults(DataSet[] dataSet, PlotOptions? plotOptions = null) {
		PlotResults(BenchmarkResultType.ElapsedTime, dataSet, plotOptions);
		PlotResults(BenchmarkResultType.PackageEnergy, dataSet, plotOptions);
		PlotResults(BenchmarkResultType.DRAMEnergy, dataSet, plotOptions);
		PlotResults(BenchmarkResultType.Temperature, dataSet, plotOptions);
	}
}