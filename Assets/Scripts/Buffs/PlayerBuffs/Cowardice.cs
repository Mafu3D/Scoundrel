using UnityEngine;

[CreateAssetMenu(fileName="Cowardice", menuName="Buffs/Player/Cowardice")]
public class Cowardice : PlayerBuff
{
    // The player gains Amount run tokens
    [SerializeField] public int Amount = 2;

    public override void OnBuffApplied()
    {
        gameManager.Player.AddRunTokens(Amount);
        Debug.Log($"Cowardice buff applied: Player gained {Amount} run tokens.");
    }
}
