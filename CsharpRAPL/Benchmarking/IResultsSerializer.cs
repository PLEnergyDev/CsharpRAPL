namespace CsharpRAPL.Benchmarking;

public interface IResultsSerializer {
	void SerializeResults(IBenchmark benchmark);
}