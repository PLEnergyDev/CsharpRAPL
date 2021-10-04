using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsharpRAPL.Data;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsharpRAPL.Benchmarking {
	public class Benchmark {
		public int Iterations { get; }
		public string Name { get; }
		public string? Group { get; }

		private const int MaxExecutionTime = 2700; //In seconds

		// Prints everything to a null stream similar to /dev/null
		private readonly TextWriter _benchmarkOutputStream = new StreamWriter(Stream.Null);
		private readonly TextWriter _stdout;
		private readonly string _outputFilePath;
		private readonly Func<int> _benchmark;
		private readonly Action<int> _benchmarkOutput;
		private readonly List<BenchmarkResult> _resultBuffer = new();

		private RAPL? _rapl;
		private double _elapsedTime;

		public Benchmark(string name, int iterations, Func<int> benchmark, Action<int> benchmarkOutput,
			bool silenceBenchmarkOutput = true, string? group = null) {
			Name = name;
			Group = group;
			Iterations = iterations;

			_benchmark = benchmark;
			_benchmarkOutput = benchmarkOutput;
			_stdout = Console.Out;

			if (!silenceBenchmarkOutput)
				_benchmarkOutputStream = _stdout;


			string time = DateTime.Now.ToString("s").Replace(":", "-");
			if (Group != null) {
				Directory.CreateDirectory($"results/{group}/{name}");
				_outputFilePath = $"results/{group}/{name}/{name}-{time}.csv";
			}
			else {
				Directory.CreateDirectory($"results/{name}");
				_outputFilePath = $"results/{name}/{name}-{time}.csv";
			}
		}

		private void Start() {
			if (_rapl != null)
				_rapl.Start();
			else
				throw new NotSupportedException("Rapl has not been initialized");
		}

		private void End() {
			if (_rapl != null)
				_rapl.End();
			else
				throw new NotSupportedException("Rapl has not been initialized");

			//Result only valid if all results are valid
			//Only then is the result added and duration is incremented
			if (!_rapl.IsValid()) return;
			_resultBuffer.Add(_rapl.GetResults());
			_elapsedTime += _resultBuffer[^1].ElapsedTime / 1_000;
		}

		public void Run() {
			Run(_benchmark, _benchmarkOutput);
		}

		//Performns benchmarking
		//Writes progress to stdout if there is more than one iteration
		public void Run<TR>(Func<TR> benchmark, Action<TR> benchmarkOutput) {
			//Sets console to write to null
			Console.SetOut(_benchmarkOutputStream);

			_rapl = new RAPL();

			_elapsedTime = 0;
			_resultBuffer.Clear();
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
			using var writer = new StreamWriter(_outputFilePath);
			using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
				{ Delimiter = ";" });
			csv.WriteRecords(GetResults());
		}

		public List<BenchmarkResult> GetResults() {
			var result = new List<BenchmarkResult>(_resultBuffer);
			return result;
		}
	}
}