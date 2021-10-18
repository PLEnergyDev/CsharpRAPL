using CsharpRAPL.Data;
using CsharpRAPL.Devices;

namespace CsharpRAPL {
	public sealed class RAPL {
		private readonly DramApi _dramApi;
		private readonly TimerApi _timerApi;
		private readonly PackageApi _packageApi;
		private readonly TempApi _tempApi;

		public RAPL() {
			_dramApi = new DramApi();
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

		public BenchmarkResult GetResults() {
			BenchmarkResult result = new() {
				DramPower = _dramApi.Delta,
				Temperature = _tempApi.Delta,
				ElapsedTime = _timerApi.Delta,
				PackagePower = _packageApi.Delta
			};
			return result;
		}
	}
}