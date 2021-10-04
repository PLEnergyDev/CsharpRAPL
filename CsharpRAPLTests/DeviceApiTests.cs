using NUnit.Framework;

namespace CsharpRAPL.Tests {
	public class DeviceApiTests {
		[SetUp]
		public void Setup() { }

		[Test]
		public void TestGetSocketDirectoryName() {
			var dummyDevice = new DummyDevice();
			string actual = dummyDevice.GetSocketDirectoryName();
			const string expected = "/sys/class/powercap/intel-rapl/intel-rapl:0";

			Assert.AreEqual(expected, actual);
		}
	}
}