using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Decks
{
    public enum Suit {
        SPADES,
        HEARTS,
        DIAMONDS,
        CLUBS
    }

    public class CardModel : IDeckStorable, IAbilityRegisterable
    {
        public Suit Suit { get; private set; }

        public int Value { get; private set; }

        public string ID => uuid.ToString();

        public List<Ability> Abilities = new();

        private Guid uuid;

        public CardModel(Suit suit, int value) {
            this.Suit = suit;
            this.Value = value;

            uuid = Guid.NewGuid();
        }

        public CardModel(Suit suit, int value, List<Ability> abilities) {
            this.Suit = suit;
            this.Value = value;
            this.Abilities = abilities;

            uuid = Guid.NewGuid();
        }

        public override string ToString()
        {
            return $"{Value} of {Suit}";
        }

        public List<Ability> GetAbilities()
        {
            return Abilities;
        }

        public void RegisterAbility(Ability ability)
        {
            Abilities.Add(ability);
            Debug.Log($"{ability.Name} registered!");
        }

        public void DeregisterAbility(Ability ability)
        {
            if (Abilities.Contains(ability))
            {
                Abilities.Remove(ability);
            }
        }
    }
}