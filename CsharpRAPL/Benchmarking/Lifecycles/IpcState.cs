using System;
using System.Diagnostics;
using System.IO;
using SocketComm;

namespace CsharpRAPL.Benchmarking.Lifecycles; 

public class IpcState {
	public string PipePath { get; }
	public IpcState(string pipe, IBenchmark benchmark) {
		Benchmark = benchmark;
		PipePath = pipe;
		Pipe = new FPipe(PipePath);
		Pipe.Listening += OnPipeListening;
	}

	public string ExecutablePath { get; set; } = "";
	
	public FPipe Pipe { get; private set; }
	public bool HasRun = false;
	private Process _runBenchmark = new ();
	public IBenchmark Benchmark { get; }
	protected string CompilationPath { get; set; }
	public bool KeepCompilationResults = false;

	public Func<IpcState,IpcState> prerun = s => s;

	public Func<IpcState,IpcState> postrun = s => s;

	protected virtual IpcState Generate() => this;

	protected virtual void OnPipeListening(object? sender, EventArgs e) {
		try {
			Generate();
			// Start pipe client
			ProcessStartInfo startinfo;
			startinfo = new ProcessStartInfo(ExecutablePath) {
				UseShellExecute = true,
			};
			startinfo.Arguments += PipePath;
			_runBenchmark.StartInfo = startinfo;
			_runBenchmark.EnableRaisingEvents = true;
			_runBenchmark.Exited += OnProcessExit;
			_runBenchmark.Start();
		}
		catch (Exception ex){
			if (File.Exists(PipePath)) {
				File.Delete(PipePath);
			}
			throw;
		}
	}

	private void OnProcessExit(object? sender, EventArgs e) {
		if (_runBenchmark.ExitCode == 0 ) {
			if (!KeepCompilationResults && Directory.Exists(CompilationPath)) {
				Directory.Delete(CompilationPath, true);
			}
		}
		else {
			Benchmark.ResetBenchmark = true;
		}
		
	}
}