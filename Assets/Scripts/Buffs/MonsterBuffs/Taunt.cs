using UnityEngine;

[CreateAssetMenu(fileName="Taunt", menuName="Buffs/Monster/Taunt")]
public class Taunt : Buff
{
    public override void OnBuffApplied()
    {
        if (Owner is MonsterCardModel)
        {
            (Owner as MonsterCardModel).HasTaunt = true;
        }
    }

    public override void OnCleanup()
    {
        if (Owner is MonsterCardModel)
        {
            (Owner as MonsterCardModel).HasTaunt = false;
        }
    }
}