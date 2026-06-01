using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Bomb", menuName="Abilities/Weapons/Bomb")]
public class Bomb : Buff
{
    protected override void OnBuffInitialized() {
    }

    protected override void OnAttack()
    {

    }

    protected override void OnDiscardPotion() { }

    protected override void OnDraw()
    {
    }

    protected override void OnDrinkPotion() { }

    protected override void OnEnterRoom() { }

    protected override void OnEquipWeapon() { }

    protected override void OnOtherDie()
    {
    }

    protected override void OnRun() { }

    protected override void OnUpdate() { }

    protected override void OnBuffApplied()
    {
        Owner.RegisterValueModifier(5);
    }

    protected override void OnCleanup()
    {
    }

    protected override void OnLeave()
    {
    }

    protected override void OnSelfDie()
    {
    }
}

