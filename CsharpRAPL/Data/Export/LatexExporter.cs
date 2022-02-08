using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsharpRAPL.Analysis;
using CsharpRAPL.CommandLine;
using Humanizer;

namespace CsharpRAPL.Data.Export;

public static class LatexExporter {
	private static CultureInfo _provider = CultureInfo.CreateSpecificCulture("da-DK");
	private static NumberFormatInfo _formatInfo = new() { NumberDecimalSeparator = ".", NumberGroupSeparator = "," };

	public static void SetCultureInfo(CultureInfo cultureInfo) {
		_provider = cultureInfo;
	}

	public static void SetNumberFormatInfo(NumberFormatInfo formatInfo) {
		_formatInfo = formatInfo;
	}

	public static void GenerateTexImages(string outputPath = "_Latex") {
		var builder = new StringBuilder();
		var groupMap = new Dictionary<string, List<string>>();

		foreach (string plotFile in Helpers.GetAllPlotFiles().Select(str => str.Replace("\\", "/"))) {
			string local = plotFile.Split('-')[0].Replace("_plots/", "");
			string group = local.Split('/')[1];

			if (!groupMap.ContainsKey(group)) {
				groupMap.Add(group, new List<string>());
			}

			groupMap[group].Add(plotFile);
		}

		foreach (string group in groupMap.Keys) {
			foreach (string plotFile in groupMap[group]) {
				string local = plotFile.Split('-')[0].Replace("_plots/", "");
				string resultType = local.Split('/')[0];
				builder.Append("\\begin{figure}[H]\n\t" +
				               "\\centering\n\t" +
				               "\\includegraphics[width=\\textwidth]{" +
				               $"figures/Plots/{group}/{resultType}.png" +
				               "}\n\t" +
				               "\\caption{" +
				               $"Boxplot showing how efficient different {group.Humanize(LetterCasing.Title)} types are with regards to {resultType.Humanize(LetterCasing.Title)}." +
				               "}\n\t" +
				               "\\label{" +
				               $"fig:{group.ToLower()}{resultType.ToLower()}" +
				               "}\n" +
				               "\\end{figure}\n\n");
				string groupPath = Path.Join(outputPath, $"Plots/{group}/");
				if (!Directory.Exists(groupPath)) {
					Directory.CreateDirectory(groupPath);
				}

				File.Copy(plotFile, $"{groupPath}{resultType}.png", true);
			}
		}


		File.WriteAllText(Path.Join(outputPath, "Figures.tex"), builder.ToString());
	}

	public static void GeneratePValueTables(double threshold = 0.05) {
		foreach ((string group, List<string> values) in GetPValueGroups()) {
			var pValues = new Dictionary<BenchmarkResultType, Dictionary<string, Dictionary<string, double>>>();

			foreach (string result in values.SelectMany(str => str.Split("\n"))) {
				string baseValue = result.Split(' ')[0];
				string to = result.Split('-')[0].Trim().Split(' ')[^1].Trim();
				double value = double.Parse(result.Split(';')[1], _formatInfo);

				BenchmarkResultType resultType = result.Split('-')[1].Split(';')[0].Trim() switch {
					"Time" => BenchmarkResultType.ElapsedTime,
					"Package" => BenchmarkResultType.PackageEnergy,
					"DRAM" => BenchmarkResultType.DRAMEnergy,
					_ => throw new ArgumentOutOfRangeException(nameof(result))
				};

				if (!pValues.ContainsKey(resultType)) {
					pValues.Add(resultType, new Dictionary<string, Dictionary<string, double>>());
				}

				if (!pValues[resultType].ContainsKey(baseValue)) {
					pValues[resultType].Add(baseValue, new Dictionary<string, double>());
				}

				if (!pValues[resultType][baseValue].ContainsKey(to)) {
					pValues[resultType][baseValue].Add(to, value);
				}


				if (!pValues[resultType].ContainsKey(to)) {
					pValues[resultType].Add(to, new Dictionary<string, double>());
				}

				if (!pValues[resultType][to].ContainsKey(to)) {
					pValues[resultType][to].Add(baseValue, value);
				}
			}

			var builder = new StringBuilder();
			foreach ((BenchmarkResultType resultType, Dictionary<string, Dictionary<string, double>> valuePairs) in
				pValues) {
				builder.Append(GetResultTypePValueTable(resultType, group, valuePairs, threshold));
			}

			Directory.CreateDirectory("_Latex/Tables/PValue/");
			File.WriteAllText("_Latex/Tables/PValue/" + group.Dehumanize() + ".tex", builder.ToString());
		}
	}

	private static Dictionary<string, List<string>> GetPValueGroups() {
		var groups = new Dictionary<string, List<string>>();
		foreach (string file in Directory.EnumerateFiles("results/_pvalues/", "*.csv",
			SearchOption.AllDirectories)) {
			string group = Path.GetRelativePath(Directory.GetCurrentDirectory(), file)
				.Split(Path.DirectorySeparatorChar)[2];
			if (!groups.ContainsKey(group)) {
				groups.Add(group, new List<string>());
			}

			groups[group].Add(string.Join("\n", File.ReadAllLines(file).Skip(1)));
		}

		return groups;
	}

