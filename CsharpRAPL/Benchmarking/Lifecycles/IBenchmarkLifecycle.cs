using System;
using System.Reflection;

namespace CsharpRAPL.Benchmarking.Lifecycles;
public interface IBenchmarkLifecycle {
	public MethodInfo BenchmarkedMethod { get; }
	public Type Type { get; }
	public BenchmarkInfo BenchmarkInfo { get; }
	public object Initialize(IBenchmark benchmark);
	public object WarmupIteration(object oldstate);
	public object PreRun(object oldstate);
	public object Run(object state);
	public object PostRun(object oldstate);
	public object AdjustLoopIterations(object oldstate);
	public object End(object oldstate);
}

public interface IBenchmarkLifecycle <T> : IBenchmarkLifecycle{
	Type IBenchmarkLifecycle.Type => typeof(T);
	object IBenchmarkLifecycle.Initialize(IBenchmark benchmark) => Initialize(benchmark);
	object IBenchmarkLifecycle.WarmupIteration(object oldstate) => WarmupIteration((T)oldstate);
	object IBenchmarkLifecycle.PreRun(object oldstate) => PreRun((T)oldstate);
	object IBenchmarkLifecycle.PostRun(object oldstate) => PostRun((T)oldstate);
	object IBenchmarkLifecycle.Run(object state) => Run((T)state);
	object IBenchmarkLifecycle.AdjustLoopIterations(object oldstate) => AdjustLoopIterations((T)oldstate);
	object IBenchmarkLifecycle.End(object oldstate) => End((T)oldstate);
	public new T Initialize(IBenchmark benchmark); //TODO: should be fine.... create tests 
	public T WarmupIteration(T oldstate);
	public T PreRun(T oldstate);
	public object Run(T state);
	public T PostRun(T oldstate);
	public T AdjustLoopIterations(T oldstate);
	public T End(T oldstate);
}
