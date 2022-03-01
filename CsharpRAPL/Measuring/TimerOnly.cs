using CsharpRAPL.Data;
using CsharpRAPL.Measuring.Devices;

namespace CsharpRAPL.Measuring;

public sealed class TimerOnly : IMeasureApi {
	private readonly TimerApi _timerApi;

	public TimerOnly() {
		_timerApi = new TimerApi();
	}

	public void Start() {
		_timerApi.Start();
	}

	public void End() {
		_timerApi.End();
	}

	public bool IsValid() {
		return _timerApi.IsValid();
	}

	public BenchmarkResult GetResults(ulong loopIterations) {
		BenchmarkResult result = new() {
			ElapsedTime = _timerApi.Delta
		};
		return result;
	}

	public BenchmarkResult GetNormalizedResults(ulong loopIterations, int normalizedIterations = 1000000) {
		BenchmarkResult result = new() {
			ElapsedTime = _timerApi.Delta / ((double)loopIterations / normalizedIterations)
		};
		return result;
	}
}