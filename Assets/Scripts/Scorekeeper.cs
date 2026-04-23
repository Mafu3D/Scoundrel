using Project.Decks;

public class ScoreKeeper
{
    private GameManager gameManager;

    public ScoreKeeper(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public bool HasPlayerWon()
    {
        int monsterScore = 0;
        foreach (CardModel card in gameManager.DeckManager.Deck.RemainingItems)
        {
            if (card.Suit == Suit.SPADES || card.Suit == Suit.CLUBS)
            {
                monsterScore += card.Value;
            }
        }
        foreach (CardModel card in gameManager.CurrentRoom.Cards)
        {
            if (card != null && (card.Suit == Suit.SPADES || card.Suit == Suit.CLUBS))
            {
                monsterScore += card.Value;
            }
        }
        if (gameManager.DeckManager.Deck.CurrentCount == 0 && monsterScore == 0)
        {
            return true;
        }
        return false;
    }

    public int GetScore()
    {
        int monsterScore = 0;
        bool monstersInRoom = false;
        foreach (CardModel card in gameManager.DeckManager.Deck.RemainingItems)
        {
            if (card.Suit == Suit.SPADES || card.Suit == Suit.CLUBS)
            {
                monsterScore += card.Value;
            }
        }
        foreach (CardModel card in gameManager.CurrentRoom.Cards)
        {
            if (card != null && (card.Suit == Suit.SPADES || card.Suit == Suit.CLUBS))
            {
                monstersInRoom = true;
                monsterScore += card.Value;
            }
        }

        if (gameManager.DeckManager.Deck.CurrentCount == 0 && !monstersInRoom)
        {
            if (gameManager.Player.CurrentHealth == gameManager.Player.MaxHealth)
            {
                int potionScore = 0;
                foreach (CardModel card in gameManager.CurrentRoom.Cards)
                {
                    if (card.Suit == Suit.HEARTS)
                    {
                        if (card.Value > potionScore)
                        {
                            potionScore = card.Value;
                        }
                    }
                }
                return gameManager.Player.MaxHealth + potionScore;
            }
            else
            {
                return gameManager.Player.CurrentHealth;
            }
        }
        return gameManager.Player.CurrentHealth - monsterScore;
    }
}