using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Bloodthirsty", menuName="Buffs/Monster/Bloodthirsty")]
public class Bloodthirsty : Buff
{
    [SerializeField] int amount = 1;
    public override void OnOtherDie(MonsterCardModel other)
    {
        Owner.RegisterValueModifier(amount);
    }
}
