using System;
using System.IO;
using CsharpRAPL.Data;

namespace CsharpRAPL.Devices; 

public sealed class DramApi : DeviceApi {
	public DramApi() : base(CollectionApproach.Difference) { }

	protected override string OpenRaplFile() {
		return GetDramFile(GetSocketDirectoryName());
	}

	private static string GetDramFile(string directoryName) {
		var raplDeviceId = 0;
		while (Directory.Exists($"{directoryName}/intel-rapl:0:{raplDeviceId}")) {
			var dirName = $"{directoryName}/intel-rapl:0:{raplDeviceId}";
			string content = File.ReadAllText($"{dirName}/name").Trim();
			if (content.Equals("dram")) {
				return $"{dirName}/energy_uj";
			}

			raplDeviceId += 1;
		}

		throw new Exception("PyRAPLCantInitDeviceAPI"); //TODO: Proper exceptions
	}
}