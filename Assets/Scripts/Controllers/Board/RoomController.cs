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

    public int RemainingCount => RemainingCards().Count;

    public bool IsEmpty => roomModel.Slots.Count(x => x.IsEmpty) == roomModel.Slots.Length;

    public RoomSlot[] Slots => roomModel.Slots;

    public RuntimeCardModel[] GetAllCards()
    {
        int amount = roomModel.Slots.Sum(slot => slot.Cards.Count);
        List<RuntimeCardModel> allCards = new ();
        foreach(RoomSlot slot in roomModel.Slots)
        {
            allCards.AddRange(slot.Cards);
        }
        return allCards.ToArray();
    }

    public List<RuntimeCardModel> RemainingCards() => GetAllCards().Where(card => card != null).ToList();

    public bool Contains(RuntimeCardModel card) => GetAllCards().Contains(card);

    public void RunCardsOnUpdate()
    {
        foreach (RuntimeCardModel card in RemainingCards())
        {
            card.Update();
        }
    }

    public bool CanGoToNextRoom()
    {
        List<RuntimeCardModel> remainingCards = RemainingCards();
        int amountRemaining = remainingCards.Count;
        foreach(RuntimeCardModel cardModel in remainingCards)
        {
            if (cardModel.CardType == CardType.DOOR)
            {
                amountRemaining -= 1;
            }
        }
        return amountRemaining <= 1;
    }

    public int GetSlotIndexOf(RuntimeCardModel card)
    {
        int index = -1;
        for (int i = 0; i < roomModel.Slots.Length; i++)
        {
            RoomSlot slot = roomModel.Slots[i];
            if (slot.Cards.Contains(card))
            {
                index = i;
            }
        }

        if (index < 0)
        {
            Debug.LogWarning("card is not in the current room!");
        }
        return index;
    }

    public RoomSlot GetSlotOf(RuntimeCardModel card)
    {
        for (int i = 0; i < roomModel.Slots.Length; i++)
        {
            RoomSlot slot = roomModel.Slots[i];
            if (slot.Cards.Contains(card))
            {
                return slot;
            }
        }

        Debug.LogWarning($"{card} is not in the current room!");
        return null;
    }

    public RoomSlot GetSlotOfindex(int index)
    {
        if (index < 0 || index >= roomModel.Slots.Count())
        {
            Debug.LogError("List index out of range!");
            return null;
        }

        return roomModel.Slots[index];
    }

    public List<RuntimeCardModel> GetActiveNeighbors(RuntimeCardModel card, bool wrap=false)
    {
        if (!Contains(card))
        {
            Debug.LogWarning($"{card} is not in the current room!");
            return new();
        }

        RoomSlot roomSlot = GetSlotOf(card);
        List<RuntimeCardModel> neighbors = new();
        foreach(RoomSlot neighborSlot in GetNeighborSlots(roomSlot, wrap))
        {
            RuntimeCardModel neighborCard = neighborSlot.ActiveCard;
            if (neighborCard != null)
            {
                neighbors.Add(neighborCard);
            }
        }

        return neighbors;
    }

    public List<RuntimeCardModel> GetActiveNeighbors(RuntimeCardModel card, List<CardType> cardTypes, bool wrap=false)
    {
        return GetActiveNeighbors(card, wrap)
               .Where(neighbor => cardTypes.Contains(neighbor.CardType)).ToList();
    }

    public List<RuntimeCardModel> GetActiveNeighbors(RuntimeCardModel card, List<Suit> acceptedSuits, bool wrap=false)
    {
        return GetActiveNeighbors(card, wrap)
               .Where(neighbor => acceptedSuits.Contains(neighbor.Suit)).ToList();
    }

    public List<RoomSlot> GetNeighborSlots(RoomSlot slot, bool wrap=false)
    {
        int originalIndex = Array.IndexOf(roomModel.Slots, slot);
        List<int> neighborIndices = new();
        if (originalIndex == 0)
        {
            if (wrap)
            {
                neighborIndices.Add(roomModel.Slots.Count() - 1);
            }
            neighborIndices.Add(originalIndex + 1);
        }
        else if (originalIndex == roomModel.Slots.Count() - 1)
        {
            if (wrap)
            {
                neighborIndices.Add(0);
            }
            neighborIndices.Add(originalIndex - 1);
        }
        else
        {
            neighborIndices.Add(originalIndex - 1);
            neighborIndices.Add(originalIndex + 1);
        }

        List<RoomSlot> neighborSlots = new();
        foreach(int index in neighborIndices)
        {
            RoomSlot neighborSlot = roomModel.Slots[index];
            if (neighborSlots.Contains(neighborSlot))
            {
                continue;
            }
            neighborSlots.Add(neighborSlot);
        }
        return neighborSlots;
    }

    public List<RuntimeCardModel> GetOthers(RuntimeCardModel card)
    {
        return RemainingCards().Where(other => other != card).ToList();
    }

    public bool TryAddCard(RuntimeCardModel card, int slotIndex, int cardIndex = 0)
    {
        return TryAddCard(card, roomModel.Slots[slotIndex], cardIndex);
    }

    public bool TryAddCard(RuntimeCardModel card, RoomSlot slot, int cardIndex = 0)
    {
        if (cardIndex < slot.Cards.Count && slot.Cards[cardIndex] != null)
        {
            Debug.LogWarning($"Tried to add card: {card} to slot {Array.IndexOf(roomModel.Slots, slot)} - {cardIndex}, but the index already has a card!");
            return false;
        }
        if (slot.Contains(card))
        {
            Debug.LogWarning($"Tried to add card: {card} to slot {Array.IndexOf(roomModel.Slots, slot)} - {cardIndex}, but that instance is already in the slot!");
            return false;
        }

        slot.AddAt(card, cardIndex);
        OnCardsChanged?.Invoke();
        return true;
    }

    public bool TryRemoveCard(RuntimeCardModel card)
    {
        if (!Contains(card))
        {
            Debug.LogWarning($"{card} is not in the current room!");
            return false;
        }

        RemoveCard(card);
        return true;
    }

    private void RemoveCard(RuntimeCardModel card)
    {
        foreach(RoomSlot slot in roomModel.Slots)
        {
            if (slot.Contains(card))
            {
                slot.Remove(card);
                OnCardsChanged?.Invoke();
                return;
            }
        }
        Debug.LogWarning($"Could not find {card} in room");
    }

    public void ClearCards()
    {
        for (int i = 0; i < roomModel.Slots.Length; i++)
        {
            roomModel.Slots[i].Clear();
        }
        OnCardsChanged?.Invoke();
    }

    public List<RuntimeCardModel> PopDoorCards()
    {
        // TODO: Refactor this to not just be used on doors, but also on any other cards that need to be reshuffled into the deck
        // Should this take the deckcontroller as an argument?
        return PopCardsFromPredicate((card) => { return card is DoorCardModel; });
    }

    public List<RuntimeCardModel> PopNonPersistantCards()
    {
        return PopCardsFromPredicate((card) => { return !card.PersistsThroughRun; });
    }

    /// <summary>
    /// Iterate through all of the remaining cards and check against a predicate.
    /// Pop all cards from the list if the predicate returns true, and return the popped cards.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    private List<RuntimeCardModel> PopCardsFromPredicate(Func<RuntimeCardModel, bool> predicate)
    {
        List<RuntimeCardModel> poppedCards = new();
        foreach(RuntimeCardModel card in RemainingCards())
        {
            if (card != null && predicate(card))
            {
                TryRemoveCard(card);
                poppedCards.Add(card);
            }
        }
        return poppedCards;
    }
}
