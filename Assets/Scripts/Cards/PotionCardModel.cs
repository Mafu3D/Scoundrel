using Project.Decks;

public class PotionCardModel : RuntimeCardModel
{
    public PotionCardModel(Suit suit, int value) : base(suit,  CardType.POTION, value)
    {
    }
}
