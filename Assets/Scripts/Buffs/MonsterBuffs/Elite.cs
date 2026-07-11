using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Elite", menuName="Buffs/Monster/Elite")]
public class Elite : Buff
{
    public override void OnBuffApplied()
    {
        DoBuffEffect();
    }

    public override void OnCleanup()
    {
        Owner.DeregisterPermanentValueModifier(1);
    }

    private void DoBuffEffect()
    {
        // ServiceLocator.Global.Get(out gameManager);
        Owner.RegisterPermanentValueModifier(2);
    }

    public override void OnSelfDiePreRemoval()
    {
        gameManager.Player.AddGold(1);
    }
}

