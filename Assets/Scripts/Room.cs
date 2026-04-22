using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using System;
using System.Linq;

public class Room
{
    public int Size = 4;
    public CardModel[] Cards;

    public Room(int roomSize, List<CardModel> cards)
    {
        Cards = new CardModel[roomSize];

        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = cards[i];
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

        int index = Array.IndexOf(Cards, card);
        Cards[index] = null;
        return true;
    }

    public void ClearCards()
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i] = null;
        }
    }

    public int RemainingCount => RemainingCards().Count;
}
