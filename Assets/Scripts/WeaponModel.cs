using System;
using System.Collections.Generic;
using Project.Decks;

public class WeaponModel : CardModel
{
    public WeaponModel(Suit suit, int value) : base(suit, value)
    {
    }

    public WeaponModel(Suit suit, int value, List<CardBuff> buffs) : base(suit, value, buffs)
    {
    }

    public List<CardModel> SlainCards { get; private set; } = new();

    // Strength is what cards the weapon can fight
    public int Strength
    {
        get
        {
            if (SlainCards.Count == 0)
            {
                return 99;
            }

            return SlainCards[^1].Power;
        }
    }

    // public WeaponModel(CardModel card)
    // {
    //     BasePower = card.BasePower;
    // }

    public override void Update()
    {
        base.Update();
        foreach(CardModel card in SlainCards)
        {
            card.Update();
        }
    }

    public string GetWeaponInfoString()
    {
        return $"Pow: {Power} Str: {Strength} Slain: {SlainCards.Count}";
    }

    public bool CanSlayMonster(int monsterStrength) => monsterStrength < Strength;

    public void AddMonsterToSlain(CardModel card)
    {
        SlainCards.Add(card);
        OnUpdate?.Invoke();
    }
}