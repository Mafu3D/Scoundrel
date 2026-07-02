using Project.Core;
using UnityEngine;

[CreateAssetMenu(fileName="TestOnDeathTimer", menuName="Buffs/TestBuffs/Monster/TestOnDeathTimer")]
public class TestOnDeathTimer : Buff
{
    [SerializeField] private int timer = 4;

    public override void OnSelfDie()
    {
        gameManager.GameplayEffectQueue.Add(new TestGameplayEffectWaitForSeconds2(timer));
    }
}