using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Decks
{
    // TODO: Turn suit into a map that connects a suit with a card type?
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

    public enum CardType
    {
        MONSTER,
        WEAPON,
        POTION,
        DOOR,
        TREASURE
    }

    public abstract class RuntimeCardModel : IDeckStorable, IBuffRegisterable
    {
        public Suit Suit { get; private set; }
        public CardType CardType { get; private set; }

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

        public BuffManager BuffManager { get; private set; }
        public List<int> ValueModifiers = new();

        private readonly Guid uuid;

        public RuntimeCardModel(Suit suit, CardType cardType, int value)
        {
            Suit = suit;
            CardType = cardType;
            BaseValue = value;
            BuffManager = new BuffManager(this);

            uuid = Guid.NewGuid();
        }

        public RuntimeCardModel(Suit suit, CardType cardType, int value, List<Buff> buffs) : this(suit, cardType, value)
        {
            foreach(Buff buff in buffs)
            {
                BuffManager.AddNewBuff(buff);
            }
        }

        public virtual void Update()
        {
            BuffManager.Update();
            OnUpdate?.Invoke();
        }

        public override string ToString()
        {
            string output = Value.ToString();
            if (Value != BaseValue)
            {
                output += $"({BaseValue})";
            }
            output += $" of {Suit} ({CardType})";
            return output;
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
                return;
            }
        }

        public void HandleDeath()
        {
            BuffManager.TriggerEffect(BuffTrigger.OnSelfDie);
            BuffManager.CleanupRemoveOnDeathBuffs();
        }

        public void HandleOnDraw()
        {

            BuffManager.TriggerEffect(BuffTrigger.OnDraw);
        }

        public void HandleOnOtherDie()
        {
            BuffManager.TriggerEffect(BuffTrigger.OnOtherDie);
        }

        public List<Buff> GetBuffs() => BuffManager.GetBuffs();
        public Buff AddNewBuff(Buff buff) => BuffManager.AddNewBuff(buff);
        public void RemoveBuff(Buff buff) => BuffManager.RemoveBuff(buff);
        public void RemoveBuff(BuffID buffID) => BuffManager.RemoveBuff(buffID);
        public bool HasBuff(Buff buff) => BuffManager.HasBuff(buff);
        public bool HasBuff(BuffID buffID) => BuffManager.HasBuff(buffID);
    }
}