using UnityEngine;

[CreateAssetMenu(fileName="PrizeFighter", menuName="Buffs/Player/PrizeFighter")]
public class PrizeFighter : PlayerBuff
{
    // When the player attacks unarmed, they gain gold equal to the Amount specified in the buff.
    [SerializeField] public int Amount = 2;

    public override void OnPlayerAttackPostDamage(CombatReport attackReport)
    {
        if (attackReport.IsUnarmed)
        {
            gameManager.Player.AddGold(Amount);
            Debug.Log($"PrizeFighter buff applied: {Amount} gold added to player.");
        }
    }
}
