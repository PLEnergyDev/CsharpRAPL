using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScottPlot;

namespace CsharpRAPL.Analysis;

public static class BenchmarkPlot {
	public static void PlotResults(BenchmarkResultType resultType, Alignment alignment = Alignment.UpperRight,
		params DataSet[] dataSets) {
		var plt = new Plot();
		//We need this to move the data along x
		double[] iterationCount = Enumerable.Range(0, dataSets.Max(set => set.Data.Count))
			.Select(i => (double)i).ToArray();

		var graphData = new List<double[]>();
		var names = new List<string>();

		foreach (DataSet dataSet in dataSets) {
			double[] data = GetPlotData(dataSet, resultType);
			graphData.Add(data);
			names.Add(dataSet.Name);
			plt.AddScatter(iterationCount, data, label: dataSet.Name);
		}

		plt.XLabel("Iteration");
		plt.XAxis2.Label(string.Join(" - ", names) + $" - {resultType}");
		plt.YLabel(GetYLabel(resultType));
		plt.Legend(location: alignment);

		plt.SetAxisLimitsY(0, graphData.Max(data => data.Max()) * 1.5f);

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
}