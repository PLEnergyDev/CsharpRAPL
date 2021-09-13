using System.Collections.Generic;

namespace CsharpRAPL {
	public interface IDeviceApi {
		List<double> Collect();
		List<string> OpenRaplFiles();
		List<int> GetCpus();
		public List<int> GetSocketIds();
		List<(string dirName, int raplId)> GetSocketDirectoryNames();
	}
}