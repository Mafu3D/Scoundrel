using UnityEngine;

[CreateAssetMenu(fileName="PackTacticsWeak", menuName="Buffs/Monster/PackTacticsWeak")]
public class PackTacticsWeak : Buff
{
    [SerializeField] private int strength = -2;

    public override void OnBuffApplied()
    {
        Owner.RegisterPermanentValueModifier(strength);
    }

    public override void OnCleanup()
    {
        Owner.DeregisterPermanentValueModifier(strength);
    }
}