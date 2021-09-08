using System;
using System.Collections.Generic;
using System.IO;
using CsharpRAPL.Devices;

namespace CsharpRAPL {
	internal struct Measure {
		public readonly double Duration;
		public readonly List<(string apiName, double apiValue)> Apis;

		public Measure(List<(string, double)> raplResult) {
			Duration = raplResult.Find(res => res.Item1.Equals("timer")).Item2;
			Apis = raplResult;
		}
	}

	public class Benchmark {
		private const int MaxExecutionTime = 2700; //In seconds
		private const string OutputFilePath = "tempResults.csv";
		private int Iterations { get; }
		private double _elapsedTime = 0;
		private List<Measure> _resultBuffer = new();
		private readonly RAPL _rapl;
		private readonly TextWriter _stdout;

		// Prints everything to a null stream similar to /dev/null
		private readonly TextWriter _benchmarkOutputStream = new StreamWriter(Stream.Null);


		public Benchmark(int iterations, bool silenceBenchmarkOutput = true) {
			_stdout = Console.Out;

			if (!silenceBenchmarkOutput)
				_benchmarkOutputStream = _stdout;

			Iterations = iterations;

			_rapl = new RAPL(
				new List<Sensor>() {
					new("timer", new TimerApi(), CollectionApproach.Difference),
					new("package", new PackageApi(), CollectionApproach.Difference),
					new("dram", new DramApi(), CollectionApproach.Difference),
					new("temp", new TempApi(), CollectionApproach.Average)
				}
			);
		}

		private void Start() => _rapl.Start();

		private void End() {
			_rapl.End();

			//Result only valid if all results are valid
			//Only then is the result added and duration is incremented
			if (!_rapl.IsValid()) return;
			var mes = new Measure(_rapl.GetResults());
			_resultBuffer.Add(mes);
			_elapsedTime += mes.Duration / 1_000;
		}


		// Used to run benchmarks which take a single input argument -- The benchmarks is curried into a function which takes zero input arguments
		public void Run<T, TR>(Func<T, TR> benchmark, T input, Action<TR> benchmarkOutput) =>
			Run(() => benchmark(input), benchmarkOutput);


		//Performns benchmarking
		//Writes progress to stdout if there is more than one iteration
		public void Run<TR>(Func<TR> benchmark, Action<TR> benchmarkOutput) {
			//Sets console to write to null
			Console.SetOut(_benchmarkOutputStream);

			_elapsedTime = 0;
			_resultBuffer = new List<Measure>();
			for (var i = 0; i < Iterations; i++) {
				if (Iterations != 1)
					Print(Console.Write, $"\r{i + 1} of {Iterations}");

				//Actually performing benchmark and resulting IO
				Start();
				TR res = benchmark();
				End();

				if (_benchmarkOutputStream.Equals(_stdout))
					Print(Console.WriteLine);
				benchmarkOutput(res);

				if (!(_elapsedTime >= MaxExecutionTime)) continue;

				Print(Console.WriteLine, "\nEnding benchmark due to time constraints");
				break;
			}

			if (Iterations != 1)
				Print(Console.WriteLine);

			SaveResults();

			//Resets console output
			Console.SetOut(_stdout);
		}


		/// Used to print to standard out -- Everything printed outside this method will not be shown
		private void Print(Action<string> printAction, string value = "") {
			Console.SetOut(_stdout);
			printAction(value);
			Console.Out.Flush();
			Console.SetOut(_benchmarkOutputStream);
		}

		//Saves result to temporary file
		//This is overwritten each time SaveResults is run
		private void SaveResults() {
			const string header = "duration(ms);pkg(µj);dram(µj);temp(C)" + "\n";
			string result = header;

			foreach (Measure m in _resultBuffer) {
				foreach ((string apiName, double apiValue) in m.Apis) {
					//Temperature api
					if (apiName.Equals("temp"))
						result += apiValue / 1000;
					//All other apis
					else if (apiName.Equals("timer"))
						result += $"{apiValue,0:N3};";
					else
						result += apiValue + ";";
				}

				result += "\n";
			}

			File.WriteAllText(OutputFilePath, result);
		}
	}
}