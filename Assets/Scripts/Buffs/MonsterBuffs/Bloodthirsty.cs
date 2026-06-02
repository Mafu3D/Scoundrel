using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Bloodthirsty", menuName="Abilities/Bloodthirsty")]
public class Bloodthirsty : Buff
{
    public override void OnOtherDie(MonsterCardModel other)
    {
        Owner.RegisterValueModifier(1);
    }
}

