using System;
using System.IO;
using CsharpRAPL.Data;

namespace CsharpRAPL;

public abstract class DeviceApi {
	public double Delta { get; private set; }

	private double _endValue;
	private double _startValue;

	private readonly CollectionApproach _approach;
	private readonly string _sysFile;

	protected DeviceApi(CollectionApproach collectionApproach) {
		_approach = collectionApproach;
		_sysFile = OpenRaplFile();
	}

	protected abstract string OpenRaplFile();

	protected static string GetSocketDirectoryName() {
		if (Directory.Exists("/sys/class/powercap/intel-rapl/intel-rapl:0")) {
			return "/sys/class/powercap/intel-rapl/intel-rapl:0";
		}

		throw new Exception("PyRAPLCantInitDeviceAPI"); //TODO: Proper exceptions
	}

	protected virtual double Collect() {
		double res = -1.0;
		//TODO: Test and see if there is a measurable diffrence between keeping the file open or opening it everytime
		if (double.TryParse(File.ReadAllText(_sysFile), out double energyValue)) {
			res = energyValue;
		}

		return res;
	}

	public void Start() {
		_startValue = Collect();
		UpdateDelta();
	}

	public void End() {
		_endValue = Collect();
		UpdateDelta();
	}

	public bool IsValid() {
		return Math.Abs(_startValue - -1.0) > double.Epsilon &&
		       Math.Abs(_endValue - -1.0) > double.Epsilon &&
		       Delta >= 0;
	}

	private void UpdateDelta() {
		Delta = _approach switch {
			CollectionApproach.Difference => _endValue - _startValue,
			CollectionApproach.Average => (_endValue + _startValue) / 2,
			_ => throw new Exception("Collection approach is not available")
		};
	}
}