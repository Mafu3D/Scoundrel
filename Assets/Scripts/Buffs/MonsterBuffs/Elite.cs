using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Elite", menuName="Abilities/Elite")]
public class Elite : Buff
{
    public override void OnBuffApplied()
    {
        DoBuffEffect();
    }

    public override void OnCleanup()
    {
        Owner.DeregisterValueModifier(1);
    }

    private void DoBuffEffect()
    {
        // ServiceLocator.Global.Get(out gameManager);
        Owner.RegisterValueModifier(1);
    }

    public override void OnSelfDie()
    {
        gameManager.Player.AddGold(1);
    }
}

