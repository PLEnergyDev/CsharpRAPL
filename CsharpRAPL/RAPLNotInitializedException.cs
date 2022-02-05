using System;

namespace CsharpRAPL;

public class RAPLNotInitializedException : Exception {
	public RAPLNotInitializedException() : base("RAPL has not been initialized") { }
}