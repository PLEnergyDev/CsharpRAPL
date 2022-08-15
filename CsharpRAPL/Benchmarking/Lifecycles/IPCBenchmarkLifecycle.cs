using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using SocketComm;

namespace CsharpRAPL.Benchmarking.Lifecycles; 

public class IpcBenchmarkLifecycle : IBenchmarkLifecycle<IpcState> {
	public MethodInfo BenchmarkedMethod { get; }
	public BenchmarkInfo BenchmarkInfo { get; }
	public string ExePath { get; }

	public IpcBenchmarkLifecycle(BenchmarkInfo benchmarkInfo, MethodInfo benchmarkedMethod, string exePath)
	{
		BenchmarkInfo = benchmarkInfo;
		BenchmarkedMethod = benchmarkedMethod;
		ExePath = exePath;
	}
	public IpcState Initialize(IBenchmark benchmark) {
		var file = "/tmp/" + BenchmarkedMethod.Name + ".pipe";
		// Open pipe server
		var s = Task.Run(() => new IpcState(new FPipe(file)));


		// Start pipe client
		ProcessStartInfo startinfo;
		//TODO: makeshift implementation. Should be dynamic via attributes
		startinfo = new ProcessStartInfo(ExePath) {
			UseShellExecute = true
		};
		startinfo.Arguments += file;
		Process.Start(startinfo);
		IpcState state;
		
		// await return of connection
		state = s.Result;
		state.Pipe.ExpectCmd(Cmd.Ready);
		return state;
	}

	public IpcState WarmupIteration(IpcState oldstate) {
		oldstate.Pipe.ExpectCmd(Cmd.Ready);
		oldstate.Pipe.WriteCmd(Cmd.Go);
		oldstate.Pipe.ExpectCmd(Cmd.Done);
		oldstate.Pipe.WriteCmd(Cmd.Ready);
		return oldstate;
	}

	public IpcState PreRun(IpcState oldstate) {
		oldstate.Pipe.ExpectCmd(Cmd.Ready);
		return oldstate;
	}

	public object Run(IpcState state) {
		state.Pipe.WriteCmd(Cmd.Go);
		state.Pipe.ExpectCmd(Cmd.Done);
		return state;
	}

	public IpcState PostRun(IpcState oldstate) {
		oldstate.Pipe.WriteCmd(oldstate.Hasrun ? Cmd.Done : Cmd.Ready);
		return oldstate;
	}

	public IpcState AdjustLoopIterations(IpcState oldstate) {
		//TODO: scale over IPC
		//BenchmarkInfo.LoopIterations = 10;
		return oldstate;
	}

	public IpcState End(IpcState oldstate) {
		oldstate.Hasrun = true;
		return oldstate;
	}
}