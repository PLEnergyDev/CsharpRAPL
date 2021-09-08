namespace ExampleProject.CardsExample {
	public class Card {
		private Suit Example { get; }
		private Value Value { get; }
		public Card(Suit s, Value v) => (Example, Value) = (s, v);
		public override string ToString() => $"{Value} of {Example}";
	}
}