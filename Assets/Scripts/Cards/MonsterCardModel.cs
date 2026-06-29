using Project.Decks;

public class MonsterCardModel : RuntimeCardModel
{
    public MonsterCardModel(Suit suit, int value) : base(suit,  CardType.MONSTER, value)
    {
    }
}
