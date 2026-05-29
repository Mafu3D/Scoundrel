using UnityEngine;

[CreateAssetMenu(fileName="Inspired", menuName="Buff/Inspired")]
public class Inspired : CardBuff
{
    [SerializeField] private int amount = 1;

    protected override void OnBuffInitialized() { }
    protected override void OnAttack() { }

    protected override void OnBuffApplied()
    {
        Owner.RegisterPowerModifier(amount);
    }


    protected override void OnCleanup()
    {
        Owner.DeregisterPowerModifier(amount);
        Debug.Log("inspired cleaning up");
    }

    protected override void OnDiscardPotion() { }

    protected override void OnDraw() { }

    protected override void OnDrinkPotion() { }

    protected override void OnEnterRoom() { }

    protected override void OnEquipWeapon() { }

    protected override void OnOtherDie() { }

    protected override void OnRun() { }

    protected override void OnUpdate() { }

    protected override void OnLeave()
    {
    }

    protected override void OnSelfDie()
    {
    }
}

