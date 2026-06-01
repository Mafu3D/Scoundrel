using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Elite", menuName="Abilities/Elite")]
public class Elite : Buff
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
        DoBuffEffect();
    }

    protected override void OnCleanup()
    {
        Owner.DeregisterValueModifier(1);
    }

    private void DoBuffEffect()
    {
        // ServiceLocator.Global.Get(out gameManager);
        Owner.RegisterValueModifier(1);
    }

    protected override void OnLeave()
    {
    }

    protected override void OnSelfDie()
    {
        gameManager.Player.AddGold(1);
    }
}

