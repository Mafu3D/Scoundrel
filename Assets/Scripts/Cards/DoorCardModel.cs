using Project.Decks;

public class DoorCardModel : RuntimeCardModel
{
    public DoorCardModel(Suit suit, int value) : base(suit,  CardType.DOOR, value)
    {
    }
}
