using Project.Decks;

public class PotionCardModel : RuntimeCardModel
{
    public bool IsArmor = false;
    public PotionCardModel(Suit suit, int value) : base(suit,  CardType.POTION, value) { }

    public override bool TryUse(Player player, GameManager gameManager)
    {
        if (IsArmor)
        {
            player.ClearArmor();
            player.AddArmor(this.Value);
            return true;
        }
        player.TryDrinkPotion(this);
        return true;
    }

    public override bool TryUseBot(Player player, GameManager gameManager) => TryUse(player, gameManager);

    public override bool TryUseTop(Player player, GameManager gameManager) => TryUse(player, gameManager);
}
