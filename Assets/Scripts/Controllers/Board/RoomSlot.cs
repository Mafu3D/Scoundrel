using System;
using System.Collections.Generic;
using System.Linq;
using Project.Decks;
using UnityEngine;

public class RoomSlot
{
    public Action OnCardsChanged;

    public List<RuntimeCardModel> Cards;

    public RoomSlot(List<RuntimeCardModel> cards)
    {
        Cards = cards ?? new();
    }

    public bool IsEmpty => Cards.Count == 0;

    public RuntimeCardModel ActiveCard => IsEmpty ? default : Cards[0];

    public List<RuntimeCardModel> GetCardsOfType(CardType cardType)
    {
        return Cards.Where(card => card.CardType == cardType).ToList();
    }

    public List<RuntimeCardModel> GetCardsOfType(List<CardType> cardTypes)
    {
        return Cards.Where(card => cardTypes.Contains(card.CardType)).ToList();
    }

    public bool Contains(RuntimeCardModel card) => Cards.Contains(card);

    public void Add(RuntimeCardModel card)
    {
        Cards.Add(card);
        OnCardsChanged?.Invoke();
    }

    public void Insert(RuntimeCardModel card, int index)
    {
        Cards.Insert(index, card);
        OnCardsChanged?.Invoke();
    }

    public void AddAt(RuntimeCardModel card, int index)
    {
        if (index < -1)
        {
            throw new IndexOutOfRangeException($"Cannot add a card to {index} of slot");
        }
        else if (index == -1 || index >= Cards.Count)
        {
            Add(card);
        }
        else
        {
            Insert(card, index);
        }
    }

    public void Remove(RuntimeCardModel card)
    {
        if (!Cards.Contains(card))
        {
            Debug.LogWarning($"{card} is not in this slot!");
            return;
        }
        Cards.Remove(card);
        OnCardsChanged?.Invoke();
    }

    public void Shift(RuntimeCardModel card, int amount)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        Cards = new();
        OnCardsChanged?.Invoke();
    }
}