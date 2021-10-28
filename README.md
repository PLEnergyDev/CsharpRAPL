# CsharpRAPL

CSharpRAPL was originally developed by lrecht and others for a university project.

The original code can be found here: [lrecht/ParadigmComparison](https://github.com/lrecht/ParadigmComparison).

# Requirements
* Linux with acesses to RAPL
* libgdiplus - Is required for plotting
* cpuset - Required for creating a cpuset to execute benchmarks. Can be used without.



# Scripts
We have provided the scripts:
* ExampleProject/execute.sh
    - Sets the cpu scaling governor to performance
    - Executes the benchmarks on shielded cores (see `cpuSet.sh`)
    - Sets the cpu scaling governor to powersave after running the benchmarks
* ExampleProject/removeResults.sh
    - Removes old results both in the form of CSV and plots. (Alternatively use -r)
* ExampleProject/zipResults.sh
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