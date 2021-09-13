using System.Collections.Generic;

namespace CsharpRAPL.Tests {
	public class DummyDevice : DummyApi {
		public override List<double> Collect() {
			return new List<double> { 2514970.492 };
		}

		public override List<string> OpenRaplFiles() {
			return new List<string>() { "/sys/class/powercap/intel-rapl:0/energy_uj", };
		}
	}
}