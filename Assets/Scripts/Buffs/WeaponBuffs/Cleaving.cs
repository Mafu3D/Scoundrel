using System.Collections.Generic;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Cleaving", menuName="Buffs/Weapons/Cleaving")]
public class Cleaving : Buff
{
    public override void OnPlayerAttackPostDamage(AttackReport attackReport)
    {
        List<RuntimeCardModel> neighbors = gameManager.CurrentRoom.GetNeighbors(attackReport.Target, new() {Suit.CLUBS, Suit.SPADES});
        foreach (RuntimeCardModel neighbor in neighbors)
        {
            if (neighbor != null && neighbor is MonsterCardModel)
            {
                neighbor.RegisterValueModifier(-1);
            }
        }
    }
}
