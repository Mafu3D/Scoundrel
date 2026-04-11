using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using TMPro;
using System;

public class DeckManager : MonoBehaviour
{
    public Deck<Card> Deck { get; private set; }

    public Action OnCardDraw;

    public void ResetDeck()
    {
        // Create black cards
        List<Card> cards = new();
        for (int i = 2; i <= 14; i++)
        {
            cards.Add(new Card(Suit.CLUBS, i));
            cards.Add(new Card(Suit.SPADES, i));
        }

        // Create red cards
        for (int i = 2; i <= 10; i++)
        {
            cards.Add(new Card(Suit.HEARTS, i));
            cards.Add(new Card(Suit.DIAMONDS, i));
        }

        // Create deck
        Deck = new Deck<Card>(cards);
    }


    public List<Card> Draw(int amount)
    {
        List<Card> drawn = Deck.DrawMultiple(amount);
        OnCardDraw?.Invoke();
        return drawn;
    }
}
