using CsvHelper.Configuration.Attributes;

namespace CsharpRAPL.Analysis {
	public class BenchmarkResult {
		[Index(0)] public double ElapsedTime { get; set; }
		[Index(1)] public double PackagePower { get; set; }
		[Index(2)] public double DramPower { get; set; }
		[Index(3)] public double Temperature { get; set; }
	}
}