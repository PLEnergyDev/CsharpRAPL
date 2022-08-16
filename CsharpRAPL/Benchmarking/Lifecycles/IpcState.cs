using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SocketComm;

namespace CsharpRAPL.Benchmarking.Lifecycles; 

public class IpcState {
	public string PipePath { get; }
	public IpcState(string pipe) {
		PipePath = pipe;
		Pipe = new FPipe(pipe);
	}

	public string ExecutablePath { get; set; }
	public FPipe Pipe { get; private set; }
	public bool Hasrun = false;

	public virtual IpcState Generate() => this;
	
	public virtual void OnPipeListening(object? sender, EventArgs e) {
		Generate();
		// Start pipe client
		ProcessStartInfo startinfo;
		//TODO: makeshift implementation. Should be dynamic via states
		startinfo = new ProcessStartInfo(ExecutablePath) {
			UseShellExecute = true
		};
		startinfo.Arguments += PipePath;
		Process.Start(startinfo);
	}
}