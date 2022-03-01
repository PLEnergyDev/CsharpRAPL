using System;

namespace CsharpRAPL.Data;

public struct MemoryMeasurement {
	/// <summary>
	/// The number of times garbage collection has occurred for gen 0 objects.
	/// </summary>
	public int GC0 { get; set; }

	/// <summary>
	/// The number of times garbage collection has occurred for gen 1 objects.
	/// </summary>
	public int GC1 { get; set; }

	/// <summary>
	/// The number of times garbage collection has occurred for gen 2 objects.
	/// </summary>
	public int GC2 { get; set; }

	/// <summary>
	/// The heap size in bytes.
	/// </summary>
	public long HeapSizeBytes { get; set; }

	/// <summary>
	/// The index of this GC. GC indices start with 1 and get increased at the beginning of a GC.
	/// Since the info is updated at the end of a GC, this means you can get the info for a BGC
	/// with a smaller index than a foreground GC finished earlier.
	/// </summary>
	public long Index { get; set; }

	/// <summary>
	/// The generation this GC collected. Collecting a generation means all its younger generation(s)
	/// are also collected.
	/// </summary>
	public int Generation { get; set; }

	/// <summary>
	/// Is this a concurrent GC (BGC) or not.
	/// </summary>
	public bool Concurrent { get; set; }

	/// <summary>
	/// This is the % pause time in GC so far. If it's 1.2%, this number is 1.2.
	/// </summary>
	public double PauseTimePercentage { get; set; }

	/// <summary>
	/// Is this a compacting GC or not.
	/// </summary>
	public bool Compacted { get; set; }
}