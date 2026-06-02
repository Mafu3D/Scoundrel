using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Exploding", menuName="Abilities/Exploding")]
public class Exploding : Buff
{
    public override void OnSelfDie()
    {
        List<RuntimeCardModel> neighbors = gameManager.CurrentRoom.GetNeighbors(Owner);
        foreach (RuntimeCardModel neighbor in neighbors)
        {
            neighbor.RegisterValueModifier(-Owner.Value);
            if (neighbor.Value <= 0)
            {
                gameManager.CurrentRoom.TryRemoveCard(neighbor);
            }
        }
    }
}