	private static string GetResultTypePValueTable(BenchmarkResultType resultType,
		string group,
		Dictionary<string, Dictionary<string, double>> valuePairs, double threshold) {
		StringBuilder builder = new StringBuilder();
		builder.Append(
			"\\begin{table}[H]\n\t" +
			"\\resizebox{\\textwidth}{!}{%\n\t\t" +
			"\\centering\n\t\t" +
			"\\begin{tabular}{" +
			"|c|" + string.Join("", Enumerable.Repeat("c|", valuePairs.Count)) +
			"}\n\t\t\t" +
			"\\hline\n\t\t\t");

		builder.Append("\\begin{tabular}[c]{@{}l@{}}" + resultType.Humanize(LetterCasing.Title) +
		               "\\\\ \\textit{p}-Values\\end{tabular} & " +
		               string.Join(" & ", valuePairs.Keys.Select(name => name.Humanize(LetterCasing.Title))) +
		               "\\\\ \\hline\n\t\t\t\t");
		foreach ((int index, KeyValuePair<string, Dictionary<string, double>> valuePair) in valuePairs.WithIndex()) {
			builder.Append(valuePair.Key.Humanize(LetterCasing.Title));
			foreach (string key in valuePairs.Keys) {
				builder.Append(" & ");
				if (valuePair.Value.ContainsKey(key)) {
					if (valuePair.Value[key] < threshold) {
						builder.Append("\\cellcolor[HTML]{d1f6ff}");
						builder.Append("\\textless ").Append(threshold.ToString(_provider));
					}
					else {
						builder.Append("\\cellcolor[HTML]{ffaaa8}");
						builder.Append(valuePair.Value[key].ToString("N3", _provider));
					}
				}
				else {
					builder.Append('-');
				}
			}

			builder.Append(valuePairs.Keys.Count - 1 == index ? "\\\\ \\hline\n\t\t" : "\\\\ \\hline\n\t\t\t\t");
		}

		builder.AppendLine(
			"\\end{tabular}\n\t" +
			"}\n\t" +
			"\\caption{" +
			"Table showing the \\textit{p}-values for the group " +
			$"{group.Humanize(LetterCasing.Title)} with regards to {resultType.Humanize(LetterCasing.Title)}." +
			"}\n\t" +
			"\\label{" +
			$"tab:{group}{resultType}Pvalues" +
			"}\n" +
			"\\end{table}\n");

		return builder.ToString();
	}

	public static void GenerateAverageTable(string outputPath = "_Latex/Tables/Results/") {
		Dictionary<string, List<DataSet>> groups = GenerateResultGroups();

		foreach ((string group, List<DataSet> dataSets) in groups) {
			var builder = new StringBuilder();

			builder.Append("\\begin{table}[H]\n\t" +
			               "\\resizebox{\\textwidth}{!}{%\n\t\t" +
			               "\\centering\n\t\t" +
			               "\\begin{tabular}{|c|c|c|c|}\n\t\t\t" +
			               "\\hline\n\t\t\t" +
			               "Benchmark & Time (ms) & Package Energy ($\\mu$J) & DRAM Energy ($\\mu$J) \\\\ \\hline\n");
			foreach (DataSet dataSet in dataSets) {
				builder.Append(
					$"\t\t\t\t{dataSet.Name.Humanize(LetterCasing.Title)} & {dataSet.Data.Average(result => result.ElapsedTime).ToString("N6", _provider)} & {dataSet.Data.Average(result => result.PackageEnergy).ToString("N3", _provider)} & {dataSet.Data.Average(result => result.DRAMEnergy).ToString("N3", _provider)}\\\\ \\hline\n");
			}

			builder.Append("\t\t\\end{tabular}\n\t" +
			               "}\n\t" +
			               "\\caption{" +
			               $"Table showing the elapsed time and energy measurement for each {group.Humanize(LetterCasing.Title)}." +
			               "\\label{" +
			               $"tab:{group.ToLower()}results" +
			               "}}" +
			               "\n\\end{table}\n\n");

			Directory.CreateDirectory(outputPath);
			File.WriteAllText(outputPath + group.Dehumanize() + ".tex", builder.ToString());
		}
	}

	private static Dictionary<string, List<DataSet>> GenerateResultGroups() {
		var groups = new Dictionary<string, List<DataSet>>();

		if (CsharpRAPLCLI.Options.Json) {
			foreach (string file in Helpers.GetAllJsonFilesFromPath(CsharpRAPLCLI.Options.OutputPath)) {
				string group = Path.GetRelativePath(Directory.GetCurrentDirectory(), file)
					.Split(Path.DirectorySeparatorChar)[1];
				if (!groups.ContainsKey(group)) {
					groups.Add(group, new List<DataSet>());
				}

				groups[group].Add(new DataSet(file));
			}
		}
		else {
			foreach (string file in Helpers.GetAllCSVFilesFromOutputPath()) {
				string group = Path.GetRelativePath(Directory.GetCurrentDirectory(), file)
					.Split(Path.DirectorySeparatorChar)[1];
				if (!groups.ContainsKey(group)) {
					groups.Add(group, new List<DataSet>());
				}

				groups[group].Add(new DataSet(file));
			}
		}

		return groups;
	}
}