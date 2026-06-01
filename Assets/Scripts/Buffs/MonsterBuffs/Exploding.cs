using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Exploding", menuName="Abilities/Exploding")]
public class Exploding : Buff
{
    protected override void OnBuffInitialized() {
    }

    protected override void OnWeaponAttackPreDamage(MonsterCardModel target) { }

    protected override void OnWeaponAttackPostDamage(MonsterCardModel target) { }

    protected override void OnAttackedPreDamage(WeaponCardModel weapon) { }

    protected override void OnAttackedPostDamage(WeaponCardModel weapon) { }

    protected override void OnDiscardPotion() { }

    protected override void OnDraw()
    {
    }

    protected override void OnDrinkPotion() { }

    protected override void OnEnterRoom() { }

    protected override void OnEquipWeapon() { }

    protected override void OnOtherDie() { }

    protected override void OnRun() { }

    protected override void OnUpdate() { }

    protected override void OnBuffApplied()
    {
    }

    protected override void OnCleanup()
    {
    }

    protected override void OnLeave()
    {
    }

    protected override void OnSelfDie()
    {
        List<RuntimeCardModel> neighbors = gameManager.CurrentRoom.GetNeighbors(Owner);
        foreach (RuntimeCardModel neighbor in neighbors)
        {
            neighbor.RegisterValueModifier(-Owner.Value);
            if (neighbor.Value <= 0)
            {
                gameManager.CurrentRoom.TryRemoveCard(neighbor);
            }
        }
    }
}

