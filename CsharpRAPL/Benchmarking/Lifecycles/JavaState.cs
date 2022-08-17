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
		string[] filesToCopy = { "JavaRun/Cmd.java", "JavaRun/FPipe.java","JavaRun/PipeCmdException.java", "MANIFEST.MF" };
		var dt = DateTime.Now;
		var dir = Directory.CreateDirectory(
			$"tmp/JavaBench/{BenchmarkSignature}-{dt.ToString("s").Replace(":", "-")}-{dt.Millisecond}");
		Directory.CreateDirectory(dir.FullName + "/JavaRun");
		Directory.CreateDirectory(dir.FullName + "/out");
		foreach (var f in filesToCopy) {
			File.Copy($"{LibPath}/{f}",$"{dir.FullName}/{f}");
		}
		
		// Write main file
		var main = File.ReadAllLines(LibPath + "/JavaRun/Main.java").ToList();
		var benchmarkLine = main.FindIndex(s => s.Contains("///Compute benchmark here"));
		main[benchmarkLine] = BenchmarkSignature;
		File.WriteAllLines(dir.FullName + "/JavaRun/Main.java", main);
		
		// Write benchmark file
		var bench = File.ReadAllLines($"{LibPath}/{JavaFile}").ToList();
		var packageLine = bench.FindIndex(s => s.StartsWith("package"));
		if (packageLine <= 0) {
			bench[packageLine] = "package JavaRun;";
		}
		else {
			bench.Insert(0, "package JavaRun;");
		}
		File.WriteAllLines($"{dir.FullName}/{JavaFile}", bench);
		
		//Run compile script
		var compile = new ProcessStartInfo("/bin/bash") {
			Arguments = "CompileJavaBenchmarks.sh " + dir.FullName + " \"" + AdditionalCompilerOptions + "\"",
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