namespace CsharpRAPL.Tests {
	public class DummyDevice : DummyApi {
		public override double Collect() {
			return 2514970.492;
		}

		public override string OpenRaplFile() {
			return "/sys/class/powercap/intel-rapl:0/energy_uj";
		}
	}
}