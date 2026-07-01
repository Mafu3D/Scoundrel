using System;
using System.Collections.Generic;
using System.Linq;
using Project.Decks;
using UnityEngine;

public class RoomController
{
    public Action OnCardsChanged;

    private readonly RoomModel roomModel;

    public RoomController(RoomModel roomModel)
    {
        this.roomModel = roomModel;
    }

    public void RunCardsOnUpdate()
    {
        foreach (RuntimeCardModel card in roomModel.Cards)
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

    public RuntimeCardModel[] GetCards() => roomModel.Cards;

    public List<RuntimeCardModel> RemainingCards()
    {
        List<RuntimeCardModel> remaining = new();
        foreach (var card in roomModel.Cards)
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
        if (!roomModel.Cards.Contains(card))
        {
            Debug.LogWarning("card is not in the current room!");
            return -1;
        }

        return Array.IndexOf(roomModel.Cards, card);
    }

    public List<RuntimeCardModel> GetNeighbors(RuntimeCardModel card)
    {
        if (!roomModel.Cards.Contains(card))
        {
            Debug.LogWarning("card is not in the current room!");
            return default;
        }

        int index = Array.IndexOf(roomModel.Cards, card);
        List<RuntimeCardModel> neighbors = new();
        if (index > 0)
        {
            RuntimeCardModel neighbor = roomModel.Cards[index - 1];
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }
        if (index < roomModel.Size - 1)
        {
            RuntimeCardModel neighbor = roomModel.Cards[index + 1];
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
        if (!roomModel.Cards.Contains(card))
        {
            Debug.LogWarning("card is not in the current room!");
            return default;
        }

        List<RuntimeCardModel> others = new();
        foreach (RuntimeCardModel other in roomModel.Cards)
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
        if (!roomModel.Cards.Contains(card))
        {
            Debug.LogWarning("card is not in the current room!");
            return false;
        }

        RemoveCard(card);
        return true;
    }

    private void RemoveCard(RuntimeCardModel card)
    {
        int index = Array.IndexOf(roomModel.Cards, card);
        card.HandleDeathPreRemoval();
        roomModel.Cards[index] = null;
        card.HandleDeathPostRemoval();
        OnCardsChanged?.Invoke();
    }

    public void ClearCards(bool keepPersistentThroughRun = false)
    {
        for (int i = 0; i < roomModel.Cards.Length; i++)
        {
            if (roomModel.Cards[i] != null && (keepPersistentThroughRun && roomModel.Cards[i].PersistsThroughRun))
            {
                continue;
            }
            roomModel.Cards[i] = null;
        }
        OnCardsChanged?.Invoke();
    }

    public List<RuntimeCardModel> PopDoorCards()
    {
        // TODO: Refactor this to not just be used on doors, but also on any other cards that need to be reshuffled into the deck
        // Should this take the deckcontroller as an argument?
        List<RuntimeCardModel> cardsToReshuffle = new();
        foreach(RuntimeCardModel card in RemainingCards())
        {
            if (card is DoorCardModel)
            {
                TryRemoveCard(card);
                cardsToReshuffle.Add(card);
            }
        }
        return cardsToReshuffle;
    }

    public int RemainingCount => RemainingCards().Count;

    public bool IsEmpty => roomModel.Cards.Count(x => x == null) == roomModel.Cards.Length;
}