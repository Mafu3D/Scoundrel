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

    public int GetIndexOf(RuntimeCardModel card)
    {
        if (!Cards.Contains(card))
        {
            Debug.LogWarning("card is not in the current room!");
            return -1;
        }

        return Array.IndexOf(Cards, card);
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
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }
        if (index < Size - 1)
        {
            RuntimeCardModel neighbor = Cards[index + 1];
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    public List<RuntimeCardModel> GetNeighbors(RuntimeCardModel card, List<Suit> acceptedSuits)
    {
        List<RuntimeCardModel> unfilteredNeighbors = GetNeighbors(card);
        List<RuntimeCardModel> filteredNeighbors = new();
        foreach(RuntimeCardModel neighbor in unfilteredNeighbors)
        {
            if (!acceptedSuits.Contains(neighbor.Suit))
            {
                continue;
            }
            filteredNeighbors.Add(neighbor);
        }
        return filteredNeighbors;
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
        card.HandleDeathPreRemoval();
        Cards[index] = null;
        card.HandleDeathPostRemoval();
        OnCardsChanged?.Invoke();
    }

    public void ClearCards(bool keepPersistentThroughRun = false)
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            if (Cards[i] != null && (keepPersistentThroughRun && Cards[i].PersistsThroughRun))
            {
                continue;
            }
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
