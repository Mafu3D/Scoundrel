using UnityEngine;

[CreateAssetMenu(fileName="Pursuer", menuName="Buffs/Monster/Pursuer")]
public class Pursuer : Buff
{
    public override void OnBuffApplied()
    {
        Owner.PersistsThroughRun = true;
    }

    public override void OnCleanup()
    {
        Owner.PersistsThroughRun = false;
    }
}

[CreateAssetMenu(fileName="Reanimate", menuName="Buffs/Monster/Reanimate")]
public class Reanimate : Buff
{
    [SerializeField] private int strength = 1;

    public override void OnSelfDiePreRemoval()
    {
        MonsterCardModel newMonster = new (Owner.Suit, strength);

    }
}