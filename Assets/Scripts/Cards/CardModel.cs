using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Decks
{
    public enum Suit {
        SPADES,
        HEARTS,
        DIAMONDS,
        CLUBS,
        SKULLS,
        GOBLINS,
        POTIONS,
        WEAPONS,
        DOORS,
        TREASURES,
        QUESTS
    }

    public class CardModel : IDeckStorable, IBuffRegisterable<CardModel>
    {
        public Suit Suit { get; private set; }

        public int Power
        {
            get
            {
                int value = BasePower;
                foreach (int val in PowerModifiers)
                {
                    value += val;
                }
                return value;
            }
        }
        public int BasePower { get; protected set; }

        public string ID => uuid.ToString();

        public Action OnUpdate;

        public BuffManager BuffManager => buffManager;
        private BuffManager buffManager;
        public List<int> PowerModifiers = new();

        private Guid uuid;

        public CardModel(Suit suit, int value) {
            this.Suit = suit;
            this.BasePower = value;
            this.buffManager = new BuffManager(this);

            uuid = Guid.NewGuid();
        }

        public CardModel(Suit suit, int value, List<CardBuff> buffs) {
            this.Suit = suit;
            this.BasePower = value;
            this.buffManager = new BuffManager(this);
            foreach(CardBuff buff in buffs)
            {
                this.buffManager.AddNewBuff(buff);
            }

            uuid = Guid.NewGuid();
        }

        public virtual void Update()
        {
            buffManager.Update();
            OnUpdate?.Invoke();
        }

        public override string ToString()
        {
            string output = Power.ToString();
            if (Power != BasePower)
            {
                output += $"({BasePower})";
            }
            output += $" of {Suit}";
            return output;
        }

        public void RegisterPowerModifier(int value)
        {
            PowerModifiers.Add(value);
        }

        public void DeregisterPowerModifier(int value)
        {
            Debug.Log("Deregistering: " + value + $" from {this.ToString()}");
            string before = "Before: ";
            for (int i = 0; i < PowerModifiers.Count; i++)
            {
                before += "\n" + PowerModifiers[i].ToString();
            }
            if (PowerModifiers.Contains(value))
            {
                PowerModifiers.Remove(value);
                return;
            }
            string after = "After: ";
            for (int i = 0; i < PowerModifiers.Count; i++)
            {
                after += "\n" + PowerModifiers[i].ToString();
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

        // public List<CardBuff> GetBuffs() => BuffManager.GetBuffs();
        // public CardBuff AddNewBuff(CardBuff buff) => BuffManager.AddNewBuff(buff);
        // public void RemoveBuff(CardBuff buff) => BuffManager.RemoveBuff(buff);
        // public void RemoveBuff(BuffID buffID) => BuffManager.RemoveBuff(buffID);
        // public bool HasBuff(CardBuff buff) => BuffManager.HasBuff(buff);
        // public bool HasBuff(BuffID buffID) => BuffManager.HasBuff(buffID);

        List<Buff<CardModel>> IBuffRegisterable<CardModel>.GetBuffs()
        {
            // return BuffManager.GetBuffs();
            throw new NotImplementedException();
        }

        public Buff<CardModel> AddNewBuff(Buff<CardModel> buff)
        {
            throw new NotImplementedException();
        }

        public void RemoveBuff(Buff<CardModel> buff)
        {
            throw new NotImplementedException();
        }

        public bool HasBuff(Buff<CardModel> buff)
        {
            throw new NotImplementedException();
        }
    }
}