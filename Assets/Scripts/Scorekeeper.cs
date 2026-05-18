using System;
using Project.Decks;

public interface IScoreKeeper
{
    public int GetScore();
}

public class AdvancedScoreKeeper : IScoreKeeper
{
    private readonly GameManager gameManager;
    private int score;

    private float roomMultipler = 0f;

    public AdvancedScoreKeeper(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void AddToScore(CardModel card)
    {
        if (card.Suit != Suit.CLUBS && card.Suit != Suit.SPADES)
        {
            return;
        }

        float floorMultiplier = (gameManager.FloorNumber - 1) * 0.2f;

        float cardScore = card.Value + (card.Value * floorMultiplier) + (card.Value * roomMultipler);

        score += (int)Math.Ceiling(cardScore);
    }

    public void IncRoomMultiplier()
    {
        roomMultipler += 0.5f;
    }

    public void ResetRoomMultiplier()
    {
        roomMultipler = 0;
    }

    public int GetScore() => score;

    public bool HasPlayerWon() => false; // REMOVE THIS!
}

public class ClassicScoreKeeper : IScoreKeeper
{
    private GameManager gameManager;

    public ClassicScoreKeeper(GameManager gameManager)
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
                monsterScore += card.BaseValue;
            }
        }
        foreach (CardModel card in gameManager.CurrentRoom.Cards)
        {
            if (card != null && (card.Suit == Suit.SPADES || card.Suit == Suit.CLUBS))
            {
                monsterScore += card.BaseValue;
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
                monsterScore += card.BaseValue;
            }
        }
        foreach (CardModel card in gameManager.CurrentRoom.Cards)
        {
            if (card != null && (card.Suit == Suit.SPADES || card.Suit == Suit.CLUBS))
            {
                monstersInRoom = true;
                monsterScore += card.BaseValue;
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
                        if (card.BaseValue > potionScore)
                        {
                            potionScore = card.BaseValue;
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