using System;
using System.Collections;
using System.Collections.Generic;
using Project.Decks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CardObject : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> valueTMPTexts = new();
    [SerializeField] private TMP_Text suitTMPText;
    [SerializeField] private Color red;
    [SerializeField] private Color black;

    public UnityEvent<Card> OnCardClicked;

    public Card Card {get; private set; }

    public void AssignCard(Card card)
    {
        Card = card;
        if (card == null) return;
        Color color = black;
        if (card.Suit == Suit.HEARTS || card.Suit == Suit.DIAMONDS)
        {
            color = red;
        }
        foreach (var TMPText in valueTMPTexts)
        {
            string valueString = card.Value.ToString();
            if (card.Value > 10)
            {
                if (card.Value == 11)
                {
                    valueString = "J";
                }
                else if (card.Value == 12)
                {
                    valueString = "Q";
                }
                else if (card.Value == 13)
                {
                    valueString = "K";
                }
                else if (card.Value == 14)
                {
                    valueString = "A";
                }
            }
            TMPText.text = valueString;
            TMPText.color = color;
        }

        suitTMPText.text = card.Suit.ToString();
        suitTMPText.color = color;
    }

    void OnMouseDown()
    {
        Debug.Log("clicky");
        OnCardClicked?.Invoke(Card);
    }
}
