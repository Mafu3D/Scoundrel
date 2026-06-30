using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Exploding", menuName="Buffs/Monster/Exploding")]
public class Exploding : Buff
{
    public override void OnSelfDiePreRemoval()
    {
        List<RuntimeCardModel> neighbors = gameManager.CurrentRoom.GetNeighbors(Owner, new() {Suit.CLUBS, Suit.SPADES, Suit.HEARTS, Suit.DIAMONDS});
        foreach (RuntimeCardModel neighbor in neighbors)
        {
            int newValue = neighbor.Value - Owner.Value;
            if (newValue <= 0)
            {
                gameManager.CurrentRoom.TryRemoveCard(neighbor);
            }
            else
            {
                Buff tempDamageBuff = GetRegisteredChildBuffByName("TempDamage");
                (tempDamageBuff as TempDamage).Amount = Owner.Value;
                Buff buff = AddBuff(neighbor, tempDamageBuff);
            }
        }
    }
}

