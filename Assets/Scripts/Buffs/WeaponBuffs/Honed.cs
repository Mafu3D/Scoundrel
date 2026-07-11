using UnityEngine;

[CreateAssetMenu(fileName="Honed", menuName="Buffs/Weapons/Honed")]
public class Honed : Buff
{
    public override void OnBuffApplied()
    {
        Owner.RegisterPermanentValueModifier(2);
    }
}