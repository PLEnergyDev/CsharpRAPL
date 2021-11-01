﻿using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using CsharpRAPL.CommandLine;

namespace CsharpRAPL;

public static class Helpers {
	[Pure]
	public static IEnumerable<(int index, TSource value)> WithIndex<TSource>(this IEnumerable<TSource> enumerable) {
		return enumerable.Select((value, index) => (index, value));
	}


	public static List<string> GetAllCSVFilesFromOutputPath() {
		return GetAllCSVFilesFromPath(CsharpRAPLCLI.Options.OutputPath);
	}

	public static List<string> GetAllCSVFilesFromPath(string path) {
		if (!Directory.Exists(path)) {
			return new List<string>();
		}

		return Directory.EnumerateFiles(path, "*.csv", SearchOption.AllDirectories)
			.Where(s => !s.Contains("_pvalues")).ToList();
	}

	public static List<string> GetAllPlotFiles() {
		if (!Directory.Exists(CsharpRAPLCLI.Options.PlotOutputPath)) {
			return new List<string>();
		}

		return Directory.EnumerateFiles(CsharpRAPLCLI.Options.PlotOutputPath, "*.png", SearchOption.AllDirectories)
			.ToList();
	}
}