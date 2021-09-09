using System.Collections.Generic;
using System.Diagnostics;

namespace CsharpRAPL.Devices {
	public sealed class TimerApi : DeviceApi {
		private readonly Stopwatch _sw = new();

		public TimerApi() {
			_sw.Start();
		}

		public override List<string> OpenRaplFiles() {
			return null;
		}

		public override List<double> Collect() {
			return new List<double> { _sw.Elapsed.TotalMilliseconds };
		}
	}
}