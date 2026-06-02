using UnityEngine;

[CreateAssetMenu(fileName="Inspired", menuName="Buff/Inspired")]
public class Inspired : Buff
{
    [SerializeField] private int amount = 1;

    public override void OnBuffApplied()
    {
        Owner.RegisterValueModifier(amount);
    }

    public override void OnCleanup()
    {
        Owner.DeregisterValueModifier(amount);
    }
}

