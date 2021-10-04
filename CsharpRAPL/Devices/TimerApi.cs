using System.Diagnostics;
using CsharpRAPL.Data;

namespace CsharpRAPL.Devices {
	public sealed class TimerApi : DeviceApi {
		private readonly Stopwatch _sw = new();

		public TimerApi() : base(CollectionApproach.Difference) {
			_sw.Start();
		}

		protected override string OpenRaplFile() {
			return "";
		}

		protected override double Collect() {
			return _sw.Elapsed.TotalMilliseconds;
		}
	}
}