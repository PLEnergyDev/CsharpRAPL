using System;
using System.Diagnostics;
using System.IO;
using Accord.Math;
using SocketComm;

namespace CsharpRAPL.Benchmarking.Lifecycles; 

public class CState : IpcState {
	public CState(string pipe) : base(pipe) { }

	public string CFile { get; set; }
	public string HeaderFile { get; set; }
	public string LibPath { get; set; }

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

	public override IpcState Generate() {
		string[] filesToCopy = {"cmd.c","cmd.h","scomm.c","scomm.h", CFile, HeaderFile };
		var dt = DateTime.Now;
		var dir= Directory.CreateDirectory($"tmp/CBench-{BenchmarkSignature}-{dt.ToString("s").Replace(":", "-")}-{dt.Millisecond}");
		foreach (var f in filesToCopy) {
			File.Copy(LibPath + "/"+ f,dir.FullName +"/"+f);
		}
		var main = File.ReadAllLines(LibPath + "/main.c");
		var includeline = main.First(s => s.Contains("///Includes here"));
		var benchmarkline = main.First(s => s.Contains("///Compute benchmark here"));
		main[benchmarkline] = BenchmarkSignature;
		main[includeline] = $"#include \"{HeaderFile}\"";
		File.WriteAllLines(dir.FullName + "/main.c", main);
		var compile = new ProcessStartInfo("CompileCBenchmarks.sh");
		compile.Arguments = dir.FullName + " " + CFile;
		var compP = Process.Start(compile);
		compP?.WaitForExit();
		ExecutablePath = dir.FullName + "/CBench";
		return this;
	}
	
}