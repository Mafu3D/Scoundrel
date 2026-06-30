using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="SideArm", menuName="Buffs/Player/SideArm")]
public class SideArm : PlayerBuff
{
    // When the player starts on a new floor, equip a weapon with strength Power if they don't already have one.
    [SerializeField] public int Power = 2;

    public override void OnEnterNewFloor()
    {
        if (gameManager.Player.Weapon == null)
        {
            WeaponCardModel weapon = new (Suit.DIAMONDS, Power);
            gameManager.Player.TryEquipWeapon(weapon);
            Debug.Log($"SideArm buff applied: Equipped player with weapon of power {Power}.");
        }
    }
}
