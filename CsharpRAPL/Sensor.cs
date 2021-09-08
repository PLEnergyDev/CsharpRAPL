using System;
using System.Linq;
using System.Collections.Generic;


namespace CsharpRAPL {
	public enum CollectionApproach {
		Average,
		Difference
	}

	public class Sensor {
		public string Name { get; }
		private readonly DeviceApi _api;
		private readonly CollectionApproach _approach;
		public List<double> Delta { get; private set; }
		private List<double> _startValue;
		private List<double> _endValue;

		public Sensor(string name, DeviceApi api, CollectionApproach approach) {
			Name = name;
			_api = api;
			_approach = approach;
		}

		public void Start() => _startValue = _api.Collect();

		public void End() {
			_endValue = _api.Collect();
			UpdateDelta();
		}

		public bool IsValid()
			=> _startValue.All(val => Math.Abs(val - -1.0) > float.Epsilon)
			   && _endValue.All(val => Math.Abs(val - (-1.0)) > float.Epsilon)
			   && Delta.Any(val => val >= 0);

		private void UpdateDelta() {
			Delta = _approach switch {
				CollectionApproach.Difference => Enumerable.Range(0, _endValue.Count)
					.Select(i => _endValue[i] - _startValue[i]).ToList(),
				CollectionApproach.Average => Enumerable.Range(0, _endValue.Count)
					.Select(i => (_endValue[i] + _startValue[i]) / 2).ToList(),
				_ => throw new Exception("Collection approach is not available")
			};
		}
	}
}