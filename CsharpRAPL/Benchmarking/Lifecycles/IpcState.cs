using System;
using System.Diagnostics;
using System.IO;
using SocketComm;

namespace CsharpRAPL.Benchmarking.Lifecycles; 

public class IpcState {
	public string PipePath { get; }
	public IBenchmark Benchmark { get; }
	public FPipe Pipe { get; private set; }
	public bool HasRun = false;
	public bool KeepCompilationResults = false;

	protected string ExecutablePath { get; set; } = "";
	protected string CompilationPath { get; set; } = "";
	
	protected virtual IpcState Generate() => this;

	public IpcState(string pipe, IBenchmark benchmark) {
		Benchmark = benchmark;
		PipePath = pipe;
		Pipe = new FPipe(PipePath);
		Pipe.Listening += OnPipeListening;
	}

	protected virtual void OnPipeListening(object? sender, EventArgs e) {
		try {
			Generate();

			var serverProcInfo = new ProcessStartInfo {
				FileName = ExecutablePath,
				WorkingDirectory = CompilationPath,
				Arguments = PipePath,
				CreateNoWindow = true,
				UseShellExecute = false
			};

			using (var proc = new Process { StartInfo = serverProcInfo }) {
				proc.EnableRaisingEvents = true;
				proc.Exited += (sender, args) => {
					if (proc.ExitCode == 0) {
						if (!KeepCompilationResults && Directory.Exists(CompilationPath)) {
							Directory.Delete(CompilationPath, true);
						}	
					} else {
						Benchmark.ResetBenchmark = true;
					}
				};

				proc.Start();
			}
		} catch (Exception){
			if (File.Exists(PipePath)) {
				File.Delete(PipePath);
			}
			throw;
		}
	}
}
