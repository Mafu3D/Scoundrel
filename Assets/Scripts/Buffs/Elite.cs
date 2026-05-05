using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Elite", menuName="Abilities/Elite")]
public class Elite : Buff
{
    List<KeyValuePair<CardModel, BuffID>> neighborsBuffMap = new();

    Buff buffToApply;

    public override void OnBuffInitialized() {
        buffToApply = GetChildBuffByName("Inspired");
    }

    public override void OnAttack() { }

    public override void OnCardRemoval() { }

    public override void OnDiscardPotion() { }

    public override void OnDraw()
    {
        DoBuffEffect();
    }

    public override void OnDrinkPotion() { }

    public override void OnEnterNewRoom() { }

    public override void OnEquipWeapon() { }

    public override void OnMonsterDie() { }

    public override void OnOpenNewRoom() { }

    public override void OnRun() { }

    public override void OnUpdate() { }

    public override void OnBuffApplied()
    {
        DoBuffEffect();
    }

    public override void OnBuffRemoved()
    {
        foreach(KeyValuePair<CardModel, BuffID> buffMap in neighborsBuffMap)
        {
            Debug.Log("Elite is deregistering: " + buffMap.Key.ToString());
            buffMap.Key.DeregisterBuff(buffMap.Value);
        }
    }

    private void DoBuffEffect()
    {
        // ServiceLocator.Global.Get(out gameManager);
        int myIndex = Array.IndexOf(gameManager.CurrentRoom.Cards, owner);
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

        foreach (CardModel neighbor in neighbors)
        {
            BuffID buffID = neighbor.RegisterBuff(buffToApply);
            neighborsBuffMap.Add(new(neighbor, buffID));
            Debug.Log(neighbor.ToString());
        }
    }
}

