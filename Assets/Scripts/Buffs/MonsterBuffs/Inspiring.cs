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
        Buff buffToApply = GetRegisteredChildBuffByName("Inspired");
        Debug.Log(Owner);
        List<RuntimeCardModel> neighbors = gameManager.DungeonController.CurrentRoom.GetActiveNeighbors(Owner, new List<CardType>() { CardType.MONSTER });
        foreach (RuntimeCardModel neighbor in neighbors)
        {
            Buff buff = AddBuff(neighbor, buffToApply);
            neighborsBuffMap.Add(new(neighbor, buff.ID));
        }
    }
}

