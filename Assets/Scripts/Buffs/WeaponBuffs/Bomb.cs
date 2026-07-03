using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Bomb", menuName="Buffs/Weapons/Bomb")]
public class Bomb : Buff
{
    public override void OnPlayerAttackPostDamage(CombatReport attackReport)
    {
        gameManager.Player.UnequipWeapon();
        Debug.Log("Bomb detonated, unequipping weapon");
    }

    public override void OnBuffApplied()
    {
        Owner.RegisterValueModifier(5);
    }
}
