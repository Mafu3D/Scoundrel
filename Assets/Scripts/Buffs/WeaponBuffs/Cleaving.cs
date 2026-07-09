using System.Collections.Generic;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Cleaving", menuName="Buffs/Weapons/Cleaving")]
public class Cleaving : Buff
{
    public override void OnPlayerAttackPostDamage(CombatReport attackReport)
    {
        List<RuntimeCardModel> neighbors = gameManager.DungeonController.CurrentRoom.GetOtherActiveCards(attackReport.Defender, new List<CardType>() { CardType.MONSTER });
        foreach (RuntimeCardModel neighbor in neighbors)
        {
            if (neighbor != null && neighbor is MonsterCardModel)
            {
                neighbor.RegisterValueModifier(-1);
            }
        }
    }
}
