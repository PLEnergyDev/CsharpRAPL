using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CsharpRAPL.Tests {
	public abstract class DummyApi : IDeviceApi {
		private readonly List<int> _socketIds;
		public abstract List<double> Collect();

		public abstract List<string> OpenRaplFiles();

		protected DummyApi() {
			_socketIds = GetSocketIds();
		}

		public List<int> GetCpus() {
			var cpuList = new List<int>();
			var cpuCountRe = new Regex(@"\d+|-");
			MatchCollection cpuMatches = cpuCountRe.Matches("0-7");
			for (var i = 0; i < cpuMatches.Count; i++) {
				if (cpuMatches[i].Value == "-") {
					int before = int.Parse(cpuMatches[i - 1].Value);
					int after = int.Parse(cpuMatches[i + 1].Value);
					cpuList.AddRange(Enumerable.Range(before, after - before));
				}
				else {
					cpuList.Add(int.Parse(cpuMatches[i].Value));
				}
			}

			return cpuList;
		}

		public List<int> GetSocketIds() {
			var socketIdList = new List<int>();

			foreach (int cpuId in GetCpus()) {
				var path = $"/sys/devices/system/cpu/cpu{cpuId}/topology/physical_package_id";
				socketIdList.Add(int.Parse("0"));
			}

			return socketIdList.Distinct().ToList();
		}

		public List<(string dirName, int raplId)> GetSocketDirectoryNames() {
			var raplId = 0;
			var resultList = new List<(int packageId, string dirName, int raplId)>();

			while (raplId < 2) {
				var dirName = $"/sys/class/powercap/intel-rapl/intel-rapl:{raplId}";

				string pkgStr = "package-" + raplId;

				if (!pkgStr.Contains("package"))
					continue;
				int packageId = int.Parse(pkgStr.Split('-')[1]);

				if (_socketIds != null && !_socketIds.Contains(packageId)) {
					raplId += 1;
					continue;
				}

				resultList.Add((packageId, dirName, raplId));
				raplId += 1;
			}

			if (_socketIds == null || resultList.Count != _socketIds.Count)
				throw new Exception("PyRAPLCantInitDeviceAPI"); //TODO: Proper exceptions


			return resultList.OrderBy(t => t.packageId).Select(t => (t.dirName, t.raplId)).ToList();
		}
	}
}