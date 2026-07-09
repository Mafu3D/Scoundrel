using System.Collections.Generic;
using System.Linq;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Hungry", menuName="Buffs/Monster/Hungry")]
public class Hungry : Buff
{
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

    private void DoBuffEffect()
    {
        List<RuntimeCardModel> neighbors = gameManager.DungeonController.CurrentRoom.GetActiveNeighbors(Owner, new List<CardType>() { CardType.MONSTER });
        int totalPower = 0;
        foreach (RuntimeCardModel neighbor in neighbors)
        {
            totalPower += neighbor.Value;
            gameManager.DungeonController.CurrentRoom.TryRemoveCard(neighbor);
        }
        Owner.RegisterValueModifier(totalPower);
    }
}