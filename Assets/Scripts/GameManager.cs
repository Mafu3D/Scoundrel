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

    public int RoomNumber = 0;

    public bool HasRunToken = true;
    private bool usedRunTokenLastRound = false;
    public int ExtraRunTokens = 0;
    public bool HasDrankPotionThisRoom { get; private set; } = false;
    public bool HasEnteredTheRoom { get; private set; } = false;

    public Action OnStartNewGame;
    public Action OnGameOver;
    public Action OnEnterNewRoom;
    public Action OnCardsChanged;

    public Room CurrentRoom = null;
    private int roomSize = 4;
    private int remainingToMove = 1;

    public bool CanGoToNextRoom => CurrentRoom != null && CurrentRoom.RemainingCount <= remainingToMove;

    public bool GameHasStarted {get; private set;}

    void OnEnable()
    {
        Player.OnDeath += GameOver;
    }

    void OnDisable()
    {
        Player.OnDeath -= GameOver;
    }

    public void StartNewGame()
    {
        RoomNumber = 0;
        DeckManager.ResetDeck();
        OnStartNewGame?.Invoke();
        EnterFirstRoom();
        GameHasStarted = true;
    }

    private void GameOver()
    {
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

    public void EnterFirstRoom()
    {
        List<CardModel> drawnCards = DeckManager.Draw(roomSize);
        Room room = new(roomSize, drawnCards);
        CurrentRoom = room;

        RoomNumber++;

        OnEnterNewRoom?.Invoke();
    }

    public void EnterNewRoom()
    {
        if (!CanGoToNextRoom) return;

        List<CardModel> newCards = new();

        newCards.AddRange(CurrentRoom.RemainingCards());
        List<CardModel> drawnCards = DeckManager.Draw(roomSize - CurrentRoom.RemainingCount);
        newCards.AddRange(drawnCards);

        Room NextRoom = new(roomSize, newCards);
        CurrentRoom = NextRoom;

        RoomNumber++;
        if (usedRunTokenLastRound)
        {
            usedRunTokenLastRound = false;
            if (!HasRunToken)
            {
                HasRunToken = true;
            }
        }

        HasEnteredTheRoom = false;

        OnEnterNewRoom?.Invoke();
    }

    public void OnCardClicked(CardModel card, CardClickContext context)
    {
        bool success = false;

        if (!CurrentRoom.Cards.Contains(card))
        {
            return;
        }

        switch(card.Suit)
        {
            case Suit.HEARTS:
                if (!HasDrankPotionThisRoom)
                {
                    DrinkPotion(card.Value);
                    success = true;
                }
                else
                {
                    success = true;
                }
                break;
            case Suit.DIAMONDS:
                EquipWeapon(card);
                success = true;
                break;
            case Suit.SPADES:
            case Suit.CLUBS:
                if (context == CardClickContext.TOP)
                {
                    if (Player.Weapon == null || Player.Weapon.GetCurrentStrength() <= card.Value)
                    {
                        success = false;
                    }
                    else
                    {
                        FightWeapon(card);
                        success = true;
                    }
                }
                else if (context == CardClickContext.BOT)
                {
                    FightUnarmed(card);
                    success = true;
                }
                break;
        }

        if (success)
        {
            if (!HasEnteredTheRoom)
            {
                HasEnteredTheRoom = true;
            }
            CurrentRoom.TryRemoveCard(card);
            OnCardsChanged?.Invoke();
        }
    }
    public void Run()
    {
        if (!HasRunToken && ExtraRunTokens <= 0 || HasEnteredTheRoom)
        {
            Debug.Log("tried to run but can't");
            return;
        }

        DeckManager.Deck.AddToRemaining(CurrentRoom.Cards.ToList(), addToTop: false, shuffle: false);
        CurrentRoom.ClearCards();
        EnterNewRoom();

        if (HasRunToken)
        {
            HasRunToken = false;
            usedRunTokenLastRound = true;
        }
    }

    public bool CanRun() => (HasRunToken || ExtraRunTokens > 0) && !HasEnteredTheRoom;

    private void FightWeapon(CardModel card)
    {
        int damage = Math.Clamp(card.Value - Player.Weapon.Power, 0, 999);
        Player.TakeDamage(damage);
        Player.Weapon.AddMonsterToSlain(card);
    }

    private void FightUnarmed(CardModel card)
    {
        Player.TakeDamage(card.Value);
    }

    private void EquipWeapon(CardModel card)
    {
        Player.EquipWeapon(card);
    }

    private void DrinkPotion(int value)
    {
        Player.Heal(value);
    }

    private void CheckForGameOver()
    {

    }
}
