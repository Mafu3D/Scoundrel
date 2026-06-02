using Project.Decks;

public class TreasureCardModel : RuntimeCardModel
{
    public TreasureCardModel(Suit suit, int value) : base(suit,  CardType.TREASURE, value)
    {
    }
}