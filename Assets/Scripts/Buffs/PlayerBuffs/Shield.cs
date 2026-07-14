using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Shield", menuName="Buffs/Player/Shield")]
public class Shield : PlayerBuff
{
    // When the player starts on a new floor, equip a weapon with strength Power if they don't already have one.
    [SerializeField] public int Amount = 4;

    public override void OnEnterNewFloor()
    {
        gameManager.Player.AddArmor(Amount);
    }
}