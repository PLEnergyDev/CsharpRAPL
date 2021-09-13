using System.Collections.Generic;
using NUnit.Framework;

namespace CsharpRAPL.Tests {
	public class DeviceApiTests {
		[SetUp]
		public void Setup() { }

		[Test]
		public void TestGetCpus() {
			var dummyDevice = new DummyDevice();
			List<int> actual = dummyDevice.GetCpus();
			List<int> expected = new() { 0, 0, 1, 2, 3, 4, 5, 6, 7 };

			Assert.That(actual, Is.EquivalentTo(expected));
		}

		[Test]
		public void TestGetSocketIds() {
			var dummyDevice = new DummyDevice();
			List<int> actual = dummyDevice.GetSocketIds();
			List<int> expected = new() { 0 };

			Assert.That(actual, Is.EquivalentTo(expected));
		}

		[Test]
		public void TestGetSocketDirectoryNames() {
			var dummyDevice = new DummyDevice();
			List<(string dirName, int raplId)> actual = dummyDevice.GetSocketDirectoryNames();
			var expected = new List<(string dirName, int raplId)> {
				("/sys/class/powercap/intel-rapl/intel-rapl:0", 0)
			};

			Assert.That(actual, Is.EquivalentTo(expected));
		}
	}
}