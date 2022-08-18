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
		Pipe = new FPipe(PipePath);
		Pipe.Listening += OnPipeListening;
	}
	public string ExecutablePath { get; set; }
	
	public FPipe Pipe { get; private set; }
	public bool HasRun = false;

	public virtual IpcState Generate() => this;
	
	public virtual void OnPipeListening(object? sender, EventArgs e) {
		Generate();
		// Start pipe client
		ProcessStartInfo startinfo;
		startinfo = new ProcessStartInfo(ExecutablePath) {
			UseShellExecute = true
		};
		startinfo.Arguments += PipePath;
		Process.Start(startinfo);
	}
}