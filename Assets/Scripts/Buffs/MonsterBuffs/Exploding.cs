using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Exploding", menuName="Buffs/Monster/Exploding")]
public class Exploding : Buff
{
    [SerializeField] int amount = 3;

    public override void OnSelfDiePreRemoval()
    {
        List<RuntimeCardModel> neighbors = gameManager.DungeonController.CurrentRoom.GetActiveNeighbors(Owner, new List<CardType>() { CardType.MONSTER, CardType.WEAPON, CardType.POTION });
        foreach (RuntimeCardModel neighbor in neighbors)
        {
            int newValue = neighbor.Value - Owner.Value;
            if (newValue <= 0)
            {
                gameManager.DungeonController.CurrentRoom.TryRemoveCard(neighbor);
            }
            else
            {
                // neighbor.RegisterTemporaryValueModifier(Owner.Value);
                neighbor.RegisterTemporaryValueModifier(amount);
            }
        }
    }
}

