using NUnit.Framework;

namespace CsharpRAPL.Tests {
	public class DeviceApiTests {
		[Test]
		public void TestGetSocketDirectoryName() {
			DummyApi dummyDevice = new DummyDevice();
			string actual = dummyDevice.GetSocketDirectoryName();
			const string expected = "/sys/class/powercap/intel-rapl/intel-rapl:0";

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestCollect() {
			DummyApi dummyDevice = new DummyDevice();
			double actual = dummyDevice.Collect();
			const double expected = 2514970.492;

			Assert.AreEqual(expected, actual, double.Epsilon);
		}

		[Test]
		public void TestOpenRaplFile() {
			DummyApi dummyDevice = new DummyDevice();
			string actual = dummyDevice.OpenRaplFile();
			const string expected = "/sys/class/powercap/intel-rapl:0/energy_uj";

			Assert.AreEqual(expected, actual);
		}
	}
}