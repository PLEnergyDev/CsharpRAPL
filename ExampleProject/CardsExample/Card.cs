namespace TestProject.CardsExample {
	public class Card {
		Suit Example { get; }
		Value Value { get; }
		public Card(Suit s, Value v) => (Example, Value) = (s, v);
		public override string ToString() => $"{Value} of {Example}";
	}
}