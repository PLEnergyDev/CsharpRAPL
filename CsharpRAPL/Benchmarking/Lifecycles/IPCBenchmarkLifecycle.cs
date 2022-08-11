using System;
using System.Diagnostics;
using System.Reflection;
using CsvHelper;
using SocketComm;

namespace CsharpRAPL.Benchmarking.Lifecycles; 

public class IpcBenchmarkLifecycle : IBenchmarkLifecycle<FPipe> {
	public MethodInfo BenchmarkedMethod { get; }
	public BenchmarkInfo BenchmarkInfo { get; }

	public IpcBenchmarkLifecycle(BenchmarkInfo benchmarkInfo, MethodInfo benchmarkedMethod)
	{
		BenchmarkInfo = benchmarkInfo;
		BenchmarkedMethod = benchmarkedMethod;
	}
	public FPipe Initialize(IBenchmark benchmark) {
		var file = "/tmp/" + BenchmarkedMethod.Name + ".pipe";
		ProcessStartInfo startinfo;
		if(BenchmarkedMethod.Name.StartsWith("C")) {
			startinfo = new ProcessStartInfo("Crun");
		}
		else
		{
			startinfo = new ProcessStartInfo("java", "-jar JavaRun.jar ");
		}
		startinfo.UseShellExecute = true;
		startinfo.Arguments += file;
		Process.Start(startinfo);
		var pipe = new FPipe(file);
		pipe.ExpectCmd(Cmd.Ready);
		return pipe;      
	}

	public FPipe WarmupIteration(FPipe oldstate) {
		var p = oldstate;
		p.ExpectCmd(Cmd.Ready);
		p.WriteCmd(Cmd.Go);
		p.ExpectCmd(Cmd.Done);
		p.WriteCmd(Cmd.Ready);
		return oldstate;
	}

	public FPipe PreRun(FPipe oldstate) {
		oldstate.ExpectCmd(Cmd.Ready);
		return oldstate;
	}

	public object Run(FPipe state) {
		state.WriteCmd(Cmd.Go);
		state.ExpectCmd(Cmd.Done);
		return state;
	}

	public FPipe PostRun(FPipe oldstate) {
		oldstate.WriteCmd(Cmd.Ready);
		return oldstate;
	}

	public FPipe AdjustLoopIterations(FPipe oldstate) {
		//TODO: scale over IPC
		BenchmarkInfo.LoopIterations = 10;
		return oldstate;
	}
}