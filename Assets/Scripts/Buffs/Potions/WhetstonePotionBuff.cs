using UnityEngine;

[CreateAssetMenu(fileName="WhetstonePotionBuff", menuName="Buffs/Potions/WhetstonePotionBuff")]
public class WhetstonePotionBuff : Buff
{
    public override void OnBuffApplied()
    {
        if (Owner is PotionCardModel)
        {
            (Owner as PotionCardModel).IsWhetstone = true;
        }
    }
}
