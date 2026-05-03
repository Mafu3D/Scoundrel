using System;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Elite", menuName="Abilities/Elite")]
public class Elite : Buff
{
    [Header("Instance Parameters")]
    [SerializeField] Buff buff;

    CardModel leftNeighbor = null;
    CardModel rightNeighbor = null;

    public override void OnAttack() { }

    public override void OnCardRemoval() { }

    public override void OnDiscardPotion() { }

    public override void OnDraw() { }

    public override void OnDrinkPotion() { }

    public override void OnEnterNewRoom() { }

    public override void OnEquipWeapon() { }

    public override void OnMonsterDie() { }

    public override void OnOpenNewRoom() { }

    public override void OnRun() { }

    public override void OnUpdate() { }

    public override void OnBuffApplied()
    {
        // ServiceLocator.Global.Get(out gameManager);
        int myIndex = Array.IndexOf(gameManager.CurrentRoom.Cards, owner);
        if (myIndex > 0)
        {
            CardModel potentialNeighbor = gameManager.CurrentRoom.Cards[myIndex - 1];
            if (potentialNeighbor.Suit == Suit.SPADES || potentialNeighbor.Suit == Suit.CLUBS)
            {
                leftNeighbor = potentialNeighbor;
                leftNeighbor.RegisterBuff(buff);
            }
        }
        if (myIndex < gameManager.CardsPerRoom - 1)
        {
            CardModel potentialNeighbor = gameManager.CurrentRoom.Cards[myIndex + 1];
            if (potentialNeighbor.Suit == Suit.SPADES || potentialNeighbor.Suit == Suit.CLUBS)
            {
                rightNeighbor = potentialNeighbor;
                rightNeighbor.RegisterBuff(buff);
            }
        }
    }

    public override void OnBuffRemoved()
    {
        if (leftNeighbor != null)
        {
            leftNeighbor.DeregisterBuff(buff);
        }
        if (rightNeighbor != null)
        {
            rightNeighbor.DeregisterBuff(buff);
        }
    }
}

