using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsharpRAPL.Benchmarking;
using ScottPlot;

namespace CsharpRAPL.Analysis;

public static class BenchmarkPlot {
	public static void PlotResults(BenchmarkResultType resultType,
		params IBenchmark[] dataSets) {
		PlotResults(resultType,
			dataSets.Select(benchmark => new DataSet(benchmark.Name, benchmark.GetResults())).ToArray());
	}

	public static void PlotResults(BenchmarkResultType resultType, params DataSet[] dataSets) {
		if (dataSets.All(set => set.Data.Count == 0))
			throw new NotSupportedException("Plotting without data is not supported.");
		

		var plt = new Plot(600, 450);
		//We need this to move the data along x
		double[] iterationCount = Enumerable.Range(0, dataSets.Max(set => set.Data.Count))
			.Select(i => (double)i).ToArray();

		var names = new List<string>();

		foreach (DataSet dataSet in dataSets) {
			double[] data = GetPlotData(dataSet, resultType);
			names.Add(dataSet.Name);
			plt.PlotScatter(iterationCount, data, label: dataSet.Name);
		}

		plt.XLabel("Iteration");
		plt.Title(string.Join(" - ", names) + $" - {resultType}");
		plt.YLabel(GetYLabel(resultType));
		plt.Legend(location: legendLocation.upperRight);

		plt.AxisAuto(0, 0.5);

		//This is what is shown since we work with integers here we want it to be presented like so
		plt.XTicks(Enumerable.Range(1, iterationCount.Length).Select(i => i.ToString()).ToArray());
		string time = DateTime.Now.ToString("s").Replace(":", "-");
		Directory.CreateDirectory($"results/graphs/{resultType}");
		plt.SaveFig($"results/graphs/{resultType}/{string.Join(" - ", names)}-{time}.png");
	}

	private static double[] GetPlotData(DataSet dataSet, BenchmarkResultType resultType) {
		double[] data = resultType switch {
			BenchmarkResultType.ElapsedTime => dataSet.Data.Select(result => result.ElapsedTime).ToArray(),
			BenchmarkResultType.PackagePower => dataSet.Data.Select(result => result.PackagePower).ToArray(),
			BenchmarkResultType.DramPower => dataSet.Data.Select(result => result.DramPower).ToArray(),
			BenchmarkResultType.Temperature => dataSet.Data.Select(result => result.Temperature / 1000).ToArray(),
			_ => throw new ArgumentOutOfRangeException(nameof(resultType), resultType, null)
		};

		return data;
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