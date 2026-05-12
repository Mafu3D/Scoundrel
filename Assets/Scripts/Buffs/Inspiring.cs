using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Inspiring", menuName="Abilities/Inspiring")]
public class Inspiring : Buff
{
    List<KeyValuePair<CardModel, BuffID>> neighborsBuffMap = new();

    protected override void OnBuffInitialized() {
    }

    protected override void OnAttack() { }

    protected override void OnDiscardPotion() { }

    protected override void OnDraw()
    {
        DoBuffEffect(); // Replace with apply when drawn parameter
    }

    protected override void OnDrinkPotion() { }

    protected override void OnEnterRoom() { }

    protected override void OnEquipWeapon() { }

    protected override void OnOtherDie() { }

    protected override void OnRun() { }

    protected override void OnUpdate() { }

    protected override void OnBuffApplied()
    {
    }

    protected override void OnCleanup()
    {
    }

    private void DoBuffEffect()
    {
        Buff buffToApply = GetRegisteredChildBuffByName("Inspired");
        List<CardModel> neighbors = gameManager.CurrentRoom.GetNeighbors(Owner);
        foreach (CardModel neighbor in neighbors)
        {
            Buff buff = AddBuff(neighbor, buffToApply);
            neighborsBuffMap.Add(new(neighbor, buff.ID));
        }
    }

    protected override void OnLeave()
    {
    }

    protected override void OnSelfDie()
    {
    }
}

