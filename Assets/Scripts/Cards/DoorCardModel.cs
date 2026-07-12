using Project.Decks;

public class DoorCardModel : RuntimeCardModel
{
    public DoorCardModel(Suit suit, int value) : base(suit,  CardType.DOOR, value)
    {
    }

    public override bool TryUse(Player player, GameManager gameManager)
    {
        if (gameManager.ScoreKeeper.GetScore() >= gameManager.GetScoreToGoToNextFloor())
        {
            gameManager.GoToExitFloorState();
            return true;
        }
        return false;
    }

    public override bool TryUseBot(Player player, GameManager gameManager) => TryUse(player, gameManager);

    public override bool TryUseTop(Player player, GameManager gameManager) => TryUse(player, gameManager);
}
