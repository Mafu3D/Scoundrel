using Project.Decks;
using UnityEngine;

public class MonsterCardModel : RuntimeCardModel
{
    public MonsterCardModel(Suit suit, int value) : base(suit,  CardType.MONSTER, value)
    {
    }

    public override bool TryUse(Player player, GameManager gameManager) { return false; }

    public override bool TryUseBot(Player player, GameManager gameManager)
    {
        return gameManager.CombatController.TryFightUnarmed(this);
    }

    public override bool TryUseTop(Player player, GameManager gameManager)
    {
        return gameManager.CombatController.TryFightWeapon(this);
    }
}
