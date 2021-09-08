using System.Collections.Generic;
using System.IO;
using System;

namespace CsharpRAPL.Devices {
	public class DramApi : DeviceApi {
		public DramApi(List<int> socketIds = null) : base(socketIds) { }

		public override List<string> OpenRaplFiles() {
			List<(string, int)> socketNames = GetSocketDirectoryNames();

			string GetDramFile(string directoryName, int raplSocketId) {
				var raplDeviceId = 0;
				while (Directory.Exists($"{directoryName}/intel-rapl:{raplSocketId}:{raplDeviceId}")) {
					var dirName = $"{directoryName}/intel-rapl:{raplSocketId}:{raplDeviceId}";
					string content = File.ReadAllText($"{dirName}/name").Trim();
					if (content.Equals("dram"))
						return $"{dirName}/energy_uj";

					raplDeviceId += 1;
				}

				throw new Exception("PyRAPLCantInitDeviceAPI"); //TODO: Proper exceptions
			}

			var raplFiles = new List<string>();
			foreach ((string socketDirectoryName, int raplSocketId) in socketNames) {
				raplFiles.Add(GetDramFile(socketDirectoryName, raplSocketId));
			}

			return raplFiles;
		}
	}
}