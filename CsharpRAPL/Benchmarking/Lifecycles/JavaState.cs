using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CsharpRAPL.Benchmarking.Lifecycles; 

public class JavaState : IpcState {
	public JavaState(IpcState state) : base(state.PipePath, state.Benchmark) { }

	public string JavaFile { get; set; }
	public string LibPath { get; set; }
	public string BenchmarkSignature { get; set; }
	public string? AdditionalCompilerOptions { get; set; }
	public string? JavaPath { get; set; }

	private List<string> _libs = new List<string> { "JSocketComm.jar" };
		
	private string _sendSignature;

	/// <summary>
	/// Java code for sending benchmark result to benchmarkrunner 
	/// </summary>
	public string SendSignature {
		get => _sendSignature;
		set {
			_sendSignature = value;
			if (!value.EndsWith(";")) {
				_sendSignature += ";";
			}
		}
	}
	
	/// <summary>
	/// List of library files relative to LibPath/lib to link in compilation
	/// </summary>
	public List<string> Libs {
		get => _libs;
		set {
			var result = new List<string>();
			result.Add("JSocketComm.jar");
			result.AddRange(value);
			_libs = result;
		}
	}

	protected override IpcState Generate() {
		//Create directory and copy lib
		string[] filesToCopy = _libs.ToArray();
		var dt = DateTime.Now;
		var dir = Directory.CreateDirectory(
			$"tmp/JavaBench/{BenchmarkSignature}-{dt.ToString("s").Replace(":", "-")}-{dt.Millisecond}");
		CompilationPath = dir.FullName;
		foreach (var f in filesToCopy) {
			File.Copy($"{LibPath}/{f}",$"{dir.FullName}/{f}");
		}
		
		// Write main file
		var main = File.ReadAllLines(LibPath + "/Main.java").ToList();
		var benchmarkLine = main.FindIndex(s => s.Contains("///Compute benchmark here"));
		var sendLine = main.FindIndex(s => s.Contains("///Send return value here"));
		main[benchmarkLine] = BenchmarkSignature;
		main[sendLine] = SendSignature;
		File.WriteAllLines(dir.FullName + "/Main.java", main);
		
		// Write benchmark file
		//var bench = File.ReadAllLines($"{LibPath}/{JavaFile}").ToList();
		//File.WriteAllLines($"{dir.FullName}/{JavaFile}", bench);
		File.Copy($"{LibPath}/{JavaFile}",$"{dir.FullName}/{JavaFile}");
		
		// Write MANIFEST file
		var manifest = new List<string> { "Main-Class: Main" };
		var cp = "Class-Path: ";
		foreach (var lib in Libs) {
			cp += lib + " ";
		}
		manifest.Add(cp);
		//manifest files need to end in blank line
		manifest.Add("");
		
		File.WriteAllLines(dir.FullName + "/MANIFEST.MF", manifest);
			

		JavaPath ??= "java";
		//Run compile script
		var classPath = Libs[0];
		foreach (var lib in Libs.Skip(1)) {
			classPath += ":" + lib;
		}

		var args = "\"" + dir.FullName + "\" " +
		           JavaPath + " " +
		           "\"-cp " + classPath + "\" " +
		           "\"" + AdditionalCompilerOptions + "\"";
		           

		var compile = new ProcessStartInfo("CompileJavaBenchmarks.sh") {
			Arguments = args,
			UseShellExecute = true,
			CreateNoWindow = true
		};
		var compP = Process.Start(compile);
		compP?.WaitForExit();
		if (compP!.ExitCode != 0) {
			throw new InvalidOperationException($"Compilation for {JavaFile} failed!");
		}
		ExecutablePath = dir.FullName + "/JavaBench.sh";
		return this;
	}
}