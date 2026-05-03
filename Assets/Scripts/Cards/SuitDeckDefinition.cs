using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Decks
{
    [CreateAssetMenu(fileName="NewSuitDeck", menuName="New Suit Deck")]
    public class SuitDeckDefinition : ScriptableObject, IDeckDefinition<CardModel>
    {
        public Suit Suit;
        public List<CardDefinition> Cards;

        public List<CardModel> UnpackContents()
        {
            List<CardModel> contents = new();
            foreach (CardDefinition card in Cards)
            {
                contents.Add(new CardModel(card.Suit, card.Value, card.Buffs));
            }
            return contents;
        }
    }
}