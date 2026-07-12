using UnityEngine;

[CreateAssetMenu(fileName="TestPotionBuff", menuName="Buffs/Potions/TestPotionBuff")]
public class TestPotionBuff : Buff
{
    [SerializeField] private int amount = 1;

    public override void OnBuffApplied()
    {
        Debug.Log("go go go");
    }
}

