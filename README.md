# CsharpRAPL
[![pipeline status](https://gitlab.com/ImDreamer/CsharpRAPL/badges/main/pipeline.svg)](https://gitlab.com/ImDreamer/CsharpRAPL/-/commits/main)
[![coverage report](https://gitlab.com/ImDreamer/CsharpRAPL/badges/main/coverage.svg)](https://gitlab.com/ImDreamer/CsharpRAPL/-/commits/main)

CSharpRAPL is a framework for benchmarking C# in regards to energy.

In the Benchmarks folder, all the benchmarks created for this project can be found.

CSharpRAPL contains the library code we have implemented to use for benchmarking.

This code is a continuation and extension of [lrecht/ParadigmComparison](https://github.com/lrecht/ParadigmComparison), which is an earlier university project.

By default, the CSharpRAPL tries to make each loop iteration take 2 seconds called Dynamic Loop Iteration Scaling.

CSharpRAPL also has Dynamic Iteration Calculation which scales the number of loop iterations according to the deviation of the results.

The results generated is a CSV file that contains the DRAM Energy, Temperature, Elapsed Time, Package Energy, and the return value of the Benchmark. 

**Note that the separator is ; and not ,**

Example
```
ElapsedTime;PackageEnergy;DramEnergy;Temperature;BenchmarkReturnValue
0.31062468886375416;3341.3842320442195;172.80355095863342;43;19
0.310964733362198;3335.9728753566737;172.84899950027466;43;19
0.31095251441001887;3353.798389434814;173.25878143310547;43;19
0.31064271926879894;3366.4405345916743;172.3937690258026;43.5;19
```

The results can be visualized using Box Plots an example of this can be seen on the following image (Note that the following image is only for illustrative purposes and the results might not be correct):

![Example Plot](https://media.discordapp.net/attachments/702101593449037844/908033950922993725/PrimitiveInteger-2021-11-10T17-20-47-400.png?width=540&height=405)



# Requirements
* Linux with access to RAPL
* libgdiplus - Is required for plotting
* cpuset - Required for creating a cpuset to execute benchmarks. Can be used without.


# Usage
To make use of Dynamic Iteration Calculation and Dynamic Loop Iteration Scaling your class must contain the following fields:


```csharp
public static int Iterations;
public static int LoopIterations;
```

Without these fields, Dynamic Iteration Calculation and Dynamic Loop Iteration Scaling won't be able to be used.

Your benchmark class doesn't have to be static same goes for benchmarks themselves.

## Registring Benchmarks
There are two ways of registering benchmarks, either using Attributes or doing it manually.
The most tested method is using Attributes.

### Attributes
We can register a benchmark by adding the Benchmark attribute where the first argument is the benchmark group and the second is the description for the benchmark.
An example can be seen below.
```csharp
[Benchmark("Addition", "Tests simple addition")]
public static int Add() {
    int a = 10;
    int res = 0;
    for (int i = 0; i < LoopIterations; i++) {
        res = a + i;
    }

    return res;
}

```
Here you have to use BenchmarkCollector which collects all methods with the Benchmark attribute except for those where skip is true.

### Manual

You can add benchmarks manually by either using BenchmarkCollector or BenchmarkSuite to do that use the RegisterBenchmark method.

Example of manual registration:
```csharp
public static void Main(string[] args){
    var benchmarkSuit = new BenchmarkSuite();
    benchmarkSuit.RegisterBenchmark("TestGroup", DummyBenchmark);
}

public static int DummyBenchmark() {
        return 1;
}
```

## Running the Benchmarks 
To run the benchmarks simply call ``RunAll`` on either your BenchmarkSuite or BenchmarkCollector instance.

For more examples look at [Suite.cs](https://gitlab.com/Plagiatdrengene/CsharpRAPL/-/blob/main/Benchmarks/Suite.cs) or at the tests.


# Scripts
We have provided the scripts:
* Benchmarks/execute.sh
    - Sets the CPU scaling governor to performance
    - Executes the benchmarks on shielded cores (see `cpuSet.sh`)
    - Sets the CPU scaling governor to powersave after running the benchmarks
* Benchmarks/removeResults.sh
    - Removes old results both in the form of CSV and plots. (Alternatively use -r)
* Benchmarks/zipResults.sh
    - Zips CSV results into a zip file. (Alternatively use -z) 
* Scripts/cpuSet.sh
    - Shields cores number 2 and 4 (0 indexed) from other processes, including the kernel. Uses the ubuntu package "cpuset", which is a Python wrapper over the Linux `cpuset` interface. This shield is to be used when executing benchmarks.
* Scripts/runAtStartup.sh
    - First makes sure hyperthread cores are disabled, then sets swappiness to 10, disables ASLR, lowers `perf` sampling interval, and disables Intel turbo-boost for consistency.
* Scripts/setPerformance.sh
    - Sets the cpu scaling governor to performance.
* Scripts/setPowerSave.sh
    - Sets the cpu scaling governor to powersave.

# CLI Options
    -g, --SkipPlotGroups         If plotting each benchmark group should be skipped.
    -i, --Iterations             (Default: -1) Sets the target iterations. (Disables Dynamic Iteration Calculation)
    -l, --LoopIterations         (Default: -1) Sets the target loop iterations. (Disables Dynamic Loop Iteration Scaling)
    -r, --RemoveOldResults       If set removes all files from the output folder and the plot folder.
    -o, --OutputPath             (Default: results/) Set the output path for results.
    -p, --PlotOutputPath         (Default: _plots/) Sets the output path for plots.
    -a, --BenchmarksToAnalyse    The names of the benchmarks to analyse.
    -z, --ZipResults             Zips the CSV results and plots into a single zip file.
    --OnlyPlot                   Plots the results in the output path.
    --OnlyAnalysis               Analysis the results in the output path.
    --help                       Display this help screen.
    --version                    Display version information.
    --Verbose                    Enables debug information.
