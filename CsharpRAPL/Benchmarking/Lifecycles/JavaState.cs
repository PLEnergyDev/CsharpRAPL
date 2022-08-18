using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CsharpRAPL.Benchmarking.Lifecycles; 

public class JavaState : IpcState {
	public JavaState(string pipe) : base(pipe) { }

	public string JavaFile { get; set; }
	public string LibPath { get; set; }
	public string BenchmarkSignature { get; set; }
	public string? AdditionalCompilerOptions { get; set; }

	public override IpcState Generate() {
		//Create directory and copy lib
		string[] filesToCopy = { "Cmd.java", "FPipe.java","PipeCmdException.java", "MANIFEST.MF" };
		var dt = DateTime.Now;
		var dir = Directory.CreateDirectory(
			$"tmp/JavaBench/{BenchmarkSignature}-{dt.ToString("s").Replace(":", "-")}-{dt.Millisecond}");
		foreach (var f in filesToCopy) {
			File.Copy($"{LibPath}/{f}",$"{dir.FullName}/{f}");
		}
		
		// Write main file
		var main = File.ReadAllLines(LibPath + "/Main.java").ToList();
		var benchmarkLine = main.FindIndex(s => s.Contains("///Compute benchmark here"));
		main[benchmarkLine] = BenchmarkSignature;
		File.WriteAllLines(dir.FullName + "/Main.java", main);
		
		// Write benchmark file
		var bench = File.ReadAllLines($"{LibPath}/{JavaFile}").ToList();
		File.WriteAllLines($"{dir.FullName}/{JavaFile}", bench);
		
		//Run compile script
		var compile = new ProcessStartInfo("CompileJavaBenchmarks.sh") {
			Arguments =  dir.FullName + " \"" + AdditionalCompilerOptions + "\"",
			UseShellExecute = true,
			CreateNoWindow = true
		};
		var compP = Process.Start(compile);
		compP?.WaitForExit();
		compP.Dispose();
		ExecutablePath = dir.FullName + "/JavaBench.sh";
		return this;
	}
}