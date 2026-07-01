using System.Linq;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="PackTactics", menuName="Buffs/Monster/PackTactics")]
public class PackTactics : Buff
{
    private bool buffApplied;


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
        if (CheckIfLastCardInRoom())
        {
            if (buffApplied)
            {
                Owner.DeregisterValueModifier(2);
            }
            buffApplied = false;
        }
        else
        {
            if (buffApplied)
            {
                Owner.DeregisterValueModifier(2);
            }
            Owner.RegisterValueModifier(2);
            buffApplied = true;
        }
    }

    private bool CheckIfLastCardInRoom()
    {
        foreach(RuntimeCardModel card in gameManager.DungeonController.CurrentRoom.GetCards())
        {
            if (card != Owner && card is MonsterCardModel)
            {
                return false;
            }
        }
        return true;
    }
}
