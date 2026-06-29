using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Decks;
using TMPro;
using System;

public class DeckManager : MonoBehaviour
{
    public Deck<RuntimeCardModel> Deck { get; private set; }

    public Action OnCardDraw;

    public void TEMP_CreateNewDeck()
    {
        // Create black cards
        List<RuntimeCardModel> cards = new();
        for (int i = 2; i <= 14; i++)
        {
            cards.Add(new MonsterCardModel(Suit.CLUBS, i));
            cards.Add(new MonsterCardModel(Suit.SPADES, i));
        }

        // Create red cards
        for (int i = 2; i <= 10; i++)
        {
            cards.Add(new PotionCardModel(Suit.HEARTS, i));
            cards.Add(new WeaponCardModel(Suit.DIAMONDS, i));
        }

        // Special cards
        cards.Add(new DoorCardModel(Suit.DOORS, 14));
        cards.Add(new DoorCardModel(Suit.DOORS, 14));
        cards.Add(new TreasureCardModel(Suit.TREASURES, 14));
        cards.Add(new TreasureCardModel(Suit.TREASURES, 14));

        // Create deck
        Deck = new Deck<RuntimeCardModel>(cards);
        Deck.Shuffle();
    }

    public void ResetDeck()
    {
        Deck.Reset();
    }


    public List<RuntimeCardModel> Draw(int amount)
    {
        List<RuntimeCardModel> drawn = Deck.DrawMultiple(amount);
        OnCardDraw?.Invoke();
        return drawn;
    }

    public int RemainingOfSuit(Suit suit)
    {
        int remaining = 0;
        foreach (RuntimeCardModel card in Deck.RemainingItems)
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
        foreach (RuntimeCardModel card in Deck.RemainingItems)
        {
            if (suits.Contains(card.Suit))
            {
                remaining++;
            }
        }
        return remaining;
    }

    public List<RuntimeCardModel> GetRemainingOfSuit(List<Suit> suits)
    {
        List<RuntimeCardModel> remaining = new();
        foreach (RuntimeCardModel card in Deck.RemainingItems)
        {
            if (suits.Contains(card.Suit))
            {
                remaining.Add(card);
            }
        }
        return remaining;
    }
}
