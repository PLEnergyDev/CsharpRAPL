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

	private static Action<Analysis.Analysis> _analysis = Analyse;

	private static Action<string>? _plot;


	public static Options Parse(string[] args, int maximumTerminalWidth = 0) {
		var parser = new Parser(settings => {
			settings.CaseSensitive = false;
			settings.HelpWriter = Console.Out;
			if (maximumTerminalWidth != 0) {
				settings.MaximumDisplayWidth = maximumTerminalWidth;
			}
		});
		parser.ParseArguments<Options>(args).WithParsed(RunOptions).WithNotParsed(HandleParseError);

		if (Options.OnlyPlot) {
			if (_plot == null) {
				BenchmarkPlot.PlotAllResultsGroupsFromFolder(Options.OutputPath);
			}
			else {
				_plot.Invoke(Options.OutputPath);
			}

			Options.ShouldExit = true;
			return Options;
		}

		if (Options.OnlyAnalysis) {
			StartAnalysis(Options.BenchmarksToAnalyse.ToArray());
			Options.ShouldExit = true;
		}

		return Options;
	}

	private static void RunOptions(Options opts) {
		Options = opts;
		opts.OutputPath = opts.OutputPath.Replace("\\", "/");
		if (!opts.OutputPath.EndsWith("/")) {
			opts.OutputPath += "/";
		}

		opts.PlotOutputPath = opts.PlotOutputPath.Replace("\\", "/");
		if (!opts.PlotOutputPath.EndsWith("/")) {
			opts.PlotOutputPath += "/";
		}

		if (!Options.RemoveOldResults) {
			return;
		}

		if (Directory.Exists(opts.PlotOutputPath)) {
			Directory.Delete(opts.PlotOutputPath, true);
		}

		if (Directory.Exists(opts.OutputPath)) {
			Directory.Delete(opts.OutputPath, true);
		}
	}

	private static void HandleParseError(IEnumerable<Error> errs) {
		foreach (Error error in errs) {
			if (error.Tag is ErrorType.HelpRequestedError or ErrorType.VersionRequestedError) {
				Options.ShouldExit = true;
			}
			else {
				throw new NotSupportedException(ParseError(error));
			}
		}
	}

	public static void StartAnalysis(IReadOnlyDictionary<string, List<IBenchmark>> benchmarksWithGroups) {
		foreach (string group in benchmarksWithGroups.Keys) {
			for (int i = 0; i < benchmarksWithGroups.Count; i++) {
				for (int j = i + 1; j < benchmarksWithGroups.Count; j++) {
					var analysis =
						new Analysis.Analysis(benchmarksWithGroups[group][i], benchmarksWithGroups[group][j]);
					_analysis.Invoke(analysis);
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
			_analysis.Invoke(analysis);
		}
	}

	public static void SetAnalysis(Action<Analysis.Analysis> action) {
		_analysis = action;
	}

	public static void SetPlotting(Action<string> action) {
		_plot = action;
	}

	private static void Analyse(Analysis.Analysis analysis) {
		(bool isValid, string? message) = analysis.EnsureResultsMutually();
		if (!isValid) {
			Console.Error.WriteLine(message);
		}
		else {
			Console.WriteLine("Results are mutually ensured!");
		}

		Dictionary<string, double> pValues = analysis.CalculatePValue();


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
		switch (error) {
			case BadFormatTokenError badFormatTokenError:
				return $"Token '{badFormatTokenError.Token}' is not recognized.";
			case MissingValueOptionError missingValueOptionError:
				return $"Option '{missingValueOptionError.NameInfo.NameText}' has no value.";
			case UnknownOptionError unknownOptionError:
				return $"Option '{unknownOptionError.Token}' is unknown.";
			case MissingRequiredOptionError missingRequiredOptionError:
				return missingRequiredOptionError.NameInfo.Equals(NameInfo.EmptyName)
					? "A required value not bound to option name is missing."
					: $"Required option '{missingRequiredOptionError.NameInfo.NameText}' is missing.";
			case BadFormatConversionError badFormatConversionError:
				return badFormatConversionError.NameInfo.Equals(NameInfo.EmptyName)
					? "A value not bound to option name is defined with a bad format."
					: $"Option '{badFormatConversionError.NameInfo.NameText + "' is defined with a bad format."}";
			case InvalidAttributeConfigurationError:
				return "Invalid Attribute Configuration";
			case MissingGroupOptionError missingGroupOptionError:
				return
					$"Missing Group {missingGroupOptionError.Group} {string.Join(", ", missingGroupOptionError.Names)}";
			case SequenceOutOfRangeError sequenceOutOfRangeError:
				return sequenceOutOfRangeError.NameInfo.Equals(NameInfo.EmptyName)
					? "A sequence value not bound to option name is defined with few items than required."
					: $"A sequence option '{sequenceOutOfRangeError.NameInfo.NameText}' is defined with fewer or more items than required.";
			case BadVerbSelectedError badVerbSelectedError:
				return $"Verb '{badVerbSelectedError.Token}' is not recognized.";
			case GroupOptionAmbiguityError groupOptionAmbiguityError:
				return
					$"Group Option Ambiguity Error{groupOptionAmbiguityError.NameInfo} {groupOptionAmbiguityError.Option}";
			case NoVerbSelectedError:
				return "No verb selected.";
			case RepeatedOptionError repeatedOptionError:
				return $"Option '{repeatedOptionError.NameInfo.NameText}' is defined multiple times.";
		}

		throw new ArgumentOutOfRangeException($"{error.Tag} is out of range.");
	}
}