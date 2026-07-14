using UnityEngine;

[CreateAssetMenu(fileName="DungeonMap", menuName="Buffs/Player/DungeonMap")]
public class DungeonMap : PlayerBuff
{
    public override void OnBuffApplied()
    {
        // gameManager.ShowNextCards = true;
        gameManager.AmountOfNextCardsToShow = 4;
    }
}