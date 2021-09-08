using System;
using CsharpRAPL;
using ExampleProject.CardsExample;


int iterations = args.Length > 0 ? int.Parse(args[0]) : 1;
var bm = new Benchmark(iterations);

bm.Run(() => {
	var count = 0;
	for (var i = 0; i < 1000; i++) {
		var d = new Deck();
		while (d.Count > 0) {
			string deck = d.ShowDeck();
			d.Shuffle();
			d.Deal();
			// Count variable is to make sure ShowDeck() is not optimised away
			count += deck.Length;
		}
	}

	return count;
}, Console.WriteLine);

