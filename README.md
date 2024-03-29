# CsharpRAPL
[![pipeline status](https://gitlab.com/ImDreamer/CsharpRAPL/badges/main/pipeline.svg)](https://gitlab.com/ImDreamer/CsharpRAPL/-/commits/main)
[![coverage report](https://gitlab.com/ImDreamer/CsharpRAPL/badges/main/coverage.svg)](https://gitlab.com/ImDreamer/CsharpRAPL/-/commits/main)

Fork of https://gitlab.com/ImDreamer/CsharpRAPL

# Table of contents 
1. [Introduction](#introduction)
2. [Requirements](#requirements)
3. [Usage](#usage)
   1. [Registering Benchmarks](#registering-benchmarks)
      1. [Attributes](#attributes)
      2. [Manual](#attributes)
      3. [IPC Benchmarks](#ipc-benchmarks)
      4. [Automatic Compilation of IPC Benchmarks](#automatic-compilation-of-ipc-benchmarks)
   2. [Running the Benchmarks](#running-the-benchmarks)
4. [Scripts](#scripts)
5. [CLI Options](#cli-options)

# Introduction

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

![Example Plot](https://i.imgur.com/52YHoH4.png)



# Requirements
* Linux with access to RAPL
* libgdiplus - Is required for plotting
* cpuset - Required for creating a cpuset to execute benchmarks. Can be used without.


# Usage
Each of your benchmark class must contain the following fields:
```csharp
public static ulong Iterations;
public static ulong LoopIterations;
```
This is needed to make use of Dynamic Iteration Calculation and Dynamic Loop Iteration Scaling. 

Note that both ulong and unit is supported.

## Registering Benchmarks
There are two ways of registering benchmarks, either using Attributes or doing it manually.
The most tested method is using Attributes.

### Attributes
We can register a benchmark by adding the Benchmark attribute where the first argument is the benchmark group and the second is the description for the benchmark.
An example can be seen below.
```c#
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

The attribute also has options for order and if it should skip the benchmark.

To take advantage of  using the Attributes you have to use the ``BenchmarkCollector`` which collects all methods with the Benchmark attribute.

When using the attributes you can also make use of ``Variations`` attribute, which creates variations of a field or property.

A `VariationBenchmark` must be present on the methods which uses the variations.
This is to avoid creating lots of extra variations that aren't needed.

Example:
```c#
[Variations(10, 15)] public int TestProp { get; set; }
[Variations(21, 12)] public int TestProp2 { get; set; }
```

Here we have two properties which will create 4 variations like seen below:
```c#
Variation 1: TestProp = 10, TestProp2 = 21
Variation 2: TestProp = 10, TestProp2 = 12
Variation 3: TestProp = 15, TestProp2 = 21
Variation 4: TestProp = 15, TestProp2 = 12
```
So it will create an instance of the benchmark where each these values are set according to the table above.

Another attribute that is available is the `TypeVariations` attribute which is used for creating benchmark variations where we want to use different instances of a class like so:

```c#
[TypeVariations(typeof(ArgumentException), typeof(ArgumentNullException), typeof(ArgumentOutOfRangeException))]
private Exception Exception = new Exception();
```
The above code if used will create 3 variations where in each variation `Exception` is assigned to a new instance for each of the types in the attribute.

The field/property should have a default value as to not give null-reference expectations when running the no-variation benchmark.

To use this the method must be marked with `VariationBenchmark` attribute.
### Manual

You can add benchmarks manually by either using ``BenchmarkCollector`` or ``BenchmarkSuite`` to do that use the RegisterBenchmark method.

Example of manual registration:
```c#
public static void Main(string[] args){
    var benchmarkSuite = new BenchmarkSuite();
    benchmarkSuite.RegisterBenchmark("TestGroup", DummyBenchmark);
}

public static int DummyBenchmark() {
        return 1;
}
```

### IPC Benchmarks
From version [1.4.1](https://github.com/PLEnergyDev/CsharpRAPL/packages/1521502?version=1.4.1) CsharpRAPL support running benchmarks that are not necessarily written in C# by communicating over sockets using our IPC benchmark protocol.
Documentation as well as implementations for C#, Java and C are available [here](https://github.com/PLEnergyDev/IPC).

To register an IPC benchmark, make use of the ``IpcBenchmarkLifecycle`` in the ``Benchamrk`` attribute like so:
```c#
[Benchmark("IPC", "An IPC benchamrk", typeof(IpcBenchmarkLifecycle)]
public static IpcState IpcBenchmark(IpcState s){
    s.ExecutablePath = "MyIpcExecutable";
    return s; 
}
```
The method needs to have the signature ``(IpcState) => IpcState`` and set the ``ExecutablePath`` property of the state.
The file located at the given path will be launched and given the path to an ``FPipe`` (See [IPC](https://github.com/PLEnergyDev/IPC)) open for connection as argument.

To function correctly, your program should adhere to the IPC protocol.

### Automatic Compilation of IPC Benchmarks
To make IPC benchmarking easier, pipelines for compiling C and Java benchmarks have been implemented.
1. Write a C or Java function you want to benchmark
2. Copy the files from either [CSocketComm](https://github.com/PLEnergyDev/IPC/tree/master/CSocketComm) or [JSocketComm](https://github.com/PLEnergyDev/IPC/tree/master/JSocketComm/src) to be in the same directory as your source file
3. Copy either the [CompileC](https://github.com/PLEnergyDev/CsharpRAPL/blob/master/CsharpRAPL/Benchmarking/CompileCBenchmarks.sh) or [CompileJava](https://github.com/PLEnergyDev/CsharpRAPL/blob/master/CsharpRAPL/Benchmarking/CompileJavaBenchmarks.sh) script to be in working directory of your C# benchmarking program.
4. Your C# benchmark method should then return either a ``CState`` or ``JavaState`` (see examples below)
5. Your benchmark method should set the following properties in the state:
    - ``LibPath`` - The path to your source directory containing the files from step 1 & 2
    - ``JavaFile``/``CFile`` - The path, from ``LibPath``, to the file containing the method you want to benchmark.
    - ``BenchmarkSignature`` - Commands you want to execute during the benchmarking (including ;)
    - ``AdditionalCompilerOptions`` - (optional) Any additional options passed to the compiler e.g. optimizations or library linking
    - ``KeepCompilationResults`` - (optional) Keep the generated source files and compilation results of the benchmark. Default is false
    - ``HeaderFile``(C only) - The Path, from ``LibPath``, to the appropriate header file to your ``CFile``

**NB!**
Make sure that your compile script is marked as executable. If not, it cannot be launched. This can be done with the command ``sudo chmod +x <path/to/script>``

**Example of a Java benchmark**
```c#
[Benchmark("Java", "A Java IPC benchamrk", typeof(IpcBenchmarkLifecycle)]
public static JavaState IpcBenchmark(IpcState s){
    return new JavaState(s.pipePath){
        LibPath = "Benchmarks/Java",
        JavaFile = "Bench.java",
        BenchmarkSignature = "Bench.Run();",
        KeepCompilationResults = true 
    };
}
```
**Example of a C benchmark**
```c#
[Benchmark("C", "A C IPC benchamrk", typeof(IpcBenchmarkLifecycle)]
public static CState IpcBenchmark(IpcState s){
    return new CState(s.pipePath){
        LibPath = "Benchmarks/C",
        JavaFile = "bench.c",
        HeaderFile = "bench.h",
        BenchmarkSignature = "RunBenchmark();",
        AdditionalCompilerOptions = "-lm"
    };
}
```
See more examples [here](#https://github.com/PLEnergyDev/CSNumericPerformance/tree/master)


## Running the Benchmarks 
To run the benchmarks simply call ``RunAll`` on either your ``BenchmarkSuite`` or ``BenchmarkCollector`` instance.

For more examples look at [Suite.cs](https://github.com/PLEnergyDev/CsharpRAPL/blob/master/Benchmarks/Suite.cs) or at the tests.


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
        -g, --SkipPlotGroups              If plotting each benchmark group should be skipped.
        -i, --Iterations                  (Default: 0) Sets the target iterations. (Disables Dynamic Iteration Calculation)
        -l, --LoopIterations              (Default: 0) Sets the target loop iterations. (Disables Dynamic Loop Iteration Scaling)
        -k, --KeepOldResults              If not set removes all files from the output folder and the plot folder.
        -o, --OutputPath                  (Default: results/) Set the output path for results.
        -p, --PlotResults                 Should the results be plotted?
        -a, --BenchmarksToAnalyse         The names of the benchmarks to analyse.
        -z, --ZipResults                  Zips the CSV results and plots into a single zip file.
        -j, --Json                        Uses json for output instead of CVS, includes more information.
        -m, --CollectMemoryInformation    Collects memory information before and after each benchmark.
        --TryTurnOffGC                    Tries to turn off GC during running of benchmarks.
        --GCMemory                        (Default: 250000000) Sets the amount of memory in bytes allowed to be used when turning off garbage collection.
        --PlotOutputPath                  (Default: _plots/) Sets the output path for plots.
        --OnlyPlot                        Plots the results in the output path.
        --OnlyAnalysis                    Analyse the results in the output path.
        --Verbose                         Enables debug information.
        --OnlyTime                        Only measures time.
        --help                            Display this help screen.
        --version                         Display version information.
