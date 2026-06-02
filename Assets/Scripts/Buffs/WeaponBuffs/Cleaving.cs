using System.Collections.Generic;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Cleaving", menuName="Buffs/Weapons/Cleaving")]
public class Cleaving : Buff
{
    public override void OnWeaponAttackPostDamage(MonsterCardModel target)
    {
        List<RuntimeCardModel> neighbors = gameManager.CurrentRoom.GetNeighbors(target);
        foreach (RuntimeCardModel neighbor in neighbors)
        {
            if (neighbor != null && neighbor is MonsterCardModel)
            {
                neighbor.RegisterValueModifier(-1);
            }
        }
    }
}