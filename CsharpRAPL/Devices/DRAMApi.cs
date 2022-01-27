using System;
using System.IO;
using CsharpRAPL.Data;

namespace CsharpRAPL.Devices;

public sealed class DRAMApi : DeviceApi {
	public DRAMApi() : base(CollectionApproach.Difference) { }

	protected override string OpenRaplFile() {
		return GetDRAMFile(GetSocketDirectoryName());
	}

	private static string GetDRAMFile(string directoryName) {
		var raplDeviceId = 0;
		while (Directory.Exists($"{directoryName}/intel-rapl:0:{raplDeviceId}")) {
			string dirName = $"{directoryName}/intel-rapl:0:{raplDeviceId}";
			string content = File.ReadAllText($"{dirName}/name").Trim();
			if (content.Equals("dram")) {
				return $"{dirName}/energy_uj";
			}

			raplDeviceId += 1;
		}

		throw new Exception("Failed to access the DRAM rapl files, make sure your computer can access these files.");
	}
}