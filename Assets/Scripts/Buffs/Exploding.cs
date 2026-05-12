using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Exploding", menuName="Abilities/Exploding")]
public class Exploding : Buff
{
    List<KeyValuePair<CardModel, BuffID>> neighborsBuffMap = new();

    protected override void OnBuffInitialized() {
    }

    protected override void OnAttack() { }

    protected override void OnDiscardPotion() { }

    protected override void OnDraw()
    {
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

    protected override void OnLeave()
    {
    }

    protected override void OnSelfDie()
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

        foreach (CardModel neighbor in neighbors)
        {
            neighbor.RegisterValueModifier(-Owner.Value);
            if (neighbor.Value <= 0)
            {
                gameManager.CurrentRoom.TryRemoveCard(neighbor);
            }
        }
    }
}

