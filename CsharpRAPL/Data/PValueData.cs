using System.Linq;
using CsharpRAPL.Analysis;

namespace CsharpRAPL.Data;

public class PValueData {
	public readonly string Name;
	public readonly double[] TimesValues;
	public readonly double[] PackageValues;
	public readonly double[] DRAMValues;

	public PValueData(DataSet dataSet) {
		Name = dataSet.Name;
		TimesValues = dataSet.Data.Select(data => data.ElapsedTime).ToArray();
		PackageValues = dataSet.Data.Select(data => data.PackageEnergy).ToArray();
		DRAMValues = dataSet.Data.Select(data => data.DRAMEnergy).ToArray();
	}
}