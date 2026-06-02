using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using System;
using System.Linq;

public class RoomModel
{
    public int Size = 4;
    public RuntimeCardModel[] Cards;

    public Action OnCardsChanged;

    public RoomModel(int roomSize, List<RuntimeCardModel> cards)
    {
        Cards = new RuntimeCardModel[roomSize];

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = cards[i];
        }
    }

    public void InitializeRoom()
    {
        foreach (RuntimeCardModel card in Cards)
        {
            if (card != null)
            {
                card.HandleOnDraw();
            }
        }
    }

    public void Update()
    {
        foreach (RuntimeCardModel card in Cards)
        {
            if (card != null)
            {
                card.Update();
            }
        }
    }

    public bool CanGoToNextRoom()
    {
        List<RuntimeCardModel> remaining = RemainingCards();
        int remainingCount = remaining.Count;
        foreach(RuntimeCardModel cardModel in remaining)
        {
            if (cardModel.Suit == Suit.DOORS)
            {
                remainingCount -= 1;
            }
        }
        return remainingCount <= 1;
    }

    public List<RuntimeCardModel> RemainingCards()
    {
        List<RuntimeCardModel> remaining = new();
        foreach (var card in Cards)
        {
            if (card != null)
            {
                remaining.Add(card);
            }
        }
        return remaining;
    }

    public List<RuntimeCardModel> GetNeighbors(RuntimeCardModel card)
    {
        if (!Cards.Contains(card))
        {
            Debug.LogWarning("card is not in the current room!");
            return default;
        }

        int index = Array.IndexOf(Cards, card);
        List<RuntimeCardModel> neighbors = new();
        if (index > 0)
        {
            RuntimeCardModel neighbor = Cards[index - 1];
            if (neighbor != null && (neighbor.Suit == Suit.SPADES || neighbor.Suit == Suit.CLUBS))
            {
                neighbors.Add(neighbor);
            }
        }
        if (index < Size - 1)
        {
            RuntimeCardModel neighbor = Cards[index + 1];
            if (neighbor != null && (neighbor.Suit == Suit.SPADES || neighbor.Suit == Suit.CLUBS))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    public List<RuntimeCardModel> GetOthers(RuntimeCardModel card)
    {
        if (!Cards.Contains(card))
        {
            Debug.LogWarning("card is not in the current room!");
            return default;
        }

        List<RuntimeCardModel> others = new();
        foreach (RuntimeCardModel other in Cards)
        {
            if (other != null && other != card)
            {
                others.Add(other);
            }
        }
        return others;
    }

    public bool TryRemoveCard(RuntimeCardModel card)
    {
        if (!Cards.Contains(card))
        {
            Debug.LogWarning("card is not in the current room!");
            return false;
        }

        RemoveCard(card);
        return true;
    }

    private void RemoveCard(RuntimeCardModel card)
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

    public void DEBUG_REMOVECARD(RuntimeCardModel card)
    {
        RemoveCard(card);
    }

    public int RemainingCount => RemainingCards().Count;

    public bool IsEmpty => Cards.Count(x => x == null) == Cards.Length;
}
