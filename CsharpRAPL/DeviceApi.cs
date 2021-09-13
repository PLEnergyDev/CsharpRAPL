using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CsharpRAPL {
	public abstract class DeviceApi : IDeviceApi {
		private readonly List<int> _socketIds;
		private readonly List<string> _sysFiles;

		protected DeviceApi(List<int> socketIds = null) {
			List<int> allSocketIds = GetSocketIds();
			if (socketIds == null)
				_socketIds = allSocketIds;
			else
				foreach (int sid in socketIds) {
					if (allSocketIds.Contains(sid))
						throw new Exception("PyRAPLBadSocketIdException"); //TODO: Proper exceptions

					_socketIds = socketIds;
				}

			_socketIds?.Sort();
			_sysFiles = OpenRaplFiles();
		}

		public List<int> GetCpus() {
			const string apiFile = "/sys/devices/system/cpu/present";
			var cpuList = new List<int>();
			var cpuCountRe = new Regex(@"\d+|-");
			MatchCollection cpuMatches = cpuCountRe.Matches(File.ReadAllText(apiFile).Trim());

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
				socketIdList.Add(int.Parse(File.ReadAllText(path).Trim()));
			}

			return socketIdList.Distinct().ToList();
		}

		public abstract List<string> OpenRaplFiles();

		public List<(string dirName, int raplId)> GetSocketDirectoryNames() {
			var raplId = 0;
			var resultList = new List<(int packageId, string dirName, int raplId)>();

			while (Directory.Exists($"/sys/class/powercap/intel-rapl/intel-rapl:{raplId}")) {
				var dirName = $"/sys/class/powercap/intel-rapl/intel-rapl:{raplId}";

				string pkgStr = File.ReadAllText($"{dirName}/name").Trim();

				if (!pkgStr.Contains("package"))
					continue;
				int packageId = int.Parse(pkgStr.Split('-')[1]);

				if (_socketIds != null && !_socketIds.Contains(packageId)) continue;

				resultList.Add((packageId, dirName, raplId));
				raplId += 1;
			}

			if (resultList.Count != _socketIds.Count)
				throw new Exception("PyRAPLCantInitDeviceAPI"); //TODO: Proper exceptions


			return resultList.OrderBy(t => t.packageId).Select(t => (t.dirName, t.raplId)).ToList();
		}

		public virtual List<double> Collect() {
			List<double> result = Enumerable.Range(0, _socketIds.Count).Select(_ => -1.0).ToList();
			for (var i = 0; i < _sysFiles.Count; i++) {
				string deviceFile = _sysFiles[i];
				//TODO: Test om der er mærkbar forskel ved at holde filen åben og læse linjen på ny
				if (double.TryParse(File.ReadAllText(deviceFile), out double energyVal))
					result[_socketIds[i]] = energyVal;
			}

			return result;
		}
	}
}