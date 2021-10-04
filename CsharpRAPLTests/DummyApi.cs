namespace CsharpRAPL.Tests {
	public abstract class DummyApi {
		public abstract double Collect();

		public abstract string OpenRaplFile();

		public string GetSocketDirectoryName() {
			return $"/sys/class/powercap/intel-rapl/intel-rapl:0";
		}
	}
}