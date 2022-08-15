using System.Threading.Tasks;
using SocketComm;

namespace CsharpRAPL.Benchmarking.Lifecycles; 

public class IpcState {
	public IpcState(FPipe pipe) {
		this.Pipe = pipe;
	}
	public FPipe Pipe { get; }
	public bool Hasrun = false;
	
}