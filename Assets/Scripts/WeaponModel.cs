using System;
using System.Collections.Generic;
using Project.Decks;

public class WeaponCardModel : RuntimeCardModel
{
    public int StartingPower { get; private set; }
    public int Power { get; private set; }
    public List<RuntimeCardModel> SlainCards { get; private set; } = new();
    public Action OnWeaponUpdate;

    public WeaponCardModel(Suit suit, int value) : base(suit, CardType.WEAPON, value) { }

    public override void Update()
    {
        base.Update();
        foreach(RuntimeCardModel card in SlainCards)
        {
            card.Update();
        }
    }

    public string GetWeaponInfoString()
    {
        return $"Pow: {Power} Str: {GetCurrentStrength()} Slain: {SlainCards.Count}";
    }

    public int GetCurrentStrength()
    {
        if (SlainCards.Count == 0)
        {
            return 99;
        }

        return SlainCards[^1].Value;
    }

    public bool CanSlayMonster(int monsterStrength) => monsterStrength < GetCurrentStrength();

    public void AddMonsterToSlain(RuntimeCardModel card)
    {
        SlainCards.Add(card);
        OnWeaponUpdate?.Invoke();
    }
}

public class MonsterCardModel : RuntimeCardModel
{
    public MonsterCardModel(Suit suit, int value) : base(suit,  CardType.MONSTER, value)
    {
    }
}

public class PotionCardModel : RuntimeCardModel
{
    public PotionCardModel(Suit suit, int value) : base(suit,  CardType.POTION, value)
    {
    }
}

public class DoorCardModel : RuntimeCardModel
{
    public DoorCardModel(Suit suit, int value) : base(suit,  CardType.DOOR, value)
    {
    }
}

public class TreasureCardModel : RuntimeCardModel
{
    public TreasureCardModel(Suit suit, int value) : base(suit,  CardType.TREASURE, value)
    {
    }
}