using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using TMPro;
using System;

public class DeckManager : MonoBehaviour
{
    public Deck<CardModel> Deck { get; private set; }

    public Action OnCardDraw;

    public void ResetDeck()
    {
        // Create black cards
        List<CardModel> cards = new();
        for (int i = 2; i <= 14; i++)
        {
            cards.Add(new CardModel(Suit.CLUBS, i));
            cards.Add(new CardModel(Suit.SPADES, i));
        }

        // Create red cards
        for (int i = 2; i <= 10; i++)
        {
            cards.Add(new CardModel(Suit.HEARTS, i));
            cards.Add(new CardModel(Suit.DIAMONDS, i));
        }

        // Special cards
        cards.Add(new CardModel(Suit.DOORS, 14));
        cards.Add(new CardModel(Suit.DOORS, 14));
        cards.Add(new CardModel(Suit.TREASURES, 14));
        cards.Add(new CardModel(Suit.TREASURES, 14));

        // Create deck
        Deck = new Deck<CardModel>(cards);
        Deck.Shuffle();
    }


    public List<CardModel> Draw(int amount)
    {
        List<CardModel> drawn = Deck.DrawMultiple(amount);
        OnCardDraw?.Invoke();
        return drawn;
    }

    public int RemainingOfSuit(Suit suit)
    {
        int remaining = 0;
        foreach (CardModel card in Deck.RemainingItems)
        {
            if (card.Suit == suit)
            {
                remaining++;
            }
        }
        return remaining;
    }

    public int RemainingOfSuit(List<Suit> suits)
    {
        int remaining = 0;
        foreach (CardModel card in Deck.RemainingItems)
        {
            if (suits.Contains(card.Suit))
            {
                remaining++;
            }
        }
        return remaining;
    }

    public List<CardModel> GetRemainingOfSuit(List<Suit> suits)
    {
        List<CardModel> remaining = new();
        foreach (CardModel card in Deck.RemainingItems)
        {
            if (suits.Contains(card.Suit))
            {
                remaining.Add(card);
            }
        }
        return remaining;
    }
}
