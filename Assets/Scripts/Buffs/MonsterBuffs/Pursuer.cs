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
