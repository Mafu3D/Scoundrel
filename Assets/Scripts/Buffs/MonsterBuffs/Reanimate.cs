using Project.Core;
using UnityEngine;

[CreateAssetMenu(fileName="Reanimate", menuName="Buffs/Monster/Reanimate")]
public class Reanimate : Buff
{
    [SerializeField] private int strength = 1;

    private int index;
    private MonsterCardModel newMonster;

    public override void OnSelfDiePreRemoval()
    {
        newMonster = new (Owner.Suit, strength);
        index = gameManager.DungeonController.CurrentRoom.GetIndexOf(Owner);

        // gameManager.GameplayEffectQueue.Add(new RuntimeGameplayEffect(
        //     onProcessMethod: (deltaTime) =>
        //     {
        //         gameManager.CurrentRoom.Cards[index] = newMonster;
        //         return Status.Complete;
        //     },
        //     onStartMethod: () => Debug.Log("Reanimate effect started"),
        //     onEndMethod: () => Debug.Log("Reanimate effect ended"),
        //     resetMethod: () => Debug.Log("Reanimate effect reset"),
        //     startMessageMethod: () => "Reanimate effect start message",
        //     endMessageMethod: () => "Reanimate effect end message"
        // ));

    }

    public override void OnSelfDiePostRemoval()
    {
        Debug.Log("Reanimate effect ended");
        gameManager.DungeonController.CurrentRoom.GetCards()[index] = newMonster;
    }
}