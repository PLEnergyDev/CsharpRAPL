using System.Linq;

namespace CsharpRAPL.Data {
	public class PValueData {
		public string Name;
		public double[] Times;
		public double[] Package;
		public double[] Dram;
		public double TimeMean;
		public double PackageMean;
		public double DramMean;

		public PValueData(string n, double[] t, double[] p, double[] d) {
			Name = n;
			Times = t;
			Package = p;
			Dram = d;
			TimeMean = t.Average();
			PackageMean = p.Average();
			DramMean = d.Average();
		}
	}
}