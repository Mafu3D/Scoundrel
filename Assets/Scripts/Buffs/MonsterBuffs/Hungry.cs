using System.Collections.Generic;
using System.Linq;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Hungry", menuName="Buffs/Monster/Hungry")]
public class Hungry : Buff
{
    private bool hasEatenThisFloor = false;
    // public override void OnBuffApplied()
    // {
    //     if (gameManager.CurrentRoom.Cards.Contains(Owner))
    //     {
    //         DoBuffEffect();
    //     }
    // }

    public override void OnDraw()
    {
        DoBuffEffect(); // Replace with apply when drawn parameter
    }

    public override void OnExitFloor()
    {
        hasEatenThisFloor = false;
        Debug.Log("resetting has eaten");
    }

    private void DoBuffEffect()
    {
        List<RuntimeCardModel> neighbors = gameManager.DungeonController.CurrentRoom.GetActiveNeighbors(Owner, new List<CardType>() { CardType.MONSTER });
        if (neighbors.Count == 0)
        {
            return;
        }
        RuntimeCardModel lowestNeighbor = neighbors[0];
        foreach (RuntimeCardModel neighbor in neighbors)
        {
            if (neighbor.Value < lowestNeighbor.Value)
            {
                lowestNeighbor = neighbor;
            }
        }
        gameManager.DungeonController.CurrentRoom.TryRemoveCard(lowestNeighbor);
        Owner.RegisterPermanentValueModifier(lowestNeighbor.Value);

        hasEatenThisFloor = true;
    }
}