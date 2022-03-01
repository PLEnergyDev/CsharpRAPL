using System;
using CsharpRAPL.Data;

namespace CsharpRAPL.Measuring;

public class MemoryApi {
	private MemoryMeasurement _measurement;

	public void Start() {
		_measurement = default;
		_measurement.GC0 = GC.CollectionCount(0);
		_measurement.GC1 = GC.CollectionCount(1);
		_measurement.GC2 = GC.CollectionCount(2);
		GCMemoryInfo info = GC.GetGCMemoryInfo();
		_measurement.HeapSizeBytes = info.HeapSizeBytes;
	}

	public MemoryMeasurement End() {
		_measurement.GC0 = GC.CollectionCount(0) - _measurement.GC0;
		_measurement.GC1 = GC.CollectionCount(1) - _measurement.GC1;
		_measurement.GC2 = GC.CollectionCount(2) - _measurement.GC2;
		GCMemoryInfo info = GC.GetGCMemoryInfo();
		_measurement.Concurrent = info.Concurrent;
		_measurement.Generation = info.Generation;
		_measurement.Index = info.Index;
		_measurement.Compacted = info.Compacted;
		_measurement.PauseTimePercentage = info.PauseTimePercentage;
		_measurement.HeapSizeBytes = info.HeapSizeBytes - _measurement.HeapSizeBytes;
		return _measurement;
	}
}