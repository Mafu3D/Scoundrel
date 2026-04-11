using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] public DeckManager DeckManager;

    public int RoomNumber = 0;

    public bool HasRunToken;
    public int ExtraRunTokens = 0;

    public Action OnStartNewGame;
    public Action OnEnterNewRoom;
    public Action OnCardsChanged;

    public Room CurrentRoom = null;
    private int roomSize = 4;
    private int remainingToMove = 1;

    public bool CanGoToNextRoom => CurrentRoom != null && CurrentRoom.RemainingCount <= remainingToMove;

    public bool GameHasStarted {get; private set;}

    public void StartNewGame()
    {
        DeckManager.ResetDeck();
        OnStartNewGame?.Invoke();
        EnterFirstRoom();
        GameHasStarted = true;
    }

    public void EnterFirstRoom()
    {
        List<Card> drawnCards = DeckManager.Draw(roomSize);
        Room room = new(roomSize, drawnCards);
        CurrentRoom = room;

        OnEnterNewRoom?.Invoke();
    }

    public void EnterNewRoom()
    {
        if (!CanGoToNextRoom) return;

        List<Card> newCards = new();

        newCards.AddRange(CurrentRoom.RemainingCards());
        List<Card> drawnCards = DeckManager.Draw(roomSize - CurrentRoom.RemainingCount);
        newCards.AddRange(drawnCards);

        Room NextRoom = new(roomSize, newCards);
        CurrentRoom = NextRoom;

        OnEnterNewRoom?.Invoke();
    }

    public void OnCardClicked(int index)
    {
        Card card = CurrentRoom.Cards[index];
        if (CurrentRoom.TryRemoveCard(card))
        {
            OnCardsChanged?.Invoke();
        }
    }
}

public class Room
{
    public int Size = 4;
    public Card[] Cards;

    public Room(int roomSize, List<Card> cards)
    {
        Cards = new Card[roomSize];

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = cards[i];
        }
    }

    public List<Card> RemainingCards()
    {
        List<Card> remaining = new();
        foreach (var card in Cards)
        {
            if (card != null)
            {
                remaining.Add(card);
            }
        }
        return remaining;
    }

    public bool TryRemoveCard(Card card)
    {
        if (!Cards.Contains(card))
        {
            Debug.LogWarning("card is not in the current room!");
            return false;
        }

        int index = Array.IndexOf(Cards, card);
        Cards[index] = null;
        return true;
    }

    public int RemainingCount => RemainingCards().Count;
}
