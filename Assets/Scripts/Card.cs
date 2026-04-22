using System;

namespace Project.Decks
{
    public enum Suit {
        SPADES,
        HEARTS,
        DIAMONDS,
        CLUBS
    }

    public class CardModel : IDeckStorable
    {
        public Suit Suit { get; private set; }

        public int Value { get; private set; }

        public string ID => uuid.ToString();

        private Guid uuid;

        public CardModel(Suit suit, int value) {
            this.Suit = suit;
            this.Value = value;

            uuid = Guid.NewGuid();
        }

        public override string ToString()
        {
            return $"{Value} of {Suit}";
        }
    }
}