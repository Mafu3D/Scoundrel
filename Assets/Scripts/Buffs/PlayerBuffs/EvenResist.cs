using UnityEngine;

[CreateAssetMenu(fileName="EvenResist", menuName="Buffs/Player/EvenResist")]
public class EvenResist : PlayerBuff
{
    // Reduce damage taken by Amount from enemies with an even value
    [SerializeField] public int Amount = 1;

    public override void OnPlayerAttackPreDamage(CombatReport attackReport)
    {
        if (attackReport.Defender != null && attackReport.Defender.Value % 2 == 0)
        {
            attackReport.DamageReceived -= Amount;
            Debug.Log($"EvenResist buff applied: Damage reduced by {Amount} from even-valued attacker.");
        }
    }
}
