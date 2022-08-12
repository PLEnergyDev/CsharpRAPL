using CsharpRAPL.Data;
using CsharpRAPL.Measuring.Devices;

namespace CsharpRAPL.Measuring;

public sealed class RAPL : IMeasureApi {
	private readonly DRAMApi _dramApi;
	private readonly TimerApi _timerApi;
	private readonly PackageApi _packageApi;
	private readonly TempApi _tempApi;

	public RAPL() {
		_dramApi = new DRAMApi();
		_timerApi = new TimerApi();
		_packageApi = new PackageApi();
		_tempApi = new TempApi();
	}

	public void Start() {
		_timerApi.Start();
		_tempApi.Start();
		_dramApi.Start();
		_packageApi.Start();
	}

	public void End() {
		_packageApi.End();
		_dramApi.End();
		_tempApi.End();
		_timerApi.End();
	}

	public bool IsValid() {
		return _dramApi.IsValid() && _tempApi.IsValid() && _timerApi.IsValid() && _packageApi.IsValid();
	}

	public BenchmarkResult GetResults(ulong loopIterations) {
		BenchmarkResult result = new() {
			DRAMEnergy = _dramApi.Delta,
			Temperature = _tempApi.Delta,
			ElapsedTime = _timerApi.Delta,
			PackageEnergy = _packageApi.Delta
		};
		return result;
	}

	public BenchmarkResult GetNormalizedResults(ulong loopIterations, int normalizedIterations = 1000000) {
		BenchmarkResult result = new() {
			DRAMEnergy = _dramApi.Delta / loopIterations,
			Temperature = _tempApi.Delta / 1000,
			ElapsedTime = _timerApi.Delta / loopIterations,
			PackageEnergy = _packageApi.Delta / loopIterations
		};
		return result;
	}
}