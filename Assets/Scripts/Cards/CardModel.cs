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

    public class CardModel : IDeckStorable, IBuffRegisterable
    {
        public Suit Suit { get; private set; }

        public int Value
        {
            get
            {
                int value = BaseValue;
                foreach (int val in ValueModifiers)
                {
                    value += val;
                }
                return value;
            }
        }
        public int BaseValue { get; private set; }

        public string ID => uuid.ToString();
        private List<Buff> buffs = new();
        public List<int> ValueModifiers = new();

        private Guid uuid;

        public CardModel(Suit suit, int value) {
            this.Suit = suit;
            this.BaseValue = value;

            uuid = Guid.NewGuid();
        }

        public CardModel(Suit suit, int value, List<Buff> buffs) {
            this.Suit = suit;
            this.BaseValue = value;
            this.buffs = buffs;

            uuid = Guid.NewGuid();
        }

        public override string ToString()
        {
            return $"{Value} of {Suit}";
        }

        public void RegisterBuff(Buff buff)
        {
            buffs.Add(buff);
            buff.Initialize(this);
            buff.OnBuffApplied();
        }

        public void DeregisterBuff(Buff buff)
        {
            if (buffs.Contains(buff))
            {
                buff.OnBuffRemoved();
                buffs.Remove(buff);
            }
        }

        public void RegisterValueModifier(int value)
        {
            ValueModifiers.Add(value);
        }

        public void DeregisterValueModifier(int value)
        {
            if (ValueModifiers.Contains(value))
            {
                ValueModifiers.Remove(value);
            }
        }

        public List<Buff> GetAbilities()
        {
            return buffs;
        }

        public void HandleRemoval()
        {
            List<Buff> buffsToBeRemoved = new();
            foreach (Buff buff in buffs)
            {
                if (buff.RemoveOnDeath)
                {
                    buffsToBeRemoved.Add(buff);
                }
                buff.OnCardRemoval();
            }
            foreach(Buff buff in buffsToBeRemoved)
            {
                DeregisterBuff(buff);
            }
        }
    }
}