using System.Collections.Generic;
using System.Linq;

namespace CsharpRAPL {
	public class RAPL {
		private readonly List<Sensor> _apis;

		public RAPL(List<Sensor> sensors) => _apis = sensors;

		public void Start() => _apis.ForEach(api => api.Start());

		public void End() => _apis.ForEach(api => api.End());

		public bool IsValid() => _apis.All(api => api.IsValid());

		public List<(string deviceName, double value)> GetResults() {
			var res = new List<(string d, double v)>();
			foreach (Sensor api in _apis) {
				if (api.Delta.Count == 1)
					res.Add((api.Name, api.Delta[0]));
				else
					res.AddRange(api.Delta.Select((t, i) => (api.Name + i, t)));
			}

			return res;
		}
	}
}