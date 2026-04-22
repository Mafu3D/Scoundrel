using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using System;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] public DeckManager DeckManager;
    [SerializeField] public Player Player;

    public bool GameHasStarted {get; private set;}
    public int RoomNumber { get; private set; }= 0;
    public Room CurrentRoom = null;

    public Action OnStartNewGame;
    public Action OnGameOver;
    public Action OnEnterNewRoom;
    public Action OnCardsChanged;

    private int roomSize = 4;
    private int remainingToMove = 1;

    public bool CanGoToNextRoom => CurrentRoom != null && CurrentRoom.RemainingCount <= remainingToMove;

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    public void StartNewGame()
    {
        RoomNumber = 0;
        DeckManager.ResetDeck();
        OnStartNewGame?.Invoke();
        EnterFirstRoom();
        GameHasStarted = true;

        Player.OnRunSuccess += OnPlayerRun;
        Player.OnDeath += GameOver;
        Player.StartNewGame();
    }

    private void EndGame()
    {
        Player.OnRunSuccess -= OnPlayerRun;
        Player.OnDeath -= GameOver;
    }

    private void GameOver()
    {
        EndGame();
        OnGameOver?.Invoke();
    }

    public int GetScore()
    {
        int monsterScore = 0;
        bool monstersInRoom = false;
        foreach (CardModel card in DeckManager.Deck.RemainingItems)
        {
            if (card.Suit == Suit.SPADES || card.Suit == Suit.CLUBS)
            {
                monsterScore += card.Value;
            }
        }
        foreach (CardModel card in CurrentRoom.Cards)
        {
            if (card != null && (card.Suit == Suit.SPADES || card.Suit == Suit.CLUBS))
            {
                monstersInRoom = true;
                monsterScore += card.Value;
            }
        }

        if (DeckManager.Deck.CurrentCount == 0 && !monstersInRoom)
        {
            if (Player.CurrentHealth == Player.MaxHealth)
            {
                int potionScore = 0;
                foreach (CardModel card in CurrentRoom.Cards)
                {
                    if (card.Suit == Suit.HEARTS)
                    {
                        if (card.Value > potionScore)
                        {
                            potionScore = card.Value;
                        }
                    }
                }
                return Player.MaxHealth + potionScore;
            }
            else
            {
                return Player.CurrentHealth;
            }
        }
        return Player.CurrentHealth - monsterScore;
    }

    private void EnterFirstRoom()
    {
        List<CardModel> drawnCards = DeckManager.Draw(roomSize);
        Room room = new(roomSize, drawnCards);
        CurrentRoom = room;

        RoomNumber++;

        OnEnterNewRoom?.Invoke();
    }

    private void EnterNewRoom()
    {
        if (!CanGoToNextRoom) return;

        List<CardModel> newCards = new();

        newCards.AddRange(CurrentRoom.RemainingCards());
        List<CardModel> drawnCards = DeckManager.Draw(roomSize - CurrentRoom.RemainingCount);
        newCards.AddRange(drawnCards);

        Room NextRoom = new(roomSize, newCards);
        CurrentRoom = NextRoom;
        RoomNumber++;

        Player.RoundReset();

        OnEnterNewRoom?.Invoke();
    }

    public void OnCardClicked(CardModel card, CardClickContext context)
    {
        if (!CurrentRoom.Cards.Contains(card))
        {
            return;
        }

        // Try to handle the player action based on the card suit
        bool success = card.Suit switch
        {
            Suit.HEARTS => HandlePotion(card),
            Suit.DIAMONDS => Player.TryEquipWeapon(card),
            Suit.SPADES or Suit.CLUBS => HandleEnemy(card, context),
            _ => false
        };

        if (success)
        {
            if (!Player.HasEnteredTheRoom)
            {
                Player.EnterNewRoom();
            }
            CurrentRoom.TryRemoveCard(card);
            OnCardsChanged?.Invoke();
        }
    }

    private bool HandlePotion(CardModel card)
    {
        Player.TryDrinkPotion(card);
        return true;
    }

    private bool HandleEnemy(CardModel card, CardClickContext context) => context == CardClickContext.TOP ? Player.TryFightWeapon(card) : Player.TryFightUnarmed(card);

    private void OnPlayerRun()
    {
        DeckManager.Deck.AddToRemaining(CurrentRoom.Cards.ToList(), addToTop: false, shuffle: false);
        CurrentRoom.ClearCards();
        EnterNewRoom();
    }
}
