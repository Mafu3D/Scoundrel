using Project.Decks;
using UnityEngine;

public class DoorCardModel : RuntimeCardModel
{
    public DoorCardModel(Suit suit, int value) : base(suit,  CardType.DOOR, value)
    {
    }

    public override bool TryUse(Player player, GameManager gameManager)
    {
        Debug.Log("use door");
        if (gameManager.ScoreKeeper.GetScore() >= gameManager.GetScoreToGoToNextFloor())
        {
            Debug.Log("user door success!");
            gameManager.GoToExitFloorState();
            return true;
        }
        return false;
    }

    public override bool TryUseBot(Player player, GameManager gameManager) => TryUse(player, gameManager);

    public override bool TryUseTop(Player player, GameManager gameManager) => TryUse(player, gameManager);
}
