using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Bomb", menuName="Buffs/Weapons/Bomb")]
public class Bomb : Buff
{
    public override void OnWeaponAttackPostDamage(MonsterCardModel target)
    {
        gameManager.Player.UnequipWeapon();
    }

    public override void OnBuffApplied()
    {
        Owner.RegisterValueModifier(5);
    }
}
