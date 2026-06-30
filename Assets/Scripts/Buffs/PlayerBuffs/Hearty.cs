using UnityEngine;

[CreateAssetMenu(fileName="Hearty", menuName="Buffs/Player/Hearty")]
public class Hearty : PlayerBuff
{
    // Increase the player's max health by the Amount specified in the buff.
    [SerializeField] public int Amount = 5;

    public override void OnBuffApplied()
    {
        gameManager.Player.IncreaseMaxHealth(Amount);
        Debug.Log($"Hearty buff applied: Player's max health increased by {Amount}.");
    }
}
