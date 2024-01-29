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
		//Initialize state
		var file = "/tmp/" + BenchmarkedMethod.Name + ".pipe";
		var state = new IpcState(file, benchmark);
		
		//Get benchmark information
		state = (IpcState)BenchmarkedMethod.Invoke(null, new object?[]{state})!;
		
		//Open pipe for connection
		state.Pipe.Connect();
		state.Pipe.ExpectCmd(Cmd.Ready);
		return state;
	}


	public IpcState WarmupIteration(IpcState oldstate) {
		try {
			if (!oldstate.Benchmark.ResetBenchmark) {
				oldstate = PreRun(oldstate);
				oldstate = Run(oldstate);
				oldstate.Pipe.WriteCmd(Cmd.Ready);
			}
		}
		catch (Exception) {
			oldstate.Benchmark.ResetBenchmark = true;
		}

		return oldstate;
	}

	public IpcState PreRun(IpcState oldstate) {
		try {
			if (!oldstate.Benchmark.ResetBenchmark) {
				// oldstate.Pipe.SendValue(BenchmarkInfo.LoopIterations, SimpleConversion.NumberToBytes);
				oldstate.Pipe.ExpectCmd(Cmd.Ready);
			}
		}
		catch (Exception) {
			oldstate.Benchmark.ResetBenchmark = true;
		}

		return oldstate;
	}

	public IpcState Run(IpcState state) {
		try {
			if (!state.Benchmark.ResetBenchmark) {
				state.Pipe.WriteCmd(Cmd.Go);
				state.Pipe.ExpectCmd(Cmd.Done);
			}
		}
		catch (Exception e) {
			state.Benchmark.ResetBenchmark = true;
		}
		return state;
	}

	public IpcState PostRun(IpcState oldstate) {
		try {
			if (!oldstate.Benchmark.ResetBenchmark) {
				oldstate.Pipe.WriteCmd(oldstate.HasRun ? Cmd.Done : Cmd.Ready);
			}
			else {
				oldstate.Pipe.WriteCmd(Cmd.Exit);
			}
		}
		catch (Exception) {
			oldstate.Benchmark.ResetBenchmark = true;
		}

		return oldstate;
	}

	public IpcState AdjustLoopIterations(IpcState oldstate) {
		//TODO: scale over IPC
		if (ScaleLoopIterations()) {
			oldstate.Benchmark.ResetBenchmark = true;
		}
		return oldstate;
	}

	public IpcState End(IpcState oldstate) {
		oldstate.HasRun = true;
		return oldstate;
	}
	
	private bool ScaleLoopIterations() {
		ulong currentValue = BenchmarkInfo.LoopIterations;
		
		switch (currentValue) {
			case ulong.MaxValue:
				return false;
			case >= ulong.MaxValue / 2:
				BenchmarkInfo.LoopIterations =  ulong.MaxValue;
				BenchmarkInfo.RawResults.Clear();
				BenchmarkInfo.NormalizedResults.Clear();
				return true;
			default:
				BenchmarkInfo.LoopIterations = currentValue + currentValue;
				BenchmarkInfo.RawResults.Clear();
				BenchmarkInfo.NormalizedResults.Clear();
				return true;
		}
	}
}