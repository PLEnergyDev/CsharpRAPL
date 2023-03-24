using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Accord.Math;
using SocketComm;

namespace CsharpRAPL.Benchmarking.Lifecycles; 

public class CState : IpcState {
	public CState(IpcState state) : base(state.PipePath, state.Benchmark) { }

	public string CFile { get; set; }
	public string HeaderFile { get; set; }
	public string LibPath { get; set; }
	private List<string> _libs = new List<string>{"libSocketComm.a"};
	private List<string> _includes = new List<string>{"SocketComm"};
	public List<string> Libs {
		get => _libs;
		set {
		var result = new List<string>();
		result.Add("libSocketComm.a");
		result.AddRange(value);
		_libs = result;
		}
	}

	public List<string> Includes {
		get => _includes;
		set {
			var result = new List<string>();
			result.Add("SocketComm");
			result.AddRange(value);
			_includes = result;
		}
	}
	
	public string? AdditionalCompilerOptions { get; set; }

	private string _bencmarkSignature;
	public string BenchmarkSignature {
		get {
			return _bencmarkSignature;
		}
		set {
			_bencmarkSignature = value;
			if (!value.EndsWith(";")) {
				_bencmarkSignature += ";";
			}
		}
	}

	private string _sendSignature;

	public string SendSignature {
		get => _sendSignature;
		set {
			_sendSignature = value;
			if (!value.EndsWith(";")) {
				_sendSignature += ";";
			}
		}
	}

	protected override IpcState Generate() {
		//Create directories and copy lib
		string[] filesToCopy = {CFile, HeaderFile };
		var dt = DateTime.Now;
		var dir= Directory.CreateDirectory($"tmp/CBench/{BenchmarkSignature}-{dt.ToString("s").Replace(":", "-")}-{dt.Millisecond}");
		CompilationPath = dir.FullName;
		foreach (var f in filesToCopy) {
			File.Copy($"{LibPath}/{f}",$"{dir.FullName}/{f}");
		}
		
		//Add includes and libraries to compiler options
		var libs = Path.GetFullPath(LibPath + "/lib");
		var includes = Path.GetFullPath(LibPath + "/include");
		AdditionalCompilerOptions = $"-L{libs} " + AdditionalCompilerOptions;
		foreach (var include in Includes) {
			AdditionalCompilerOptions = $"-I{includes}/{include} " + AdditionalCompilerOptions;
		}
		foreach (var lib in Libs) {
			AdditionalCompilerOptions += $" -l:{lib}";
		}

		//Write main file
		var mainFile = File.ReadAllLines(LibPath + "/main.c");

		var includeLine = mainFile.First(s => s.Contains("///Includes here"));
		var benchmarkLine = mainFile.First(s => s.Contains("///Compute benchmark here"));
		var sendLine = mainFile.First(s => s.Contains("///Send return value here"));
		mainFile[benchmarkLine] = BenchmarkSignature;
		mainFile[includeLine] = $"#include \"{HeaderFile}\"";
		mainFile[sendLine] = SendSignature;
		File.WriteAllLines(dir.FullName + "/main.c", mainFile);
		
		//Run compile script
		var compile = new ProcessStartInfo("CompileCBenchmarks.sh");
		compile.Arguments = $"\"{dir.FullName}\" {CFile} \"{AdditionalCompilerOptions}\"";
		compile.CreateNoWindow = true;
		compile.UseShellExecute = true;
		var compP = Process.Start(compile);
		compP?.WaitForExit();
		if (compP!.ExitCode != 0) {
			throw new InvalidOperationException($"Compilation for {CFile} failed!");
		}
		ExecutablePath = dir.FullName + "/CBench";
		return this;
	}
	
}