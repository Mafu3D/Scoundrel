public class AttackReport
{
    // NOTE: Had this as a struct but changed to class due to mutability issues with structs in C#. Using a class allows for easier modification of the DamageReceived property during buff processing.
    public Player Attacker;
    public MonsterCardModel Target;
    public WeaponCardModel Weapon;
    public int DamageReceived;
    public bool IsUnarmed => Weapon == null;

    public AttackReport(Player attacker, MonsterCardModel target, WeaponCardModel weapon, int damageReceived)
    {
        Attacker = attacker;
        Target = target;
        Weapon = weapon;
        DamageReceived = damageReceived;
    }
}
