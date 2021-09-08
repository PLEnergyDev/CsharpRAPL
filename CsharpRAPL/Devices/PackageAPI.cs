using System.Collections.Generic;

namespace CsharpRAPL.Devices {
	public class PackageApi : DeviceApi {
		public PackageApi(List<int> socketIds = null) : base(socketIds) { }

		public override List<string> OpenRaplFiles() {
			List<(string, int)> socketNames = GetSocketDirectoryNames();
			var raplFiles = new List<string>();

			foreach ((string dir, int id) in socketNames) {
				raplFiles.Add($"{dir}/energy_uj");
			}

			return raplFiles;
		}
	}
}