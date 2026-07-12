using UnityEngine;

[CreateAssetMenu(fileName="ArmorPotionBuff", menuName="Buffs/Potions/ArmorPotionBuff")]
public class ArmorPotionBuff : Buff
{
    public override void OnBuffApplied()
    {
        if (Owner is PotionCardModel)
        {
            (Owner as PotionCardModel).IsArmor = true;
        }
    }
}

