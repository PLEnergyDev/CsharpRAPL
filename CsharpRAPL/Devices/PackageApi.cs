using CsharpRAPL.Data;

namespace CsharpRAPL.Devices {
	public sealed class PackageApi : DeviceApi {
		public PackageApi() : base(CollectionApproach.Difference) { }

		protected override string OpenRaplFile() {
			return $"{GetSocketDirectoryName()}/energy_uj";
		}
	}
}