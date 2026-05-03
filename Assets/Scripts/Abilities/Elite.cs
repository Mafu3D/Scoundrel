using UnityEngine;

[CreateAssetMenu(fileName="Elite", menuName="Abilities/Elite")]
public class Elite : Ability
{
    public override string Name => "Elite";

    public override void OnAttack() { }

    public override void OnDeath() { }

    public override void OnDiscardPotion() { }

    public override void OnDraw() { }

    public override void OnDrinkPotion() { }

    public override void OnEnterNewRoom() { }

    public override void OnEquipWeapon() { }

    public override void OnMonsterDie() { }

    public override void OnOpenNewRoom() { }

    public override void OnRun() { }
}
