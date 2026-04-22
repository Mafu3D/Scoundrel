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
}
