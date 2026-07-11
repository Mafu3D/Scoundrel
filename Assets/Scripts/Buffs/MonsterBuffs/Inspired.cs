using UnityEngine;

[CreateAssetMenu(fileName="Inspired", menuName="Buffs/Monster/Inspired")]
public class Inspired : Buff
{
    [SerializeField] private int amount = 1;

    public override void OnBuffApplied()
    {
        Owner.RegisterPermanentValueModifier(amount);
    }

    public override void OnCleanup()
    {
        Owner.DeregisterPermanentValueModifier(amount);
    }
}

