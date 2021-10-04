using System;
using System.IO;
using CsharpRAPL.Data;

namespace CsharpRAPL.Devices {
	public sealed class TempApi : DeviceApi {
		protected override string OpenRaplFile() {
			const string path = "/sys/class/thermal/";
			var thermalId = 0;
			while (Directory.Exists($"{path}/thermal_zone{thermalId}")) {
				var dirname = $"{path}/thermal_zone{thermalId}";
				string type = File.ReadAllText($"{dirname}/type").Trim();
				if (type.Contains("pkg_temp"))
					return $"{dirname}/temp";
				thermalId++;
			}

			throw new Exception("No thermal zone found for the package");
		}

		public TempApi() : base(CollectionApproach.Average) { }
	}
}