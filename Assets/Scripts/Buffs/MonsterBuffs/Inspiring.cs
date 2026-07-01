using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Inspiring", menuName="Buffs/Monster/Inspiring")]
public class Inspiring : Buff
{
    List<KeyValuePair<RuntimeCardModel, BuffID>> neighborsBuffMap = new();

    public override void OnDraw()
    {
        DoBuffEffect(); // Replace with apply when drawn parameter
    }

    private void DoBuffEffect()
    {
        Buff buffToApply = GetRegisteredChildBuffByName("Inspired");
        List<RuntimeCardModel> neighbors = gameManager.DungeonController.CurrentRoom.GetNeighbors(Owner, new() {Suit.CLUBS, Suit.SPADES});
        foreach (RuntimeCardModel neighbor in neighbors)
        {
            Buff buff = AddBuff(neighbor, buffToApply);
            neighborsBuffMap.Add(new(neighbor, buff.ID));
        }
    }
}

