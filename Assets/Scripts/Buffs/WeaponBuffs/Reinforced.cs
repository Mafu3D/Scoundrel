using System.Collections.Generic;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Reinforced", menuName="Buffs/Weapons/Reinforced")]
public class Reinforced : Buff
{
    private bool hasSlainFirstMonster = false;
    public override void OnWeaponAttackPostDamage(MonsterCardModel target)
    {
        if (!hasSlainFirstMonster)
        {
            (Owner as WeaponCardModel).RemoveMonsterFromSlain(target);
            hasSlainFirstMonster = true;
        }
    }
}
