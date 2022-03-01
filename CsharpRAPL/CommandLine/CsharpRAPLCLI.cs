using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Plotting;

namespace CsharpRAPL.CommandLine;

public static class CsharpRAPLCLI {
	public static Options Options { get; private set; } = new();

	private static Action<Analysis.Analysis> _analysisCallback = Analyse;

	private static Action<string>? _plotCallback;


	public static Options Parse(IEnumerable<string> args, int maximumTerminalWidth = 0) {
		var parser = new Parser(settings => {
			settings.CaseSensitive = false;
			settings.HelpWriter = Console.Out;
			if (maximumTerminalWidth != 0) {
				settings.MaximumDisplayWidth = maximumTerminalWidth;
			}
		});
		parser.ParseArguments<Options>(args).WithParsed(RunOptions).WithNotParsed(HandleParseError);

		if (Options.OnlyAnalysis) {
			StartAnalysis(Options.BenchmarksToAnalyse.ToArray());
			Exit();
			return Options;
		}

		return Options;
	}

	private static void RunOptions(Options opts) {
		Options = opts;
		Options.OutputPath = Options.OutputPath.Replace("\\", "/");
		if (!Options.OutputPath.EndsWith("/")) {
			Options.OutputPath += "/";
		}

		Options.PlotOutputPath = Options.PlotOutputPath.Replace("\\", "/");
		if (!Options.PlotOutputPath.EndsWith("/")) {
			Options.PlotOutputPath += "/";
		}

		if (!Options.Json && Options.CollectMemoryInformation) {
			Console.Error.WriteLine("Memory information can only be collected/saved when using JSON output.");
		}

		if (Options.OnlyPlot) {
			if (_plotCallback == null) {
				BenchmarkPlot.PlotAllResultsGroupsFromFolder(Options.OutputPath);
			}
			else {
				_plotCallback.Invoke(Options.OutputPath);
			}

			Exit();
		}

		if (Options.KeepOldResults) {
			return;
		}

		if (Directory.Exists(Options.PlotOutputPath)) {
			Directory.Delete(Options.PlotOutputPath, true);
		}

		if (Directory.Exists(Options.OutputPath)) {
			Directory.Delete(Options.OutputPath, true);
		}
	}

	private static void HandleParseError(IEnumerable<Error> errs) {
		foreach (Error error in errs) {
			if (error.Tag is ErrorType.HelpRequestedError or ErrorType.VersionRequestedError) {
				Exit();
			}
			else {
				throw new NotSupportedException(ParseError(error));
			}
		}
	}

	public static void StartAnalysis(Dictionary<string, List<IBenchmark>> benchmarksWithGroups) {
		foreach (string group in benchmarksWithGroups.Keys) {
			if (benchmarksWithGroups[group].Count < 2) {
				Console.WriteLine($"Not enough benchmarks in the group {group} minimum of two is required. Skipping.");
				continue;
			}

			for (var i = 0; i < benchmarksWithGroups[group].Count; i++) {
				for (int j = i + 1; j < benchmarksWithGroups[group].Count; j++) {
					var analysis =
						new Analysis.Analysis(benchmarksWithGroups[group][i], benchmarksWithGroups[group][j]);
					_analysisCallback.Invoke(analysis);
				}
			}
		}
	}


	public static void StartAnalysis(string[] thingsToAnalyse) {
		if (thingsToAnalyse.Length == 0) {
			throw new NotSupportedException("You have to pass at least two benchmarks to analyse.");
		}

		if (thingsToAnalyse.Length % 2 != 0) {
			throw new NotSupportedException("You need to pass an even number of benchmarks to analyse.");
		}

		foreach (string[] chunk in thingsToAnalyse.Chunk(2)) {
			string firstPath = chunk[0];
			string secondPath = chunk[1];

			if (!firstPath.EndsWith(".csv")) {
				firstPath = GetMostRecentFile(firstPath);
			}

			if (!secondPath.EndsWith(".csv")) {
				secondPath = GetMostRecentFile(secondPath);
			}

			firstPath = firstPath.Replace("\\", "/");
			secondPath = secondPath.Replace("\\", "/");

			if (!firstPath.Contains(Options.OutputPath)) {
				firstPath = $"{Options.OutputPath}/{firstPath}";
			}

			if (!secondPath.Contains(Options.OutputPath)) {
				secondPath = $"{Options.OutputPath}/{secondPath}";
			}

			var analysis = new Analysis.Analysis(firstPath, secondPath);
			_analysisCallback.Invoke(analysis);
		}
	}

