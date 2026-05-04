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

        public Action OnUpdate;

        public BuffManager Buffs => buffs;
        private BuffManager buffs;
        public List<int> ValueModifiers = new();

        private Guid uuid;

        public CardModel(Suit suit, int value) {
            this.Suit = suit;
            this.BaseValue = value;
            this.buffs = new BuffManager(this);

            uuid = Guid.NewGuid();
        }

        public CardModel(Suit suit, int value, List<Buff> buffs) {
            this.Suit = suit;
            this.BaseValue = value;
            this.buffs = new BuffManager(this);
            foreach(Buff buff in buffs)
            {
                this.buffs.RegisterBuff(buff);
            }

            uuid = Guid.NewGuid();
        }

        public void Update()
        {
            buffs.Update();
            OnUpdate?.Invoke();
        }

        public override string ToString()
        {
            string output = Value.ToString();
            if (Value != BaseValue)
            {
                output += $"({BaseValue})";
            }
            output += $" of {Suit}";
            return output;
        }

        public void RegisterValueModifier(int value)
        {
            ValueModifiers.Add(value);
        }

        public void DeregisterValueModifier(int value)
        {
            Debug.Log("Deregistering: " + value + $" from {this.ToString()}");
            string before = "Before: ";
            for (int i = 0; i < ValueModifiers.Count; i++)
            {
                before += "\n" + ValueModifiers[i].ToString();
            }
            Debug.Log(before);
            if (ValueModifiers.Contains(value))
            {
                ValueModifiers.Remove(value);
                return;
            }
            string after = "After: ";
            for (int i = 0; i < ValueModifiers.Count; i++)
            {
                after += "\n" + ValueModifiers[i].ToString();
            }
            Debug.Log(after);
            Debug.LogWarning($"Tried to deregister modifier value of {value} from {this.ToString()} but failed!");

        }

        public void HandleDeath()
        {
            buffs.OnDeath();
        }

        public void HandleOnDraw()
        {
            buffs.ActivateBuffTrigger(BuffTrigger.OnDraw);
        }

        public BuffManager GetBuffs() => Buffs;

        public void RegisterBuff(Buff buff) => Buffs.RegisterBuff(buff);

        public void DeregisterBuff(Buff buff) => Buffs.DeregisterBuff();
    }
}