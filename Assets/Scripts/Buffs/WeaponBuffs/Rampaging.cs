using UnityEngine;

[CreateAssetMenu(fileName="Rampaging", menuName="Buffs/Weapons/Rampaging")]
public class Rampaging : Buff
{
    [SerializeField] int amount = 1;
    public override void OnOtherDiePostRemoval(MonsterCardModel other)
    {
        Owner.RegisterPermanentValueModifier(amount);
    }
}