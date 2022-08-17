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


	public IpcBenchmarkLifecycle(BenchmarkInfo benchmarkInfo, MethodInfo benchmarkedMethod)
	{
		BenchmarkInfo = benchmarkInfo;
		BenchmarkedMethod = benchmarkedMethod;
	}
	public IpcState Initialize(IBenchmark benchmark) {
		var file = "/tmp/" + BenchmarkedMethod.Name + ".pipe";
		var state = new IpcState(file);
		
		//Get benchmark information
		state = (IpcState)BenchmarkedMethod.Invoke(null, new object?[]{state})!;
		
		//Open pipe for connection
		state.Pipe.Connect();
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
		oldstate.Pipe.WriteCmd(oldstate.HasRun ? Cmd.Done : Cmd.Ready);
		return oldstate;
	}

	public IpcState AdjustLoopIterations(IpcState oldstate) {
		//TODO: scale over IPC
		//BenchmarkInfo.LoopIterations = 10;
		return oldstate;
	}

	public IpcState End(IpcState oldstate) {
		oldstate.HasRun = true;
		return oldstate;
	}
}