using System;
using System.Collections.Generic;
using Project.Decks;
using UnityEngine;

public class WeaponCardModel : RuntimeCardModel
{
    public List<RuntimeCardModel> SlainCards { get; private set; } = new();
    public Action OnWeaponUpdate;

    public WeaponCardModel(Suit suit, int value) : base(suit, CardType.WEAPON, value) { }

    public WeaponCardModel(RuntimeCardModel baseCard) : base(baseCard.Suit, CardType.WEAPON, baseCard.BaseValue, baseCard.BuffManager.GetBuffs())
    {

    }

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
        return $"Pow: {Value} Str: {GetCurrentStrength()} Slain: {SlainCards.Count}";
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

    public void RemoveMonsterFromSlain(int index)
    {
        if (index >+ SlainCards.Count)
        {
            Debug.LogWarning($"Tried to remove a slain card from {this} at index {index}. Index out of range!");
            return;
        }
        RemoveMonsterFromSlain(SlainCards[index]);
    }

    public void RemoveMonsterFromSlain(RuntimeCardModel card)
    {
        if (SlainCards.Contains(card))
        {
            SlainCards.Remove(card);
        }
        OnWeaponUpdate?.Invoke();
    }

    public override bool TryUse(Player player, GameManager gameManager)
    {
        return player.TryEquipWeapon(this);
    }

    public override bool TryUseBot(Player player, GameManager gameManager) => TryUse(player, gameManager);
    public override bool TryUseTop(Player player, GameManager gameManager) => TryUse(player, gameManager);
}
