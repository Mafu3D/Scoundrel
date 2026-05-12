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

        public BuffManager BuffManager => buffManager;
        private BuffManager buffManager;
        public List<int> ValueModifiers = new();

        private Guid uuid;

        public CardModel(Suit suit, int value) {
            this.Suit = suit;
            this.BaseValue = value;
            this.buffManager = new BuffManager(this);

            uuid = Guid.NewGuid();
        }

        public CardModel(Suit suit, int value, List<Buff> buffs) {
            this.Suit = suit;
            this.BaseValue = value;
            this.buffManager = new BuffManager(this);
            foreach(Buff buff in buffs)
            {
                this.buffManager.AddNewBuff(buff);
            }

            uuid = Guid.NewGuid();
        }

        public void Update()
        {
            buffManager.Update();
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
            buffManager.TriggerEffect(BuffTrigger.OnSelfDie);
            buffManager.CleanupRemoveOnDeathBuffs();
        }

        public void HandleOnDraw()
        {
            buffManager.TriggerEffect(BuffTrigger.OnDraw);
        }

        public void HandleOnOtherDie()
        {
            buffManager.TriggerEffect(BuffTrigger.OnOtherDie);
        }

        public List<Buff> GetBuffs() => BuffManager.GetBuffs();
        public Buff AddNewBuff(Buff buff) => BuffManager.AddNewBuff(buff);
        public void RemoveBuff(Buff buff) => BuffManager.RemoveBuff(buff);
        public void RemoveBuff(BuffID buffID) => BuffManager.RemoveBuff(buffID);
        public bool HasBuff(Buff buff) => BuffManager.HasBuff(buff);
        public bool HasBuff(BuffID buffID) => BuffManager.HasBuff(buffID);
    }
}