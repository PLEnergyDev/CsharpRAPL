using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
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

	public static List<string> GetAllCSVFilesFromPath(string path, bool excludePValues = true) {
		return excludePValues
			? GetAllFilesFromPath(path, path).Where(s => !s.Contains("_pvalues")).ToList()
			: GetAllFilesFromPath(path, path).ToList();
	}

	public static List<string> GetAllJsonFilesFromPath(string path) {
		return GetAllFilesFromPath(path, "*.json").ToList();
	}

	public static List<string> GetAllFilesFromPath(string path, string pattern) {
		return !Directory.Exists(path)
			? new List<string>()
			: Directory.EnumerateFiles(path, pattern, SearchOption.AllDirectories).ToList();
	}

	public static List<string> GetAllPlotFiles() {
		if (!Directory.Exists(CsharpRAPLCLI.Options.PlotOutputPath)) {
			return new List<string>();
		}

		return Directory.EnumerateFiles(CsharpRAPLCLI.Options.PlotOutputPath, "*.png", SearchOption.AllDirectories)
			.ToList();
	}

	/// <summary>
	/// Checks if a method is anonymous.
	/// </summary>
	/// <param name="method">The method we want to check.</param>
	/// <returns>Returns true if it is anonymous otherwise false</returns>
	public static bool IsAnonymous(this MemberInfo method) {
		var invalidChars = new[] { '<', '>' };
		return method.Name.Any(invalidChars.Contains);
	}
}