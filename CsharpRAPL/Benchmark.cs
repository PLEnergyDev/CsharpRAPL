using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsharpRAPL.Data;
using CsharpRAPL.Devices;

namespace CsharpRAPL {
	public class Benchmark {
		private const int MaxExecutionTime = 2700; //In seconds

		// Prints everything to a null stream similar to /dev/null
		private readonly TextWriter _benchmarkOutputStream = new StreamWriter(Stream.Null);
		private readonly RAPL _rapl;
		private readonly TextWriter _stdout;
		private double _elapsedTime;
		private List<Measure> _resultBuffer = new();
		private readonly string _outputFilePath;
		public int Iterations { get; private set; }
		public string Name { get; private set; }

		private readonly Func<int> _benchmark;
		private readonly Action<int> _benchmarkOutput;

		public Benchmark(string name, int iterations, Func<int> benchmark, Action<int> benchmarkOutput,
			bool silenceBenchmarkOutput = true) : this(name, iterations, silenceBenchmarkOutput) {
			_benchmark = benchmark;
			_benchmarkOutput = benchmarkOutput;
		}


		public Benchmark(string name, int iterations, bool silenceBenchmarkOutput = true) {
			Name = name;

			_stdout = Console.Out;
			Directory.CreateDirectory($"results/{name}");
			_outputFilePath =
				$"results/{name}/{name}-{DateTime.Now.ToString("s").Replace(":", "-")}.csv";

			if (!silenceBenchmarkOutput)
				_benchmarkOutputStream = _stdout;

			Iterations = iterations;

			_rapl = new RAPL(
				new List<Sensor> {
					new("timer", new TimerApi(), CollectionApproach.Difference),
					new("package", new PackageApi(), CollectionApproach.Difference),
					new("dram", new DramApi(), CollectionApproach.Difference),
					new("temp", new TempApi(), CollectionApproach.Average)
				}
			);
		}

		private void Start() {
			_rapl.Start();
		}

		private void End() {
			_rapl.End();

			//Result only valid if all results are valid
			//Only then is the result added and duration is incremented
			if (!_rapl.IsValid()) return;
			var mes = new Measure(_rapl.GetResults());
			_resultBuffer.Add(mes);
			_elapsedTime += mes.Duration / 1_000;
		}

		public void Run() {
			Run(_benchmark, _benchmarkOutput);
		}

		// Used to run benchmarks which take a single input argument -- The benchmarks is curried into a function which takes zero input arguments
		public void Run<T, TR>(Func<T, TR> benchmark, T input, Action<TR> benchmarkOutput) {
			Run(() => benchmark(input), benchmarkOutput);
		}

		//Performns benchmarking
		//Writes progress to stdout if there is more than one iteration
		public void Run<TR>(Func<TR> benchmark, Action<TR> benchmarkOutput) {
			//Sets console to write to null
			Console.SetOut(_benchmarkOutputStream);

			_elapsedTime = 0;
			_resultBuffer = new List<Measure>();
			for (var i = 0; i < Iterations; i++) {
				if (Iterations != 1)
					Print(Console.Write, $"\r{i + 1} of {Iterations} for {Name}");

				//Actually performing benchmark and resulting IO
				Start();
				TR res = benchmark();
				End();

				if (_benchmarkOutputStream.Equals(_stdout))
					Print(Console.WriteLine);
				benchmarkOutput(res);

				if (!(_elapsedTime >= MaxExecutionTime)) continue;

				Print(Console.WriteLine, $"\nEnding for {Name} benchmark due to time constraints");
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
			double packageValues = _resultBuffer.SelectMany(measure =>
				measure.Apis.Where(tuple => tuple.apiName == "package")).Average(tuple => tuple.apiValue);

			double timeValues = _resultBuffer.SelectMany(measure =>
				measure.Apis.Where(tuple => tuple.apiName == "timer")).Average(tuple => tuple.apiValue);

			double tempValues = _resultBuffer.SelectMany(measure =>
				measure.Apis.Where(tuple => tuple.apiName == "temp")).Average(tuple => tuple.apiValue);

			double dramValues = _resultBuffer.SelectMany(measure =>
				measure.Apis.Where(tuple => tuple.apiName == "dram")).Average(tuple => tuple.apiValue);

			string header =
				$"duration(ms) avg={timeValues};pkg(µj) avg={packageValues};dram(µj) avg={dramValues};temp(C) avg={tempValues}" +
				"\n";
			var result = new StringBuilder(header);


			foreach (Measure m in _resultBuffer) {
				foreach ((string apiName, double apiValue) in m.Apis)
					switch (apiName) {
						case "temp":
							result.Append(apiValue / 1000);
							break;
						case "timer":
							result.Append($"{apiValue,0:N3};");
							break;
						default:
							result.Append(apiValue + ";");
							break;
					}

				result.AppendLine();
			}

			File.WriteAllText(_outputFilePath, result.ToString());
		}
	}
}