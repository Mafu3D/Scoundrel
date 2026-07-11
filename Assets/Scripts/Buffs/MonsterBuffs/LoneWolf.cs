using System.Linq;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="LoneWolf", menuName="Buffs/Monster/LoneWolf")]
public class LoneWolf : Buff
{
    private bool buffApplied = false;

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
                Owner.DeregisterPermanentValueModifier(2);
            }
            Owner.RegisterPermanentValueModifier(2);
            buffApplied = true;
        }
        else
        {
            if (buffApplied)
            {
                Owner.DeregisterPermanentValueModifier(2);
            }
            buffApplied = false;
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