	public static void SetAnalysisCallback(Action<Analysis.Analysis> action) {
		_analysisCallback = action;
	}

	public static void SetPlottingCallback(Action<string> action) {
		_plotCallback = action;
	}

	private static void Analyse(Analysis.Analysis analysis) {
		(bool isValid, string? message) = analysis.EnsureResultsMutually();
		if (!isValid) {
			Console.Error.WriteLine(message);
		}
		else {
			Console.WriteLine("Results are mutually ensured!");
		}

		List<(string Message, double Value)> pValues = analysis.CalculatePValue();


		// Print the p-values to console
		// The lower the p-value the higher the chance for that statement to be correct
		// P-value means the chance of the null hypothesis to be true
		Console.WriteLine("Is the null hypothesis true? I.e. is the opposite of what the key implies true?");
		foreach ((string name, double value) in pValues) {
			Console.WriteLine($"{name}:{value}");
		}

		Console.WriteLine("Is the alternate hypothesis true? I.e. is what the key implies true?");
		foreach ((string name, double value) in pValues) {
			Console.WriteLine($"{name}:{1 - value}");
		}
	}

	private static string GetMostRecentFile(string path) {
		const string pattern = "*.csv";
		var dirInfo = new DirectoryInfo($"{Options.OutputPath}/{path}");
		FileInfo file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f)
			.First();
		return $"{path}/{file.Name}";
	}

	private static string ParseError(Error error) {
		return error switch {
			BadFormatTokenError badFormatTokenError =>
				$"Token '{badFormatTokenError.Token}' is not recognized.",

			MissingValueOptionError missingValueOptionError =>
				$"Option '{missingValueOptionError.NameInfo.NameText}' has no value.",

			UnknownOptionError unknownOptionError =>
				$"Option '{unknownOptionError.Token}' is unknown.",

			MissingRequiredOptionError missingRequiredOptionError =>
				missingRequiredOptionError.NameInfo.Equals(NameInfo.EmptyName)
					? "A required value not bound to option name is missing."
					: $"Required option '{missingRequiredOptionError.NameInfo.NameText}' is missing.",

			BadFormatConversionError badFormatConversionError =>
				badFormatConversionError.NameInfo.Equals(NameInfo.EmptyName)
					? "A value not bound to option name is defined with a bad format."
					: $"Option '{badFormatConversionError.NameInfo.NameText + "' is defined with a bad format."}",

			InvalidAttributeConfigurationError =>
				"Invalid Attribute Configuration",

			MissingGroupOptionError missingGroupOptionError =>
				$"Missing Group {missingGroupOptionError.Group} {string.Join(", ", missingGroupOptionError.Names)}",

			SequenceOutOfRangeError sequenceOutOfRangeError =>
				sequenceOutOfRangeError.NameInfo.Equals(NameInfo.EmptyName)
					? "A sequence value not bound to option name is defined with few items than required."
					: $"A sequence option '{sequenceOutOfRangeError.NameInfo.NameText}' is defined with fewer or more items than required.",

			BadVerbSelectedError badVerbSelectedError =>
				$"Verb '{badVerbSelectedError.Token}' is not recognized.",

			GroupOptionAmbiguityError groupOptionAmbiguityError =>
				$"Group Option Ambiguity Error{groupOptionAmbiguityError.NameInfo} {groupOptionAmbiguityError.Option}",
			NoVerbSelectedError =>
				"No verb selected.",

			RepeatedOptionError repeatedOptionError =>
				$"Option '{repeatedOptionError.NameInfo.NameText}' is defined multiple times.",
			_ => throw new ArgumentOutOfRangeException($"{error.Tag} is out of range.")
		};
	}

	private static void Exit() {
		if (Options.ShouldExit) {
			Environment.Exit(0);
		}
	}
}