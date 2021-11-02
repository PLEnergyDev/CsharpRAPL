using System.Linq;
using CsharpRAPL.Analysis;

namespace CsharpRAPL.Data;

public class PValueData {
	public readonly string Name;
	public readonly double[] TimesValues;
	public readonly double[] PackageValues;
	public readonly double[] DramValues;

	public PValueData(DataSet dataSet) {
		Name = dataSet.Name;
		TimesValues = dataSet.Data.Select(data => data.ElapsedTime).ToArray();
		PackageValues = dataSet.Data.Select(data => data.PackageEnergy).ToArray();
		DramValues = dataSet.Data.Select(data => data.DramEnergy).ToArray();
	}
}