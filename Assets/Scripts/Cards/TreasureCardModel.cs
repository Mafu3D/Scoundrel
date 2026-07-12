using Project.Decks;

public class TreasureCardModel : RuntimeCardModel
{
    public TreasureCardModel(Suit suit, int value) : base(suit,  CardType.TREASURE, value)
    {
    }

    public override bool TryUse(Player player, GameManager gameManager)
    {
        player.AddGold(this.Value);
        return true;
    }

    public override bool TryUseBot(Player player, GameManager gameManager) => TryUse(player, gameManager);
    public override bool TryUseTop(Player player, GameManager gameManager) => TryUse(player, gameManager);
}