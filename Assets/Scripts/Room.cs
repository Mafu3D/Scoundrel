using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using System;
using System.Linq;

public class RoomModel
{
    public int Size = 4;
    public CardModel[] Cards;

    public Action OnCardsChanged;

    public RoomModel(int roomSize, List<CardModel> cards)
    {
        Cards = new CardModel[roomSize];

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = cards[i];
        }
    }

    public void InitializeRoom()
    {
        foreach (CardModel card in Cards)
        {
            if (card != null)
            {
                card.HandleOnDraw();
            }
        }
    }

    public void Update()
    {
        foreach (CardModel card in Cards)
        {
            if (card != null)
            {
                card.Update();
            }
        }
    }

    public bool CanGoToNextRoom()
    {
        List<CardModel> remaining = RemainingCards();
        int remainingCount = remaining.Count;
        foreach(CardModel cardModel in remaining)
        {
            if (cardModel.Suit == Suit.DOORS)
            {
                remainingCount -= 1;
            }
        }
        return remainingCount <= 1;
    }

    public List<CardModel> RemainingCards()
    {
        List<CardModel> remaining = new();
        foreach (var card in Cards)
        {
            if (card != null)
            {
                remaining.Add(card);
            }
        }
        return remaining;
    }

    public List<CardModel> GetNeighbors(CardModel card)
    {
        if (!Cards.Contains(card))
        {
            Debug.LogWarning("card is not in the current room!");
            return default;
        }

        int index = Array.IndexOf(Cards, card);
        List<CardModel> neighbors = new();
        if (index > 0)
        {
            CardModel neighbor = Cards[index - 1];
            if (neighbor != null && (neighbor.Suit == Suit.SPADES || neighbor.Suit == Suit.CLUBS))
            {
                neighbors.Add(neighbor);
            }
        }
        if (index < Size - 1)
        {
            CardModel neighbor = Cards[index + 1];
            if (neighbor != null && (neighbor.Suit == Suit.SPADES || neighbor.Suit == Suit.CLUBS))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    public List<CardModel> GetOthers(CardModel card)
    {
        if (!Cards.Contains(card))
        {
            Debug.LogWarning("card is not in the current room!");
            return default;
        }

        List<CardModel> others = new();
        foreach (CardModel other in Cards)
        {
            if (other != null && other != card)
            {
                others.Add(other);
            }
        }
        return others;
    }

    public bool TryRemoveCard(CardModel card)
    {
        if (!Cards.Contains(card))
        {
            Debug.LogWarning("card is not in the current room!");
            return false;
        }

        RemoveCard(card);
        return true;
    }

    private void RemoveCard(CardModel card)
    {
        int index = Array.IndexOf(Cards, card);
        card.HandleDeath();
        Cards[index] = null;
        OnCardsChanged?.Invoke();
    }

    public void ClearCards()
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = null;
        }
        OnCardsChanged?.Invoke();
    }

    public void DEBUG_REMOVECARD(CardModel card)
    {
        RemoveCard(card);
    }

    public int RemainingCount => RemainingCards().Count;

    public bool IsEmpty => Cards.Count(x => x == null) == Cards.Length;

    public void OnRun()
    {
        foreach(CardModel card in Cards)
        {
            if (card != null)
            {
                card.BuffManager.TriggerEffect(BuffTrigger.OnRun);
            }
        }
    }
}
