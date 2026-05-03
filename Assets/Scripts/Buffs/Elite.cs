using System;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Elite", menuName="Abilities/Elite")]
public class Elite : Buff
{
    CardModel leftNeighbor = null;
    CardModel rightNeighbor = null;

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
        if (leftNeighbor != null)
        {
            leftNeighbor.Buffs.DeregisterBuff(buffToApply);
        }
        if (rightNeighbor != null)
        {
            rightNeighbor.Buffs.DeregisterBuff(buffToApply);
        }
    }

    private void DoBuffEffect()
    {
        // ServiceLocator.Global.Get(out gameManager);
        int myIndex = Array.IndexOf(gameManager.CurrentRoom.Cards, owner);
        if (myIndex > 0)
        {
            CardModel neighbor = gameManager.CurrentRoom.Cards[myIndex - 1];
            if (neighbor != null && (neighbor.Suit == Suit.SPADES || neighbor.Suit == Suit.CLUBS))
            {
                leftNeighbor = neighbor;
                leftNeighbor.RegisterBuff(buffToApply);
            }
        }
        if (myIndex < gameManager.CardsPerRoom - 1)
        {
            CardModel neighbor = gameManager.CurrentRoom.Cards[myIndex + 1];
            if (neighbor != null && (neighbor.Suit == Suit.SPADES || neighbor.Suit == Suit.CLUBS))
            {
                rightNeighbor = neighbor;
                rightNeighbor.RegisterBuff(buffToApply);
            }
        }
    }
}

