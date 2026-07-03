public class CombatReport
{
    // NOTE: Had this as a struct but changed to class due to mutability issues with structs in C#. Using a class allows for easier modification of the DamageReceived property during buff processing.
    public Player Attacker;
    public MonsterCardModel Defender;
    public WeaponCardModel Weapon;
    public int DamageReceived;
    public bool IsUnarmed => Weapon == null;

    public CombatReport(Player attacker, MonsterCardModel defender, WeaponCardModel weapon, int damageReceived)
    {
        Attacker = attacker;
        Defender = defender;
        Weapon = weapon;
        DamageReceived = damageReceived;
    }
}
