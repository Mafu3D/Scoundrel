using UnityEngine;

[CreateAssetMenu(fileName="Inspired", menuName="Buff/Inspired")]
public class Inspired : Buff
{
    [SerializeField] private int amount = 1;

    public override void OnBuffInitialized() { }
    public override void OnAttack() { }

    public override void OnBuffApplied()
    {
        owner.RegisterValueModifier(amount);
    }


    public override void OnBuffRemoved()
    {
        owner.DeregisterValueModifier(amount);
    }

    public override void OnCardRemoval() { }

    public override void OnDiscardPotion() { }

    public override void OnDraw() { }

    public override void OnDrinkPotion() { }

    public override void OnEnterNewRoom() { }

    public override void OnEquipWeapon() { }

    public override void OnMonsterDie() { }

    public override void OnOpenNewRoom() { }

    public override void OnRun() { }

    public override void OnUpdate() { }
}

