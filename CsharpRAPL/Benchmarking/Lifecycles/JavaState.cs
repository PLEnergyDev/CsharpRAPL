using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CsharpRAPL.Benchmarking.Lifecycles; 

public class JavaState : IpcState {
	public JavaState(IpcState state) : base(state.PipePath, state.Benchmark) { }

	public string RootPath { get; set; }
	public string BenchmarkPath { get; set; }
	public string? AdditionalCompilerOptions { get; set; }

	private string _benchmarkSignature;
	// private string _sendSignature;

	public string BenchmarkSignature {
		get => _benchmarkSignature;
		set {
	        if (value == null) throw new ArgumentNullException(nameof(value));
	        _benchmarkSignature = value.EndsWith(";") ? value : value + ";";
		}
	}

	/// <summary>
	/// Java code for sending benchmark result to benchmarkrunner
	/// </summary>
	// public string SendSignature {
	// 	get => _sendSignature;
	// 	set {
	//         if (value == null) throw new ArgumentNullException(nameof(value));
	//         _sendSignature = value.EndsWith(";") ? value : value + ";";
	// 	}
	// }

	private void ReplaceLine(ref string[] fileLines, string lineIdentifier, string newLineContent) {
	    var lineIndex = Array.FindIndex(fileLines, line => line.Contains(lineIdentifier));
	    if (lineIndex != -1) {
	        fileLines[lineIndex] = newLineContent;
	    }
	}

	protected override IpcState Generate() {
		var now = DateTime.Now;
		var safeDateTime = now.ToString("yyyyMMdd-HHmmss-fff");

		var sourceLibPath = Path.Combine(RootPath, "lib");
		var sourceScriptPath = Path.Combine(RootPath, "CompileBenchmark.sh");
		var sourceBenchmarkPath = Path.Combine(RootPath, BenchmarkPath);

		var destinationPath = $"tmp/{RootPath}/{BenchmarkSignature}-{safeDateTime}";
		Directory.CreateDirectory(destinationPath);
		var destinationLibPath = Path.Combine(destinationPath, "lib");
		Directory.CreateDirectory(destinationLibPath);
		var destinationScriptPath = Path.Combine(destinationPath, "CompileBenchmark.sh");

		var libFiles = Directory.GetFiles(sourceLibPath);
		var benchmarkFiles = Directory.GetFiles(sourceBenchmarkPath);
		var filesToCopy = libFiles.Concat(benchmarkFiles);
		foreach (var file in filesToCopy) {
			var destPath = Path.Combine(destinationLibPath, Path.GetFileName(file));
			File.Copy(file, destPath, true);
		}

		File.Copy(sourceScriptPath, destinationScriptPath);

		var mainFilePath = Path.Combine(destinationLibPath, "Main.java");
		var mainFile = File.ReadAllLines(mainFilePath);

	    ReplaceLine(ref mainFile, "///[BENCHMARK]", BenchmarkSignature);
	    // ReplaceLine(ref mainFile, "///[RESULT]", SendSignature);

	    File.WriteAllLines(mainFilePath, mainFile);
		
	    var compileProc = new ProcessStartInfo() {
	    	FileName = destinationScriptPath,
	        WorkingDirectory = destinationPath,
	        Arguments = AdditionalCompilerOptions,
	        CreateNoWindow = true,
	        UseShellExecute = false,
	    };

	    using (var proc = Process.Start(compileProc)) {
	        proc.WaitForExit();
		    if (proc == null || proc.ExitCode != 0) {
		        throw new InvalidOperationException($"[ERROR] Compilation failed!");
		    }
	    }

		CompilationPath = destinationPath;
	    ExecutablePath = Path.Combine(destinationPath, "JavaBench");

	    return this;
	}
}