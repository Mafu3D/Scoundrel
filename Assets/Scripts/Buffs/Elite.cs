using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Elite", menuName="Abilities/Elite")]
public class Elite : Buff
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

    protected override void OnEnterNewRoom() { }

    protected override void OnEquipWeapon() { }

    protected override void OnOtherDie() { }

    protected override void OnOpenNewRoom() { }

    protected override void OnRun() { }

    protected override void OnUpdate() { }

    protected override void OnBuffApplied()
    {
        DoBuffEffect();
    }

    protected override void OnCleanup()
    {
        // foreach(KeyValuePair<CardModel, BuffID> buffMap in neighborsBuffMap)
        // {
        //     Debug.Log("Elite is deregistering: " + buffMap.Key.ToString());
        //     // Clean up the child buffs
        //     if (buffMap.Key.HasBuff(buffMap.Value))
        //     {
        //         buffMap.Key.RemoveBuff(buffMap.Value);
        //     }
        // }
        Debug.Log("elite cleaning up");
    }

    private void DoBuffEffect()
    {
        // ServiceLocator.Global.Get(out gameManager);
        int myIndex = Array.IndexOf(gameManager.CurrentRoom.Cards, Owner);
        List<CardModel> neighbors = new();
        if (myIndex > 0)
        {
            CardModel neighbor = gameManager.CurrentRoom.Cards[myIndex - 1];
            if (neighbor != null && (neighbor.Suit == Suit.SPADES || neighbor.Suit == Suit.CLUBS))
            {
                neighbors.Add(neighbor);
            }
        }
        if (myIndex < gameManager.CardsPerRoom - 1)
        {
            CardModel neighbor = gameManager.CurrentRoom.Cards[myIndex + 1];
            if (neighbor != null && (neighbor.Suit == Suit.SPADES || neighbor.Suit == Suit.CLUBS))
            {
                neighbors.Add(neighbor);
            }
        }

        Buff buffToApply = GetRegisteredChildBuffByName("Inspired");
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

