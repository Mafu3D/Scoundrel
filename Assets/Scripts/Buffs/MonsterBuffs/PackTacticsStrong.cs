using UnityEngine;

[CreateAssetMenu(fileName="PackTacticsStrong", menuName="Buffs/Monster/PackTacticsStrong")]
public class PackTacticsStrong : Buff
{
    [SerializeField] private int strength = 4;

    public override void OnBuffApplied()
    {
        Owner.RegisterValueModifier(strength);
    }

    public override void OnCleanup()
    {
        Owner.DeregisterValueModifier(strength);
    }
}
