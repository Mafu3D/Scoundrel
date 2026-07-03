using Project.Core;
using UnityEngine;

[CreateAssetMenu(fileName="TestOnDeathTimer", menuName="Buffs/TestBuffs/Monster/TestOnDeathTimer")]
public class TestOnDeathTimer : Buff
{
    [SerializeField] private int timer = 4;

    public override void OnSelfAttackedPreDamage(CombatReport combatReport)
    {
        Debug.Log("OnSelfAttackedPreDamage");
        gameManager.GameplayEffectQueue.Add(new TestGameplayEffectWaitForSecondsButOutputRoomFirst(timer));
    }

    public override void OnSelfAttackedPostDamage(CombatReport combatReport)
    {
        Debug.Log("OnSelfAttackedPostDamage");
        gameManager.GameplayEffectQueue.Add(new TestGameplayEffectWaitForSecondsButOutputRoomFirst(timer));
    }


    public override void OnSelfDiePreRemoval()
    {
        Debug.Log("OnSelfDiePreRemoval");
        gameManager.GameplayEffectQueue.Add(new TestGameplayEffectWaitForSecondsButOutputRoomFirst(timer));
    }

    public override void OnSelfDiePostRemoval()
    {
        Debug.Log("OnSelfDiePostRemoval");
        gameManager.GameplayEffectQueue.Add(new TestGameplayEffectWaitForSecondsButOutputRoomFirst(timer));
    }
}