using System.Collections.Generic;

namespace CsharpRAPL.Data {
	internal readonly struct Measure {
		public readonly double Duration;
		public readonly List<(string apiName, double apiValue)> Apis;

		public Measure(List<(string, double)> raplResult) {
			Duration = raplResult.Find(res => res.Item1.Equals("timer")).Item2;
			Apis = raplResult;
		}
	}
}