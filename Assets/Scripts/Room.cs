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
    }

    public void DEBUG_REMOVECARD(CardModel card)
    {
        RemoveCard(card);
    }

    public int RemainingCount => RemainingCards().Count;

    public void OnRun()
    {
        foreach(CardModel card in Cards)
        {
            if (card != null)
            {
                card.Buffs.OnRun();
            }
        }
    }
}
