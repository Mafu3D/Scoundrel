using System.Collections.Generic;
using System.Linq;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="PackTactics", menuName="Buffs/Monster/PackTactics")]
public class PackTactics : Buff
{
    [SerializeField] int valuePerMonster = 2;
    private bool buffApplied;

    int lastValue = 0;

    public override void OnDraw()
    {
        DoBuffEffect(); // Replace with apply when drawn parameter
    }

    public override void OnCardsChanged()
    {
        DoBuffEffect();
    }

    private void DoBuffEffect()
    {
        int value = gameManager.DungeonController.CurrentRoom.GetOtherActiveCards(Owner, new () { CardType.MONSTER }).Count * valuePerMonster;
        if (CheckIfLastCardInRoom())
        {
            if (buffApplied)
            {
                Owner.DeregisterTemporaryValueModifier(lastValue);
            }
            buffApplied = false;
        }
        else
        {
            if (buffApplied)
            {
                Owner.DeregisterTemporaryValueModifier(lastValue);
            }
            Owner.RegisterTemporaryValueModifier(value);
            lastValue = value;
            buffApplied = true;
        }
    }

    private bool CheckIfLastCardInRoom()
    {
        foreach(RuntimeCardModel card in gameManager.DungeonController.CurrentRoom.GetAllCards())
        {
            if (card != Owner && card is MonsterCardModel)
            {
                return false;
            }
        }
        return true;
    }
}
