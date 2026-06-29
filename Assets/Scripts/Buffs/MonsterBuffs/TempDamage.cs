using UnityEngine;

[CreateAssetMenu(fileName="TempDamage", menuName="Buffs/Monster/TempDamage")]
public class TempDamage : Buff
{
    [HideInInspector] public int Amount = 0;

    public override void OnBuffApplied()
    {
        Owner.RegisterValueModifier(-Amount);
    }

    public override void OnCleanup()
    {
        Owner.DeregisterValueModifier(-Amount);
    }
}

