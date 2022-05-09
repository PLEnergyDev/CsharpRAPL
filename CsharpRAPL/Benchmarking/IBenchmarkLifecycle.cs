using System;

namespace CsharpRAPL.Benchmarking;
public interface IBenchmarkLifecycle {
	public IBenchmark Benchmark { get; }
	public Type Type { get; }

	public object Initialize(IBenchmark benchmark);
	public object WarmupIteration(object oldstate);
	public object PreRun(object oldstate);
	public object Run(object state);
	public object PostRun(object oldstate);
}

public interface IBenchmarkLifecycle <T> : IBenchmarkLifecycle{
	Type IBenchmarkLifecycle.Type => typeof(T);
	object IBenchmarkLifecycle.Initialize(IBenchmark benchmark) => Initialize(benchmark);
	object IBenchmarkLifecycle.WarmupIteration(object oldstate) => WarmupIteration((T)oldstate);
	object IBenchmarkLifecycle.PreRun(object oldstate) => PreRun((T)oldstate);
	object IBenchmarkLifecycle.PostRun(object oldstate) => PostRun((T)oldstate);
	object IBenchmarkLifecycle.Run(object state) => Run((T)state);
	public T Initialize(IBenchmark benchmark);
	public T WarmupIteration(T oldstate);
	public T PreRun(T oldstate);
	public object Run(T state);
	public T PostRun(T oldstate);
}
