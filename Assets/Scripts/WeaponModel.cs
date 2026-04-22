using System;
using System.Collections.Generic;
using Project.Decks;

public class WeaponModel
{
    public int StartingPower { get; private set; }
    public int Power { get; private set; }
    public CardModel Card;
    public List<CardModel> SlainCards { get; private set; } = new();
    public Action OnWeaponUpdate;

    public WeaponModel(CardModel card)
    {
        Card = card;
        StartingPower = card.Value;
        Power = card.Value;
    }

    public string GetWeaponInfoString()
    {
        return $"Pow: {Power} Str: {GetCurrentStrength()} Slain: {SlainCards.Count}";
    }

    public int GetCurrentStrength()
    {
        if (SlainCards.Count == 0)
        {
            return 15;
        }

        return SlainCards[^1].Value;
    }

    public bool CanSlayMonster(int monsterStrength) => monsterStrength < GetCurrentStrength();

    public void AddMonsterToSlain(CardModel card)
    {
        SlainCards.Add(card);
        OnWeaponUpdate?.Invoke();
    }
}