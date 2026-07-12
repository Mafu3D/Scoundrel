using System;
using System.Collections.Generic;
using UnityEngine;

using Mafu.Extensions;

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

    public enum CardType
    {
        MONSTER,
        WEAPON,
        POTION,
        DOOR,
        TREASURE
    }

    public abstract class RuntimeCardModel : IDeckStorable, IDisposable
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
                foreach (int val in TemporaryValueModifiers)
                {
                    value += val;
                }
                return value;
            }
        }
        public int BaseValue { get; private set; }

        public string ID => uuid.ToString();

        public Action OnUpdate;
        public Action OnCardActivated;

        public bool PersistsThroughRun = false;

        public BuffManager BuffManager { get; private set; }

        public bool IsTemporary => isTemporary;

        private bool isTemporary;

        public List<int> ValueModifiers = new();
        public List<int> TemporaryValueModifiers = new();

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

        public abstract bool TryUse(Player player, GameManager gameManager);

        public abstract bool TryUseTop(Player player, GameManager gameManager);

        public abstract bool TryUseBot(Player player, GameManager gameManager);

        public void SetIsTemporary(bool value)
        {
            isTemporary = value;
        }

        public virtual void Update()
        {
            BuffManager.Update();
            OnUpdate?.Invoke();
        }

        public override string ToString()
        {
            return $"{GetCardInfoString()} ({CardType.ToString().ToFirstUppercase()})";
        }

        public string GetCardInfoString()
        {
            string output = Value.ToString();
            if (Value != BaseValue)
            {
                output += $" ({BaseValue})";
            }
            output += $" of {Suit.ToString().ToFirstUppercase()}";
            return output;
        }

        public string GetName()
        {
            string output = Value.ToString();
            if (Value != BaseValue)
            {
                output += $"({BaseValue})";
            }
            output += $" of {Suit}";
            return output;
        }

        public string GetCardValueStringSymbol()
        {
            return CardType switch
            {
                CardType.DOOR => "D",
                CardType.TREASURE => "T",
                _ => Value switch
                {
                    11 => "J",
                    12 => "Q",
                    13 => "K",
                    14 => "A",
                    _ => Value.ToString(),
                }
            };
        }

        public void OnShuffleBackIn()
        {
            BuffManager.CleanupTemporaryBuffs();
            ClearTemporaryValueModifiers();
        }

        public void RegisterPermanentValueModifier(int value)
        {
            ValueModifiers.Add(value);
        }

        public void DeregisterPermanentValueModifier(int value)
        {
            if (ValueModifiers.Contains(value))
            {
                ValueModifiers.Remove(value);
            }
        }

        public void RegisterTemporaryValueModifier(int value)
        {
            TemporaryValueModifiers.Add(value);
        }

        public void DeregisterTemporaryValueModifier(int value)
        {
            if (TemporaryValueModifiers.Contains(value))
            {
                TemporaryValueModifiers.Remove(value);
            }
        }

        public void ClearTemporaryValueModifiers()
        {
            TemporaryValueModifiers = new();
        }

        public void Kill()
        {
            BuffManager.CleanupRemoveOnDeathBuffs();
            BuffManager.Dispose();
            this.Dispose();
        }

        public void HandleOnDraw()
        {
            BuffManager.HandleOnDraw();
        }

        public List<Buff> GetBuffs() => BuffManager.GetBuffs();
        public Buff AddNewBuff(Buff buff) => BuffManager.AddNewBuff(buff);
        public void RemoveBuff(Buff buff) => BuffManager.RemoveBuff(buff);
        public void RemoveBuff(BuffID buffID) => BuffManager.RemoveBuff(buffID);
        public bool HasBuff(Buff buff) => BuffManager.HasBuff(buff);
        public bool HasBuff(BuffID buffID) => BuffManager.HasBuff(buffID);

        public void Dispose()
        {
            BuffManager.Dispose();
        }

        public void OnDrawnFromDeck()
        {
            // This is called any time the card is drawn, even if its put back
        }

        public void OnReturnToDeck()
        {
            BuffManager.CleanupTemporaryBuffs();
            ClearTemporaryValueModifiers();
        }
    }
}