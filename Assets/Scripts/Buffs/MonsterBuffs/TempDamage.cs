using UnityEngine;

[CreateAssetMenu(fileName="TempDamage", menuName="Buffs/Monster/TempDamage")]
public class TempDamage : Buff
{
    [HideInInspector] public int Amount = 0;

    public override void OnBuffApplied()
    {
        Owner.RegisterPermanentValueModifier(-Amount);
    }

    public override void OnCleanup()
    {
        Owner.DeregisterPermanentValueModifier(-Amount);
    }
}

