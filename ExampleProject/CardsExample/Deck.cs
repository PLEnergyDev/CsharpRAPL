using System;
using System.Collections.Generic;

namespace TestProject.CardsExample {
	public class Deck {
		private readonly List<Card> _deck = new();
		private Random Random { get; set; }

		public Deck() {
			Random = new Random(2);
			foreach (Suit s in Enum.GetValues(typeof(Suit)))
			foreach (Value v in Enum.GetValues(typeof(Value)))
				_deck.Add(new Card(s, v));
		}

		public int Count => _deck.Count;

		public void Shuffle() {
			for (var i = 0; i < _deck.Count; i++) {
				int r = Random.Next(i, _deck.Count);
				(_deck[i], _deck[r]) = (_deck[r], _deck[i]);
			}
		}

		public Card Deal() {
			int last = _deck.Count - 1;
			Card card = _deck[last];
			_deck.RemoveAt(last);
			return card;
		}

		public string ShowDeck() => string.Join('\n', _deck);
	}
}