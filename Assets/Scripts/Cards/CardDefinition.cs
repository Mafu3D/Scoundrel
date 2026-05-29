using System.Collections.Generic;
using UnityEngine;

namespace Project.Decks
{
    [CreateAssetMenu(fileName="NewCard", menuName="New Card")]
    public class CardDefinition : ScriptableObject
    {
        public Suit Suit;
        public int Value;
        public List<CardBuff> Buffs;
    }
}