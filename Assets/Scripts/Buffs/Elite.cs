using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Elite", menuName="Abilities/Elite")]
public class Elite : CardBuff
{
    protected override void OnBuffInitialized() {
    }

    protected override void OnAttack() { }

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
        Owner.DeregisterPowerModifier(1);
    }

    private void DoBuffEffect()
    {
        // ServiceLocator.Global.Get(out gameManager);
        Owner.RegisterPowerModifier(1);
    }

    protected override void OnLeave()
    {
    }

    protected override void OnSelfDie()
    {
        gameManager.Player.AddGold(1);
    }
}

