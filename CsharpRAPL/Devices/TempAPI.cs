using System.Collections.Generic;
using System.IO;
using System;

namespace CsharpRAPL.Devices {
	public class TempApi : DeviceApi {
		public override List<string> OpenRaplFiles() {
			const string path = "/sys/class/thermal/";
			var thermalId = 0;
			while (Directory.Exists($"{path}/thermal_zone{thermalId}")) {
				var dirname = $"{path}/thermal_zone{thermalId}";
				string type = File.ReadAllText($"{dirname}/type").Trim();
				if (type.Contains("pkg_temp"))
					return new List<string>() { $"{dirname}/temp" };
				thermalId++;
			}

			throw new Exception("No thermal zone found for the package");
		}
	}
}